﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NewGame;
using Newtonsoft.Json;
using DataFromClientToServer = NewGame.DataFromClientToServer;
using DataFromServerToClient = NewGame.DataFromServerToClient;

namespace NewClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void SendMessageFromSocket(int port)
        {
            
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством
            
            // Устанавливаем удаленную точку для сокета
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[1];
            var ipEndPoint = new IPEndPoint(ipAddr, port);
            
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            while (true)
            {
                sender.Receive(bytes);
                var str = Encoding.UTF8.GetString(bytes);
                Console.Write(JsonConvert.DeserializeObject(str,typeof(DataFromServerToClient)));
                var data = new DataFromClientToServer {NewBullets = new List<Bullet>(), NewPlayerPosition = new Point(0, 0)};
                sender.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            }
        }
    }
}