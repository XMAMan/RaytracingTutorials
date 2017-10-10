using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Triangle
    {
        private Vector v1, v2, v3;
        private Vector edge1, edge2;

        public Vector Normal { get; private set; }
        public Vector Color { get; private set; }
        public float Area { get; private set; }

        public LightSource AssociatedLightSource { get; set; }

        public Triangle(Vector v1, Vector v2, Vector v3, Vector color)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.Color = color;
            this.edge1 = v2 - v1;
            this.edge2 = v3 - v1;
            this.Normal = Vector.Cross(this.edge1, this.edge2).Normalize();

            Vector pa2 = Vector.Cross(edge1, v3 - v2);
            this.Area = (float)Math.Sqrt(Vector.Dot(pa2, pa2)) * 0.5f;
        }

        public IntersectionPoint Intersect(Ray ray)
        {
            // begin calculating determinant - also used to calculate U parameter
            Vector pvec = Vector.Cross(ray.Direction, this.edge2);

            // if determinant is near zero, ray lies in plane of triangle
            float det = Vector.Dot(this.edge1, pvec);

            const float EPSILON = 0.000001f;
            if ((det > -EPSILON) && (det < EPSILON))
            {
                return null;
            }

            float inv_det = 1.0f / det;

            // calculate distance from vertex 0 to ray origin
            Vector tvec = ray.Origin - v1;

            // calculate U parameter and test bounds
            float u = Vector.Dot(tvec, pvec) * inv_det;
            if ((u < 0.0) || (u > 1.0))
            {
                return null;
            }

            // prepare to test V parameter
            Vector qvec = Vector.Cross(tvec, this.edge1);

            // calculate V parameter and test bounds
            float v = Vector.Dot(ray.Direction, qvec) * inv_det;
            if ((v < 0.0) || (u + v > 1.0))
            {
                return null;
            }

            // calculate t, ray intersects triangle
            float distance = Vector.Dot(this.edge2, qvec) * inv_det;
            if (distance < 0)
            {
                return null;
            }
            return new IntersectionPoint(ray.Origin + ray.Direction * distance, this, distance);
        }

        public Vector GetRandomPointOnSurface(Random rand)
        {
            // get two randoms
            float sqr1 = (float)Math.Sqrt(rand.NextDouble());
            float r2 = (float)rand.NextDouble();

            // make barycentric coords
            float a = 1.0f - sqr1;
            float b = (1.0f - r2) * sqr1;

            // make position from barycentrics
            // calculate interpolation by using two edges as axes scaled by the
            // barycentrics
            return this.edge1 * a + this.edge2 * b + v1;
        }
    }
}
