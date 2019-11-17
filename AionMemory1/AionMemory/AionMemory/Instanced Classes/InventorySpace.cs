using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AionMemory;
using MemoryLib;

namespace AionMemory
{
    public static class InventorySpace
    {
        /// <summary>
        /// The memory offset that stores the "XX/YY" label on the inventory UI component.
        /// Do not use const here, since we expect this to change each patch -
        /// we want to prevent other assemblies from copying the value and having to be rebuilt
        /// </summary>
        private static int inventorySpaceLabelOffset = 0x75118D04;

        /// <summary>
        /// Returns the raw "XX/YY" label on the inventory UI component.
        /// </summary>
        private static string GetLabel()
        {
            //Over-reading the memory block is valid here
            //The implementation will return a trimmed string
            return Memory.ReadString(Process.handle, (uint)(Process.Modules.Game + inventorySpaceLabelOffset), 32, true);
        }

        /// <summary>
        /// Parses the "XX/YY" label on the inventory UI component.
        /// </summary>
        /// <returns>A string array, containing "XX" and "YY" respectively, where "XX" is the
        /// number of items currently held and "YY" is the maximum capacity.</returns>
        private static string[] GetParsedLabel()
        {
            return GetLabel().Split('/');
        }

        /// <summary>
        /// Finds the number of items currently in the player's inventory.
        /// </summary>
        /// <returns>The number of items currently in the player's inventory</returns>
        public static int GetCurrentItemCount()
        {
            return Int32.Parse(GetParsedLabel()[0]);
        }

        /// <summary>
        /// Finds the maximum capacity of the player's inventory.
        /// </summary>
        /// <returns>The maximum capacity of the player's inventory</returns>
        public static int GetMaximumCapacity()
        {
            return Int32.Parse(GetParsedLabel()[1]);
        }

        /// <summary>
        /// Determine whether or not the player can carry any more items.
        /// </summary>
        /// <returns>False if the player has room for more items, true otherwise</returns>
        public static bool IsFull()
        {
            return Int32.Parse(GetParsedLabel()[0]) == Int32.Parse(GetParsedLabel()[1]);
        }
    }
}
