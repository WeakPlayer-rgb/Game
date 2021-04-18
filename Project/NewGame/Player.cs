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
                    Direction = Direction.Rotate(-Math.PI / 20);
                    break;
                }
                case KeyButton.Right:
                {
                    Direction = Direction.Rotate(Math.PI / 20);
                    break;
                }
                case KeyButton.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }

        public void ChangeVelocity(KeyButton ctrl, Vector currentDir)
        {
            switch (ctrl)
            {
                case KeyButton.Backward:
                {
                    if (Direction.Length - 1 <= 0) Direction *= -3;
                    //if (Direction.X-1e-3<=0 || Direction.Y-1e-3<=0) Direction=Vector.Zero;
                    else Direction = 0.5 * currentDir;
                    break;
                }
                case KeyButton.Forward:
                {
                    if (Direction.Length < 10)
                    {
                        if (Equals(Direction, Vector.Zero)) Direction += new Vector(currentDir.X, currentDir.Y);
                        Direction *= 3;
                    }

                    break;
                }
                case KeyButton.None:
                    Direction *= 0.96;
                    if ((Direction - new Vector(0.5, 0.5)).Length < 0) Direction = Vector.Zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}