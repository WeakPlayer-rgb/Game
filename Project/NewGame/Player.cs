using System;
using System.Drawing;
using System.IO;

namespace NewGame
{
    public class Player
    {
        public Point Position { get; set; }
        public int Damage { get; set; }
        public int MaxHealth => maxHealth;

        public Rectangle ObjRectangle =>
            new(Position.X, Position.Y, 45, 80);

        public float Direction { get; set; }
        public Vector Speed => new Vector(1, 0).Rotate(Direction) * velocity;
        public int CoolDown { get; set; }

        public int Health { get; set; }
        public int Score { get; set; }

        private double velocity;
        private const int maxHealth = 100;

        public Player(Point p, float angle)
        {
            Position = p;
            Direction = angle;
            velocity = 0;
            Damage = 10;
        }

        public string GetImage() => "car1.png";

        public void ChangeDirection(KeyButton ctrl)
        {
            switch (ctrl)
            {
                case KeyButton.Left:
                {
                    Direction -= (float) (Math.PI / 30);
                    break;
                }
                case KeyButton.Right:
                {
                    Direction += (float) (Math.PI / 30);
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
                    switch (velocity)
                    {
                        case 0:
                            velocity -= 1;
                            break;
                        case < 0.25 and > 0:
                            velocity = 0;
                            break;
                        case < 0:
                            velocity *= 1.05;
                            break;
                        case > 0:
                            velocity *= 0.6;
                            break;
                    }

                    if (velocity <= -5) velocity = -5;
                    break;
                }
                case KeyButton.Forward:
                {
                    switch (velocity)
                    {
                        case 0:
                            velocity += 1;
                            break;
                        case <= -0.25:
                            velocity = 0;
                            break;
                    }

                    if (velocity is > 0 and < 8) velocity *= 1.07;
                    if (velocity >= 40) velocity = 40;
                    break;
                }
                case KeyButton.None:
                    velocity *= 0.97;
                    if (velocity is < 0.25 and > -0.25) velocity = 0;
                    break;
                case KeyButton.Break:
                    velocity = 0.9;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }

        public override string ToString() => Position.ToString();
    }
}