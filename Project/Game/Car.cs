namespace Game
{
    public class Car
    {
        public readonly double Direction;
        public readonly Vector Location;
        public readonly int TakenResources;
        public readonly int Time;
        public readonly Vector Velocity;

        public Car(Vector location, Vector velocity, double direction, int time = 0, int takenResources = 0)
        {
            Location = location;
            Velocity = velocity;
            Direction = direction;
            Time = time;
            TakenResources = takenResources;
        }

        public Vector GetNextCarCheckpoint(Level level)
        {
            return level.Checkpoints[TakenResources % level.Checkpoints.Length];
        }

        public bool IsCompleted(Level level)
        {
            return Time >= level.MaxTicksCount;
        }

        public Car Move(Turn turn, Level level)
        {
            if (IsCompleted(level)) return this;
            var nextCheckpoint = TakenResources % level.Checkpoints.Length;
            var car = this;
            if (nextCheckpoint != level.Checkpoints.Length
                && (Location - level.Checkpoints[nextCheckpoint]).Length < 20)
                car = IncreaseCheckpoints();
            return level.Physics.MoveCar(car, 1.0, turn, 0.5);
        }

        public Car IncreaseCheckpoints()
        {
            return new Car(Location, Velocity, Direction, Time, TakenResources + 1);
        }

        protected bool Equals(Car car)
        {
            return Location.Equals(car.Location) && Velocity.Equals(car.Velocity) &&
                   Vector.DoubleEquals(Direction, car.Direction) && Time == car.Time &&
                   TakenResources == car.TakenResources;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Car) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = (hashCode * 397) ^ Location.GetHashCode();
                hashCode = (hashCode * 397) ^ TakenResources;
                hashCode = (hashCode * 397) ^ Time;
                hashCode = (hashCode * 397) ^ Velocity.GetHashCode();
                return hashCode;
            }
        }
    }
}