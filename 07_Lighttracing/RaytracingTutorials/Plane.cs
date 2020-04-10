using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Plane
    {
        public float a, b, c, d; //a*x + b*y + c*z + d = 0

        public Plane(Vector N, Vector P)//N steht auf P
        {
            this.a = N.X;
            this.b = N.Y;
            this.c = N.Z;
            this.d = -Vector.Dot(N, P);
        }

        public Vector GetIntersectionPointWithRay(Ray ray)
        {
            float mr_proper = this.a * ray.Direction.X + this.b * ray.Direction.Y + this.c * ray.Direction.Z;
            if (mr_proper == 0) return null;
            float f = this.a * ray.Origin.X + this.b * ray.Origin.Y + this.c * ray.Origin.Z + this.d;
            float distance = (-f) / mr_proper;
            if (distance < 0) return null; //throw new Exception("Ebene liegt hinter Strahl-Start-Punkt"); //Ebene liegt hinter Strahl-Start-Punkt
            return ray.Origin + ray.Direction * distance;
        }
    }
}
