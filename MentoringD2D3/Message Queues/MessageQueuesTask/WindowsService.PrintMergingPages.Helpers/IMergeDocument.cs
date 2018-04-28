using MigraDoc.DocumentObjectModel;

namespace WindowsService.PrintMergingPages.Helpers
{
    public interface IMergeDocument
    {
        void AddPage(string filePath);
        Document ResultingDocument { get; }
    }
}
