using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace WindowsServices.PrintMergingPages.Service
{
    public  class ImagePdfMergeDocument : IMergeDocument
    {
        private readonly Document resultPdfDocument;
        private Section pdfSection;
        private PdfDocumentRenderer pdfRender;

        public ImagePdfMergeDocument()
        {
            resultPdfDocument = new Document();
            pdfSection = resultPdfDocument.AddSection();
            pdfRender = new PdfDocumentRenderer();
        }
        public void AddPage(string filePath)
        {
            var page = pdfSection.AddImage(filePath);
            
            page.Width = resultPdfDocument.DefaultPageSetup.PageWidth;
            page.Height = resultPdfDocument.DefaultPageSetup.PageHeight;

            pdfSection.AddPageBreak();
        }

        public void SaveResult(string resultingFilePath)
        {
            pdfRender.Document = resultPdfDocument;
            pdfRender.RenderDocument();
            pdfRender.Save(resultingFilePath);
        }
        
    }

}
