using System.Windows.Forms;

namespace NewGame
{
    public class Game
    {
        public readonly Player Car;
        public IGameObject[,] Map;
        private readonly int size;

        public Game(int s)
        {
            size = s;
            Map = new IGameObject[size, size];
            Car = new Player(new Vector(size/2,size/2));
        }
    }
}