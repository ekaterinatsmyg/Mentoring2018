using System;
using PowerManagementTask;

namespace PowerManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Power.SleepSystem();
            Console.WriteLine(PowerManager.GetSystemPowerInformation());
            Console.WriteLine(PowerManager.GetSystemBatteryState());
            PowerManager.HandleHibernationFile(false);
            Console.ReadKey();
        }
    }
}
