using System;
using System.Configuration;
using System.Diagnostics;
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
            var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var res = client.SearchFTS<EmployeeEntity>(new []{"workstation:(EPRUIZHW0249)"}, 0, 1);

            foreach (var emp in res)
            {
				Console.WriteLine("{0} {1}", emp.nativeName, emp.shortStartWorkDate);
            }
        }

        [TestMethod]
        public void WithoutProviderNonGeneric()
        {
            var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var res = client.SearchFTS(typeof(EmployeeEntity), new[] {"workstation:(EPRUIZHW0249)"}, 0, 10);

            foreach (var emp in res.OfType<EmployeeEntity>())
            {
                Console.WriteLine("{0} {1}", emp.nativeName, emp.shortStartWorkDate);
            }
        }


        [TestMethod]
        public void WithProvider()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            
            foreach (var emp in employees.Where(e =>  e.workStation == "EPRUIZHW0249"))
            {
                Debug.WriteLine("{0} {1}", emp.nativeName, emp.shortStartWorkDate);
            }
        }

        [TestMethod]
        public void WhereTest()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var result = employees.Where(e => "EPRUIZHW0249" == e.workStation).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void StartsWithTest()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var result = employees.Where(e => e.workStation.StartsWith("EPRUIZHW02")).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void EndsWithTest()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var result = employees.Where(e => e.firstName.EndsWith("rew")).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ContainsTest()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var result = employees.Where(e => e.firstName.Contains("ryna")).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void AndTest()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var result = employees.Where(e => e.firstName.Contains("aryn") && e.workStation.StartsWith("EPBYMIN")).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}
