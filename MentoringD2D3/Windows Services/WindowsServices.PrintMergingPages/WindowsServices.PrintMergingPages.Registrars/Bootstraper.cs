using Unity;

namespace WindowsServices.PrintMergingPages.Registrars
{
    public class Bootstraper
    {
        private static readonly IUnityContainer container = new UnityContainer();

        public static void BuildUnityContainer()
        {
            //container
            //    .RegisterType<IUnitOfWork, OracleClientUnitOfWork>(new InjectionConstructor(ProductConfiguration.ConnectionString))
            //    .RegisterType<IJobsErrorsRepository, ProductionErrorsDBRepository>(new InjectionConstructor(new ResolvedParameter<IUnitOfWork>()))
            //    .RegisterType<IWebRepository, FTWebRepository>()
            //    .RegisterType<IOutputService<OutputModel>, OutputCSVService>(new InjectionConstructor("Output.txt", "Ф"))
            //    .RegisterType<IProductionErrorsService, ProductionErrorsService>(new InjectionConstructor(new ResolvedParameter<IJobsErrorsRepository>(), new ResolvedParameter<IWebRepository>(), new ResolvedParameter<IOutputService<OutputModel>>()));
        }

        public static T Resolve<T>()
        {
            var ret = default(T);

            if (container.IsRegistered(typeof(T)))
            {
                ret = container.Resolve<T>();
            }

            return ret;
        }
    }
}
