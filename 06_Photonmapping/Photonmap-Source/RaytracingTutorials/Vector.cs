using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class Vector : IPoint
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public float this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                }
                throw new ArgumentException("key is out of range 0-2");
            }
            set
            {
                switch (key)
                {
                    case 0: this.X = value; break;
                    case 1: this.Y = value; break;
                    case 2: this.Z = value; break;
                }
            }
        }


        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v, float scale)
        {
            return new Vector(v.X * scale, v.Y * scale, v.Z * scale);
        }
        public static Vector operator *(float scale, Vector v)
        {
            return new Vector(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.X / f, v.Y / f, v.Z / f);
        }

        public static float Dot(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector Cross(Vector v1, Vector v2)
        {
            return new Vector(v1.Y * v2.Z - v2.Y * v1.Z, -v1.X * v2.Z + v2.X * v1.Z, v1.X * v2.Y - v2.X * v1.Y);
        }

        public static Vector Mult(Vector v1, Vector v2)
        {
            return new Vector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z); 
        }

        public float SqrLength()
        {
            return X * X + Y * Y + Z * Z;
        }

        public Vector Normalize()
        {
            return this * (1 / Length());
        }

        public static Vector operator -(Vector v)
        {
            return -1 * v;
        }

        public float Max()
        {
            return Math.Max(Math.Max(this.X, this.Y), this.Z);
        }
    }
}
