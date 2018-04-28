using System;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Threading;
using WindowsService.PrintMergingPages.Helpers;
using WindowsService.PrintMergingPages.QueueHelper;
using MigraDoc.Rendering;

namespace WindowsServices.PrintMergingPages.Service
{
    /// <summary>
    /// Provides service for working with the client side of the service.
    /// </summary>
    public class MessageService
    {
        /// <summary>
        /// The current state of the server.
        /// </summary>
        public string QueueStatus { get; set; } = "Wairing for messages...";

        /// <summary>
        /// The current timeot setting for waiting the next page.
        /// </summary>
        public int RecieveMessageTimeOut { get; set; } = 5000;

        /// <summary>
        /// Sends resulting document to the client.
        /// </summary>
        /// <param name="document">The resulting document.</param>
        public void SendDocument(IMergeDocument document)
        {
            var documentRenderer = new PdfDocumentRenderer {Document = document.ResultingDocument};
            documentRenderer.RenderDocument();
            var pageCount = documentRenderer.PdfDocument.PageCount - 1;
            documentRenderer.PdfDocument.Pages.RemoveAt(pageCount);
            var documentToSend = documentRenderer.PdfDocument;

            var buffer = new byte[1024];

            using (var stream = new MemoryStream())
            {
                documentToSend.Save(stream, false);
                stream.Position = 0;
                var position = 0;
                var size = (int)Math.Ceiling((double)(stream.Length) / 1024) - 1;

                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var chunk = new DocumentChuckMessage
                    {
                        Position = position,
                        Size = size,
                        Buffer = buffer.ToList(),
                        BufferSize = bytesRead
                    };

                    position++;

                    using (var serverQueue = new MessageQueue(MergePagesQueuesNames.Server, QueueAccessMode.Send))
                    {
                        var message = new Message(chunk);
                        serverQueue.Send(message);
                    }
                }
            }
        }

        /// <summary>
        /// Sends current status of the server.
        /// </summary>
        /// <param name="token"></param>
        public void SendSettings(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var settings = new ServerSettings
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = QueueStatus,
                    RecieveMessageTimeOut = RecieveMessageTimeOut
                };

                using (var serverQueue = new MessageQueue(MergePagesQueuesNames.Monitor))
                {
                    var message = new Message(settings);
                    serverQueue.Send(message);
                }

                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Monitor the client side of the merge page service in order to change timeout.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="stopWorkEvent"></param>
        public void MonitoreClient(CancellationToken token, ManualResetEvent stopWorkEvent)
        {
            using (var clientQueue = new MessageQueue(MergePagesQueuesNames.Client))
            {
                clientQueue.Formatter = new XmlMessageFormatter(new[] { typeof(int) });

                while (!token.IsCancellationRequested)
                {
                    var asyncReceive = clientQueue.BeginPeek();

                    if (WaitHandle.WaitAny(new[] { stopWorkEvent, asyncReceive.AsyncWaitHandle }) == 0)
                    {
                        break;
                    }

                    var message = clientQueue.EndPeek(asyncReceive);
                    clientQueue.Receive();
                    RecieveMessageTimeOut = (int)message.Body;
                }
            }
        }
    }
}
