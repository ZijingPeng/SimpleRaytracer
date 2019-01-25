using Raytracer.Materials;

namespace Raytracer.Hitables
{
    public class HitRecord
    {
        public float Distance;
        public float U;
        public float V;
        public Vector P;
        public Vector Normal;
        public IMaterial Material;
    }
}