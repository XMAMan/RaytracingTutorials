using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RaytracingTutorials
{
    //Lighttracing implementation in c#
    class Lighttracing
    {
        private CornellBoxSceneData data;
        private IntersectionFinder intersectionFinder;
        private int width;
        private int height;

        public Lighttracing(int width, int height)
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

            int iterationCount = 10;

            Vector[,] frames = new Vector[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    frames[x, y] = new Vector(0, 0, 0);
                }

            for (int i=0;i< iterationCount;i++)
            {
                var frame = EstimateFrameWithLightracing(width, height, rand);

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        frames[x, y] += frame[x,y];
                    }
            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    image.SetPixel(x, y, VectorToColor(frames[x, y] / iterationCount));
                }

            return image;
        }

        private Vector[,] EstimateFrameWithLightracing(int width, int height, Random rand)
        {
            int lightPathsPerIteration = width * height;
            int maxPathLength = 5;

            Vector[,] frame = new Vector[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    frame[x, y] = new Vector(0, 0, 0);
                }

            for (int j = 0; j < lightPathsPerIteration; j++)
            {
                //Sample Position on Lightsource
                var point = data.LightSource.SampleRandomPointOnLightSource(rand);

                //Pathweight
                Vector pathWeight = new Vector(1, 1, 1) * data.LightSource.EmissionPerArea / data.LightSource.PdfA;

                //Connect Point on Lightsource with Camera
                var c = TryToConnectPointWithCamera(point, pathWeight, lightPathsPerIteration);
                if (c != null) frame[c.PixelX, c.PixelY] += c.PathContribution;

                //Sample Direction from Point on LightSource into scene. Use the diffuse Brdf
                Vector direction = DiffuseBrdf.SampleDirection(point.Normal, rand); //Direct vom Point on Light into some point in scene
                float pdfW = DiffuseBrdf.PdfW(point.Normal, direction); //PDF with Respect to Solid Angle from 'direction'

                //New Pathweight after sampling direction 
                float cosAtPoint = Math.Max(0, Vector.Dot(point.Normal, direction));
                pathWeight *= cosAtPoint / pdfW;

                Ray ray = new Ray(point.Position, direction);

                for (int k = 0; k < maxPathLength; k++)
                {
                    //Get next intersectionpoint with scene starting from old point-Position
                    point = this.intersectionFinder.GetIntersectionPoint(ray);
                    if (point == null) break; //Break Light-Path-Creation

                    //Connect Point in scene with Camera
                    c = TryToConnectPointWithCamera(point, pathWeight, lightPathsPerIteration);
                    if (c != null) frame[c.PixelX, c.PixelY] += c.PathContribution;

                    //The Lightsource dosn't reflect light
                    if (point.IsLocatedOnLightSource) break;

                    //Break with Russia Rollete
                    float continuationPdf = Math.Min(1, point.Color.Max());
                    if (rand.NextDouble() >= continuationPdf)
                        break; // Absorbation

                    //Go ahead with lighttracing after connection with camera
                    direction = DiffuseBrdf.SampleDirection(point.Normal, rand);
                    pdfW = DiffuseBrdf.PdfW(point.Normal, direction);
                    cosAtPoint = Math.Max(0, Vector.Dot(point.Normal, direction));
                    pathWeight = Vector.Mult(pathWeight, DiffuseBrdf.Evaluate(point)) * cosAtPoint / (pdfW * continuationPdf);
                    ray = new Ray(point.Position, direction);
                }
            }

            return frame;
        }

        class ConnectionData
        {
            public int PixelX;
            public int PixelY;
            public Vector PathContribution;
        }

        private ConnectionData TryToConnectPointWithCamera(IntersectionPoint point, Vector pathWeight, int lightPathsPerIteration)
        {
            Vector2D pixelPosition = GetPixelPositionForScenePoint(point.Position);

            if (pixelPosition != null)
            {
                //GeometryTerm
                Vector cameraToLightPointDirection = (point.Position - data.Camera.Position).Normalize();
                float cosAtCamera = Vector.Dot(cameraToLightPointDirection, data.Camera.Forward);
                float cosAtPoint = Vector.Dot(-cameraToLightPointDirection, point.Normal);
                float geomertryFaktor = (cosAtCamera * cosAtPoint) / (point.Position - data.Camera.Position).SqrLength();

                //Brdf (Inputdirection is Direction from preceeding point to 'point' and Outgoingdirection is direction from 'point' to Camera-Point
                //In this case its a Diffuse Brdf and no Input- or Outputdirection is needed
                Vector brdf = DiffuseBrdf.Evaluate(point);

                //If you use Pathtracing and don't divide the Pathweight with the PdfW from the Camera, then you implicite multiply the Pathweight with (1 / CameraPdfW)
                //To get with Lightracing the same Results as with Pathtracing, you have to multipli the PathWeight with the cameraPdfW, which is also called PixelFilter
                float cameraPdfW = data.Camera.PdfWForPrimaryRayDirectionSampling(cameraToLightPointDirection); //cameraPdfW == PixelFilter

                Vector pathContribution = Vector.Mult(pathWeight, brdf) * geomertryFaktor / lightPathsPerIteration * cameraPdfW;

                int x = (int)pixelPosition.X;
                int y = (int)pixelPosition.Y;
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    return new ConnectionData()
                    {
                        PixelX = x,
                        PixelY = y,
                        PathContribution = pathContribution,
                    };                    
                }
            }

            return null;
        }

        private Vector2D GetPixelPositionForScenePoint(Vector point)
        {
            //Step 1: Create Direction from Camera to current point
            Vector cameraToLightPoint = point - data.Camera.Position;
            float cameraToLightPointDistance = cameraToLightPoint.Length();
            Vector cameraToLightPointDirection = cameraToLightPoint / cameraToLightPointDistance;

            //Step 2: Brdf-VisibleTest
            float cameraCos = Vector.Dot(data.Camera.Forward, cameraToLightPointDirection);
            if (cameraCos <= 0) return null; //Normal from Point dosn't show to Camera

            //Step 3: View-Frustum-Test
            var pixelPosition = data.Camera.GetPixelPositionFromScenePoint(point);
            if (pixelPosition == null) return null; //Point is outside from ViewFrustum

            //Step 4: Shadow-Ray-Test
            Ray primaryRay = new Ray(data.Camera.Position, cameraToLightPointDirection);
            var lightVisiblePoint = this.intersectionFinder.GetIntersectionPoint(primaryRay);

            float DistanceForPoint2PointVisibleCheck = 0.0001f; //Magic Number
            bool isVisible = lightVisiblePoint != null && (lightVisiblePoint.Position - point).Length() < DistanceForPoint2PointVisibleCheck;
            if (isVisible == false) return null;

            return pixelPosition;
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
