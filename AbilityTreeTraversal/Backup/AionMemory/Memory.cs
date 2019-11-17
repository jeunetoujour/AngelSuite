using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MemoryLib
{
    public class Memory
    {
        #region Constants
        /// <summary>
        /// Pass to OpenProcess to gain all access to an external process.
        /// </summary>
        public const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_DECOMMIT = 0x4000;
        public const uint PAGE_READWRITE = 0x04;
        public const uint PAGE_EXECUTE = 0x10;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        #endregion

        #region DllImports
        //From pinvoke.net
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        private static extern bool _CloseHandle(IntPtr hObject);
        /// <summary>
        /// Close a handle object (clean up your code).
        /// </summary>
        /// <param name="hObject">The object handle to be closed.</param>
        /// <returns>Returns true if success, false if not.</returns>
        public static bool CloseHandle(IntPtr hObject) { return _CloseHandle(hObject); }

        //From pinvoke.net
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        //From pinvoke.net
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        //From pinvoke.net
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx")]
        private static extern UIntPtr _VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        public static UIntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect)
        {
            return _VirtualAllocEx(hProcess, lpAddress, dwSize, flAllocationType, flProtect);
        }
        public static uint AllocateMemory(IntPtr hProcess, long lpAddress, uint dwSize)
        {
            return (uint)VirtualAllocEx(hProcess, (IntPtr)lpAddress, dwSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        }
        public static uint AllocateMemory(IntPtr hProcess, uint dwSize)
        {
            return AllocateMemory(hProcess, 0, dwSize);
        }

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        private static extern IntPtr _GetModuleHandle(string lpModuleName);
        public static IntPtr GetModuleHandle(string lpModuleName) { return _GetModuleHandle(lpModuleName); }

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        private static extern UIntPtr _GetProcAddress(IntPtr hModule, string procName);
        public static uint GetProcAddress(IntPtr hModule, string procName) { return (uint)_GetProcAddress(hModule, procName); }

        [DllImport("kernel32.dll", EntryPoint = "CreateRemoteThread")]
        private static extern IntPtr _CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, UIntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        public static IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, UIntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId)
        {
            return _CreateRemoteThread(hProcess, lpThreadAttributes, dwStackSize, lpStartAddress, lpParameter, dwCreationFlags, lpThreadId);
        }

        public static IntPtr CreateRemoteThread(IntPtr hProcess, long lpStartAddress, long lpParameter)
        {
            return CreateRemoteThread(hProcess, IntPtr.Zero, 0, (UIntPtr)lpStartAddress, (UIntPtr)lpParameter, 0, IntPtr.Zero);
        }

        public const int INFINITE = -1;
        public const int WAIT_OBJECT_0 = 0;
        [DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", SetLastError = true)]
        private static extern int _WaitForSingleObject(IntPtr hObject, int milliseconds);
        public static int WaitForSingleObject(IntPtr hObject, int milliseconds) { return _WaitForSingleObject(hObject, milliseconds); }
        public static int WaitForSingleObject(IntPtr hObject) { return _WaitForSingleObject(hObject, INFINITE); }

        [DllImport("kernel32.dll", EntryPoint = "GetExitCodeThread")]
        private static extern bool _GetExitCodeThread(IntPtr hThread, out uint dwExitCode);
        public static bool GetExitCodeThread(IntPtr hThread, out uint dwExitCode)
        {
            return _GetExitCodeThread(hThread, out dwExitCode);
        }
        public static uint GetExitCodeThread(IntPtr hThread)
        {
            uint dwExitCode;
            if (GetExitCodeThread(hThread, out dwExitCode))
                return dwExitCode;
            else
                return 0;
        }

        [DllImport("kernel32.dll", EntryPoint = "TerminateThread")]
        private static extern bool _TerminateThread(IntPtr hThread, uint dwExitCode);
        public static bool TerminateThread(IntPtr hThread, uint dwExitCode)
        {
            return _TerminateThread(hThread, dwExitCode);
        }
        public static bool TerminateThread(IntPtr hThread)
        {
            return _TerminateThread(hThread, 0);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(IntPtr lpSecurity, bool bManualReset, bool bInitialState, string lpEventName);
        public static IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpEventName)
        {
            return CreateEvent(IntPtr.Zero, bManualReset, bInitialState, lpEventName);
        }

        [DllImport("kernel32.dll", EntryPoint = "SetEvent")]
        private static extern bool _SetEvent(IntPtr hEvent);
        public static bool SetEvent(IntPtr hEvent) { return _SetEvent(hEvent); }

        [DllImport("kernel32.dll", EntryPoint = "ResetEvent")]
        private static extern bool _ResetEvent(IntPtr hEvent);
        public static bool ResetEvent(IntPtr hEvent) { return _ResetEvent(hEvent); }

        [DllImport("kernel32.dll", EntryPoint = "VirtualFreeEx")]
        private static extern bool _VirtualFreeEx(IntPtr hProcess, UIntPtr lpAddress, uint dwSize, uint dwFreeType);
        public static bool VirtualFreeEx(IntPtr hProcess, long lpAddress, uint dwSize, uint dwFreeType)
        {
            return _VirtualFreeEx(hProcess, (UIntPtr)lpAddress, dwSize, dwFreeType);
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr _FindWindow(string classname, string windowtitle);
        public static IntPtr FindWindow(string classname, string windowtitle)
        {
            return _FindWindow(classname, windowtitle);
        }

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        //public static bool IsWindowVisible(IntPtr hWnd) { return _IsWindowVisible(hWnd); }

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        private static extern int _GetWindowText(IntPtr hWnd, StringBuilder buf, int nMaxCount);
        /// <summary>
        /// Returns the window title of the specified window
        /// </summary>
        /// <param name="hWnd">A handle to the window</param>
        /// <param name="length">Length of the string to be returned</param>
        /// <returns></returns>
        public static string GetWindowTitle(IntPtr hWnd, int length)
        {
            StringBuilder str = new StringBuilder(length);
            _GetWindowText(hWnd, str, length);
            return str.ToString();
        }
        #endregion

        private static string lasterror = "";
        /// <summary>
        /// Contains a string describing the last error to occur.
        /// </summary>
        public string LastError { get { return lasterror; } /*set { lasterror = value; }*/ }

        private static bool error = false;
        /// <summary>
        /// True if something in the Memory class has errored.  If true when checked, Error is reset to false.
        /// </summary>
        public bool Error
        {
            get
            {
                if (!error)
                    return false;
                else
                {
                    error = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// The process object that contains its information
        /// </summary>
        public class MLProc
        {
            /// <summary>
            /// Internal window handle.
            /// </summary>
            private IntPtr hwnd;
            /// <summary>
            /// The process' window handle.
            /// </summary>
            public IntPtr hWnd { get { return hwnd; } /*set { hwnd = value; }*/ }

            private IntPtr phandle;
            /// <summary>
            /// The process' process handle.
            /// </summary>
            public IntPtr pHandle { get { return phandle; }/* set { phandle = value; }*/ }

            private int dwprocessid;
            /// <summary>
            /// The process' process ID.
            /// </summary>
            public int dwProcessId { get { return dwprocessid; } /*set { dwprocessid = value; }*/ }

            private bool isopen;
            /// <summary>
            /// Returns whether or not the process is open for read/write.
            /// </summary>
            public bool IsOpen { get { return isopen; } set { isopen = value; } }

            /// <summary>
            /// Opens the process for read/write and populate the window handle, process ID, and process handle.
            /// </summary>
            /// <param name="pid">The ID of the process to be opened.</param>
            public MLProc(int pid)
            {
                if (pid == 0)
                {
                    lasterror = "dwProcessId == 0";
                    error = true;
                    return;
                }

                dwprocessid = pid;

                phandle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
                if ((uint)phandle == 0)
                {
                    lasterror = "OpenProcess failed.";
                    error = true;
                    return;
                }

                WindowArray winenum = new WindowArray();

                foreach (IntPtr handle in winenum)
                    if (GetProcessIdByHWnd(handle) == pid)
                    {
                        hwnd = handle;
                        break;
                    }

                if (hwnd == IntPtr.Zero)
                {
                    lasterror = "Could not find window handle.";
                    error = true;
                    return;
                }

                isopen = true;

                winenum = null;
                GC.Collect();
            }

            /// <summary>
            /// Opens the process for read/write and populate the window handle, process ID, and process handle.
            /// </summary>
            /// <param name="hWnd">The window handle of the main window of the process to be opened.</param>
            public MLProc(IntPtr hWnd)
            {
                if (hWnd == IntPtr.Zero)
                {
                    lasterror = "hWnd == 0";
                    error = true;
                    return;
                }

                hwnd = hWnd;

                GetWindowThreadProcessId(hWnd, out dwprocessid);
                if (dwprocessid == 0)
                {
                    lasterror = "dwProcessId == 0";
                    error = true;
                    return;
                }

                phandle = OpenProcess(PROCESS_ALL_ACCESS, false, dwprocessid);
                if ((uint)phandle == 0)
                {
                    lasterror = "OpenProcess failed.";
                    error = true;
                    return;
                }

                isopen = true;
            }

            /// <summary>
            /// Converts the three process members into string format.
            /// </summary>
            /// <returns>A string containing the values of the three process members.</returns>
            public override string ToString() { return String.Format("hWnd: 0x{0:X08} | dwProcessId: {1} | pHandle: 0x{2:X08} | IsOpen: {3}", (uint)hwnd, dwprocessid, (uint)phandle, isopen.ToString()); }
        }

        public class MLProcesses
        {
            private ArrayList processes;

            /// <summary>
            /// The number of processes that have been opened.  Processes that are closed are not removed from the total of Count.
            /// </summary>
            public int Count { get { return processes.Count; } }

            /// <summary>
            /// Opens a process for read/write and adds it to the list of processes.
            /// </summary>
            /// <param name="dwProcessId">The ID of the process to be opened.</param>
            public int Open(int dwProcessId)
            {
                error = false;

                int count = processes.Count;
                MLProc proc = new MLProc(dwProcessId);
                if (error)
                    return -1;

                processes.Add(proc);
                return count;
            }

            /// <summary>
            /// Opens a process for read/write and adds it to the list of processes.
            /// </summary>
            /// <param name="hWnd">The window handle of the main window of the process to be opened.</param>
            public int Open(IntPtr hWnd) { return Open(GetProcessIdByHWnd(hWnd)); }

            /// <summary>
            /// Opens a process for read/write and adds it to the list of processes.
            /// </summary>
            /// <param name="classname">The classname of the main window of the process to be opened.  Can be null.</param>
            /// <param name="windowTitle">The window title of the main window of the process to be opened.  Can be null.</param>
            public int Open(string classname, string windowTitle) { return Open(FindWindow(classname, windowTitle)); }

            public int[] Open(int[] dwProcessIds)
            {
                error = false;

                if (dwProcessIds == null || dwProcessIds.Length == 0)
                {
                    lasterror = "Could not get Process Id.";
                    error = true;
                    return null;
                }

                int count = processes.Count;
                int[] clientindexes = new int[dwProcessIds.Length];

                for (int i = 0; i < clientindexes.Length; i++)
                    clientindexes[i] = Open(dwProcessIds[i]);

                if (error)
                    return null;
                else
                    return clientindexes;
            }

            /// <summary>
            /// Closes the process handle associated with the process.
            /// </summary>
            /// <param name="index">The zero-based index of the process in MLProcesses to be closed.</param>
            public void Close(int index)
            {
                MLProc proc = (MLProc)processes[index];
                CloseHandle(proc.pHandle);
                proc.IsOpen = false;
            }

            /// <summary>
            /// Closes the process handle associated with the process.
            /// </summary>
            /// <param name="hWnd">The window handle of the main window of the process to be closed.</param>
            public void Close(IntPtr hWnd)
            {
                foreach (MLProc proc in processes)
                    if (proc.hWnd == hWnd)
                    {
                        CloseHandle(proc.pHandle);
                        proc.IsOpen = false;
                        break;
                    }
            }

            /// <summary>
            /// Closes the process handle associated with the process.
            /// </summary>
            /// <param name="dwProcessId">The ID of the process to be closed.  (int)dwProcessId must be cast to (uint).</param>
            public void Close(uint dwProcessId)
            {
                foreach (MLProc proc in processes)
                    if (proc.dwProcessId == dwProcessId)
                    {
                        CloseHandle(proc.pHandle);
                        proc.IsOpen = false;
                        break;
                    }
            }

            /// <summary>
            /// Process object MLProc.
            /// </summary>
            /// <param name="index">The index of the process in MLProcesses.</param>
            /// <returns>MLProc object associated with the given index.</returns>
            public MLProc this[int index]
            {
                get
                {
                    if (processes.Count > index)
                        return (MLProc)processes[index];
                    else
                        return null;
                }
            }

            /// <summary>
            /// Process object MLProc.
            /// </summary>
            /// <param name="hWnd">The window handle associated with main window of the process.</param>
            /// <returns>MLProc object associated with the given window handle.</returns>
            public MLProc this[IntPtr hWnd]
            {
                get
                {
                    foreach (MLProc proc in processes)
                        if (proc.hWnd == hWnd)
                            return proc;

                    return null;
                }
            }

            /// <summary>
            /// Process object MLProc.
            /// </summary>
            /// <param name="dwProcessId">The ID of the process.</param>
            /// <returns>MLProc object associated with the given process ID.</returns>
            public MLProc this[uint dwProcessId]
            {
                get
                {
                    foreach (MLProc proc in processes)
                        if ((uint)proc.dwProcessId == dwProcessId)
                            return proc;

                    return null;
                }
            }

            public MLProcesses() { processes = new ArrayList(); }
            public MLProcesses(int capacity) { processes = new ArrayList(capacity); }
            public MLProcesses(ICollection c) { processes = new ArrayList(c); }
        }

        private MLProcesses procs;
        public MLProcesses Processes { get { return procs; } }

        public Memory() { procs = new MLProcesses(); }
        public Memory(int capacity) { procs = new MLProcesses(capacity); }
        public Memory(ICollection c) { procs = new MLProcesses(c); }

        #region ReadMemory
        public bool ReadMemory(int index, long Address, ref byte[] buffer, int size)
        {
            return ReadMemory(procs[index].pHandle, Address, ref buffer, size);
        }

        public bool ReadMemory(int index, long Address, ref byte[] buffer)
        {
            return ReadMemory(procs[index].pHandle, Address, ref buffer);
        }

        public double ReadDouble(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(double)];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }
        public double ReadDouble(int index, long Address)
        { return ReadDouble(index, Address, false); }

        public float ReadFloat(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(float)];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }
        public float ReadFloat(int index, long Address)
        { return ReadFloat(index, Address, false); }

        public UInt64 ReadUInt64(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(UInt64)];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
        public UInt64 ReadUInt64(int index, long Address)
        { return ReadUInt64(index, Address, false); }

        public Int64 ReadInt64(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(Int64)];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
        public Int64 ReadInt64(int index, long Address)
        { return ReadInt64(index, Address, false); }

        public uint ReadUInt(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[4];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public uint ReadUInt(int index, long Address) { return ReadUInt(index, Address, false); }

        public int ReadInt(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[4];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadInt(int index, long Address) { return ReadInt(index, Address, false); }

        public ushort ReadUShort(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[2];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public ushort ReadUShort(int index, long Address) { return ReadUShort(index, Address, false); }

        public short ReadShort(int index, long Address, bool reverse)
        {
            byte[] buffer = new byte[2];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            if (reverse) Array.Reverse(buffer);
            return BitConverter.ToInt16(buffer, 0);
        }

        public short ReadShort(int index, long Address) { return ReadShort(index, Address, false); }

        public byte ReadByte(int index, long Address)
        {
            byte[] buffer = new byte[1];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            return buffer[0];
        }

        public sbyte ReadSByte(int index, long Address)
        {
            byte[] buffer = new byte[1];
            if (!ReadMemory(index, Address, ref buffer))
            {
                lasterror = "ReadMemory failed!";
                error = true;
                return 0;
            }
            return (sbyte)buffer[0];
        }

        public string ReadString(int index, long Address, int length)
        {
            byte[] buffer = new byte[length];

            ReadMemory(index, Address, ref buffer);

            string ret = Encoding.UTF8.GetString(buffer);

            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));

            return ret;
        }

        public string ReadString(int index, long Address, int length, Encoding encoding)
        {
            byte[] buffer = new byte[length];

            ReadMemory(index, Address, ref buffer);

            string ret = encoding.GetString(buffer);

            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));

            return ret;
        }
        #endregion

        /// <summary>
        /// Enumerate open windows
        /// </summary>
        public class WindowArray : ArrayList
        {
            private delegate bool EnumWindowsCB(IntPtr handle, IntPtr param);

            [DllImport("user32")]
            private static extern int EnumWindows(EnumWindowsCB cb,
                IntPtr param);

            private static bool MyEnumWindowsCB(IntPtr hwnd, IntPtr param)
            {
                GCHandle gch = (GCHandle)param;
                WindowArray itw = (WindowArray)gch.Target;
                itw.Add(hwnd);
                return true;
            }

            /// <summary>
            /// Returns an array of all open windows and their hWnds
            /// </summary>
            public WindowArray()
            {
                GCHandle gch = GCHandle.Alloc(this);
                EnumWindowsCB ewcb = new EnumWindowsCB(MyEnumWindowsCB);
                EnumWindows(ewcb, (IntPtr)gch);
                gch.Free();
            }
        }

        #region GetProcessWindowStuff
        public static IntPtr[] FindWindowsByTitle(string windowTitle)
        {
            ArrayList handles = new ArrayList();
            WindowArray winenum = new WindowArray();

            foreach (IntPtr handle in winenum)
                if (IsWindowVisible(handle) && GetWindowTitle(handle, 255).IndexOf(windowTitle) == 0)
                    handles.Add(handle);

            if (handles.Count == 0)
            {
                lasterror = windowTitle + " window was not found.";
                error = true;
                return null;
            }

            IntPtr[] hwnds = new IntPtr[handles.Count];

            for (int i = 0; i < handles.Count; i++)
                hwnds[i] = (IntPtr)handles[i];

            return hwnds;
        }

        public static IntPtr FindWindowByTitle(string windowTitle)
        {
            IntPtr[] windows = FindWindowsByTitle(windowTitle);
            if (windows == null)
                return IntPtr.Zero;

            return windows[0];
        }

        public static IntPtr[] FindWindowsByClassName(string classname)
        {
            ArrayList handles = new ArrayList();
            WindowArray winenum = new WindowArray();
            StringBuilder tmp_classname;

            foreach (IntPtr handle in winenum)
                if (IsWindowVisible(handle))
                {
                    tmp_classname = new StringBuilder(256);
                    GetClassName(handle, tmp_classname, 256);
                    if (tmp_classname.ToString().Equals(classname))
                        handles.Add(handle);
                }

            if (handles.Count == 0)
            {
                lasterror = classname + " window was not found.";
                error = true;
                return null;
            }

            IntPtr[] hwnds = new IntPtr[handles.Count];

            for (int i = 0; i < handles.Count; i++)
                hwnds[i] = (IntPtr)handles[i];

            return hwnds;
        }

        public static IntPtr FindWindowByClassName(string classname)
        {
            return FindWindow(classname, null);
        }

        public static IntPtr FindWindowByProcessId(int dwProcessId)
        {
            if (dwProcessId == 0)
            {
                lasterror = "dwProcessId == 0";
                error = true;
                return IntPtr.Zero;
            }

            //WindowArray winenum = new WindowArray();
            //foreach (IntPtr handle in winenum)
            //	if (GetProcessIdByHWnd(handle) == dwProcessId)
            //		return handle;

            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(dwProcessId);
            if (proc != null) return proc.MainWindowHandle;

            lasterror = "Could not find window.";
            error = true;
            return IntPtr.Zero;
        }

        public static IntPtr[] FindWindowsByProcessName(string processname)
        {
            int[] pids = GetProcessIdsByProcessName(processname);
            if (pids == null || pids.Length == 0)
                return null;

            IntPtr[] hwnds = new IntPtr[pids.Length];
            System.Diagnostics.Process[] procs = new System.Diagnostics.Process[pids.Length];

            for (int i = 0; i < pids.Length; i++)
            {
                procs[i] = System.Diagnostics.Process.GetProcessById(pids[i]);
                hwnds[i] = procs[i].MainWindowHandle;
            }

            return hwnds;
        }

        public static IntPtr FindWindowByProcessName(string processname)
        {
            IntPtr[] windows = FindWindowsByProcessName(processname);
            if (windows == null)
                return IntPtr.Zero;

            return windows[0];
        }

        public static int GetProcessIdByHWnd(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                lasterror = "hWnd == 0";
                error = true;
                return 0;
            }

            int dwProcessId;
            GetWindowThreadProcessId(hWnd, out dwProcessId);

            if (dwProcessId == 0)
            {
                lasterror = "dwProcessId == 0";
                error = true;
                return 0;
            }

            return dwProcessId;
        }

        public static int GetProcessIdByProcessName(string processname)
        {
            int[] procs = GetProcessIdsByProcessName(processname);
            if (procs == null)
                return 0;

            return procs[0];
        }

        public static int[] GetProcessIdsByProcessName(string procname)
        {
            string processname = procname;

            if (processname.IndexOf(".exe") != -1)
                processname = processname.Remove(processname.Length - 4);

            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(processname);
            if (procs.Length == 0)
            {
                lasterror = "Process " + processname + " could not be found.";
                error = true;
                return null;
            }

            int[] pids = new int[procs.Length];

            for (int i = 0; i < procs.Length; i++)
                pids[i] = procs[i].Id;

            return pids;
        }

        public static int GetProcessIdByWindowTitle(string windowtitle)
        {
            int[] procs = GetProcessIdsByWindowTitle(windowtitle);
            if (procs == null)
                return 0;

            return procs[0];
        }

        public static int[] GetProcessIdsByWindowTitle(string windowtitle)
        {
            IntPtr[] hwnds = FindWindowsByTitle(windowtitle);
            if (hwnds == null || hwnds.Length == 0)
                return null;

            int[] pids = new int[hwnds.Length];

            for (int i = 0; i < hwnds.Length; i++)
                GetWindowThreadProcessId((IntPtr)hwnds[i], out pids[i]);

            return pids;
        }

        public static int GetProcessIdByClassname(string classname)
        {
            int[] procs = GetProcessIdsByClassName(classname);
            if (procs == null)
                return 0;

            return procs[0];
        }

        public static int[] GetProcessIdsByClassName(string classname)
        {
            IntPtr[] hwnds = FindWindowsByClassName(classname);
            if (hwnds == null || hwnds.Length == 0)
                return null;

            int[] pids = new int[hwnds.Length];

            for (int i = 0; i < hwnds.Length; i++)
                GetWindowThreadProcessId((IntPtr)hwnds[i], out pids[i]);

            return pids;
        }

        public static string GetWindowTitleFromProcessId(int dwProcessId)
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(dwProcessId);
            if (proc != null) return proc.MainWindowTitle;

            lasterror = "Could not find window.";
            error = true;
            return null;
        }

        public static IntPtr OpenProcess(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                lasterror = "hWnd == 0";
                error = true;
                return IntPtr.Zero;
            }

            int dwProcessId = GetProcessIdByHWnd(hWnd);
            if (dwProcessId == 0)
            {
                lasterror = "Could not get ProcessId.";
                error = true;
                return IntPtr.Zero;
            }

            return OpenProcess(PROCESS_ALL_ACCESS, false, dwProcessId);
        }

        public static IntPtr[] OpenProcesses(IntPtr[] hWnds)
        {
            if (hWnds == null || hWnds.Length == 0)
            {
                lasterror = "hWnds == 0";
                error = true;
                return null;
            }

            IntPtr[] pHandles = new IntPtr[hWnds.Length];

            for (int i = 0; i < hWnds.Length; i++)
            {
                if (hWnds[i] == IntPtr.Zero)
                {
                    lasterror = String.Format("hWnds[{0}] == 0", i);
                    error = true;
                    return null;
                }

                pHandles[i] = OpenProcess(hWnds[i]);

                if (pHandles[i] == IntPtr.Zero)
                    return null;
            }

            return pHandles;
        }

        public static IntPtr OpenProcess(int dwProcessId)
        {
            if (dwProcessId == 0)
            {
                lasterror = "Could not get ProcessId.";
                error = true;
                return IntPtr.Zero;
            }

            return OpenProcess(PROCESS_ALL_ACCESS, false, dwProcessId);
        }

        public static IntPtr[] OpenProcesses(int[] dwProcessIds)
        {
            if (dwProcessIds == null || dwProcessIds.Length == 0)
            {
                lasterror = "Could not get ProcessId.";
                error = true;
                return null;
            }

            for (int i = 0; i < dwProcessIds.Length; i++)
                if (dwProcessIds[i] == 0)
                {
                    lasterror = "Could not get ProcessId.";
                    error = true;
                    return null;
                }

            IntPtr[] pHandles = new IntPtr[dwProcessIds.Length];

            for (int i = 0; i < pHandles.Length; i++)
                pHandles[i] = OpenProcess(dwProcessIds[i]);

            return pHandles;
        }
        #endregion

        #region ReadWriteInject
        public static bool ReadMemory(IntPtr pHandle, long Address, ref byte[] buffer, int size)
        {
            return ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)size, IntPtr.Zero);
        }

        public static bool ReadMemory(IntPtr pHandle, long Address, ref byte[] buffer)
        {
            return ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        }

        public static double ReadDouble(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(double)];
            if (ReadMemory(pHandle, Address, ref buffer, 8))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }
            else
                return double.MaxValue;
        }

        public static double ReadDouble(IntPtr pHandle, long Address)
        { return ReadDouble(pHandle, Address, false); }

        public static float ReadFloat(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(float)];
            if (ReadMemory(pHandle, Address, ref buffer, 4))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToSingle(buffer, 0);
            }
            else
                return float.MaxValue;
        }

        public static float ReadFloat(IntPtr pHandle, long Address)
        { return ReadFloat(pHandle, Address, false); }

        public static UInt64 ReadUInt64(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(UInt64)];
            if (ReadMemory(pHandle, Address, ref buffer, 8))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToUInt64(buffer, 0);
            }
            else
                return UInt64.MaxValue;
        }

        public static UInt64 ReadUInt64(IntPtr pHandle, long Address)
        { return ReadUInt64(pHandle, Address, false); }

        public static Int64 ReadInt64(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(Int64)];
            if (ReadMemory(pHandle, Address, ref buffer, 8))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToInt64(buffer, 0);
            }
            else
                return Int64.MaxValue;
        }

        public static Int64 ReadInt64(IntPtr pHandle, long Address)
        { return ReadInt64(pHandle, Address, false); }

        public static uint ReadUInt(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(uint)];
            if (ReadMemory(pHandle, Address, ref buffer, 4))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToUInt32(buffer, 0);
            }
            else
                return uint.MaxValue;
        }

        public static uint ReadUInt(IntPtr pHandle, long Address)
        { return ReadUInt(pHandle, Address, false); }

        public static int ReadInt(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(int)];
            if (ReadMemory(pHandle, Address, ref buffer, 4))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToInt32(buffer, 0);
            }
            else
                return int.MaxValue;
        }

        public static int ReadInt(IntPtr pHandle, long Address)
        { return ReadInt(pHandle, Address, false); }

        public static ushort ReadUShort(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(ushort)];
            if (ReadMemory(pHandle, Address, ref buffer, 2))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToUInt16(buffer, 0);
            }
            else
                return ushort.MaxValue;
        }

        public static ushort ReadUShort(IntPtr pHandle, long Address)
        { return ReadUShort(pHandle, Address, false); }

        public static short ReadShort(IntPtr pHandle, long Address, bool reverse)
        {
            byte[] buffer = new byte[sizeof(short)];
            if (ReadMemory(pHandle, Address, ref buffer, 2))
            {
                if (reverse) Array.Reverse(buffer);
                return BitConverter.ToInt16(buffer, 0);
            }
            else
                return short.MaxValue;
        }

        public static short ReadShort(IntPtr pHandle, long Address)
        { return ReadShort(pHandle, Address, false); }

        public static byte ReadByte(IntPtr pHandle, long Address)
        {
            byte[] buffer = new byte[sizeof(byte)];
            if (ReadMemory(pHandle, Address, ref buffer, 1))
                return buffer[0];
            else
                return byte.MaxValue;
        }

        public static sbyte ReadSByte(IntPtr pHandle, long Address)
        {
            byte[] buffer = new byte[sizeof(sbyte)];
            if (ReadMemory(pHandle, Address, ref buffer, 1))
                return (sbyte)buffer[0];
            else
                return sbyte.MaxValue;
        }

        public static string ReadString(IntPtr pHandle, long Address, int length)
        {
            byte[] buffer = new byte[length];
            ReadMemory(pHandle, Address, ref buffer);
            string ret = Encoding.UTF8.GetString(buffer);
            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));
            return ret;
        }

        public static string ReadString(IntPtr pHandle, long Address, int length, bool IsUnicode)
        {
            byte[] buffer = new byte[length];
            ReadMemory(pHandle, Address, ref buffer);
            string ret;
            UnicodeEncoding enc = new UnicodeEncoding();
            if (IsUnicode)
                ret = enc.GetString(buffer);
            else
                ret = Encoding.UTF8.GetString(buffer);
            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));
            return ret;
        }

        public static string ReadWStdString(IntPtr pHandle, long Address)
        {
            Address += 0x4;
            
            int length = ReadInt(pHandle, Address + 0x14);

            if (length > 8)
            {
                // length needed is in bytes, not chars - 2 bytes per char. :)
                return ReadString(pHandle, Memory.ReadInt(pHandle, Address), length * 2, true);
            }
            else
            {
                return ReadString(pHandle, Address, length * 2, true);
            }
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, string value)
        {
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, UInt64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, Int64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, double value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, sbyte value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 1);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, byte value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 1);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, short value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 2);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, ushort value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 2);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 4);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteMemory(pHandle, Address, buffer, 4);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, byte[] buffer, int size)
        {
            return WriteProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)size, IntPtr.Zero);
        }

        public static bool WriteMemory(IntPtr pHandle, long Address, byte[] buffer)
        {
            return WriteProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        }

        public static uint InjectDll(IntPtr pHandle, string dllname)
        {
            uint pLibModule = AllocateMemory(pHandle, 0x1000);

            WriteMemory(pHandle, pLibModule, dllname);

            uint lpLoadLibrary = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            //IntPtr hThread = CreateRemoteThread(pHandle, IntPtr.Zero, 0, lpLoadLibrary, pLibModule, 0, IntPtr.Zero);
            IntPtr hThread = CreateRemoteThread(pHandle, lpLoadLibrary, pLibModule);
            WaitForSingleObject(hThread, INFINITE);
            uint dwExitCode = GetExitCodeThread(hThread);
            CloseHandle(hThread);

            VirtualFreeEx(pHandle, pLibModule, 0x1000, MEM_DECOMMIT);

            return dwExitCode;
        }

        public static uint InjectDllToWindow(IntPtr hWnd, string dllname)
        {
            IntPtr pHandle = OpenProcess(hWnd);
            uint dwExitCode = InjectDll(pHandle, dllname);
            CloseHandle(pHandle);
            return dwExitCode;
        }

        public static uint InjectDllToProcess(int dwProcessId, string dllname)
        {
            IntPtr pHandle = OpenProcess(dwProcessId);
            uint dwExitCode = InjectDll(pHandle, dllname);
            CloseHandle(pHandle);
            return dwExitCode;
        }

        public uint InjectDll(int index, string dllname)
        {
            return InjectDll(procs[index].pHandle, dllname);
        }

        public static bool UninjectDll(IntPtr pHandle, uint dwBaseAddress)
        {
            IntPtr hThread = CreateRemoteThread(pHandle, GetProcAddress(GetModuleHandle("kernel32.dll"), "FreeLibrary"), dwBaseAddress);
            WaitForSingleObject(hThread);
            uint dwExitCode = Memory.GetExitCodeThread(hThread);
            CloseHandle(hThread);
            return dwExitCode > 0 ? true : false;
        }

        public static bool UninjectDllFromWindow(IntPtr hWnd, uint dwBaseAddress)
        {
            IntPtr pHandle = OpenProcess(hWnd);
            if (pHandle == IntPtr.Zero) return false;
            bool ret = UninjectDll(pHandle, dwBaseAddress);
            CloseHandle(pHandle);
            return ret;
        }

        public static bool UninjectDllFromProcess(int dwProcessId, uint dwBaseAddress)
        {
            IntPtr pHandle = OpenProcess(dwProcessId);
            if (pHandle == IntPtr.Zero) return false;
            bool ret = UninjectDll(pHandle, dwBaseAddress);
            CloseHandle(pHandle);
            return ret;
        }

        //I know, I know, this is the tough way of doing it.  I have no idea why it didn't occur
        //to me to simply load the dll, find the difference from the proc and the dll base in my
        //own memory, then translate that to the external process by finding its dll base...
        //yeah, I'm kinda dumb sometimes
        public static uint GetProcAddressEx(IntPtr pHandle, string modulename, string procname)
        {
            //allocate memory for our codecave
            uint codecave = AllocateMemory(pHandle, 0x1000);

            //our codecave injection which basically calls 
            //GetModuleHandle and GetProcAddress inside the context of the given process
            byte[] Inject0 = { 0x68, 0, 0, 0, 0, 0xE8, 0, 0, 0, 0, 0x68, 0, 0, 0, 0, 0x50, 0xE8, 0, 0, 0, 0, 0xC3 };
            WriteMemory(pHandle, codecave, Inject0);

            //write the given module name to codecave+0x300 so we can pass
            //it as an argument to GetModuleHandleA
            WriteMemory(pHandle, codecave + 0x300, modulename);
            //overwrite the first PUSH operand with a pointer to the modulename string
            WriteMemory(pHandle, codecave + 1, codecave + 0x300);

            //write the given proc name to codecave+0x600 so we can pass
            //it as an argument to GetProcAddress
            WriteMemory(pHandle, codecave + 0x600, procname);
            //overwrite the second PUSH operand with a pointer to the procname string
            WriteMemory(pHandle, codecave + 11, codecave + 0x600);

            //get the proc addresses of GetProcAddress and GetModuleHandleA
            IntPtr kernel32 = GetModuleHandle("kernel32.dll");
            uint lpGetProcAddress = GetProcAddress(kernel32, "GetProcAddress");
            uint lpGetModuleHandle = GetProcAddress(kernel32, "GetModuleHandleA");
            //overwrite the addresses of the calls to GetProcAddress and GetModuleHandleA
            WriteMemory(pHandle, codecave + 6, (lpGetModuleHandle - (codecave + 10)));
            WriteMemory(pHandle, codecave + 17, (lpGetProcAddress - (codecave + 21)));

            //create a thread on the codecave
            IntPtr hThread = CreateRemoteThread(pHandle, codecave, 0);
            //wait until that thread returns
            WaitForSingleObject(hThread);
            //get the return value of the thread (the external proc address)
            uint ret = GetExitCodeThread(hThread);
            //close the handle to the thread
            CloseHandle(hThread);

            //return the proc address of the given proc name inside the given process' context
            return ret;
        }

        public static uint GetWindowProcAddressEx(IntPtr hWnd, string modulename, string procname)
        {
            IntPtr pHandle = OpenProcess(hWnd);
            uint ret = GetProcAddressEx(pHandle, modulename, procname);
            CloseHandle(pHandle);
            return ret;
        }

        public static uint GetProcAddressEx(int dwProcessId, string modulename, string procname)
        {
            IntPtr pHandle = OpenProcess(dwProcessId);
            uint ret = GetProcAddressEx(pHandle, modulename, procname);
            CloseHandle(pHandle);
            return ret;
        }
        #endregion
    }
}