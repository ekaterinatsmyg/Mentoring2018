using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXing;

namespace WindowsServices.PrintMergingPages.Tests
{
    [TestClass]
    public class MergePagesServiceTest
    {
        [TestMethod]
        public void GenerateBarcodeImages()
        {
            var barcodeWriter = new BarcodeWriter();

            // set the barcode format
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            
            barcodeWriter
                .Write("https://jeremylindsayni.wordpress.com/")
                .Save(@"D:\test\img_005.bmp");

            barcodeWriter
                .Write("https://jeremylindsayni.wordpress.com/")
                .Save(@"D:\test\img_003.bmp");


            barcodeWriter
                .Write("https://jeremylindsayni.wordpress.com/")
                .Save(@"D:\test\img_100.bmp");
        }
    }
}
