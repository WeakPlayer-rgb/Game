namespace Game
{
    public class Game
    {
        public Player Car;
        public IGameObject[,] Map;
        private readonly int size;

        public Game(int s)
        {
            size = s;
            Map = new IGameObject[size, size];
            Car = new Player();
        }

        public void ChangePlayerPosition() => Car.position += Car.Direction;
    }
}