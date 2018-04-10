using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerManagementTask;

namespace PowerManagmentTaskTest
{
    [TestClass]
    public class PowerManagmentTest
    {
        [TestMethod]
        public void GetSystemBatteryStateTest()
        {
            Console.WriteLine($"Current Battery State {PowerManager.GetSystemBatteryState()}");
        }

        [TestMethod]
        public void GetSystemPowerInformationTest()
        {
            Console.WriteLine($"System Power Information {PowerManager.GetSystemPowerInformation()}");
        }

        [TestMethod]
        public void GetLastSleepTimeTest()
        {
            Console.WriteLine($"Last Sleep Time {PowerManager.GetLastSleepTime()}");
        }

        [TestMethod]
        public void GetLastWakeTimeTest()
        {
            Console.WriteLine($"Last Sleep Time {PowerManager.GetLastWakeTime()}");
        }
        
        [TestMethod]
        public void SleepSystemTest()
        {
            PowerManager.SleepSystem();
        }

        [TestMethod]
        public void HybernateSystemTest()
        {
            PowerManager.HybernateSystem();
        }

        [TestMethod]
        public void RemoveHibernationFileTest()
        {
            PowerManager.RemoveHibernationFile();
        }


        [TestMethod]
        public void ReserveHibernationFileTest()
        {
            PowerManager.ReserveHibernationFile();
        }
    }
}
