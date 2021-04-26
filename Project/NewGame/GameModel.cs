using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NewGame
{
    public class GameModel
    {
        public readonly Player Car;
        public Dictionary<Point, IGameObject> Map;
        public readonly int size;

        public GameModel(int s)
        {
            size = s;
            Map = new Dictionary<Point, IGameObject>();
            Car = new Player(new Vector(500, 500));
            var rnd = new Random();
            for (var i = 0; i < 3000; i++)
            {
                var newPoint = new Point(rnd.Next(0, size / 32), rnd.Next(0, size / 32));
                Map[new Point(newPoint.X*32,newPoint.Y*32)] = new Tree();
            }
            Map[new Point(0, 0)] = new Tree();
        }

        public void ChangePosition()
        {
            Car.Position += Car.Speed;
            var x = Car.Position.X;
            var y = Car.Position.Y;
            //if (Car.Position.X > size) Car.Position -= new Vector(size, 0);
            //if (Car.Position.Y > size) Car.Position -= new Vector(0, size);
            //if (Car.Position.X < 0) Car.Position += new Vector(size, 0);
            //if (Car.Position.Y < 0) Car.Position += new Vector(0, size);
            Car.Position += new Vector(x < 0 ? size : x > size ? -size : 0,
                y < 0 ? size : y > size ? -size : 0);
        }
    }
}