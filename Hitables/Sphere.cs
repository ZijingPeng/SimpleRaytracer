using System;
using Raytracer.Materials;

namespace Raytracer.Hitables
{
    internal class Sphere : IHitable
    {
        public Vector Center;
        public readonly IMaterial Material;
        public readonly float Radius;

        public Sphere(Vector center, float radius, IMaterial material)
        {
            Center = center;
            Radius = radius;
            Material = material;
        }

        public bool Hit(Ray ray, float min, float max, ref HitRecord record)
        {
            var oc = ray.Origin - Center;
            var a = ray.Direction.Dot(ray.Direction);
            var b = oc.Dot(ray.Direction);
            var c = oc.Dot(oc) - Radius * Radius;
            var delta = b * b - a * c;
            
            // if does not hit 
            if (delta <= 0)
            {
                return false;
            }
            // if hits
            var temp = (float)(-b - Math.Sqrt(b * b - a * c)) / a;
            if (temp < max && temp > min)
            {
                record.Distance = temp;
                record.P = ray.PointAt(record.Distance);
                record.Normal = (record.P - Center) / Radius;
                record.Material = Material;
                SetSphereUv(record);
                return true;
            }

            temp = (float)(-b + Math.Sqrt(b * b - a * c)) / a;
            if (temp < max && temp > min)
            {
                record.Distance = temp;
                record.P = ray.PointAt(record.Distance);
                record.Normal = (record.P - Center) / Radius;
                record.Material = Material;
                SetSphereUv(record);
                return true;
            }
            return false;
        }

        private void SetSphereUv(HitRecord record)
        {
            var p = (record.P - Center) / Radius;
            var phi = Math.Atan2(p.Z, p.X);
            var theta = Math.Asin(p.Y);
            record.U = (float) (1 - (phi + Math.PI) / (2 * Math.PI));
            record.V = (float) ((theta + Math.PI / 2) / Math.PI);
        }
    }
}