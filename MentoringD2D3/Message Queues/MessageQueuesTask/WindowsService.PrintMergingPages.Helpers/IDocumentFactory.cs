namespace WindowsService.PrintMergingPages.Helpers
{
    public interface IDocumentFactory
    {
        IMergeDocument CreateDocument();
    }
}
