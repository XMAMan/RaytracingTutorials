using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class MonteCarloIntegration
    {
        //Summe aller Würfelzahlen mit 10 Samples
        public static string Example1()
        {
            Random rand = new Random(0);

            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6 }; 
            
            int sum1 = numbers.Sum();

            //........

            float sum2 = 0;
            int N = 10; //Samplecount
            float pdf = 1.0f / 6;
            for (int i = 0; i < N; i++)
            {
                int x = rand.Next(6); //Erzeuge Zufallszahl im Bereich von  0-5
                sum2 += numbers[x] / pdf; //numbers entspricht f(x) = x + 1
            }
            sum2 /= N;

            string result = sum1 + " " + sum2;
            return result;
        }

        //Integral von cos(x) im Bereich x=0..PI/2 (Ohne Importancesampling)
        public static string Example2()
        {
            Random rand = new Random(0);
            float sum = 0;
            int sampleCount = 1000;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)(rand.NextDouble() * Math.PI / 2);
                float pdf = 1.0f / (float)(Math.PI / 2);
                sum += (float)Math.Cos(x) / pdf;
            }

            sum /= sampleCount;

            return sum.ToString(); 
        }

        //Integral von cos(x) im Bereich x=0..PI/2 (Mit Importancesampling mit gerader Linie)
        public static string Example3()
        {
            Random rand = new Random(0);
            int sampleCount = 10;

            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)(Math.Sqrt((rand.NextDouble() - 1) / (-4 / (Math.PI * Math.PI))) + Math.PI / 2);
                float pdf = (float)(4 / Math.PI - 8 / (Math.PI * Math.PI) * x);
                sum += (float)Math.Cos(x) / pdf;
            }
            sum /= sampleCount;
            string result = sum.ToString();
            return result;
        }

        public static string Example4()
        {
            Random rand = new Random(0);
            int sampleCount = 10;

            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)Math.Asin(rand.NextDouble());
                float pdf = (float)Math.Cos(x);
                sum += (float)Math.Cos(x) / pdf; //Cos(x) / Cos(x) -> Kürzt sich zu 1
            }

            sum /= sampleCount;
            string result = sum.ToString();
            return result;
        }

        //.................................
        public static void Example22()
        {
            Random rand = new Random(0);

            int[] numbers = new int[20];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rand.Next(100);
            }

            int sum1 = numbers.Sum();

            //........

            string sum2 = sum1 + "\n....\n";
            int sampleCount = 1;
            for (int i = 0; i < 20; i++)
            {
                sum2 += sampleCount + ": " + GetMonteCarloSum(numbers, sampleCount, rand) + "\n";
                sampleCount *= 2;
            }

            string result = sum1 + " " + sum2;
        }

        private static float GetMonteCarloSum(int[] numbers, int sampleCount, Random rand)
        {
            float pdf = 1.0f / numbers.Length;

            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                sum += numbers[rand.Next(numbers.Length)] / pdf;
            }

            return sum / sampleCount;
        }

        public static string Example234()
        {
            Random rand = new Random(0);

            int sampleCount = 1;
            string sum = "";
            for (int i = 0; i < 20; i++)
            {
                sum += sampleCount + ": " + Example4(sampleCount, rand) + "\n";
                //sampleCount *= 2;
                sampleCount++;
            }

            return sum;
        }

        //Integral von cos(x) im Bereich x=0..PI/2 (Ohne Importancesampling)
//1: 0,6548422
//2: 0,502185
//3: 1,165795
//4: 0,7302852
//5: 0,9276736
//6: 0,7514502
//7: 0,6628547
//8: 1,0144
//9: 1,033252
//10: 1,148234
//11: 0,6615029
//12: 0,8510067
//13: 0,8358173
//14: 1,081396
//15: 1,010166
//16: 1,069548
//17: 0,9140706
//18: 0,9428813
//19: 1,139536
//20: 0,7930642
        private static float Example2(int sampleCount, Random rand)
        {
            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)(rand.NextDouble() * Math.PI / 2);
                float pdf = 1.0f / (float)(Math.PI / 2);
                sum += (float)Math.Cos(x) / pdf;
            }
            return sum / sampleCount;
        }

        //Integral von cos(x) im Bereich x=0..PI/2 (Mit Importancesampling mit gerader Linie)
        //Linie: y = x * (-1 / PI/2) + 1
        //       y = 1 - 2/PI*x 
        //       Stammfunktion von der Linie = y = x - 1/PI * x² 
        //      Integral im Bereich 0..PI/2 = [PI/2 - 1/PI * (PI/2)²] - [0] = PI/2 - 1/PI * PI²/4 = PI/2 - PI/4 = PI/4
        //      Pdf = 4/PI - 8/PI² * x
        // CDS => y = 4/PI * x - 4/PI² * x²

        //4/pi * x - 4/(pi * Pi) * x * x

        //http://www.onlinemathe.de/forum/Umkehrfunktion-von-x2-6x-1-berechnen
        //http://www.mathebibel.de/quadratische-ergaenzung
        // Inverse von CDS => y = -4/PI² * (-PI * x + x²)   
        //                 => y = -4/PI² * (x² - PI * x + (-PI/2)² - (-PI/2)²)
        //                 => y = -4/PI² * ((x - PI/2)² - PI² / 4)
        //                 => y = (x - PI/2)² * (-4/PI²) + 1
        //                 => (y - 1) / (-4/PI²) = (x - PI/2)²
        //                 => Sqrt( (y - 1) / (-4/PI²) ) = x - PI/2
        //                 => Sqrt( (y - 1) / (-4/PI²) ) + PI/2 = x

//1: 1,099429
//2: 1,13121
//3: 0,9705912
//4: 1,068832
//5: 1,029426
//6: 1,054531
//7: 1,080236
//8: 0,9976196
//9: 0,9875166
//10: 0,9706128
//11: 1,084198
//12: 1,039945
//13: 1,045907
//14: 0,9829416
//15: 0,9923815
//16: 0,980034
//17: 1,023347
//18: 1,017908
//19: 0,9530557
//20: 1,053975
        private static float Example3(int sampleCount, Random rand)
        {
            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)(Math.Sqrt((rand.NextDouble() - 1) / (-4 / (Math.PI * Math.PI))) + Math.PI / 2);
                float pdf = (float)(4 / Math.PI - 8 / (Math.PI * Math.PI) * x);
                sum += (float)Math.Cos(x) / pdf;
            }
            return sum / sampleCount;
        }

        //Integral von cos(x) im Bereich x=0..PI/2 (Mit Importancesampling mit Cos(x))
        //Stammfunktion: Sin(x)
        //      Integral im Bereich 0..PI/2 = 1 -> Pdf-Normilisierungskonstante ist bereits 1. Somit ist Pdf = cos(x)
        //      CDS = Sin(x)
        //      Inverse von CDS = Sinh(x)
        private static float Example4(int sampleCount, Random rand)
        {
            float sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                float x = (float)Math.Sinh(rand.NextDouble());
                float pdf = (float)Math.Cos(x);
                sum += (float)Math.Cos(x) / pdf; //Cos(x) / Cos(x) -> Kürzt sich zu 1
            }
            return sum / sampleCount;
        }
    }
}
