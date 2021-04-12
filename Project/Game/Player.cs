namespace Game
{
    public class Player : IGameObject
    {
        public string GetImage(string path) => "car.png";

        public int DrawPriority(int priority) => 0;

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}