using System;
using System.IO;

namespace WindowsServices.PrintMergingPages.Service
{
    public class DirectoryService
    {
        public DirectoryInfo InputDirectory { get; }
        public DirectoryInfo OutputDirectory { get; }
        public DirectoryService(string inputFolderPath, string outputFolderPath)
        {
            InputDirectory = Directory.Exists(inputFolderPath)
                ? new DirectoryInfo(inputFolderPath)
                : Directory.CreateDirectory(inputFolderPath);

            OutputDirectory = Directory.Exists(outputFolderPath)
                ? new DirectoryInfo(outputFolderPath)
                : Directory.CreateDirectory(outputFolderPath);
        }

        public void SaveResultDocument(ref IMergeDocument document, IDocumentFactory factory)
        {
            document.SaveResult($"{OutputDirectory.FullName}\\{Guid.NewGuid()}.pdf");
            document = factory.CreateDocument();
        }
    }
}
