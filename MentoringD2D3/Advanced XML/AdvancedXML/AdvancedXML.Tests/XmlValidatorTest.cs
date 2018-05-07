using AdvancedXML.DependencyRegistrars;
using AdvancedXML.XSD;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedXML.Tests
{
    [TestClass]
    public class XmlValidatorTest
    {
        [TestInitialize]
        public void Setup()
        {
            Bootstrapper.BuildUnityContainer();
        }

        [TestMethod]
        public void IsXmlValidTest()
        {
            var xmlValidator = Bootstrapper.Resolve<IXmlValidator>();
            Assert.IsTrue(xmlValidator.IsXmlValid(XmlValidatorHelper.XmlPath));
        }
    }
}
