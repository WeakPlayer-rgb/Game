namespace Game
{
    public class Physics
    {
        private readonly double mass;
        private readonly double maxTurnRate;

        public Physics() : this(1.0, 0.25)
        {
        }

        public Physics(double mass, double maxTurnRate)
        {
            this.mass = mass;
            this.maxTurnRate = maxTurnRate;
        }

        public Car MoveCar(Car car, double forceValue, Turn turn, double dt)
        {
            var turnRate = turn == Turn.Left ? -maxTurnRate : turn == Turn.Right ? maxTurnRate : 0;
            var dir = car.Direction + turnRate * dt;
            var force = new Vector(forceValue, 0).Rotate(car.Direction);
            var velocity = car.Velocity + force * dt / mass;
            var location = car.Location + velocity * dt;
            velocity = velocity * (1 - 0.01 * dt);
            return new Car(location, velocity, dir, car.Time + 1, car.TakenResources);
        }
    }
}