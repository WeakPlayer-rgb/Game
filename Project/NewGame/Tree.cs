using System.Drawing;

namespace NewGame
{
    public class Tree
    {
        public Rectangle ObjRectangle => new(_position.X, _position.Y, 50, 85);
        public int MaxHealth() => maxHealth;
        public double Health { get; }
        private const int maxHealth = 100;
        private Point _position;

        public Tree(Point position)
        {
            _position = position;
            Health = maxHealth;
        }

        public string GetImage() => "tree.png";
    }
}