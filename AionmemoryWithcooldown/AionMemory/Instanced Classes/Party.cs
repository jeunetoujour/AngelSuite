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
    /// <summary>Contains information about party.</summary>
    public class Party
    {
        /// <summary>Number of players in party.</summary>
        public int Members;
        /// <summary>Party member data.</summary>
        public PartyMember[] Member = new PartyMember[5];

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Party()
        {
            this.Members = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8E4D3C));
            for (int i = 1; i <= this.Members; i++)
            {
                this.Member[i-1] = new PartyMember(i);
            }
        }

        /// <summary>
        /// Reads data from memory and sets struct values accordingly.
        /// </summary>
        public void Update()
        {
            this.Members = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8E4D3C));
            for (int i = 0; i < this.Members; i++)
            {
                this.Member[i].Update();
            }
        }
    }
}
