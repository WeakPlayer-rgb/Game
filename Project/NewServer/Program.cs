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
        public const int Size = 8000;
        public static List<Bullet> bullets;
        public static Dictionary<Point, Tree> fullMap;
        public static string serializedMap;

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
                var forReceive = new Dictionary<Socket, Task<int>>();
                var clientData = new Dictionary<Socket, ArraySegment<byte>>();
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
                var allPlayers = new Dictionary<Socket, Player>();
                bullets = new List<Bullet>();

                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                var task = sListener.AcceptAsync();

                // Начинаем слушать соединения
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
                            foreach (var oldData in dataForClient.Values) oldData.OtherPlayers.Add(data.Player);
                            allPlayers[task.Result] = data.Player;
                            task = sListener.AcceptAsync();
                            break;
                        case true:
                            task = sListener.AcceptAsync();
                            break;
                    }

                    foreach (var socket in sendDictionary.Keys.Where(socket =>
                        sendDictionary[socket].IsCompleted &&
                        (!forReceive.ContainsKey(socket) || forReceive[socket].IsCompleted)))
                    {
                        var data = dataForClient[socket];
                        data.OtherPlayers = allPlayers.Where(x => x.Key != socket).Select(x => x.Value).ToList();
                        socket.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))),
                            SocketFlags.None);
                        if (!forReceive.ContainsKey(socket))
                        {
                            clientData.Add(socket, new ArraySegment<byte>(new byte[2048]));
                            forReceive.Add(socket, socket.ReceiveAsync(clientData[socket], SocketFlags.None));
                        }
                        else
                            forReceive[socket] = socket.ReceiveAsync(clientData[socket], SocketFlags.None);
                    }

                    foreach (var socket in forReceive.Keys.Where(socket => forReceive[socket].IsCompleted).ToList())
                    {
                        if (!socket.Connected)
                        {
                            clientData.Remove(socket);
                            forReceive.Remove(socket);
                            continue;
                        }
                        var newData = new DataFromClientToServer();
                        lock (clientData[socket].Array)
                        {
                            var str = Encoding.UTF8.GetString(clientData[socket].Array);
                            Console.WriteLine(str);
                            newData =
                                (DataFromClientToServer) JsonConvert.DeserializeObject(str,
                                    typeof(DataFromClientToServer));
                        }

                        if (newData == null) continue;
                        allPlayers[socket] = newData.NewPlayerPosition;
                        bullets.AddRange(newData.NewBullets);
                    }

                    // var IWrote = false;
                    // foreach (var p in allPlayers.Values)
                    // {
                    //     Console.Write(p.ToString());
                    //     IWrote = true;
                    // }
                    // if(IWrote)
                    //     Console.WriteLine();
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

        private static Player CreateNewPlayer()
        {
            var rnd = new Random();
            return new Player(new Point(rnd.Next(Size), rnd.Next(Size)));
        }

        public static void MoveBullets()
        {
            var forRemoveBullets = new List<Bullet>();
            var forRemoveGameObject = new List<Point>();
            foreach (var bullet in bullets)
            {
                bullet.Position = new Point(bullet.Position.X + (int) bullet.Direction.X,
                    bullet.Position.Y + (int) bullet.Direction.Y);
                var point = new Point(bullet.Position.X / 32 * 32, bullet.Position.Y / 32 * 32);
                if (fullMap.ContainsKey(point))
                {
                    fullMap[point].Health -= bullet.Damage;
                    forRemoveBullets.Add(bullet);
                    if (fullMap[point].Health <= 0)
                        forRemoveGameObject.Add(point);
                }
                else if (fullMap.ContainsKey(new Point(point.X, point.Y - 32)))
                {
                    fullMap[new Point(point.X, point.Y - 32)].Health -= bullet.Damage;
                    forRemoveBullets.Add(bullet);
                    if (fullMap[new Point(point.X, point.Y - 32)].Health <= 0)
                        forRemoveGameObject.Add(new Point(point.X, point.Y - 32));
                }
            }

            lock (bullets)
                foreach (var bullet in forRemoveBullets)
                    bullets.Remove(bullet);
            foreach (var point in forRemoveGameObject) fullMap.Remove(point);
        }
    }
}