using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class KdTree
    {
        private IKdNode rootNode;

        public KdTree(IPoint[] points, int dimension = 3, int maxPointCountPerLeafNode = 10)
        {
            this.rootNode = CreateKdNode(points, dimension, maxPointCountPerLeafNode, 15);
        }

        private static IKdNode CreateKdNode(IPoint[] points, int dimension, int maxPointCountPerLeafNode, int rekursionDeep)
        {
            if (points.Length <= maxPointCountPerLeafNode || rekursionDeep < 0)   // n small, make a leaf node
            {
                return new KdLeafNode(points, dimension);
            }
            else
            {
                var splitResult = MidpointSplitFromLongestEdge.Split(points, dimension);

                IKdNode leftNode = CreateKdNode(splitResult.PoingsOnLeftSide, dimension, maxPointCountPerLeafNode, rekursionDeep - 1);
                IKdNode rightNode = CreateKdNode(splitResult.PoingsOnRightSide, dimension, maxPointCountPerLeafNode, rekursionDeep - 1);

                return new KdSplitNode(splitResult.CuttingDimension, splitResult.CuttingValue, leftNode, rightNode);
            }
        }

        public List<IPoint> FixedRadiusSearch(IPoint queryPoint, float searchRadius)   // Search Radius bound                                              
        {
            QueryAndResultData data = new QueryAndResultData()
            {
                QueryPoint = queryPoint,
                SearchRadius = searchRadius,
                SquaredSearchRadius = searchRadius * searchRadius
            };

            this.rootNode.FixedRadiusSearch(data);

            return data.ResultList;
        }
    }
}
