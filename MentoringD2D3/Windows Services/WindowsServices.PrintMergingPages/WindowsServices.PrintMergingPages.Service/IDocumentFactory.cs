namespace WindowsServices.PrintMergingPages.Service
{
    public interface IDocumentFactory
    {
        IMergeDocument CreateDocument();
    }
}
