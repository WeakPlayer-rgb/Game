using System;
using Newtonsoft.Json;

namespace NewGame
{
    public class Vector
    {
        public static Vector Zero = new Vector(0, 0);

        public readonly double X;
        public readonly double Y;

        [JsonConstructor]
        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector v)
        {
            X = v.X;
            Y = v.Y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);
        public double Angle => Math.Atan2(Y, X);


        public static bool DoubleEquals(double a, double b)
        {
            return Math.Abs(a - b) < 1e-6;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        protected bool Equals(Vector other)
        {
            return DoubleEquals(X, other.X) && DoubleEquals(Y, other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double k)
        {
            return new(a.X * k, a.Y * k);
        }

        public static Vector operator /(Vector a, double k)
        {
            return new(a.X / k, a.Y / k);
        }

        public static Vector operator *(double k, Vector a)
        {
            return a * k;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public Vector Rotate(double angle)
        {
            return new(X * Math.Cos(angle) - Y * Math.Sin(angle), X * Math.Sin(angle) + Y * Math.Cos(angle));
        }
    }
}