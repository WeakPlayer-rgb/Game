﻿using System.Collections;
using System.Drawing;

namespace NewGame
{
    public interface IGameObject
    {
        Vector Direction { get;  }
        double Health { get; set; }
        Bitmap GetImage();
        int DrawPriority(int priority);
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}