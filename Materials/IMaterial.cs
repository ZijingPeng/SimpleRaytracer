using Raytracer.Hitables;

namespace Raytracer.Materials
{
    public interface IMaterial
    {
        bool Scatter(Ray ray, HitRecord record, ref Vector attenuation, ref Ray scattered);
    }
}