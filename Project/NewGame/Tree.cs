using System;
using System.Drawing;

namespace NewGame
{
    class Tree : IGameObject
    {
        public Rectangle ObjRectangle => new(Position.X, Position.Y, 50, 85);
        private Point Position { get;  }
        public double Direction { get; }
        public int MaxHealth() => maxHealth;
        public double Health { get; set; }
        private const int maxHealth = 100;

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