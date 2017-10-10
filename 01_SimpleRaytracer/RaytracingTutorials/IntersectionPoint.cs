using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class IntersectionPoint
    {
        private Triangle triangle;

        public Vector Position { get; private set; }
        public Vector Normal { get { return this.triangle.Normal; } }
        public Vector Color { get { return this.triangle.Color; } }
        public float DistanceToRayOrigin { get; private set; }

        public IntersectionPoint(Vector position, Triangle triangle, float distanceToRayOrigin)
        {
            this.Position = position;
            this.triangle = triangle;
            this.DistanceToRayOrigin = distanceToRayOrigin;
        }
    }
}
