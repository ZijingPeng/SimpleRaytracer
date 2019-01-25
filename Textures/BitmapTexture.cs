using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace Raytracer.Textures
{
    class BitmapTexture : ITexture
    {
        public BitmapTexture(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            this.bitmap = new Color[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    this.bitmap[i, j] = Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                }
            }
        }

        public BitmapTexture(string imagePath) 
            : this((Bitmap)Image.FromFile(imagePath)) {}

        private readonly Color[,] bitmap;
        private readonly int Width;
        private readonly int Height;

        public Vector GetColor(float u, float v, Vector p)
        {
            var i = (int)(u * Width);
            var j = (int)((1 - v) * Height - 0.001f);
            // clampÎÆÀíÑ°Ö·
            if (i < 0)
            {
                i = 0;
            }
            else if (i > Width - 1)
            {
                i = Width - 1;
            }

            if (j < 0)
            {
                j = 0;
            }
            else if (j > Height - 1)
            {
                j = Height - 1;
            }
            var color = bitmap[i, j];
            return new Vector(color.R, color.G, color.B) / 255;
        }
    }
}