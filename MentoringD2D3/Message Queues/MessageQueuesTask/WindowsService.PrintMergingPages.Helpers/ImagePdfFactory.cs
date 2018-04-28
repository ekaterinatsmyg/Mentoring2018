namespace WindowsService.PrintMergingPages.Helpers
{
    public class ImagePdfFactory : IDocumentFactory
    {
        public IMergeDocument CreateDocument() => new ImagePdfMergeDocument();
    }
}
