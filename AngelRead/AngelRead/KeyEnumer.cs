using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace AngelRead
{
    public class KeyEnumer
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("KProcCheck.dll")]
        static extern int getaionhwd();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int extraInfo);
        [DllImport("user32.dll")]
        static extern short MapVirtualKey(int wCode, int wMapType);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);
        //[DllImport("user32.dll", SetLastError = true)]
        bool keypressing = false;

        public void PauseForMilliSeconds(int MilliSecondsToPauseFor)
        {
            Application.DoEvents();
            Thread.Sleep(MilliSecondsToPauseFor);
        }

        public void keyenumerator(string key)
        {
            try
            {
                if (key.Length > 1)
                {
                    string[] stringarray = key.Split(',');
                    if (stringarray[0] == "Alt" || stringarray[0] == "alt")
                    {
                        keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                        PauseForMilliSeconds(50);
                        oldkeyenumerator(stringarray[1]);
                        PauseForMilliSeconds(50);
                        keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    }
                    else if (stringarray[0] == "Ctrl" || stringarray[0] == "ctrl")
                    {
                        keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                        PauseForMilliSeconds(50);
                        oldkeyenumerator(stringarray[1]);
                        PauseForMilliSeconds(50);
                        keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    }
                    else if (stringarray[0] == "Shift" || stringarray[0] == "shift")
                    {
                        keybd_event((int)Keys.ShiftKey, (byte)MapVirtualKey((int)Keys.ShiftKey, 0), 0, 0); //  Alt
                        PauseForMilliSeconds(50);
                        oldkeyenumerator(stringarray[1]);
                        PauseForMilliSeconds(50);
                        keybd_event((int)Keys.ShiftKey, (byte)MapVirtualKey((int)Keys.ShiftKey, 0), 2, 0); //  Alt
                    }
                    else
                    {
                        oldkeyenumerator(stringarray[0]);
                    }
                }
                else { oldkeyenumerator(key); }
            }
            catch (Exception doo) { MessageBox.Show("Error with finding key: " + key + ". Format wrong. " + doo); };
        }
        public void testkeyenumerator(string key)
        {

        }

        public void oldkeyenumerator(string key)
        {
            if (key.Contains('\0').ToString() == "True")
            {
                key = key.Substring(0, key.LastIndexOf('\0') - 0);
            }
            if (key.Length == 1) key = key.ToLower();
            keypressing = true;
            switch (key)
            {
                case "DOWN":
                    keybd_event((int)Keys.Down, (byte)MapVirtualKey((int)Keys.Down, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Down, (byte)MapVirtualKey((int)Keys.Down, 0), 2, 0); //  Up 
                    break;
                case "SPACE":
                    keybd_event((int)Keys.Space, (byte)MapVirtualKey((int)Keys.Space, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Space, (byte)MapVirtualKey((int)Keys.Space, 0), 2, 0); //  Up 
                    break;
                case "ENTER":
                    keybd_event((int)Keys.Enter, (byte)MapVirtualKey((int)Keys.Enter, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Enter, (byte)MapVirtualKey((int)Keys.Enter, 0), 2, 0); //  Up 
                    break;
                case "NUMLOCK":
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 2, 0); //  Up 
                    break;
                case "F1":
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 2, 0); //  Up 
                    break;
                case "F2":
                    keybd_event((int)Keys.F2, (byte)MapVirtualKey((int)Keys.F2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F2, (byte)MapVirtualKey((int)Keys.F2, 0), 2, 0); //  Up 
                    break;
                case "F3":
                    keybd_event((int)Keys.F3, (byte)MapVirtualKey((int)Keys.F3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F3, (byte)MapVirtualKey((int)Keys.F3, 0), 2, 0); //  Up 
                    break;
                case "F4":
                    keybd_event((int)Keys.F4, (byte)MapVirtualKey((int)Keys.F4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F4, (byte)MapVirtualKey((int)Keys.F4, 0), 2, 0); //  Up 
                    break;
                case "F5":
                    keybd_event((int)Keys.F5, (byte)MapVirtualKey((int)Keys.F5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F5, (byte)MapVirtualKey((int)Keys.F5, 0), 2, 0); //  Up 
                    break;
                case "F6":
                    keybd_event((int)Keys.F6, (byte)MapVirtualKey((int)Keys.F6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F6, (byte)MapVirtualKey((int)Keys.F6, 0), 2, 0); //  Up 
                    break;
                case "F7":
                    keybd_event((int)Keys.F7, (byte)MapVirtualKey((int)Keys.F7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F7, (byte)MapVirtualKey((int)Keys.F7, 0), 2, 0); //  Up 
                    break;
                case "F8":
                    keybd_event((int)Keys.F8, (byte)MapVirtualKey((int)Keys.F8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F8, (byte)MapVirtualKey((int)Keys.F8, 0), 2, 0); //  Up 
                    break;
                case "F9":
                    keybd_event((int)Keys.F9, (byte)MapVirtualKey((int)Keys.F9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F9, (byte)MapVirtualKey((int)Keys.F9, 0), 2, 0); //  Up 
                    break;
                case "F10":
                    keybd_event((int)Keys.F10, (byte)MapVirtualKey((int)Keys.F10, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F10, (byte)MapVirtualKey((int)Keys.F10, 0), 2, 0); //  Up 
                    break;
                case "F11":
                    keybd_event((int)Keys.F11, (byte)MapVirtualKey((int)Keys.F11, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F11, (byte)MapVirtualKey((int)Keys.F11, 0), 2, 0); //  Up 
                    break;
                case "F12":
                    keybd_event((int)Keys.F12, (byte)MapVirtualKey((int)Keys.F12, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F12, (byte)MapVirtualKey((int)Keys.F12, 0), 2, 0); //  Up 
                    break;
                case "a":
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 0, 0); //  Down
                    PauseForMilliSeconds(20);
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 2, 0); //  Up 
                    break;
                case "b":
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 2, 0); //  Up 
                    break;
                case "c":
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 2, 0); //  Up 
                    break;
                case "d":
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 2, 0); //  Up 
                    break;
                case "e":
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 2, 0); //  Up 
                    break;
                case "f":
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 2, 0); //  Up 
                    break;
                case "g":
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 2, 0); //  Up 
                    break;
                case "h":
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 2, 0); //  Up 
                    break;
                case "i":
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 2, 0); //  Up 
                    break;
                case "j":
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 2, 0); //  Up 
                    break;
                case "k":
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 2, 0); //  Up 
                    break;
                case "l":
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 2, 0); //  Up 
                    break;
                case "m":
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 2, 0); //  Up 
                    break;
                case "n":
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 2, 0); //  Up 
                    break;
                case "o":
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 2, 0); //  Up 
                    break;
                case "p":
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 2, 0); //  Up 
                    break;
                case "q":
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 2, 0); //  Up 
                    break;
                case "r":
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 2, 0); //  Up 
                    break;
                case "s":
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 2, 0); //  Up 
                    break;
                case "t":
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 2, 0); //  Up 
                    break;
                case "u":
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 2, 0); //  Up 
                    break;
                case "v":
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 2, 0); //  Up 
                    break;
                case "w":
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0); //  Up 
                    break;
                case "x":
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 2, 0); //  Up 
                    break;
                case "y":
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 2, 0); //  Up 
                    break;
                case "z":
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 2, 0); //  Up 
                    break;
                case "ESC":
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 0, 0); //  Down
                    PauseForMilliSeconds(20);
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(150);
                    break;
                case "1":

                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    break;
                case "2":

                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    break;
                case "3":

                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    break;
                case "4":

                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    break;
                case "5":

                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    break;
                case "6":

                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    break;
                case "7":

                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    break;
                case "8":

                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    break;
                case "9":

                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    break;
                case "0":

                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    break;
                case "-":
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    break;
                case "=":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case "+":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case ",":
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 2, 0); //  Up 
                    break;
                case "/":
                    keybd_event((int)Keys.Divide, (byte)MapVirtualKey((int)Keys.Divide, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Divide, (byte)MapVirtualKey((int)Keys.Divide, 0), 2, 0); //  Up 
                    break;
                case "TAB":
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 2, 0); //  Up 
                    break;
                case "UP":
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0); //  Up 
                    break;
            }
            keypressing = false;
            
        }
    }
}