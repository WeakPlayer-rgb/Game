using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NewGame
{
    public class GameModel
    {
        public readonly Player Player;
        public Dictionary<Point, IGameObject> Map;
        public readonly int Size;
        public readonly List<Bullet> Bullets;

        public GameModel(int s)
        {
            Size = s;
            Map = new Dictionary<Point, IGameObject>();

            Player = new Player(new Point(Size / 2, Size / 2));

            var rnd = new Random();
            for (var i = 0; i < 500; i++)
            {
                var newPoint = new Point(rnd.Next(0, Size / 32), rnd.Next(0, Size / 32)); // 40, 70
                if (!Map.ContainsKey(newPoint))
                    Map[new Point(newPoint.X * 32, newPoint.Y * 32)] =
                        new Tree(new Point(newPoint.X * 32, newPoint.Y * 32));
            }

            Bullets = new List<Bullet>();
        }

        public void ChangePosition()
        {
            Player.Position = new Point((int) Player.Speed.X + Player.Position.X,
                (int) Player.Speed.Y + Player.Position.Y);
            var dx = Player.Position.X < 0 ? Size : Player.Position.X > Size ? -Size : 0;
            var x = Player.Position.X + dx;
            var dy = Player.Position.Y < 0 ? Size : Player.Position.Y > Size ? -Size : 0;
            var y = Player.Position.Y + dy;
            Player.Position = new Point(x, y);
            // foreach (var key in Map.Keys)
            // {
            //     if (AreIntersected(Player.ObjRectangle, Map[key].ObjRectangle)) Player.ChangeVelocity(KeyButton.Break);
            // }
        }

        public void MoveBullets()
        {
            var forRemoveBullets = new List<Bullet>();
            var forRemoveGameObject = new List<Point>();
            foreach (var bullet in Bullets)
            {
                bullet.Position = new Point(bullet.Position.X + (int) bullet.Direction.X,
                    bullet.Position.Y + (int) bullet.Direction.Y);
                var point = new Point(bullet.Position.X / 32 * 32, bullet.Position.Y / 32 * 32);
                if (Map.ContainsKey(point))
                {
                    Map[point].Health -= bullet.Damage;
                    forRemoveBullets.Add(bullet);
                    if(Map[point].Health <=0)
                        forRemoveGameObject.Add(point);
                }
                else if (Map.ContainsKey(new Point(point.X, point.Y - 32)))
                {
                    Map[new Point(point.X, point.Y - 32)].Health -= bullet.Damage;
                    forRemoveBullets.Add(bullet);
                    if(Map[new Point(point.X, point.Y - 32)].Health <=0)
                        forRemoveGameObject.Add(new Point(point.X, point.Y - 32));
                }
            }

            foreach (var bullet in forRemoveBullets) Bullets.Remove(bullet);
            foreach (var point in forRemoveGameObject)
            {
                Map.Remove(point);
                Player.Damage += 10;
            }
        }

        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            var a = Math.Max(r1.Left, r2.Left) <= Math.Min(r1.Right, r2.Right);
            var b = Math.Max(r1.Top, r2.Top) <= Math.Min(r1.Bottom, r2.Bottom);
            return a && b;
        }

        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (!AreIntersected(r1, r2))
                return 0;
            var a = Math.Max(r1.Left, r2.Left) - Math.Min(r1.Right, r2.Right);
            var b = Math.Max(r1.Top, r2.Top) - Math.Min(r1.Bottom, r2.Bottom);
            return a * b;
        }

        public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            if (r1.Bottom <= r2.Bottom && r1.Top >= r2.Top && r1.Left >= r2.Left && r1.Right <= r2.Right)
                return 0;
            if (r2.Bottom <= r1.Bottom && r2.Top >= r1.Top && r2.Left >= r1.Left && r2.Right <= r1.Right)
                return 1;
            return -1;
        }

        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }
    }
}