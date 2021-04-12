namespace Game
{
    public interface IGameObject
    {
        int Speed { get; set; }
        int Health { get; set; }
        string GetImage(string path);
        int DrawPriority(int priority);
        void Move();
    }
}