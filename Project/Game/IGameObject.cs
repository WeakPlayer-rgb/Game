namespace Game
{
    public interface IGameObject
    {
        int StartingPosX { get; set; }
        int StartingPosY { get; set; }
        int Speed { get; set; }
        int Health { get; set; }
        string GetImage(string path);
        int DrawPriority(int priority);
        void Move();
    }
}