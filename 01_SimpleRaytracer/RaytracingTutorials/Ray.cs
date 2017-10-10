using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Ray
    {
        public Vector Origin { get; private set; }
        public Vector Direction { get; private set; }

        public Ray(Vector origin, Vector direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }
    }
}
