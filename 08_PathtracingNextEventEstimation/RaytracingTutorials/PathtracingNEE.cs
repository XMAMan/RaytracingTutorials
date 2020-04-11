using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RaytracingTutorials
{
    //Pathtracing with Next Event Estimation and Multipe Importance Sampling - implementation in c#
    class PathtracingNEE
    {
        private CornellBoxSceneData data;
        private IntersectionFinder intersectionFinder;
        private int width;
        private int height;

        public PathtracingNEE(int width, int height)
        {
            this.data = new CornellBoxSceneData(width, height);
            this.intersectionFinder = new IntersectionFinder(data.Triangles);
            this.width = width;
            this.height = height;
        }

        public Bitmap CreateBitmap()
        {
            Bitmap image = new Bitmap(this.width, this.height);

            Random rand = new Random(0);

            int sampleCount = 1;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Vector sum = new Vector(0, 0, 0);
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sum += EstimateColorWithPathtracingWithNextEventEstimation(x, y, rand);
                    }
                    sum /= sampleCount;
                    image.SetPixel(x, y, VectorToColor(sum));
                }

            return image;
        }

        private Vector EstimateColorWithPathtracingWithNextEventEstimation(int x, int y, Random rand)
        {
            //This is the sum from all Pathcontributions from all Pathlenghts up to maxPathLength
            //This means Contribution(C L) + Contribution(C D L) + Contribution(C D D L) + Contribution(C D D D L) + Contribution(C D D D D L)
            //For Each Pathlength-Step, the Contribution from Brdf-Sampling and Lightsource-Sampling is added but only one from this two Sampling-Methods 
            //will create a Contribution != 0. If you hit the Lightsource, then you only get the BrdfSampling-Contribution. The LightSource-Sampling-Contribution
            //will be zero because if your running point is on the light and the light don't reflect any light, then you don't can create any more Lightpoints
            //If you are on a Point in the Scene (Not Lightsoure) then only the Contribution from Lightsource-Sampling can be greater zero. The Brdf-Sampling
            //Contribution (Pathtracing) will be zero.
            Vector contributionSum = new Vector(0, 0, 0); 

            Ray primaryRay = data.Camera.GetPrimaryRay(x, y);
            float cosAtCamera = Vector.Dot(primaryRay.Direction, this.data.Camera.Forward);
            Vector pathWeight = new Vector(1, 1, 1) * cosAtCamera;

            Ray ray = primaryRay;
            float pdfW = data.Camera.PdfWForPrimaryRayDirectionSampling(ray.Direction); //This will store the PdfW and Contination Pdf, which was used for Brdf-Sampling

            int maxPathLength = 5;
            for (int i = 0; i < maxPathLength; i++)
            {
                IntersectionPoint point = this.intersectionFinder.GetIntersectionPoint(ray);
                if (point == null) break; //Ray leave the scene

                //Try to add Contribution from Brdf-Sampling (This Brdf-Sampling is also called Pathtracing because you do this for every Pathpoint)
                if (point.IsLocatedOnLightSource && Vector.Dot(point.Normal, -ray.Direction) > 0)
                {
                    //This is the PdfA on the LightPoint, if you would use LightSource-Sampling, to reach from lastPoint(ray.Origin) to current point 'point'
                    float pdfAFromLightSourceSampling = data.LightSource.PdfA;
 
                    //This is the PdfA on the LightPoint, which you get from BrdfSampling to reach rom lastPoint(ray.Origin) to current point 'point'
                    float pdfAFromBrdfSampling = pdfW * Vector.Dot(point.Normal, -ray.Direction) / (ray.Origin - point.Position).SqrLength();

                    //We have used Brdf-Sampling. So the pdfAFromBrdfSampling is in the numerator
                    float misFactor = pdfAFromBrdfSampling / (pdfAFromLightSourceSampling + pdfAFromBrdfSampling);
                    Vector contributionFromBrdfSampling = Vector.Mult(pathWeight, point.Emission) * misFactor;

                    contributionSum += contributionFromBrdfSampling;

                    break; //The light don't reflect any light. So stop here with Pathtracing
                }

                //Try to add Contribution from Lightsource-Sampling (This is the Next Event Estimation-Sampling)
                contributionSum += GetContributionFromLightSourceSampling(point, rand, pathWeight);

                //Stop with Russia Rollete
                float continuationPdf = Math.Min(1, point.Color.Max());
                if (rand.NextDouble() >= continuationPdf)
                    break; // Absorbation                

                //Sample Direction and try to hit to lightSource
                Vector direction = DiffuseBrdf.SampleDirection(point.Normal, rand);
                pdfW = DiffuseBrdf.PdfW(point.Normal, direction) * continuationPdf;
                float cosAtPoint = Math.Max(0, Vector.Dot(point.Normal, direction));

                pathWeight = Vector.Mult(pathWeight, DiffuseBrdf.Evaluate(point)) * cosAtPoint / pdfW; //New Pathweight after Brdf-Sampling
                ray = new Ray(point.Position, direction);
            }

            return contributionSum;
        }

        private Vector GetContributionFromLightSourceSampling(IntersectionPoint point, Random rand, Vector pathWeight)
        {
            //Sample Point on LightSource
            var lightPoint = data.LightSource.SampleRandomPointOnLightSource(rand);
            Vector directionToLight = (lightPoint.Position - point.Position).Normalize();

            //Shadow-Ray-Test: Test is the Point on Lightsource visible from 'point'?
            var lightVisiblePoint = this.intersectionFinder.GetIntersectionPoint(new Ray(point.Position, directionToLight));
            float DistanceForPoint2PointVisibleCheck = 0.0001f; //Magic Number
            bool isVisible = lightVisiblePoint != null && (lightVisiblePoint.Position - lightPoint.Position).Length() < DistanceForPoint2PointVisibleCheck;
            if (isVisible == false) return new Vector(0,0,0); //No contribution, if 'point' is located in shadow

            //Pathcontribution from Lightsource-Sampling
            float cosAtPoint = Math.Max(0, Vector.Dot(point.Normal, directionToLight));
            float cosAtLight = Math.Max(0, Vector.Dot(lightPoint.Normal, -directionToLight));
            float geometryFactor = cosAtPoint * cosAtLight / (lightPoint.Position - point.Position).SqrLength();
            float pdfAFromLightSourceSampling = data.LightSource.PdfA; //This is the PdfA for sampling the lightPoint on the Position lightPoint.Position
            Vector contributionFromLightSampling = Vector.Mult(pathWeight, DiffuseBrdf.Evaluate(point)) * geometryFactor * data.LightSource.EmissionPerArea / pdfAFromLightSourceSampling;

            //Get PdfA from Brdf-Sampling. 
            float pdfW = DiffuseBrdf.PdfW(point.Normal, directionToLight);
            float continuationPdf = Math.Min(1, point.Color.Max());

            //This is the PdfA, to hit the LightSource with Brdf-Sampling and hit the Lightsource on the Position lightPoint.Position
            //The Factor "cosAtLight / (lightPoint.Position - point.Position).SqrLength()" is the pdfW to PdfA-Conversionfactor. 
            //This means, you project to Solid Angle Measure from the Brdf to the surface area measure on the Point on the Light
            float pdfAFromBrdfSampling = pdfW * continuationPdf * cosAtLight / (lightPoint.Position - point.Position).SqrLength();

            //Heare we have used LightSource-Sampling for the Creation of the Point on the LightSource. So we use pdfAFromLightSourceSampling in the numerator
            float misFactor = pdfAFromLightSourceSampling / (pdfAFromLightSourceSampling + pdfAFromBrdfSampling);
            contributionFromLightSampling *= misFactor;

            return contributionFromLightSampling;
        }

        private Color VectorToColor(Vector v)
        {
            Vector c = new Vector(Clamp(v.X), Clamp(v.Y), Clamp(v.Z));
            return Color.FromArgb((int)(c.X * 255), (int)(c.Y * 255), (int)(c.Z * 255));
        }

        private float Clamp(float f)
        {
            if (f < 0) f = 0;
            if (f > 1) f = 1;
            return f;
        }
    }
}
