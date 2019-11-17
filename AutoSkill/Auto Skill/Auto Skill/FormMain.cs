using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AionMemory;
using MemoryLib;
using Utility;
using Auto_Skill.States;


namespace Auto_Skill
{
    public partial class FormMain : Form, IPrintable
    {

        public EntityList entityList;
        private Player player;
        private Target target;
        KeyEvents keyEvents = new KeyEvents();

        State currentState = null;
        StateFactory stateFactory = null;
        SkillManager skills = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void GetAbilities()
        {
            string ThisName;
            int pointer1;
            int pointer2;
            int pointer3;
            int pointer4;
            int len;
            int pointer5;
            int pointer6;
            int pointer7;
            int offset = 0x0;
            int ThisID;
            int ThisBase;

            pointer1 = Memory.ReadInt(Process.handle, Process.Modules.Game + 0x924c50);
            pointer2 = Memory.ReadInt(Process.handle, pointer1 + 0x2c0);
            pointer3 = Memory.ReadInt(Process.handle, pointer2 + 0x28c);
            
           
            for (int ctr = 0; ctr < 100; ++ctr)
            {
                try
                {
                    pointer4 = Memory.ReadInt(Process.handle, pointer3 + offset);
                    ThisID = Memory.ReadInt(Process.handle, pointer4 + 0x90);
                    ThisBase = Memory.ReadInt(Process.handle, pointer4 + 0x0);
                    if (ThisID != 0)
                    {
                        pointer5 = Memory.ReadInt(Process.handle, pointer4 + 0x44);
                        pointer6 = Memory.ReadInt(Process.handle, pointer5 + 0x4);
                        len = Memory.ReadInt(Process.handle, pointer6 + 0x14);
                        if ((len > 7) & (ThisID < 10000))
                        {
                            pointer7 = Memory.ReadInt(Process.handle, pointer6 + 0x4);
                            ThisName = Memory.ReadString(Process.handle, pointer7 + 0x0, 64, true);
                        }
                        else
                        {
                            ThisName = Memory.ReadString(Process.handle, pointer6 + 0x4, 64, true);
                        }
                        if ((ThisID < 1000000) & (ThisID >= 0) & (ThisName.Length < 50))
                        {
                            //  ListViewItem NewSkill = new ListViewItem(ThisName);
                            //  skilllist.Items.Add(NewSkill);
                            //  found += 1;


                            ListViewItem NewSkill = new ListViewItem(new string[] { ThisID.ToString(), ThisName }, -1);


                            skilllist.Items.AddRange(new ListViewItem[] { NewSkill });

                        }


                    }
                }
                catch
                {
                }
                offset += 0x4;
            }

        }

        public void Print(string text)
        {
         if (textBoxMsg.TextLength > 5000) textBoxMsg.Clear();
         textBoxMsg.Text = text + "\r\n" + textBoxMsg.Text; ;

        }

        private void addskill_Click(object sender, EventArgs e)
        {
            GetAbilities();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (AionMemory.Process.Open() == false)
            {

                MessageBox.Show("Holy Cow! Are You An Idiot? You Need To Run AION First! Just Kiddin Mate!");
                Application.Exit();
            }
           
            entityList = new EntityList();
            player = new Player();
            target = new Target();
         
            player.Update();
            target.Update();
          
            FillTree();
            Print("This will automate your skills when you click a monster! the skill cooldown is memory base so it should be fast! "+
                   "To begin.. assign a shortcut key to looting! (must use a slot for looting!) sitting and healing is not implemented on this! after that \n\r" +
                " click Get Skills  to propagate skill list. if you want to add a skill to the Pull Skill Tree, then click" +
                    " Pull Skills, then select a skill and click >>>> to add. same thing with Combat Heal Skills and Chain Skills.. to add more Chain Skills" +
                    " press Chain. assign shortcut keys to each skill by clicking the skill name on the tree to expand it! " +
                    " and when your ready, save your list, and click start.I have include my save file so you can see how it is setup. just click Load, then click clear" +
                    "once your ready with yours");
                   


        }


        public void FillTree()
        {

            //I know this is just stupid to do it this way, but I couldnt be bother as long as it works im okay lol
            TreeNode mainNode = new TreeNode();
            mainNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainNode.Text = "General Settings";
            this.skilltree.Nodes.Add(mainNode);
            TreeNode mainPNode = new TreeNode();
            mainPNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainPNode.Text = "Pull Skills"; 
            this.skilltree.Nodes.Add(mainPNode);
            TreeNode mainHNode = new TreeNode();
            mainHNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainHNode.Text = "Combat Heal Skills";
            this.skilltree.Nodes.Add(mainHNode);
            TreeNode mainCNode = new TreeNode();
            mainCNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainCNode.Text = "Chain Skills 1";
            this.skilltree.Nodes.Add(mainCNode);

            skilltree.SelectedNode = mainNode;
            TreeNode GNode = new TreeNode();
            GNode.Text = "Looting";
            GNode.Tag = "Looting";
            GNode.Name = "Looting";
            skilltree.SelectedNode.Nodes.Add(GNode);

            skilltree.SelectedNode = GNode;
            TreeNode SNode = new TreeNode();
            SNode.Name = "Looting";
            SNode.Text = "";
            skilltree.SelectedNode.Nodes.Add(SNode);



            skilltree.SelectedNode = mainNode;
            GNode = new TreeNode();
            GNode.Text = "Sitting";
            GNode.Tag = "Sitting";
            GNode.Name = "Sitting";
            skilltree.SelectedNode.Nodes.Add(GNode);

            skilltree.SelectedNode = GNode;
            SNode = new TreeNode();
            SNode.Name = "Sitting";
            SNode.Text = "";
            skilltree.SelectedNode.Nodes.Add(SNode);

            skilltree.SelectedNode = mainNode;
            GNode = new TreeNode();
            GNode.Text = "Healing (NC)";
            GNode.Tag = "Healing";
            GNode.Name = "Healing";
            skilltree.SelectedNode.Nodes.Add(GNode);

            skilltree.SelectedNode = GNode;
            SNode = new TreeNode();
            SNode.Name = "Healing";
            SNode.Text = "";
            skilltree.SelectedNode.Nodes.Add(SNode);

            skilltree.SelectedNode = mainCNode;

        }

