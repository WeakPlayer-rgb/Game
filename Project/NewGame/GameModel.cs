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
            if (IsPlayerIntersected())
            {
                var slowDownMultiplier = Player.Speed.X/2;
                ChangePos(slowDownMultiplier);
            }

            ChangePos();
            // foreach (var key in Map.Keys)
            // {
            //     if (AreIntersected(Player.ObjRectangle, Map[key].ObjRectangle)) Player.ChangeVelocity(KeyButton.Break);
            // }
        }

        private void ChangePos(double multiplier=0)
        {
            Player.Position = new Point((int) ((int) Player.Speed.X-multiplier + Player.Position.X),
                (int) ((int) Player.Speed.Y + Player.Position.Y));
            var dx = Player.Position.X < 0 ? Size : Player.Position.X > Size ? -Size : 0;
            var x = Player.Position.X + dx;
            var dy = Player.Position.Y < 0 ? Size : Player.Position.Y > Size ? -Size : 0;
            var y = Player.Position.Y + dy;
            Player.Position = new Point(x, y);
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
            }

            return false;
        }

        /*public bool IsIntersected(Vector leftDot, Vector rightDot, int left, int right, int bottom, int top)
        {
            var a = Math.Max(leftDot.X, left) <= Math.Min(rightDot.X, right);
            var b = Math.Max(leftDot.Y, bottom) <= Math.Min(rightDot.Y, top);
            var c = Math.Max(rightDot.X, left) <= Math.Min(leftDot.Y, right);
            var d = Math.Max(rightDot.Y, bottom) <= Math.Min(leftDot.Y, top);
            //return a && b || c&&d || a&&c || a&&d||b&&c||b&&d;
            return a && b;
        }*/

        /*private double CalcLength(int point1, int point2)
        {
            return Math.Sqrt(point1 * point1 - point2 * point2);
        }*/

        private bool isIntersected(IGameObject toCheck, IGameObject potentiallyIntersecting)
        {
            return false;
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