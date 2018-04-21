using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using ZXing;

namespace WindowsServices.PrintMergingPages.Service
{
    public static class FileHelper
    {
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

        public static bool TryOpenPage(string path, int tryCount)
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
                    catch (IOException ex)
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

        public static void DeletePagesFromSeparateFile(IEnumerable<string> pages)
        {
            foreach (var page in pages)
            {
                if (TryOpenPage(page, 3))
                {
                    File.Delete(page);
                }
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (TryOpenPage(filePath, 3))
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
