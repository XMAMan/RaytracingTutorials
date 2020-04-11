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
        public float PdfA { get; private set; } //This is the Pdf with Respect to the surface Area for each sampled point, if you use equal-sampling

        public LightSource(Triangle[] triangles, float emission)
        {
            this.triangles = triangles;
            this.EmissionPerArea = emission / triangles.Sum(x => x.Area);
            this.PdfA = 1.0f / triangles.Sum(x => x.Area);
            triangles.ToList().ForEach(x => x.AssociatedLightSource = this);
        }

        public IntersectionPoint SampleRandomPointOnLightSource(Random rand)
        {
            int index = rand.Next(this.triangles.Length);
            Vector position = this.triangles[index].GetRandomPointOnSurface(rand);

            return new IntersectionPoint(position, this.triangles[index], 0);
        }
    }
}
