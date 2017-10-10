using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    static class DiffuseBrdf
    {
        public static Vector Evaluate(IntersectionPoint point)
        {
            return point.Color / (float)Math.PI;
        }

        public static Vector SampleDirection(Vector normale, Random rand)
        {
            float r1 = (float)(2 * Math.PI * rand.NextDouble());
            float r2 = (float)rand.NextDouble();

            float r2s = (float)Math.Sqrt(1 - r2);

            Vector w = normale,
                   u = Vector.Cross((Math.Abs(w.X) > 0.1f ? new Vector(0, 1, 0) : new Vector(1, 0, 0)), w).Normalize(),
                   v = Vector.Cross(w, u);

            Vector d = ((u * (float)Math.Cos(r1) * r2s + v * (float)Math.Sin(r1) * r2s + w * (float)Math.Sqrt(r2))).Normalize();

            return d;
        }

        public static float PdfW(Vector normal, Vector sampledDirection)
        {
            return Math.Max(1e-6f, Math.Max(0, Vector.Dot(normal, sampledDirection)) / (float)Math.PI);
        }
    }
}
