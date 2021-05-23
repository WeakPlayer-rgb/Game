/*using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using NewGame;

namespace Server
{
    internal static class Program
    {
        public const int Size = 8000;
        public static List<Bullet> bullets;
        public static Dictionary<Point, IGameObject> fullMap;
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
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[1];
            var ipEndPoint = new IPEndPoint(ipAddr, 11000);


            var timer = new Timer {Interval = 5};
            timer.Elapsed += (sender,e) => {
                MoveBullets();
            };
            timer.Start();
            // Создаем сокет Tcp/Ip
            var sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                var sendDictionary = new Dictionary<Socket, Task<int>>();
                var forReceive = new Dictionary<Socket, Task<int>>();
                var clientData = new Dictionary<Socket, byte[]>();
                fullMap = new Dictionary<Point, IGameObject>();
                var rnd = new Random();
                for (var i = 0; i < 500; i++)
                {
                    var newPoint = new Point(rnd.Next(0, Size / 32), rnd.Next(0, Size / 32)); // 40, 70
                    if (!fullMap.ContainsKey(newPoint))
                        fullMap[new Point(newPoint.X * 32, newPoint.Y * 32)] =
                            new Tree(new Point(newPoint.X * 32, newPoint.Y * 32));
                }

                serializedMap = JsonSerializer.Serialize(fullMap);
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
                            var send = JsonSerializer.Serialize(data);
                            sendDictionary[task.Result] =
                                task.Result.SendAsync(Encoding.UTF8.GetBytes(send), SocketFlags.Broadcast);
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
                        socket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)), SocketFlags.None);
                        if (!forReceive.ContainsKey(socket))
                        {
                            clientData.Add(socket, new byte[2048]);
                            forReceive.Add(socket, socket.ReceiveAsync(clientData[socket], SocketFlags.None));
                        }
                        else
                            forReceive[socket] = socket.ReceiveAsync(clientData[socket], SocketFlags.None);
                    }

                    foreach (var socket in forReceive.Keys.Where(socket => forReceive[socket].IsCompleted))
                    {
                        if (socket.Blocking)
                        {
                            clientData.Remove(socket);
                            forReceive.Remove(socket);
                            continue;
                        }
                        Console.WriteLine(Encoding.UTF8.GetString(clientData[socket]));
                        var newData = (DataFromClientToServer)JsonSerializer.Deserialize(clientData[socket],typeof(DataFromClientToServer));
                        if (newData == null) continue;
                        allPlayers[socket] = new Player(newData.NewPlayerPosition);
                        bullets.AddRange(newData.NewBullets);
                    }
                    // foreach (var task in TaskList)
                    // {
                    //     if(task.IsCompleted) ClientList.Add(task.Result);
                    // }

                    // Мы дождались клиента, пытающегося с нами соединиться

                    // byte[] bytes = new byte[1024];
                    // int bytesRec = handler.Receive(bytes);
                    //
                    // data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    //
                    // // Показываем данные на консоли
                    // Console.Write("Полученный текст: " + data + "\n\n");
                    //
                    // // Отправляем ответ клиенту\
                    // string reply = "Спасибо за запрос в " + data.Length.ToString()
                    //         + " символов";
                    // byte[] msg = Encoding.UTF8.GetBytes(reply);
                    // handler.Send(msg);
                    //
                    // if (data.IndexOf("<TheEnd>") > -1)
                    // {
                    //     Console.WriteLine("Сервер завершил соединение с клиентом.");
                    //     break;
                    // }

                    // handler.Shutdown(SocketShutdown.Both);
                    // handler.Close();
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
                    if(fullMap[point].Health <=0)
                        forRemoveGameObject.Add(point);
                }
                else if (fullMap.ContainsKey(new Point(point.X, point.Y - 32)))
                {
                    fullMap[new Point(point.X, point.Y - 32)].Health -= bullet.Damage;
                    forRemoveBullets.Add(bullet);
                    if(fullMap[new Point(point.X, point.Y - 32)].Health <=0)
                        forRemoveGameObject.Add(new Point(point.X, point.Y - 32));
                }
            }

            foreach (var bullet in forRemoveBullets) bullets.Remove(bullet);
            foreach (var point in forRemoveGameObject)
            {
                fullMap.Remove(point);
            }
        }
    }
}*/