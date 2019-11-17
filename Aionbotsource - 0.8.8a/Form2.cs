﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AngelBot
{
    public partial class waypointeditor : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);
        List<cwaypoint> waypointlist = new List<cwaypoint>();
        List<cwaypoint> deathpointlist = new List<cwaypoint>();
        Form1 f1 = (Form1)Application.OpenForms["Form1"];
        bool testwaypoint = false;

        public waypointeditor()
        {
            InitializeComponent();
        }

        private void waypointeditor_Load(object sender, EventArgs e)
        {
            waypointlist = f1.waypointlist;
            deathpointlist = f1.deathpointlist;
            loadwaypoints();
        }

        public void loadwaypoints()
        {
            foreach (cwaypoint wpoint in waypointlist)
            {
                string x, y, z, fullstring;
                
                x = string.Format("{0:f2}",wpoint.X);
                y = string.Format("{0:f2}",wpoint.Y);
                z = string.Format("{0:f2}",wpoint.Z);
                fullstring = x + " | " + y + " | " + z;
                listBox1.Items.Add(fullstring);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            waypointlist.Clear();
            foreach (string newloc in listBox1.Items)
            {
                string[] theitem = newloc.Split('|');
                cwaypoint wpoint = new cwaypoint();
                wpoint.X = Convert.ToDouble(theitem[0]);
                wpoint.Y = Convert.ToDouble(theitem[1]);
                wpoint.Z = Convert.ToDouble(theitem[2]);
                waypointlist.Add(wpoint);
            }

            this.Close();
        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string butt = (string)listBox1.SelectedItem; ;
            if (butt != null)
            {
                //butt = butt.Substring(butt.IndexOf(':')+2);
                string[] theitem = butt.Split('|');
                textBox1.Text = theitem[0].Trim();
                textBox2.Text = theitem[1].Trim();
                textBox3.Text = theitem[2].Trim();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string newitem = textBox1.Text + " | " + textBox2.Text + " | " + textBox3.Text;
            listBox1.Items.Add(newitem);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            object o = this.listBox1.SelectedItem;
            object newobj = textBox1.Text + " | " + textBox2.Text + " | " + textBox3.Text;
            this.listBox1.Items[i] = newobj;         
        }

        private void button17_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            object o = this.listBox1.SelectedItem;

            if (i > 0)
            {
                this.listBox1.Items.RemoveAt(i);
                this.listBox1.Items.Insert(i - 1, o);
                this.listBox1.SelectedIndex = i - 1;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            object o = this.listBox1.SelectedItem;

            if (i < this.listBox1.Items.Count - 1)
            {
                this.listBox1.Items.RemoveAt(i);
                this.listBox1.Items.Insert(i + 1, o);
                this.listBox1.SelectedIndex = i + 1;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                listBox1.Items.Clear();
                foreach (cwaypoint wpoint in deathpointlist)
                {
                    string x, y, z, fullstring;

                    x = string.Format("{0:f2}", wpoint.X);
                    y = string.Format("{0:f2}", wpoint.Y);
                    z = string.Format("{0:f2}", wpoint.Z);
                    fullstring = x + " | " + y + " | " + z;
                    listBox1.Items.Add(fullstring);
                    //listBox1.Items.Add(wpoint);
                    //counter++;
                }
            }
            else
            {
                listBox1.Items.Clear();
                foreach (cwaypoint wpoint in waypointlist)
                {
                    string x, y, z, fullstring;

                    x = string.Format("{0:f2}", wpoint.X);
                    y = string.Format("{0:f2}", wpoint.Y);
                    z = string.Format("{0:f2}", wpoint.Z);
                    fullstring = x + " | " + y + " | " + z;
                    listBox1.Items.Add(fullstring);
                    //listBox1.Items.Add(wpoint);
                    //counter++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (testwaypoint == false)
            {
                SetForegroundWindow(f1.hwndAion);
                testwaypoint = true;
                
                button1.Text = "Stop testing";
                Application.DoEvents();
                Thread.Sleep(700);          
                
                do
                {
                    //Application.DoEvents();
                    f1.waymovement();
                    f1.keyenumerator("w");
                } while (testwaypoint == true);
                
            }
            else
            {
                SetForegroundWindow(f1.hwndAion); 
                testwaypoint = false;
                f1.keyenumerator("s");
                button1.Text = "Test Waypoints";
            }

        }
    }
}
