using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using AionMemory;

namespace Sandbox
{
    public partial class Form1 : Form
    {
        EntityList elist = new EntityList();
        Player player;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process.Open();
            player = new Player();
            Target target = new Target();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            elist.Update();
            player.Update();
            listBox1.Items.Clear();
            foreach (var entity in elist)
            {
                if (entity.Name == "Kerubiel Looter")
                    listBox1.Items.Add(entity.Name + " " + player.Distance3D(entity));
            }
        }
    }
}
