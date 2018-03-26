using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CorporateChat.Diagnostics;
using CorporateChat.Server.Clients;
using CorporateChat.Server.Interfaces;
using CorporateChat.ServerClient;

namespace CorporateChat.Server.Servers
{
    public class Server : IServer
    {
        private static TcpListener tcpListener;
        private List<IClient> connectedClients = new List<IClient>();
        private ChatHistory chatHistory = new ChatHistory();
        private const int PORT = 8888;

        /// <summary>
        /// Listen incoming connections.
        /// </summary>
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, PORT);
                tcpListener.Start();
                ApplicationLogger.LogMessage(LogMessageType.Info, "Server started. Waiting for conections...");;

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    IClient clientObject = new Client(tcpClient, this);
                    Task.Run(() => clientObject.LaunchClient());
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, $"{ex.Message} {Environment.NewLine} {ex.StackTrace}");
                Disconnect();
            }
        }
        
        /// <summary>
        /// Connect a client to the server.
        /// </summary>
        /// <param name="clientObject">The client that is connecting.</param>
        public void AddConnection(IClient clientObject)
        {
            connectedClients.Add(clientObject);
        }

        /// <summary>
        /// Disconnect client by client's id.
        /// </summary>
        /// <param name="clientId">Identifier of the connected client.</param>
        public void RemoveConnection(string clientId)
        {
            IClient client = connectedClients.FirstOrDefault(c => c.Id == clientId);

            if (client != null)
                connectedClients.Remove(client);
        }

        /// <summary>
        ///  Broadcast messages to all connected clients.
        /// </summary>
        /// <param name="message">A message that should be sent to all connected clients.</param>
        /// <param name="clientId">Connected clients.</param>
        public void BroadcastMessage(string message, string clientId)
        {
            chatHistory.AddMessageToHistory(message);
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (var client in connectedClients)
            {
                if (client.Id != clientId)
                {
                    client.Stream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Disconnect all clients.
        /// </summary>
        public void Disconnect()
        {
            tcpListener.Stop();

            foreach (IClient client in connectedClients)
            {
                client.CloseConnection();
            }

            chatHistory.SaveHistory();
            ApplicationLogger.LogMessage(LogMessageType.Info, "Server disconnected.");
            Environment.Exit(0);
        }
    }
}
