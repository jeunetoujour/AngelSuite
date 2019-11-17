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
//using System.Windows.Forms;//using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace AionMemory
{
    
    /// <summary>Contains information about target.</summary>
    public class Target : Entity
    {
        /// <summary>TRUE if target has a target.</summary>
        public bool HasTarget;
        /// <summary>TRUE if target is lootable.</summary>
        public bool IsLootable
        {
            get
            {
                _IsLootable = false;
                if (this.IsDead)
                {
                    //Memory.ReadByte(Process.handle, IS_LOOTABLE_ADDRESS);
                    _IsLootable = true;
                }
                return _IsLootable;
            }
        }
        private bool _IsLootable;
        /// <summary>Pointer to target.</summary>
        public int PtrTarget;
        /// <summary>Targets's type.</summary>
        new public eType Type
        {
            get
            {
                if (_Type168 == 0 && _Type18 == 2)
                    return eType.FriendlyNPC;
                else
                    return (eType)_Type168;
            }
        }
        private int _Type18;
        private int _Type168;

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Target()
        {
            this.Update();
        }

        public void UpdateID()
        {
            this.PtrTarget = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0x636BBC));
            this.PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrTarget + 0x1C4));
            this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x20));
            this.TargetID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x284));
            this.HasTarget = (TargetID != 0);
        }
        
        /// <summary>
        /// Reads values from AION's memory and sets class member values accordingly.
        /// </summary>
        new public void Update()
        {
            try
            {
                this.PtrTarget = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0x636BBC));
                if (PtrTarget != 0)
                {
                    this.PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrTarget + 0x1C4));//Memory.ReadInt(Process.handle, Process.Modules.Game + 0x62ED40);       
                    //this.PtrEntity = Magic.SMemory.ReadInt(Process.handle, (uint)(PtrTarget + 0x1C4)); 
                    //this.Attitude = (eAttitude)Magic.SMemory.ReadInt(Process.handle, (uint)(PtrEntity + 0x1C));//Memory.ReadInt(Process.handle, 0x62ED5C);//(PtrEntity + 0x1C));
                    this.Attitude = (eAttitude)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1C));//Memory.ReadInt(Process.handle, 0x62ED5C);//(PtrEntity + 0x1C));
                    this.Class = (eClass)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x19C));
                    this.Health = Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x34));
                    this.TargetID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x284)); 
                    this.HasTarget = (TargetID != 0);
                    this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x20));//Memory.ReadInt(Process.handle, (PtrEntity + 0x20));
                    this.Stance = (eStance)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x20C));
                    this.IsDead = ((int)Stance == 7);
                    this.Level = Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x32));
                    this.Name = Memory.ReadString(Process.handle, (uint)(PtrEntity + 0x36), 64, true);//, 64, true);
                    this._Type18 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x18));
                    this._Type168 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x168));
                    this.X = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x28));//Memory.ReadFloat(Process.handle, tarx);//(PtrTarget + 0x28));
                    this.Y = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x2C));//(PtrTarget + 0x2C));
                    this.Z = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x30));//(PtrTarget + 0x30));
                }
                else
                    SetZero();
            }
            catch 
                (Exception ) { }
        }

        /// <summary>
        /// Sets all class member values to 0.
        /// </summary>
        private void SetZero()
        {
            this.Attitude = 0;
            this.Class = 0;
            this.Health = 0;
            this.HasTarget = false;
            this.ID = 0;
            this.IsDead = false;
            this.Level = 0;
            this.Name = "";
            this.Stance = 0;
            this.TargetID = 0;
            this._Type18 = 0;
            this._Type168 = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }
}
