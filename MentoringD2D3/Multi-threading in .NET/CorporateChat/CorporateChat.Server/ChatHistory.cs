using System.Collections.Concurrent;
using System.IO;
using CorporateChat.Diagnostics;

namespace CorporateChat.ServerClient
{
    public class ChatHistory
    {
        /// <summary>
        /// Last d clients' messages.
        /// </summary>
        ConcurrentQueue<string> clientMessages = new ConcurrentQueue<string>();

        private const string logfileName = "chat_history.txt";

        /// <summary>
        /// Update latest 5 messages.
        /// </summary>
        /// <param name="message">The new incoming message from a client.</param>
        public void AddMessageToHistory(string message)
        {
            if (clientMessages.Count > 5)
            {
                if (!clientMessages.TryDequeue(out var outdatedMessage))
                {
                    ApplicationLogger.LogMessage(LogMessageType.Warn, $"Unable to remove outdated message {outdatedMessage} from a chat history");
                }
            }
            clientMessages.Enqueue(message);
        }

        /// <summary>
        /// Save a chat history to a file.
        /// </summary>
        public void SaveHistory()
        {
            using (StreamWriter sw = File.Exists(logfileName) ? File.AppendText(logfileName) : File.CreateText(logfileName))
            {
                foreach (string message in clientMessages)
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}

