using System;
using System.Configuration;
using System.Linq;
using IQueryableTask.E3SQueryProvider;
using IQueryableTask.E3SQueryProvider.E3SClient;
using IQueryableTask.E3SQueryProvider.E3SClient.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQueryableTask.Tests
{
    [TestClass]
    public class E3SProviderTests
    {
        [TestMethod]
        public void WithoutProvider()
        {
            var client = new E3SQueryClient("katsiaryna_tsmyh@epam.com", "UnusoH832505");
            var res = client.SearchFTS<EmployeeEntity>("workstation:(EPRUIZHW0249)", 0, 1);

            foreach (var emp in res)
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        [TestMethod]
        public void WithoutProviderNonGeneric()
        {
            var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var res = client.SearchFTS(typeof(EmployeeEntity), "workstation:(EPRUIZHW0249)", 0, 10);

            foreach (var emp in res.OfType<EmployeeEntity>())
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }


        [TestMethod]
        public void WithProvider()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation == "EPRUIZHW0249"))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }
    }
}
