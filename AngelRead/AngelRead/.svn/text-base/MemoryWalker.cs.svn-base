using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MemoryLib;

namespace AionMemory
{
    public class MemoryWalker
    {
        public MemoryWalker(IntPtr processHandle)
        {
            this.handle = processHandle;
            this.ptr = 0;
        }

        public MemoryWalker(MemoryWalker otherMemoryWalker)
        {
            this.handle = otherMemoryWalker.handle;
            this.ptr = otherMemoryWalker.ptr;
        }

        private MemoryWalker(IntPtr processHandle, int ptr)
            : this(processHandle)
        {
            this.ptr = ptr;
        }

        public int Ptr
        {
            get { return ptr; }
        }
        int ptr;

        IntPtr handle;

        public MemoryWalker this[int offset]
        {
            get
            {
                return new MemoryWalker(handle, ptr == 0 ? offset : this.Int + offset);
            }
        }

        #region Read Memory

        public byte Byte
        {
            get { return Memory.ReadByte(handle, ptr); }
        }

        public double Double
        {
            get { return Memory.ReadDouble(handle, ptr); }
        }

        public float Float
        {
            get { return Memory.ReadFloat(handle, ptr); }
        }

        public int Int
        {
            get { return Memory.ReadInt(handle, ptr); }
        }

        public long Int64
        {
            get { return Memory.ReadInt64(handle, ptr); }
        }

        public sbyte SByte
        {
            get { return Memory.ReadSByte(handle, ptr); }
        }

        public short Short
        {
            get { return Memory.ReadShort(handle, ptr); }
        }

        public uint UInt
        {
            get { return Memory.ReadUInt(handle, ptr); }
        }

        public ulong ULong
        {
            get { return Memory.ReadUInt64(handle, ptr); }
        }

        public ushort UShort
        {
            get { return Memory.ReadUShort(handle, ptr); }
        }

        public string WStdString
        {
            get { return Memory.ReadWStdString(handle, ptr); }
        }

        public bool ReadMemory(ref byte[] buffer)
        {
            return Memory.ReadMemory(handle, ptr, ref buffer);
        }

        #endregion

        #region Write Memory

        public bool ReadMemory(ref byte[] buffer, int size)
        {
            return Memory.ReadMemory(handle, ptr, ref buffer, size);
        }

        public string ReadString(int length)
        {
            return Memory.ReadString(handle, ptr, length);
        }

        public string ReadString(int length, bool isUnicode)
        {
            return Memory.ReadString(handle, ptr, length, isUnicode);
        }

        public bool WriteMemory(string value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(UInt64 value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(Int64 value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(double value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(float value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(sbyte value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(byte value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(short value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(ushort value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(int value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(uint value)
        {
            return Memory.WriteMemory(handle, ptr, value);
        }

        public bool WriteMemory(byte[] buffer, int size)
        {
            return Memory.WriteMemory(handle, ptr, buffer, size);
        }

        public bool WriteMemory(byte[] buffer)
        {
            return Memory.WriteMemory(handle, ptr, buffer);
        }

        #endregion

    }
}
