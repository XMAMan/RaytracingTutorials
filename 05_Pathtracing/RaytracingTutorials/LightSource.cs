using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class LightSource
    {
        private Triangle[] triangles;
        public float EmissionPerArea { get; private set; }

        public LightSource(Triangle[] triangles, float emission)
        {
            this.triangles = triangles;
            this.EmissionPerArea = emission / triangles.Sum(x => x.Area);
            triangles.ToList().ForEach(x => x.AssociatedLightSource = this);
        }
    }
}
