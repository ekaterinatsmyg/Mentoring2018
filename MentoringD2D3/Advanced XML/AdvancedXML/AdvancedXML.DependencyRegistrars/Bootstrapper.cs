using AdvancedXML.XSD;
using Unity;
using Unity.Injection;

namespace AdvancedXML.DependencyRegistrars
{
    public class Bootstrapper
    {
        private static readonly IUnityContainer container = new UnityContainer();

        public static void BuildUnityContainer()
        {
            //TODO: pass here correctly connection string
            container
                .RegisterType<IXmlValidator, XmlValidator>(new InjectionConstructor(XmlValidatorHelper.XmlSchemaPath));
        }

        public static T Resolve<T>()
        {
            T ret = default(T);

            if (container.IsRegistered(typeof(T)))
            {
                ret = container.Resolve<T>();
            }

            return ret;
        }
    }
}
