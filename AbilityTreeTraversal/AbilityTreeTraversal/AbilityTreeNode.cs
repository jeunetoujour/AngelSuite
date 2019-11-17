using System;
using System.Collections.Generic;
using System.Text;

using MemoryLib;
using AionMemory;

namespace WindowsFormsApplication1
{
    class AbilityTreeNode
    {
        public int Ptr { get; private set; }

        public int LeftPtr { get; private set; }
        public int ParentPtr { get; private set; }
        public int RightPtr { get; private set; }

        public int AbilityID { get; private set; }

        public int ValuePtr { get; private set; }

        public int CheckValue1 { get; private set; }
        public bool CheckValue2 { get; private set; }
        public bool CheckValue3 { get; private set; }

        public AbilityTreeNode Left
        {
            get
            {
                if (left == null)
                    left = new AbilityTreeNode(LeftPtr);
                return left;
            }
        }
        AbilityTreeNode left;

        public AbilityTreeNode Parent
        {
            get
            {
                if (parent == null)
                    parent = new AbilityTreeNode(ParentPtr);
                return parent;
            }
        }
        AbilityTreeNode parent;

        public AbilityTreeNode Right
        {
            get
            {
                if (right == null)
                    right = new AbilityTreeNode(RightPtr);
                return right;
            }
        }
        AbilityTreeNode right;

        public int AbilityPtr
        {
            get
            {
                if (CheckValue1 != 1)
                    return 0;

                try
                {
                    int next = ValuePtr;
                    next = Memory.ReadInt(Process.handle, next);
                    next = Memory.ReadInt(Process.handle, next + 0x14);
                    next = Memory.ReadInt(Process.handle, next + 0x4);
                    next = Memory.ReadInt(Process.handle, next + 0x8);
                    return next;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int AbilityCooldownTS
        {
            get
            {
                int abilityPtr = AbilityPtr;
                if (abilityPtr == 0)
                    return 0;

                return Memory.ReadInt(Process.handle, abilityPtr + 0x40);
            }
        }

        public int AbilityCooldownMax
        {
            get
            {
                int abilityPtr = AbilityPtr;
                if (abilityPtr == 0)
                    return 0;

                return Memory.ReadInt(Process.handle, abilityPtr + 0x3c);
            }
        }

        public AbilityTreeNode(int ptr)
        {
            Ptr = ptr;
            Update();
        }

        public void Update()
        {
            if (Ptr != 0)
            {
                LeftPtr = Memory.ReadInt(Process.handle, Ptr);
                ParentPtr = Memory.ReadInt(Process.handle, Ptr + 0x04);
                RightPtr = Memory.ReadInt(Process.handle, Ptr + 0x08);

                AbilityID = Memory.ReadInt(Process.handle, Ptr + 0x0C);

                ValuePtr = Memory.ReadInt(Process.handle, Ptr + 0x14);

                CheckValue1 = Memory.ReadInt(Process.handle, Ptr + 0x18);
                CheckValue2 = Memory.ReadByte(Process.handle, Ptr + 0x1C) == 1;
                CheckValue2 = Memory.ReadByte(Process.handle, Ptr + 0x1C) == 1;
            }
        }
    }
}
