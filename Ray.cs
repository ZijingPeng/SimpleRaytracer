namespace Raytracer
{
    public class Ray
    {
        public Ray()
        {
            Origin = Vector.Zero;
            Direction = Vector.Zero;
        }

        public Ray(Vector origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector Origin { get; }
        public Vector Direction { get; }

        public Vector PointAt(float t)
        {
            return Origin + Direction * t;
        }
    }
}