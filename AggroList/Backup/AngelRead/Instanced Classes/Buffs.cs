using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MemoryLib;

namespace AngelRead
{
    public class Buffs : IEnumerable<Buff>
    {
        Dictionary<int, Buff> abilities;
        HashSet<int> found;
        public uint ABILITY_OFFSET = 0;//0xA273A4;
        public uint HOTBAR_OFFSET = 0;//0xA273A4;

        private void Recurse(Buff ability)
        {
            if (!found.Add(ability.PtrBase))
                return;

            if (ability.AbilityID != 0 && !abilities.ContainsKey(ability.AbilityID))
                abilities.Add(ability.AbilityID, ability);
            Recurse(new Buff(ability.PtrChild1, HOTBAR_OFFSET));
            Recurse(new Buff(ability.PtrChild2, HOTBAR_OFFSET));
            Recurse(new Buff(ability.PtrChild3, HOTBAR_OFFSET));
        }

        public void Update()
        {
            found = new HashSet<int>();
            abilities = new Dictionary<int, Buff>();

            int headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + ABILITY_OFFSET));//A2 73A4
            headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(headPtr + 0x1FC)); //buffs

            Recurse(new Buff(headPtr, HOTBAR_OFFSET));
        }

        public Buff this[int AbilityName]
        {

            get
            {
                try
                {
                    return abilities[AbilityName];
                }
                catch (Exception abil) { MessageBox.Show("Ability: " + AbilityName + " " + abil); return abilities[AbilityName]; }
            }

        }


        #region IEnumerable<Ability> Members

        public IEnumerator<Buff> GetEnumerator()
        {
            return abilities.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

