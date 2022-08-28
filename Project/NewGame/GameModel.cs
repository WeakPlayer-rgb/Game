using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewGame
{
    public class GameModel
    {
        public readonly Player Player;
        public readonly Dictionary<Point, Tree> Map;
        public List<Player> PlayerMap;
        public readonly int Size;
        public List<Bullet> Bullets;
        private readonly Socket connection;
        private readonly DataFromClientToServer dataFromClientToServer;
        private DataFromServerToClient dataFromServerToClient;

        public GameModel(int s, string text)
        {
            Size = s;
            var ip = Dns.GetHostName();
            var ipHost = Dns.GetHostEntry(ip);
            var ipAddr = ipHost.AddressList[6];
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(text), 11000);
            connection = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            connection.Connect(ipEndPoint);
            var dataFromServer = new byte[100000];
            connection.Receive(dataFromServer);
            var str = Encoding.UTF8.GetString(dataFromServer);
            var data = (FirstConnection) JsonConvert.DeserializeObject(str, typeof(FirstConnection));
            Player = new Player(Point.Empty, 0);
            Map = new Dictionary<Point, Tree>();
            Bullets = new List<Bullet>();
            PlayerMap = new List<Player>();
            if (data != null)
            {
                Player = data.Player;
                Map = data.Map;
                Bullets = data.Bullets;
                PlayerMap = data.OtherPlayers;
            }

            dataFromClientToServer = new DataFromClientToServer
            {
                NewBullets = new List<Bullet>(), NewPlayerPosition = Player
            };
            WorkWithServer();
        }

        public void ChangePosition()
        {
            if (IsPlayerIntersected())
            {
                var slowDownMultiplier = Player.Speed.X / 2;
                ChangePos(slowDownMultiplier);
                return;
            }
            ChangePos();
        }

        private void ChangePos(double multiplier = 0)
        {
            Player.Position = new Point((int) ((int) Player.Speed.X - multiplier + Player.Position.X),
                (int) Player.Speed.Y + Player.Position.Y);
            var dx = Player.Position.X < 0 ? Size : Player.Position.X > Size ? -Size : 0;
            var dy = Player.Position.Y < 0 ? Size : Player.Position.Y > Size ? -Size : 0;

            var positionX = Player.Position.X + dx;
            var positionY = Player.Position.Y + dy;
            var rect = new Rectangle(positionX, positionY, 45, 80);
        }

        public bool IsPlayerIntersected()
        {
            var playerPos = Player.Position;
            /*var leftDot = new Vector(playerPos.X - 17, playerPos.Y - 30).Rotate(Player.Direction);
            var rightDot = new Vector(playerPos.X + 28, playerPos.Y - 30).Rotate(Player.Direction);
            */
            //var playerRect = new Rectangle(leftTop., rightTop, 1, 1);
            const int limit = 50;
            for (var dy = -limit; dy <= limit; dy++)
            for (var dx = -limit; dx <= limit; dx++)
            {
                var potentialPos = new Point(playerPos.X + dx, playerPos.Y + dy);
                if (!Map.ContainsKey(potentialPos)) continue;
                var obj = Map[potentialPos];
                var objRectangle = obj.ObjRectangle;
                var top = objRectangle.Top;
                var bottom = objRectangle.Bottom;
                var left = objRectangle.Left;
                var right = objRectangle.Right;
                //return IsIntersected(leftDot, rightDot, left,right, bottom, top);
                return AreIntersected(Player.ObjRectangle, objRectangle);
                //return IsIntersected(leftDot, rightDot, left, right, bottom, top);
            }

            return false;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.Char[]")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String")]
        Task MakeAsync()
        {
            var task = new Task(() =>
            {
                while (connection.Connected)
                {
                    var data = new byte[2048];
                    string dataForServer;
                    lock (dataFromClientToServer) dataFromClientToServer.NewPlayerPosition = Player;
                    lock (dataFromClientToServer) dataForServer = JsonConvert.SerializeObject(dataFromClientToServer);
                    connection.Send(Encoding.UTF8.GetBytes(dataForServer));
                    lock (dataFromClientToServer) dataFromClientToServer.NewBullets = new List<Bullet>();
                    connection.Receive(data);
                    dataFromServerToClient = new DataFromServerToClient();
                    var str = Encoding.UTF8.GetString(data);
                    lock (dataFromServerToClient)
                    {
                        dataFromServerToClient =
                            (DataFromServerToClient) JsonConvert.DeserializeObject(str,
                                typeof(DataFromServerToClient));

                        if (dataFromServerToClient.YourNewPlayer != null)
                        {
                            lock (Player)
                            {
                                Player.Position = dataFromServerToClient.YourNewPlayer.Position;
                            }
                        }

                        Debug.Assert(dataFromServerToClient != null, nameof(dataFromServerToClient) + " != null");
                        lock (Bullets)
                            Bullets = dataFromServerToClient.Bullets.ToList();
                        foreach (var point in Map.Where(x => x.Value.GetType() == typeof(Player)).Select(x => x.Key)
                            .ToList()) Map.Remove(point);
                        PlayerMap = dataFromServerToClient.OtherPlayers;
                        lock (Player)
                        {
                            Player.Health -= dataFromServerToClient.ChangeHpPlayer;
                        }

                        if (dataFromServerToClient.ChangeHpTree == null) continue;
                        var forDelete = new List<Point>();
                        foreach (var position in dataFromServerToClient.ChangeHpTree.Keys)
                        {
                            lock (Map)
                            {
                                Map[position].Health -= dataFromServerToClient.ChangeHpTree[position];
                                if(Map[position].Health == 0) forDelete.Add(position);
                            }
                        }
                        foreach (var point in forDelete) Map.Remove(point);
                    }
                }
            });
            task.Start();
            return task;
        }

        public async void WorkWithServer()
        {
            await MakeAsync();
        }

        /*public bool IsPlayerIntersected()
        {
            var playerPos = Player.Position;
            var leftDot = new Vector(playerPos.X - 17, playerPos.Y - 30).Rotate(Player.Direction);
            var rightDot = new Vector(playerPos.X + 28, playerPos.Y - 30).Rotate(Player.Direction);
            //var playerRect = new Rectangle(leftTop., rightTop, 1, 1);
            const int limit = 50;
            for (var dy = -limit; dy <= limit; dy++)
            for (var dx = -limit; dx <= limit; dx++)
            {
                var potentialPos = new Point(
                    playerPos.X + dx, playerPos.Y + dy);
                if (!Map.ContainsKey(potentialPos)) continue;
                var obj = Map[potentialPos];
                var objRectangle = obj.ObjRectangle;
                var top = objRectangle.Top;
                var bottom = objRectangle.Bottom;
                var left = objRectangle.Left;
                var right = objRectangle.Right;
                return IsIntersected(leftDot, rightDot, left, right, bottom, top);
            }

            return false;
        }*/

        public bool IsIntersected(Vector leftDot, Vector rightDot, int left, int right, int bottom, int top)
        {
            var a = Math.Max(leftDot.X, left) <= Math.Min(rightDot.X, right);
            var b = Math.Max(leftDot.Y, bottom) <= Math.Min(rightDot.Y, top);
            var c = Math.Max(rightDot.X, left) <= Math.Min(leftDot.Y, right);
            var d = Math.Max(rightDot.Y, bottom) <= Math.Min(leftDot.Y, top);
            //return a && b || c&&d || a&&c || a&&d||b&&c||b&&d;
            return a && b;
        }

        /*private double CalcLength(int point1, int point2)
        {
            return Math.Sqrt(point1 * point1 - point2 * point2);
        }*/

        private static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            var a = Math.Max(r1.Left, r2.Left) <= Math.Min(r1.Right, r2.Right);
            var b = Math.Max(r1.Top, r2.Top) <= Math.Min(r1.Bottom, r2.Bottom);
            return a && b;
        }


        public void Shoot(Bullet bullet)
        {
            lock (dataFromClientToServer)
            {
                dataFromClientToServer.NewBullets.Add(bullet);
            }
        }
    }
}