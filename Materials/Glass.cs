using System;
using Raytracer.Hitables;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    internal class Glass : IMaterial
    {
        private readonly ITexture albedo;
        private readonly float refractivity;

        public Glass(ITexture albedo, float refractivity)
        {
            this.refractivity = refractivity;
            this.albedo = albedo;
        }

        public Glass(float refractivity = 1.5f)
        {
            albedo = new SolidTexture(Vector.One);
            this.refractivity = refractivity;
        }

        public Glass(Vector albedo, float refractivity)
        {
            this.refractivity = refractivity;
            this.albedo = new SolidTexture(albedo);
        }

        public bool Scatter(Ray ray, HitRecord record, ref Vector attenuation, ref Ray scattered)
        {
            attenuation = albedo.GetColor(record.U, record.V, record.P);
            var refracted = Vector.Zero;
            Vector outwardNormal;
            float newRefractivity;
            float cosin;
            float possible;
            var reflected = Reflect(ray.Direction, record.Normal);

            if (ray.Direction.Dot(record.Normal) > 0f)
            {
                outwardNormal = record.Normal.Negative();
                newRefractivity = refractivity;
                cosin = ray.Direction.Normalize().Dot(record.Normal);
            }
            else
            {
                outwardNormal = record.Normal;
                newRefractivity = 1f / refractivity;
                cosin = -ray.Direction.Normalize().Dot(record.Normal);
            }

            if (Refract(ray.Direction, outwardNormal, newRefractivity, ref refracted))
            {
                possible = SchlickEquation(cosin, newRefractivity);
            }
            else
            {
                scattered = new Ray(record.P, reflected);
                possible = 1f;
            }

            // reflection or refraction
            if (MathHelper.Randf() < possible)
            {
                scattered = new Ray(record.P, reflected);
            }
            else
            {
                scattered = new Ray(record.P, refracted);
            }
            return true;
        }

        private bool Refract(Vector ray, Vector normal, float rate, ref Vector refracted)
        {
            var costheta = ray.Normalize().Dot(normal);
            var discriminant = 1 - rate * rate * (1 - costheta * costheta);
            if (discriminant > 0)
            {
                refracted = rate * (ray - normal * costheta) - normal * (float)Math.Sqrt(discriminant);
                return true;
            }
            return false;
        }

        public float SchlickEquation(float cos, float newRefractivity)
        {
            var r = (1 - newRefractivity) / (1 + newRefractivity);
            r = r * r;
            return (float)(r + (1 - r) * Math.Pow(1 - cos, 5));
        }

        private Vector Reflect(Vector v, Vector n)
        {
            return v - 2 * v.Dot(n) * n;
        }
    }
}