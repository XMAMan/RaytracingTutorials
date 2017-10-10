using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    static class MidpointSplitFromLongestEdge
    {
        public static KdSplitResult Split(IPoint[] points, int dimension)
        {
            KdSplitResult result = new KdSplitResult();
            AxisAlignedBox boundingBox = new AxisAlignedBox(points, dimension);
            result.CuttingDimension = boundingBox.GetAxisIndizeFromLongestEdge();
            result.CuttingValue = (boundingBox.LowerBound[result.CuttingDimension] + boundingBox.UpperBound[result.CuttingDimension]) / 2;

            List<IPoint> left = new List<IPoint>();
            List<IPoint> right = new List<IPoint>();

            foreach (var p in points)
            {
                if (p[result.CuttingDimension] < result.CuttingValue) left.Add(p); else right.Add(p);
            }

            result.PoingsOnLeftSide = left.ToArray();
            result.PoingsOnRightSide = right.ToArray();
            
            return result;
        }
    }
}
