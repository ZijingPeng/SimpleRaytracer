using System;

namespace Raytracer.Textures
{
    class CheckerTexture : ITexture
    {
        private readonly ITexture odd;
        private readonly ITexture even;

        public CheckerTexture(ITexture odd, ITexture even)
        {
            this.odd = odd;
            this.even = even;
        }

        public Vector GetColor(float u, float v, Vector p)
        {
            var sin = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            return sin < 0
                ? odd.GetColor(u, v, p)
                : even.GetColor(u, v, p);
        }
    }
}
