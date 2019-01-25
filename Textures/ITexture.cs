namespace Raytracer.Textures {
    interface ITexture
    {
        Vector GetColor(float u, float v, Vector p);
    }
}