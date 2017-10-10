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
                    Vector pixelColor = GetPixelColorFromPhotonmap(x, y, photonmap);
                    image.SetPixel(x, y, VectorToColor(pixelColor));
                }

            return image;
        }

        private Vector GetPixelColorFromPhotonmap(int x, int y, Photonmap photonmap)
        {
            Ray primaryRay = data.Camera.GetPrimaryRay(x, y);
            float cosAtCamera = Vector.Dot(primaryRay.Direction, this.data.Camera.Forward);
            Vector pathWeight = new Vector(1, 1, 1) * cosAtCamera;

            var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

            Vector pixelColor = new Vector(0, 0, 0);

            if (eyePoint != null)
            {
                //100000 3000 1000
                float searchRadius = 0.00634257728f * 2;//0.0383904837f;//0.065260686f;
                var photons = photonmap.SearchPhotons(eyePoint, searchRadius);
                float kernelFunction = 1.0f / (searchRadius * searchRadius * (float)Math.PI);
                foreach (var photon in photons)
                {
                    pixelColor += Vector.Mult(pathWeight, Vector.Mult(DiffuseBrdf.Evaluate(eyePoint), photon.PathWeight) * kernelFunction);
                }
            }

            return pixelColor;
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
