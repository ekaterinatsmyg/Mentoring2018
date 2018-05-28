using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace KeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Your code for CrackMe.exe");
            Console.WriteLine(GenerateKey());
            Console.ReadLine();
        }
        
        private static string GenerateKey()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();

            var dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());

            var code = addressBytes
                .Select((x, i) => x ^ dateBytes[i])
                .Select(x => x <= 999 ? x * 10 : x)
                .ToArray();

            return string.Join("-", code);
        }
    }
}
