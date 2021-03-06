﻿/*
    Copyright © 2009, AionHacker.net
    All rights reserved.
    http://www.aionhacker.net
    http://www.assembla.com/spaces/AionMemory


    This file is part of AionMemory.

    AionMemory is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AionMemory is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AionMemory.  If not, see <http://www.gnu.org/licenses/>.
*/

// Custom Includes
using MemoryLib;

// Standard Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AionMemory
{
    /// <summary>
    /// AION process handler.
    /// </summary>
    public class Process
    {

        #region Structs

        /// <summary>
        /// Contains base addresses for AION's essential modules.
        /// </summary>
        public struct ModuleBase
        {
            public int Game;
            public int CryEntitySystem;
            public int Cry3DEngine;

            /// <summary>
            /// Struct instance initializer.
            /// </summary>
            /// <param name="GenerateMembers">Retrieve base addresses and set struct values accordingly if TRUE.</param>
            public ModuleBase(bool GenerateMembers)
            {
                this.Game = GetModuleBase("Game.dll");
                this.CryEntitySystem = GetModuleBase("CryEntitySystem.dll");
                this.Cry3DEngine = GetModuleBase("Cry3DEngine.dll");
            }
        }

        #endregion

        /// <summary>Handle of current AION process.</summary>
        public static IntPtr handle;
        /// <summary>Contains base addresses for AION's essential modules.</summary>
        public static ModuleBase Modules;
        /// <summary>Address of player's macro slot 1</summary>
        public static int macro1;
        /// <summary>Handle of current AION window.</summary>
        public static IntPtr whandle;

        /// <summary>
        /// Open current AION process for reading/writing.
        /// </summary>
        /// <returns>TRUE if success.</returns>
        public static bool Open()
        {
            
            handle = Memory.OpenProcess(Memory.GetProcessIdByProcessName("aion.bin"));
            whandle = Memory.FindWindowByTitle("AION Client");
            if (handle != IntPtr.Zero)
                Modules = new ModuleBase(true);
            return (handle != IntPtr.Zero);
        }
        /// <summary>
        /// Open AION process by pid for reading/writing.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        /// <returns>TRUE if success.</returns>
        public static bool Open(int pid)
        {
            handle = Memory.OpenProcess(pid);
            return (handle != IntPtr.Zero);
        }
        /// <summary>
        /// Open AION process by character name for reading/writing.
        /// </summary>
        /// <param name="name">Character name.</param>
        /// <returns>TRUE if success.</returns>
        public static bool Open(string name)
        {
            System.Diagnostics.Process[] pArray  = System.Diagnostics.Process.GetProcessesByName("aion.bin");
            foreach (System.Diagnostics.Process proc in pArray)
            {
                Open(proc.Id);
                using (Player player = new Player())
                {
                    if (player.Name.ToLower() == name.ToLower())
                    {
                        return true;
                    }
                    else
                        Close();
                }
            }
            return (handle != IntPtr.Zero);
        }

        /// <summary>
        /// Closes handle generated by Open().
        /// </summary>
        /// <returns>TRUE if success.</returns>
        public static bool Close()
        {
            return Memory.CloseHandle(handle);
        }

        /// <summary>
        /// Gets address of player's macro slot 1.  *TO BE REMOVED*
        /// </summary>
        public static void GetMacro1Addr()
        {
            int addr = 0;
            addr = Memory.ReadInt(handle, (Modules.Game + 0x8E3930), false);
            addr = Memory.ReadInt(handle, (addr + 0x7B0), false);
            addr = Memory.ReadInt(handle, (addr + 0x0), false);
            addr = Memory.ReadInt(handle, (addr + 0x10), false);
            addr = Memory.ReadInt(handle, (addr + 0x9C), false);
            addr = Memory.ReadInt(handle, (addr + 0x0), false);
            addr += 0x04;

            macro1 = addr;
        }

        /// <summary>
        /// Gets base address of module.
        /// </summary>
        /// <param name="modulename">Module to get base address of.</param>
        /// <returns>Base address if module found, else 0.</returns>
        public static int GetModuleBase(string modulename)
        {

            System.Diagnostics.Process[] HandleP = System.Diagnostics.Process.GetProcessesByName("aion.bin");

            foreach (System.Diagnostics.ProcessModule Module in HandleP[0].Modules)
            {
                if (modulename == Module.ModuleName)
                {
                    return Module.BaseAddress.ToInt32();
                }
            }
            return 0;
        }
    }
}
