using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.IO;

using MemoryLib;
using System.Diagnostics;

using System.Collections;

namespace AionBot
{
    class aionUtil
    {
        // Fields
        private const int has_target = 0x4f6904;
        public static keyEvent keyPress = new keyEvent();
        public static mouseEvent mouseClick = new mouseEvent();
        public static Player player = new Player();
        private const int player_hp = 0x8eeec0;
        private const int player_max_hp = 0x8eeebc;
        private const int player_rot = 0x8e40c8;
        private const int player_x = 0x8e6948;
        private const int player_y = 0x8e694c;
        private const int player_z = 0x8e336c;
        public static Player target = new Player();
        private const int target_guid = 0x4f6900;
        private const int target_ptr = 0x4f68fc;

        // Methods
        public static void actionSeq(string[] actionArray, int delay)
        {
            keyEvent event2 = new keyEvent();
            foreach (string str in actionArray)
            {
                if (str.Split(new char[] { ' ' })[0].Equals("A"))
                {
                    event2.alt(str.Split(new char[] { ' ' })[1]);
                }
                else if (str.Split(new char[] { ' ' })[0].Equals("S"))
                {
                    event2.shift(str.Split(new char[] { ' ' })[1]);
                }
                else
                {
                    event2.pressKey(str, 500);
                }
                Thread.Sleep(delay);
            }
        }

        public static Player[] buildPathArray(string fileName)
        {
            string str;
            int index = 0;
            int num2 = 0;
            StreamReader reader = new StreamReader(fileName);
            while ((str = reader.ReadLine()) != null)
            {
                num2++;
            }
            reader.Close();
            Player[] playerArray = new Player[num2];
            StreamReader reader2 = new StreamReader(fileName);
            while ((str = reader2.ReadLine()) != null)
            {
                playerArray[index] = new Player();
                string[] strArray = str.Split(new char[] { ',' });
                playerArray[index].X = Convert.ToSingle(strArray[0]);
                playerArray[index].Y = Convert.ToSingle(strArray[1]);
                playerArray[index].Z = Convert.ToSingle(strArray[2]);
                index++;
            }
            if (playerArray == null)
            {
                
            }
            return playerArray;
        }

        public static void faceWaypoint(Player waypoint, int base_addr)
        {
            setPlayerInfo(base_addr);
            float num = 0f;
            num = (float)Math.Atan2((double)(waypoint.Y - player.Y), (double)(waypoint.X - player.X));
            Console.WriteLine(string.Concat(new object[] { "waypoint.y = ", waypoint.X, "player.y = ", player.X }));
            num = (num * 180f) / 3.141593f;
            num += 100f;
            Console.WriteLine("Degree: " + num);
            IntPtr pHandle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin"));
            Memory.WriteMemory(pHandle, (long)(base_addr + 0x8e40c8), num);
            Memory.CloseHandle(pHandle);
            keyPress.pressLeft(10);
            keyPress.pressRight(10);
        }

        public static void followPath(int base_addr, string fileName)
        {
            Player[] psource = buildPathArray(fileName);

            List<Player> source = new List<Player>();
            source.AddRange(psource);

            int num = source.Count;

            int index = 0;
            while (index < (num - 1))
            {
                faceWaypoint(source[index], base_addr);
                keyPress.Run();
                if (getDistance(source[index], base_addr) < 2.5)
                {
                    index++;
                    faceWaypoint(source[index], base_addr);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            keyPress.stopRun();
        }

        public static int getBaseAddr()
        {
            int num = 0;
            Process process = new Process();
            process = Process.GetProcessesByName("Aion.bin")[0];
            ProcessModuleCollection modules = process.Modules;
            foreach (ProcessModule module in modules)
            {
                if (module.ModuleName == "Game.dll")
                {
                    num = module.BaseAddress.ToInt32();
                }
            }
            return num;
        }

        public static double getDistance(Player waypoint, int base_addr)
        {
            setPlayerInfo(base_addr);
            return Math.Sqrt((Math.Pow((double)(player.X - waypoint.X), 2.0) + Math.Pow((double)(player.Y - waypoint.Y), 2.0)) + Math.Pow((double)(player.Z - waypoint.Z), 2.0));
        }

        public static uint getObjectBase(ulong GUID)
        {
            IntPtr pHandle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("wow.exe"));
            uint num = Memory.ReadUInt(pHandle, 0x11ca260L);
            uint num2 = Memory.ReadUInt(pHandle, (long)(num + 0x2864));
            uint num3 = Memory.ReadUInt(pHandle, (long)(num2 + 0xac));
            uint num4 = num3;
            ulong num5 = Memory.ReadUInt64(pHandle, (long)(num2 + 0xc0));
            while ((num3 != 0) && ((num3 % 1) == 0))
            {
                ulong num6 = Memory.ReadUInt64(pHandle, (long)(num3 + 0x30));
                uint num7 = Memory.ReadUInt(pHandle, (long)(num3 + 8));
                if (num6 == GUID)
                {
                    return num7;
                }
                num4 = Memory.ReadUInt(pHandle, (long)(num3 + 60));
                if (num4 == num3)
                {
                    break;
                }
                num3 = num4;
            }
            Memory.CloseHandle(pHandle);
            return 0;
        }

        public static int getPlayerHealth(int base_addr)
        {
            return Memory.ReadInt(Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin")), (long)(base_addr + 0x8eeec0));
        }

        public static int getPlayerMaxHealth(int base_addr)
        {
            return Memory.ReadInt(Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin")), (long)(base_addr + 0x8eeebc));
        }

        public static int getTargetHealth(int base_addr)
        {
            IntPtr pHandle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin"));
            int num = Memory.ReadInt(pHandle, (long)(base_addr + 0x4f68fc));
            if (num != 0)
            {
                int num2 = Memory.ReadInt(pHandle, (long)(num + 0x1c4));
                return Memory.ReadByte(pHandle, (long)(num2 + 0x34));
            }
            return num;
        }

        public static int getTargetStatus(int base_addr)
        {
            IntPtr pHandle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin"));
            int num = Memory.ReadInt(pHandle, (long)(base_addr + 0x4f68fc));
            return Memory.ReadInt(pHandle, (long)(num + 8));
        }

        public static int hasTarget(int base_addr)
        {
            return Memory.ReadInt(Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin")), (long)(base_addr + 0x4f6904));
        }

        public static void setPlayerInfo(int base_addr)
        {
            IntPtr pHandle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("AION.bin"));
            float num = Memory.ReadFloat(pHandle, (long)(base_addr + 0x8e6948));
            float num2 = Memory.ReadFloat(pHandle, (long)(base_addr + 0x8e694c));
            float num3 = Memory.ReadFloat(pHandle, (long)(base_addr + 0x8e336c));
            float num4 = Memory.ReadFloat(pHandle, (long)(base_addr + 0x8e40c8));
            player.X = num;
            player.Y = num2;
            player.Z = num3;
            player.Face = ((float)(3.1415926535897931 * num4)) / 180f;
            Memory.CloseHandle(pHandle);
        }
    }

}
