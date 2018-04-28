namespace WindowsService.PrintMergingPages.QueueHelper
{
    /// <summary>
    /// Provides data related to the state of the service.
    /// </summary>
    public class ServerSettings
    {
        public string Date { get; set; }

        public string Status { get; set; }

        public int RecieveMessageTimeOut { get; set; }
    }
}
