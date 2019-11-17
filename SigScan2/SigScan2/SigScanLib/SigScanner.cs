using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SigScanLib
{
    public class SigScanner
    {
        public SigScanner()
        {
            Results = new List<ScanResultData>();
        }

        #region PROPERTIES
        /// <summary>
        /// List of scan results
        /// </summary>
        public List<ScanResultData> Results;
        #endregion

        #region EVENTS
        /// <summary>
        /// Delegate for ScanException event.
        /// </summary>
        /// <param name="sender">The SigScanner object that fired the exception.</param>
        /// <param name="e">The Exception object.</param>
        public delegate void ScanExceptionEventHandler(object sender, Exception e);
        /// <summary>
        /// ScanException event is fired when an exception is caught within SigScanner
        /// </summary>
        public event ScanExceptionEventHandler ScanException;

        /// <summary>
        /// Delegate for ScanTotalProgressChanged event.
        /// </summary>
        /// <param name="progress">The current progress percentage.</param>
        public delegate void ScanTotalProgressChangedEventHandler(object sender, float progress);
        /// <summary>
        /// Fired when the scan total progress percentage changes.
        /// </summary>
        public event ScanTotalProgressChangedEventHandler ScanTotalProgressChanged;

        /// <summary>
        /// Delegate for ScanSignatureProgressChanged event.
        /// </summary>
        /// <param name="progress">Progress percentage.</param>
        public delegate void ScanSignatureProgressChangedEventHandler(object sender, float progress);

        /// <summary>
        /// ScanSignatureProgressChanged event. Fired when the scan progress for the current signature changes.
        /// </summary>
        public event ScanSignatureProgressChangedEventHandler ScanSignatureProgressChanged;

        /// <summary>
        /// Delegate for ScanComplete event.
        /// </summary>
        /// <param name="sender">SigScanner sender object.</param>
        /// <param name="results">ScanResultData object.</param>
        public delegate void ScanCompleteEventHandler(object sender, List<ScanResultData> results);
        /// <summary>
        /// ScanComplete event, is fired when the scan has been completed and contains the results
        /// </summary>
        public event ScanCompleteEventHandler ScanComplete;
        #endregion

        #region MEMORY
        #region Imports
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess,
                                                     UIntPtr lpBaseAddress,
                                                    [Out()] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400)
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Int32 CloseHandle(IntPtr hObject);
        #endregion

        #region Read Memory
        private byte[] ReadMemory(long MemoryAddress, int size)
        {
            byte[] buffer = new byte[size];

            try
            {
                bool success = ReadProcessMemory(ProcessHandle, (UIntPtr)MemoryAddress, buffer, (UIntPtr)size, IntPtr.Zero);
            }
            catch (Exception e)
            {
                ScanException(this, e);
            }
            return buffer;
        }

        private int ReadInt(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(int)];
            buffer = ReadMemory(MemoryAddress, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        private uint ReadUInt(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(uint)];
            buffer = ReadMemory(MemoryAddress, 4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        private Int64 ReadInt64(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(Int64)];
            buffer = ReadMemory(MemoryAddress, 8);
            return BitConverter.ToInt64(buffer, 0);
        }

        private UInt64 ReadUInt64(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(UInt64)];
            buffer = ReadMemory(MemoryAddress, 8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        private double ReadDouble(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(double)];
            buffer = ReadMemory(MemoryAddress, 8);
            return BitConverter.ToDouble(buffer, 0);
        }

        private short ReadShort(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(short)];
            buffer = ReadMemory(MemoryAddress, 2);
            return BitConverter.ToInt16(buffer, 0);
        }

        private float ReadFloat(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(float)];
            buffer = ReadMemory(MemoryAddress, 4);
            return BitConverter.ToSingle(buffer, 0);
        }

        private string ReadString(long MemoryAddress, int bytesToRead)
        {
            byte[] buffer = new byte[bytesToRead];
            buffer = ReadMemory(MemoryAddress, bytesToRead);
            return ConvertUtil.ByteToString(buffer);
        }

        private byte ReadByte(long MemoryAddress)
        {
            byte[] buffer = new byte[sizeof(byte)];
            buffer = ReadMemory(MemoryAddress, 1);
            return buffer[0];
        }

        private byte[] ReadBytes(long MemoryAddress, int bytes)
        {
            byte[] buffer = new byte[sizeof(byte) * bytes];
            buffer = ReadMemory(MemoryAddress, bytes);
            return buffer;
        }
        #endregion


