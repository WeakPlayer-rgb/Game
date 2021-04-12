namespace Game
{
    public class Player : IGameObject
    {
        public int StartingPosX { get; set; }
        public int StartingPosY { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public string GetImage(string path) => "car.png";

        public int DrawPriority(int priority) => 0;

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}