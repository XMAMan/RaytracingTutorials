using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RaytracingTutorials
{
    class SimpleRenderer
    {
        private CornellBoxSceneData data;
        private IntersectionFinder intersectionFinder;
        private int width;
        private int height;

        public SimpleRenderer(int width, int height)
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

            Photonmap photonmap = new Photonmap(this.intersectionFinder, this.data.LightSource, 100000);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    int sampleCount = 10;
                    Vector sum = new Vector(0, 0, 0);
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sum += EstimateColorWithPhotonmapping(x, y, rand, photonmap);
                    }
                    sum /= sampleCount;
                    image.SetPixel(x, y, VectorToColor(sum));
                }

            return image;
        }

        private Vector EstimateColorWithPhotonmapping(int x, int y, Random rand, Photonmap photonmap)
        {
            Ray primaryRay = data.Camera.GetPrimaryRay(x, y);
            float cosAtCamera = Vector.Dot(primaryRay.Direction, this.data.Camera.Forward);
            Vector pathWeight = new Vector(1, 1, 1) * cosAtCamera;

            var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

            Vector pixelColor = new Vector(0, 0, 0);

            if (eyePoint != null)
            {
                if (eyePoint.IsLocatedOnLightSource)
                {
                    return eyePoint.Emission;
                }

                pathWeight = Vector.Mult(pathWeight, DiffuseBrdf.Evaluate(eyePoint));

                //Direct Lighting
                var lightPoint = data.LightSource.GetRandomPointOnLightSource(rand);
                if (IsVisible(eyePoint.Position, lightPoint.Position))
                {
                    pixelColor += Vector.Mult(pathWeight, lightPoint.Color * GeometryTerm(eyePoint, lightPoint) / lightPoint.PdfA);
                }

                //Final Gathering
                Vector newDirection = DiffuseBrdf.SampleDirection(eyePoint.Normal, rand);
                pathWeight = pathWeight * Vector.Dot(eyePoint.Normal, newDirection) / DiffuseBrdf.PdfW(eyePoint.Normal, newDirection);
                var finalGatherPoint = this.intersectionFinder.GetIntersectionPoint(new Ray(eyePoint.Position, newDirection));

                if (finalGatherPoint != null)
                {
                    //100000 3000 1000
                    float searchRadius = 0.00634257728f * 2;//0.0383904837f;//0.065260686f;
                    var photons = photonmap.SearchPhotons(finalGatherPoint, searchRadius);
                    float kernelFunction = 1.0f / (searchRadius * searchRadius * (float)Math.PI);
                    foreach (var photon in photons)
                    {
                        pixelColor += Vector.Mult(pathWeight, Vector.Mult(DiffuseBrdf.Evaluate(finalGatherPoint), photon.PathWeight) * kernelFunction);
                    }
                }
            }

            return pixelColor;
        }

        private bool IsVisible(Vector point1, Vector point2)
        {
            var point = this.intersectionFinder.GetIntersectionPoint(new Ray(point1, (point2 - point1).Normalize()));
            if (point == null) return false;
            if ((point.Position - point2).Length() < 0.0001f) return true;
            return false;
        }

        private float GeometryTerm(IVertexPoint point1, IVertexPoint point2)
        {
            Vector v1To2 = point2.Position - point1.Position;
            float distance = v1To2.Length();
            Vector direction1To2 = v1To2 / distance;
            float lamda1 = Math.Max(Vector.Dot(point1.Normal, direction1To2), 0);
            float lamda2 = Math.Max(Vector.Dot(point2.Normal, -direction1To2), 0);
            return lamda1 * lamda2 / (distance * distance);
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
