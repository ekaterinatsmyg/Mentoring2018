using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AsyncTasks.Task3.Mappers;
using AsyncTasks.Task3.Registrars;

namespace AsyncTasks.Task3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingRegistrar.Register();
            Mapper.Initialize();
        }
    }
}
