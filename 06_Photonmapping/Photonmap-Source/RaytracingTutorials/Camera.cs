using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Camera
    {
        private Vector position;
        private Vector imagePlaneCorner;
        private float[] cameraToWorldSpace;

        public Vector Forward { get; private set; }

        public Camera(Vector position, Vector forward, Vector up, float openingAngleY, int screenWidth, int screenHeight)
        {
            double gradRad = (openingAngleY * Math.PI / 180.0);
            float imagePlaneDistance = (float)(screenHeight / (2 * Math.Tan(gradRad / 2)));

            this.position = position;
            this.Forward = forward.Normalize();
            this.imagePlaneCorner = new Vector(-screenWidth / 2.0f, -screenHeight / 2.0f, imagePlaneDistance);

            Vector N = forward.Normalize();
            Vector T = up.Normalize();//-(up - N * Vector.Dot(up, N)).Normalize();
            Vector B = Vector.Cross(N, T);
            this.cameraToWorldSpace = new float[] { B.X,  B.Y,  B.Z,  0,
                                                    T.X,  T.Y,  T.Z,  0,
                                                    N.X,  N.Y,  N.Z,  0,
                                                    0,    0,    0,    0};
        }

        public Ray GetPrimaryRay(int x, int y)
        {
            Vector pointOnImagePlane = this.imagePlaneCorner + new Vector(x, 0, 0) + new Vector(0, y, 0);
            Vector directionInCameraSpace = (pointOnImagePlane - new Vector(0, 0, 0)).Normalize();

            return new Ray(this.position, Matrix.MatrixVectorMultiplication(this.cameraToWorldSpace, directionInCameraSpace));
        }
    }
}
