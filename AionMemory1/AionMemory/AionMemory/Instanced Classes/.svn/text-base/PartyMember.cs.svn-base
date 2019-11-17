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
    /// <summary>Contains information about party members.</summary>
    public class PartyMember : Entity
    {
        /// <summary>Player index.</summary>
        public int Index;
        /// <summary>Player's class.</summary>
        public byte Class;
        /// <summary>Player's current HP.</summary>
        new public int Health;
        /// <summary>Player's maximum HP</summary>
        public int MaxHealth;
        /// <summary>Player's current mana.</summary>
        public int MP;
        /// <summary>Player's maximum mana.</summary>
        public int MaxMP;
        /// <summary>Player's remaining flight time (in milliseconds).</summary>
        public int FlightTime;
        /// <summary>Player's maximum flight time (in milliseconds).</summary>
        public int MaxFlightTime;
        public int NextPartyMember;
        public int PreviousPartyMember;
        public int ThisPartyMember;

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        /// <param name="i">Index of the player in the party.</param>
        public PartyMember(int i)
        {
            this.Index = i;
            this.Update();
        }

        /// <summary>
        /// Reads data from memory and sets class member values accordingly.
        /// </summary>
        new public void Update()
        {
            int i;
            int PtrPlayer = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8E4D38));

            for (i = 0; i < Index; i++)
            {
                PtrPlayer = Memory.ReadInt(Process.handle, PtrPlayer);
            }

            ThisPartyMember = PtrPlayer;
            NextPartyMember = Memory.ReadInt(Process.handle, (PtrPlayer));
            PreviousPartyMember = Memory.ReadInt(Process.handle, (PtrPlayer + 0x4));
            PtrPlayer = Memory.ReadInt(Process.handle, (PtrPlayer + 0x8));

            if (PtrPlayer != 0)
            {
                this.Name = Memory.ReadString(Process.handle, (PtrPlayer + 0x3A), 32, true);
                this.Class = Memory.ReadByte(Process.handle, (PtrPlayer + 0x34));
                this.Level = Memory.ReadByte(Process.handle, (PtrPlayer + 0x36));
                this.ID = Memory.ReadInt(Process.handle, (PtrPlayer + 0x4));
                this.Health = Memory.ReadInt(Process.handle, (PtrPlayer + 0x8));
                this.MaxHealth = Memory.ReadInt(Process.handle, (PtrPlayer + 0xC));
                this.MP = Memory.ReadInt(Process.handle, (PtrPlayer + 0x14));
                this.MaxMP = Memory.ReadInt(Process.handle, (PtrPlayer + 0x10));
                this.FlightTime = Memory.ReadInt(Process.handle, (PtrPlayer + 0x1C));
                this.MaxFlightTime = Memory.ReadInt(Process.handle, (PtrPlayer + 0x18));
                this.X = Memory.ReadFloat(Process.handle, (PtrPlayer + 0x28));
                this.Y = Memory.ReadFloat(Process.handle, (PtrPlayer + 0x2C));
                this.Z = Memory.ReadFloat(Process.handle, (PtrPlayer + 0x30));
            }
            else
            {
                SetZero();
            }
        }

        /// <summary>
        /// Sets all class member values to 0.
        /// </summary>
        public void SetZero()
        {
            this.Name = "";
            this.Class = 0;
            this.Level = 0;
            this.ID = 0;
            this.Health = 0;
            this.MaxHealth = 0;
            this.MP = 0;
            this.MaxMP = 0;
            this.FlightTime = 0;
            this.MaxFlightTime = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }
}
