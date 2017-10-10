using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class LightSource
    {
        private Triangle[] triangles;
        public float Emission { get; private set; }

        public LightSource(Triangle[] triangles, float emission)
        {
            this.triangles = triangles;
            this.Emission = emission;
            triangles.ToList().ForEach(x => x.AssociatedLightSource = this);
        }

        public LightSourcePoint GetRandomPointOnLightSource(Random rand)
        {
            var triangle = this.triangles[rand.Next(this.triangles.Length)];
            Vector position = triangle.GetRandomPointOnSurface(rand);

            return new LightSourcePoint()
            {
                Position = position,
                Color = triangle.Color * this.Emission,
                Normal = triangle.Normal
            };
        }
    }
}
