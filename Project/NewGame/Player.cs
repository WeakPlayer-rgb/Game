using System;

namespace NewGame
{
    public class Player : IGameObject
    {
        public Vector Position { get; set; }
        public Vector Direction { get; set; }
        public double Health { get; set; }

        public Player(Vector p)
        {
            Direction = Vector.Zero;
            Position = p;
        }

        public string GetImage() => "car.png";

        public int DrawPriority(int priority) => 0;

        public void ChangeDirection(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Left:
                {
                    Direction = Direction.Rotate(Math.PI / 20);
                    break;
                }
                case KeyButton.Right:
                {
                    Direction = Direction.Rotate(-Math.PI / 20);
                    break;
                }
                case KeyButton.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }

        public void ChangeVelocity(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Backward:
                {
                    //if (Direction.X-1e-3<=0 || Direction.Y-1e-3<=0) Direction=Vector.Zero;
                    Direction *= 0.5;
                    break;
                }
                case KeyButton.Forward:
                {
                    Direction *= 3;
                    break;
                }
                case KeyButton.None:
                    Direction *= 0.95;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}