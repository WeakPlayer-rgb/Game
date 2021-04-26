using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGame
{
    class Tree : IGameObject
    {
        public Rectangle ObjRectangle => new((int)Position.X, (int)Position.Y, 40, 50);
        public Vector Position { get; }
        public double Direction { get; }
        public double Health { get; set; }

        public Tree(Point position)
        {
            Position = new Vector(position.X, position.Y);
        }
        public Image GetImage() => Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Images", "tree.png"));

        public int DrawPriority(int priority) => 0;

        public void ChangeDirection(KeyButton ctrl)
        {
            throw new NotImplementedException();
        }

        public void ChangeVelocity(KeyButton ctrl)
        {
            throw new NotImplementedException();
        }
    }
}