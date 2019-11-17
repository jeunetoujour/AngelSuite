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

// Custom Includes
using MemoryLib;

// Standard Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AionMemory
{
    /// <summary>
    /// Read various values from AION's memory.
    /// </summary>
    public class Read
    {
        /// <summary>
        /// Reads macro text from AION's memory for the current player.  *NOT FINISHED*
        /// </summary>
        /// <param name="e">Current player structure.</param>
        /// <param name="slot">Macro slot.</param>
        public static void Macro(ref Player e, int slot)
        {
            e.Macro1 = Memory.ReadString(Process.handle, Process.macro1, 255, true);
        }
    }
}
