namespace WindowsServices.PrintMergingPages.Service
{
    public class ImagePdfFactory : IDocumentFactory
    {
        public IMergeDocument CreateDocument() => new ImagePdfMergeDocument();
    }
}
