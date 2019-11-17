using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AngelRead
{
    /// <summary>Contains information about player.</summary>
    public class Player : Entity
    {
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
        /// <summary>Maximum number of slots in all cubes.</summary>
        public int MaxSlots;
        public int Kinah;
        public int PtrKinah;
        public int AP;
        public bool InventoryFull;
        public byte isFlying;

        public int SelfPtr;
        public int HasTarget;
        /// <summary>Players's current stance.</summary> 
        new public eStance Stance;
        /*          ReactionInfoAddress = 0xA687C8
                    InventoryInfoAddress = 0xA68658
         *                         HOTBAR  0xA66E38
                    TargetInfoAddress = 0x639BBC
                    EntityInfoAddress = 0xA32C40
                    AbilityInfoAddress = 0xA273A4
                    PlayerInfoAddress = 0xA32978
                */
        public uint PLAYER_INFOADDRESS_OFFSET = 0xB22120;//0xB04A78;//xA32978;
        public uint PLAYER_INVENTORY_OFFSET = 0xB59D6C;//0xB3C0C4;
        //public uint ENTITY_OFFSET = 0;
        public uint PLAYER_GUID_OFFSET = 0xB17280; //0xAF9C30;

        const uint PLAYER_AP_OFFSET = 0x22E0; //neg  A80108/A823E8
        const uint PLAYER_LVL_OFFSET = 24;
        const uint PLAYER_EXPLVL_OFFSET = 32;
        const uint PLAYER_EXP_OFFSET = 48;
        const uint PLAYER_MAXHP_OFFSET = 60;
        const uint PLAYER_HP_OFFSET = 64;
        const uint PLAYER_MAXMP_OFFSET = 68;
        const uint PLAYER_MP_OFFSET = 72;
        const uint PLAYER_BAGSLOTS_OFFSET = 148; //1.9
        const uint PLAYER_CLASS_OFFSET = 164; //1.9
        const uint PLAYER_FLYING_OFFSET = 0x58; //1.9
        const uint PLAYER_NAME_OFFSET = 0x39070; //1.90x38A70
        const uint PLAYER_X_OFFSET = 0xB17290; //neg //1.9
        const uint PLAYER_Y_OFFSET = PLAYER_X_OFFSET + 4; //neg
        const uint PLAYER_Z_OFFSET = PLAYER_Y_OFFSET + 4; //neg
        const uint PLAYER_ROT_OFFSET = 0xB16EC8; //neg 1.9
        const uint PLAYER_KINAHPTR_OFFSET = 0xB156E0 + 1020; //neg 1.9
        const uint PLAYER_SPELL_OFFSET = 0xA33C; //neg
        const uint PLAYER_KINAH_OFFSET = 0x708; //1.9
        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Player()
        {
            //Updatenamelvl();
            //this.Update();
        }
        
        /*public void getpcptr()
        {
            EntityList elist = new EntityList(ENTITY_OFFSET);
            elist.Update();
            foreach (Entity thing in elist)
            {
                if (thing.Name == this.Name)
                {
                    if (thing._PtrEntity != 0)
                    {
                        SelfPtr = thing._PtrEntity;
                    }
                    else SelfPtr = thing.PtrEntity;
                }
            }
        }*/
        public int FlightTime
        {
            get
            {
                //return playerEntity.ID;
                return (Memory.ReadInt(Process.handle, Process.Modules.Game + 0xb04aCC)) / 1000;
            }
        }

        public int MaxFlightTime
        {
            get
            {
                //return playerEntity.ID;
                return (Memory.ReadInt(Process.handle, Process.Modules.Game + 0xb04ac8)) / 1000;
            }
        }

        public int GetUsedSpace()
        {
            /*int UsedSpace = 0;
            uint InventoryPtr = Memory.ReadUInt(Process.handle, (Process.Modules.Game + PLAYER_INVENTORY_OFFSET)); //A2739C
            int cubeID = 0x380; //2E8
            int MaxSlots = GetMaxCubes();

            int currentItem = 0x0;
            int Offset2 = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryPtr + cubeID));
            int InventoryList = (int)Memory.ReadUInt(Process.handle, (uint)(Offset2 + 0x284)); //0x298

            for (int slot = 0; slot < MaxSlots; slot++)
            {
                currentItem = currentItem + 0x4;
                int thisItem = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryList + currentItem));
                int thisItemID = (int)Memory.ReadUInt(Process.handle, (uint)(thisItem + 0x98));
                int SlotID = (int)Memory.ReadUInt(Process.handle, (uint)(thisItem + 0x8C));

                if (thisItemID != 0 && SlotID >= 0) UsedSpace++;
                
                if ((slot+1) % 27 == 0)
                {
                    currentItem = 0;
                    cubeID += 4;
                    Offset2 = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryPtr + cubeID));
                    InventoryList = (int)Memory.ReadUInt(Process.handle, (uint)(Offset2 + 0x283));
                }
            }
            */
            return Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xB221B8)); //0xB04B10
        }

        public int GetMaxCubes()
        {
            return Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_BAGSLOTS_OFFSET));//A2F9F0
        }

        public bool CubeFull()
        {
            if (GetMaxCubes() == GetUsedSpace()) return true; //Until fixed have false
            else return false;
        }

        public void Updatenamelvl()
        {
            this.Name = Memory.ReadString(Process.handle, (Process.Modules.Game + 0xB5B190), 32, true);
            this.Level = Memory.ReadByte(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_LVL_OFFSET));
            this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + PLAYER_GUID_OFFSET));//A1CC58//0xA1CC5C
            this.Class = (eClass)(int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_MAXHP_OFFSET + 0x68));
            
        }

        public void Updateafterkill()
        {
            this.PtrKinah = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0xB156E0+1020));
            this.Kinah = (int)Memory.ReadUInt(Process.handle, (uint)(PtrKinah + PLAYER_KINAH_OFFSET));
            this.XP = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_EXP_OFFSET));//A2f9A0
            this.MaxXP = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_EXPLVL_OFFSET));//A2F990
        }

        public void WriteRot(float value)
        {
            Memory.WriteMemory(Process.handle, (Process.Modules.Game +  PLAYER_ROT_OFFSET), value);
        }

        public void WriteZ(float value)
        {
            Memory.WriteMemory(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET - PLAYER_Z_OFFSET), value);
        }

        public void UpdateRot()
        {
            this.Rotation = Memory.ReadFloat(Process.handle, (Process.Modules.Game + PLAYER_ROT_OFFSET));//A24B28
            System.Threading.Thread.Sleep(10);
        }
        //A32978

        public void UpdateAP()
        {
            this.AP = (int)Memory.ReadUInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET - PLAYER_AP_OFFSET));//A24B28
        }
        public void UpdateFlying()
        {
            this.isFlying = Memory.ReadByte(Process.handle, (Process.Modules.Game + 0xB21108));//A24B28
        }

        new public void Update()
        {
            this.Spell = Memory.ReadByte(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET - PLAYER_SPELL_OFFSET)); //A25730
            this.Health = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_HP_OFFSET));//A2F9b0
            this.MaxHealth = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_MAXHP_OFFSET));
            this.MP = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_MP_OFFSET));
            this.MaxMP = Memory.ReadInt(Process.handle, (Process.Modules.Game + PLAYER_INFOADDRESS_OFFSET + PLAYER_MAXMP_OFFSET));
            this.X = Memory.ReadFloat(Process.handle, (Process.Modules.Game + PLAYER_X_OFFSET)); //Y
            this.Y = Memory.ReadFloat(Process.handle, (Process.Modules.Game + PLAYER_Y_OFFSET)); //Z
            this.Z = Memory.ReadFloat(Process.handle, (Process.Modules.Game + PLAYER_Z_OFFSET)); //X
            //this.Stance = (eStance)Memory.ReadInt(Process.handle, (uint)(_PtrEntity + 0x218));
        }
    }
}
