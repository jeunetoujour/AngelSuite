
using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>
    /// Description of EntityList.
    /// </summary>
    public class EntityList : IEnumerable<Entity>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        HashSet<int> found = null;

        const int stop = -842150451; // 0xCDCDCDCD
        public uint ENTITYLIST_OFFSET = 0;//xA32C40;

        public EntityList(uint offset)
        {
            ENTITYLIST_OFFSET = offset;
        }
        public EntityList()
        {
           
        }

        public void Update()
        {
            found = new HashSet<int>();

            int basePtr = Process.Modules.Game;

            int entityMap = (int)Memory.ReadUInt(Process.handle, (uint)(basePtr + ENTITYLIST_OFFSET)); //NEED NEW OFFSET
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
