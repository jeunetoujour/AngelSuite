using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using AionMemory;
using MemoryLib;

namespace AngelBot
{
    public partial class SkillEditor : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int GetTickCount();
        AbilityList abilities = new AbilityList();
        //TreeNode tn;
        private TreeNode sourceNode;

        object[,] ControlList = new object[12, 5];

        public SkillEditor()
        {
            InitializeComponent();
        }


        static bool ContainsStr(List<string> list, string comparedValue)
        {
            foreach (string listValue in list)
            {
                string[] stringarray1 = comparedValue.Split(' ');
                string[] stringarray2 = listValue.Split(' ');

                if (stringarray1[0] == stringarray2[0])
                {
                    if (listValue.Contains(comparedValue)) return true;
                }

            }
            return false;
        }

        private void GetWindows()
        {
            for (int b = 0; b < 12; b++)
            {
                ControlList[b, 0] = 0; //parent
                ControlList[b, 1] = 0; //address
                ControlList[b, 2] = 0; //name of slot
                ControlList[b, 3] = 0; //ready
                ControlList[b, 4] = 0; //skill number
            }
            AddControl((Process.Modules.Game + 0xA657D0), -1);


            int i = 0;

            if (Convert.ToInt32(ControlList[i, 1]) != 0)
            {
                int baseoffset = Memory.ReadInt(Process.handle, (Convert.ToInt32(ControlList[i, 1])));
                int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                for (int j = 1; j <= childsize - 1; j++)
                {
                    thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                    int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                    AddControl(final, i);
                }

            }

        }
        private void AddControl(int address, int parent)
        {
            int controlname = (int)Memory.ReadUInt(Process.handle, (uint)(address + 0x1c));
            string scontrolname = "";
            byte controlname2 = 0;
            int scontrolname1 = 0;
            if ((controlname > 15 || controlname < 0) && controlname != -1)
            {
                controlname = (int)Memory.ReadUInt(Process.handle, (uint)(address));
                int controlname1 = (int)Memory.ReadUInt(Process.handle, (uint)(controlname + 0xc));
                //int temp  = (int)Memory.ReadUInt(Process.handle, (uint)address );
                controlname2 = Memory.ReadByte(Process.handle, (uint)(controlname + 0x24));
                scontrolname = Memory.ReadString(Process.handle, (controlname1 + 0x0), 32, false);
            }
            else if (controlname > 0)
            {
                scontrolname = Memory.ReadString(Process.handle, (uint)(address + 0xc), 32, false);
                controlname2 = Memory.ReadByte(Process.handle, (uint)(address + 0x24));
                scontrolname1 = (int)Memory.ReadUInt(Process.handle, ((uint)(address + 0x2D8)));
            }
            else
                scontrolname = "xxxxxxxxxxxxx";

            for (int i = 0; i < 12; i++)
            {
                if (Convert.ToInt32(ControlList[i, 1]) == 0)
                {
                    ControlList[i, 0] = parent;
                    ControlList[i, 1] = address; //address
                    ControlList[i, 2] = scontrolname; //chainname
                    if (controlname2 == 7) ControlList[i, 3] = true; //skill is up
                    else ControlList[i, 3] = false;
                    //ControlList[i,3] = controlname2; //ready
                    ControlList[i, 4] = scontrolname1; //skill number
                    break;
                }

            }
        }


        private void button27_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            List<string> Abilitylist = new List<string>();
            List<string> Abilitylist2 = new List<string>();
            abilities.Update();

            foreach (Ability item in abilities)
            { //IsAlphaNumeric(item.AbilityName) == false &&
                if ((item.AbilityName.Contains("Basic") == false) && (item.AbilityName.Contains("Advanced") == false) && (item.AbilityName.Contains("Boost") == false) && (item.AbilityName.Contains("Increase") == false))
                {
                    Abilitylist.Add(item.AbilityName);
                }
            }
            Abilitylist.Sort(); //group skills together
            Abilitylist.Reverse(); //Newest skills first to filter

