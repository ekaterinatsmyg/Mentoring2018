using System;

namespace CorporateChat
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientChat client = new ClientChat();
            client.Launch();

            Console.ReadKey();
        }

    }
}

