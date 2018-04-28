namespace WindowsService.PrintMergingPages.QueueHelper
{
    /// <summary>
    /// Provides  a set of necessary queues.
    /// </summary>
    public class MergePagesQueuesNames
    {

        public static string Monitor = @".\private$\MergPagesMonitorQueue";
        public static string Server = @".\private$\MergPagesServerQueue";
        public static string Client = @".\Private$\MergPagesClientQueue";
    }
}
