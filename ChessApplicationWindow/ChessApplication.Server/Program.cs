using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChessApplication.Server
{
    class Program
    {
        static int port = 8888; // Порт
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

                while (users.Count != 2)
                {
                    users.Add(listenSocket.Accept());
                    Console.WriteLine("Пользователь подключен.");
                }

                Socket white = users[0];
                Socket black = users[1];

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
    }
}
