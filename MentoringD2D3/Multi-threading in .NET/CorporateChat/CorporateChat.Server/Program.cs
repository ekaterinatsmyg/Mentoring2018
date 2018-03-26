using System;
using System.Threading;
using CorporateChat.Diagnostics;

namespace CorporateChat.Server
{
    class Program
    {
        static Servers.Server server; 
        static Thread listenThread; 
        static void Main(string[] args)
        {
            try
            {
                server = new Servers.Server();
                listenThread = new Thread(server.Listen);
                listenThread.Start();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, ex.Message);
                server.Disconnect();
            }
        }
    }
}
