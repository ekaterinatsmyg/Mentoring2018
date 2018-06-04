using System.Xml;
using System.Xml.Schema;
using AdvancedXML.Diagnostics;
using AdvancedXML.Logger;
using NLog;

namespace AdvancedXML.XSD
{
    public class XmlValidator : IXmlValidator
    {
        private readonly XmlReaderSettings xmlReaderSettings;
        private bool isValid;

        public XmlValidator(string xmlSchemaPath)
        {
            xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.Schemas.Add("http://library.by/catalog", xmlSchemaPath);
            xmlReaderSettings.ValidationEventHandler += ValidationEventHandler;
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            xmlReaderSettings.ValidationFlags = xmlReaderSettings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
        }


        /// <summary>
        /// Validates the xml by the xml schema.
        /// </summary>
        /// <param name="xmlPath">The path of the verifying xml.</param>
        /// <returns>If the xml file is valid returns true, else - false.</returns>
        [MethodInfoLogging]
        public bool IsXmlValid(string xmlPath)
        {
            isValid = true;

            var reader = XmlReader.Create(xmlPath, xmlReaderSettings);
            while (reader.Read());

            return isValid;
        }

        /// <summary>
        /// Calls if the xml has issues during reading with format/structure.
        /// </summary>
        /// <param name="sender">The initiator of the event.</param>
        /// <param name="e">The data related to the errors/warning that appeared during reading the xml.</param>
        [MethodInfoLogging]
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            isValid = false;
            var message = $"{e.Message} | Line Number: { e.Exception.LineNumber} | Line Position {e.Exception.LinePosition}";

            ApplicationLogger.LogMessage(
                e.Severity == XmlSeverityType.Warning ? LogMessageType.Warn : LogMessageType.Error, message);
        }
    }
}
