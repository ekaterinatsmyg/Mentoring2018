using System.Messaging;

namespace WindowsService.PrintMergingPages.QueueHelper
{
    /// <summary>
    /// Manager of the queues
    /// </summary>
    public static class MergePagesQueueManager
    {
        /// <summary>
        /// Prepares work with the queues.
        /// </summary>
        public static void PrepareQueues()
        {
            if (!MessageQueue.Exists(MergePagesQueuesNames.Server))
                MessageQueue.Create(MergePagesQueuesNames.Server);

            if (!MessageQueue.Exists(MergePagesQueuesNames.Monitor))
                MessageQueue.Create(MergePagesQueuesNames.Monitor);

            if (!MessageQueue.Exists(MergePagesQueuesNames.Client))
                MessageQueue.Create(MergePagesQueuesNames.Client);

        }
    }
}
