namespace ABHeal
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class SKeys
    {
        private Process proc;

        public SKeys(Process process)
        {
            this.proc = process;
        }

        [DllImport("User32.Dll", EntryPoint="PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        public void SendKey(int keyCode)
        {
            PostMessage(this.proc.MainWindowHandle, 0x100, keyCode, 0);
            Thread.Sleep(50);
        }

        public void SendLine(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(this.proc.MainWindowHandle, 0x100, VkKeyScan(message[i]), 0);
            }
            Thread.Sleep((int) (message.Length * 0x4b));
            PostMessage(this.proc.MainWindowHandle, 0x100, 13, 0);
            Thread.Sleep(50);
        }

        public void SendMessage(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(this.proc.MainWindowHandle, 0x100, VkKeyScan(message[i]), 0);
            }
        }

        [DllImport("user32.dll")]
        private static extern byte VkKeyScan(char ch);
    }
}

