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

namespace AionMemory
{
    public class Ability
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int GetTickCount();
        //Base pointer to the ability.
        private int _ptrBase;
        private SkillList skilllist = new SkillList();
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
        private int Bar = -1;
        private int Hotkey = -1;
        public string Keypress = "";
        //CheckValues for traversing the tree.
        //public int CheckValue1 { get; private set; }
        //public bool CheckValue2 { get; private set; }
        
        public int AvailableAtTick { get; private set; }
        public int CooldownLength { get; private set; }
        public int CooldownTick { get; private set; }
        public int CoolRemaining { get; private set; }
        public int CastTime { get; set; }
        public int CastTimeMod { get; set; }
        public int CastPercent { get; set; }
        public bool PerChance { get; set; }
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
                        _ptrAbility = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x14));
                        _ptrAbility = (int)Memory.ReadUInt(Process.handle, (uint)_ptrAbility);
                        _ptrAbility = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrAbility + 0x14));
                        _ptrAbility = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrAbility + 0x4));
                        _ptrAbility = (int)Memory.ReadUInt(Process.handle, (uint)(_ptrAbility + 0x8));
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

        public bool Ready
        {
            get
            {
                
                if (this.AvailableAtTick < GetTickCount())
                    return true;
                else return false;
            }
        }

        public void MatchHotbar()
        {
            //"Type" 0x4 = Data1 0x8 = Data2  skill = 0x15
            
            for (int bars = 0; bars <= 3; bars++)
            {
                int thisbar = bars * 0x90;
                for (int i = 0; i < 12; i++)
                {
                    long caddress = Process.Modules.Game + 0xA66E30 + 8 + thisbar + (i * 0xC);
                    int skillnum = (int)Memory.ReadUInt(Process.handle, (uint)(caddress + 0x4));
                    if (skillnum == AbilityID)
                    {
                        Bar = bars + 1;
                        Hotkey = i + 1;
                        string tempkey = EnumHotbar(Bar, Hotkey);
                        if(tempkey != "")
                            Keypress = tempkey;
                    }
                }  
            }        
        }
        private string EnumHotbar(int bar, int key)
        {
            string together = "";
            if (bar == -1) return together;

            if (bar == 1) together = "";
            if (bar == 2) together = "Alt,";
            if (bar == 3) together = "Ctrl,";
            if (bar == 4) together = "";

            if (key == 11) together = together + "-";
            else if (key >= 12) 
                together = together + "=";
            else if (key == 10) together = together + "0";
            else together = together + (key).ToString();
            return together;
        }

        public Ability(int ptr)
        {
            this.PtrBase = ptr;
            SetZero();
            this.Update();
        }

        public void AddCastTime()
        {
            this.CastTime = skilllist.CastTime(this.AbilityName);
        }
        bool IsAlphaNumeric(string text)
        {
            //Regex regex = new Regex("[^a-zA-Z0-9]");
            Regex regex = new Regex(@".*[^\u0000-\u052f].*");//(@"^\w+$");//@"[^a-zA-Z0-9- ']");
            return regex.IsMatch(text);
        }
        /// <summary>
        /// Reads values from AION's memory and sets struct values accordingly.
        /// </summary>
        public void Update()
        {
            if (PtrBase != 0)
            {
                PtrChild1 = (int)Memory.ReadUInt(Process.handle, (uint)PtrBase);
                PtrChild2 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x04));
                PtrChild3 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x08));

                AbilityID = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x0C));
                var checkValue1 = (int)Memory.ReadUInt(Process.handle, (uint)(PtrBase + 0x18));
               
                if (checkValue1 != 1) { _ptrAbility = -1; }
                
                //CheckValue2 = Memory.ReadByte(Process.handle, PtrBase + 0x1C) == 1;

            }
            if (PtrAbility != -1 && ((AbilityID < 30002 || AbilityID > 30003) && (AbilityID < 40001 || AbilityID > 40009) && (AbilityID < 50000 || AbilityID > 50037)))
            {
                CooldownLength = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x3c));
                AvailableAtTick = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x40));
                CoolRemaining = (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x54));
                //var ptrName = Memory.ReadInt(Process.handle, PtrAbility + 0x1C);
                AbilityName = Memory.ReadString(Process.handle, (uint)(PtrAbility + 0x1C), 64, true);
                //if (AbilityName == "Rage IV") 
                    //AbilityName = AbilityName;
                /*Local $thisReuse     = _MemoryRead(Dec(Hex($thisSkill + 0x3C)), $openmem)
                   If ($thisID < 30002 Or $thisID > 30003) And ($thisID < 40001 Or $thisID > 40009) And 
                 * ($thisID < 50009 Or $thisID > 50037) And $thisReuse > 0 Or 
                 * ($thisReuse <= 0 And ($thisID > 50000 And $thisID < 50009)) Then
                 * */
                /*if (AbilityName.Length > 7 || AbilityName.Length <= 4 && AbilityID < 10000) // if the string is > 8 characters here, this was actually a pointer to the string.
                {
                    AbilityName = Memory.ReadString(Process.handle, (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x1C)), 64, true);
                }*/
                if (IsAlphaNumeric(AbilityName) == true)
                    AbilityName = Memory.ReadString(Process.handle, (int)Memory.ReadUInt(Process.handle, (uint)(PtrAbility + 0x1C)), 64, true);
                //AbilityName = Memory.ReadString(Process.handle, (uint)(PtrAbility + 0x1C), 64, true);

                MatchHotbar();
                AddCastTime();
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
