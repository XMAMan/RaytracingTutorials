using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class LightSourcePoint : IVertexPoint
    {
        public Vector Position { get; set; }
        public Vector Color;
        public Vector Normal { get; set; }
    }
}
