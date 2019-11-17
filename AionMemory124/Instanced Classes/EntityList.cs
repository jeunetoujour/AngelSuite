/*
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
    /// Description of EntityList.
    /// </summary>
    public class EntityList : IEnumerable<Entity>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        HashSet<int> found = null;

        const int stop = -842150451; // 0xCDCDCDCD

        public EntityList()
        {
        }

        public void Update()
        {
            found = new HashSet<int>();

            int basePtr = Process.Modules.Game;

            int entityMap = (int)Memory.ReadUInt(Process.handle, (uint)(basePtr + 0xA2FC38)); //NEED NEW OFFSET
            int entityArray = Memory.ReadInt(Process.handle, (uint)(entityMap + 0x48));
            int entityArrayCount = Memory.ReadInt(Process.handle, (uint)(entityMap + 0x58));

            for (int i = 0; i < entityArrayCount; i++)
            {
                int struct1Node = Memory.ReadInt(Process.handle, (uint)(entityArray + (i * 4)));
                TraverseNode(struct1Node);
            }

            // Remove entities not present in memory anymore
            // Update entities still present
            // Add new entities and update them
            List<int> entitiesToRemove = new List<int>();

            foreach (var entity in entities.Values)
            {
                if (found.Contains(entity.PtrEntity))
                    entity.Update();
                else
                    entitiesToRemove.Add(entity.PtrEntity);
            }

            foreach (var entityPtr in entitiesToRemove)
                entities.Remove(entityPtr);

            foreach (var newEntityPtr in found.Except(entities.Keys))
                entities[newEntityPtr] = new Entity(newEntityPtr);
        }

        void TraverseNode(int nodePtr)
        {
            try
            {
                int entityPtr = Memory.ReadInt(Process.handle, (uint)(nodePtr + 12));

                if (entityPtr == 0 || entityPtr == stop || !found.Add(entityPtr))
                    return;

                int leftPtr = Memory.ReadInt(Process.handle, (uint)nodePtr);
                int rightPtr = Memory.ReadInt(Process.handle, (uint)(nodePtr + 4));

                if (leftPtr != 0 && leftPtr != stop)
                {
                    TraverseNode(leftPtr);
                }

                if (rightPtr != 0 && rightPtr != stop)
                {
                    TraverseNode(rightPtr);
                }
            }
            catch (Exception)
            {
                // uh oh, changing memory structure?
                // lets just back away slowly.
            }
        }

        public Entity this[int entityPtr]
        {
            get
            {
                return entities[entityPtr];
            }
        }


        #region IEnumerable<Entity> Members

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.Values.GetEnumerator();
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
