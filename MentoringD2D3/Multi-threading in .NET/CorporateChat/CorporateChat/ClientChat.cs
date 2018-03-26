using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CorporateChat.Diagnostics;
using CorporateChat.Helpers;

namespace CorporateChat
{
    public class ClientChat
    {
        #region consts
        private const string HOST = "127.0.0.1";
        private const int PORT = 8888;
        private const int MIN_SENT_MSG = 1;
        private const int MAX_SENT_MSG = 10;
        private const int MAX_MSG_SEND_DELAY = 5;
        #endregion 

        private readonly Random randomGenerator = new Random();

        private readonly string userName;
        private ClientState clientState;

        private static TcpClient tcpClient;

        /// <summary>
        /// Event signals that reading is done.
        /// </summary>
        private static ManualResetEvent ReadDone = new ManualResetEvent(false);

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken token = cancelTokenSource.Token;
        public ClientChat()
        {
            userName = Guid.NewGuid().ToString();
            tcpClient = new TcpClient();
        }

        /// <summary>
        /// Launch a bot. Starts exchanging messages.
        /// </summary>
        public void Launch()
        {
            try
            {
                Connect();

                MessageExchangeAsync(userName);

                Task.Factory.StartNew(MessageExchange);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Disconnect();
            }
        }

        /// <summary>
        /// Send a random number of messages and receive messages from server.
        /// </summary>
        private void MessageExchange()
        {
            int numberOfMessages = randomGenerator.Next(MIN_SENT_MSG, MAX_SENT_MSG);
            for (int i = 0; i < numberOfMessages; i++)
            {
                MessageDelayInitiate();

                MessageExchangeAsync(ClientMessagesHelper.GetRandomMessage());
            }
            cancelTokenSource.Cancel();
            Disconnect();
        }

        /// <summary>
        /// Imitate delay between messages.
        /// </summary>
        private void MessageDelayInitiate()
        {
            int delaySeconds = randomGenerator.Next(0, MAX_MSG_SEND_DELAY);
            var delayTask = Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            Task.WaitAll(delayTask);
        }

        /// <summary>
        /// Open connection of a TCP client.
        /// </summary>
        public void Connect()
        {
            tcpClient.Connect(HOST, PORT);
            clientState = new ClientState(tcpClient.GetStream());
            ApplicationLogger.LogMessage(LogMessageType.Info, $"Client {userName} was connected to the {HOST}:{PORT}.");
        }

        /// <summary>
        /// Close connection of a TCP client.
        /// </summary>
        public void Disconnect()
        {
            ApplicationLogger.LogMessage(LogMessageType.Info, $"Disconnectiong {userName} ...");

            tcpClient.Client?.Close();
            tcpClient?.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Send and receive messages from server.
        /// </summary>
        /// <param name="message"></param>
        private void MessageExchangeAsync(string message)
        {
            if (message != null)
            {
                clientState.ByteBuffer = Encoding.Unicode.GetBytes(message);
                var result = clientState.NetworkStream.BeginWrite(clientState.ByteBuffer, 0, clientState.ByteBuffer.Length, WriteAysnc, clientState);

                Console.WriteLine($"you: {message}");

                result.AsyncWaitHandle.WaitOne();

                TryToReceiveMessageAsync();
            }
        }

        /// <summary>
        /// Receive incoming messages from a server.
        /// </summary>
        private void TryToReceiveMessageAsync()
        {
            Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested)
                {
                    ApplicationLogger.LogMessage(LogMessageType.Info, "Receiving was canceled ...");
                    return;
                }
                ReceiveMessagesAsync();
            }, token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Current);
        }


        /// <summary>
        /// Receive messages from a server.
        /// </summary>
        private void ReceiveMessagesAsync()
        {
            clientState.ByteBuffer = new byte[1024];
            clientState.NetworkStream.BeginRead(clientState.ByteBuffer, 0, clientState.ByteBuffer.Length, ReadCallback, clientState);
            
            ReadDone.WaitOne();
        }

        /// <summary>
        /// Post writing work.
        /// </summary>
        /// <param name="result"></param>
        private static void WriteAysnc(IAsyncResult result)
        {
            ClientState clientState = result.AsyncState as ClientState;
            clientState?.NetworkStream.EndWrite(result);

        }

        /// <summary>
        /// Post reading work.
        /// </summary>
        /// <param name="result"></param>
        public static void ReadCallback(IAsyncResult result)
        {
            if (result.AsyncState is ClientState clientState)
            {
                int bytesRecieved = clientState.NetworkStream.EndRead(result);
                string icomingMessage = Encoding.Unicode.GetString(clientState.ByteBuffer, 0, bytesRecieved);

                if(!String.IsNullOrEmpty(icomingMessage))
                    Console.WriteLine(icomingMessage);
            }

            ReadDone.Set();
        }
    }
}
