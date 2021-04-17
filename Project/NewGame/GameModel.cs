using System.Windows.Forms;

namespace NewGame
{
    public class GameModel
    {
        public readonly Player Car;
        public IGameObject[,] Map;
        private readonly int size;

        public GameModel(int s)
        {
            size = s;
            Map = new IGameObject[size, size];
            Car = new Player(new Vector(size/2,size/2));
            Map[(int) Car.Position.X, (int) Car.Position.Y] = Car;
        }

        public void ChangePosition() => Car.Position += Car.Direction;
    }
}