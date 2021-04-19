using System;

namespace NewGame
{
    public class Player : IGameObject
    {
        public Vector Position { get; set; }
        public Vector Direction => new Vector(1, 0).Rotate(angle);
        public Vector Speed => new Vector(Direction.X, Direction.Y) * velocity;
        public double Health { get; set; }
        private double angle;
        private double velocity;

        public Player(Vector p)
        {
            Position = p;
            angle = 0;
            velocity = Direction.Length;
        }

        public string GetImage() => "car.png";

        public int DrawPriority(int priority) => 0;

        public void ChangeDirection(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Left:
                {
                    angle -= Math.PI / 30;
                    break;
                }
                case KeyButton.Right:
                {
                    angle += Math.PI / 30;
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
                    if (velocity == 0)
                    {
                        velocity -= 1;
                    }
                    else if (velocity is < 1 and > 0)
                    {
                        velocity = 0;
                    }
                    else if (velocity < 0)
                    {
                        velocity *= 3;
                    }
                    else if (velocity > 0) velocity *= 0.6;
                    if (velocity <= -10) velocity = -10;

                    break;
                }
                case KeyButton.Forward:
                {
                    if (velocity == 0)
                    {
                        velocity += 1;
                    }
                    else if (velocity + 1 <= 0)
                    {
                        velocity = 0;
                    }

                    else
                        switch (velocity)
                        {
                            case > 0 and < 10:
                                velocity *= 3;
                                break;
                            case >= 10:
                                velocity = 10;
                                break;
                        }

                    break;
                }
                case KeyButton.None:
                    velocity *= 0.95;
                    if (velocity is < 1 and > -1) velocity = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}