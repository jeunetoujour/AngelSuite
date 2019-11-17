
using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
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
        public uint TARGETPTR_OFFSET = 0x703580;//0x6E6D80;//x639BBC;6FAD50


        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Target()
        {
            //this.Update();
        }

        public void UpdateID()
        {
            this.PtrTarget = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + TARGETPTR_OFFSET));
            this.PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrTarget + 0x1D4));
            this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x24));
            this.TargetID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x2D4));
            this.HasTarget = (TargetID != 0);
        }

     
        /// <summary>
        /// Reads values from AION's memory and sets class member values accordingly.
        /// </summary>
        new public void Update()
        {
            try
            {
                this.PtrTarget = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + TARGETPTR_OFFSET));
                if (PtrTarget != 0)
                {
                    this.PtrEntity = (int)Memory.ReadUInt(Process.handle, (uint)(PtrTarget + 0x1D4));//Memory.ReadInt(Process.handle, Process.Modules.Game + 0x62ED40);       
                    this.Attitude = (eAttitude)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1C));//Memory.ReadInt(Process.handle, 0x62ED5C);//(PtrEntity + 0x1C));
                    this.Class = (eClass)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1DC));
                    this.Health = Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x38));
                    this.HealthHP = Memory.ReadInt(Process.handle, (uint)(PtrEntity + 0x1034));
                    this.HealthHPMax = Memory.ReadInt(Process.handle, (uint)(PtrEntity + 0x1038));
                    this.TargetID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x2D4 )); 
                    this.HasTarget = (TargetID != 0);
                    
                    this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x28));//Memory.ReadInt(Process.handle, (PtrEntity + 0x20));
                    this.Stance = (eStance)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x258));
                    this.IsDead = ((int)Stance == 7);
                    this.Level = Memory.ReadByte(Process.handle, (uint)(PtrEntity + 0x36));
                    this.Name = Memory.ReadString(Process.handle, (uint)(PtrEntity + 0x3A), 64, true);//, 64, true);
                    this._Type18 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x18));
                    this._Type168 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrEntity + 0x1A8));
                    this.X = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x284));//Memory.ReadFloat(Process.handle, tarx);//(PtrTarget + 0x28));
                    this.Y = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x288));//(PtrTarget + 0x2C));
                    this.Z = Memory.ReadFloat(Process.handle, (uint)(PtrTarget + 0x28C));//(PtrTarget + 0x30));34
                    //if (this.Name == "JeunaLM") 
                      //  this.TargetID = this.TargetID;
                }
                else
                    Clear();
            }
            catch 
                (Exception ) { }
        }

        /// <summary>
        /// Sets all class member values to 0.
        /// </summary>
        public void Clear()
        {
            this.Attitude = 0;
            this.Class = 0;
            this.Health = 0;
            this.HasTarget = false;
            this.ID = 0;
            this.IsDead = false;
            this.Level = 0;
            this.Name = "";
            this.HealthHP = 0;
            this.HealthHPMax = 0;
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
