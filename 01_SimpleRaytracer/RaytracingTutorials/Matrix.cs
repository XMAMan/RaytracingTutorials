using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    static class Matrix
    {
        public static Vector MatrixVectorMultiplication(float[] M, Vector v)
        {
            Vector res = new Vector(M[0] * v.X + M[3] * v.Y + M[6] * v.Z,
                                    M[1] * v.X + M[4] * v.Y + M[7] * v.Z,
                                    M[2] * v.X + M[5] * v.Y + M[8] * v.Z);						
           
            return res;
        }
    }
}