        private void newchain_Click(object sender, EventArgs e)
        {

            TreeNode mainNode = new TreeNode();
            mainNode.Name = "mainNode" + skilltree.Nodes.Count;
            mainNode.Text = "Chain Skills " + (skilltree.Nodes.Count - 2);
            this.skilltree.Nodes.Add(mainNode);
            skilltree.SelectedNode = mainNode;
        }

        private void delnode_Click(object sender, EventArgs e)
        {
            if (skilltree.SelectedNode.Level == 1)
                skilltree.SelectedNode.Remove();

        }

        private void clearnode_Click(object sender, EventArgs e)
        {
            skilltree.Nodes.Clear();

            FillTree();
        }

        private void savenodes_Click(object sender, EventArgs e)
        {

            FileStream file = new FileStream("Skills.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            for (int ctr = 0; ctr < skilltree.Nodes.Count; ctr++)
            {
                string x = skilltree.Nodes[ctr].Text;
                int level = skilltree.Nodes[ctr].Level;
                sw.WriteLine(level + "," + x);
                for (int cnt = 0; cnt < skilltree.Nodes[ctr].Nodes.Count; cnt++)
                {
                    x = skilltree.Nodes[ctr].Nodes[cnt].Text;
                    level = skilltree.Nodes[ctr].Nodes[cnt].Level;
                    string ID = (string)skilltree.Nodes[ctr].Nodes[cnt].Tag;
                    string y = skilltree.Nodes[ctr].Nodes[cnt].Nodes[0].Text;
                    sw.WriteLine(level + "," + ID + "," + x + "," + y);
                }

            }


            sw.Close();


            file.Close();

        }

        private void loadnodes_Click(object sender, EventArgs e)
        {
            if (File.Exists("Skills.txt"))
            {

                FileStream file = new FileStream("Skills.txt", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);


                string line;
                TreeNode parent;
                skilltree.Nodes.Clear();

                while ((line = sr.ReadLine()) != null)
                {
                   
                    string[] parts = line.Split(',');



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
                            skilltree.SelectedNode = skilltree.SelectedNode.Parent;
                        }
                        //Again couldnt be bothered...
                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[2];
                        NewNode.Tag = parts[1];
                        NewNode.Name = "Skill" + skilltree.Nodes.Count + 1; //this is wrong;
                        skilltree.SelectedNode.Nodes.Add(NewNode);
                       
                        skilltree.SelectedNode = NewNode;


                        TreeNode NewSCNode = new TreeNode();
                        NewSCNode.Name = "Skill" + skilltree.Nodes.Count + 1; //this is wrong;
                        NewSCNode.Text = parts[3];
                       
                        skilltree.SelectedNode.Nodes.Add(NewSCNode);





                    }

                }




                sr.Close();


                file.Close();
            }


        }

        private void delskill_Click(object sender, EventArgs e)
        {
            //i dont know what im doing here..
            for (int ctr = 0; ctr < skilllist.SelectedItems.Count; ctr++)
            {
                if (skilltree.SelectedNode == null) skilltree.SelectedNode = skilltree.Nodes[0];
              
                else if (skilltree.SelectedNode.Parent != null)
                {
                   
                    skilltree.SelectedNode = skilltree.SelectedNode.Parent;
                    if (skilltree.SelectedNode.Parent != null) skilltree.SelectedNode = skilltree.SelectedNode.Parent;
                }


                TreeNode NewNode = new TreeNode();
                NewNode.Text = skilllist.SelectedItems[ctr].SubItems[1].Text;
                NewNode.Tag = skilllist.SelectedItems[ctr].SubItems[0].Text;
                NewNode.Name = "Skill" + skilltree.Nodes.Count + 1; //this is wrong;
                skilltree.SelectedNode.Nodes.Add(NewNode);
               
                skilltree.SelectedNode = NewNode;

               
                TreeNode NewSCNode = new TreeNode();
                NewSCNode.Name = "Skill" + skilltree.Nodes.Count + 1; //this is wrong;
                NewSCNode.Text = "1";
               
                skilltree.SelectedNode.Nodes.Add(NewSCNode);

              
            }

        }

        private void BStart_Click(object sender, EventArgs e)
        {

           
            skills = new SkillManager(keyEvents, this);
            stateFactory = new StateFactory(this, keyEvents, skills, this);
            currentState = stateFactory.NewState<Searching>();
            PlayerTimer.Start(); 
           
            
        }

        private void PlayerTimer_Tick(object sender, EventArgs e)
        {
            target.Update();
            player.Update();
            var newState = currentState.Run(player, target);

          
            currentState = newState;

        }

        private void BExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
