using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WindowsServices.PrintMergingPages.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;


namespace WindowsServices.PrintMergingPages.Service
{
    public static class ImageMergingHelper
    {
        public static byte[] ConvertIntoSinglePdf(List<string> filePaths)
        {
            Document doc = new Document();
            doc.SetPageSize(PageSize.A4);

            var stream = new MemoryStream();

            PdfCopy pdf = new PdfCopy(doc, stream);
            doc.Open();

            foreach (string path in filePaths)
            {
                byte[] data = File.ReadAllBytes(path);
                doc.NewPage();
                Document imageDocument = null;
                PdfWriter imageDocumentWriter = null;
                var extenision = Path.GetExtension(path).ToLower().Trim('.');
                switch (extenision)
                {
                    case "bmp":
                    case "gif":
                    case "jpg":
                    case "png":
                        AddPage(doc, pdf, null, data);
                        break;
                    case "tif":
                    case "tiff":
                        List<Bitmap> bmpLst = new List<Bitmap>();
                        using (var temporaryStream = new MemoryStream(data))
                        {
                            TiffBitmapDecoder decoder = new TiffBitmapDecoder(temporaryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                            int totFrames = decoder.Frames.Count;

                            for (int i = 0; i < totFrames; ++i)
                            {
                                Bitmap bmpSingleFrame = BitmapFromSource(decoder.Frames[i]);

                                bmpLst.Add(bmpSingleFrame);
                            }
                        }


                        
                        foreach (Bitmap bmp in bmpLst)
                        {
                            AddPage(doc, pdf, bmp);
                        }
                        break;
                    case "pdf":
                        var reader = new PdfReader(data);
                        for (int i = 0; i < reader.NumberOfPages; i++)
                        {
                            pdf.AddPage(pdf.GetImportedPage(reader, i + 1));
                        }
                        pdf.FreeReader(reader);
                        reader.Close();
                        break;
                    default:
                        ApplicationLogger.LogMessage(LogMessageType.Warn, $"Not supported image format {extenision}");
                        break;
                }
            }

            if (doc.IsOpen()) doc.Close();
            return stream.ToArray();

        }

        private static void AddPage(Document doc, PdfCopy pdf, Bitmap bmp = null,  byte[] data = null)
        {
            Image image;
            using (var imageStream = new MemoryStream())
            {
                using (var imageDocument = new Document())
                {
                    imageDocument.Open();

                    if (!imageDocument.NewPage())
                        return;

                    if (bmp != null && data == null)
                    {
                        using (var tmp = new Bitmap(bmp))
                        {
                            var converter = new ImageConverter();
                            image = Image.GetInstance((byte[])converter.ConvertTo(tmp, typeof(byte[])));
                        }
                    }
                    else
                    {
                        image = Image.GetInstance(data);
                    }

                    image.Alignment = Element.ALIGN_CENTER;
                    image.ScaleToFit(doc.PageSize.Width - 10, doc.PageSize.Height - 10);

                    if (!imageDocument.Add(image))
                    {
                        throw new Exception("Unable to add image to page!");
                    }

                    using (var imageDocumentReader = new PdfReader(imageStream.ToArray()))
                    {
                        var page = pdf.GetImportedPage(imageDocumentReader, 1);
                        pdf.AddPage(page);
                    }
                }
            }
        }

        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            source.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
    }

}
