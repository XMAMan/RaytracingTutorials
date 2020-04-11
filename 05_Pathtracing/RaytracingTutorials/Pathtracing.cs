using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RaytracingTutorials
{
    //Pathtracing implementation in c#
    class Pathtracing
    {
        private CornellBoxSceneData data;
        private IntersectionFinder intersectionFinder;
        private int width;
        private int height;

        public Pathtracing(int width, int height)
        {
            this.data = new CornellBoxSceneData(width, height);
            this.intersectionFinder = new IntersectionFinder(data.Triangles);
            this.width = width;
            this.height = height;
        }

        public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            int sampleCount = 10;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Vector sum = new Vector(0, 0, 0);
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sum += EstimateColorWithPathtracing(x, y, rand);
                    }
                    sum /= sampleCount;
                    image.SetPixel(x, y, VectorToColor(sum));
                }

            return image;
        }

        private Vector EstimateColorWithPathtracing(int x, int y, Random rand)
        {
            Ray primaryRay = data.Camera.GetPrimaryRay(x, y);
            float cosAtCamera = Vector.Dot(primaryRay.Direction, this.data.Camera.Forward);
            Vector pathWeight = new Vector(1, 1, 1) * cosAtCamera;

            Ray ray = primaryRay;

            int maxPathLength = 5;
            for (int i = 0; i < maxPathLength; i++)
            {
                var point = this.intersectionFinder.GetIntersectionPoint(ray);
                if (point == null)
                {
                    return new Vector(0, 0, 0);
                }

                if (point.IsLocatedOnLightSource && Vector.Dot(point.Normal, -ray.Direction) > 0)
                {
                    pathWeight = Vector.Mult(pathWeight, point.Emission);
                    return pathWeight;
                }

                //Abbruch mit Russia Rollete
                float continuationPdf = Math.Min(1, point.Color.Max());
                if (rand.NextDouble() >= continuationPdf)
                    return new Vector(0, 0, 0); // Absorbation

                Vector newDirection = DiffuseBrdf.SampleDirection(point.Normal, rand);
                pathWeight = Vector.Mult(pathWeight * Vector.Dot(point.Normal, newDirection) / DiffuseBrdf.PdfW(point.Normal, newDirection), DiffuseBrdf.Evaluate(point)) / continuationPdf;
                ray = new Ray(point.Position, newDirection);
            }

            return new Vector(0, 0, 0);
        }

        private Color VectorToColor(Vector v)
        {
            Vector c = new Vector(Clamp(v.X), Clamp(v.Y), Clamp(v.Z));
            return Color.FromArgb((int)(c.X * 255), (int)(c.Y * 255), (int)(c.Z * 255));
        }

        private float Clamp(float f)
        {
            if (f < 0) f = 0;
            if (f > 1) f = 1;
            return f;
        }
    }
}
