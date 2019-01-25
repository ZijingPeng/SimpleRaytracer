namespace Raytracer.Textures
{
    class SolidTexture : ITexture
    {
        public SolidTexture(Vector color)
        {
            this.color = color;
        }

        private readonly Vector color;

        public Vector GetColor(float u, float v, Vector p)
        {
            return color;
        }
    }
}
