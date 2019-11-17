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
        public int trueID;
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
        public int _Type18;
        public int _Type168;
        /// <summary>Entity's class.</summary>
        public eClass Class;
        /// <summary>Entity's current stance.</summary>
        public eStance Stance
        {
            get { return _Stance; }
            set { }
        }
        public eStance _Stance;
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
        public string PetOwner;
        public int DP;
        public float Attackspeed; //486
        public float Speed; //638
        public int GatherID;
        public int RankID;

        public Entity() { }

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

        public eType nType
        {
            get
            {
                if (_Type168 == 0 && _Type18 == 1)
                    return eType.Player;
                else if (_Type168 == 14 && _Type18 == 2)
                    return eType.FriendlyNPC;
                else if (_Type168 == 0 && _Type18 == 2)
                    return eType.FriendlyNPC;
                else if (_Type168 == 0 && _Type18 == 10)
                    return eType.Place;
                else if (_Type168 == 12 && _Type18 == 2)
                {
                    if (Name.Contains("Kisk"))
                        return eType.Rift;
                    else if (Name.Contains("Artifact ("))
                        return eType.Object;
                    else
                        return eType.AttackableNPC;
                }
                else if (_Type168 == 12 && _Type18 == 1)
                    return eType.EnemyPlayer;
                else if (_Type168 == 7 && _Type18 == 2 && Name.Contains("Rift"))
                    return eType.Rift;
                else if (_Type168 == 7 && _Type18 == 2 && Name.Contains("Kisk"))
                    return eType.Kisk;
                else if (_Type168 == 8 && _Type18 == 2)
                    return eType.Object;
                else if (_Type168 == 15 && _Type18 == 2)
                    return eType.Object;
                else if (_Type168 == 0 && _Type18 == 20)
                    return eType.Vendor;
                else if (_Type168 == 39 && _Type18 == 7)
                    return eType.Gatherable;
                else if (_Type168 == 40 && _Type18 == 7)
                    return eType.GatherableNoSkill;
                else if (_Type168 == 36 && _Type18 == 2)
                    return eType.DeadwLoot;
                else if (_Type168 == 37 && _Type18 == 2)
                    return eType.DeadnoLoot;
                else
                    return (eType)_Type168;
            }
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
        /// 
        public void UpdateZ(float value)
        {
            Memory.WriteMemory(Process.handle, (PtrEntity + 0x30), value);
        }

        public uint coords;

        public float Zchangeable
        {
            get
            {
                return Memory.ReadFloat(Process.handle, (uint)(this.coords + 0x6c));
            }
            set
            {
                Memory.WriteMemory(Process.handle, (this.coords + 0x6C), value);
            }
        }

        public float AttackSpd
        {
            get
            {
                return this.Attackspeed = Memory.ReadFloat(Process.handle, (uint)(_PtrEntity + 0x486));
            }
            set
            {
                //Memory.WriteMemory(Process.handle, (this.coords + 0x6C), value);
            }
        }

        public void WriteID(int value)
        {
            //this._PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1D0));
            if (value != 666)
            {
                trueID = ID;
                //this.TargetID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x24));
                Memory.WriteMemory(Process.handle, (uint)(_PtrEntity + 0x28), value);
            }
            else
            { Memory.WriteMemory(Process.handle, (uint)(_PtrEntity + 0x28), trueID);
              Memory.WriteMemory(Process.handle, (uint)(_PtrEntity + 0x2D8), -1);
            }
        }

        public void Update()
        {
            if (PtrEntity != 0)
            {
                try
                {
                    this.IsDead = Convert.ToBoolean(Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x8)));
                    this.X = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x2C));
                    this.Y = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x30));
                    this.Z = Memory.ReadFloat(Process.handle, (uint)(PtrEntity + 0x34));
                    this.coords = Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0xAC)); //64?

                    this._PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1D4));
                    if (this._PtrEntity != 0 && this._PtrEntity != -842150451)
                    {
                        this.Name = Memory.ReadString(Process.handle, (uint)(_PtrEntity + 0x3A), 40, true);
                        if (Name != "")
                        {
                            this.Attitude = (eAttitude)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1C));
                            this.ID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x28));
                            this.GatherID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x32));
                            this.Level = Memory.ReadByte(Process.handle, (uint)(_PtrEntity + 0x36));
                            this.Health = Memory.ReadByte(Process.handle, (uint)(_PtrEntity + 0x38));
                            this.PetOwner = Memory.ReadString(Process.handle, (uint)(_PtrEntity + 0xC0), 32, true);
                            this._Type18 = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x18));
                            this._Type168 = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1A8));
                            this.Class = (eClass)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1DC));
                            this.TargetID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x2D4));
                            this._Stance = (eStance)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x258));
                            this.DP = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x320));
                            this.RankID = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0xE7C));
                            this.HealthHP = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1034));
                            this.HealthHPMax = Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x1038)); //4
                        }

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
