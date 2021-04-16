using System;

namespace Game
{
    public interface IGameObject
    {
        double StartingPosX { get; set; }
        double StartingPosY { get; set; }
        Vector Speed { get; set; }
        double Health { get; set; }
        string GetImage(string path);
        int DrawPriority(int priority);
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}