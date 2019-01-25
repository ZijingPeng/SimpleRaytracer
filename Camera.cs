using System;

namespace Raytracer
{
    internal class Camera
    {
        public Camera(Vector lookfrom, Vector lookto, Vector viewup, float fov, float aspect)
        {
            Lookfrom = lookfrom;
            Lookto = lookto;
            Viewup = viewup;
            Fov = fov;
            Aspect = aspect;
        }

        private float Theta
        {
            get { return (float) (Fov * Math.PI / 180); }
        }

        private float HalfHeight
        {
            get { return (float) Math.Tan(Theta / 2); }
        }

        private float HalfWidth
        {
            get { return Aspect * HalfHeight; }
        }

        private Vector W
        {
            get { return (Lookfrom - Lookto).Normalize(); }
        }

        private Vector U
        {
            get { return W.Cross(Viewup).Normalize(); }
        }

        private Vector V
        {
            get { return U.Cross(W); }
        }

        public Vector Horizontal
        {
            get { return 2 * HalfWidth * U; }
        }

        public Vector LowerLeftCorner
        {
            get { return Origin - HalfWidth * U - HalfHeight * V - W; }
        }

        public Vector Origin
        {
            get { return Lookfrom; }
        }

        public Vector Vertical
        {
            get { return 2 * HalfHeight * V; }
        }

        public Vector Lookfrom { get; set; }
        public Vector Lookto { get; set; }
        public Vector Viewup { get; set; }
        public float Fov { get; set; }
        public float Aspect { get; set; }

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeftCorner + u * Horizontal + v * Vertical - Origin);
        }
    }
}