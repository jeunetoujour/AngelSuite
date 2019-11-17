using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryLib;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;

namespace AngelRead
{
    public class Item
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int GetTickCount();
        //Base pointer to the ability.
        private long _ptrBase;
        public long PtrBase 
        {
            get { return _ptrBase; }
            //when setting the base pointer, set the ability pointer to zero.
            set { _ptrBase = value; _ptrItem = 0; }
        }
        
        //Pointers to other PtrBase, in the Ability Tree.
        public long PtrChild1 { get; set; }
        public long PtrChild2 { get; set; }
        public long PtrChild3 { get; set; }        

        public int ItemID { get; set; }
        public int AvailableAtTick { get; private set; }
        public int CooldownLength { get; private set; }
        public int CooldownTick { get; private set; }
        public int CoolRemaining { get; private set; }
        public string ItemName { get;  set; }
        //public int Remaining { get; set; }

        //After making a handful of jumps, this points to the actual ability.
        private long _ptrItem;
        public long PtrItem
        {
            get
            {

                if (_ptrItem == 0)
                {
                    try
                    {
                        _ptrItem = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrItem + 0x10));//14
                        _ptrItem = (int)Memory.ReadUInt(Process.handle, (uint)_ptrItem);
                        _ptrItem = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrItem + 0x10));//14
                        _ptrItem = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrItem + 0x4));
                        _ptrItem = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrItem + 0x8));
                    }
                    catch
                    {
                        _ptrItem = -1;
                    }
                }
                return _ptrItem;

                
            }
        }

        public Item(long ptr)
        {
            //HOTBAR_OFFSET = hotoffset; 
            this.PtrBase = ptr;
            SetZero();
            this.Update();
        }

        public Item()
        {
           // HOTBAR_OFFSET = hotoffset;
        }

        public bool Ready
        {
            get
            {
                if (this.AvailableAtTick < GetTickCount())
                    return true;
                else return false;
            }
        }

      
        bool IsAlphaNumeric(string text)
        {
            //Regex regex = new Regex("[^a-zA-Z0-9]");
            Regex regex = new Regex(@".*[^\u0000-\u052f].*");//(@"^\w+$");//@"[^a-zA-Z0-9- ']");
            return regex.IsMatch(text);
        }


        public void Update()
        {
            if (PtrBase != 0)
            {
                PtrChild1 = Memory.ReadUInt(Process.handle, (uint)PtrBase);
                PtrChild2 = Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x04));
                PtrChild3 = Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x08));

                long thisNamePtr = (int)Memory.ReadUInt(Process.handle, (uint)(PtrItem + 0x1C));
                if (thisNamePtr.ToString().Length > 8)
                    ItemName = Memory.ReadString(Process.handle, (uint)thisNamePtr,64,true); //openmem "wchar[64]")
                else
                    ItemName = Memory.ReadString(Process.handle, (uint)(PtrItem + 0x1C), 64, true); //openmem "wchar[64]")
  if (ItemName != "") 
                    ItemName = ItemName;
                /*var thisIdentifier = Memory.ReadUInt(Process.handle, (PtrBase + 0x0));
                var thisAID = Memory.ReadUInt(Process.handle, (PtrBase + 0x8));
                var thisID = Memory.ReadUInt(Process.handle, (PtrBase + 0xC));
                var thisStack = Memory.ReadUInt(Process.handle, (PtrBase + 0x10));
                var thisReuse = Memory.ReadUInt(Process.handle,(PtrItem + 0x3C)) ;
                var thisType = Memory.ReadUInt(Process.handle,(PtrItem + 0x48)) ;
                var thisSlot = Memory.ReadUInt(Process.handle,(PtrItem + 0x5C)) ;
                var thisWorth = Memory.ReadUInt(Process.handle,(PtrItem + 0x64)) ;
                var thisStatHP = Memory.ReadUInt(Process.handle,(PtrItem + 0x138));
                var thisStatMP = Memory.ReadUInt(Process.handle,(PtrItem + 0x148)) ;
                var thisStatpDef = Memory.ReadUInt(Process.handle,(PtrItem + 0x178)) ;
                var thisStatmRes = Memory.ReadUInt(Process.handle,(PtrItem + 0x188)) ;
                var thisStatEva = Memory.ReadUInt(Process.handle,(PtrItem + 0x1A0)) ;
                var thisStatpCrit = Memory.ReadUInt(Process.handle,(PtrItem + 0x1B8) );
                var thisStatCon = Memory.ReadUInt(Process.handle,(PtrItem + 0x1F0)) ;
                var thisStatBoost = Memory.ReadUInt(Process.handle,(PtrItem + 0x3E8)) ;
                var thisAucPrice = Memory.ReadUInt(Process.handle,(PtrItem + 0x460)) ;
                var thisAucSeller = Memory.ReadString(Process.handle,(PtrItem + 0x468),32,true); //openmem "wchar[32]")
                var thisUnk = Memory.ReadUInt(Process.handle,(PtrItem + 0x474));
                */

              

                ItemID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrItem + 0x0C));
                //var checkValue1 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x18));

                //if (checkValue1 != 1) { _ptrItem = -1; }
                
                //CheckValue2 = Memory.ReadByte(Process.handle, PtrBase + 0x1C) == 1;

            }
           
                //try
               // {
                    //CooldownLength = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x3c));
                    //AvailableAtTick = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x40));
                    //CoolRemaining = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x54));
                    //var ptrName = Memory.ReadInt(Process.handle, PtrAbility + 0x1C);
                    //ItemName = Memory.ReadString(Process.handle, (uint)(PtrAbility + 0x1C), 64, true);
                    //AbilityName = Memory.ReadString(Process.handle, (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x1C)), 64, true);
               // }
               // catch (Exception)
                //{ }
                
        }

        /// <summary>
        /// Sets all class member values to 0.
        /// </summary>
        public void SetZero()
        {
            AvailableAtTick = 0;
            CooldownLength = 0;
            ItemName = "";
        }

    }
}
