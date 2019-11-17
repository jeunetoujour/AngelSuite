using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>Represents a point in 3D space.</summary>
    public class Location : ILocation3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Location(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
