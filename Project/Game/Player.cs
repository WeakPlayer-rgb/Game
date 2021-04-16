using System;

namespace Game
{
    public class Player : IGameObject
    {
        public Vector position;
        public Vector Direction { get; set; }
        public double Health { get; set; }
        
        public string GetImage(string path) => "car.png";

        public int DrawPriority(int priority) => 0;
        public void ChangeDirection(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Left:
                {
                    Direction += new Vector(-1, 0);
                    break;
                }
                case KeyButton.Right:
                {
                    Direction += new Vector(1, 0);
                    break;
                }
            }
        }

        public void ChangeVelocity(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Backward:
                {
                    Direction += new Vector(0, -1);
                    break;
                }
                case KeyButton.Forward:
                {
                    Direction += new Vector(0, 1);
                    break;
                }
            }
        }
    }
}