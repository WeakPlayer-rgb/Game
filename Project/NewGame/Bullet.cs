using System.Drawing;

namespace NewGame
{
    public class Bullet
    {
        public Point Position { get; set; }
        public int Damage { get; }
        public int Tick { get; private set; }
        public Vector Direction { get; }

        public Bullet(Vector direction, Point position,int damage)
        {
            Direction = direction / direction.Length * 10;
            Position = position;
            Damage = damage;
        }

        public void MoveThisBullet()
        {
            Position = new Point(Position.X + (int) Direction.X, Position.Y + (int) Direction.Y);
        }

        public void ChangeTick() => Tick += 1;
    }
}