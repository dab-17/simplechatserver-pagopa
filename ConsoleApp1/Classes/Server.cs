using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PagoPa.SimpleChat.Console.Classes
{
    public class Server : TcpListener
    {
        public Server(IPAddress localaddr, int port) : base(localaddr, port)
        {
            Clients = new();
        }

        public List<Guest> Clients { get; set; }

        public void Broadcast(string message, Guid? from = null, bool? excludeSender = null)
        {
            var clientToSend = Clients;
            if (excludeSender.GetValueOrDefault(false))
                clientToSend = Clients
                    .Where(c => c.Id != from)
                    .ToList();

            foreach (Guest c in clientToSend)
                Write(message, c.TcpClient.GetStream());
        }

        public void Init()
        {
            try
            {
                Start();

                byte[] bytes = new byte[256];
                string data = null;

                while (true)
                {
                    TcpClient client = AcceptTcpClient();
                    new Thread(() =>
                    {
                        data = null;
                        NetworkStream stream = client.GetStream();
                        Write("Type your nickname: ", stream);
                        StreamReader sr = new(stream);
                        string nickName = sr.ReadLine();
                        Write($"Hello {nickName} !", stream);
                        Guest guest = new(nickName, client);
                        Clients.Add(guest);
                        while (true)
                        {
                            try
                            {
                                data = sr.ReadLine();
                                byte[] msg = Encoding.UTF8.GetBytes(data);
                                Broadcast($"\n\n{guest.NickName} says {data}", guest.Id, true);
                            }
                            finally
                            {
                                client.Close();
                                Clients.Remove(guest);
                            }
                        }
                    }).Start();

                }
            }
            catch (SocketException e)
            {
                //log
                throw;
            }
            finally
            {
                Stop();
            }
        }

        private static void Write(string message, NetworkStream stream)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            stream.Write(msg, 0, msg.Length);
        }
    }
}
