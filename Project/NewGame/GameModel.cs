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
        public Dictionary<Point, Bullet> Bullets;

        public GameModel(int s)
        {
            Size = s;
            Map = new Dictionary<Point, IGameObject>();
            Player = new Player(new Vector(Size / 2, Size / 2), 100);
            SpawnTrees();
            Bullets = new Dictionary<Point, Bullet>();
        }

        public void ChangePosition()
        {
            if (IsPlayerIntersected())
            {
                Player.ChangeVelocity(KeyButton.Break);
                return;
            }

            Player.Position += Player.Speed;
            var x = Player.Position.X;
            var y = Player.Position.Y;
            //if (Car.Position.X > size) Car.Position -= new Vector(size, 0);
            //if (Car.Position.Y > size) Car.Position -= new Vector(0, size);
            //if (Car.Position.X < 0) Car.Position += new Vector(size, 0);
            //if (Car.Position.Y < 0) Car.Position += new Vector(0, size);
            Player.Position += new Vector(x < 0 ? Size : x > Size ? -Size : 0,
                y < 0 ? Size : y > Size ? -Size : 0);
        }

        public bool IsPlayerIntersected()
        {
            var playerPos = Player.Position;
            var leftDot = new Vector(playerPos.X - 17, playerPos.Y - 30).Rotate(Player.Direction);
            var rightDot = new Vector(playerPos.X + 28, playerPos.Y - 30).Rotate(Player.Direction);
            //var playerRect = new Rectangle(leftTop., rightTop, 1, 1);
            const int limit = 50;
            for (var dy = -limit; dy <= limit; dy++)
            for (var dx = -limit; dx <= limit; dx++)
            {
                var potentialPos = new Point((int) playerPos.X + dx, (int) playerPos.Y + dy);
                if (!Map.ContainsKey(potentialPos)) continue;
                var obj = Map[potentialPos];
                var objRectangle = obj.ObjRectangle;
                var top = objRectangle.Top;
                var bottom = objRectangle.Bottom;
                var left = objRectangle.Left;
                var right = objRectangle.Right;
                return IsIntersected(leftDot, rightDot, left,right, bottom, top);
            }

            return false;
        }

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

        public void MoveBullets()
        {
            var newBullets = new Dictionary<Point, Bullet>();
            foreach (var point in Bullets.Keys)
            {
                newBullets[new Point(point.X + (int) Bullets[point].Direction.X,
                        point.Y + (int) Bullets[point].Direction.Y)] =
                    Bullets[point];
            }

            Bullets = newBullets;
        }

        public void AddBullet(Bullet bullet, Point position)
        {
            Bullets[position] = bullet;
        }

        private void SpawnTrees()
        {
            var rnd = new Random();
            for (var i = 0; i < 500; i++)
            {
                var newPoint = new Point(rnd.Next(0, Size / 32), rnd.Next(0, Size / 32)); // 40, 70
                Map[new Point(newPoint.X * 32, newPoint.Y * 32)] =
                    new Tree(new Point(newPoint.X * 32, newPoint.Y * 32));
            }
        }
    }
}