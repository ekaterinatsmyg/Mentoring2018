using System;
using System.IO;
using WindowsServices.PrintMergingPages.Client;
using WindowsServices.PrintMergingPages.Registrars;
using NLog;
using NLog.Config;
using NLog.Targets;
using Topshelf;

namespace WindowsService.PrintMergingPages.ClientManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstraper.BuildUnityContainer();

            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget()
            {
                Name = "Default",
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"),
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            HostFactory.Run(conf =>
            {
                conf.Service<IMergePagesClient>(s =>
                {
                    s.ConstructUsing(Bootstraper.Resolve<IMergePagesClient>);
                    s.WhenStarted(x => x.Start());
                    s.WhenStopped(x => x.Stop());
                }).UseNLog(new LogFactory(config));

                conf.SetServiceName("Image Merge Client");
                conf.SetDisplayName("Merge Service");
                conf.SetDescription("Service for merging images into pdf. Client Side");
                conf.StartAutomaticallyDelayed();
                conf.RunAsLocalSystem();
            });
        }
    }
}
