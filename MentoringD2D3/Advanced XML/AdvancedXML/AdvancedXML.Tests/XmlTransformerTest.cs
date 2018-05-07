using AdvancedXML.XSD;
using AdvancedXML.XSLT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedXML.Tests
{
    [TestClass]
    public class XmlTransformerTest
    {
        [TestMethod]
        public void ToAtomFormatTest()
        {
            XmlTransformer.ToAtomFormat(XmlValidatorHelper.XmlPath, "result.xml");
        }

        [TestMethod]
        public void ToHtmlFormatTest()
        {
            XmlTransformer.ToHtmlFormat(XmlValidatorHelper.XmlPath, "result.xml");
        }
    }
}
