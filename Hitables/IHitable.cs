namespace Raytracer.Hitables
{
    public interface IHitable
    {
        bool Hit(Ray ray, float min, float max, ref HitRecord record);
    }
}