using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ByteSigScanExample
{
    public partial class Form1 : Form
    {
        enum ProcessAccessFlags
        {
            PROCESS_ALL_ACCESS = 0x1F0FFF,
            PROCESS_CREATE_THREAD = 0x2,
            PROCESS_DUP_HANDLE = 0x40,
            PROCESS_QUERY_INFORMATION = 0x400,
            PROCESS_SET_INFORMATION = 0x200,
            PROCESS_TERMINATE = 0x1,
            PROCESS_VM_OPERATION = 0x8,
            PROCESS_VM_READ = 0x10,
            PROCESS_VM_WRITE = 0x20,
            SYNCHRONIZE = 0x100000
        }

        //this sig has lasted since it was found using version .08 
        string playerInfoSig = "8b5720535068XXXXXXXX8d4f3651";
        string abilityinfo = "a1XXXXXXXX8b88380100008b903c0100008b83000500008b30bf1cbf0d";
        //8B 0D XX XX XX XX 8B 89 EC 02 00 00
        //string entityinfo = "85C0A3XXXXXXXX741D8B10";
        //66 85 C0 ?? ?? 8B 0D XX XX XX XX 85 C9 ?? ?? 8B 11
        //string targetinfo = "8b35XXXXXXXXe8f3fa1500";
        string entityinfo = "08008B0DXXXXXXXX3BcE740c";
        string targetinfo = "E8????????8B35XXXXXXXXE8????????E8????????";
        //89 3D XX XX XX XX 8B 07 8B 90 30 01 ?? ?? 8B CF
        string inventoryinfo = "833dXXXXXXXX005356578bf8";
        string pguidinfo = "558BEC5668800000006A0068????????E8????????F30F1005????????8B45080F5AC0F20F5805????????83C40C3B05XXXXXXXX";
        string hotbarinfo = "8DB8XXXXXXXX85FF????????????8B47??8B4F??8B17505152";

        uint currentPID = 0;
        uint playerInfoAddress = 0;
        uint pGUIDInfoAddress = 0;
        uint abilityInfoAddress = 0;
        uint entityInfoAddress = 0;
        uint targetInfoAddress = 0;
        uint inventoryInfoAddress = 0;
        uint reactionInfoAddress = 0;
        uint hotkeyInfoAddress = 0;
        //8D B8 XX XX XX XX 85 FF ?? ?? ?? ?? ?? ?? 8B 47 ?? 8B 4F ?? 8B 17 50 51 52
        uint gamedll = 0;

        

        public Form1()
        {
            InitializeComponent();

            Process[] ProcessList = Process.GetProcesses();
            String ProcessName;

            pidBox.Items.Clear();
            for (int i = 0; i < ProcessList.Length; i++)
            {
                ProcessName = ProcessList[i].ProcessName;

                if (ProcessName == "aion.bin")
                {
                    if ((ProcessList[i].MainWindowTitle.IndexOf("Aion Client") > -1))
                    {

                    }
                    else
                    {
                        pidBox.Items.Add("aion.bin" + " - " + ProcessList[i].Id);
                        pidBox.SelectedIndex = 0;
                        System.Diagnostics.Process HandleP = System.Diagnostics.Process.GetProcessById(ProcessList[i].Id);

                        foreach (System.Diagnostics.ProcessModule Module in HandleP.Modules)
                        {
                            if ("Game.dll" == Module.ModuleName)
                            {
                                gamedll = (uint)Module.BaseAddress.ToInt32();
                            }
                        }
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            string AionID = pidBox.Text;
            int AionIDPosition = AionID.IndexOf(" - ");
            AionID = AionID.Remove(0, AionIDPosition + 3);
            currentPID = (uint)Convert.ToUInt32(AionID);

            // get the address of the playerInfo structure
            InitializeSigScan(currentPID, "Game.dll");
            playerInfoAddress = SigScan(playerInfoSig, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            abilityInfoAddress = SigScan(abilityinfo, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            entityInfoAddress = SigScan(entityinfo, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            targetInfoAddress = SigScan(targetinfo, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            inventoryInfoAddress = SigScan(inventoryinfo, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            pGUIDInfoAddress = SigScan(pguidinfo, 0);
            FinalizeSigScan();

            InitializeSigScan(currentPID, "Game.dll");
            hotkeyInfoAddress = SigScan(hotbarinfo, 0);
            FinalizeSigScan();
            
            reactionInfoAddress = inventoryInfoAddress + 0x170;
            uint temphot = inventoryInfoAddress - 0x1820;
            //if (hotkeyInfoAddress == temphot)
            //    hotkeyInfoAddress = temphot;


            //TxtBoxMsg("Attached...");
            //TxtBoxMsg("Gamedll = 0x" + gamedll.ToString("X"));
            TxtBoxMsg("PlayerInfoAddress = 0x" + (playerInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("AbilityInfoAddress = 0x" + (abilityInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("EntityInfoAddress = 0x" + (entityInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("TargetInfoAddress = 0x" + (targetInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("InventoryInfoAddress = 0x" + (inventoryInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("ReactionInfoAddress = 0x" + (reactionInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("HotkeyInfoAddress = 0x" + (hotkeyInfoAddress - gamedll).ToString("X"));
            TxtBoxMsg("TempHotInfoAddress = 0x" + (temphot - gamedll).ToString("X"));
            TxtBoxMsg("pGUIDInfoAddress = 0x" + (pGUIDInfoAddress - gamedll).ToString("X"));
        }

        private void TxtBoxMsg(string msg)
        {
            textBox1.Text = msg + System.Environment.NewLine + textBox1.Text;

        }

        private void pidBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const uint PLAYER_GUID_OFFSET = 0xAAB8; //neg
            const uint PLAYER_GUID_OFFSET2 = 0; //neg
            //55 8B EC 56 68 80 00 00 00 6A 00 68 ?? ?? ?? ?? E8 ?? ?? ?? ?? F3 0F 10 05 ?? ?? ?? ?? 8B 45 08 0F 5A C0 F2 0F 58 05 ?? ?? ?? ?? 83 C4 0C 3B 05 XX XX XX XX
            const uint PLAYER_LVL_OFFSET = 24;
            const uint PLAYER_EXPLVL_OFFSET = 32;
            const uint PLAYER_EXP_OFFSET = 48;
            const uint PLAYER_MAXHP_OFFSET = 60;
            const uint PLAYER_HP_OFFSET = 64;
            const uint PLAYER_MAXMP_OFFSET = 68;
            const uint PLAYER_MP_OFFSET = 72;
            const uint PLAYER_BAGSLOTS_OFFSET = 128;
            const uint PLAYER_CLASS_OFFSET = 144;
            const uint PLAYER_NAME_OFFSET = 0x37038;
            const uint PLAYER_X_OFFSET = 0x85B0; //neg
            const uint PLAYER_Y_OFFSET = 0x85B4; //neg
            const uint PLAYER_Z_OFFSET = 0x85B8; //neg
            const uint PLAYER_ROT_OFFSET = 0xAE48; //neg
            const uint PLAYER_KINAHPTR_OFFSET = 0xB5D4; //neg
            const uint PLAYER_KINAH_OFFSET = 0x138;

            uint pclass = 0;
            uint bagslots = 0;
            uint guid = 0;
            uint lvl = 0;
            uint xp = 0;
            uint MaxXP = 0;
            uint hp = 0;
            uint maxhp = 0;
            uint Spell = 0;
            uint MP = 0;
            uint MaxMP = 0;
            uint Kinah = 0;
            uint PtrKinah = 0;
            uint HasTarget = 0;
            uint Stance = 0;

            //A32978 //a273A4
            IntPtr hProc = OpenProcess(ProcessAccessFlags.PROCESS_VM_READ, false, currentPID);

            ReadProcessMemory(hProc, playerInfoAddress - PLAYER_KINAHPTR_OFFSET, ref PtrKinah, 4, 0);
            ReadProcessMemory(hProc, PtrKinah + PLAYER_KINAH_OFFSET, ref Kinah, 4, 0);

            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_CLASS_OFFSET, ref pclass, 2, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_BAGSLOTS_OFFSET, ref bagslots, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress - PLAYER_GUID_OFFSET, ref guid, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_LVL_OFFSET, ref lvl, 2, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_EXP_OFFSET, ref xp, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_EXPLVL_OFFSET, ref MaxXP, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_HP_OFFSET, ref hp, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_MAXHP_OFFSET, ref maxhp, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_MP_OFFSET, ref MP, 4, 0);
            ReadProcessMemory(hProc, playerInfoAddress + PLAYER_MAXMP_OFFSET, ref MaxMP, 4, 0);

            label4.Text = lvl.ToString();
            label5.Text = xp.ToString();
            label6.Text = hp.ToString();
            TxtBoxMsg("pGUIDInfoAddress2 = 0x" + ((playerInfoAddress - PLAYER_GUID_OFFSET) - gamedll).ToString("X"));

        }

        [DllImport("ByteSigScan.dll", EntryPoint = "InitializeSigScan")]
        public static extern void InitializeSigScan(uint iPID, [MarshalAs(UnmanagedType.LPStr)] string szModule);
        [DllImport("ByteSigScan.dll", EntryPoint = "SigScan")]
        public static extern UInt32 SigScan([MarshalAs(UnmanagedType.LPStr)] string Pattern, int Offset);
        [DllImport("ByteSigScan.dll", EntryPoint = "FinalizeSigScan")]
        public static extern void FinalizeSigScan();


        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, ref uint lpBuffer, int dwSize, int lpNumberOfBytesRead);


    }






}
