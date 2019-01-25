using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Raytracer.Hitables;
using Raytracer.Materials;
using Raytracer.Textures;

namespace Raytracer
{
    public class Model
    {
        private readonly List<IHitable> hitables = new List<IHitable>();

        private readonly List<Tuple<int, int, int, IMaterial>> indiceList
            = new List<Tuple<int, int, int, IMaterial>>();

        public Model() { }

        public Model(IHitable hitable)
        {
            AddHitable(hitable);
        }

        public Vector Center {
            get { return VertexList.Aggregate(Vector.Zero, (acc, v) => acc + v) / VertexList.Count; }
        }

        public List<IHitable> Hitables => hitables.Concat(
            indiceList.Select(it =>
                new Triangle(
                    VertexList[it.Item1],
                    VertexList[it.Item2],
                    VertexList[it.Item3],
                    it.Item4))
        ).ToList();

        public List<Vector> VertexList { get; } = new List<Vector>();

        public static Model FromObjFile(string path)
        {
            return FromObjFile(path, new Metal(new Vector(0.235f, 0.702f, 0.443f), 1.5f));
        }

        public static Model FromObjFile(string path, IMaterial defaultMaterial)
        {
            var root = Path.GetDirectoryName(path);
            var model = new Model();
            var lastMaterial = defaultMaterial;
            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    // dismiss blank space
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // use comment to inform texture
                    if (line.StartsWith("#m"))
                    {
                        var words = line.Split(' ');
                        var materialStr = words[1];
                        switch (materialStr)
                        {
                            case "metal":
                                {
                                    var arg1 = float.Parse(words[2]);
                                    var arg2 = words[3];
                                    Debug.Assert(root != null, nameof(root) + " != null");
                                    var imagePath = Path.Combine(root, arg2);
                                    if (!Directory.Exists(imagePath))
                                    {
                                        throw new FileNotFoundException(imagePath);
                                    }
                                    lastMaterial = new Metal(new BitmapTexture(imagePath), arg1);
                                    break;
                                }
                        }
                    }

                    // dismiss comment
                    if (line.StartsWith("#"))
                    {
                        continue;
                    }

                    var seps = line.Split(' ');
                    var pattern = seps[0];
                    if (pattern == "f")
                    {
                        // define face
                        // only get index of vertex
                        seps = seps.Select(it =>
                        {
                            var p = it.IndexOf("/", StringComparison.Ordinal);
                            return p == -1 ? it : it.Substring(0, p);
                        }).ToArray();
                        switch (seps.Length)
                        {
                            case 4:
                                {
                                    var arg1 = int.Parse(seps[1]) - 1;
                                    var arg2 = int.Parse(seps[2]) - 1;
                                    var arg3 = int.Parse(seps[3]) - 1;
                                    model.AddFace(arg1, arg2, arg3, lastMaterial);
                                    break;
                                }
                            case 5:
                                {
                                    var arg1 = int.Parse(seps[1]) - 1;
                                    var arg2 = int.Parse(seps[2]) - 1;
                                    var arg3 = int.Parse(seps[3]) - 1;
                                    var arg4 = int.Parse(seps[4]) - 1;
                                    model.AddFace(arg1, arg2, arg3, lastMaterial);
                                    model.AddFace(arg1, arg2, arg4, lastMaterial);
                                    break;
                                }
                            default:
                                throw new InvalidDataException(path);
                        }
                    }
                    else if (pattern == "v")
                    {
                        // is vertex
                        var arg1 = float.Parse(seps[1]);
                        var arg2 = float.Parse(seps[2]);
                        var arg3 = float.Parse(seps[3]);
                        model.AddVertex(arg1, arg2, arg3);
                    }
                }
            }
            return model;
        }

        public void AddFace(int a, int b, int c, IMaterial material)
        {
            indiceList.Add(new Tuple<int, int, int, IMaterial>(a, b, c, material));
        }

        public void AddHitable(IHitable hitable)
        {
            hitables.Add(hitable);
        }

        public void AddVertex(float x, float y, float z)
        {
            VertexList.Add(new Vector(x, y, z));
        }

        // translation
        public Model Translation(float x, float y, float z)
        {
            var vec = new Vector(x, y, z);
            for (var i = 0; i < VertexList.Count; i++)
            {
                VertexList[i] += vec;
            }
            return this;
        }

        // scale
        public Model Scale(float x = 1, float y = 1, float z = 1)
        {
            var vec = new Vector(x, y, z);
            for (var i = 0; i < VertexList.Count; i++)
            {
                VertexList[i].Scale(vec);
            }
            return this;
        }
    }
}