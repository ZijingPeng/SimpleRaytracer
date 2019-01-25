using Raytracer.Hitables;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    internal class Lambertian : IMaterial
    {
        private readonly ITexture albedo;

        public Lambertian(Vector v)
        {
            albedo = new SolidTexture(v);
        }

        public Lambertian(ITexture texture)
        {
            albedo = texture;
        }

        public bool Scatter(Ray ray, HitRecord record, ref Vector attenuation, ref Ray scattered)
        {
            var reflected = record.Normal + record.P + MathHelper.RandUnitVector(record.Normal);
            attenuation = albedo.GetColor(record.U, record.V, record.P);
            scattered = new Ray(record.P, reflected - record.P);
            return true;
        }
    }
}