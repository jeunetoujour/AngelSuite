using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using AionMemory;
//using MemoryLib;
using Ini;

namespace AngelBot
{
    public partial class settings : Form
    {
       
        public settings()
        {
            InitializeComponent();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void loadsettings()
        {
            //Tab1
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
            IniFile initemplate = new IniFile(Environment.CurrentDirectory + "\\template.ini");

            if (initemplate.Exists())
            {
                initemplate.Load();
            }
            else
            {
                MessageBox.Show("You have no template.ini file! ");
            }


            if (ini.Exists())
                ini.Load();
            else //copy file
            {
                using (FileStream fs = File.Create(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini")) { }
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
                ini.Load();
            }
            textBox1.Text = GetSetting(ini, initemplate, "limits", "RestHP");
            textBox2.Text = GetSetting(ini, initemplate, "limits", "RestMana");
            textBox3.Text = GetSetting(ini, initemplate, "limits", "HealHP");
            textBox17.Text = GetSetting(ini, initemplate, "limits", "HealDelay");
            textBox18.Text = GetSetting(ini, initemplate, "limits", "HealCD");
            textBox4.Text = GetSetting(ini, initemplate, "limits", "PotHP");
            textBox5.Text = GetSetting(ini, initemplate, "limits", "PotMP");
            textBox6.Text = GetSetting(ini, initemplate, "limits", "IgnoreLevel");
            textBox7.Text = GetSetting(ini, initemplate, "limits", "IgnoreTime");
            txtooc.Text = GetSetting(ini, initemplate, "limits", "OOCHeal");
            comboBox1.SelectedItem = GetSetting(ini, initemplate, "limits", "OOCType");
            textBox25.Text = GetSetting(ini, initemplate, "limits", "Shutoff");
            textBox26.Text = GetSetting(ini, initemplate, "limits", "DeathRest");

            //Tab2
            if ("True" == GetSetting(ini, initemplate, "character", "Healer"))
            { checkBox1.Checked = true; }
            else { checkBox1.Checked = false; }

            if ("True" == GetSetting(ini, initemplate, "character", "Ranged"))
            { checkBox2.Checked = true; }
            else { checkBox2.Checked = false; }

            if ("True" == GetSetting(ini, initemplate, "character", "Antistuck"))
            { checkBox3.Checked = true; }
            else { checkBox3.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "FullInv"))
            { checkBox4.Checked = true; }
            else { checkBox4.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "Logging"))
            { checkBox5.Checked = true; }
            else { checkBox5.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "LeftRes"))
            { checkBox6.Checked = true; }
            else { checkBox6.Checked = false; }

            textBox8.Text = GetSetting(ini, initemplate, "character", "RangeDist");
            textBox19.Text = GetSetting(ini, initemplate, "character", "Lootdelay");
            //Tab3
            string pretemp = GetSetting(ini, initemplate, "preattacks", "PreAttacks");
            if (pretemp != "")
            {
                if (pretemp.StartsWith("|")) pretemp = pretemp.Substring(1, pretemp.Length);
                if (pretemp.Contains('\0').ToString() == "True")
                {
                    pretemp = pretemp.Substring(0, pretemp.LastIndexOf('\0') - 0);
                }
                string[] listpreattack = pretemp.Split('|');
                listBox1.Items.AddRange(listpreattack);
            }
            //Tab4
            string attacktemp = GetSetting(ini, initemplate, "attacks", "Attacks");
            if (attacktemp != "")
            {
                if (attacktemp.StartsWith("|")) attacktemp = attacktemp.Substring(1, attacktemp.Length);
                if (attacktemp.Contains('\0').ToString() == "True")
                {
                    attacktemp = attacktemp.Substring(0, attacktemp.LastIndexOf('\0') - 0);
                }
                string[] listattack = attacktemp.Split('|');
                listBox2.Items.AddRange(listattack);
            }//Tab5
            string buffstemp = GetSetting(ini, initemplate, "buffs", "Buffs");
            if (buffstemp != "")
            {
                if (buffstemp.StartsWith("|")) buffstemp = buffstemp.Substring(1, buffstemp.Length);
                if (buffstemp.Contains('\0').ToString() == "True")
                {
                    buffstemp = buffstemp.Substring(0, buffstemp.LastIndexOf('\0') - 0);
                }
                string[] listbuffs = buffstemp.Split('|');
                listBox3.Items.AddRange(listbuffs);
            }//Tab6
            string healstemp = GetSetting(ini, initemplate, "heals", "Heals");
            if (healstemp != "")
            {
                if (healstemp.StartsWith("|")) healstemp = healstemp.Substring(1, healstemp.Length);
                if (healstemp.Contains('\0').ToString() == "True")
                {
                    healstemp = healstemp.Substring(0, healstemp.LastIndexOf('\0') - 0);
                }
                string[] listheals = healstemp.Split('|');
                listBox5.Items.AddRange(listheals);
            }
            //Tab7
            txtloot.Text = GetSetting(ini, initemplate, "keybinds", "LootBtn");
            txtrest.Text = GetSetting(ini, initemplate, "keybinds", "RestBtn");
            txthppot.Text = GetSetting(ini, initemplate, "keybinds", "Healthpot");
            txtmppot.Text = GetSetting(ini, initemplate, "keybinds", "Manapot");
            txttarget.Text = GetSetting(ini, initemplate, "keybinds", "TargetBtn");
            txtself.Text = GetSetting(ini, initemplate, "keybinds", "SelfTarget");
            txtturn.Text = GetSetting(ini, initemplate, "keybinds", "TurnAround");
            txtautoattack.Text = GetSetting(ini, initemplate, "keybinds", "Autoattack");
            txtheal.Text = GetSetting(ini, initemplate, "keybinds", "Heal");
            textBox16.Text = GetSetting(ini, initemplate, "keybinds", "OOCH");
            textBox24.Text = GetSetting(ini, initemplate, "keybinds", "StrafeL");
            textBox23.Text = GetSetting(ini, initemplate, "keybinds", "StrafeR");
            textBox31.Text = GetSetting(ini, initemplate, "keybinds", "Return");
        
        }
        private string GetSetting(IniFile character, IniFile template, string section, string key)
        {
            if (character.HasKey(section, key))
                return character[section][key];
            else if (template.HasKey(section, key))
            {
                return template[section][key];
            }
            else
            {
                MessageBox.Show("You have no template.ini file or a corrupted copy! ");
                return "";
            }
        }
        private void settings_Load(object sender, EventArgs e)
        {
            
            loadsettings();

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string butt = (string)listBox2.SelectedItem; ;
            if (butt != null)
            {
                string[] theitem = butt.Split(':');
                textBox11.Text = theitem[0];
                textBox12.Text = theitem[1];
            }
            button12.Enabled = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string butt = (string)listBox1.SelectedItem;
            if (butt != null && butt != "")
            {
                string[] theitem = butt.Split(':');
                textBox9.Text = theitem[0];
                textBox10.Text = theitem[1];
            }
            button11.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            int i = this.listBox2.SelectedIndex;
            object o = this.listBox2.SelectedItem;

            if (i > 0)
            {
                this.listBox2.Items.RemoveAt(i);
                this.listBox2.Items.Insert(i - 1, o);
                this.listBox2.SelectedIndex = i - 1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = this.listBox2.SelectedIndex;
            object o = this.listBox2.SelectedItem;

            if (i < this.listBox2.Items.Count - 1)
            {
                this.listBox2.Items.RemoveAt(i);
                this.listBox2.Items.Insert(i + 1, o);
                this.listBox2.SelectedIndex = i + 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
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

        private void savesettings()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");

            if (ini.Exists())
                ini.Load();
            else //copy file
            {
                using (FileStream fs = File.Create(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini")) { }
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
                ini.Load();
            }

            ini["limits"]["RestHP"] = textBox1.Text;
            ini["limits"]["RestMana"] = textBox2.Text;
            ini["limits"]["HealHP"] = textBox3.Text;
            ini["limits"]["HealDelay"] = textBox17.Text;
            ini["limits"]["PotHP"] = textBox4.Text;
            ini["limits"]["PotMP"] = textBox5.Text;
            ini["limits"]["IgnoreLevel"] = textBox6.Text;
            ini["limits"]["IgnoreTime"] = textBox7.Text;
            ini["limits"]["OOCHeal"] = txtooc.Text;
            ini["limits"]["OOCType"] = comboBox1.SelectedItem.ToString();
            ini["limits"]["HealCD"] = textBox18.Text;
            ini["limits"]["Shutoff"] = textBox25.Text;
            ini["limits"]["DeathRest"] = textBox26.Text;

            if (checkBox1.Checked == true)
            {
                ini["character"]["Healer"] = "True";
            }
            else { ini["character"]["Healer"] = "False"; }

            if (checkBox2.Checked == true)
            {
                ini["character"]["Ranged"] = "True";
            }
            else { ini["character"]["Ranged"] = "False"; }

            if (checkBox3.Checked == true)
            {
                ini["character"]["Antistuck"] = "True";
            }
            else { ini["character"]["Antistuck"] = "False"; }

            if (checkBox4.Checked == true)
            {
                ini["character"]["FullInv"] = "True";
            }
            else { ini["character"]["FullInv"] = "False"; }

            if (checkBox5.Checked == true)
            {
                ini["character"]["Logging"] = "True";
            }
            else { ini["character"]["Logging"] = "False"; }
            if (checkBox6.Checked == true)
            {
                ini["character"]["LeftRes"] = "True";
            }
            else { ini["character"]["LeftRes"] = "False"; }

            ini["character"]["RangeDist"] = textBox8.Text;
            ini["character"]["Lootdelay"] = textBox19.Text;
            string listpreattack = "";
            foreach (object item in listBox1.Items)
            {
                listpreattack = listpreattack + item.ToString().TrimEnd(null) + "|";
            }
            listpreattack = listpreattack.TrimEnd('|');
            ini["preattacks"]["PreAttacks"] = listpreattack;

            string listattacks = "";
            foreach (object item in listBox2.Items)
            {
                listattacks = listattacks + Convert.ToString(item).TrimEnd(null) + "|";
            }
            listattacks = listattacks.TrimEnd('|');
            ini["attacks"]["Attacks"] = listattacks;

            string listbuffs = "";//buffs
            foreach (object item in listBox3.Items)
            {
                listbuffs = listbuffs + Convert.ToString(item).TrimEnd(null) + "|";
            }
            listbuffs = listbuffs.TrimEnd('|');
            ini["buffs"]["Buffs"] = listbuffs;

            string listheals = "";//heals
            foreach (object item in listBox5.Items)
            {
                listheals = listheals + Convert.ToString(item).TrimEnd(null) + "|";
            }
            listheals = listheals.TrimEnd('|');
            ini["heals"]["Heals"] = listheals;

            ini["keybinds"]["LootBtn"] = txtloot.Text;
            ini["keybinds"]["RestBtn"] = txtrest.Text;
            ini["keybinds"]["Healthpot"] = txthppot.Text;
            ini["keybinds"]["Manapot"] = txtmppot.Text;
            ini["keybinds"]["TargetBtn"] = txttarget.Text;
            ini["keybinds"]["SelfTarget"] = txtself.Text;
            ini["keybinds"]["TurnAround"] = txtturn.Text;
            ini["keybinds"]["Autoattack"] = txtautoattack.Text;
            ini["keybinds"]["Heal"] = txtheal.Text;
            ini["keybinds"]["OOCH"] = textBox16.Text;
            ini["keybinds"]["StrafeL"] = textBox24.Text;
            ini["keybinds"]["StrafeR"] = textBox23.Text;
            ini["keybinds"]["Return"] = textBox31.Text;

            ini.Save();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            savesettings(); //writes to ini
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (f1.savelog == true) { f1.savelog = false; f1.tw.Close(); }
            f1.loadsettings(); //loads new settings into RAM
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newitem = textBox9.Text + ":" + textBox10.Text;
            listBox1.Items.Add(newitem);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string newitem = textBox11.Text + ":" + textBox12.Text;
            listBox2.Items.Add(newitem);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Opacity = (Convert.ToDouble(textBox13.Text) / 100);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            object o = this.listBox1.SelectedItem;
            object newobj = textBox9.Text + ":" + textBox10.Text;

            this.listBox1.Items[i] = newobj;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            int i = this.listBox2.SelectedIndex;
            object o = this.listBox2.SelectedItem;
            object newobj = textBox11.Text + ":" + textBox12.Text;


            this.listBox2.Items[i] = newobj;

        }

        private void button18_Click(object sender, EventArgs e)
        {
            string newitem = textBox14.Text + ":" + textBox15.Text;
            listBox3.Items.Add(newitem);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            listBox3.Items.RemoveAt(listBox3.SelectedIndex);
        }

        private void button16_Click(object sender, EventArgs e)
        {

            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;

            if (i > 0)
            {
                this.listBox3.Items.RemoveAt(i);
                this.listBox3.Items.Insert(i - 1, o);
                this.listBox3.SelectedIndex = i - 1;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;

            if (i < this.listBox3.Items.Count - 1)
            {
                this.listBox3.Items.RemoveAt(i);
                this.listBox3.Items.Insert(i + 1, o);
                this.listBox3.SelectedIndex = i + 1;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string butt = (string)listBox3.SelectedItem; ;
            if (butt != null)
            {
                string[] theitem = butt.Split(':');
                textBox14.Text = theitem[0];
                textBox15.Text = theitem[1];
            }
            button13.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;
            object newobj = textBox14.Text + ":" + textBox15.Text;


            this.listBox3.Items[i] = newobj;

        }

        private void button21_Click(object sender, EventArgs e)
        {
            string newitem = textBox27.Text + ":" + textBox28.Text + ":" + textBox29.Text + ":" + textBox30.Text;
            listBox5.Items.Add(newitem);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            int i = this.listBox5.SelectedIndex;
            object o = this.listBox5.SelectedItem;
            object newobj = textBox27.Text + ":" + textBox28.Text + ":" + textBox29.Text + ":" + textBox30.Text;


            this.listBox5.Items[i] = newobj;

        }

        private void button23_Click(object sender, EventArgs e)
        {
            listBox5.Items.RemoveAt(listBox5.SelectedIndex);
        }

        private void button24_Click(object sender, EventArgs e)
        {

            int i = this.listBox5.SelectedIndex;
            object o = this.listBox5.SelectedItem;

            if (i > 0)
            {
                this.listBox5.Items.RemoveAt(i);
                this.listBox5.Items.Insert(i - 1, o);
                this.listBox5.SelectedIndex = i - 1;
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            int i = this.listBox5.SelectedIndex;
            object o = this.listBox5.SelectedItem;

            if (i < this.listBox5.Items.Count - 1)
            {
                this.listBox5.Items.RemoveAt(i);
                this.listBox5.Items.Insert(i + 1, o);
                this.listBox5.SelectedIndex = i + 1;
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string butt = (string)listBox5.SelectedItem; ;
            if (butt != null)
            {
                string[] theitem = butt.Split(':');
                textBox27.Text = theitem[0];
                textBox28.Text = theitem[1];
                textBox29.Text = theitem[2];
                textBox30.Text = theitem[3];
            }
            button22.Enabled = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (checkBox3.Checked == false)
            {
                f1.antistuck = true;
            }
            else f1.antistuck = false;
        }

       

       
        

    }
}
