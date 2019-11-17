using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MemoryLib;

namespace AngelRead
{
    public class InventoryList: IEnumerable<Item>
    {
        Dictionary<string, Item> items;
        HashSet<long> found;
        public uint ABILITY_OFFSET = 0xAF90B8;//0xA273A4;
       // public uint HOTBAR_OFFSET =  0xB3C0A0;//0xA273A4;

        private void Recurse(Item Item)
        {
            if (!found.Add(Item.PtrBase))
                return;

            if (Item.ItemName != "" && !items.ContainsKey(Item.ItemName))
              items.Add(Item.ItemName, Item);
            Recurse(new Item(Item.PtrChild1));
            Recurse(new Item(Item.PtrChild2));
            Recurse(new Item(Item.PtrChild3));
        }
        
        public void Update()
        {
            found = new HashSet<long>();
            items = new Dictionary<string, Item>();

            long headPtr = Memory.ReadUInt(Process.handle, (Process.Modules.Game + ABILITY_OFFSET));//A2 73A4 0xA76CC4
            headPtr = Memory.ReadUInt(Process.handle, (uint)(headPtr + 0x71C)); //1.9 1Ec

            Recurse(new Item(headPtr));
        }

        /*public void Rename(string FromAbilityName, string ToAbilityName)
        {
            try
            {
                if(!items.ContainsKey(ToAbilityName))
                {
                items.Add(ToAbilityName, items[FromAbilityName]);
                items.Remove(FromAbilityName);
                }
            }
            catch (Exception rename) {MessageBox.Show("Rename issue:" + FromAbilityName + " :: to " + ToAbilityName + rename.ToString());}
        }*/

        public void Remove(string ItemName)
        {
            items.Remove(ItemName);
        }

        public Item this[string ItemName]
        {
            get
            {
                try
                {
                    return items[ItemName];
                }
                catch (Exception abil) {
                    Item nothing = new Item();
                    nothing.ItemName = "Nothing";
                    return nothing; }
            } 
        }
     

        #region IEnumerable<Item> Members

        public IEnumerator<Item> GetEnumerator()
        {
            return items.Values.GetEnumerator();
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
