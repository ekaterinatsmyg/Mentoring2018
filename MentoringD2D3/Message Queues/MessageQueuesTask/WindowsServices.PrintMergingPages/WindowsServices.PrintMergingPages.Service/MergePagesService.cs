using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WindowsService.PrintMergingPages.Helpers;
using WindowsService.PrintMergingPages.QueueHelper;
using Topshelf.Logging;

namespace WindowsServices.PrintMergingPages.Service
{
    /// <summary>
    /// Provides a server site of a service that merge pages into single pdf doc for printing.
    /// </summary>
    public class MergePagesService : IMergePagesService
    {
        #region fields
        private readonly FileSystemWatcher directoryWatcher;
        private readonly DirectoryService directoryService;

        private readonly Task mergeTask;
        private readonly Task monitoreTask;
        private readonly Task sendQueueSettingsTask;
        private readonly CancellationTokenSource tokenSource;

        private readonly AutoResetEvent newFileEvent;
        private readonly ManualResetEvent stopWorkEvent;

        private readonly IDocumentFactory documentFactory;

        private readonly LogWriter logger;
        private readonly MessageService messageService;
        #endregion

        public MergePagesService(string inputFolderPath, IDocumentFactory documentFactory)
        {
            MergePagesQueueManager.PrepareQueues();

            logger = HostLogger.Get<MergePagesService>();

            this.documentFactory = documentFactory;
            directoryService = new DirectoryService();
            directoryService.SetInputDirectory(inputFolderPath);

            newFileEvent = new AutoResetEvent(false);
            stopWorkEvent = new ManualResetEvent(false);

            directoryWatcher = new FileSystemWatcher { Path = inputFolderPath, NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite };
            directoryWatcher.Created += (sender, fileSystemEventArgs) => newFileEvent.Set();

            tokenSource = new CancellationTokenSource();
            mergeTask = new Task(() => WorkProcedure(tokenSource.Token));
            monitoreTask = new Task(() => messageService.MonitoreClient(tokenSource.Token, stopWorkEvent));
            sendQueueSettingsTask = new Task(() => messageService.SendSettings(tokenSource.Token));

            messageService = new MessageService();
        }

        /// <summary>
        /// Starts the serever side of a merge page service.
        /// </summary>
        /// <returns>Returns that state of starting process.</returns>
        public bool Start()
        {
            mergeTask.Start();
            monitoreTask.Start();
            sendQueueSettingsTask.Start();

            directoryWatcher.EnableRaisingEvents = true;

            logger.Info($"{nameof(MergePagesService)} was started at {DateTime.Now}");

            return true;
        }

        /// <summary>
        /// Completes the serever side of a merge page service.
        /// </summary>
        /// <returns>Returns that state of completing process.</returns>
        public bool Stop()
        {
            directoryWatcher.EnableRaisingEvents = false;
            tokenSource.Cancel();
            mergeTask.Wait();
            stopWorkEvent.Set();

            logger.Info($"{nameof(MergePagesService)} was stopped at {DateTime.Now}");

            return true;
        }

        /// <summary>
        /// Merges input pages into pdf document and sends the result to a client side.
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled.</param>
        private void WorkProcedure(CancellationToken token)
        {
            var pdfResultDocument = documentFactory.CreateDocument();
            var isWaitingPage = false;
            var pages = new ConcurrentBag<string>();

            try
            {
                do
                {
                    messageService.QueueStatus = "Waiting messages...";

                    foreach (var page in Directory.EnumerateFiles(directoryService.InputDirectory.FullName))
                    {
                        messageService.QueueStatus = "In progress...";

                        if (IsValidImageFormat(page))
                        {
                            pages.Add(page);
                            if (FileHelper.IsLastPage(page) && isWaitingPage)
                            {
                                messageService.SendDocument(pdfResultDocument);
                                pdfResultDocument = documentFactory.CreateDocument();
                                FileHelper.DeletePagesFromSeparateFile(pages);
                                pages = new ConcurrentBag<string>();
                                isWaitingPage = false;
                            }
                            else
                            {
                                if (!FileHelper.TryOpenFile(page, 3)) continue;

                                pdfResultDocument.AddPage(page);
                                isWaitingPage = true;
                            }
                        }
                        else
                        {
                            FileHelper.DeleteFile(page);

                        }
                    }
                } while (!token.IsCancellationRequested);

            }
            catch (Exception e)
            {
                logger.Error($"{e.Message} {e.StackTrace}");
            }
        }

        /// <summary>
        /// Verifies the name format and the file type of a pages that should be merged.
        /// </summary>
        /// <param name="filePath">The file path of the merging page.</param>
        /// <returns></returns>
        private static bool IsValidImageFormat(string filePath)
        {
            return Regex.IsMatch(filePath, @"img_[0-9]{3}.(gif|tif|bmp|tiff|jpg|png|jpeg|pdf)");
        }
    }
}
