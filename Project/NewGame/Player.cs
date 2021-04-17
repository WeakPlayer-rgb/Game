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
                   Position = Direction.Rotate(Math.PI / 20);
                    
                   break;
                }
                case KeyButton.Right:
                {
                    Position = Direction.Rotate(Math.PI / 20);
                    Direction=Direction.Rotate(-Math.PI / 20);
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
                    Direction += new Vector(0, -1)*500;
                    Position += Direction.Rotate(Math.PI / 20);
                    break;
                }
                case KeyButton.Forward:
                {
                    Direction += new Vector(0, 1)*500;
                    Position += Direction.Rotate(Math.PI / 20);
                    break;
                }
                case KeyButton.None:
                    Position = Direction.Rotate(Math.PI / 20);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ctrl), ctrl, null);
            }
        }
    }
}