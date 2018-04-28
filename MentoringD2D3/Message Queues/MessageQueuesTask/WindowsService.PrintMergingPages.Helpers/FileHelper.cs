using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using ZXing;

namespace WindowsService.PrintMergingPages.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Verifies if the document conatins barcode or not.
        /// </summary>
        /// <param name="filePath">The image's path that should be verified.</param>
        /// <returns>Retuns true if the image conatins barcodem and false if not.</returns>
        public static bool IsLastPage(string filePath)
        {
            if (!IsBitmapImage(filePath))
                return false;

            var barcodeReader = new BarcodeReader();
            Result barcodeResult;

            using (var barcodeBitmap = (Bitmap)Image.FromFile(filePath))
            {
                barcodeResult = barcodeReader.Decode(barcodeBitmap);
            }

            return barcodeResult != null;
        }

        /// <summary>
        /// Trying to open merging image.
        /// </summary>
        /// <param name="path">The image's path that should be opened.</param>
        /// <param name="tryCount">The number of attempts to open the image.</param>
        /// <returns>Returna true if the image could be opened, false - if not.</returns>
        public static bool TryOpenFile(string path, int tryCount)
        {
            for (int i = 0; i < tryCount; i++)
            {
                if (File.Exists(path))
                {
                    FileStream stream = null;
                    try
                    {
                        stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
                        return true;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5000);
                    }
                    finally
                    {
                        stream?.Close();
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes pages of one document from disk.
        /// </summary>
        /// <param name="pages">The pathes of the files that should be deleted.</param>
        public static void DeletePagesFromSeparateFile(IEnumerable<string> pages)
        {
            foreach (var page in pages)
            {
                if (TryOpenFile(page, 3))
                {
                    File.Delete(page);
                }
            }
        }

        /// <summary>
        /// Deletes file from disk.
        /// </summary>
        /// <param name="filePath">The path of the file that should be deleted.</param>
        public static void DeleteFile(string filePath)
        {
            if (TryOpenFile(filePath, 3))
            {
                File.Delete(filePath);
            }
        }
        
        private static bool IsBitmapImage(string filePath)
        {
            return Path.GetExtension(filePath) == ".bmp";
        }
    }
}
