using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raytracer.Hitables;
using Raytracer.Materials;

namespace Raytracer
{
    internal static class Program
    {
        private const int Width = 400;
        private const int Height = 200;
        private const int SampleCount = 1;
        private const int ReflectionCount = 50; // max reflection times

        private static readonly HitableList World = new HitableList(Scenes.CurrentWorld);

        public static readonly PictureBox PictureBox = new PictureBox
        {
            Dock = DockStyle.Fill
        };

        // shader
        private static Vector Shade(Ray ray, int n)
        {
            var record = new HitRecord();
            if (World.Hit(ray, 0.01f, float.MaxValue, ref record))
            {
                var scattered = new Ray();
                var attenuation = new Vector();
                if (n > 0 && record.Material.Scatter(ray, record, ref attenuation, ref scattered))
                {
                    return Shade(scattered, n - 1).Scale(attenuation);
                }
                return Vector.Zero;
            }
            var u = ray.Direction.Normalize();
            var t = 0.5f * (u.Y + 1);
            return new Vector(1, 1, 1) * (1f - t) + new Vector(0.5f, 0.7f, 1.0f) * t;
        }

        private static Task<Bitmap> Draw()
        {
            var bitmap = new Bitmap(Width, Height);
            var tasks = new List<Task>();
            var colors = new Color[Width, Height];
            for (var xCounter = 0; xCounter < Width; xCounter++)
            {
                var x = xCounter;
                var task = Task.Run(() =>
                {
                    for (var yCounter = 0; yCounter < Height; yCounter++)
                    {
                        var y = yCounter;
                        var color = new Vector(0);
                        for (var k = 0; k < SampleCount; k++)
                        {
                            var u = (x + MathHelper.Randf()) / Width;
                            var v = (y + MathHelper.Randf()) / Height;
                            var ray = Camera.GetRay(u, v);
                            color += Shade(ray, ReflectionCount);
                        }
                        color /= SampleCount;
                        colors[x, y] = color.Sqrt();
                    }
                });
                tasks.Add(task);
            }
            return Task.WhenAll(tasks).ContinueWith(task =>
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        bitmap.SetPixel(x, Height - y - 1, colors[x, y]);
                    }
                }
                return bitmap;
            });
        }

        private static readonly Camera Camera = new Camera(
            new Vector(11F, 1, 0),
            new Vector(0.1f, 1, 0f),
            new Vector(0f, 2f, 0f),
            60,
            Width / (float)Height
        );
        //private static readonly Camera Camera = new Camera(
        //    new Vector(0, 2, 6),
        //    new Vector(0.1f, 1, 0f),
        //    new Vector(0f, 2f, 0f),
        //    60,
        //    Width / (float)Height
        //);
        private static int count = 100;
        private static Vector to = new Vector(5, 1, 0);
        private static int state = 0;
        private static Vector step = (to - Camera.Lookfrom) / count;
        private const float radius = 4.9f;

        private static void Update()
        {
            if (state < 100)
            {
                // translate
                Camera.Lookfrom += step;
            }
            else
            {
                // rotate
                var t = (float)Math.PI / 180 * (state - 100);
                var x = (float)Math.Cos(t) * radius + 0.1f;
                var z = (float)Math.Sin(t) * radius + 1;
                Camera.Lookfrom = new Vector(x, Camera.Lookfrom.Y, z);
            }
            state++;
            if (state > 200)
            {
                Application.Exit();
            }
            // rotate
            if (state != 0 && state % 20 == 0)
            {
                World.Models[World.Models.Count - 1] = Model.FromObjFile(
                    "Assets/tetrahedron.obj",
                    new Lambertian(new Vector(50, 205, 50) / 255)
                ).Scale(1.2f, 1.2f, 1.2f);
                World.Models[World.Models.Count - 2] =
                    Model.FromObjFile(
                        "Assets/star.obj",
                        new Lambertian(new Vector(255, 193, 37) / 255)
                    ).Translation(0, 3, 3).Scale(0.5f, 0.5f, 0.5f);
            }
            var obj = World.Models.Last();
            for (var i = 0; i < obj.VertexList.Count; i++)
            {
                var it = obj.VertexList[i];
                obj.VertexList[i] = MathHelper.RotateY(it, obj.Center, (float)Math.PI / 10);
            }

            var star = World.Models[World.Models.Count - 2];
            for (var i = 0; i < star.VertexList.Count; i++)
            {
                var it = star.VertexList[i];
                star.VertexList[i] = MathHelper.RotateY(it, star.Center, (float)Math.PI / 10);
            }
        }

        private static void Main(string[] args)
        {
            var form = new Form
            {
                Width = Width + 16,
                Height = Height + 39,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };
            form.Controls.Add(PictureBox);
            form.Load += OnFormOnLoad;
            Application.Run(form);
        }

        private static async void OnFormOnLoad(object sender, EventArgs e)
        {
            while (true)
            {
                Console.WriteLine("start");
                var watch = new Stopwatch();
                watch.Start();
                    var bitmap = await Draw();
                    PictureBox.Image = bitmap;
                    bitmap.Save($"result_{DateTime.Now.Ticks}.png", ImageFormat.Png);
                
                Update();

                watch.Stop();
                Console.WriteLine("{0}ms", watch.ElapsedMilliseconds);
            }
        }
    }
}