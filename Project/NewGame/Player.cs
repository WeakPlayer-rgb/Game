using System;
using System.Drawing;
using System.IO;

namespace NewGame
{
    public class Player : IGameObject
    {
        public Point Position { get; set; }
        private const int maxHealth = 100;
        public int Damage { get; set; }
        public int MaxHealth() => maxHealth;

        public Rectangle ObjRectangle => 
            new((int) Position.X, (int) Position.Y, 35, 65);

        public double Direction => angle;
        public Vector Speed => new Vector(1, 0).Rotate(Direction) * velocity;
        public double Health { get; set; }
        private double angle;
        private double velocity;
        public int CoolDown { get; set; }

        public Player(Point p)
        {
            Position = p;
            angle = 0;
            velocity = 0;
            Damage = 10;
        }

        public string GetImage() => "car1.png";

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
                    velocity = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}