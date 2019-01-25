using System;
using System.Threading;

namespace Raytracer
{
    public static class StaticRandom
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        // generate a random float
        public static float Randf()
        {
            return (float) Random.Value.NextDouble();
        }

        public static float RandfUniform()
        {
            var buffer = new byte[4];
            Random.Value.NextBytes(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }

        public static float RandfUniform1()
        {
            var mantissa = Random.Value.NextDouble() * 2.0 - 1.0;
            var exponent = Math.Pow(2.0, Random.Value.Next(-126, 128));
            return (float) (mantissa * exponent);
        }
    }

    public static class MathHelper
    {
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            return value.CompareTo(max) > 0 ? max : value;
        }

        public static Vector RandUnitVector(Vector normal)
        {
            Vector p;
            do
            {
                p = new Vector(2 * Randf() - 1, 2 * Randf() - 1, 2 * Randf() - 1);
            } while (normal.Dot(p) <= 0);
            return p;
        }

        public static float Randf()
        {
            return StaticRandom.Randf();
        }

        public static Vector RotateY(Vector point, Vector center, float theta)
        {
            var x = point.X - center.X;
            var z = point.Z - center.Z;
            var cos = (float) Math.Cos(theta);
            var sin = (float) Math.Sin(theta);

            return new Vector(
                x * cos + z * sin + center.X,
                point.Y,
                -x * sin + z * cos + center.Z
            );
        }
    }
}