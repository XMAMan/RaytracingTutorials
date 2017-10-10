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

        public LightSourcePoint GetRandomPointOnLightSource(Random rand)
        {
            var triangle = this.triangles[rand.Next(this.triangles.Length)];
            Vector position = triangle.GetRandomPointOnSurface(rand);

            float triangleSelectionPdf = 1.0f / this.triangles.Length;
            float trianglePdfA = 1.0f / triangle.Area;

            return new LightSourcePoint()
            {
                Position = position,
                PdfA = triangleSelectionPdf * trianglePdfA,
                Color = triangle.Color * this.EmissionPerArea,
                Normal = triangle.Normal
            };
        }
    }
}
