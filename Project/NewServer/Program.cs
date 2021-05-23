using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NewGame;
using Newtonsoft.Json;

namespace NewServer
{
    internal static class Program
    {
        private const int Size = 8000;
        private static List<Bullet> bullets;
        private static Dictionary<Point, Tree> fullMap;
        private static string serializedMap;
        private static Dictionary<Socket, Player> allPlayers;

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.String")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.Char[]")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.RuntimeMethodInfoStub")]
        [SuppressMessage("ReSharper.DPA", "DPA0004: Closure object allocation")]
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        static void Main(string[] args)
        {
            // Устанавливаем для сокета локальную конечную точку
            var ip = Dns.GetHostName();
            var ipHost = Dns.GetHostEntry(ip);
            var ipAddr = ipHost.AddressList[3];
            var ipEndPoint = new IPEndPoint(ipAddr, 11000);

            var timer = new Timer {Interval = 5};
            timer.Elapsed += (sender, e) => { MoveBullets(); };
            timer.Start();
            // Создаем сокет Tcp/Ip
            var sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                var sendDictionary = new Dictionary<Socket, Task<int>>();
                // var forReceive = new Dictionary<Socket, Task<int>>();
                // var clientData = new Dictionary<Socket, ArraySegment<byte>>();
                fullMap = new Dictionary<Point, Tree>();
                var rnd = new Random();
                for (var i = 0; i < 500; i++)
                {
                    var newPoint = new Point(rnd.Next(0, Size / 32), rnd.Next(0, Size / 32)); // 40, 70
                    if (!fullMap.ContainsKey(newPoint))
                    {
                        var point = new Point(newPoint.X * 32, newPoint.Y * 32);
                        fullMap[point] = new Tree(point);
                    }
                }

                serializedMap = JsonConvert.SerializeObject(fullMap);
                var dataForClient = new Dictionary<Socket, DataFromServerToClient>();
                allPlayers = new Dictionary<Socket, Player>();
                bullets = new List<Bullet>();

                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                Console.WriteLine(@"Ожидаем соединение через порт {0}", ipEndPoint);
                var task = sListener.AcceptAsync();

                while (true)
                {
                    switch (task.IsCompleted)
                    {
                        case true when !sendDictionary.ContainsKey(task.Result):
                            var data = new FirstConnection();
                            data.Map = fullMap;
                            data.Player = CreateNewPlayer();
                            data.Bullets = bullets;
                            data.OtherPlayers = allPlayers.Values.ToList();
                            dataForClient[task.Result] = new DataFromServerToClient
                                {Bullets = bullets, OtherPlayers = allPlayers.Values.ToList()};
                            var str = JsonConvert.SerializeObject(data);
                            var send = new ArraySegment<byte>(
                                Encoding.UTF8.GetBytes(str));
                            sendDictionary[task.Result] =
                                task.Result.SendAsync(send, SocketFlags.None);
                            WorkWithClient(task.Result);
                            foreach (var oldData in dataForClient.Values) oldData.OtherPlayers.Add(data.Player);
                            lock (allPlayers)
                            {
                                allPlayers[task.Result] = data.Player;
                            }

                            task = sListener.AcceptAsync();
                            break;
                        case true:
                            task = sListener.AcceptAsync();
                            break;
                    }

                    if (allPlayers != null)
                    {
                        lock (allPlayers)
                        {
                            foreach (var p in allPlayers.Values)
                            {
                                Console.WriteLine(p.Direction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static async void WorkWithClient(Socket socket)
        {
            await MakeAsync(socket);
        }

        private static Task MakeAsync(Socket connect)
        {
            DataFromServerToClient dataServerClient;
            DataFromClientToServer dataClientServer;
            var task = new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        var data = new byte[2048];
                        connect.Receive(data);
                        var str = Encoding.UTF8.GetString(data);
                        dataClientServer = (DataFromClientToServer) JsonConvert.DeserializeObject(str,
                            typeof(DataFromClientToServer));
                        lock (allPlayers) allPlayers[connect] = dataClientServer?.NewPlayerPosition;
                        lock (bullets)
                            if (dataClientServer?.NewBullets != null)
                                foreach (var bull in dataClientServer?.NewBullets)
                                    bullets.Add(bull);
                        dataServerClient = new DataFromServerToClient();
                        lock (bullets) dataServerClient.Bullets = bullets;
                        lock (allPlayers)
                            dataServerClient.OtherPlayers = allPlayers.Values.ToList();
                        lock (allPlayers)
                            if (dataClientServer != null)
                                dataServerClient.ChangeHpPlayer =
                                    dataClientServer.NewPlayerPosition.Health - allPlayers[connect].Health;
                        connect.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataServerClient)),
                            SocketFlags.None);
                    }
                }
                catch
                {
                    lock (allPlayers)
                    {
                        allPlayers.Remove(connect);
                    }
                }
            });
            task.Start();
            return task;
        }

        private static Player CreateNewPlayer()
        {
            var rnd = new Random();
            return new Player(new Point(rnd.Next(Size), rnd.Next(Size)),0);
        }

        private static void MoveBullets()
        {
            var forRemoveBullets = new List<Bullet>();
            var forRemoveGameObject = new List<Point>();
            Dictionary<Point, Player> players;
            lock (allPlayers)
            {
                players = allPlayers.Values.ToDictionary(x => x.Position);
            }

            lock (bullets)
            {
                lock (fullMap)
                {
                    foreach (var bullet in bullets)
                    {
                        bullet.MoveThisBullet();
                        for (var x = bullet.Position.X - 50; x < bullet.Position.X + 50; x++)
                        for (var y = bullet.Position.Y - 50; y < bullet.Position.Y + 50; y++)
                        {
                            if (!players.ContainsKey(new Point(x, y)) || bullet.Tick <= 10) continue;
                            players[new Point(x, y)].Health -= bullet.Damage;
                            forRemoveBullets.Add(bullet);
                        }

                        if (bullet.Tick > 60)
                            forRemoveBullets.Add(bullet);
                    }
                }
            }

            lock (bullets)
                foreach (var bullet in forRemoveBullets)
                    bullets.Remove(bullet);
            foreach (var point in forRemoveGameObject) fullMap.Remove(point);
        }
    }
}