using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace AngelSuite
{
    public partial class Launcher : Form
    {
        public string prog = "";
        public string version1 = "XX";
        public string version2 = "2";
        public string version3 = "2";
        public string version4 = "2";
        public string version5 = "3"; //radar
        public string version6 = "2";
        public string version7 = "3";
        public string version8 = "6";

        public Launcher()
        {
            InitializeComponent();
        }

        private static string FindFile()
        {
            FileInfo[] files = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*.woo");
            if (files.Length == 1)
            {
                return files[0].Name.Substring(0, files[0].Name.Length - 4);
            }
            return "";
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor((double)((26.0 * random.NextDouble()) + 65.0))));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            label1.Text = "AngelSuite " + this.ProductVersion;
            label2.Text = version1;
            label3.Text = version2;
            label4.Text = version3;
            label5.Text = version4;
            label6.Text = version5;
            label7.Text = version6;
            label8.Text = version7;
            label9.Text = version8;

        }

        private void startab2()
        {
            File.Delete(FindFile() + ".exe");
            File.Delete(FindFile() + ".woo");
            Random random = new Random((int)DateTime.Now.Ticks);
            string str = RandomString(random.Next(4, 8), false);
            using (StreamWriter writer = File.CreateText(str + ".woo"))
            {
                writer.Close();
            }
            try
            {
                File.Copy("Angelbot.exe", str + ".exe");
            }
            catch
            {
                Environment.Exit(1);
            }
            Process process = new Process();
            process.StartInfo.FileName = str + ".exe";
            process.Start();
        }

        private void startabhealer()
        {
            Process process = new Process();
            process.StartInfo.FileName = prog + ".exe";
            process.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (prog == "AB2") startab2();
            if (prog == "AngelHealer") startabhealer();

            if (prog == "Target")
            {
                AngelTarget targetfrm = new AngelTarget();
                targetfrm.Show();
            }
            if (prog == "PVP") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "PVP";
                loginfrm.theversion = version3;
                loginfrm.Show();
            }
            if (prog == "DPS") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "DPS";
                loginfrm.theversion = version4;
                loginfrm.Show();
            }
            if (prog == "Radar") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "Radar";
                loginfrm.theversion = version5;
                loginfrm.Show();
            }
            if (prog == "List") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "List";
                loginfrm.theversion = version6;
                loginfrm.Show();
            }
            if (prog == "AP") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "AP";
                loginfrm.theversion = version7;
                loginfrm.Show();
            }
            if (prog == "Wings") 
            {
                Login loginfrm = new Login();
                loginfrm.program = "Wings";
                loginfrm.theversion = version8;
                loginfrm.Show();
            } 

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            prog = "AB2";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            prog = "AngelHealer";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            prog = "PVP";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            prog = "DPS";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            prog = "Radar";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            prog = "List";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            prog = "AP";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            prog = "Wings";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            prog = "Target";
        }
    }
}
