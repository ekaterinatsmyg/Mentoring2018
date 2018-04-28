using System.Collections.Generic;

namespace WindowsService.PrintMergingPages.QueueHelper
{
    /// <summary>
    /// Represents chunks of sending documents.
    /// </summary>
    public class DocumentChuckMessage
    {
        public int Position { get; set; }

        public int Size { get; set; }

        public List<byte> Buffer { get; set; }

        public int BufferSize { get; set; }
    }
}
