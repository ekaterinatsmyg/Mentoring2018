using System.Net.Sockets;

namespace CorporateChat.Server.Interfaces
{
    public interface IClient
    {
        string Id { get; }
        NetworkStream Stream { get; set; }
        void LaunchClient();
        void OpenConnection();
        void CloseConnection();
    }
}
