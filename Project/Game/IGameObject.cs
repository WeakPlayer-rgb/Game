﻿namespace Game
{
    public interface IGameObject
    {
        Vector Direction { get; set; }
        double Health { get; set; }
        string GetImage(string path);
        int DrawPriority(int priority);
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}