namespace Game
{
    public interface IGameObject
    {
        Vector Speed { get; set; }
        double Health { get; set; }
        string GetImage(string path);
        int DrawPriority(int priority);
        void ChangeDirection(KeyButton ctrl);
    }
}