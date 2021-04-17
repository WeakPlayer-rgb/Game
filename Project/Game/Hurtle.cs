namespace Game
{
    public class Hurtle : IGameObject
    {
        public double StartingPosX { get; set; }
        public double StartingPosY { get; set; }

        public Vector Speed { get; set; }
        public double Health { get; set; }

        public string GetImage()
        {
            throw new System.NotImplementedException();
        }

        public int DrawPriority(int priority)
        {
            return 0;
        }

        public void ChangeDirection(KeyButton ctrl)
        {
            StartingPosX = StartingPosX;
            StartingPosY = StartingPosY;
        }

        public void ChangeVelocity(KeyButton ctrl)
        {
            //throw new System.NotImplementedException();
        }
    }
}