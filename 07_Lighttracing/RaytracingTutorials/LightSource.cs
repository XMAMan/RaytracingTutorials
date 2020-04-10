using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class LightSourcePoint
    {
        public Vector Position { get; set; }
        public Vector Normal { get; set; }
        public float PdfA { get; set; } //PDF from Position with Respect to Surface Area
        public float EmissionPerArea { get; set; }
    }

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

        public LightSourcePoint SampleRandomPointOnLightSource(Random rand)
        {
            int index = rand.Next(this.triangles.Length);
            float selectionPdf = 1.0f / this.triangles.Length;              //Pdf for sampling index
            float triangleSamplingPdfA = 1.0f / this.triangles[index].Area; //PdfA for Sampling Point on Triangle

            return new LightSourcePoint()
            {
                Position = this.triangles[index].GetRandomPointOnSurface(rand),
                Normal = this.triangles[index].Normal,
                PdfA = selectionPdf * triangleSamplingPdfA,
                EmissionPerArea = this.EmissionPerArea
            };
        }
    }
}
