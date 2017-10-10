using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class KdSplitResult
    {
        public int CuttingDimension;
        public float CuttingValue;
        public IPoint[] PoingsOnLeftSide;
        public IPoint[] PoingsOnRightSide;
    }
}
