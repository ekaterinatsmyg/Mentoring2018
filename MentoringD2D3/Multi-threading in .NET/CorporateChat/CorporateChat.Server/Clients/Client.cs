using System;
using System.Net.Sockets;
using System.Text;
using CorporateChat.Server.Interfaces;
using CorporateChat.Diagnostics;

namespace CorporateChat.Server.Clients
{
    public class Client : IClient
    {
        private string userName;
        private TcpClient tcpClient;
        private readonly IServer chatServer;

        public string Id { get; }
        public NetworkStream Stream { get; set; }


        public Client(TcpClient tcpClient, IServer chatServer)
        {
            Id = Guid.NewGuid().ToString();
            this.tcpClient = tcpClient;
            this.chatServer = chatServer;
            OpenConnection();
        }

        /// <summary>
        /// Exchanges of messages with a client.
        /// </summary>
        public void LaunchClient()
        {
            try
            {
                Stream = tcpClient.GetStream();
                userName = GetInputMessages();

                SendMessagesToClients($"{userName} connected...");

                while (tcpClient.Client.Connected)
                {
                    string message = GetInputMessages();

                    if (!String.IsNullOrEmpty(message))
                        SendMessagesToClients($"{userName}: {message}");
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, $"{ex.Message} {Environment.NewLine} {ex.StackTrace}");
                SendMessagesToClients($"{userName} disconnected...");
                chatServer.RemoveConnection(this.Id);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Connects the client to the server.
        /// </summary>
        public void OpenConnection()
        {
            chatServer.AddConnection(this);
        }

        /// <summary>
        /// Close client's connection.
        /// </summary>
        public void CloseConnection()
        {
            tcpClient.Client?.Close();
            Stream?.Close();
            tcpClient?.Close();
        }

        /// <summary>
        /// Broadcast received documents to connected clients.
        /// </summary>
        /// <param name="message">A message that should be sent to the connected clients.</param>
        private void SendMessagesToClients(string message)
        {
            Console.WriteLine(message);
            chatServer.BroadcastMessage(message, this.Id);
        }

        /// <summary>
        /// Receive messages from the connected clients.
        /// </summary>
        /// <returns></returns>
        private string GetInputMessages()
        {
            byte[] data = new byte[64];
            var builder = new StringBuilder();
            do
            {
                var bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }
    }
}
