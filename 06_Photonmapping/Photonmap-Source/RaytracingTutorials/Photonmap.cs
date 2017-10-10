using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Photonmap
    {
        private IntersectionFinder intersectionFinder;
        private LightSource lightSource;
        private int photonCount;
        private KdTree kdTree;

        public Photonmap(IntersectionFinder intersectionFinder, LightSource lightSource, int photonCount)
        {
            this.intersectionFinder = intersectionFinder;
            this.lightSource = lightSource;
            this.photonCount = photonCount;

            Random rand = new Random(0);

            List<Photon> photons = new List<Photon>();
            for (int i = 0; i < photonCount; i++)
            {
                photons.AddRange(TracePhoton(rand));
            }

            this.kdTree = new KdTree(photons.Cast<IPoint>().ToArray());
        }

        private List<Photon> TracePhoton(Random rand)
        {
            var lightPoint = this.lightSource.GetRandomPointOnLightSource(rand);
            Vector direction = DiffuseBrdf.SampleDirection(lightPoint.Normal, rand);
            float pdfW = DiffuseBrdf.PdfW(lightPoint.Normal, direction);
            float lambda = Vector.Dot(lightPoint.Normal, direction);
            Vector pathWeight = lightPoint.Color * lambda / (lightPoint.PdfA * pdfW);
            Ray ray = new Ray(lightPoint.Position, direction);
           
            int maxPathLength = 5;

            List<Photon> photons = new List<Photon>();

            for (int i = 0; i < maxPathLength; i++)
            {
                var point = this.intersectionFinder.GetIntersectionPoint(ray);
                if (point == null) return photons;

                photons.Add(new Photon(point.Position, point.Normal, pathWeight / this.photonCount, ray.Direction));

                //Abbruch mit Russia Rollete
                float continuationPdf = Math.Min(1, point.Color.Max());
                if (rand.NextDouble() >= continuationPdf)
                    return photons; // Absorbation

                Vector newDirection = DiffuseBrdf.SampleDirection(point.Normal, rand);
                pathWeight = Vector.Mult(pathWeight * Vector.Dot(point.Normal, newDirection) / DiffuseBrdf.PdfW(point.Normal, newDirection), DiffuseBrdf.Evaluate(point)) / continuationPdf;
                ray = new Ray(point.Position, newDirection);
            }

            return photons;
        }

        public List<Photon> SearchPhotons(IntersectionPoint searchPoint, float searchRadius)
        {
            float rSqr = searchRadius * searchRadius;
            return this.kdTree.FixedRadiusSearch(searchPoint.Position, searchRadius).Cast<Photon>().Where(x => Vector.Dot(x.Normal, searchPoint.Normal) > 0).ToList();
        }
    }

    class Photon : IPoint
    {
        public float this[int key]
        {
            get
            {
                return this.Position[key];
            }
        }

        public Vector Position { get; private set; }
        public Vector Normal { get; private set; }
        public Vector PathWeight { get; private set; }
        public Vector DirectionToThisPoint { get; private set; }

        public Photon(Vector position, Vector normal, Vector pathWeight, Vector directionToThisPoint)
        {
            this.Position = position;
            this.Normal = normal;
            this.PathWeight = pathWeight;
            this.DirectionToThisPoint = directionToThisPoint;
        }
    }
}
