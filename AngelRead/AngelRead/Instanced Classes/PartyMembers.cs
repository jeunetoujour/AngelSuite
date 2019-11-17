// Custom Includes
using MemoryLib;

// Standard Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>Contains information about party members.</summary>
    public class PartyMembers : Entity
    {
        /// <summary>Player index.</summary>
        public int Index;
        /// <summary>Player's class.</summary>
        //public eClass Class;
        /// <summary>Player's current HP.</summary>
        new public int Health;
        /// <summary>Player's maximum HP</summary>
        new public int MaxHealth;
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
        public float HealthPct;
        /// <summary>
        /// Class instance initializer.
        /// </summary>
        /// <param name="i">Index of the player in the party.</param>
        public PartyMembers(int i)
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
            int PtrPlayer = Memory.ReadInt(Process.handle, (uint)(Process.Modules.Game + 0xB17BA4));

            for (i = 0; i < Index; i++)
            {
                PtrPlayer = Memory.ReadInt(Process.handle, (uint)PtrPlayer);
            }

            ThisPartyMember = PtrPlayer;
            NextPartyMember = Memory.ReadInt(Process.handle, (uint)(PtrPlayer));
            PreviousPartyMember = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x4));
            PtrPlayer = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x8));
            if (PtrPlayer != 0 && PtrPlayer != -842150451)
            {
                this.Name = Memory.ReadString(Process.handle, (uint)(PtrPlayer + 0x3A), 40, true);
                if (Name != "")
                {
                    //this.Attitude = (eAttitude)Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x1C));
                    this.ID = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x4));
                    //this.GatherID = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x28));
                    this.Level = Memory.ReadByte(Process.handle, (uint)(PtrPlayer + 0x36));
                    //this.Health = Memory.ReadByte(Process.handle, (uint)(PtrPlayer + 0x38));
                    //this.PetOwner = Memory.ReadString(Process.handle, (uint)(PtrPlayer + 0xC0), 32, true);
                    //this._Type18 = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x18));
                    //this._Type168 = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x1A8));
                    this.Class = (eClass)Memory.ReadByte(Process.handle, (uint)(PtrPlayer + 0x34));
                    //this.TargetID = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x2D4));
                    //this._Stance = (eStance)Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x258));
                    //this.DP = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x320));
                    //this.RankID = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0xE7C));
                    this.Health = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0xC));
                    this.MaxHealth = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x8)); //4
                    this.MP = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x14));
                    this.MaxMP = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x10)); //4
                    this.FlightTime = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x1C));
                    this.MaxFlightTime = Memory.ReadInt(Process.handle, (uint)(PtrPlayer + 0x18)); //4
                    this.X = Memory.ReadFloat(Process.handle, (uint)(PtrPlayer + 0x28));
                    this.Y = Memory.ReadFloat(Process.handle, (uint)(PtrPlayer + 0x2C));
                    this.Z = Memory.ReadFloat(Process.handle, (uint)(PtrPlayer + 0x30));
                    this.HealthPct = (float)Health / (float)MaxHealth * 100;
                }
            }
            /*if (PtrPlayer != 0)
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
            }*/
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
            //this.MaxHealth = 0;
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
