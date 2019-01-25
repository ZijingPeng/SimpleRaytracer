using System.Collections.Generic;
using System.Drawing;
using Raytracer.Hitables;
using Raytracer.Materials;
using Raytracer.Textures;

namespace Raytracer
{
    public static class Scenes
    {
        public static readonly List<Model> CurrentWorld = Scene3();

        private static List<Model> Scene11()
        {
            var s1 = new Model(
                new Sphere(new Vector(0, 2, -0.8f), 2,
                    new Lambertian(new Vector(0.235f, 0.702f, 0.443f))));

            var s2 = new Model(
                new Sphere(new Vector(0, 2, -0.8f), 2,
                    new Glass(new SolidTexture(Vector.One), 1.5f)));

            var s3 = new Model(
                new Sphere(new Vector(0f, 2f, -0.8f), 2f,
                    new Metal(new Vector(117,117,117) / 255, 1.5f)));

            var t1 = new Model();
            t1.AddVertex(20f, 0f, -7f);
            t1.AddVertex(-20f, 0f, -7f);
            t1.AddVertex(0f, 20f, -7f);
            t1.AddFace(0, 1, 2, new Lambertian(new Vector(1f)));

            var t2 = new Model(
                new Triangle(new Vector(20f, 0f, -8f), new Vector(-20f, 0f, -8f), new Vector(0f, 0f, 10f),
                    new Lambertian(new BitmapTexture((Bitmap)Image.FromFile("Assets/floor.jpg")))
                ));

            return new List<Model> { s3, t1, t2 };
        }
        
        public static List<Model> Scene1()
        {
            var s1 = new Model(
                new Sphere(new Vector(0, 2, -0.8f), 2,
                    new Lambertian(new Vector(0.235f, 0.702f, 0.443f))));

            //var s2 = new Model(
            //    new Sphere(new Vector(0, 2, -0.8f), 2,
            //        new Glass(new SolidTexture(Vector.One), 1.5f)));

            //var s3 = new Model(
            //    new Sphere(new Vector(0f, 2f, -0.8f), 2f,
            //        new Metal(new Vector(255, 240, 245) / 255, 1.5f)));

            var t = new Model();
            t.AddVertex(20f, 0f, -8f);
            t.AddVertex(-20f, 0f, -8f);
            t.AddVertex(0f, 0f, 10f);
            t.AddFace(0, 1, 2, new Lambertian(new Vector(0.5f)));

            return new List<Model> { s1, t };
        }

        public static List<Model> Scene2()
        {
            var bottom = new Model();
            bottom.AddVertex(20f, 0f, -8f);
            bottom.AddVertex(-20f, 0f, -8f);
            bottom.AddVertex(0f, 0f, 10f);
            bottom.AddFace(0, 1, 2, new Lambertian(new Vector(0.5f)));

            var model = Model.FromObjFile(
                "Assets/tetrahedron.obj",
                new Metal(new Vector(255, 235, 59) / 255, 0.2f)
            );

            return new List<Model>
            {
                model,
                bottom
            };
        }

        public static List<Model> Scene3()
        {
            return new List<Model>
            {
                new Model(new Sphere(new Vector(1.16f, 0.3f, -0.85f), 0.3f,
                    new Lambertian(new Vector(72, 61, 139) / 255))),
                new Model(new Sphere(new Vector(0.95f, 0.3f, 1.9f), 0.3f,
                    new Metal(new Vector(84, 139, 84) / 255, 0.2f))),
                new Model(new Sphere(new Vector(1.13f, 0.3f, 0.5f), 0.3f,
                    new Glass(new Vector(1, 1, 1), 1.5f))),
                new Model(new Sphere(new Vector(1.00f, 0.3f, -2.0f), 0.3f,
                    new Glass(new Vector(255, 240, 245) / 255, 1.5f))),
                new Model(new Sphere(new Vector(2.01f, 0.3f, -1.1f), 0.3f,
                    new Glass(new Vector(255, 250, 205) / 255, 1.5f))),
                new Model(new Sphere(new Vector(2.00f, 0.3f, 1.15f), 0.3f,
                    new Metal(new Vector(205, 205, 180) / 255, 0.2f))),
                new Model(new Sphere(new Vector(2.22f, 0.3f, 0.1f), 0.3f,
                    new Lambertian(new Vector(250, 128, 114) / 255))),
                new Model(new Sphere(new Vector(0f, 1.0f, 0.1f), 1.0f,
                    new Glass(new Vector(1), 1.5f))),
                new Model(new Sphere(new Vector(-1.05f, 0.3f, 2.0f), 0.3f,
                    new Lambertian(new Vector(0.235f, 0.702f, 0.443f)))),
                new Model(new Sphere(new Vector(-1.09f, 0.3f, -2.2f), 0.3f,
                    new Metal(new Vector(255, 250, 205) / 255, 0.2f))),
                //new Model(new Sphere(new Vector(4.05f, 0.3f, -0.7f), 0.3f,
                //    new Lambertian(new Vector(0.235f, 0.702f, 0.443f)))),
                new Model(new Sphere(new Vector(-4.09f, 0.3f, -0.7f), 0.3f,
                    new Metal(new Vector(255, 250, 205) / 255, 0.2f))),
                new Model(
                    new Triangle(new Vector(-30f, 0f, -36f), new Vector(-30f, 0f, 36f), new Vector(40f, 0f, 0f),
                        new Lambertian(new BitmapTexture((Bitmap) Image.FromFile("Assets/floor.jpg")))
                    )),
                Model.FromObjFile(
                    "Assets/star.obj",
                    new Lambertian(new Vector(255, 193, 37) / 255)
                ).Translation(0, 3, 3).Scale(0.5f,0.5f,0.5f),
                Model.FromObjFile(
                    "Assets/tetrahedron.obj",
                    new Lambertian(new Vector(50, 205, 50) / 255)
                ).Scale(1.2f, 1.2f,1.2f),
            };
        }

