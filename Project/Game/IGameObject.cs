namespace Game
{
    public interface IGameObject
    {
        string GetImage(string path);
        int DrawPrioritet(int priority);
        void Move();
    }
}