using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        public static void Main()
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
            var bytes = new byte[100000];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[1];
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("10.97.160.27"), 11000);
            var dataFromClientToServer = new DataFromClientToServer();
            var connection = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var dataFromServerToClient = new DataFromServerToClient();
            var Bullets = new List<Bullet>();
            var Map = new Dictionary<Point, Tree>();
            var PlayerMap = new List<Player>();

            // Соединяем сокет с удаленной точкой
            connection.Connect(ipEndPoint);
            ConnectSocket(dataFromClientToServer, connection, Bullets, Map, PlayerMap);
        }

        private static void ConnectSocket(DataFromClientToServer dataFromClientToServer, Socket connection, List<Bullet> Bullets,
            Dictionary<Point, Tree> Map, List<Player> PlayerMap)
        {
            DataFromServerToClient dataFromServerToClient;
            while (true)
            {
                var data = new byte[100000];
                string dataForServer;
                lock (dataFromClientToServer) dataForServer = JsonConvert.SerializeObject(dataFromClientToServer);
                connection.Send(Encoding.UTF8.GetBytes(dataForServer));
                connection.Receive(data);
                dataFromServerToClient = new DataFromServerToClient();
                lock (dataFromServerToClient)
                {
                    dataFromServerToClient =
                        (DataFromServerToClient) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data),
                            typeof(DataFromServerToClient));
                    Debug.Assert(dataFromServerToClient != null, nameof(dataFromServerToClient) + " != null");
                    Bullets.AddRange(dataFromServerToClient.Bullets);
                    foreach (var point in Map.Where(x => x.Value.GetType() == typeof(Player)).Select(x => x.Key)
                        .ToList()) Map.Remove(point);
                    PlayerMap.AddRange(dataFromServerToClient.OtherPlayers);
                }
            }
        }
    }
}