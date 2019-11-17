using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>Basic entity class.</summary>
    public class Entity : System.IDisposable, ILocation3D
    {
        /// <summary>Entity's attitude.</summary>
        public eAttitude Attitude;
        /// <summary>Entity's GUID.</summary>
        public int ID;   
        /// <summary>TRUE if Entity is dead.</summary>
        public bool IsDead;
        /// <summary>Entity's name.</summary>
        public string Name;
        /// <summary>Entity's level.</summary>
        public byte Level;
        /// <summary>Entity's current HP (represented as %).</summary>
        public byte Health;
        /// <summary>
        /// True HP number
        /// </summary>
        public int HealthHP;
        public int HealthHPMax;
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
        private int _Type18;
        private int _Type168;
        /// <summary>Entity's class.</summary>
        public eClass Class;
        /// <summary>Entity's current stance.</summary>
        public eStance Stance
        {
            get { return _Stance; }
            set {}
        }
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

        public Entity() {}

        public Entity(int ptr)
        {
            this.PtrEntity = ptr;
            Clear();
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
                    this.IsDead = Convert.ToBoolean(Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x8)));
                    this.X = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x28));
                    this.Y = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x2C));
                    this.Z = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x30));

                    this._PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1C4));
                    if (this._PtrEntity != 0 && this._PtrEntity != -842150451)
                    {
                        this.Name = Memory.ReadString(Process.handle, (uint)(_PtrEntity + 0x36), 40,true);
                        this.Level = Memory.ReadByte(Process.handle, (uint)(_PtrEntity + 0x32));
                        this.Health = Memory.ReadByte(Process.handle, (uint)(_PtrEntity + 0x34));
                        this.HealthHP = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0xF48));
                        this.HealthHPMax = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0xF4C));
                        this._Type18 = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x18));
                        this._Type168 = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x168));
                        this.Attitude = (eAttitude)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1C));
                        this.ID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x20));
                        this.TargetID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x284));
                        this._Stance = (eStance)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x20C));
                        this.Class = (eClass)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x19C));
                    }
                }
                catch (Exception)
                {
                    Clear();
                }
            }
            else
                Clear();
        }

        /// <summary>
        /// Sets all struct values to 0.
        /// </summary>
        private void Clear()
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
