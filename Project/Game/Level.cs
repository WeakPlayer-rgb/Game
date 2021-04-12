using System.Drawing;

namespace Game
{
    public class Level
    {
        public readonly Vector[] Checkpoints;
        public readonly Car InitialCar;
        public readonly int MaxTicksCount = 1000;
        public readonly Physics Physics;

        public readonly Size SpaceSize = new Size(800, 600);

        public Level(Car car, Vector[] checkpoints, Physics physics)
        {
            InitialCar = car;
            Checkpoints = checkpoints;
            Physics = physics;
        }

        public Level Clone()
        {
            return new Level(InitialCar, Checkpoints, Physics);
        }
    }
}