namespace Game
{
    public class Car
    {
        public readonly double Direction;

        public readonly Vector Location;
        public readonly int TakenCheckpointsCount;
        public readonly int Time;
        public readonly Vector Velocity;

        public Car(Vector location, Vector velocity, double direction, int time = 0, int takenCheckpointsCount = 0)
        {
            Location = location;
            Velocity = velocity;
            Direction = direction;
            Time = time;
            TakenCheckpointsCount = takenCheckpointsCount;
        }

        public Vector GetNextRocketCheckpoint(Level level)
        {
            return level.Checkpoints[TakenCheckpointsCount % level.Checkpoints.Length];
        }

        public bool IsCompleted(Level level)
        {
            return Time >= level.MaxTicksCount;
        }

        public Car Move(Turn turn, Level level)
        {
            if (IsCompleted(level)) return this;
            var nextCheckpoint = TakenCheckpointsCount % level.Checkpoints.Length;
            var rocket = this;
            if (nextCheckpoint != level.Checkpoints.Length
                && (Location - level.Checkpoints[nextCheckpoint]).Length < 20)
                rocket = IncreaseCheckpoints();
            return level.Physics.MoveRocket(rocket, 1.0, turn, 0.5);
        }

        public Car IncreaseCheckpoints()
        {
            return new Car(Location, Velocity, Direction, Time, TakenCheckpointsCount + 1);
        }

        protected bool Equals(Car rocket)
        {
            return Location.Equals(rocket.Location) && Velocity.Equals(rocket.Velocity) &&
                   Vector.DoubleEquals(Direction, rocket.Direction) && Time == rocket.Time &&
                   TakenCheckpointsCount == rocket.TakenCheckpointsCount;
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
                hashCode = (hashCode * 397) ^ TakenCheckpointsCount;
                hashCode = (hashCode * 397) ^ Time;
                hashCode = (hashCode * 397) ^ Velocity.GetHashCode();
                return hashCode;
            }
        }
    }
}