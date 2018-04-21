using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace WindowsServices.PrintMergingPages.Service
{
    public class MergePagesService : IMergePagesService
    {
        private readonly FileSystemWatcher directoryWatcher;
        private readonly DirectoryService directoryService;

        private readonly Task mergeTask;

        private readonly AutoResetEvent newFileEvent;
        private readonly ManualResetEvent stopWorkEvent;

        private readonly IDocumentFactory documentFactory;

        private readonly LogWriter logger;

        public MergePagesService(string inputFolderPath, string outputFolderPath, IDocumentFactory documentFactory)
        {
            logger = HostLogger.Get<MergePagesService>();

            this.documentFactory = documentFactory;
            directoryService = new DirectoryService(inputFolderPath, outputFolderPath);

            newFileEvent = new AutoResetEvent(true);
            stopWorkEvent = new ManualResetEvent(false);

            directoryWatcher = new FileSystemWatcher { Path = inputFolderPath, NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite };
            directoryWatcher.Created += (sender, fileSystemEventArgs) => newFileEvent.Set();
            
            mergeTask = new Task(WorkProcedure);
        }

        public bool Start()
        {
            mergeTask.Start();
            directoryWatcher.EnableRaisingEvents = true;

            logger.Info($"{nameof(MergePagesService)} was started at {DateTime.Now}");

            return true;
        }

        public bool Stop()
        {
            directoryWatcher.EnableRaisingEvents = false;
            stopWorkEvent.Set();
            mergeTask.Wait();

            logger.Info($"{nameof(MergePagesService)} was stopped at {DateTime.Now}");

            return true;
        }

        private void WorkProcedure()
        {
            var pdfResultDocument = documentFactory.CreateDocument();
            var isWaitingPage = false;
            var pages = new ConcurrentBag<string>();

            try
            {
                do
                {
                    foreach (var page in Directory.EnumerateFiles(directoryService.InputDirectory.FullName))
                    {
                        if (stopWorkEvent.WaitOne(TimeSpan.Zero))
                            return;
                        
                        if (IsValidImageFormat(page))
                        {
                            pages.Add(page);
                            if (FileHelper.IsLastPage(page) && isWaitingPage)
                            {
                                directoryService.SaveResultDocument(ref pdfResultDocument, documentFactory);
                                FileHelper.DeletePagesFromSeparateFile(pages);
                                pages = new ConcurrentBag<string>();
                                isWaitingPage = false;
                            }
                            else
                            {
                                if (!FileHelper.TryOpenPage(page, 3)) continue;

                                pdfResultDocument.AddPage(page);
                                isWaitingPage = true;
                            }
                        }
                        else
                        {
                            if (File.Exists(page))
                            {
                                FileHelper.DeleteFile(page);
                            }
                            else
                            {
                                File.Move(page, Path.Combine(directoryService.OutputDirectory.FullName, page));
                            }
                        }
                    }
                } while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, newFileEvent }, 1000) != 0);

            }
            catch (Exception e)
            {
                logger.Error($"{e.Message} {e.StackTrace}");
            }
        }

        private static bool IsValidImageFormat(string filePath)
        {
            return Regex.IsMatch(filePath, @"img_[0-9]{3}.(gif|tif|bmp|tiff|jpg|png|jpeg|pdf)");
        }
    }
}
