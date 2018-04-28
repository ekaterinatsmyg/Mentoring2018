using System;
using System.Collections.Generic;
using System.IO;
using WindowsService.PrintMergingPages.QueueHelper;

namespace WindowsService.PrintMergingPages.Helpers
{
    public class DirectoryService
    {
        public DirectoryInfo InputDirectory { get; private set; }
        public DirectoryInfo OutputDirectory { get; private set; }

        /// <summary>
        ///  Initializes the input directory.
        /// </summary>
        /// <param name="inputFolderPath">The input folder path.</param>
        public void SetInputDirectory(string inputFolderPath)
        {
            InputDirectory = Directory.Exists(inputFolderPath)
                ? new DirectoryInfo(inputFolderPath)
                : Directory.CreateDirectory(inputFolderPath);
        }

        /// <summary>
        /// Initializes the output directory.
        /// </summary>
        /// <param name="outputFolderPath">The output folder path.</param>
        public void SetOutputDirectory(string outputFolderPath)
        {
            OutputDirectory = Directory.Exists(outputFolderPath)
                ? new DirectoryInfo(outputFolderPath)
                : Directory.CreateDirectory(outputFolderPath);
        }

        /// <summary>
        /// Saves the resulting pdf document to disk.
        /// </summary>
        /// <param name="messages">The list of messages that cames from server.</param>
        public void SaveResultDocument(List<DocumentChuckMessage> messages)
        {
            var resultFilePath = Path.Combine(OutputDirectory.FullName, $"result_{Guid.NewGuid().ToString()}.pdf");
            using (Stream destination = File.Create(resultFilePath))
            {
                foreach (var message in messages)
                {
                    destination.Write(message.Buffer.ToArray(), 0, message.BufferSize);
                }
            }
        }
    }
}
