using System.Drawing;
using System.Numerics;

namespace Raytracer
{
    public struct Vector
    {
        public Vector(float x, float y, float z)
        {
            value = new Vector3(x, y, z);
        }

        public Vector(float value)
        {
            this.value = new Vector3(value);
        }

        public Vector(Vector3 vec)
        {
            value = vec;
        }

        public float Length
        {
            get { return value.Length(); }
        }

        public float LengthSqr
        {
            get { return value.LengthSquared(); }
        }

        public static Vector One
        {
            get { return new Vector(1); }
        }

        public static Vector UnitX
        {
            get { return new Vector(1, 0, 0); }
        }

        public static Vector UnitY
        {
            get { return new Vector(0, 1, 0); }
        }

        public static Vector UnitZ
        {
            get { return new Vector(0, 0, 1); }
        }


        public float X
        {
            get { return value.X; }
            set { this.value = new Vector3(value, Y, Z); }
        }

        public float Y
        {
            get { return value.Y; }
            set { this.value = new Vector3(X, value, Z); }
        }

        public float Z
        {
            get { return value.Z; }
            set { this.value = new Vector3(X, Y, value); }
        }

        public static Vector Zero
        {
            get { return new Vector(0); }
        }

        private Vector3 value;

        public static implicit operator Point(Vector v)
        {
            return new Point((int) v.X, (int) v.Y);
        }

        public static implicit operator PointF(Vector v)
        {
            return new PointF(v.X, v.Y);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.value);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.value - b.value);
        }

        public static Vector operator *(Vector a, float factor)
        {
            return new Vector(Vector3.Multiply(a.value, factor));
        }

        public static Vector operator *(float factor, Vector a)
        {
            return new Vector(Vector3.Multiply(factor, a.value));
        }

        public static Vector operator /(Vector a, float factor)
        {
            return new Vector(Vector3.Divide(a.value, factor));
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.value + b.value);
        }

        public static implicit operator Color(Vector a)
        {
            return Color.FromArgb((int) (255.99f * a.X), (int) (255.99f * a.Y), (int) (255.99f * a.Z));
        }

        public Vector Cross(Vector v)
        {
            return new Vector(Vector3.Cross(value, v.value));
        }

        public float Dot(Vector v)
        {
            return Vector3.Dot(value, v.value);
        }

        public Vector Interpolate(Vector v, float factor)
        {
            return new Vector(Vector3.Lerp(value, v.value, factor));
        }

        public Vector Normalize()
        {
            return new Vector(Vector3.Normalize(value));
        }

        public Vector Scale(Vector v)
        {
            return new Vector(X * v.X, Y * v.Y, Z * v.Z);
        }

        public Vector Negative()
        {
            return new Vector(-X, -Y, -Z);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public Vector Sqrt()
        {
            return new Vector(Vector3.SquareRoot(value));
        }
    }
}