using System.Collections.Generic;
using System.Linq;

namespace Raytracer.Hitables
{
    public class HitableList : IHitable
    {
        public HitableList(List<Model> models)
        {
            Models = models;
        }

        public List<Model> Models { get; }

        public bool Hit(Ray ray, float min, float max, ref HitRecord record)
        {
            var temp = new HitRecord();
            var isHit = false;
            var distance = max;
            var hitables = Models.SelectMany(it => it.Hitables).Reverse().ToList();
            foreach (var h in hitables)
            {
                if (h.Hit(ray, min, distance, ref temp))
                {
                    isHit = true;
                    distance = temp.Distance;
                    record = temp;
                }
            }
            return isHit;
        }
    }
}