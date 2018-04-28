using MigraDoc.DocumentObjectModel;

namespace WindowsService.PrintMergingPages.Helpers
{
    /// <summary>
    /// Represents a resulting pdf document.
    /// </summary>
    public  class ImagePdfMergeDocument : IMergeDocument
    {
        private readonly Document resultPdfDocument;
        private Section pdfSection;

        public ImagePdfMergeDocument()
        {
            resultPdfDocument = new Document();
            pdfSection = resultPdfDocument.AddSection();
        }

        /// <summary>
        /// Inserts image as page into a document.
        /// </summary>
        /// <param name="filePath">The imeage that should be inserting.</param>
        public void AddPage(string filePath)
        {
            var page = pdfSection.AddImage(filePath);
            
            page.Width = resultPdfDocument.DefaultPageSetup.PageWidth;
            page.Height = resultPdfDocument.DefaultPageSetup.PageHeight;

            pdfSection.AddPageBreak();
        }
        
        /// <summary>
        /// The resulting pdf file.
        /// </summary>
        public Document ResultingDocument => resultPdfDocument;
    }

}
