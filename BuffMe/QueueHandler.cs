using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace BuffMe
{
    class QueueHandler
    {
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public bool running = false;
        private Queue<Skill> queueSkill;
        private int interval = 0;

        /// <summary>
        /// Initialize with an instance of Queue&lt;Skill&gt;, and run Program.StartQueueHandler(). Default interval of 800ms.
        /// </summary>
        public QueueHandler(Queue<Skill> queueSkill) : this(queueSkill, 2500) { }
        /// <summary>
        /// Initialize with an instance of Queue&lt;Skill&gt;, and run Program.StartQueueHandler(). Interval in ms.
        /// </summary>
        public QueueHandler(Queue<Skill> queueSkill, int interval)
        {
            this.queueSkill = queueSkill;
            this.interval = interval;
        }

        public bool Exists(Skill s)
        {
            return false; // not implemented yet
        }

        public void Add(Skill addSkill)
        {
            queueSkill.Enqueue(addSkill);
        }

        public void Start()
        {
            try
            {
                if (Program.hooked) this.running = true;
                
                Console.WriteLine("QueueHandler Started");
                
                while (this.running)
                {
                    Program.myPlayer.Update();
                    lock (Program.myPlayer) // Not implemented properly in this version of AionMemory
                        while (Program.myPlayer.Stance == AionMemory.eStance.Combat || Program.myPlayer.Stance == AionMemory.eStance.FlyingCombat) { Console.WriteLine("Combat active, waiting."); Thread.Sleep(this.interval); }

                    if (queueSkill.Count > 0)
                    {
                        Skill nextSkill;
                        lock (queueSkill)
                        {
                            nextSkill = this.queueSkill.Dequeue();
                        }

                        if (nextSkill.byteCode != 0x00)
                        {
                            if (GetForegroundWindow() != FindWindow(null, "AION Client"))
                            {
                                SetForegroundWindow(FindWindow(null, "AION Client")); // Needs to be edited some day to support more than one window?
                                keybd_event(0x39, 0, 0x0, 0); // Do a spacebar
                            }
                        }

                        Console.Write("Sending keys to Aion: ");

                        if (nextSkill.specialCode != 0x00)
                        {
                            keybd_event(nextSkill.specialCode, 0, 0x0, 0); Console.Write("sC:{0} down|", nextSkill.specialCode);
                            Thread.Sleep(50);
                        }
                        if (nextSkill.byteCode != 0x00)
                        {
                            keybd_event(nextSkill.byteCode, 0, 0x0, 0); Console.Write("bC:{0} down|", nextSkill.byteCode);
                            keybd_event(nextSkill.byteCode, 0, 0x2, 0); Console.Write("bC:{0} up", nextSkill.byteCode);
                        }
                        if (nextSkill.specialCode != 0x00)
                        {
                            Thread.Sleep(50);
                            keybd_event(nextSkill.specialCode, 0, 0x2, 0);Console.Write("|sC:{0} up", nextSkill.specialCode);
                        }

                        Console.WriteLine();
                    }
                    
                    Thread.Sleep(this.interval - 150);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void Stop()
        {
            this.running = false;
        }

    }
}
