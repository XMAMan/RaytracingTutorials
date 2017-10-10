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

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Ray primaryRay = data.Camera.GetPrimaryRay(x, y);
                    var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);
                    if (eyePoint != null)
                    {
                        image.SetPixel(x, y, VectorToColor(eyePoint.Color * Vector.Dot(eyePoint.Normal, -primaryRay.Direction)));
                    }
                }

            return image;
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