            foreach (string thing in Abilitylist)
            {
                string[] stringarray = thing.Split(' ');
                string temp = "";
                int last = stringarray.Length - 1;
                if (stringarray[last] == "I" || stringarray[last] == "II" || stringarray[last] == "III" || stringarray[last] == "IV" || stringarray[last] == "V" || stringarray[last] == "VI")
                {
                    for (int i = 0; i < last; i++)
                    {
                        if (temp != "") temp = temp + " ";
                        temp = temp + stringarray[i];
                    }
                }
                else temp = thing;
                //if (ContainsStr(Abilitylist2, temp) == false) //removes skills that are below level
                Abilitylist2.Add(thing);

            }
            Abilitylist2.Reverse(); //put list back in alphabetical order
            listBox4.Items.AddRange(Abilitylist2.ToArray());
        }


        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox20.Text = listBox4.SelectedItem.ToString();
            abilities[textBox20.Text].Update();
            //textBox21.Text = abilities[textBox20.Text].CooldownLength.ToString();
            //textBox22.Text = abilities[textBox20.Text].Ready.ToString();
            checkBox1.Checked = abilities[textBox20.Text].PerChance;
            hotkey.Text = abilities[textBox20.Text].Keypress;
            txtcasttime.Text = abilities[textBox20.Text].CastTime.ToString();
            txtcasttimemod.Text = abilities[textBox20.Text].CastTimeMod.ToString();
        }

        private void SkillEditor_Load(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            List<string> Abilitylist = new List<string>();
            List<string> Abilitylist2 = new List<string>();
            abilities.Update();

            foreach (Ability item in abilities)
            { //IsAlphaNumeric(item.AbilityName) == false &&
                if ((item.AbilityName.Contains("Toggle") == false) && (item.AbilityName.Contains("Basic") == false) && (item.AbilityName.Contains("Advanced") == false) && (item.AbilityName.Contains("Boost") == false) && (item.AbilityName.Contains("Increase") == false))
                {
                    Abilitylist.Add(item.AbilityName);
                }
            }
            Abilitylist.Sort(); //group skills together
            Abilitylist.Reverse(); //Newest skills first to filter

            foreach (string thing in Abilitylist)
            {
                string[] stringarray = thing.Split(' ');
                string temp = "";
                int last = stringarray.Length - 1;
                if (stringarray[last] == "I" || stringarray[last] == "II" || stringarray[last] == "III" || stringarray[last] == "IV" || stringarray[last] == "V" || stringarray[last] == "VI")
                {
                    for (int i = 0; i < last; i++)
                    {
                        if (temp != "") temp = temp + " ";
                        temp = temp + stringarray[i];
                    }
                }
                else temp = thing;
                if (ContainsStr(Abilitylist2, temp) == false) Abilitylist2.Add(thing);

            }
            Abilitylist2.Reverse(); //put list back in alphabetical order
            listBox4.Items.AddRange(Abilitylist2.ToArray());

            FillTree();
            loadbtn();
        }

        public void FillTree()
        {

            //I know this is just stupid to do it this way, but I couldnt be bother as long as it works im okay lol
            TreeNode mainPNode = new TreeNode();
            mainPNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainPNode.Text = "PreAttacks";
            this.skilltree.Nodes.Add(mainPNode);
            TreeNode mainANode = new TreeNode();
            mainANode.Name = "mainNode" + skilltree.Nodes.Count;
            mainANode.Text = "Attacks";
            this.skilltree.Nodes.Add(mainANode);
            TreeNode mainRNode = new TreeNode();
            mainRNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainRNode.Text = "Reactions";
            this.skilltree.Nodes.Add(mainRNode);
            TreeNode mainHNode = new TreeNode();
            mainHNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainHNode.Text = "Heals";
            this.skilltree.Nodes.Add(mainHNode);
            skilltree.SelectedNode = mainPNode;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            TreeNode NewNode = new TreeNode();
            NewNode.Text = textBox20.Text;//skilllist.SelectedItems[ctr].SubItems[1].Text;

            if (skilltree.SelectedNode.Level > 1 && hotkey.Text == "") hotkey.Text = abilities[skilltree.SelectedNode.Parent.Text].Keypress;

            abilities[textBox20.Text].Keypress = hotkey.Text;
            abilities[textBox20.Text].CastTimeMod = Convert.ToInt32(txtcasttimemod.Text);
            if (txtcastpercent.Text != "")
                abilities[textBox20.Text].CastPercent = Convert.ToInt32(txtcastpercent.Text);
            abilities[textBox20.Text].PerChance = checkBox1.Checked;
            NewNode.Tag = txtcasttimemod.Text + "|" + hotkey.Text + "|" + txtcastpercent.Text + "|" + checkBox1.Checked.ToString();//skilllist.SelectedItems[ctr].SubItems[0].Text;
            NewNode.Name = "Skill" + skilltree.Nodes.Count + 1; //better way of doing this?

            skilltree.SelectedNode.Nodes.Add(NewNode);
            skilltree.SelectedNode.Expand();

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            skilltree.SelectedNode.Remove();
        }



        private void button6_Click(object sender, EventArgs e)
        {
            skilltree.Nodes.Clear();
            FillTree();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            FileStream file = new FileStream(f1.pc.Name + ".skills", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            for (int ctr = 0; ctr < skilltree.Nodes.Count; ctr++)
            {
                string x = skilltree.Nodes[ctr].Text;
                int level = skilltree.Nodes[ctr].Level;
                sw.WriteLine(level + "|" + x);
                for (int cnt = 0; cnt < skilltree.Nodes[ctr].Nodes.Count; cnt++)
                {
                    x = skilltree.Nodes[ctr].Nodes[cnt].Text;
                    level = skilltree.Nodes[ctr].Nodes[cnt].Level;
                    string ID = (string)skilltree.Nodes[ctr].Nodes[cnt].Tag;
                    //string y = skilltree.Nodes[ctr].Nodes[cnt].Nodes[0].Text;
                    //string y = skilltree.Nodes[ctr].Nodes[cnt].Text;
                    sw.WriteLine(level + "|" + ID + "|" + x);
                    if (skilltree.Nodes[ctr].Nodes[cnt].Nodes.Count > 0)
                    {
                        for (int cnt1 = 0; cnt1 < skilltree.Nodes[ctr].Nodes[cnt].Nodes.Count; cnt1++)
                        {
                            x = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Text;
                            level = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Level;
                            string ID1 = (string)skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Tag;
                            //string y1 = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Text;
                            sw.WriteLine(level + "|" + ID1 + "|" + x);
                            if (skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes.Count > 0) //2nd chain
                            {
                                for (int cnt2 = 0; cnt2 < skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes.Count; cnt2++)
                                {
                                    x = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Text;
                                    level = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Level;
                                    string ID2 = (string)skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Tag;
                                    //string y2 = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Text;
                                    sw.WriteLine(level + "|" + ID2 + "|" + x);
                                    if (skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes.Count > 0) //3nd chain
                                    {
                                        for (int cnt3 = 0; cnt3 < skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes.Count; cnt3++)
                                        {
                                            x = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Text;
                                            level = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Level;
                                            string ID3 = (string)skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Tag;
                                            //string y2 = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Text;
                                            sw.WriteLine(level + "|" + ID3 + "|" + x);
                                            if (skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Nodes.Count > 0) //3nd chain
                                            {
                                                for (int cnt4 = 0; cnt4 < skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Nodes.Count; cnt4++)
                                                {
                                                    x = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Nodes[cnt4].Text;
                                                    level = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Nodes[cnt4].Level;
                                                    string ID4 = (string)skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Nodes[cnt2].Nodes[cnt3].Nodes[cnt4].Tag;
                                                    //string y2 = skilltree.Nodes[ctr].Nodes[cnt].Nodes[cnt1].Text;
                                                    sw.WriteLine(level + "|" + ID4 + "|" + x);
                                                }
                                            }//4end if chain
                                        }
                                    }//3end if chain
                                }
                            }//2end if chain
                        }
                    }//1end if chain
                }

            }

            sw.Close();
            file.Close();
            f1.getattacklist();
        }
        public void loadbtn()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (File.Exists(f1.pc.Name + ".skills"))
            {

                FileStream file = new FileStream(f1.pc.Name + ".skills", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);


                string line;
                TreeNode parent = new TreeNode();
                TreeNode parent1 = new TreeNode();
                TreeNode parent2 = new TreeNode();
                TreeNode parent3 = new TreeNode();
                TreeNode parent4 = new TreeNode();
                skilltree.Nodes.Clear();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');

                    if (parts[0] == "0")
                    {
                        TreeNode mainNode = new TreeNode();
                        mainNode.Name = "mainNode" + skilltree.Nodes.Count;
                        mainNode.Text = parts[1];
                        this.skilltree.Nodes.Add(mainNode);
                        skilltree.SelectedNode = mainNode;
                        parent = mainNode;

                    }

                    else if (parts[0] == "1")
                    {

                        if (skilltree.SelectedNode.Level != 0)
                        {
                            skilltree.SelectedNode = parent;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);

                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1;
                        skilltree.SelectedNode.Nodes.Add(NewNode);
                        skilltree.SelectedNode = NewNode;

                        parent1 = NewNode;
                    }
                    else if (parts[0] == "2")
                    {

                        if (skilltree.SelectedNode.Level != 1)
                        {
                            skilltree.SelectedNode = parent1;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1;
                        skilltree.SelectedNode.Nodes.Add(NewNode);
                        skilltree.SelectedNode = NewNode;

                        parent2 = NewNode;
                    }
                    else if (parts[0] == "3")
                    {

                        if (skilltree.SelectedNode.Level != 2)
                        {
                            skilltree.SelectedNode = parent2;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1;
                        skilltree.SelectedNode.Nodes.Add(NewNode);
                        skilltree.SelectedNode = NewNode;

                        parent3 = NewNode;
                    }
                    else if (parts[0] == "4")
                    {

                        if (skilltree.SelectedNode.Level != 3)
                        {
                            skilltree.SelectedNode = parent3;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1;
                        skilltree.SelectedNode.Nodes.Add(NewNode);
                        skilltree.SelectedNode = NewNode;

                        parent4 = NewNode;
                    }
                    else if (parts[0] == "5")
                    {

                        if (skilltree.SelectedNode.Level != 4)
                        {
                            skilltree.SelectedNode = parent4;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1;
                        skilltree.SelectedNode.Nodes.Add(NewNode);

                        skilltree.SelectedNode = NewNode;

                    }
                }

                sr.Close();
                file.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadbtn();
        }


        private void skilltree_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (skilltree.SelectedNode.Level != 0)
            {
                textBox20.Text = skilltree.SelectedNode.Text;
                abilities[textBox20.Text].Update();
                if (skilltree.SelectedNode.Parent.Text == "Heals")
                {
                    txtcastpercent.Visible = true;
                    txtcastpercent.Text = abilities[textBox20.Text].CastPercent.ToString();
                }
                else
                {
                    txtcastpercent.Text = "";
                    txtcastpercent.Visible = false;
                }//textBox21.Text = abilities[textBox20.Text].CooldownLength.ToString();
                //textBox22.Text = abilities[textBox20.Text].Ready.ToString();
                checkBox1.Checked = abilities[textBox20.Text].PerChance;
                hotkey.Text = abilities[textBox20.Text].Keypress;
                txtcasttime.Text = abilities[textBox20.Text].CastTime.ToString();
                txtcasttimemod.Text = abilities[textBox20.Text].CastTimeMod.ToString();
                skilltree.Focus();
            }
        }

        private void updateskill()
        {
            skilltree.SelectedNode.Text = textBox20.Text;
            abilities[textBox20.Text].Keypress = hotkey.Text;
            abilities[textBox20.Text].CastTimeMod = Convert.ToInt32(txtcasttimemod.Text);
            if (txtcastpercent.Text != "")
                abilities[textBox20.Text].CastPercent = Convert.ToInt32(txtcastpercent.Text);
            abilities[textBox20.Text].PerChance = checkBox1.Checked;

            skilltree.SelectedNode.Tag = txtcasttimemod.Text + "|" + hotkey.Text + "|" + txtcastpercent.Text + "|" + checkBox1.Checked.ToString();//BUG, What if more than one of same skill?
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateskill();

        }

        private void label4_Click(object sender, EventArgs e)
        {
            loadbtn();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            int i = this.skilltree.SelectedNode.Index;
            TreeNode o = this.skilltree.SelectedNode;
            
            if (i > 0)
            {               
                this.skilltree.SelectedNode.Parent.Nodes.RemoveAt(i);
                this.skilltree.SelectedNode.Parent.Nodes.Insert(i - 1, o);
                if (i == this.skilltree.SelectedNode.Parent.Nodes.Count-1)
                    this.skilltree.SelectedNode = this.skilltree.SelectedNode.PrevNode;
                else
                    this.skilltree.SelectedNode = this.skilltree.SelectedNode.PrevNode.PrevNode;
            }
            skilltree.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = this.skilltree.SelectedNode.Index;
            TreeNode o = this.skilltree.SelectedNode;

            if (i < this.skilltree.SelectedNode.Parent.Nodes.Count-1)
            {
                this.skilltree.SelectedNode.Parent.Nodes.RemoveAt(i);
                this.skilltree.SelectedNode.Parent.Nodes.Insert(i+1, o);
                this.skilltree.SelectedNode = this.skilltree.SelectedNode.NextNode;
            }
            skilltree.Focus();
        }

    }
}