        public static List<Model> Scene4()
        {
            return new List<Model>
            {
                new Model(new Sphere(new Vector(0.1f, 1.0f, 0f), 1.0f,
                    new Metal(new Vector(255, 250, 205) / 255, 0))),
                new Model(
                    new Triangle(new Vector(20f, 0f, -8f), new Vector(-20f, 0f, -8f), new Vector(0f, 0f, 10f),
                        new Metal(new Vector(0.753f, 0.753f, 0.753f), 0f)
                    ))
            };
        }

        public static List<Model> Scene5()
        {
            return new List<Model>
            {
                new Model(new Sphere(new Vector(0, 0, -1), 0.5f, new Lambertian(new Vector(0.1f, 0.2f, 0.5f)))),
                new Model(new Sphere(new Vector(0, -100.5f, -1), 100, new Lambertian(new Vector(0.8f, 0.8f, 0.0f)))),
                new Model(new Sphere(new Vector(1, 0, -1), 0.5f, new Metal(new Vector(0.8f, 0.6f, 0.2f), 0.0f))),
                new Model(new Sphere(new Vector(-1, 0, -1), 0.5f, new Glass(Vector.Zero, 1.5f))),
                new Model(new Sphere(new Vector(-1, 0, -1), -0.45f, new Glass(Vector.Zero, 1.5f)))
            };
        }

        public static List<Model> Scene8()
        {
            return new List<Model>
            {
                new Model(new Sphere(new Vector(-0.85f, 0.3f, 1.16f), 0.3f,
                    new Lambertian(new Vector(72, 61, 139) / 255))),
                new Model(new Sphere(new Vector(1.9f, 0.3f, 0.98f), 0.3f,
                    new Metal(new Vector(84, 139, 84) / 255, 0.2f))),
                new Model(new Sphere(new Vector(0.5f, 0.3f, 1.13f), 0.3f,
                    new Glass(new Vector(1, 1, 1), 1.5f))),
                new Model(new Sphere(new Vector(-2.0f, 0.3f, 1.10f), 0.3f,
                    new Glass(new Vector(255, 240, 245) / 255, 1.5f))),
                new Model(new Sphere(new Vector(-1.1f, 0.3f, 1.95f), 0.3f,
                    new Glass(new Vector(255, 250, 205) / 255, 1.5f))),
                new Model(new Sphere(new Vector(1.15f, 0.3f, 1.96f), 0.3f,
                    new Metal(new Vector(205, 205, 180) / 255, 0.2f))),
                new Model(new Sphere(new Vector(0.1f, 0.3f, 2.02f), 0.3f,
                    new Lambertian(new Vector(255, 193, 193) / 255))),
                new Model(new Sphere(new Vector(0.1f, 1.0f, 0f), 1.0f,
                    new Glass(new Vector(1), 1.5f))),
                new Model(new Sphere(new Vector(2.2f, 0.3f, -1.05f), 0.3f,
                    new Lambertian(new Vector(0.235f, 0.702f, 0.443f)))),
                new Model(new Sphere(new Vector(-2.5f, 0.3f, -1.09f), 0.3f,
                    new Metal(new Vector(1), 0.2f))),

                new Model(new Triangle(new Vector(20f, 0f, -8f), new Vector(-20f, 0f, -8f), new Vector(0f, 0f, 10f),
                    new Lambertian(new BitmapTexture((Bitmap) Image.FromFile("Assets/floor.jpg")))
                ))
            };
        }
    }
}