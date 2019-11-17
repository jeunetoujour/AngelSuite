using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;
namespace AionBot
{
    public class keyEvent
    {
        // Fields
        private const uint KEYEVENTF_KEYUP = 2;
        private const int VK_0 = 0x30;
        private const int VK_1 = 0x31;
        private const int VK_2 = 50;
        private const int VK_3 = 0x33;
        private const int VK_4 = 0x34;
        private const int VK_5 = 0x35;
        private const int VK_6 = 0x36;
        private const int VK_7 = 0x37;
        private const int VK_8 = 0x38;
        private const int VK_9 = 0x39;
        private const int VK_LEFT = 0x25;
        private const int VK_MENU = 0x12;
        private const int VK_MINUS = 0xbd;
        private const int VK_NUMPAD1 = 0x61;
        private const int VK_NUMPAD2 = 0x62;
        private const int VK_PLUS = 0xbb;
        private const int VK_R = 0x52;
        private const int VK_RIGHT = 0x27;
        private const int VK_SHIFT = 0x10;
        private const int VK_TAB = 9;
        private const int VK_UP = 0x26;
        private const int VK_W = 0x57;

        // Methods
        public void alt(string key)
        {
            keybd_event(0x12, 0, 0, 0);
            this.pressKey(key, 500);
            keybd_event(0x12, 0, 2, 0);
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        public void Numkey1()
        {
            keybd_event(0x61, 0, 0, 0);
            keybd_event(0x61, 0, 2, 0);
        }

        public void Numkey2()
        {
            keybd_event(0x62, 0, 0, 0);
            keybd_event(0x62, 0, 2, 0);
        }

        public void pressKey(string key, int length)
        {
            if (key.Equals("r"))
            {
                keybd_event(0x52, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x52, 0, 2, 0);
            }
            else if (key.Equals("0"))
            {
                keybd_event(0x30, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x30, 0, 2, 0);
            }
            else if (key.Equals("w"))
            {
                keybd_event(0x57, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x57, 0, 2, 0);
            }
            else if (key.Equals("0"))
            {
                keybd_event(0x30, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x30, 0, 2, 0);
            }
            else if (key.Equals("1"))
            {
                keybd_event(0x31, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x31, 0, 2, 0);
            }
            else if (key.Equals("2"))
            {
                keybd_event(50, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(50, 0, 2, 0);
            }
            else if (key.Equals("3"))
            {
                keybd_event(0x33, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x33, 0, 2, 0);
            }
            else if (key.Equals("4"))
            {
                keybd_event(0x34, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x34, 0, 2, 0);
            }
            else if (key.Equals("5"))
            {
                keybd_event(0x35, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x35, 0, 2, 0);
            }
            else if (key.Equals("6"))
            {
                keybd_event(0x36, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x36, 0, 2, 0);
            }
            else if (key.Equals("7"))
            {
                keybd_event(0x37, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x37, 0, 2, 0);
            }
            else if (key.Equals("8"))
            {
                keybd_event(0x38, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x38, 0, 2, 0);
            }
            else if (key.Equals("9"))
            {
                keybd_event(0x39, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0x39, 0, 2, 0);
            }
            else if (key.Equals("-"))
            {
                keybd_event(0xbd, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0xbd, 0, 2, 0);
            }
            else if (key.Equals("+"))
            {
                keybd_event(0xbb, 0, 0, 0);
                Thread.Sleep(length);
                keybd_event(0xbb, 0, 2, 0);
            }
        }

        public void pressLeft(int length)
        {
            keybd_event(0x25, 0, 0, 0);
            Thread.Sleep(length);
            keybd_event(0x25, 0, 2, 0);
        }

        public void pressRight(int length)
        {
            keybd_event(0x27, 0, 0, 0);
            Thread.Sleep(length);
            keybd_event(0x27, 0, 2, 0);
        }

        public void Run()
        {
            keybd_event(0x26, 0, 0, 0);
        }

        public void shift(string key)
        {
            keybd_event(0x10, 0, 0, 0);
            this.pressKey(key, 500);
            keybd_event(0x10, 0, 2, 0);
        }

        public void shiftDown()
        {
            keybd_event(0x10, 0, 0, 0);
        }

        public void shiftUp()
        {
            keybd_event(0x10, 0, 2, 0);
        }

        public void stopRun()
        {
            keybd_event(0x26, 0, 2, 0);
        }

        public void tab()
        {
            keybd_event(9, 0, 0, 0);
            keybd_event(9, 0, 2, 0);
        }

    }
}
