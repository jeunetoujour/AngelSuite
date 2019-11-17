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
            //listBox1.Items.Clear();
            //elist.Update();
            //player.Update();
            
            //foreach (var entity in elist)
            //{
            //    //if (entity.Name == "Kerubiel Looter")
            //        listBox1.Items.Add(entity.Name + " " + player.Distance3D(entity));
            //}

            //player.MaxXP

            //var t = new Target() { ID = player.TargetID, };
            //t.Update();                                  


        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var abilities = new AbilityList();
            abilities.Update();

            listBox1.Items.Add(String.Format("Count: {0}", abilities.Count()));

            foreach (var ability in abilities)
            {
                listBox1.Items.Add(String.Format("Name: {0}, UsableAt: {1}, CooldownLength: {2}", ability.AbilityName, ability.AvailableAtTick, ability.CooldownLength));
            }

            
        }
    }
}
