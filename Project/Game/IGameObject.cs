namespace Game
{
    public interface IGameObject
    {
        string GetImage(string path);
        int DrawPriority(int priority);
        void Move();
    }
}