using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.Server
{
    class Program
    {
        static int port = 8888; // Порт
        static Socket whiteChat;
        static Socket blackChat;
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("25.56.174.87"), port); // Адрес Хамачи

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                Console.WriteLine("Ожидание подключений...");

                List<Socket> users = new List<Socket>();
                List<Socket> chatUsers = new List<Socket>();

                while (users.Count != 2)
                {
                    users.Add(listenSocket.Accept());
                    chatUsers.Add(listenSocket.Accept());
                    Console.WriteLine("Пользователь подключен.");
                }

                Socket white = users[0];
                whiteChat = chatUsers[0];
                Socket black = users[1];
                blackChat = chatUsers[1];

                Console.WriteLine("Успешно.");

                StringBuilder builderChatBlack = new StringBuilder();

                ChatWhiteThread();
                ChatBlackThread();

                StringBuilder builderWhite = new StringBuilder();
                StringBuilder builderBlack = new StringBuilder();

                black.Send(Encoding.Unicode.GetBytes("b"));
                white.Send(Encoding.Unicode.GetBytes("w"));

                while (true)
                {
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    builderBlack.Clear();
                    builderWhite.Clear();

                    do
                    {
                        bytes = white.Receive(data);
                        builderWhite.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (white.Available > 0);

                    string messageWhite = builderWhite.ToString();
                    data = Encoding.Unicode.GetBytes(messageWhite);
                    Console.WriteLine(messageWhite);
                    black.Send(data);

                    bytes = 0; // количество полученных байтов
                    data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = black.Receive(data);
                        builderBlack.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (black.Available > 0);

                    string messageBlack = builderBlack.ToString();
                    data = Encoding.Unicode.GetBytes(messageBlack);
                    Console.WriteLine(messageBlack);
                    white.Send(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async void ChatWhiteThread()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    int bytesChatW = 0; // количество полученных байтов
                    byte[] dataChatW = new byte[256]; // буфер для получаемых данных
                    StringBuilder builderChatWhite = new StringBuilder();

                    do
                    {
                        bytesChatW = whiteChat.Receive(dataChatW);
                        builderChatWhite.Append(Encoding.Unicode.GetString(dataChatW, 0, bytesChatW));
                    }
                    while (whiteChat.Available > 0);

                    string messageChatWhite = builderChatWhite.ToString();
                    dataChatW = Encoding.Unicode.GetBytes(messageChatWhite);
                    blackChat.Send(dataChatW);
                }
            });
        }
        private static async void ChatBlackThread()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    int bytesChatB = 0; // количество полученных байтов
                    byte[] dataChatB = new byte[256]; // буфер для получаемых данных
                    StringBuilder builderChatBlack = new StringBuilder();

                    do
                    {
                        bytesChatB = blackChat.Receive(dataChatB);
                        builderChatBlack.Append(Encoding.Unicode.GetString(dataChatB, 0, bytesChatB));
                    }
                    while (blackChat.Available > 0);

                    string messageChatBlack = builderChatBlack.ToString();
                    dataChatB = Encoding.Unicode.GetBytes(messageChatBlack);
                    whiteChat.Send(dataChatB);
                }
            }); 
        }
    }
}
