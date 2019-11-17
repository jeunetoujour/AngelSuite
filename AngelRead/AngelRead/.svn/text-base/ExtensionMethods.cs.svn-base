/*
    Copyright © 2009, AionHacker.net
    All rights reserved.
    http://www.aionhacker.net
    http://www.assembla.com/spaces/AionMemory


    This file is part of AionMemory.

    AionMemory is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AionMemory is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AionMemory.  If not, see <http://www.gnu.org/licenses/>.
*/

// Standard Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AionMemory
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="f">Rotation in degrees.</param>
        /// <param name="east">Assumes degrees are East 0 if TRUE, else compass degrees (North 0).</param>
        /// <returns>Rotation in radians.</returns>
        public static float ToRadians(this float f, bool east)
        {
            if (east)
                return (float)Math.PI * f.ToNorth() / (float)180.0;
            else
                return (float)Math.PI * f / (float)180.0;
        }

        /// <summary>
        /// Converts AION's East 0 degrees representation of rotation to compass degrees (North 0).
        /// </summary>
        /// <param name="f">Rotation in East 0.</param>
        /// <returns>Rotation in compass degrees.</returns>
        public static float ToNorth(this float f)
        {
            if (f < 270)
                return f + 90;
            else
                return (f + 90) - 360;
        }

        /// <summary>
        /// Converts compass degrees (North 0) to AION's East 0 degrees representation of rotation.
        /// </summary>
        /// <param name="f">Rotation in compass degrees.</param>
        /// <returns>Rotation in North 0.</returns>
        public static float ToEast(this float f)
        {
            if (f > -270)
                return f - 90;
            else
                return (f - 90) + 360;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="f">Rotation in radians.</param>
        /// <param name="east">Return degrees in East 0 if TRUE, else in compass degrees (North 0).</param>
        /// <returns>Rotation in degrees.</returns>
        public static float ToDegrees(this float f, bool east)
        {
            float rot = f * (float)180.0 / (float)Math.PI;
            if (east)
                return rot.ToEast();
            else
                return rot;
        }
    }
}
