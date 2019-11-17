using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BuffMe
{
    class SkillHandler
    {
        Timer skillTimer = new Timer();

        private bool verified = false;

        private byte byteCode = 0x00;
        private byte specialCode = 0x00;

        private QueueHandler qHandler;

        // Currently for aesthetic purposes only
        public string command = "";
        public int interval = 0;

        /// <summary>
        /// Initialize with a command string (see below), interval in ms and an instance of QueueHandler.
        /// </summary>
        public SkillHandler(string command, int interval, QueueHandler qHandler)
        {
            Console.WriteLine("SkillHandler created: Cmd: {0} Int.: {1}", command, interval);

            this.qHandler = qHandler;
            this.interval = interval;
            this.command = command;

            #region this.byteCodeSwitch
            string special = "";
            if (command.Contains(","))
            {
                special = command.Split(',')[0];
                command = command.Split(',')[1];
            }
            switch (command.ToLower())
            {
                case "a": this.byteCode = 0x41; break;
                case "b": this.byteCode = 0x42; break;
                case "c": this.byteCode = 0x43; break;
                case "d": this.byteCode = 0x44; break;
                case "e": this.byteCode = 0x45; break;
                case "f": this.byteCode = 0x46; break;
                case "g": this.byteCode = 0x47; break;
                case "h": this.byteCode = 0x48; break;
                case "i": this.byteCode = 0x49; break;
                case "j": this.byteCode = 0x4a; break;
                case "k": this.byteCode = 0x4b; break;
                case "l": this.byteCode = 0x4c; break;
                case "m": this.byteCode = 0x4d; break;
                case "n": this.byteCode = 0x4e; break;
                case "o": this.byteCode = 0x4f; break;
                case "p": this.byteCode = 0x50; break;
                case "q": this.byteCode = 0x51; break;
                case "r": this.byteCode = 0x52; break;
                case "s": this.byteCode = 0x53; break;
                case "t": this.byteCode = 0x54; break;
                case "u": this.byteCode = 0x55; break;
                case "v": this.byteCode = 0x56; break;
                case "w": this.byteCode = 0x57; break;
                case "x": this.byteCode = 0x58; break;
                case "y": this.byteCode = 0x59; break;
                case "z": this.byteCode = 0x5a; break;

                case "0": this.byteCode = 0x30; break;
                case "1": this.byteCode = 0x31; break;
                case "2": this.byteCode = 0x32; break;
                case "3": this.byteCode = 0x33; break;
                case "4": this.byteCode = 0x34; break;
                case "5": this.byteCode = 0x35; break;
                case "6": this.byteCode = 0x36; break;
                case "7": this.byteCode = 0x37; break;
                case "8": this.byteCode = 0x38; break;
                case "9": this.byteCode = 0x39; break;
            }

            if (special != "")
            {
                switch (special.ToLower())
                {
                    case "shift": this.specialCode = 0x10; break;
                    case "ctrl": this.specialCode = 0x11; break;
                    case "alt": this.specialCode = 0x12; break;
                }
            }
            #endregion
            if (byteCode != 0x00)
            {
                this.skillTimer.Interval = interval;
                this.skillTimer.Tick += new EventHandler(skillTimer_Tick);

                if (qHandler.running)
                {
                    Start();
                }

                this.verified = true;
            }
        }

        public bool Verified()
        {
            return verified;
        }

        void QueueUp()
        {
            Skill newSkill = new Skill(this.byteCode, this.specialCode);
            if(!this.qHandler.Exists(newSkill))
                this.qHandler.Add(newSkill);
        }

        void skillTimer_Tick(object sender, EventArgs e)
        {
            QueueUp();
            Console.WriteLine("Tick? Tock! SkillHandler's skillTimer ticked.");
        }

        public void Stop()
        {
            this.skillTimer.Stop();
        }

        public void Start()
        {
            QueueUp();
            this.skillTimer.Start();
        }
    }
}
