using System;
using Raytracer.Materials;

namespace Raytracer.Hitables
{
    internal class Triangle : IHitable
    {
        public readonly IMaterial Material;
        public Vector Normal;
        public readonly Vector Point1;
        public readonly Vector Point2;
        public readonly Vector Point3;

        public Triangle(Vector point1, Vector point2, Vector point3, IMaterial material)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Material = material;
            Normal = (Point2 - Point1).Cross(Point3 - Point1).Normalize();
        }

        public bool Hit(Ray ray, float min, float max, ref HitRecord record)
        {
            if (Math.Abs(Normal.Dot(ray.Direction)) < float.Epsilon)
            {
                return false;
            }

            var newNormal = Normal;

            if (newNormal.Dot(ray.Direction) > 0)
            {
                newNormal = Normal.Negative();
            }

            var t = (Point2 - ray.Origin).Dot(Normal) / ray.Direction.Dot(Normal);
            var d = t * ray.Direction + ray.Origin;

            var ad = (Point1 - d).Length;
            var bd = (Point2 - d).Length;
            var cd = (Point3 - d).Length;

            var ab = (Point1 - Point2).Length;
            var bc = (Point2 - Point3).Length;
            var ca = (Point1 - Point3).Length;

            var s1 = Area(ad, bd, ab);
            var s2 = Area(ad, cd, ca);
            var s3 = Area(bd, cd, bc);
            var total = Area(ab, bc, ca);

            if (Math.Abs(s1 + s2 + s3 - total) <= 0.01f && t < max && t > min)
            {
                // if p is in triangle
                record.Distance = t;
                record.P = d;
                record.Normal = newNormal;
                record.Material = Material;

                var hmax = total / ab;
                var h = s1 / ab;
                record.U = (float)Math.Sqrt(ad * ad - h * h) / ab;
                record.V = h / hmax;

                return true;
            }
            return false;
        }

        private static float Area(float a, float b, float c)
        {
            var s = (a + b + c) / 2;
            return (float)Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }
    }
}