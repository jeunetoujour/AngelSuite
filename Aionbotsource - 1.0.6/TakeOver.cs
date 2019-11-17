using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

public class TakeOver

{
#region constants

    private int handle;

    private const int INPUT_MOUSE = 0;
    private const int INPUT_KEYBOARD = 1;
    private const int INPUT_HARDWARE = 2;
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const uint KEYEVENTF_KEYUP = 0x0002;
    private const uint KEYEVENTF_UNICODE = 0x0004;
    private const uint KEYEVENTF_SCANCODE = 0x0008;
    private const uint XBUTTON1 = 0x0001;
    private const uint XBUTTON2 = 0x0002;
    private const uint MOUSEEVENTF_MOVE = 0x0001;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    private const uint MOUSEEVENTF_XDOWN = 0x0080;
    private const uint MOUSEEVENTF_XUP = 0x0100;
    private const uint MOUSEEVENTF_WHEEL = 0x0800;
    private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

#endregion
#region DLLImports

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern short VkKeyScan(char ch);
    private const int WM_CHAR = 0x0102;

#endregion

#region Structs
    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]  
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    { 
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUT 
    { 
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

#endregion


    public TakeOver(int handle)
    {
        this.handle = handle;
    }

    public void SetFocus()
    {
        SetForegroundWindow(new IntPtr(this.handle));
    }

    public void MoveMouse(int x, int y)
    {
        Rectangle screen = Screen.PrimaryScreen.Bounds;
        int x2 = (65535 * x) / screen.Width;
        int y2 = (65535 * y) / screen.Height;

        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(x2, y2, 0, 0, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }

    private KEYBDINPUT createKeybdInput(short wVK, uint flag)
    { 
        KEYBDINPUT i = new KEYBDINPUT();
        i.wVk = (ushort)wVK;
        i.wScan = 0;
        i.time = 0;
        i.dwExtraInfo = IntPtr.Zero;
        i.dwFlags = flag;
        return i;
    }


      private MOUSEINPUT createMouseInput(int x, int y, uint data, uint t, uint flag)
      {  
          MOUSEINPUT mi = new MOUSEINPUT();
          mi.dx = x;
          mi.dy = y;
          mi.mouseData = data;
          mi.time = t;
          //mi.dwFlags = MOUSEEVENTF_ABSOLUTE| MOUSEEVENTF_MOVE;
          mi.dwFlags = flag;
          return mi; 
      }

    public void MouseLeftClick()
    { 
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }

    public void MouseLeftDown()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTDOWN);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseLeftUp()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }


    public void MouseRightClick()
    {
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseRightDown()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTDOWN);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseRightUp()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }

    public void MouseMiddleClick()
    {
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_MIDDLEDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_MIDDLEUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }



    //this uses sendinput rather than sendmessage, it will only send the actual key press not with shift or alt just key press
    public void KeypressString(string txt)
    {
        short c;
        INPUT[] inp;
        if (txt == null || txt.Length == 0)
            return;
        inp = new INPUT[2];
        for (int i = 0; i < txt.Length; i++)
        { 
            c = VkKeyScan(txt[i]);
            inp[0].type = INPUT_KEYBOARD;
            inp[0].ki = createKeybdInput(c, 0);
            inp[1].type = INPUT_KEYBOARD;
            inp[1].ki = createKeybdInput(c, KEYEVENTF_KEYUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        } 
    }

    //this will send a string to the window minimized but it does not support shift'ed or alt'ed characters
    public void SendKeyboardText(string txt)
    {
          foreach (char c in txt)
            SendMessage(handle, WM_CHAR, c, IntPtr.Zero);
    }
    //this will send a char to a minamized window but does not support shif'ed or alt'd characters or movement characters
    public void SendChar(char c)
    {
        SendMessage(handle, WM_CHAR, c, IntPtr.Zero);
    }


    public void PressKey(Keys key)
    { 

        INPUT[] inp;
        inp = new INPUT[2];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, 0);
        inp[1].type = INPUT_KEYBOARD;
        inp[1].ki = createKeybdInput((short)key, KEYEVENTF_KEYUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        
    }

    // this will work with movment characters but not in the background
    public void PressKeyDown(Keys key)
    {
        INPUT[] inp;
        inp = new INPUT[1];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, 0);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));

   }

    public void PressKeyUp(Keys key)
    {
        INPUT[] inp;
        inp = new INPUT[1];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, KEYEVENTF_KEYUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));

    }


}


