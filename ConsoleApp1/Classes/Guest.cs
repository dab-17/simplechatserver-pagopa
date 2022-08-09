using System.Net.Sockets;

namespace PagoPa.SimpleChat.Console.Classes
{
    public class Guest
    {
        public Guest(string name, TcpClient client)
        {
            NickName = name;
            Id = Guid.NewGuid();
            TcpClient = client;
        }
        public Guid Id { get; private set; }
        public string NickName { get; private set; }
        public TcpClient TcpClient { get; private set; }
    }
}
