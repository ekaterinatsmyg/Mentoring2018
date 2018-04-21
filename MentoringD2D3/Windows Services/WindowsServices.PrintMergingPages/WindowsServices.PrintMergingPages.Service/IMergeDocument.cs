namespace WindowsServices.PrintMergingPages.Service
{
    public interface IMergeDocument
    {
        void AddPage(string filePath);
        void SaveResult(string resultingFilePath);
    }
}
