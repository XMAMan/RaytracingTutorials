using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class LightSourcePoint
    {
        public Vector Position { get; set; }
        public float PdfA;
        public Vector Color;
        public Vector Normal { get; set; }
    }
}
