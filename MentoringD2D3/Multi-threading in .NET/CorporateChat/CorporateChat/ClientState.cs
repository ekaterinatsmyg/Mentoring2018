using System;
using System.Net.Sockets;
using System.Text;

namespace CorporateChat
{
    /// <summary>
    /// The state of a client for async writing and reading
    /// </summary>
    public class ClientState
    {

        private StringBuilder response;

        public int TotalBytes { get; set; }

        public NetworkStream NetworkStream { get; set; }

        public Byte[] ByteBuffer { get; set; }

        public string Response => response.ToString();

        public ClientState(NetworkStream networkStream)
        {
            NetworkStream = networkStream;
            response = new StringBuilder();
        }
    }
}
