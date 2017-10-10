using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    interface IKdNode
    {
        void FixedRadiusSearch(QueryAndResultData data);
    }

    class QueryAndResultData
    {
        //Query-Data
        public IPoint QueryPoint;
        public float SearchRadius;
        public float SquaredSearchRadius;

        //Result-Data
        public List<IPoint> ResultList = new List<IPoint>();
    }

    class KdSplitNode : IKdNode
    {
        private int cuttingDimension;
        private float cuttingValue;
        private IKdNode leftChild, rightChild;

        public KdSplitNode(int cuttingDimension, float cuttingValue, IKdNode leftChild, IKdNode rightChild)
        {
            this.cuttingDimension = cuttingDimension;
            this.cuttingValue = cuttingValue;
            this.leftChild = leftChild;
            this.rightChild = rightChild;
        }

        public void FixedRadiusSearch(QueryAndResultData data)
        {
            float cutDiff = data.QueryPoint[this.cuttingDimension] - this.cuttingValue; // distance to cutting plane

            if (cutDiff < 0)    // left of cutting plane
            {
                if (cutDiff < -data.SearchRadius)
                    this.leftChild.FixedRadiusSearch(data); // visit closer child first
                else
                {
                    this.leftChild.FixedRadiusSearch(data);
                    this.rightChild.FixedRadiusSearch(data);
                }
            }
            else  // right of cutting plane
            {
                if (cutDiff > data.SearchRadius)
                    this.rightChild.FixedRadiusSearch(data);    // visit closer child first
                else
                {
                    this.leftChild.FixedRadiusSearch(data);
                    this.rightChild.FixedRadiusSearch(data);
                }
            }
        }
    }

    class KdLeafNode : IKdNode
    {
        public const int MaxPointsPerLeafe = 5;

        private IPoint[] points;
        private int dimension;  //Jede Punkt aus der 'points'-List hat die Dimension 'dimension' (Wenn 'points' also ein Liste von 2D-Punkten ist, dann steht hier eine 2)

        public KdLeafNode(IPoint[] points, int dimension)
        {
            this.points = points;
            this.dimension = dimension;
        }

        public void FixedRadiusSearch(QueryAndResultData data)
        {
            for (int i = 0; i < this.points.Length; i++)            // check points in bucket
            {
                IPoint pp = this.points[i];
                IPoint qq = data.QueryPoint;
                float distanceToDataPoint = 0;
                int d;

                for (d = 0; d < this.dimension; d++)
                {
                    float t = qq[d] - pp[d];
                    distanceToDataPoint += t * t;
                    if (distanceToDataPoint > data.SquaredSearchRadius) break;       // exceeds dist to k-th smallest?
                }

                if (d >= this.dimension)                                 // among the k best?
                {
                    data.ResultList.Add(this.points[i]);
                }
            }
        }
    }
}
