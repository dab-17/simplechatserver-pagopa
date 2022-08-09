using PagoPa.SimpleChat.Console.Classes;
using System.Net;

int port = 10000;
IPAddress localAddr = IPAddress.Parse("127.0.0.1");
Server server = new(localAddr, port);
server.Init();