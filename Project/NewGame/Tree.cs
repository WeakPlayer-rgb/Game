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
        public Rectangle ObjRectangle => new((int) Position.X, (int) Position.Y, 20, 50);
        private Point Position { get;  }
        public double Direction { get; }
        private const int maxHealth = 100;
        public int MaxHealth() => maxHealth;
        public double Health { get; set; }

        public Tree(Point position)
        {
            Position = position;
            Health = maxHealth;
        }

        public string GetImage() => "tree.png";

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