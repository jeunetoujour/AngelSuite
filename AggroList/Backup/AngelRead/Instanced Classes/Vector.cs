using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    public class Vec2D
    {

    }

    public class Vec3D
    {
        const float VECTOR_SMALL_NUMBER = 0.001f;

        public float X;
        public float Y;
        public float Z;

        public Vec3D(float GlobalVal)
        {
            X = Y = Z = GlobalVal;
        }

        public Vec3D(float inX, float inY, float inZ)
        {
            X = inX;
            Y = inY;
            Z = inZ;
        }

        //addition operator
        public static Vec3D operator +(Vec3D v1, Vec3D v2)
        {
            return new Vec3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        //subtraction operator
        public static Vec3D operator -(Vec3D v1, Vec3D v2)
        {
            return new Vec3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        //division operator
        public static Vec3D operator /(Vec3D v1, float Divisor)
        {
            return new Vec3D(v1.X / Divisor, v1.Y / Divisor, v1.Z / Divisor);
        }

        //multiplication operator
        public static Vec3D operator *(Vec3D v1, float Coef)
        {
            return new Vec3D(v1.X * Coef, v1.Y * Coef, v1.Z * Coef);
        }

        // return squared size of this vector
        public float SizeSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }
        //return size (magnitude) of this vector
        public float Size
        {
            get { return (float)System.Math.Sqrt(this.SizeSquared); }
        }
        //
        public float SizeSquared2D
        {
            get { return X * X + Y * Y; }
        }
        // return 2-D (discounting Z) size of this vector
        public float Size2D
        {
            get { return (float)System.Math.Sqrt(this.SizeSquared2D); }
        }


        // return magnitude of this vector
        public float Mag
        {
            get { return Size; }
        }

        // normalize this vector so that it is of length 1
        public void Normalize()
        {
            float Magnitude = Size;

            if (Magnitude < VECTOR_SMALL_NUMBER)
            {
                return;
            }

            Vec3D thisc = this / Magnitude;
            this.X = thisc.X;
            this.Y = thisc.Y;
            this.Z = thisc.Z;
        }

        // return a normalized version of this vector
        public Vec3D Normal
        {
            get
            {
                Vec3D ret = this;
                ret.Normalize();

                return ret;
            }
        }

        // dot product operator
        // http://en.wikipedia.org/wiki/Dot_product
        public static float operator |(Vec3D v1, Vec3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        // cross product operator
        // http://en.wikipedia.org/wiki/Cross_product
        public static Vec3D operator ^(Vec3D v1, Vec3D v2)
        {
            return new Vec3D(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static bool operator ==(Vec3D v1, Vec3D v2)
        {
            return (System.Math.Abs(v1.X - v2.X) < VECTOR_SMALL_NUMBER &&
                    System.Math.Abs(v1.Y - v2.Y) < VECTOR_SMALL_NUMBER &&
                    System.Math.Abs(v1.Z - v2.Z) < VECTOR_SMALL_NUMBER);
        }

        public static bool operator !=(Vec3D v1, Vec3D v2)
        {
            return !(v1==v2);
        }

        // returns TRUE of vectors are parallel
        public static bool Parallel(Vec3D v1, Vec3D v2)
        {
            float Dot = v1 | v2;
            if (System.Math.Abs(1.0f - Dot) < VECTOR_SMALL_NUMBER)
            {
                return true;
            }

            return false;
        }
        public bool Parallel(Vec3D vOther)
        {
            return Parallel(this, vOther);
        }

        // returns the angle between two vectors
        public static float AngleBetweenVectors(Vec3D v1, Vec3D v2)
        {
            return (float)System.Math.Acos(v1 | v2);
        }

        /// <summary>this will rotate this vector about the supplied axis according to the angle supplied, and return the rotated result</summary>
        /// <param name="Axis">Axis to rotate about</param>
        public Vec3D RotateAboutAxis(Vec3D Axis, float Angle)
        {
            Vec3D Result = new Vec3D(0f);

            Axis.Normalize();

            float cos = (float)System.Math.Cos(Angle);
            float sin = (float)System.Math.Sin(Angle);

            Result.X += (cos + (1f - cos) * Axis.X * Axis.X) * X;
            Result.X += ((1f - cos) * Result.X * Result.Y - Result.Z * sin) * Axis.Y;
            Result.X += ((1f - cos) * Result.X * Result.Z + Result.Y * sin) * Axis.Z;
                
            Result.Y += ((1f - cos) * Result.X * Result.Y + Result.Z * sin) * Axis.X;
            Result.Y += (cos + (1f - cos) * Result.Y * Result.Y) * Axis.Y;
            Result.Y += ((1f - cos) * Result.Y * Result.Z - Result.X * sin) * Axis.Z;

            Result.Z += ((1f - cos) * Result.X * Result.Z - Result.Y * sin) * Axis.X;
            Result.Z += ((1f - cos) * Result.Y * Result.Z + Result.X * sin) * Axis.Y;
            Result.Z += (cos + (1f - cos) * Result.Z * Result.Z) * Axis.Z;

            return Result;
        }

           /// <summary>
           /// this will mirror this vector about the supplied normal
           /// </summary>
           /// <param name="Normal">normal to mirror by (e.g. normal of the plane we are reflecting)</param>
           /// <returns></returns>
        public Vec3D Mirror(Vec3D Normal)
        {
            float dot = this | Normal;
            return this - Normal * (2f * dot);
        }        
    }
}
