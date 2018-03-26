namespace CorporateChat.Server.Interfaces
{
    public interface IServer
    {
        void Listen();
        void AddConnection(IClient clientObject);
        void RemoveConnection(string clientId);
        void BroadcastMessage(string message, string clientId);
        void Disconnect();

    }
}
