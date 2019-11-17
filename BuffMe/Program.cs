using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using AionMemory;

namespace BuffMe
{
    static class Program
    {
        public static Queue<Skill> queueSkill = new Queue<Skill>();
        public static List<SkillHandler> listSkillHandler = new List<SkillHandler>();
        public static Player myPlayer = new Player();

        public static MainForm mForm;
        public static QueueHandler qHandler;
        private static Thread qHandlerThread;

        public static bool hooked = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Console.WriteLine("Hello, I am the debug window. I show all the program happenings. Don't mind me though, I'm only useful when everything blows up, but don't close me either, or the program will stop. :)");

            Program.qHandler = new QueueHandler(queueSkill);

            Program.mForm = new MainForm();
            Application.Run(mForm);
            Program.StopQueueHandler();
        }

        /// <summary>
        /// Makes the queueHandler go.
        /// </summary>
        public static void StartQueueHandler()
        {
            if (!Program.qHandler.running)
            {
                Program.queueSkill.Enqueue(new Skill(0x39, 0x00)); // Queue a Spacebar
                Console.WriteLine("Starting the qHandler.");
                Program.qHandlerThread = new Thread(new ThreadStart(Program.qHandler.Start));
                Program.qHandlerThread.Start();
                foreach (SkillHandler s in Program.listSkillHandler)
                {
                    s.Start();
                }
            }
        }
        public static void StopQueueHandler()
        {
            if (Program.qHandler.running)
            {
                Console.WriteLine("Stopping the qHandler.");
                Program.qHandler.Stop();
                Program.qHandlerThread.Join();
                Console.WriteLine("qHandler stopped.");
                foreach (SkillHandler s in Program.listSkillHandler)
                {
                    s.Stop();
                }
            }
        }

        public static bool AddSkillHandler(string command, int interval)
        {
            SkillHandler newSkillHandler = new SkillHandler(command, interval, Program.qHandler);
            if (newSkillHandler.Verified())
            {
                Program.listSkillHandler.Add(newSkillHandler);
                return true;
            }
            else
                return false;
        }

        public static void Hook()
        {
            if (!Program.hooked)
                Program.hooked = Process.Open();
            if (!Program.hooked)
                Console.WriteLine("Unable to hook to Aion process.");
        }
        public static void Unhook()
        {
            if (Program.hooked)
                Program.hooked = !Process.Close();
            if (Program.hooked)
                Console.WriteLine("Unable to unhook from Aion process!");

            if (qHandler.running) { Program.StopQueueHandler(); }
            foreach (SkillHandler s in Program.listSkillHandler)
            {
                s.Stop();
            }
        }

    }

    class Skill {
        public byte byteCode = 0x00;
        public byte specialCode = 0x00;

        public Skill(byte byteCode, byte specialCode)
        {
            this.byteCode = byteCode;
            this.specialCode = specialCode;
        }
    }
}
