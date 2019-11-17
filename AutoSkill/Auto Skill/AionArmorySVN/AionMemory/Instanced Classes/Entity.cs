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
    /// <summary>Basic entity class.</summary>
    public class Entity : System.IDisposable, ILocation3D
    {
        /// <summary>Entity's attitude.</summary>
        public eAttitude Attitude;
        /// <summary>Entity's GUID.</summary>
        public int ID
        {
            get { return _ID; }
            set
            {
                Memory.WriteMemory(Process.handle, (_PtrEntity + 0x20), value);
                _ID = value;
            }
        }

        public int TargetPtr;

        private int _ID;
        /// <summary>TRUE if Entity is dead.</summary>
        public bool IsDead;
        /// <summary>Entity's name.</summary>
        public string Name;
        /// <summary>Entity's level.</summary>
        public byte Level;
        /// <summary>Entity's current HP (represented as %).</summary>
        public byte Health;
        /// <summary>Entity's type.</summary>
        public eType Type
        {
            get
            {
                if (_Type168 == 0 && _Type18 == 2)
                    return eType.FriendlyNPC;
                else
                    return (eType)_Type168;
            }
        }
        public int _Type18;
        public int _Type168;
        /// <summary>Entity's class.</summary>
        public eClass Class;
        /// <summary>Entity's current stance.</summary>
        public eStance Stance
        {
            get { return _Stance; }
            set {
                _Stance = (eStance)value;
                Memory.WriteMemory(Process.handle, (_PtrEntity + 0x20C), (int)value);
            }
        }
        public int State;
        private eStance _Stance;
        /// <summary>GUID of Entity's target.</summary>
        public int TargetID;
        /// <summary>Entity's X location.</summary>
        public float X { get; set; }
        /// <summary>Entity's Y location.</summary>
        public float Y { get; set; }
        /// <summary>Entity's Z location.</summary>
        public float Z { get; set; }
        /// <summary>Entity's rotation.</summary>
        public float Rotation;
        /// <summary>Pointer to entity.</summary>
        public int PtrEntity;
        public int _PtrEntity;

        public Entity() { }

        public Entity(int ptr)
        {
            this.PtrEntity = ptr;
            SetZero();
            this.Update();
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            this.Name = null;
            this.Level = 0;
            this.Health = 0;
            this.ID = 0;
            this.IsDead = false;
            this.TargetID = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Rotation = 0;
            this.PtrEntity = 0;
            this._PtrEntity = 0;
            this._Stance = 0;
            GC.Collect();
        }

        /// <summary>
        /// Reads values from AION's memory and sets struct values accordingly.
        /// </summary>
        public void Update()
        {
            if (PtrEntity != 0)
            {
                try
                {
                    this.IsDead = Convert.ToBoolean(Memory.ReadByte(Process.handle, (PtrEntity + 0x8)));
                    this.X = Memory.ReadFloat(Process.handle, (PtrEntity + 0x28));
                    this.Y = Memory.ReadFloat(Process.handle, (PtrEntity + 0x2C));
                    this.Z = Memory.ReadFloat(Process.handle, (PtrEntity + 0x30));

                    this._PtrEntity = Memory.ReadInt(Process.handle, (PtrEntity + 0x1C4));
                    if (this._PtrEntity != 0 && this._PtrEntity != -842150451)
                    {
                        this.Name = Memory.ReadString(Process.handle, (_PtrEntity + 0x36), 40, true);
                        this.Level = Memory.ReadByte(Process.handle, (_PtrEntity + 0x32));
                        this.Health = Memory.ReadByte(Process.handle, (_PtrEntity + 0x34));
                        this._Type18 = Memory.ReadInt(Process.handle, (_PtrEntity + 0x18));
                        this._Type168 = Memory.ReadInt(Process.handle, (_PtrEntity + 0x168));
                        this.Attitude = (eAttitude)Memory.ReadInt(Process.handle, (_PtrEntity + 0x1C));
                        this._ID = Memory.ReadInt(Process.handle, (_PtrEntity + 0x20));
                        this.TargetID = Memory.ReadInt(Process.handle, (_PtrEntity + 0x284));
                        this._Stance = (eStance)Memory.ReadInt(Process.handle, (_PtrEntity + 0x20C));
                        this.State = Memory.ReadInt(Process.handle, (_PtrEntity + 0x20C));
                        this.Class = (eClass)Memory.ReadInt(Process.handle, (_PtrEntity + 0x19C));
                    }
                }
                catch (Exception)
                {
                    SetZero();
                }
            }
            else
                SetZero();
        }

        /// <summary>
        /// Sets all struct values to 0.
        /// </summary>
        private void SetZero()
        {
            this._PtrEntity = 0;
            this.Name = "";
            this.Level = 0;
            this.Health = 0;
            this._Type18 = 0;
            this._Type168 = 0;
            this.Attitude = 0;
            this.ID = 0;
            this.IsDead = false;
            this.TargetID = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this._Stance = 0;
        }
    }
}
