namespace Game
{
    public class Game
    {
        private IGameObject[,] map;
        private readonly int Size;
        private Vector playerPosition;

        public Game(int s)
        {
            Size = s;
            map = new IGameObject[Size, Size];
            playerPosition = Vector.Zero;
        }

        void ChangePlayerPosition(Vector move) => playerPosition += move;
        
        
    }
}