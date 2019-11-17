
using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    public static class Tools
    {
        /// <summary>
        /// Finds the distance between this entity and another entity on a 2D surface.
        /// </summary>
        /// <param name="location1">this</param>
        /// <param name="location2">Entity.</param>
        /// <returns>Distance</returns>
        public static double Distance2D(this ILocation2D location1, ILocation2D location2)
        {
            return Distance2D(location1.X, location1.Y, location2.X, location2.Y);
        }
        /// <summary>
        /// Finds the distance between this entity and a point on a 2D surface.
        /// </summary>
        /// <param name="location1">this</param>
        /// <param name="x2">The point on the x-axis.</param>
        /// <param name="y2">The point on the y-axis.</param>
        /// <returns>Distance</returns>
        public static double Distance2D(this ILocation2D location1, float x2, float y2)
        {
            return Distance2D(location1.X, location1.Y, x2, y2);
        }
        /// <summary>
        /// Finds the distance between two entities on a 2D surface.
        /// </summary>
        /// <param name="x1">The point on the x-axis of the first entity.</param>
        /// <param name="x2">The point on the x-axis of the second entity.</param>
        /// <param name="y1">The point on the y-axis of the first entity.</param>
        /// <param name="y2">The point on the y-axis of the second entity.</param>
        /// <returns>Distance</returns>
        public static double Distance2D(float x1, float y1, float x2, float y2)
        {
            //     ______________________
            //d = √ (x2-x1)^2 + (y2-y1)^2
            //

            // Our end result
            double result = 0;
            // Take x2-x1, then square it
            double part1 = System.Math.Pow((x2 - x1), 2);
            // Take y2-y1, then sqaure it
            double part2 = System.Math.Pow((y2 - y1), 2);
            // Add both of the parts together
            double underRadical = part1 + part2;
            // Get the square root of the parts
            result = System.Math.Sqrt(underRadical);
            // Return our result
            return result;
        }
        public static float Distance2D(Vec3D v1, Vec3D v2)
        {
            return (v1 - v2).Size2D;
        }

        /// <summary>
        /// Finds the distance between this entity and another entity on a 3D surface.
        /// </summary>
        /// <param name="location1">this</param>
        /// <param name="location2">Entity.</param>
        /// <returns>Distance</returns>
        public static double Distance3D(this ILocation3D location1, ILocation3D location2)
        {
            return Distance3D(location1.X, location1.Y, location1.Z, location2.X, location2.Y, location2.Z);
        }
        /// <summary>
        /// Finds the distance between this entity and a point on a 3D surface.
        /// </summary>
        /// <param name="location1">this</param>
        /// <param name="x2">The point on the x-axis.</param>
        /// <param name="y2">The point on the y-axis.</param>
        /// <param name="z2">The point on the z-axis.</param>
        /// <returns></returns>
        public static double Distance3D(this ILocation3D location1, float x2, float y2, float z2)
        {
            return Distance3D(location1.X, location1.Y, location1.Z, x2, y2, z2);
        }
        /// <summary>
        /// Finds the distance between two entities on a 3D surface.
        /// </summary>
        /// <param name="x1">The point on the x-axis of the first entity.</param>
        /// <param name="x2">The point on the x-axis of the second entity.</param>
        /// <param name="y1">The point on the y-axis of the first entity.</param>
        /// <param name="y2">The point on the y-axis of the second entity.</param>
        /// <param name="z1">The point on the z-axis of the first entity.</param>
        /// <param name="z2">The point on the z-axis of the second entity.</param>
        /// <returns>Distance</returns>
        public static double Distance3D(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            //     __________________________________
            //d = √ (x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2
            //

            // Our end result
            double result = 0;
            // Take x2-x1, then square it
            double part1 = System.Math.Pow((x2 - x1), 2);
            // Take y2-y1, then sqaure it
            double part2 = System.Math.Pow((y2 - y1), 2);
            // Take z2-z1, then square it
            double part3 = System.Math.Pow((z2 - z1), 2);
            // Add both of the parts together
            double underRadical = part1 + part2 + part3;
            // Get the square root of the parts
            result = System.Math.Sqrt(underRadical);
            // Return our result
            return result;
        }
        public static float Distance3D(Vec3D v1, Vec3D v2)
        {
            return (v1 - v2).Size;
        }
    }
}
