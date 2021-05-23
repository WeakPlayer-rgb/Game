using System.Drawing;
using Newtonsoft.Json;

namespace NewGame
{
    public class Bullet
    {
        public Point Position { get; set; }
        public int Damage { get; }
        public int Tick { get; private set; }
        public float DirectionX { get; }
        public float DirectionY { get; }

        public Bullet(float directionX,float directionY, Point position,int damage)
        {
            DirectionX = directionX;
            DirectionY = directionY;
            Position = position;
            Damage = damage;
        }

        public void MoveThisBullet()
        {
            
            Position = new Point(Position.X + (int)DirectionX, Position.Y +  (int)DirectionY);
            ChangeTick();
        }

        private void ChangeTick() => Tick += 1;
    }
}