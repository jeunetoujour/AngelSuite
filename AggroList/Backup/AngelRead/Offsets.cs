using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AngelRead
{
    public class Offsets
    {
        [DllImport("ByteSigScan.dll", EntryPoint = "InitializeSigScan")]
        public static extern void InitializeSigScan(uint iPID, [MarshalAs(UnmanagedType.LPStr)] string szModule);
        [DllImport("ByteSigScan.dll", EntryPoint = "SigScan")]
        public static extern UInt32 SigScan([MarshalAs(UnmanagedType.LPStr)] string Pattern, int Offset);
        [DllImport("ByteSigScan.dll", EntryPoint = "FinalizeSigScan")]
        public static extern void FinalizeSigScan();
        
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

        //string playerInfoSig = "8b5720535068XXXXXXXX8d4f3651";
        //string abilityinfo = "a1XXXXXXXX8b88380100008b903c0100008b83000500008b30bf1cbf0d";
        //string entityinfo = "08008B0DXXXXXXXX3BcE740c";
        //string targetinfo = "E8????????8B35XXXXXXXXE8????????E8????????";
        //string inventoryinfo = "833dXXXXXXXX005356578bf8";
                
        string playerInfoSig = "5268XXXXXXXX8d4336";
        string abilityinfo = "a1XXXXXXXX50893d14??????e8a3ee2200";
        string entityinfo = "8B0DXXXXXXXX85c9750433c0eb088b1150"; 
        string targetinfo = "E8????????8B35XXXXXXXXE8????????E8????????";
        string inventoryinfo = "833dXXXXXXXX005356578bf8";
        string pguidinfo = "535633db391dXXXXXXXX578bf9";
        string hotbarinfo = "8DB8XXXXXXXX85FF????????????8B47??8B4F??8B17505152";


        public uint playerInfoAddress = 0;
        public uint pGUIDInfoAddress = 0;
        public uint abilityInfoAddress = 0;
        public uint entityInfoAddress = 0;
        public uint targetInfoAddress = 0;
        public uint inventoryInfoAddress = 0;
        public uint reactionInfoAddress = 0;
        public uint hotkeyInfoAddress = 0;
        public uint resInfoAddress = 0;
        public uint vendorInfoAddress = 0;
        public uint sellInfoAddress = 0;
        public uint buffInfoAddress = 0;
        public uint gatheringInfoAddress = 0;

        public uint gamedll = 0;
        public uint aionpid = 0;

        public void Update()
        {
            InitializeSigScan(aionpid, "Game.dll");
            playerInfoAddress = SigScan(playerInfoSig, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            pGUIDInfoAddress = SigScan(pguidinfo, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            abilityInfoAddress = SigScan(abilityinfo, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            entityInfoAddress = SigScan(entityinfo, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            targetInfoAddress = SigScan(targetinfo, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            inventoryInfoAddress = SigScan(inventoryinfo, 0) - gamedll;
            FinalizeSigScan();

            InitializeSigScan(aionpid, "Game.dll");
            hotkeyInfoAddress = SigScan(hotbarinfo, 0) - gamedll;
            FinalizeSigScan();
            

            reactionInfoAddress = inventoryInfoAddress + 0x170 ;
            //hotkeyInfoAddress = inventoryInfoAddress - 0x1820;
            resInfoAddress = inventoryInfoAddress + 0x12C;
            vendorInfoAddress = inventoryInfoAddress + 0x4C;
            sellInfoAddress = inventoryInfoAddress + 0x54;
            buffInfoAddress = inventoryInfoAddress + 0x40;
            gatheringInfoAddress = inventoryInfoAddress + 0x5C;
            //CHAT   A686EC
            //GAT    A686B4
            //INV    A68658
            //RES  0xA68784  12C
            //VENDOR A686A4  4C
            //SELL 0xA686AC
            //System.Windows.Forms.MessageBox.Show(abilityInfoAddress.ToString("X"));
        }

        public Offsets()
        {
            System.Diagnostics.Process[] ProcessList = System.Diagnostics.Process.GetProcesses();
            String ProcessName;

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
                        aionpid = (uint)ProcessList[i].Id;
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
        }//OFFSETS

    }

}
