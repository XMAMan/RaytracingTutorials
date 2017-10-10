using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class IntersectionFinder
    {
        private Triangle[] triangles;

        public IntersectionFinder(Triangle[] triangles)
        {
            this.triangles = triangles;
        }

        public IntersectionPoint GetIntersectionPoint(Ray ray)
        {
            float currentMinDistance = float.MaxValue;
            float minAllowedDistance = 0.000001f;

            IntersectionPoint resultPoint = null;

            foreach (var triangle in this.triangles)
            {
                var point = triangle.Intersect(ray);
                if (point != null && point.DistanceToRayOrigin > minAllowedDistance && point.DistanceToRayOrigin < currentMinDistance)
                {
                    resultPoint = point;
                    currentMinDistance = point.DistanceToRayOrigin;
                }
            }

            return resultPoint;
        }
    }
}
