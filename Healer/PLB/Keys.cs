// This class is based on SoulMonger's SendKeysEx class from aionhacker.net
// All credit to him, I only tweaked a couple things and the name to save some
// typing.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ABHeal
{
    class SKeys
    {
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern byte VkKeyScan(char ch);

        private Process proc;

        public SKeys(Process process)
        {
            proc = process;
        }

        public void SendMessage(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(proc.MainWindowHandle, KeyConstants.WM_KEYDOWN, VkKeyScan(message[i]), 0);
            }
        }

        public void SendLine(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(proc.MainWindowHandle, KeyConstants.WM_KEYDOWN, VkKeyScan(message[i]), 0);
            }
            System.Threading.Thread.Sleep(message.Length * 75);
            PostMessage(proc.MainWindowHandle, KeyConstants.WM_KEYDOWN, KeyConstants.VK_RETURN, 0);
            System.Threading.Thread.Sleep(50);
        }

        public void SendKey(int keyCode)
        {
            PostMessage(proc.MainWindowHandle, KeyConstants.WM_KEYDOWN, keyCode, 0);
            System.Threading.Thread.Sleep(50);
        }




    }

    public class KeyConstants
    {
        public const uint WM_LBUTTONDOWN = 0x201;
        public const uint WM_LBUTTONUP = 0x202;
        public const uint WM_LBUTTONDBLCLK = 0x203;
        public const uint WM_RBUTTONDOWN = 0x204;
        public const uint WM_RBUTTONUP = 0x205;
        public const uint WM_RBUTTONDBLCLK = 0x206;
        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_KEYUP = 0x101;
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;
        public const int VK_A = 0x41;
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;
        public const int VK_D = 0x44;
        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;
        public const int VK_S = 0x53;
        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;
        public const int VK_W = 0x57;
        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;
        public const int VK_ADD = 0x6B;
        public const int VK_ATTN = 0xF6;
        public const int VK_BACK = 0x8;
        public const int VK_CANCEL = 0x3;
        public const int VK_CAPITAL = 0x14;
        public const int VK_CLEAR = 0xC;
        public const int VK_CONTROL = 0x11;
        public const int VK_CRSEL = 0xF7;
        public const int VK_DECIMAL = 0x6E;
        public const int VK_DELETE = 0x2E;
        public const int VK_DIVIDE = 0x6F;
        public const int VK_DOWN = 0x28;
        public const int VK_END = 0x23;
        public const int VK_EREOF = 0xF9;
        public const int VK_ESCAPE = 0x1B;
        public const int VK_EXECUTE = 0x2B;
        public const int VK_EXSEL = 0xF8;
        public const int VK_F1 = 0x70;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;
        public const int VK_F13 = 0x7C;
        public const int VK_F14 = 0x7D;
        public const int VK_F15 = 0x7E;
        public const int VK_F16 = 0x7F;
        public const int VK_F17 = 0x80;
        public const int VK_F18 = 0x81;
        public const int VK_F19 = 0x82;
        public const int VK_F2 = 0x71;
        public const int VK_F20 = 0x83;
        public const int VK_F21 = 0x84;
        public const int VK_F22 = 0x85;
        public const int VK_F23 = 0x86;
        public const int VK_F24 = 0x87;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_HELP = 0x2F;
        public const int VK_HOME = 0x24;
        public const int VK_INSERT = 0x2D;
        public const int VK_LBUTTON = 0x1;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_LEFT = 0x25;
        public const int VK_LMENU = 0xA4;
        public const int VK_LSHIFT = 0xA0;
        public const int VK_MBUTTON = 0x4;
        public const int VK_MENU = 0x12;
        public const int VK_MULTIPLY = 0x6A;
        public const int VK_NEXT = 0x22;
        public const int VK_NONAME = 0xFC;
        public const int VK_NUMLOCK = 0x90;
        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;
        public const int VK_OEM_CLEAR = 0xFE;
        public const int VK_PA1 = 0xFD;
        public const int VK_PAUSE = 0x13;
        public const int VK_PLAY = 0xFA;
        public const int VK_PRINT = 0x2A;
        public const int VK_PRIOR = 0x21;
        public const int VK_PROCESSKEY = 0xE5;
        public const int VK_RBUTTON = 0x2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_RETURN = 0xD;
        public const int VK_RIGHT = 0x27;
        public const int VK_RMENU = 0xA5;
        public const int VK_RSHIFT = 0xA1;
        public const int VK_SCROLL = 0x91;
        public const int VK_SELECT = 0x29;
        public const int VK_SEPARATOR = 0x6C;
        public const int VK_SHIFT = 0x10;
        public const int VK_SNAPSHOT = 0x2C;
        public const int VK_SPACE = 0x20;
        public const int VK_SUBTRACT = 0x6D;
        public const int VK_TAB = 0x9;
        public const int VK_UP = 0x26;
        public const int VK_ZOOM = 0xFB;
    }
}
