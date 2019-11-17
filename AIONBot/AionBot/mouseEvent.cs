using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;

namespace AionBot
{
    class mouseEvent
    {
        // Fields
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 2;
        private const int MOUSEEVENTF_LEFTUP = 4;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_MOVE = 1;
        private const int MOUSEEVENTF_RIGHTDOWN = 8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_WHEEL = 0x800;

        // Methods
        public void leftClick(int x, int y)
        {
            SetCursorPos(x, y);
            Thread.Sleep(500);
            mouse_event(2, 0, 0, 0, 0);
            Thread.Sleep(200);
            mouse_event(4, 0, 0, 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        public void rightClick(int x, int y)
        {
            SetCursorPos(x, y);
            Thread.Sleep(500);
            mouse_event(8, 0, 0, 0, 0);
            Thread.Sleep(200);
            mouse_event(0x10, 0, 0, 0, 0);
        }

        public void setCurPos(int x, int y)
        {
            SetCursorPos(x, y);
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

    }
}
