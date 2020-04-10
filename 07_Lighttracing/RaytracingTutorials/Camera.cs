using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Camera
    {
        private Plane imagePlane;
        private float imagePlaneDistance;
        private float[] worldToCameraSpace;
        private int screenWidth; 
        private int screenHeight;

        public Vector Position { get; private set; }
        public Vector Forward { get; private set; }

        public Camera(Vector position, Vector forward, Vector up, float openingAngleY, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            double gradRad = (openingAngleY * Math.PI / 180.0);
            this.imagePlaneDistance = (float)(screenHeight / (2 * Math.Tan(gradRad / 2)));
            this.imagePlane = new Plane(new Vector(0, 0, 1), new Vector(0, 0, imagePlaneDistance));

            this.Position = position;
            this.Forward = forward.Normalize();
    
            Vector N = forward.Normalize();
            Vector T = up.Normalize();
            Vector B = Vector.Cross(N, T);
            this.worldToCameraSpace = new float[] { B.X,  T.X,  N.X,  0,
                                                    B.Y,  T.Y,  N.Y,  0,
                                                    B.Z,  T.Z,  N.Z,  0,
                                                    0,    0,    0,    0};
        }

        //Gets the pixel from which you can so the 'point'
        public Vector2D GetPixelPositionFromScenePoint(Vector point)
        {
            Vector toPointDirection = Matrix.MatrixVectorMultiplication(this.worldToCameraSpace, (point - this.Position).Normalize());

            Ray ray = new Ray(new Vector(0, 0, 0), toPointDirection);

            Vector p = this.imagePlane.GetIntersectionPointWithRay(ray);//x = -f*0.5 .. +f*0.5    y = -0.5 .. +0.5f            
            if (p == null) return null;

            var pixelPosition = new Vector2D((p.X + this.screenWidth / 2.0f), (p.Y + this.screenHeight / 2.0f));

            if (pixelPosition.X < 0 || pixelPosition.Y < 0 || pixelPosition.X > this.screenWidth || pixelPosition.Y > this.screenHeight) return null;

            return pixelPosition;
        }

        //This is the PdfW (Pdf with Respect to Solid Angle) from the direction 'primaryRayDirection', if you sample equaly over the pixel-area
        //If you use Pathtracing and don't divide with this Camera-PdfW-Factor here, than you implicite multipli with 1/CameraPdfW
        public float PdfWForPrimaryRayDirectionSampling(Vector primaryRayDirection)
        {
            float cosAtCamera = Vector.Dot(this.Forward, primaryRayDirection);
            float distanceToPixel = this.imagePlaneDistance / cosAtCamera;
            float pixelSizeOnImagePlane = 1; //The Imageplane has per definition this distance, that each pixel edge has length from 1
            float pixelPdfA = 1.0f / (pixelSizeOnImagePlane * pixelSizeOnImagePlane);
            return pixelPdfA * distanceToPixel * distanceToPixel / cosAtCamera;
        }
    }
}
