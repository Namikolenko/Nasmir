﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChessApplication.Server
{
    class Program
    {
        static int port = 8888; // Unused port
        static IPAddress localAddr = IPAddress.Parse("25.56.174.87"); // Hamachi IP address
        static byte[] data = new byte[4096]; // Buffer
        static void Main(string[] args)
        {
            TcpListener server = null;
            List<TcpClient> users = new List<TcpClient>();

            try
            {
                server = new TcpListener(localAddr, port);
                //START SERVER
                server.Start();
                Console.WriteLine("Ожидание подключений...");

                while (users.Count != 2) // Wait for a connection
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент.");
                    users.Add(client);
                }
                NetworkStream streamWhite = users[0].GetStream(); // Set streamwriters
                NetworkStream streamBlack = users[1].GetStream();

                string messWhite = "white";
                string messBlack = "black";

                //data = Encoding.Unicode.GetBytes(messWhite); // Sending approval message
                //streamWhite.Write(data, 0, data.Length);

                //data = Encoding.Unicode.GetBytes(messBlack);
                //streamBlack.Write(data, 0, data.Length);

                StringBuilder builderWhite = new StringBuilder();
                StringBuilder builderBlack = new StringBuilder();

                while (true)  // Dialog between white and black
                {
                    int bytes = 0;
                    do
                    {
                        bytes = streamWhite.Read(data, 0, data.Length);
                        builderWhite.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (streamWhite.DataAvailable);
                    
                    do
                    {
                        bytes = streamBlack.Read(data, 0, data.Length);
                        builderBlack.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (streamBlack.DataAvailable);

                    string messageBlack = builderBlack.ToString();
                    string messageWhite = builderWhite.ToString();

                    data = Encoding.Unicode.GetBytes(messageBlack);
                    streamWhite.Write(data, 0, data.Length);

                    data = Encoding.Unicode.GetBytes(messageWhite);
                    streamBlack.Write(data, 0, data.Length);
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //finally
            //{
            //    if (server != null)
            //        server.Stop();
            //}
        }
    }
}
