namespace Game
{
    public class Player : IGameObject
    {
        public string GetImage(string path) => "car.png";

        public int DrawPrioritet(int priority) => 0;

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}