#endregion

        #region Private Members
        /// <summary>
        /// handle to the process we are reading from
        /// </summary>
        private IntPtr ProcessHandle { get; set; }

        /// <summary>
        /// Have we opened the process yet
        /// </summary>
        private bool processOpened { get; set; }

        /// <summary>
        /// Name of the process we want to open
        /// </summary>
        private string processName { get; set; }
        
        /// <summary>
        /// Name of the module we want to scan
        /// </summary>
        private string moduleName { get; set; }
        #endregion

        /// <summary>
        /// Scans for a list of byte signatures and returns the result as a list of ScanResultData
        /// </summary>
        /// <param name="sigList"></param>
        /// <returns></returns>
        public void SignatureListScan(List<ByteSignature> sigList, Process proc, ProcessModule mod)
        {
            // create results list
            Results = new List<ScanResultData>();

            // keep count of how many sigs we've scanned for
            int sigCt = 0;

            // open this process
            OpenProcess(proc);

            // go through eahc signatuer and scan
            foreach (ByteSignature sig in sigList)
            {
                try
                {
                    // perform a signature scan
                    ScanResultData scan = SignatureScan(sig, proc, mod);

                    // add this scan result to results list
                    Results.Add(scan);

                    // increase sig count
                    sigCt++;

                    // report scan total progress changed
                    ScanTotalProgressChanged(this, (Convert.ToSingle(sigCt) / Convert.ToSingle(sigList.Count) * 100));
                }
                catch { }
            }

            // fire finished event
            ScanComplete(this, Results);
        }

        /// <summary>
        /// Scans for the specified signature.
        /// </summary>
        /// <param name="sig">Byte signature to scan for.</param>
        /// <returns>ScanResultData object.</returns>
        private ScanResultData SignatureScan(ByteSignature sig, Process proc, ProcessModule mod)
        {
            // return new ScanResultData();

            // create default 'failure' result
            ScanResultData result = new ScanResultData();
            result.Name = sig.Name;

            //  match
            Match match;
            Regex regex;

            uint BaseAddr = (uint)mod.BaseAddress.ToInt32();
            uint StopAddr = (uint)(mod.BaseAddress.ToInt32() + mod.ModuleMemorySize);

            // current position in memory
            uint position = BaseAddr;
            uint searchMemSize = StopAddr - BaseAddr;

            // current scan progress
            float prog = 0f;

            string opcode = sig.Signature.Substring(0, 2);
            string curOpcode = string.Empty;
            string hexString;
            byte[] bytes;

            try
            {
                while (position < StopAddr)
                {
                    // go through memory until we hit a byte matching our signatures first byte
                    while (!ReadByte(position).ToString("X2").Equals(opcode))
                    {
                        position++;
                    }

                    // read current position as hex string
                    bytes = ReadBytes(position, sig.SignatureTrimmed.Length);
                    hexString = ConvertUtil.BytesToHexString(bytes);

                    // see if it matches
                    regex = new Regex(sig.RegExpStr);
                    match = regex.Match(hexString);
                    if (match.Success)
                    {
                        // get the bytes of the value we're looking for
                        string hexVal = hexString.Substring(sig.TargetByteStartIndex, sig.TargetByteCount);

                        // reverse the bytes of this string
                        string reversedHexVal = string.Empty;
                        int bytesToReverse = hexVal.Length / 2;
                        for (int i = bytesToReverse; i > 0; i--)
                        {
                            reversedHexVal += hexVal.Substring((i * 2) - 2, 2);
                        }

                        // subtract the base addr from this to get the offset
                        int val = ConvertUtil.HexStringToInt(reversedHexVal);

                        uint offset = (uint)val - BaseAddr;
                        result.Values.Add(offset.ToString("X2"));

                        // flag success
                        result.Result = ScanResult.Success;

                        // return result
                        return result;
                    }

                    // increase position
                    position++;

                    // report progress
                    prog = (Convert.ToSingle(position - BaseAddr) / Convert.ToSingle(searchMemSize)) * 100;
                    ScanSignatureProgressChanged(this, prog);
                }
            }
            catch { }

            // failure
            ScanSignatureProgressChanged(this, 100);
            return result;
        }

        private bool OpenProcess(Process proc)
        {
            bool success = false;

            // set up our process access type
            ProcessAccessType access;
            access = ProcessAccessType.PROCESS_VM_READ
                | ProcessAccessType.PROCESS_VM_WRITE
                | ProcessAccessType.PROCESS_VM_OPERATION;

            try
            {
                // get the process handle
                ProcessHandle = OpenProcess((uint)access, true, proc.Id);
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
