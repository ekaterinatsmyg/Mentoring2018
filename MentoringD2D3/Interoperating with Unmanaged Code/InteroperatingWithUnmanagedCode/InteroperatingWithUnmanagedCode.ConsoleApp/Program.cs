using System;
using PowerManagementTask;

namespace InteroperatingWithUnmanagedCode.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Charge level {PowerManager.GetBatteryLifePercent()} %");
            Console.Read();
        }
    }
}
