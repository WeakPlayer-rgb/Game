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
        public readonly int size;
        public Dictionary<Point,Bullet> Bullets;

        public GameModel(int s)
        {
            size = s;
            Map = new Dictionary<Point, IGameObject>();
            Player = new Player(new Vector(500, 500), 100);
            
            Player = new Player(new Vector(size/2, size/2));
            
            var rnd = new Random();
            for (var i = 0; i < 500; i++)
            {
                var newPoint = new Point(rnd.Next(0, size / 32), rnd.Next(0, size / 32)); // 40, 70
                Map[new Point(newPoint.X * 32, newPoint.Y * 32)] =
                    new Tree(new Point(newPoint.X * 32, newPoint.Y * 32));
            }
            Bullets = new Dictionary<Point, Bullet>();
        }

        public void ChangePosition()
        {
            Player.Position += Player.Speed;
            var x = Player.Position.X;
            var y = Player.Position.Y;
            //if (Car.Position.X > size) Car.Position -= new Vector(size, 0);
            //if (Car.Position.Y > size) Car.Position -= new Vector(0, size);
            //if (Car.Position.X < 0) Car.Position += new Vector(size, 0);
            //if (Car.Position.Y < 0) Car.Position += new Vector(0, size);
            Player.Position += new Vector(x < 0 ? size : x > size ? -size : 0,
                y < 0 ? size : y > size ? -size : 0);
            foreach (var key in Map.Keys)
            {
                // three vectors in all points of the rectangle
                if (AreIntersected(Player.ObjRectangle, Map[key].ObjRectangle))
                {
                    Player.ChangeVelocity(KeyButton.Break);
                    break;
                }
            }
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

        public void AddBullet(Bullet bullet, Point position)
        {
            Bullets[position] = bullet;
        }
    }
}