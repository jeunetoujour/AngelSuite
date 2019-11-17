using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Ini;


namespace Loadini
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IniFile ini = new IniFile("C:\\settings.ini");
            textBox1.Text = ini.IniReadValue("limits", "PotHP");

        }

    }

}
