using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MemoryLib;

namespace AngelRead
{
    public class AbilityList: IEnumerable<Ability>
    {
        Dictionary<string, Ability> abilities;
        HashSet<int> found;
        public uint ABILITY_OFFSET = 0xB16700;//0xAF90B8;//0xA273A4; 
       // public uint HOTBAR_OFFSET =  0xB3C0A0;//0xA273A4;

        private void Recurse(Ability ability)
        {
            if (!found.Add(ability.PtrBase))
                return;

            if (ability.AbilityName != "" && !abilities.ContainsKey(ability.AbilityName))
              abilities.Add(ability.AbilityName, ability);
            Recurse(new Ability(ability.PtrChild1));
            Recurse(new Ability(ability.PtrChild3));
            Recurse(new Ability(ability.PtrChild2));
        }
        
        public void Update()
        {
            found = new HashSet<int>();
            abilities = new Dictionary<string, Ability>();

            int headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + ABILITY_OFFSET));//A2 73A4 0xA76CC4
            headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(headPtr + 0x7D0)); //1.9 1Ec

            Recurse(new Ability(headPtr));
        }

        public void Rename(string FromAbilityName, string ToAbilityName)
        {
            try
            {
                if(!abilities.ContainsKey(ToAbilityName))
                {
                abilities.Add(ToAbilityName, abilities[FromAbilityName]);
                abilities.Remove(FromAbilityName);
                }
            }
            catch (Exception rename) {MessageBox.Show("Rename issue:" + FromAbilityName + " :: to " + ToAbilityName + rename.ToString());}
        }
        public void Remove(string AbilityName)
        {
            abilities.Remove(AbilityName);
        }

        public Ability this[string AbilityName]
        {
            get
            {
                try
                {
                    return abilities[AbilityName];
                }
                catch (Exception abil) {
                    Ability nothing = new Ability();
                    nothing.AbilityName = "Nothing";
                    return nothing; }
            } 
        }
     

        #region IEnumerable<Ability> Members

        public IEnumerator<Ability> GetEnumerator()
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
