using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;


namespace Utility
{
	public class KeyEvents
	{
		[DllImport("user32.dll")]
		public static extern void keybd_event(
			byte bVk,
			byte bScan,
			uint dwFlags,
			uint dwExtraInfo);

		const byte VK_LEFT = 0x25;
		const byte VK_UP = 0x26;
		const byte VK_RIGHT = 0x27;
		const byte VK_TAB = 0x09;
		const byte VK_SHIFT = 0x10;
		const byte VK_CONTROL = 0x11;
		const byte VK_ALT = 0x12;
		const byte KEYEVENTF_KEYUP = 0x2;
		
		public void Down(byte vk)
		{
			keybd_event(vk, 0, 0, 0);
		}
		
		public void Up(byte vk)
		{
			keybd_event(vk, 0, KEYEVENTF_KEYUP, 0);
		}
		
		public void Press(byte vk)
		{
			Press(vk, 0);
		}

		public void Press(byte vk, int delay)
		{
			if (delay == 0)
				delay = 50;
					
			Down(vk);
			
			if (delay > 0)
				Thread.Sleep(delay);
			
			Up(vk);
		}
		
		public void Tab()
		{
			Press(VK_TAB);
		}
		
		public void Run()
		{
			Down(VK_UP);
		}
		
		public void StopRun()
		{
			Up(VK_UP);
		}
		
		public void PressLeft(int length)
		{
			Press(VK_LEFT, length);
		}

		public void PressRight(int length)
		{
			Press(VK_RIGHT, length);
		}
		
		public void Up(char letter)
		{
			Up((byte)letter);
		}
		
		public void Press(char letter)
		{
			Press((byte)letter);
		}

        public void Press(string letter)
        {
            if (letter == "")
            {
            }
            else if (letter[0] == '%')
            {
                Down(VK_ALT);
                Press((byte)letter[1]);
                Up(VK_ALT);
            }
            else if (letter[0] == '^')
            {
                Down(VK_CONTROL);
                Press((byte)letter[1]);
                Up(VK_CONTROL);
            }
            else
            {
                Press((byte)letter[0]);
            }


           
        }

		public void Press(char letter, int delay)
		{
			Press((byte)letter, delay);
		}
		
		public void Down(char letter)
		{
			Down((byte)letter);
		}
		
		public void Control(Action d)
		{
			Down(VK_CONTROL);
			d();
			Up(VK_CONTROL);
		}
		
		public void Alt(Action d)
		{
			Down(VK_ALT);
			d();
			Up(VK_ALT);
		}
	}
}
