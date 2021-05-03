namespace NewGame
{
    public class Bullet
    {
        public int Tick { get; private set; }
        public Vector Direction { get; }

        public Bullet(Vector direction)
        {
            Direction = (direction/direction.Length*10);
        }

        public void ChangeTick() => Tick += 1;
    }
}