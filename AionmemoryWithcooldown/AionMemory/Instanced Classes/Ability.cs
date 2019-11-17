using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryLib;

namespace AionMemory
{
    public class Ability
    {
        //Base pointer to the ability.
        private int _ptrBase;
        public int PtrBase 
        {
            get { return _ptrBase; }
            //when setting the base pointer, set the ability pointer to zero.
            set { _ptrBase = value; _ptrAbility = 0; }
        }
        
        //Pointers to other PtrBase, in the Ability Tree.
        public int PtrChild1 { get; set; }
        public int PtrChild2 { get; set; }
        public int PtrChild3 { get; set; }        

        public int AbilityID { get; set; }
        //CheckValues for traversing the tree.
        //public int CheckValue1 { get; private set; }
        //public bool CheckValue2 { get; private set; }
        
        public int AvailableAtTick { get; private set; }
        public int CooldownLength { get; private set; }
        public string AbilityName { get; private set; }

        //After making a handful of jumps, this points to the actual ability.
        private int _ptrAbility;
        public int PtrAbility
        {
            get
            {

                if (_ptrAbility == 0)
                {
                    try
                    {
                        _ptrAbility = Memory.ReadInt(Process.handle, PtrBase + 0x14);
                        _ptrAbility = Memory.ReadInt(Process.handle, _ptrAbility);
                        _ptrAbility = Memory.ReadInt(Process.handle, _ptrAbility + 0x14);
                        _ptrAbility = Memory.ReadInt(Process.handle, _ptrAbility + 0x4);
                        _ptrAbility = Memory.ReadInt(Process.handle, _ptrAbility + 0x8);
                    }
                    catch
                    {
                        _ptrAbility = -1;
                    }
                }
                return _ptrAbility;

                
            }
        }


        public Ability()
        {

        }

        public Ability(int ptr)
        {
            this.PtrBase = ptr;
            SetZero();
            this.Update();
        }

        /// <summary>
        /// Reads values from AION's memory and sets struct values accordingly.
        /// </summary>
        public void Update()
        {
            if (PtrBase != 0)
            {
                PtrChild1 = Memory.ReadInt(Process.handle, PtrBase);
                PtrChild2 = Memory.ReadInt(Process.handle, PtrBase + 0x04);
                PtrChild3 = Memory.ReadInt(Process.handle, PtrBase + 0x08);

                AbilityID = Memory.ReadInt(Process.handle, PtrBase + 0x0C);
                var checkValue1 = Memory.ReadInt(Process.handle, PtrBase + 0x18);
               
                if (checkValue1 != 1) { _ptrAbility = -1; }
                
                //CheckValue2 = Memory.ReadByte(Process.handle, PtrBase + 0x1C) == 1;

            }
            if (PtrAbility != -1)
            {
                CooldownLength = Memory.ReadInt(Process.handle, PtrAbility + 0x3c);
                AvailableAtTick = Memory.ReadInt(Process.handle, PtrAbility + 0x40);
                var ptrName = Memory.ReadInt(Process.handle, PtrAbility + 0x1C);

                AbilityName = Memory.ReadString(Process.handle, PtrAbility + 0x1C, 64, true);
                if (AbilityName.Length > 8) // if the string is > 8 characters here, this was actually a pointer to the string.
                {
                    AbilityName = Memory.ReadString(Process.handle, Memory.ReadInt(Process.handle, PtrAbility + 0x1C), 64, true);
                }
            }
        }

        /// <summary>
        /// Sets all class member values to 0.
        /// </summary>
        public void SetZero()
        {
            AvailableAtTick = 0;
            CooldownLength = 0;
            AbilityName = "";
        }

    }
}
