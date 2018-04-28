using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WindowsService.PrintMergingPages.Helpers;
using WindowsService.PrintMergingPages.QueueHelper;
using Topshelf.Logging;

namespace WindowsServices.PrintMergingPages.Client
{
    /// <summary>
    /// Provides a client site of a service that merge pages into single pdf doc for printing.
    /// </summary>
    public class MergePagesClient : IMergePagesClient
    {
        #region fields
        private readonly LogWriter logger;

        private readonly FileSystemWatcher watcher;
        private readonly DirectoryService directoryService;

        private readonly Task processFilesTask;
        private readonly Task monitoringTask;

        private readonly CancellationTokenSource tokenSource;
        private readonly ManualResetEvent stopWaitEvent;

        private int lastTimeout;
        private const string currentSettingFileName = "Current_Settings.csv";
        private const string settingFileName = "TimeOutSettings.xml";
        #endregion

        public MergePagesClient(string outputPath)
        {
            MergePagesQueueManager.PrepareQueues();
            logger = HostLogger.Get<MergePagesClient>();

            directoryService = new DirectoryService();
            directoryService.SetOutputDirectory(outputPath);

            watcher = new FileSystemWatcher(outputPath) { Filter = "*.xml" };
            watcher.Changed += QueueSettingsChanged;

            stopWaitEvent = new ManualResetEvent(false);
            tokenSource = new CancellationTokenSource();

            processFilesTask = new Task(() => WorkProcedure(tokenSource.Token));
            monitoringTask = new Task(() => MonitorServer(tokenSource.Token));

        }
        
        /// <summary>
        /// Starts the client side of a merge page service.
        /// </summary>
        /// <returns>Returns that state of starting process.</returns>
        public bool Start()
        {
            processFilesTask.Start();
            monitoringTask.Start();
            watcher.EnableRaisingEvents = true;

            logger.Info($"Client Sipe of Merge Pages Service was started at {DateTime.Now}");

            return true;
        }

        /// <summary>
        /// Completes the client side of a merge page service.
        /// </summary>
        /// <returns>Returns that state of completing process.</returns>
        public bool Stop()
        {
            watcher.EnableRaisingEvents = false;
            tokenSource.Cancel();
            stopWaitEvent.Set();
            Task.WaitAll(processFilesTask, monitoringTask);

            logger.Info($"Client Sipe of Merge Pages Service was stopped at {DateTime.Now}");

            return true;
        }

        /// <summary>
        /// Listens the server client for receiving documents and saving them to the disk.
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled.</param>
        public void WorkProcedure(CancellationToken token)
        {
            using (var serverQueue = new MessageQueue(MergePagesQueuesNames.Server))
            {
                serverQueue.Formatter = new XmlMessageFormatter(new[] { typeof(DocumentChuckMessage) });
                var documentChunks = new List<DocumentChuckMessage>();

                do
                {
                    var enumerator = serverQueue.GetMessageEnumerator2();
                    var count = 0;

                    while (enumerator.MoveNext())
                    {
                        var body = enumerator.Current?.Body;
                        if (body is DocumentChuckMessage section)
                        {
                            var chunk = section;
                            documentChunks.Add(chunk);

                            if (chunk.Position == chunk.Size)
                            {
                                directoryService.SaveResultDocument(documentChunks);
                                documentChunks.Clear();
                            }
                        }

                        count++;
                    }

                    for (var i = 0; i < count; i++)
                    {
                        try
                        {
                            serverQueue.Receive();
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"{ex.Message} | Stack Trace: {Environment.NewLine} {ex.StackTrace}");
                        }
                    }

                    Thread.Sleep(1000);
                }
                while (!token.IsCancellationRequested);
            }
        }

        /// <summary>
        /// Monitores the server queue in order to get current setting of the server.
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled.</param>
        public void MonitorServer(CancellationToken token)
        {
            using (var serverQueue = new MessageQueue(MergePagesQueuesNames.Monitor))
            {
                serverQueue.Formatter = new XmlMessageFormatter(new[] { typeof(ServerSettings) });

                while (!token.IsCancellationRequested)
                {
                    var asyncReceive = serverQueue.BeginPeek();

                    if (WaitHandle.WaitAny(new[] { stopWaitEvent, asyncReceive.AsyncWaitHandle }) == 0)
                    {
                        break;
                    }

                    var message = serverQueue.EndPeek(asyncReceive);
                    serverQueue.Receive();
                    var settings = (ServerSettings)message.Body;
                    lastTimeout = settings.RecieveMessageTimeOut;
                    WriteSettings(settings);
                }
            }
        }

        /// <summary>
        /// Writes current state of a queue to a file.
        /// </summary>
        /// <param name="settings">The current server state.</param>
        private void WriteSettings(ServerSettings settings)
        {
            var fullPath = Path.Combine(directoryService.OutputDirectory.FullName, currentSettingFileName);

            using (var file = File.AppendText(fullPath))
            {
                var settingsRecord = $"{settings.Date},{settings.Status},{settings.RecieveMessageTimeOut}ms";
                file.WriteLine(settingsRecord);
            }
        }

        /// <summary>
        /// Listens output directory in order to change timeout settings.
        /// </summary>
        /// <param name="sender">The object that initiate an event.</param>
        /// <param name="e">The data provider for the directory events</param>
        private void QueueSettingsChanged(object sender, FileSystemEventArgs e)
        {
            var fullPath = Path.Combine(directoryService.OutputDirectory.FullName, settingFileName);
            if (FileHelper.TryOpenFile(fullPath, 3))
            {
                var doc = XDocument.Load(fullPath);
                if (doc.Root != null)
                {
                    var timeout = int.Parse(doc.Root.Value);
                    if (lastTimeout == timeout) return;

                    using (var clientQueue = new MessageQueue(MergePagesQueuesNames.Client))
                    {
                        clientQueue.Send(timeout);
                    }
                }
            }
        }
    }
}
