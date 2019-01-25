using Raytracer.Hitables;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    internal class Metal : IMaterial
    {
        private readonly ITexture albedo;
        private readonly float fuzz;

        public Metal(Vector v, float f)
        {
            albedo = new SolidTexture(v);
            fuzz = f < 1 ? f : 1;
        }
        
        public Metal(ITexture v, float f)
        {
            albedo = v;
            fuzz = f < 1 ? f : 1;
        }

        public bool Scatter(Ray ray, HitRecord record, ref Vector attenuation, ref Ray scattered)
        {
            var reflected = Reflect(ray.Direction.Normalize(), record.Normal);
            scattered = new Ray(record.P, reflected + fuzz * MathHelper.RandUnitVector(record.Normal));
            attenuation = albedo.GetColor(record.U, record.V, record.P);
            return scattered.Direction.Dot(record.Normal) > 0;
        }

        private static Vector Reflect(Vector v, Vector n)
        {
            return v - 2 * v.Dot(n) * n;
        }
    }
}