using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AionMemory;
using System.Threading;

namespace BuffMe
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Program.Hook();

            btnSwitcharoo();
        }

        private void btnSwitcharoo()
        {
            btnHook.Enabled = !Program.hooked;
            btnUnhook.Enabled = Program.hooked;

            btnStart.Enabled = (Program.hooked && !Program.qHandler.running);
            btnStop.Enabled = (Program.hooked && Program.qHandler.running);
        }

        private void btnHook_Click(object sender, EventArgs e)
        {
            Program.Hook();
            btnSwitcharoo();
        }

        private void btnUnhook_Click(object sender, EventArgs e)
        {
            Program.Unhook();
            btnSwitcharoo();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try {
                if (Program.AddSkillHandler(boxCommand.Text, int.Parse(boxInterval.Text) * 1000))
                {
                    this.listBox1.Items.Add(string.Format("Cmd: {0} Int.: {1}", boxCommand.Text, boxInterval.Text));
                }
            } catch (Exception exc) { Console.WriteLine("Failed to add skill: {0}", exc.Message); }

            btnSwitcharoo();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (Program.listSkillHandler.Count > index)
            {
                this.listBox1.Items.Remove(listBox1.Items[index]);
                Program.listSkillHandler.Remove(Program.listSkillHandler[index]); 
            }
            btnSwitcharoo();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Program.StartQueueHandler();
            Thread.Sleep(500);
            btnSwitcharoo();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Program.StopQueueHandler();
            btnSwitcharoo();
        }
    }
}
