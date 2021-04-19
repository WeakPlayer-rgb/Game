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
            //Direction =  
        }

        public string GetImage() => "car.png";

        public int DrawPriority(int priority) => 0;

        public void ChangeDirection(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Left:
                {
                    //Direction += Direction.Rotate(-Math.PI / 30);
                    angle -= Math.PI / 30;
                    //Direction.Angle += angle;
                    //Direction += angle;
                    break;
                }
                case KeyButton.Right:
                {
                    angle += Math.PI / 30;
                    //Direction = Direction.Rotate(Math.PI / 30);
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
                    if (velocity - 1 <= 0)
                    {
                        velocity = 0;
                    }

                    if (velocity < 0)
                    {
                        velocity *= 3;
                    }

                    if (velocity <= -10) velocity = -10;
                    if (velocity > 0) velocity *= 0.6;
                    //if (Direction.X-1e-3<=0 || Direction.Y-1e-3<=0) Direction=Vector.Zero;
                    
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

                    if (velocity > 0 && velocity < 10)
                    {
                        velocity *= 3;
                    }

                    if (velocity >= 10) velocity = 10;
                    break;
                }
                case KeyButton.None:
                    velocity *= 0.4;
                    if (velocity < 1) velocity = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}