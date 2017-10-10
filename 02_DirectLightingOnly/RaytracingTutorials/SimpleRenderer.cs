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

        //Version 1: Pixel-Schatten ohne Geometryterm und Brdf
        /*public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Ray primaryRay = data.Camera.GetPrimaryRay(x, y);

                    var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

                    Vector pixelColor = new Vector(0, 0, 0);

                    if (eyePoint != null)
                    {
                        if (eyePoint.IsLocatedOnLightSource)
                        {
                            pixelColor += eyePoint.Emission;
                        }
                        else
                        {
                            //Direct Lighting
                            var lightPoint = data.LightSource.GetRandomPointOnLightSource(rand);
                            if (IsVisible(eyePoint.Position, lightPoint.Position))
                            {
                                pixelColor += eyePoint.Color;
                            }
                        }
                    }

                    image.SetPixel(x, y, VectorToColor(pixelColor));
                }

            return image;
        }*/

        //Version 2: Pixel-Schatten mit Geometryterm und ohne Brdf
        /*public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Ray primaryRay = data.Camera.GetPrimaryRay(x, y);

                    var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

                    Vector pixelColor = new Vector(0, 0, 0);

                    if (eyePoint != null)
                    {
                        if (eyePoint.IsLocatedOnLightSource)
                        {
                            pixelColor += eyePoint.Emission;
                        }
                        else
                        {
                            //Direct Lighting
                            var lightPoint = data.LightSource.GetRandomPointOnLightSource(rand);
                            if (IsVisible(eyePoint.Position, lightPoint.Position))
                            {
                                pixelColor += eyePoint.Color * GeometryTerm(eyePoint, lightPoint);
                            }
                        }
                    }

                    image.SetPixel(x, y, VectorToColor(pixelColor));
                }

            return image;
        }*/

        //Version 3: Pixel-Schatten mit Geometryterm und mit Brdf
        /*public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Ray primaryRay = data.Camera.GetPrimaryRay(x, y);

                    var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

                    Vector pixelColor = new Vector(0, 0, 0);

                    if (eyePoint != null)
                    {
                        if (eyePoint.IsLocatedOnLightSource)
                        {
                            pixelColor += eyePoint.Emission;
                        }
                        else
                        {
                            //Direct Lighting
                            var lightPoint = data.LightSource.GetRandomPointOnLightSource(rand);
                            if (IsVisible(eyePoint.Position, lightPoint.Position))
                            {
                                pixelColor += Vector.Mult(DiffuseBrdf.Evaluate(eyePoint), eyePoint.Color * GeometryTerm(eyePoint, lightPoint));
                            }
                        }
                    }

                    image.SetPixel(x, y, VectorToColor(pixelColor));
                }

            return image;
        }*/

        //Verison 4: Weiche Schatten
        public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    int sampleCount = 10;
                    Vector sum = new Vector(0, 0, 0);
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sum += EstimatePixelColor(x, y, rand);
                    }
                    sum /= sampleCount;
                    image.SetPixel(x, y, VectorToColor(sum));
                }

            return image;
        }

        private Vector EstimatePixelColor(int x, int y, Random rand)
        {
            Ray primaryRay = data.Camera.GetPrimaryRay(x, y);

            var eyePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

            Vector pixelColor = new Vector(0, 0, 0);

            if (eyePoint != null)
            {
                if (eyePoint.IsLocatedOnLightSource)
                {
                    return eyePoint.Emission;
                }

                //Direct Lighting
                var lightPoint = data.LightSource.GetRandomPointOnLightSource(rand);
                if (IsVisible(eyePoint.Position, lightPoint.Position))
                {
                    pixelColor += Vector.Mult(DiffuseBrdf.Evaluate(eyePoint), lightPoint.Color * GeometryTerm(eyePoint, lightPoint));
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
