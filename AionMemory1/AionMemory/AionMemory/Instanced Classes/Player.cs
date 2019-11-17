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
    /// <summary>Contains information about player.</summary>
    public class Player : Entity
    {
        /// <summary>Player's legion.</summary>
        public string Legion;
        /// <summary>Current spell being cast by player.  If 0, no spell is being cast, else value is spell ID.</summary>
        public byte Spell;
        /// <summary>Current experience.</summary>
        public int XP;
        /// <summary>Total experience needed for next level.</summary>
        public int MaxXP;
        /// <summary>Player's current HP.</summary>
        new public int Health;
        /// <summary>Player's maximum HP</summary>
        public int MaxHealth;
        /// <summary>Player's current mana.</summary>
        public int MP;
        /// <summary>Player's maximum mana.</summary>
        public int MaxMP;
        /// <summary>Player's current Divine Power.</summary>
        public Int16 DP;
        /// <summary>Player's maximum Divine Power.</summary>
        public Int16 MaxDP;
        /// <summary>Number of empty slots in all cubes.</summary>
        public int Slots;
        /// <summary>Maximum number of slots in all cubes.</summary>
        public int MaxSlots;
        /// <summary>Player's remaining flight time (in milliseconds).</summary>
        public int FlightTime;
        /// <summary>Player's maximum flight time (in milliseconds).</summary>
        public int MaxFlightTime;
        /// <summary>Player's current Kinah.</summary>
        public int Kinah;
        /// <summary>Current region (ends in \n)</summary>
        public string Region;
        /// <summary>Pointer to Kinah (offset 0x138 from this address)</summary>
        public int PtrKinah;
        /// <summary>Macro text in slot 1.</summary>
        public string Macro1;
        /// <summary>Player's inventory.</summary>
        public Cube Inventory;
        /// <summary>TRUE if player is casting spell.</summary>
        public bool IsCasting;
        public int HasTarget;
        /// <summary>Players's current stance.</summary> 
        new public eStance Stance
        {
            get { return _Stance; }
            set
            {
                _Stance = (eStance)value;
                //Memory.WriteMemory(Process.handle, (PtrEntity + 0x20C), (int)value);
            }
        }
        private eStance _Stance;

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Player()
        {
            this.Update();
        }

        /// <summary>
        /// Reads data from memory and sets class member values accordingly.
        /// </summary>
        new public void Update()
        {
            //this.PtrKinah = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8E3930));
            //this.Name = Magic.SMemory.ReadUnicodeString(Process.handle, (uint)(Process.Modules.Game + 0xA5E718), 32);
            this.Name = Memory.ReadString(Process.handle, (Process.Modules.Game + 0xA5E718), 32,true);
            //this.Legion = Memory.ReadString(Process.handle, (Process.Modules.Game + 0x8E4DB0), 64, true);
            //this.Macro1 = Memory.ReadString(Process.handle, Process.macro1, 255, true);
            this.Level = Memory.ReadByte(Process.handle, (Process.Modules.Game + 0xA276F8));
            this.Spell = Memory.ReadByte(Process.handle, (Process.Modules.Game + 0xA1D4D0));
            this.IsCasting = (Spell != 0);
            this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0xA1CC58));//A1CC58//0xA1CC5C
            this.XP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA27710));
            this.MaxXP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA27700));
            this.Health = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA27720));
            this.MaxHealth = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA2771C));
            this.MP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA27728));
            this.MaxMP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA27724));
            //this.HasTarget = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x62EB84));
            //this.DP = Memory.ReadShort(Process.handle, (Process.Modules.Game + 0x8EEEC6));
            //this.MaxDP = Memory.ReadShort(Process.handle, (Process.Modules.Game + 0x8EEEC4));
            //this.FlightTime = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8EEECC));
            //this.MaxFlightTime = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8EEEC8));
            //this.Slots = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8EEEFC));
            //this.MaxSlots = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0x8EEEF8));
            //this.Kinah = Memory.ReadInt(Process.handle, (PtrKinah + 0x138));
            this.X = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA1CC68));
            this.Y = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA1CC6C));
            this.Z = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA1CC70));
            //this.Region = Memory.ReadString(Process.handle, (Process.Modules.Game + 0x8E6D68), 64, true);
            this.Rotation = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA1C8C8));
            //this._Stance = (eStance)Magic.SMemory.ReadInt(Process.handle, (uint)(PtrEntity + 0x20C));
        }
    }
}
