// Standard Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryLib;

namespace AngelRead
{
    /// <summary>Contains information about party.</summary>
    public class Party
    {
        /// <summary>Number of players in party.</summary>
        public int Members;
        /// <summary>Party member data.</summary>
        public PartyMembers[] Member = new PartyMembers[5];

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Party()
        {

            this.Members = Memory.ReadInt(Process.handle, (uint)(Process.Modules.Game + 0xB17BA8));
            for (int i = 1; i <= this.Members; i++)
            {
                this.Member[i-1] = new PartyMembers(i);
            }
        }

        /// <summary>
        /// Reads data from memory and sets struct values accordingly.
        /// </summary>
        public void Update()
        {
            this.Members = Memory.ReadInt(Process.handle, (uint)(Process.Modules.Game + 0xB17BA8));
            for (int i = 0; i < this.Members; i++)
            {
                this.Member[i].Update();
            }
        }
    }
}
