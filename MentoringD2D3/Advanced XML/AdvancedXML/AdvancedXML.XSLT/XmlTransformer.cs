using System;
using System.IO;
using System.Xml.Xsl;
using AdvancedXML.Diagnostics;

namespace AdvancedXML.XSLT
{
    public static class XmlTransformer
    {

        /// <summary>
        /// Genereates atom feed file based on input xml.
        /// </summary>
        /// <param name="inputXmlFilePath">The input xml path.</param>
        /// <param name="outputXmlFilePath">The output xml path.</param>
        public static void ToAtomFormat(string inputXmlFilePath, string outputXmlFilePath)
        {
            if (String.IsNullOrEmpty(outputXmlFilePath) || Path.GetExtension(outputXmlFilePath) != ".xml")
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, $"Incorrect output format file {outputXmlFilePath}");
                return;
            }

            Transform(inputXmlFilePath, outputXmlFilePath, XsltHelper.XsltToAtomPath);
        }

        /// <summary>
        /// Genereates html report file based on input xml by genres.
        /// </summary>
        /// <param name="inputXmlFilePath">The input xml path.</param>
        /// <param name="outputHtmlFilePath">The output html path.</param>
        public static void ToHtmlFormat(string inputXmlFilePath, string outputHtmlFilePath)
        {
            if (String.IsNullOrEmpty(outputHtmlFilePath) || Path.GetExtension(outputHtmlFilePath) != ".html")
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, $"Incorrect output format file {outputHtmlFilePath}");
                return;
            }

            Transform(inputXmlFilePath, outputHtmlFilePath, XsltHelper.XsltToHtmlPath);
        }

        /// <summary>
        /// Trasforms input xml file by xslt.
        /// </summary>
        /// <param name="inputXmlFilePath">The input xml path.</param>
        /// <param name="outputFilePath">The output  path.</param>
        /// <param name="xsltPath">The path of the template for xml transformation.</param>
        private static void Transform(string inputXmlFilePath, string outputFilePath, string xsltPath)
        {
            if (String.IsNullOrEmpty(inputXmlFilePath) || Path.GetExtension(outputFilePath) != ".xml")
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, $"Incorrect input format file {inputXmlFilePath}");
                return;
            }
            var xslt = new XslCompiledTransform();

            var settings = new XsltSettings {EnableScript = true};

            xslt.Load(xsltPath, settings, null);

            using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
            {
                xslt.Transform(inputXmlFilePath, new XsltArgumentList(), fileStream);
            }
        }
    }
}
