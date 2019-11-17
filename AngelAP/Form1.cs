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
using AngelRead;
using MemoryLib;
using System.Reflection;
using Ini;


namespace AngelBot
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }


    public partial class Form1 : Form
    {


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("KProcCheck.dll")]
        static extern int getaionhwd();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int extraInfo);
        [DllImport("user32.dll")]
        static extern short MapVirtualKey(int wCode, int wMapType);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        //static extern RECT GetWindowRect(IntPtr hWnd, RECT lpRect);
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);


        public IntPtr hwndAion;

        bool keypresswindow = false;
        public Player pc = new Player();
        public Target tar = new Target();
        public Buffs BuffList = new Buffs();
        public Offsets aionOffsets;
        public bool running = false;
        public bool characterNeedsSave = false;

        //int tabfind = 2;
        int myselfptr;
        double xpStart;
        int kinahStart;
        int kills;
        //int tarID;
        //int tarLastKillID;
        DateTime xpStartTime;
        DateTime xpCurTime;
        TimeSpan elapsed;
        int resthp;
        int restmana;
        //int healhp;
        //double healdelay;
        //double healcool;
        int pothp;
        int potmp;
        int ignorelevel;
        int ignoretime;
        int oochealper;
        int stuckcounter;
        int shutofftime;
        int deathresttime;
        //vender vars start here
        string venderName;
        public bool usevender = true;
        bool soldcrap = false;
        int curkinah = 0;
        //bool revdeathrun = false;
        EntityList elist = new EntityList();
        public int petptr = 0;
        int justignored = 0;

        string ooctype;
        bool ishealer;
        bool isranged;
        bool isresting = false;
        public bool potready = true;
        //bool healready = true;
        bool istabbing = false;
        bool invcheck = true;
        public bool savelog = false;
        public bool leftres = false;
        bool inreactions = false;
        bool gatherenabled = false;
        public bool optimizer = false;
        string gatherselect = "";
        byte gatherdistance = 18;
        public bool antistuck = true;
        //bool largeangle = false;
        int rangedist;

        //List<string> preattacks = new List<string>();
        //List<KeyValuePair<string, int>> preattacksequence = new List<KeyValuePair<string, int>>();
        //int numpreattacks = 0;

        //List<string> attacks = new List<string>();
        //List<KeyValuePair<string, int>> attacksequence = new List<KeyValuePair<string, int>>();
        //int numattacks = 0;

        List<string> buffs = new List<string>();
        List<string> buffbtns = new List<string>();
        List<DateTime> bufftimes = new List<DateTime>();
        List<TimeSpan> buffdelays = new List<TimeSpan>();
        int numbuffs = 0;
        public bool loopit = false;

        List<string> heals = new List<string>();
        List<string> healbtns = new List<string>();
        List<int> healpercentages = new List<int>();
        List<TimeSpan> healdelays = new List<TimeSpan>();
        List<int> healcasts = new List<int>();
        List<DateTime> healtimes = new List<DateTime>();

        public AbilityList abilities = new AbilityList();
        //TreeView PreAttacklist = new TreeView();
        //TreeView AttackLooplist = new TreeView();
        TreeView Attacklist = new TreeView();
        TreeNode Aparent = new TreeNode();
        TreeNode lastattack = new TreeNode();
        object[,] RControlList = new object[13, 7];

        int numheals = 0;
        int maxtries = 0;
        float dps = 0;
        int dpscounter = 0;
        int lasttarHP = 0;
        int heartbeat = 0;

        int combatcounting = 0;
        int diedcount = 0;

        List<string> ignorelist = new List<string>();
        List<string> gatherignorelist = new List<string>();

        public List<cwaypoint> waypointlist = new List<cwaypoint>();
        public List<cwaypoint> deathpointlist = new List<cwaypoint>();
        public List<cwaypoint> venderpointlist = new List<cwaypoint>();
        public List<string> gatherlist = new List<string>();

        public cwaypoint previouspoint = new cwaypoint();
        public int movecounter = 0;
        public int deathcounter = 0;
        public int vendercounter = 0;
        public int lootdelay = 1500;
        public double prevdistance = 0;
        public int returncounter = 0;
        public cwaypoint awaypoint = new cwaypoint();
        //CultureInfo usa = new CultureInfo("en-US");

        string keyloot;
        string keyrest;
        string keyhppot;
        string keymppot;
        string keytarget;
        string keyself;
        string keyturn;
        string keyautoatk;
        string keyheal;
        string keyooch;
        string keystrafeL;
        string keystrafeR;
        string keyreturn;

        //bool cantattack = false;
        int petid = 0;
        int lootcounter = 0;
        bool attackflag;
        public bool deathrun = false;
        public bool venderrun = false;
        bool autoway;
        bool ismoving = false;
        bool forward = true;
        bool keypressing = false;
        bool killsteal = false;
        bool defenddeath = true;
        public TextWriter tw;
        public uint gatheroffset = 0;

        public Form1()
        {
            try
            {
                Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                InitializeComponent();

                bool progname = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToString().Contains("Angel");
#if (!DEBUG)
                if (progname)
                {
                    System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                    process1.StartInfo.FileName = "AngelSuite.exe";
                    process1.Start();
                    Environment.Exit(1);
                }
#endif

                aionOffsets = new Offsets();
                aionOffsets.Update();
                abilities.ABILITY_OFFSET = aionOffsets.abilityInfoAddress;
                //if (abilities.ABILITY_OFFSET != 0xA76CC4) MessageBox.Show("AbilityList offset issue. " + abilities.ABILITY_OFFSET);
                abilities.HOTBAR_OFFSET = aionOffsets.gamedll + 0xB3C0A0;//aionOffsets.hotkeyInfoAddress;
                pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;
                pc.PLAYER_GUID_OFFSET = aionOffsets.pGUIDInfoAddress;
                pc.ENTITY_OFFSET = aionOffsets.entityInfoAddress;
                pc.PLAYER_INVENTORY_OFFSET = aionOffsets.inventoryInfoAddress;

                tar.TARGETPTR_OFFSET = aionOffsets.targetInfoAddress;
                elist.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
                /*if (aionOffsets.gamedll + 0xAB85A4 != aionOffsets.gamedll + aionOffsets.reactionInfoAddress)
                    MessageBox.Show("Error with reaction address " + aionOffsets.reactionInfoAddress.ToString("X"));
                if(aionOffsets.resInfoAddress != 0xAB8560)
                    MessageBox.Show("Error with res address " + aionOffsets.resInfoAddress.ToString("X"));
                */
                //BuffList.ABILITY_OFFSET = aionOffsets.abilityInfoAddress;
                //BuffList.HOTBAR_OFFSET = aionOffsets.hotkeyInfoAddress;
                gatheroffset = aionOffsets.gatheringInfoAddress;
                //MessageBox.Show(pc.PLAYER_INFOADDRESS_OFFSET.ToString("X"));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error starting Angelbot!! Exiting " + e);
                Environment.Exit(1);
            }
        }

        public void loadnewheals()
        {
            heals.Clear();
            int count = Attacklist.Nodes[3].Nodes.Count;
            if (count > 0) //there are heals
            {
                for (int i = 0; i < count; i++)
                {
                    //Attacklist.Nodes[3].Nodes[i].tag;
                    //txtcasttimemod.Text + "|" + hotkey.Text + "|" + txtcastpercent.Text + "|" + checkBox1.Checked.ToString();
                    heals.Add(Attacklist.Nodes[3].Nodes[i].Text);
                }
            }
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

        public void getattacklist()//Adds attacks to list
        {
            if (File.Exists(pc.Name + ".skills"))
            {
                FileStream file = new FileStream(pc.Name + ".skills", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                List<string> Abilitylist = new List<string>();
                List<string> Abilitylist2 = new List<string>();
                List<string> RemoveList = new List<string>();
                abilities.Update();

                foreach (Ability item in abilities)
                { //IsAlphaNumeric(item.AbilityName) == false &&

                    if ((item.AbilityName.Contains("Toggle") == false) && (item.AbilityName.Contains("Basic") == false) && (item.AbilityName.Contains("Advanced") == false) && (item.AbilityName.Contains("Boost") == false) && (item.AbilityName.Contains("Increase") == false))
                    {
                        Abilitylist.Add(item.AbilityName);
                    }
                    else
                    {
                        RemoveList.Add(item.AbilityName);
                    }
                }
                foreach (string removename in RemoveList)
                {
                    abilities.Remove(removename);
                }
                RemoveList.Clear();
                Abilitylist.Sort(); //group skills together
                Abilitylist.Reverse(); //Newest skills first to filter

                foreach (string thing in Abilitylist)
                {
                    string[] stringarray = thing.Split(' ');
                    string temp = "";
                    int last = stringarray.Length - 1;
                    if (stringarray[last] == "I" || stringarray[last] == "II" || stringarray[last] == "III" || stringarray[last] == "IV" || stringarray[last] == "V" || stringarray[last] == "VI" || stringarray[last] == "VII")
                    {
                        for (int i = 0; i < last; i++)
                        {
                            if (temp != "") temp = temp + " ";
                            temp = temp + stringarray[i];
                        }
                    }
                    else temp = thing;
                    if (ContainsStr(Abilitylist2, temp) == false)
                    {
                        Abilitylist2.Add(temp);
                        if (temp != thing)
                        {
                            abilities[thing].AbilityName = temp;
                            abilities.Rename(thing, temp);
                        }
                    }
                    //else abilities.Remove(thing);
                }

                string line;
                TreeNode parent = new TreeNode();
                TreeNode parent1 = new TreeNode();
                TreeNode parent2 = new TreeNode();
                TreeNode parent3 = new TreeNode();
                TreeNode parent4 = new TreeNode();
                Attacklist.Nodes.Clear();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');

                    if (parts[0] != "0")
                        if (abilities[parts[5]].AbilityName == "Nothing")
                            continue;

                    if (parts[0] == "0")
                    {
                        TreeNode mainNode = new TreeNode();
                        mainNode.Name = "mainNode" + Attacklist.Nodes.Count;
                        mainNode.Text = parts[1];
                        this.Attacklist.Nodes.Add(mainNode);
                        Attacklist.SelectedNode = mainNode;
                        parent = mainNode;
                    }

                    else if (parts[0] == "1")
                    {

                        if (Attacklist.SelectedNode.Level != 0)
                        {
                            Attacklist.SelectedNode = parent;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);

                        NewNode.Name = "Skill" + Attacklist.Nodes.Count + 1;
                        Attacklist.SelectedNode.Nodes.Add(NewNode);
                        Attacklist.SelectedNode = NewNode;
                        parent1 = NewNode;
                    }
                    else if (parts[0] == "2")
                    {

                        if (Attacklist.SelectedNode.Level != 1)
                        {
                            Attacklist.SelectedNode = parent1;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + Attacklist.Nodes.Count + 1;
                        Attacklist.SelectedNode.Nodes.Add(NewNode);
                        Attacklist.SelectedNode = NewNode;
                        parent2 = NewNode;
                    }
                    else if (parts[0] == "3")
                    {

                        if (Attacklist.SelectedNode.Level != 2)
                        {
                            Attacklist.SelectedNode = parent2;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + Attacklist.Nodes.Count + 1;
                        Attacklist.SelectedNode.Nodes.Add(NewNode);
                        Attacklist.SelectedNode = NewNode;
                        parent3 = NewNode;
                    }
                    else if (parts[0] == "4")
                    {

                        if (Attacklist.SelectedNode.Level != 3)
                        {
                            Attacklist.SelectedNode = parent3;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + Attacklist.Nodes.Count + 1;
                        Attacklist.SelectedNode.Nodes.Add(NewNode);
                        Attacklist.SelectedNode = NewNode;
                        parent4 = NewNode;
                    }
                    else if (parts[0] == "5")
                    {

                        if (Attacklist.SelectedNode.Level != 4)
                        {
                            Attacklist.SelectedNode = parent4;
                        }

                        TreeNode NewNode = new TreeNode();
                        NewNode.Text = parts[5];
                        NewNode.Tag = parts[1] + "|" + parts[2] + "|" + parts[3] + "|" + parts[4];
                        abilities[parts[5]].Keypress = parts[2];
                        abilities[parts[5]].CastTimeMod = Convert.ToInt32(parts[1]);
                        abilities[parts[5]].PerChance = Convert.ToBoolean(parts[4]);
                        if (parts[3] != "")
                            abilities[parts[5]].CastPercent = Convert.ToInt32(parts[3]);
                        NewNode.Name = "Skill" + Attacklist.Nodes.Count + 1;
                        Attacklist.SelectedNode.Nodes.Add(NewNode);
                        Attacklist.SelectedNode = NewNode;

                    }
                }
                sr.Close();
                file.Close();
                loadnewheals();
            }
        }

        private void clearAll()
        {
            //pc = new Player();
            //tar = new Target();
            //tar.Clear();
            pc.Updatenamelvl();
            pc.Updateafterkill();
            pc.Update();
            //pc.Update();
            //PauseForMilliSeconds(50);
            playerName.Text = pc.Name;
            xpStart = pc.XP;
            kills = 0;
            //tarID = 0;
            killLabel.Text = "Kills\n" + kills;
            kinahStart = pc.Kinah;
            xpStartTime = DateTime.Now;
            xpCurTime = xpStartTime;
            expHRLabel.Text = "Exp/Hr: 0";
            kinahLabel.Text = "Kinah: " + (pc.Kinah - kinahStart);
            elapsedLabel.Text = "Running\n00:00:00";
            if (pc.MaxXP < 0) playerProg.Maximum = pc.XP + 100000;
            else playerProg.Maximum = pc.MaxXP;
            playerProg.Value = pc.XP;
            diedcount = 0;
            
            getattacklist();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Process.Open();

            }
            catch (Exception g)
            {
                MessageBox.Show("Aion process not found! Exiting " + g);//Doesnt work!

                Environment.Exit(1);
            }
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("aion.bin");

            hwndAion = proc[0].MainWindowHandle;
            //if ((int)hwndAion == 0) hwndAion = (System.IntPtr)getaionhwd(); //if can't find aion.bin
            this.Text = "AB2 Release " + Assembly.GetExecutingAssembly().GetName().Version.MinorRevision.ToString();
            clearAll();
            Keys k = Keys.Z | Keys.Control;
            WindowsShell.RegisterHotKey(this, k);
            pc.Updatenamelvl();
            pc.Update();
            /*if (pc.Level > 30)
            {
                MessageBox.Show("This is a Demo for levels 30 and under. Please donate at http://angelbot.forumbuild.com");
                Process.Close();
                Environment.Exit(0);
            }*/
            tar.Update();
            getattacklist();
            xpStart = pc.XP;
            //getpcptr();
            timer1.Start();
            this.TopMost = true;
            this.Opacity = 1;
            loadsettings();
            Get_Petid();
            //loadnewheals();
            keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
            keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
            tmrheart.Enabled = true;
            this.Text = "AB2 Release " + Assembly.GetExecutingAssembly().GetName().Version.MinorRevision.ToString();
        }


        public void timer1_Tick(object sender, EventArgs e) //200ms refresh
        {
            double xpperhr;
            double pcXP;
            double totalSeconds;
            float pcx = pc.X;
            float pcy = pc.Y;
            float pcz = pc.Z;
            int pcMaxXP;
            //
            //Application.DoEvents();
            try
            {
                pc.Update();
                tar.Update();
            }
            catch (Exception) { stop(); MessageBox.Show("Aion closed or problem reading aion."); }

            elapsed = DateTime.Now - xpStartTime;
            elapsedLabel.Text = "Running\n" + (elapsed.ToString()).Substring(0, 8);
            elapsed = DateTime.Now - xpCurTime;

            if (elapsed.TotalSeconds > .2)
            {

                //label1.Text = "X: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", pcx);
                //label2.Text = "Y: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", pcy);
                //label3.Text = "Z: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", (pcz + 1.1)); //note the 1.1 added
                //lblrot.Text = pc.Rotation.ToString();

                //tarID = tar.ID;

                if (ignorelist.Contains(tar.Name) == true || ignorelist.Contains(tar.ID.ToString()) == true)//ignore button
                {
                    btnuningnore.Enabled = true;
                    btnignore.Enabled = false;

                }
                else // (ignorelist.Contains(tar.Name) )//== false || ignorelist.Contains(tar.ID.ToString()) == false)
                {
                    btnuningnore.Enabled = false;
                    btnignore.Enabled = true;

                }
                if (tar.Name != "")
                {

                    if (tar.Attitude == eAttitude.Hostile) lbltarget.ForeColor = Color.Red;
                    if (tar.Attitude == eAttitude.Friendly) lbltarget.ForeColor = Color.Teal;
                    if (tar.Attitude == eAttitude.Passive) lbltarget.ForeColor = Color.WhiteSmoke;
                    if (tar.Attitude == eAttitude.Utility) lbltarget.ForeColor = Color.Green;
                    lbltarget.Text = tar.Name + "(" + tar.Level + ")";
                }
                else lbltarget.Text = "";
                tarhealth.Maximum = 100;

                if (tar.Health > 100) tarhealth.Value = 100;
                else tarhealth.Value = tar.Health;

                /*if(tar.Name == "")
                {
                     tarhealth.CreateGraphics().DrawString("No Target", new Font("Arial", (float)8.25, FontStyle.Bold), Brushes.WhiteSmoke, new PointF(tarhealth.Width / 2 - 25, tarhealth.Height / 2 - 7));
                }
                else*/
                tarhealth.CreateGraphics().DrawString(tarhealth.Value.ToString() + "%", new Font("Microsoft Sans Serif", Convert.ToSingle(8.25), FontStyle.Bold), Brushes.WhiteSmoke, new PointF(tarhealth.Width / 2 - 15, tarhealth.Height / 2 - 7));
                healthbar.Maximum = pc.MaxHealth;
                if ((int)pc.Health <= healthbar.Maximum) healthbar.Value = (int)pc.Health;
                healthbar.CreateGraphics().DrawString(pc.Health + "/" + pc.MaxHealth + " " + string.Format("{0:F1}", (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) + "%", new Font("Microsoft Sans Serif", Convert.ToSingle(8.25), FontStyle.Regular), Brushes.WhiteSmoke, new PointF(healthbar.Width / 2 - 40, healthbar.Height / 2 - 7));
                manabar.Maximum = pc.MaxMP;
                if ((int)pc.MP <= manabar.Maximum) manabar.Value = (int)pc.MP;
                manabar.CreateGraphics().DrawString(pc.MP + "/" + pc.MaxMP + " " + string.Format("{0:F1}", (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100) + "%", new Font("Microsoft Sans Serif", Convert.ToSingle(8.25), FontStyle.Regular), Brushes.WhiteSmoke, new PointF(manabar.Width / 2 - 40, manabar.Height / 2 - 7));
                displayrange();
                label7.Text = "Health: " + tar.HealthHP + "/" + tar.HealthHPMax;


                if (elapsed.TotalSeconds > 4 && savelog == true) tw.Flush();
          
                if (elapsed.TotalSeconds > 5) //updates screen stats
                {
                    pcXP = pc.XP;
                    if (pcXP < 0) pcXP = 0;
                    pcMaxXP = pc.MaxXP;
                    playerExp.Text = "Exp: " + string.Format("{0:f3}", ((pcXP / pcMaxXP) * 100)) + "%";//string.Format("{0:n0}", pcXP) + "/" + string.Format("{0:n0}", pcMaxXP);
                    playerProg.Maximum = pcMaxXP;
                    playerProg.Minimum = 0;
                    playerProg.Value = (int)pcXP;
                    xpCurTime = DateTime.Now;
                    elapsed = xpCurTime - xpStartTime;
                    totalSeconds = elapsed.TotalSeconds;
                    xpperhr = Math.Round((pcXP - xpStart) * 3600 / totalSeconds / 1000, 2);
                    lblxpgain.Text = "XP: " + (pcXP - xpStart);
                    if (xpperhr < 0) { clearAll(); if (savelog == true) tw.WriteLine(DateTime.Now + " Leveled!"); }
                    expHRLabel.Text = "Exp/Hr: " + string.Format("{0:n2}", xpperhr) + "k";
                    double leveltime = (Convert.ToDouble(pcMaxXP, CultureInfo.InvariantCulture) - Convert.ToDouble(pcXP, CultureInfo.InvariantCulture)) / (Convert.ToDouble(xpperhr, CultureInfo.InvariantCulture) * 1000);
                    lbltimelvl.Text = "Level In: " + string.Format("{0:n2}", leveltime) + "h";
                    kinahLabel.Text = "Kinah " + (pc.Kinah - kinahStart);
                    killLabel.Text = "Kills: " + kills;
                    label6.Text = "Inv: " + pc.GetUsedSpace() + "/" + pc.GetMaxCubes();
                    label9.Text = "Deaths: " + diedcount;
                    if (btnstop.Visible == true)
                    {
                        if (AionFocused() == false)
                            SetForegroundWindow(hwndAion);
                    }
                }
            }
        }

        private void resetAll_Click(object sender, EventArgs e)
        {
            clearAll();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Reset pressed");
            //stopmovement = true;
        }

        private void hotstart()
        {
            if (button1.Visible == true)
                startbtn();
            else stopbtn();
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WindowsShell.WM_HOTKEY)
                hotstart();
        }

        private void Form1_OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
            stop();
            WindowsShell.UnregisterHotKey(this);
            if (savelog == true) tw.WriteLine(DateTime.Now + " Angelbot Exiting");
            tw.Close();
            Process.Close();
            Environment.Exit(0);
        }
        private void frmFormName_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.Close();
            Environment.Exit(0);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Close();
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1 = new AboutBox1();
            ab1.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings f2 = new settings();
            f2.Show();

        }
        public void clearsettings()
        {
            //temp = "";
            resthp = 0;
            restmana = 0;
            //healhp = 0;
            pothp = 0;
            potmp = 0;
            ignorelevel = 0;
            ignoretime = 0;
            oochealper = 0;
            ishealer = false;
            isranged = false;
            rangedist = 0;
            buffs.Clear();
            buffdelays = new List<TimeSpan>();
            bufftimes = new List<DateTime>();
            buffbtns = new List<string>();
            heals.Clear();
            healdelays = new List<TimeSpan>();
            healtimes = new List<DateTime>();
            healbtns = new List<string>();
            keyloot = "";
            keyrest = "";
            keyhppot = "";
            keymppot = "";
            keytarget = "";
            keyself = "";
            keyturn = "";
            keyautoatk = "";
            keyheal = "";
            keyooch = "";
        }

        private string GetSetting(IniFile character, IniFile template, string section, string key)
        {
            if (character.HasKey(section, key))
                return character[section][key];
            else if (template.HasKey(section, key))
            {
                if (!character.HasSection(section))
                    character.Add(section);
                character[section][key] = template[section][key];
                characterNeedsSave = true;
                return template[section][key];
            }
            else
            {
                MessageBox.Show("You have no template.ini file or a corrupted copy! Missing in section: " + section + " key: " + key);
                return "";
            }
        }

        public void loadsettings()
        {
            clearsettings();

            pc.Updatenamelvl();
            PauseForMilliSeconds(300);
            IniFile initemplate = new IniFile(Environment.CurrentDirectory + "\\template.ini");
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + pc.Name + ".ini");
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
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + pc.Name + ".ini");
                ini.Load();
            }
            try
            {
                resthp = Int32.Parse(GetSetting(ini, initemplate, "limits", "RestHP"));
                restmana = Int32.Parse(GetSetting(ini, initemplate, "limits", "RestMana"));
                pothp = Int32.Parse(GetSetting(ini, initemplate, "limits", "PotHP").Trim());
                potmp = Int32.Parse(GetSetting(ini, initemplate, "limits", "PotMP").TrimEnd(' '));
                ignorelevel = Int32.Parse(GetSetting(ini, initemplate, "limits", "IgnoreLevel").TrimEnd(' '));
                ignoretime = Int32.Parse(GetSetting(ini, initemplate, "limits", "IgnoreTime").TrimEnd(' '));
                tmrunstuck.Interval = ignoretime * 1000;
                oochealper = Int32.Parse(GetSetting(ini, initemplate, "limits", "OOCHeal").TrimEnd(' '));
                oochealper = Int32.Parse(GetSetting(ini, initemplate, "limits", "OOCHeal").TrimEnd(' '));
                ooctype = GetSetting(ini, initemplate, "limits", "OOCType");

                shutofftime = Int32.Parse(GetSetting(ini, initemplate, "limits", "Shutoff").TrimEnd(' '));
                if (shutofftime > 0) tmrshutoff.Interval = shutofftime * 60000;
                deathresttime = Int32.Parse(GetSetting(ini, initemplate, "limits", "DeathRest").TrimEnd(' '));

                ishealer = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "Healer").TrimEnd(' '));
                isranged = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "Ranged").TrimEnd(' '));
                rangedist = Int32.Parse(GetSetting(ini, initemplate, "character", "RangeDist").TrimEnd(' '));
                lootdelay = Int32.Parse(GetSetting(ini, initemplate, "character", "Lootdelay").TrimEnd(' '));

                antistuck = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "Antistuck").TrimEnd(' '));
                invcheck = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "FullInv").TrimEnd(' '));
                savelog = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "Logging").TrimEnd(' '));
                leftres = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "LeftRes").TrimEnd(' '));
                killsteal = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "KillSteal").TrimEnd(' '));
                defenddeath = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "DefendDeath").TrimEnd(' '));
                gatherenabled = Convert.ToBoolean(GetSetting(ini, initemplate, "character", "Gathering").TrimEnd(' '));
                gatherselect = GetSetting(ini, initemplate, "character", "GatheringSelect").TrimEnd(' ');
                gatherdistance = Convert.ToByte(GetSetting(ini, initemplate, "character", "GatherDistance").TrimEnd(' '));

                string bufftemp = GetSetting(ini, initemplate, "buffs", "Buffs");
                if (bufftemp != "")
                {
                    if (bufftemp.StartsWith("|")) bufftemp = bufftemp.Substring(1, bufftemp.Length);
                    if (bufftemp.Contains('\0').ToString() == "True")
                    {
                        bufftemp = bufftemp.Substring(0, bufftemp.LastIndexOf('\0') - 0);
                    }
                    string[] listbuff = bufftemp.Split('|');
                    buffs.AddRange(listbuff);
                }

                keyloot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "LootBtn"));
                keyrest = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "RestBtn"));
                keyhppot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Healthpot"));
                keymppot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Manapot"));
                keytarget = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "TargetBtn"));
                keyself = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "SelfTarget"));
                keyturn = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "TurnAround"));
                keyautoatk = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Autoattack"));

                keyooch = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "OOCH"));
                keystrafeL = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "StrafeL"));
                keystrafeR = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "StrafeR"));
                keyreturn = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Return"));

                if (characterNeedsSave)
                {
                    ini.Save();
                }

            }
            catch (Exception e)
            {
                //antistuck = true;
                MessageBox.Show("Problem with your ini file! " + e);
            }
            if (savelog == true)
            {
                tw = new StreamWriter(Environment.CurrentDirectory + "\\" + pc.Name + ".log");
                tw.WriteLine(DateTime.Now + " Loaded ini settings good");
            }
            buffload();
            //healload();
            //loadnewheals();
            //preattackload();
            //attackload();
            getattacklist();
            tmrpot.Stop();
            potready = true;

        }

        /*private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {

                if (Process.Open() == false)
                {

                    MessageBox.Show("Aion process not found! Exiting. Process.open == false");//Doesnt work!
                    Environment.Exit(1);
                }
                pc = new Player();
                tar = new Target();
                pc.Updatenamelvl();
                pc.Updateafterkill();
            }
            catch (Exception e2)
            {
                MessageBox.Show("Problem found on startup! Exiting" + e2);//Doesnt work!
                Environment.Exit(1);
            }

            clearAll();
            pc.Update();
            xpStart = pc.XP;
            this.TopMost = true;
            this.Opacity = 1;
            loadsettings();
            keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
            keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
        }
        */
        public void usehppot()
        {
            try
            {
                if (potready == true)
                {

                    if (((Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) < pothp)
                    {
                        int trycount = 0;
                        int prevhealth = pc.Health;
                        //float currhealth = (Convert.ToSingle(pc.Health) / Convert.ToSingle(pc.MaxHealth)) * 100;

                        while (prevhealth >= pc.Health)
                        {
                            if (trycount == 5) break;
                            lblstatus.Text = "Status: Using HP Pot";
                            PauseForMilliSeconds(600);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Using Pot HP:" + pc.Health + "/" + pc.MaxHealth);
                            keyenumerator(keyhppot);
                            Application.DoEvents();
                            lblstatus.Text = "Status: HP Pot used";
                            PauseForMilliSeconds(300);
                            if (pc.Health == 0) return;
                            if (prevhealth > pc.Health) prevhealth = pc.Health;
                            trycount++;
                        }
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Finished Pot HP:" + pc.Health + "/" + pc.MaxHealth);
                        potready = false;
                        Application.DoEvents();
                        tmrpot.Start();
                    }
                }
            }
            catch (Exception e) { MessageBox.Show("Error HPPot: " + e); }
        }

        public void usemanapot()
        {
            if (potready == true)
            {
                if (((Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100) < potmp)
                {
                    int prevmp = pc.MP;
                    int trycount = 0;
                    while (prevmp >= pc.MP)
                    {
                        if (trycount == 5) break;
                        lblstatus.Text = "Status: Using Mana Pot";
                        PauseForMilliSeconds(600);
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Using Mana Pot MP: " + pc.MP + "/" + pc.MaxMP);
                        keyenumerator(keymppot);
                        lblstatus.Text = "Status: Mana Pot used";
                        PauseForMilliSeconds(300);
                        trycount++;
                    }
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Finished Mana Pot MP: " + pc.MP + "/" + pc.MaxMP);
                    potready = false;
                    tmrpot.Start();
                }
            }
        }

        public void useoocheal()
        {
            double hp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
            double mp = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;

            if (ooctype.Trim() == "Heal")
            {
                if (hp < oochealper && tar.Name == "")
                {
                    lblstatus.Text = "Status: Using OOCHeal";
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Using OOCHeal HP: " + pc.Health);
                    keyenumerator(keyooch);
                    lblstatus.Text = "Status: OOCHeal used";
                    PauseForMilliSeconds(2000);
                    Application.DoEvents();
                    PauseForMilliSeconds(2200);
                    pc.Update();
                }
            }
            if (ooctype.Trim() == "Mana")
            {
                if (mp < oochealper && tar.Name == "")
                {
                    lblstatus.Text = "Status: Using OOCMana";
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Using OOCMana MP: " + pc.MP);
                    keyenumerator(keyooch);
                    lblstatus.Text = "Status: OOCMana used";
                    PauseForMilliSeconds(2000);
                    Application.DoEvents();
                    PauseForMilliSeconds(2200);
                    pc.Update();
                }
            }
        }

        private int getHeal()
        {
            double healthpercentage = Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth) * 100;

            for (int i = 0; i < numheals; i++)
            {
                if ((healtimes[i] <= DateTime.Now) && (healpercentages[i] >= healthpercentage))
                {
                    return i;
                }
            }

            return -1;
        }

        private int getLightestHeal()
        {
            int highestHPPercentage = 100;
            int indexOfLightest = -1;
            for (int i = 0; i < numheals; i++)
            {
                if (healpercentages[i] >= highestHPPercentage)
                {
                    indexOfLightest = i;
                    highestHPPercentage = healpercentages[i];
                }
            }

            return indexOfLightest;
        }

        public void healcast()
        {
            if (pc.Class != eClass.Spiritmaster)
            {
                double healthpercentage = Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth) * 100;
                int prevhealth = pc.Health;
                int healcounter = 0;
                int numtries = 0;
                string key = "";
                int delay = 0;

                foreach (string healname in heals) //get a heal that is ready and meets criteria
                {
                    numtries = 0;
                    key = abilities[healname].Keypress;
                    if (key == "") MessageBox.Show(healname + " has no key bound");
                    delay = abilities[healname].CastTime;
                    delay = delay + Convert.ToInt32(abilities[healname].CastTimeMod);
                    if (delay < 200) delay = 600;
                    if (skillready(healname) == true && abilities[healname].CastPercent >= healthpercentage)
                    {
                        while (numtries < 3 && skillready(healname) == true)
                        {
                            lblstatus.Text = "Status: Trying to Heal";
                            prevhealth = pc.Health;
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Casting " + healname + " #" + healcounter + "Ready: " + skillready(healname) + ">> " + "Key: " + key + " HP: " + pc.Health + "/" + pc.MaxHealth);
                            keyenumerator(key);
                            if (skillready(healname) == false) break;
                            PauseForMilliSeconds(delay);
                            Application.DoEvents();
                            if (prevhealth < pc.Health) break;
                            numtries++;
                            if (numtries >= 3)
                            {
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Failed Casting Heal");
                                listkeypress.Items.Add(healname + " Skipping");
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                //This unhighlights the last line
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                return;
                            }
                            if (skillready(healname) == true)
                            {
                                listkeypress.Items.Add(healname + " Not Fired"); listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                //This unhighlights the last line
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                            }
                        }
                        listkeypress.Items.Add(healname + " Fired");
                        listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                        //This unhighlights the last line
                        listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Successfully Cast Heal # " + healcounter + ">> HP: " + pc.Health + "/" + pc.MaxHealth);
                    }
                }
            }
            else //SPIRITMASTER HEALS
            {
                Entity pet = new Entity(petptr);
                if (pet.Health > 0)
                {
                    double healthpercentage = pet.Health;
                    int prevhealth = pc.Health;
                    int healcounter = 0;
                    int numtries = 0;
                    string key = "";
                    int delay = 0;

                    foreach (string healname in heals) //get a heal that is ready and meets criteria
                    {
                        numtries = 0;
                        key = abilities[healname].Keypress;
                        if (key == "") MessageBox.Show(healname + " has no key bound");
                        delay = abilities[healname].CastTime;
                        delay = delay + Convert.ToInt32(abilities[healname].CastTimeMod);
                        if (delay < 200) delay = 600;
                        if (skillready(healname) == true && abilities[healname].CastPercent >= healthpercentage)
                        {
                            while (numtries < 3 && skillready(healname) == true)
                            {
                                lblstatus.Text = "Status: Trying to Heal Pet";
                                prevhealth = pet.Health;
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Casting " + healname + " #" + healcounter + " Ready: " + skillready(healname) + "Key: " + key + " HP: " + pet.Health);
                                keyenumerator(key);
                                if (skillready(healname) == false) break;
                                PauseForMilliSeconds(delay);
                                Application.DoEvents();
                                if (prevhealth < pet.Health) break;
                                numtries++;
                                if (numtries >= 3)
                                {
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " Failed Casting HealPet");
                                    listkeypress.Items.Add(healname + " Skipping");
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                    //This unhighlights the last line
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                    return;
                                }
                                if (skillready(healname) == true)
                                {
                                    listkeypress.Items.Add(healname + " Not Fired"); listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                    //This unhighlights the last line
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                }
                            }
                            listkeypress.Items.Add(healname + " Fired");
                            listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                            //This unhighlights the last line
                            listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Successfully Cast HealPet # " + healcounter + ">> HP: " + pet.Health);
                        }
                    }
                }
            }
            return;
        }
        /*while ((prevhealth >= pc.Health) && (numtries < 3))
        {
            numtries++;

            // If we were specified a heal to use, use it if available..
            if (index >= 0) healcounter = (healtimes[index] <= DateTime.Now) ? index : -1;
            // Otherwise, try to get a heal to use...
            else healcounter = getHeal();

            if (healcounter == -1)
            {
                // No heal can be used right now!
                if (savelog == true) tw.WriteLine(DateTime.Now + " No heal to cast.");
                return;
            }

            keyenumerator(healbtns[healcounter]);
            PauseForMilliSeconds(350);
            Application.DoEvents();
            PauseForMilliSeconds(50);

            if (savelog == true) tw.WriteLine(DateTime.Now + " Try#" + numtries + " to Cast Heal " + healcounter + " >> HP: " + pc.Health + "/" + pc.MaxHealth);
            if (pc.IsCasting == true)
            {
                PauseForMilliSeconds(200);
            }
            if (prevhealth > pc.Health) prevhealth = pc.Health;
        }*/

        //healtimes[healcounter] = DateTime.Now + healdelays[healcounter];
        //            PauseForMilliSeconds(healcasts[healcounter]);
        //PauseForMilliSeconds(350);


        public void doheal()
        {
            // If we're dead, let's not waste our time.
            if (pc.Health == 0) { return; }

            // Busy, or unable/unwilling to use healing skills.
            if (ishealer == false) return;

            healcast();
        }

        private void displayrange()
        {
            double distance = 0;
            if (tar.Name != "")
            {
                distance = pc.Distance2D(tar);
                lbldistance.Text = "Range: " + string.Format("{0:f2}", distance);
            }
        }
        private bool combatchecks()
        {
            if (tar.IsDead == true)
            {
                if (savelog == true) tw.WriteLine(DateTime.Now + " Attacks, Mob dead");
                return true;
            }
            // FIXME: is this going to cause problems?
            if (tar.Name == "")
            {
                if (savelog == true) tw.WriteLine(DateTime.Now + " Attacks, No target!");
                return true; //exits the attack loop
            }
            // Check if mob already tagged, stop if so.
            try
            {
                Entity targetstar = new Entity(tar.PtrTarget);
                if (targetstar.ID != pc.ID && targetstar.Type == eType.Player && tar.IsDead != true) //if mob doesnt have me targeted
                {
                    lblstatus.Text = "Status: Someone is already on mob";
                    if (ignorelist.Contains(tar.ID.ToString()) == false) ignorelist.Add(tar.ID.ToString());
                    return true;
                }
            }
            catch (Exception) { MessageBox.Show("Error: issue with someone on mob in attack"); }

            // Do any queued events.
            Application.DoEvents();

            // Check if mob is dead, stop if so.
            if (tar.IsDead == true || btnstop.Visible == false) //tar.HasTarget == false ||
            {
                lblstatus.Text = "Status: Exiting attack seq..";
                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack seq..");
                return true;
            }

            // If target is attacking us, UNignore it!
            if (ignorelist.Contains(tar.ID.ToString()) == true && tar.TargetID == pc.ID)
            {
                if (savelog == true) tw.WriteLine(DateTime.Now + " Unignoring in attack: " + tar.ID.ToString());
                ignorelist.Remove(tar.ID.ToString());
            }

            if (pc.Health == 0) { return true; }
            if (pc.Health != 0) usehppot();
            if (pc.Health != 0) usemanapot();

            if (pc.Health != 0) doheal();
            return false; //if false, then keep fighting
        }

        private void mainattackloop()//NEW ATTACKLOOP
        {
            maxtries = 0;
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);

            if (pc.TargetID != pc.ID || killsteal == true) //&& (targetstar1.ID == pc.ID || targetstar1.Type == eType.FriendlyNPC || tar.HasTarget != true)) //selftarget
            {
                //tmrstuck.Start(); //starts antistuck timer
                tmrtabby.Stop();
                Attacklist.SelectedNode = Attacklist.Nodes[1].FirstNode; //Attacks level
                Aparent = Attacklist.SelectedNode;
                //while ((tar.IsDead != true || tar.Name != "") && btnstop.Visible == true)

                lblstatus.Text = "Status: Begin Attacking..";

                if (findpcstance() == eStance.Resting)
                {
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
                    PauseForMilliSeconds(1200);
                }
                inreactions = false;

                if (antistuck == true)
                {
                    stuckcounter = 0;
                    returncounter = 0;
                    tmrstuck.Start();
                }

                while (tar.Stance != eStance.Dead && pc.Health != 0 && btnstop.Visible == true) //Top level 
                {
                    if (combatchecks() == true) return;
                    string skillname = Attacklist.SelectedNode.Text;
                    string[] info = Convert.ToString(Attacklist.SelectedNode.Tag).Split('|');

                    string key = info[1];
                    if (key == "") MessageBox.Show(skillname + " has no key bound");
                    int delay = abilities[Attacklist.SelectedNode.Text].CastTime;
                    delay = delay + Convert.ToInt32(info[0]);
                    if (delay < 200)
                    {
                        delay = 600; MessageBox.Show("Skill detected with < 200ms! Fix skills!");
                    }
                    skillready(skillname);

                    // Combatchecks to stop fighting and misc


                    // Do any queued events.
                    Application.DoEvents();

                    if (tar.TargetID == pc.ID || tar.TargetID == petid) //doesnt have me targetted
                    {
                        tmrstuck.Stop();
                        stuckcounter = 0;
                        returncounter = 0;
                    }


                    if (tar.TargetID == pc.ID) tmrstuck.Stop(); //if mob is on me, no antistuck

                    if (tar.IsDead == true || tar.Type == eType.DeadwLoot)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack");
                        lblstatus.Text = "Status: Exiting attack";
                        return;
                    }

                    if (tar.IsDead != true) usehppot();//POTION
                    if (tar.IsDead != true) usemanapot();//POTION
                    bool chainup = true;

                    if (Attacklist.SelectedNode.Level > 1) chainup = chainskillready(skillname); //TODO: check if active on support gui

                    if (skillready(skillname) == true && chainup == true) //if skill is ready(not on cooldown)
                    {
                        if (maxtries >= 3 || (inreactions == true && maxtries >= 2))
                        {
                            if (savelog == true)
                            {
                                tw.WriteLine(DateTime.Now + " Skipping " + skillname);
                                listkeypress.Items.Add(skillname + " Skipping");
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                //This unhighlights the last line
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                PauseForMilliSeconds(50); //incase all cooldowns at level 1 are down
                            }
                            notchain();
                            maxtries = 0;

                        } //after 3 tries, try next skill
                        else
                        {
                            lblstatus.Text = "Status: Attacking with " + skillname;
                            if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " key pressed: " + key + " MOBHP: " + tar.Health);
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                            maxtries++;

                            if (skillready(skillname) == false) //skill was fired and is on cooldown
                            {
                                maxtries = 0; //reset counter
                               // listkeypress.Items.Add(skillname + " Fired");
                                if(listkeypress.Items.Count > 0)listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                //This unhighlights the last line
                                if (listkeypress.Items.Count > 0) listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                if (optimizer == true)
                                {
                                    abilities[Attacklist.SelectedNode.Text].CastTime -= 10;
                                    abilities[Attacklist.SelectedNode.Text].CastTimeMod -= 10;
                                    listkeypress.Items.Add(skillname + " Fired: -10ms");
                                    listkeypress.Items.Add(skillname + " Casttime:" + abilities[Attacklist.SelectedNode.Text].CastTime);
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, true);

                                }
                                listkeypress.Items.Add(skillname + " Fired");
                                if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " cooldown detected");
                                achaincheck();
                            }
                            else
                            {

                                if (optimizer == true)
                                {
                                    abilities[Attacklist.SelectedNode.Text].CastTime += 25;
                                    abilities[Attacklist.SelectedNode.Text].CastTimeMod += 25;
                                    listkeypress.Items.Add(skillname + " Didn't Fire: added 25ms"); 
                                    listkeypress.Items.Add(skillname + " Casttime:" + abilities[Attacklist.SelectedNode.Text].CastTime);
                                    if (listkeypress.Items.Count > 0) listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                }
                                else { 
                                    listkeypress.Items.Add(skillname + " Didn't Fire");
                                    if (listkeypress.Items.Count > 0) listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                }
                                if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " didn't fire");
                                //This unhighlights the last line
                                listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                            }
                        }
                    }
                    else //next one on list (not chain)
                    {
                        if (savelog == true)
                        {
                            tw.WriteLine(DateTime.Now + " Skipping " + skillname);
                            PauseForMilliSeconds(100); //incase all cooldowns at level 1 are down
                        }
                        notchain();
                    }
                    if (inreactions == false)//if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.Parent.Text == "Attacks") 
                        reactionready(); //If there is a reaction up, switch to reaction tree
                    if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.Parent.Text == "Reactions")
                        reactionisup(skillname);//checks to see if its available anymore
                }
                tmrstuck.Stop();
                if (btnstop.Visible == false) return;

                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack loop");
                lblstatus.Text = "Status: Exiting attack..";
            }
        }

        private bool chainskillready(string skillname) //this function checks if a % chance chain is up
        {
            if (abilities[skillname].PerChance == false) 
                return true;
            UpdateReactions();
            
            for (int r = 6; r >= 1; r--)
            {
                if(Convert.ToInt32(RControlList[r, 4]) > 1 )
                    if (Convert.ToInt32(RControlList[r, 4]) == abilities[skillname].AbilityID)
                    return true;
            }
           
            return false;
        }

        private bool reactionisup(string skillname)//TODO: Add timer to keep track of reaction time up
        {
            UpdateReactions();
            for (int r = 12; r >= 6; r--) //reaction range
            {
                if (Convert.ToInt32(RControlList[r, 4]) != 0)
                {
                    if (abilities[skillname].AbilityID == Convert.ToInt32(RControlList[r, 4]) && abilities[skillname].Ready == true)
                    {
                        inreactions = true;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool reactionready()
        {
            UpdateReactions();
            //TODO, only do this if in attacks
            //lastattack = Attacklist.SelectedNode;//reserves place in attack list
            for (int r = 12; r >= 6; r--) //reaction range
            {
                if (Convert.ToInt32(RControlList[r, 4]) != 0)//reaction is up
                {

                    for (int p = 0; p < Attacklist.Nodes[2].Nodes.Count; p++)//loops through the reaction list to match
                    {

                        if (abilities[Attacklist.Nodes[2].Nodes[p].Text].AbilityID == Convert.ToInt32(RControlList[r, 4]) && abilities[Attacklist.Nodes[2].Nodes[p].Text].Ready == true)
                        { //if the selected reaction is up
                            Attacklist.SelectedNode = Attacklist.Nodes[2].Nodes[p]; //match done, select this chain sequence
                            inreactions = true;
                            //maxtries++;
                            return true;
                        }
                    }
                }
            }
            return false;//Attacklist.SelectedNode = Attacklist.Nodes[1].FirstNode; //returns tree back to attacks
        }

        private void UpdateReactions()
        {
            for (int b = 0; b < 13; b++)
            {
                RControlList[b, 0] = 0; //parent
                RControlList[b, 1] = 0; //address
                RControlList[b, 2] = 0; //name of slot
                RControlList[b, 3] = 0; //ready
                RControlList[b, 4] = 0; //skill number
            }

            RControlList = AddControl(Memory.ReadInt(Process.handle, (uint)((Process.Modules.Game + aionOffsets.reactionInfoAddress))), -1, RControlList); //A657D0  


            int i = 0;

            if (Convert.ToInt32(RControlList[i, 1]) != 0)
            {
                int baseoffset = Convert.ToInt32(RControlList[i, 1]);
                int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                for (int j = 1; j <= childsize - 1; j++)
                {
                    thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                    int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                    RControlList = AddControl(final, i, RControlList);
                }
            }
        }

        private object[,] AddControl(int address, int parent, object[,] ControlList)
        {
            int newcontrolname = address;
            //int newcontrolname = (int)Memory.ReadUInt(Process.handle, (uint)(address));
            int controlname = (int)Memory.ReadSByte(Process.handle, (uint)(newcontrolname + 0x1C));
            string scontrolname = "";
            byte controlname2 = 0;
            int scontrolname1 = 0;
            double posx = 0, posy = 0;
            posx = Memory.ReadDouble(Process.handle, (uint)(newcontrolname + 0x40));
            posy = Memory.ReadDouble(Process.handle, (uint)(newcontrolname + 0x48));
            if (controlname > 15) //|| controlname < 0) && controlname != -1)
            {
                controlname = (int)Memory.ReadUInt(Process.handle, (uint)(address));
                int controlname1 = (int)Memory.ReadUInt(Process.handle, (uint)(address + 0xc));

                controlname2 = Memory.ReadByte(Process.handle, (uint)(address + 0x24));
                scontrolname = Memory.ReadString(Process.handle, (uint)(controlname1 + 0x0), 32, false);//
            }
            else if (controlname > 0)
            {
                scontrolname = Memory.ReadString(Process.handle, (uint)(newcontrolname + 0xc), 32, false);
                controlname2 = Memory.ReadByte(Process.handle, (uint)(newcontrolname + 0x24));
                scontrolname1 = (int)Memory.ReadUInt(Process.handle, ((uint)(newcontrolname + 0x2C8)));
            }
            else
            {
                scontrolname = Memory.ReadString(Process.handle, (uint)(newcontrolname + 0x25c), 32, true);
                controlname2 = Memory.ReadByte(Process.handle, (uint)(newcontrolname + 0x24));
                scontrolname1 = (int)Memory.ReadUInt(Process.handle, ((uint)(newcontrolname + 0x2C8 )));
                if (scontrolname.Length < 3 || scontrolname.Length > 20)
                {
                    scontrolname = Memory.ReadString(Process.handle, (uint)(newcontrolname + 0x27c), 32, false);
                    if (scontrolname.Length < 3 || scontrolname.Length > 20)
                    {
                        scontrolname = Memory.ReadString(Process.handle, (uint)(newcontrolname + 0x26c), 32, false);
                        if (scontrolname.Length < 3 || scontrolname.Length > 20)
                        {
                            scontrolname = "xxxxxxxxxxxxxx";
                        }
                    }
                }
                /*If StringLen($controlName) < 3 or StringLen($controlName) > 20 Then
                    controlName = _MemoryRead(Dec(Hex($address + 0x27C)), $openmem, "char[32]")
                    If StringLen(controlName) < 3 or StringLen($controlName) > 20 Then
                        controlName = _MemoryRead(Dec(Hex($address + 0x26C)), $openmem, "char[32]")
                        If StringLen($controlName) < 3 or StringLen($controlName) > 20 Then
                            controlName = "xxxxxxxxxxxxxx";*/
            }
            //object[,] ControlList = new object[12, 6];

            for (int i = 0; i < ControlList.GetLength(0); i++)
            {
                if (Convert.ToInt32(ControlList[i, 1]) == 0)
                {
                    ControlList[i, 0] = parent;
                    ControlList[i, 1] = address; //address
                    ControlList[i, 2] = scontrolname; //chainname
                    if (scontrolname == "ok")
                        scontrolname = "ok";
                    if (controlname2 == 7) ControlList[i, 3] = true; //skill is up
                    else ControlList[i, 3] = false;
                    //ControlList[i,3] = controlname2; //ready
                    ControlList[i, 4] = scontrolname1; //skill number
                    ControlList[i, 5] = posx;
                    ControlList[i, 6] = posy;
                    break;
                }
            }
            return ControlList;
        }
        private bool notchain()
        {
            //Check if it has chain, if not check next skill. If no next skill go to parent and try next skill.

            if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode == null) //no child, next node on same level
            {//If level one and nothing next, go back to first skill
                if (Attacklist.Nodes[2].Nodes.Count > 0) //BUGFIX: missing reference
                {
                    if (Attacklist.SelectedNode.Text != Attacklist.Nodes[2].LastNode.Text)
                    {
                        Attacklist.SelectedNode = Aparent;
                        return true;
                    } //Sets it back to first attack
                    else
                    {
                        Attacklist.SelectedNode = Aparent;//lastattack;
                        inreactions = false;
                        return true;
                    }
                }
                else
                {
                    Attacklist.SelectedNode = Aparent;//lastattack;
                    inreactions = false;
                    return true;
                }

            }
            else if (Attacklist.SelectedNode.NextNode != null) //no child, next node on same level
            {
                //Need check to see if we are at the end of the list, and to go back to top
                Attacklist.SelectedNode = Attacklist.SelectedNode.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 5 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 5 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
                return true;
            }
            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 4 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 4 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
                return true;
            }
            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 3 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 3 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
                return true;
            }
            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode != null) //end of chain
            { //end of lvl 2 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode == null) //end of chain
            { //end of lvl 2 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
                return true;
            }
            return false;
        }
        /*private void notchain()
        {
            //Check if it has chain, if not check next skill. If no next skill go to parent and try next skill.

            if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode == null) //no child, next node on same level
            {//If level one and nothing next, go back to first skill
                if (Attacklist.Nodes[2].Nodes.Count > 0) //BUGFIX: missing reference
                {
                    if (Attacklist.SelectedNode.Text != Attacklist.Nodes[2].LastNode.Text) { Attacklist.SelectedNode = Aparent; } //Sets it back to first attack
                    else
                    {
                        Attacklist.SelectedNode = Aparent;//lastattack;
                        inreactions = false;
                    }
                }
                else
                {
                    Attacklist.SelectedNode = Aparent;//lastattack;
                    inreactions = false;
                }

            }
            else if (Attacklist.SelectedNode.NextNode != null) //no child, next node on same level
            {
                //Need check to see if we are at the end of the list, and to go back to top
                Attacklist.SelectedNode = Attacklist.SelectedNode.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 5 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 5 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
            }
            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 4 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 4 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
            }
            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode != null) //end of chain
            { //end of lvl 3 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode == null) //end of chain
            { //end of lvl 3 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
            }
            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode != null) //end of chain
            { //end of lvl 2 chain, return to level 1 and next skill
                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.NextNode;
            }
            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode == null) //end of chain
            { //end of lvl 2 chain, no next skill, return to first skill
                Attacklist.SelectedNode = Aparent;
                inreactions = false;
            }
        }
        */

        private bool skillready(string skill)
        {
            abilities[skill].Update();
            if (abilities[skill].Ready == true) return true;
            else return false;
        }

        private void achaincheck()
        {
            if (Attacklist.SelectedNode.Nodes.Count != 0) //has child
            { //check for chain
                Attacklist.SelectedNode = Attacklist.SelectedNode.FirstNode;//goes to child for chain
            }
            else //NO CHILD!
            {
                if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode == null) //no child, next node on same level
                {//If level one and nothing next, go back to first skill

                    if (Attacklist.Nodes[2].Nodes.Count > 0) //BUGFIX: missing reference
                    {
                        if (Attacklist.SelectedNode.Text != Attacklist.Nodes[2].LastNode.Text)
                        {
                            Attacklist.SelectedNode = Aparent;
                        } //Sets it back to first attack
                        else
                        {
                            Attacklist.SelectedNode = Aparent;//lastattack;
                            //Attacklist.SelectedNode.Level > 
                            inreactions = false;
                        }
                    }
                    else
                    {
                        Attacklist.SelectedNode = Aparent;//lastattack;
                        //Attacklist.SelectedNode.Level > 
                        inreactions = false;
                    }
                }
                else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode != null) //end of chain
                { //end of lvl 5 chain, return to level 1 and next skill
                    Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode;
                }
                else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode == null) //end of chain
                { //end of lvl 5 chain, no next skill, return to first skill
                    Attacklist.SelectedNode = Aparent;
                    inreactions = false;
                }
                else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode != null) //end of chain
                { //end of lvl 4 chain, return to level 1 and next skill
                    Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.NextNode;
                }
                else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode == null) //end of chain
                { //end of lvl 4 chain, no next skill, return to first skill
                    Attacklist.SelectedNode = Aparent;
                    inreactions = false;
                }
                else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode != null) //end of chain
                { //end of lvl 3 chain, return to level 1 and next skill
                    Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.NextNode;
                }
                else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode == null) //end of chain
                { //end of lvl 3 chain, no next skill, return to first skill
                    Attacklist.SelectedNode = Aparent;
                    inreactions = false;
                }
                else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode != null) //end of chain
                { //end of lvl 2 chain, return to level 1 and next skill
                    Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.NextNode;
                }
                else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode == null) //end of chain
                { //end of lvl 2 chain, no next skill, return to first skill
                    Attacklist.SelectedNode = Aparent;
                    inreactions = false;
                }
                else if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode != null) //end of chain
                { //end of lvl 2 chain, no next skill, return to first skill
                    Attacklist.SelectedNode = Attacklist.SelectedNode.NextNode;
                    inreactions = false;
                }
            }

        }

        private bool preattackcheck()
        {
            if (Attacklist.SelectedNode.NextNode == null && tar.TargetID == pc.ID)
            {
                //MessageBox.Show("End of preattacks");
                return true;
            }
            return false;
        }

        private void Get_Petid()
        {
            EntityList elist1 = new EntityList();
            elist1.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
            elist1.Update();
            //lblstatus.Text = "Status: Getting Pet Entity";
            Application.DoEvents();
            foreach (Entity thing in elist1)
            {

                if (thing.PetOwner == pc.Name)
                {
                    petid = thing.ID;
                    petptr = thing.PtrEntity;
                }
            }
            //lblstatus.Text = "Status: Got Pet entity";
            if (savelog == true) tw.WriteLine(DateTime.Now + " Got pet entity");

            Application.DoEvents();
            PauseForMilliSeconds(100);
        }

        private void attackmob()
        {
            //Entity targetstar = new Entity(tar.PtrTarget);
            tmrtabby.Stop();
            //bool started = false;

            if ((tar.Type == eType.AttackableNPC || tar.Type == eType.Gatherable || tar.Type == eType.GatherableL) && (tar.Health == 100))//can I fight it? && tar.HasTarget != true
            {
                try
                {
                    if (killsteal == false)
                    {
                        if ((tar.TargetID != pc.ID || tar.TargetID != petid || tar.TargetID != tar.ID) && tar.Type != eType.AttackableNPC && tar.Health > 0) //if mob doesnt have me targeted
                        {
                            lblstatus.Text = "Status: Someone is already on mob";
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Not killstealing: " + tar.Name);
                            ignorelist.Add(tar.ID.ToString());
                            return;
                        }
                    }
                }
                catch (Exception woop) { MessageBox.Show("Error: someone is on mob in preattack. " + woop); }

                if (ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
                {
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Went into preattacks but mob is on ingnore list. " + tar.Name);
                    return;
                }
                if (isranged == true) getinrange();
                lblstatus.Text = "Status: Pre-Attacking..";
                //start preattacks
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);


                //LOOP this until mob targets me
                if (Attacklist.Nodes[0].Nodes.Count != 0)
                {
                    maxtries = 0;
                    Attacklist.SelectedNode = Attacklist.Nodes[0].FirstNode; //Attacks level
                    Aparent = Attacklist.SelectedNode;
                    inreactions = false;
                    if (antistuck == true)
                    {
                        stuckcounter = -1;
                        returncounter = -1;
                        tmrstuck.Start();
                    }

                    if (petid == 0) petid = 9999999;
                    while (tar.Stance != eStance.Dead && pc.Health != 0 && btnstop.Visible == true && ((tar.TargetID != pc.ID || tar.TargetID != petid) || (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode != null))) //Top level
                    {
                        if (combatchecks() == true) return;
                        string skillname = Attacklist.SelectedNode.Text;
                        string[] info = Convert.ToString(Attacklist.SelectedNode.Tag).Split('|');

                        string key = info[1];
                        if (key == "") MessageBox.Show(skillname + " has no key bound");
                        int delay = abilities[Attacklist.SelectedNode.Text].CastTime;
                        delay = delay + Convert.ToInt32(info[0]);
                        if (delay < 200) delay = 300;
                        skillready(skillname);

                        // Combatchecks to stop fighting and misc
                        if (combatchecks() == true) return;

                        // Do any queued events.
                        Application.DoEvents();

                        if (tar.TargetID == pc.ID || tar.TargetID == petid) //doesnt have me targetted
                        {
                            stuckcounter = 0;
                            returncounter = 0;
                            tmrstuck.Stop();
                        }


                        //if (tar.TargetID == pc.ID) tmrstuck.Stop(); //if mob is on me, no antistuck

                        if (tar.IsDead == true || tar.Type == eType.DeadwLoot)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting preattacks");
                            lblstatus.Text = "Status: Exiting preattacks";
                            return;
                        }

                        if (tar.IsDead != true) usehppot();//POTION
                        if (tar.IsDead != true) usemanapot();//POTION
                        bool chainup = true;

                        if (Attacklist.SelectedNode.Level > 1) chainup = chainskillready(skillname); //TODO: check if active on support gui
                        if (Attacklist.SelectedNode.Parent.Text == "Reactions" && reactionisup(skillname) == false)
                        {
                            achaincheck();
                            continue;
                        }

                        if (skillready(skillname) == true && chainup == true) //if skill is ready(not on cooldown)
                        {
                            if (maxtries >= 3 || (inreactions == true && maxtries >= 2))
                            {
                                if (savelog == true)
                                {
                                    tw.WriteLine(DateTime.Now + " Skipping " + skillname);
                                    listkeypress.Items.Add(skillname + " Skipping");
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                    //This unhighlights the last line
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                    PauseForMilliSeconds(50); //incase all cooldowns at level 1 are down
                                }
                                if (notchain()) break;
                                maxtries = 0;

                            } //after 3 tries, try next skill
                            else
                            {
                                lblstatus.Text = "Status: PreAttacking with " + skillname;
                                if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " key pressed: " + key + " MOBHP: " + tar.Health);
                                keyenumerator(key);
                                PauseForMilliSeconds(delay);
                                maxtries++;

                                if (skillready(skillname) == false) //skill was fired and is on cooldown
                                {
                                    maxtries = 0; //reset counter
                                    listkeypress.Items.Add(skillname + " Fired");
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                    //This unhighlights the last line
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " cooldown detected");
                                    achaincheck();
                                }
                                else
                                {
                                    listkeypress.Items.Add(skillname + " Didn't Fire"); listkeypress.SetSelected(listkeypress.Items.Count - 1, true);
                                    //This unhighlights the last line
                                    listkeypress.SetSelected(listkeypress.Items.Count - 1, false);
                                }
                            }
                        }
                        else //next one on list (not chain)
                        {
                            if (savelog == true)
                            {
                                tw.WriteLine(DateTime.Now + " Skipping " + skillname);
                                PauseForMilliSeconds(50); //incase all cooldowns at level 1 are down
                            }
                            if (notchain()) break;
                        }
                        if (inreactions == false)//if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.Parent.Text == "Attacks") 
                            reactionready(); //If there is a reaction up, switch to reaction tree
                        if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.Parent.Text == "Reactions")
                            reactionisup(skillname);//checks to see if its available anymore 
                    }
                }
                tmrstuck.Stop();
                lblstatus.Text = "Status: Done Pre-Attacking..";
                if (savelog == true) tw.WriteLine(DateTime.Now + " Preattacks finished");
                //textBox1.Text += "Mainattackloop from pre" + Environment.NewLine;
                mainattackloop();
                kills += 1;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Killed mob. Kills: " + kills);
            }
        }

        /*old private void attackmob()
        {
            Entity targetstar = new Entity(tar.PtrTarget);
            tmrtabby.Stop();
            //bool started = false;

            if ((tar.Type == eType.AttackableNPC || tar.Type == eType.Gatherable) && (tar.Health == 100 || targetstar.Type == eType.FriendlyNPC || killsteal == true))//can I fight it? && tar.HasTarget != true
            {
                try
                {
                    if (targetstar.ID != pc.ID && targetstar.Type == eType.Player && tar.Health > 0) //if mob doesnt have me targeted
                    {
                        lblstatus.Text = "Status: Someone is already on mob";
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Someone is on mob already");
                        ignorelist.Add(tar.ID.ToString());
                        return;
                    }
                }
                catch (Exception) { MessageBox.Show("Error: someone is on mob in preattack"); }

                if (ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
                {
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Went into preattacks but mob is on ingnore list. " + tar.Name);
                    return;
                }
                if (isranged == true) getinrange();
                lblstatus.Text = "Status: Pre-Attacking..";
                //start preattacks
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);


                //LOOP this until mob targets me
                if (Attacklist.Nodes[0].Nodes.Count != 0)
                {
                    maxtries = 0;
                    Attacklist.SelectedNode = Attacklist.Nodes[0].FirstNode; //Attacks level
                    Aparent = Attacklist.SelectedNode;
                    if (antistuck == true)
                    {
                        stuckcounter = 0;
                        returncounter = 0;
                        tmrstuck.Start();
                    }
                    dpstimer.Enabled = true; //DPS starts

                    while (tar.Stance != eStance.Dead && pc.Health != 0 && btnstop.Visible == true && preattackcheck() == false) //Top level IS THIS LOOP REQUIRED?
                    {
                        string skillname = Attacklist.SelectedNode.Text;
                        string[] info = Convert.ToString(Attacklist.SelectedNode.Tag).Split('|');

                        string key = info[1];
                        if (key == "") MessageBox.Show(skillname + " has no key bound");
                        int delay = abilities[Attacklist.SelectedNode.Text].CastTime;
                        delay = delay + Convert.ToInt32(info[0]);
                        if (delay < 200) delay = 600;
                        skillready(skillname);

                        // Combatchecks to stop fighting and misc
                        if (combatchecks() == true) return;

                        // Do any queued events.
                        Application.DoEvents();

                        if (tar.TargetID == pc.ID) tmrstuck.Stop(); //if mob is on me, no antistuck

                        if (tar.IsDead == true || tar.Type == eType.DeadwLoot)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack");
                            lblstatus.Text = "Status: Exiting attack";
                            return;
                        }

                        if (tar.IsDead != true) usehppot();//POTION

                        if (skillready(skillname) == true) //if skill is ready(not on cooldown)
                        {
                            if (maxtries >= 3) { maxtries = 0; achaincheck(); } //after 3 tries, try next skill
                            lblstatus.Text = "Status: Prettacking with " + skillname;
                            if (savelog == true) tw.WriteLine(DateTime.Now + " (PRE)" + skillname + " key pressed: " + key + " MOBHP: " + tar.Health);
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                            maxtries++;

                            if (skillready(skillname) == false)
                            {
                                maxtries = 0;
                                listkeypress.Items.Add(skillname + " Fired");
                                if (savelog == true) tw.WriteLine(DateTime.Now + " " + skillname + " cooldown detected");
                                tmrstuck.Stop();
                                achaincheck();
                            }
                            else { listkeypress.Items.Add(skillname + " Didn't Fire"); }

                        }
                        else //next one on list (not chain)
                        {
                            //Check if it has chain, if not check next skill. If no next skill go to parent and try next skill.
                            if (savelog == true)
                            {
                                tw.WriteLine(DateTime.Now + " Skipping " + skillname);
                                PauseForMilliSeconds(50); //incase all cooldowns at level 1 are down
                            }
                            if (preattackcheck() == true) break;
                            if (Attacklist.SelectedNode.Level == 1 && Attacklist.SelectedNode.NextNode == null) //no child, next node on same level
                            {//If level one and nothing next, go back to first skill

                                Attacklist.SelectedNode = Aparent; //Sets it back to first attack
                            }
                            else if (Attacklist.SelectedNode.NextNode != null) //no child, next node on same level
                            {
                                //Need check to see if we are at the end of the list, and to go back to top
                                Attacklist.SelectedNode = Attacklist.SelectedNode.NextNode;
                            }
                            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode != null) //end of chain
                            { //end of lvl 5 chain, return to level 1 and next skill
                                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode;
                            }
                            else if (Attacklist.SelectedNode.Level == 5 && Attacklist.SelectedNode.Parent.Parent.Parent.Parent.NextNode == null) //end of chain
                            { //end of lvl 5 chain, no next skill, return to first skill
                                Attacklist.SelectedNode = Aparent;
                            }
                            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode != null) //end of chain
                            { //end of lvl 4 chain, return to level 1 and next skill
                                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.Parent.NextNode;
                            }
                            else if (Attacklist.SelectedNode.Level == 4 && Attacklist.SelectedNode.Parent.Parent.Parent.NextNode == null) //end of chain
                            { //end of lvl 4 chain, no next skill, return to first skill
                                Attacklist.SelectedNode = Aparent;
                            }
                            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode != null) //end of chain
                            { //end of lvl 3 chain, return to level 1 and next skill
                                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.Parent.NextNode;
                            }
                            else if (Attacklist.SelectedNode.Level == 3 && Attacklist.SelectedNode.Parent.Parent.NextNode == null) //end of chain
                            { //end of lvl 3 chain, no next skill, return to first skill
                                Attacklist.SelectedNode = Aparent;
                            }
                            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode != null) //end of chain
                            { //end of lvl 2 chain, return to level 1 and next skill
                                Attacklist.SelectedNode = Attacklist.SelectedNode.Parent.NextNode;
                            }
                            else if (Attacklist.SelectedNode.Level == 2 && Attacklist.SelectedNode.Parent.NextNode == null) //end of chain
                            { //end of lvl 2 chain, no next skill, return to first skill
                                Attacklist.SelectedNode = Aparent;
                            }
                        }
                    }
                }
                tmrstuck.Stop();
                lblstatus.Text = "Status: Done Pre-Attacking..";
                if (savelog == true) tw.WriteLine(DateTime.Now + " Preattacks finished");
                //textBox1.Text += "Mainattackloop from pre" + Environment.NewLine;
                mainattackloop();
                dpstimer.Enabled = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " DPS: " + dps);
                dps = 0;
                dpscounter = 0;
                kills += 1;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Killed mob. Kills: " + kills);
            }
        } 
         * /

        /*private void oldmainattackloop(int sequenceindex)
        {
            // If we've got an index to start with, assume we've already hit this mob or are in range.
            bool started = sequenceindex == 0 ? false : true;

            // If we were at the end, start at the beginning!
            if (sequenceindex > numattacks) sequenceindex = 0;
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
            //Entity targetstar1 = new Entity(tar.PtrTarget);

            if (pc.TargetID != pc.ID) //&& (targetstar1.ID == pc.ID || targetstar1.Type == eType.FriendlyNPC || tar.HasTarget != true)) //selftarget
            {
                while ((tar.IsDead != true || tar.Name != "") && btnstop.Visible == true)
                {
                    lblstatus.Text = "Status: Begin Attacking..";
                    tmrtabby.Stop();
                    if (findpcstance() == eStance.Resting)
                    {
                        keyenumerator("UP");//get up
                        PauseForMilliSeconds(1200);
                    }

                    int counter = 0;
                    foreach (KeyValuePair<string, int> attack in attacksequence)
                    {
                        counter++;

                        // For resumption purposes, allow an "offset" to be used to start
                        // us in any given ability.
                        if (counter < sequenceindex)
                            continue;
                        else if (counter == sequenceindex)
                            // If we've hit the mark, remove the sequence index so next loop starts at
                            // beginning.
                            sequenceindex = 0;

                        string key = attack.Key;
                        int delay = attack.Value;

                        // Combatchecks to stop fighting and misc
                        if (combatchecks() == true) return;

                        // Do any queued events.
                        Application.DoEvents();

                        // Start first attack, and keep trying till it fires.
                        if (started == false)
                        {
                            if (antistuck == true)
                            {
                                stuckcounter = 0;
                                returncounter = 0;
                                tmrstuck.Start();
                            }
                            //Entity targetstar1 = new Entity(tar.PtrTarget);
                            lblstatus.Text = "Status: First attack sequence..";
                            started = true;
                            do
                            {
                                keyenumerator(key);
                                PauseForMilliSeconds(delay);
                                if (pc.Health == 0) { return; }
                                if (savelog == true) tw.WriteLine(DateTime.Now + " First attack tried: " + key);
                                if (ignorelist.Contains(tar.ID.ToString()) == true && tar.TargetID != pc.ID) { tmrstuck.Stop(); return; }
                                if (btnstop.Visible == false) return;
                            } while (tar.HasTarget != true && tar.ID != 0);

                            tmrstuck.Stop();
                        }
                        else //attack seq. continues
                        {
                            stuckcounter = 0;
                            returncounter = 0;
                            tmrstuck.Stop();
                            lblstatus.Text = "Status: Attacking..";
                            if (tar.IsDead == true || tar.Type == eType.DeadwLoot)
                            {
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack");
                                lblstatus.Text = "Status: Exiting attack";
                                return;
                            }
                            if (tar.IsDead != true) usehppot();
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Attack key pressed: " + key + " MOBHP: " + tar.Health);
                        }
                        tmrstuck.Stop();
                        PauseForMilliSeconds(100); //try to fix invalid target
                        if (btnstop.Visible == false) break;
                        tar.Update();
                    }
                }

                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack loop");
                lblstatus.Text = "Status: Exiting attack..";
            }
        }
        */
        private void getinrange()
        {
            lblstatus.Text = "Status: Getting in range..";
            float distancetotar = 0;
            distancetotar = (float)(pc.Distance2D(tar) + 1);
            //awaypoint.Set(tar.X, tar.Y, tar.Z);
            //waymovement(false); //face mob
            //PauseForMilliSeconds(300);
            if (findpcstance() == eStance.Resting)
            {
                keyenumerator(keyrest);//get up
                PauseForMilliSeconds(1650);
            }
            //if (distancetotar >= rangedist) keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
            tmrstuck.Start();
            //make bindable

            while (distancetotar >= rangedist)
            {
                distancetotar = (float)(pc.Distance2D(tar) + 1);
                keyenumerator(keyautoatk);
                PauseForMilliSeconds(500);
                Application.DoEvents();
                if (btnstop.Visible == false) { keyenumerator("DOWN"); break; }
            }
            keyenumerator("DOWN");
            /*while (distancetotar >= rangedist)
            {
                distancetotar = (float)(pc.Distance2D(tar) + 1);
                Application.DoEvents();
                awaypoint.Set(tar.X, tar.Y, tar.Z);
                waymovement(false);
                if (adddetect() == true && tar.TargetID != pc.ID) { PauseForMilliSeconds(80); keyenumerator("ESC"); break; }   //check if someone has you targeted)
                //PauseForMilliSeconds(30);
                if (btnstop.Visible == false) { keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0); return; }
                //keyenumerator("DOWN");
            }
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);//press down
            //stops the bot in range
            */
            //tmrstuck.Stop();

        }

        /*private void oldattackmob()
        {
            Entity targetstar = new Entity(tar.PtrTarget);
            tmrtabby.Stop();
            if (tar.Type == eType.AttackableNPC && (tar.Health == 100 || targetstar.Type == eType.FriendlyNPC))//can I fight it? && tar.HasTarget != true
            {
                try
                {
                    if (targetstar.ID != pc.ID && targetstar.Type == eType.Player && tar.Health > 0) //if mob doesnt have me targeted
                    {
                        lblstatus.Text = "Status: Someone is already on mob";
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Someone is on mob already");
                        ignorelist.Add(tar.ID.ToString());
                        return;
                    }
                }
                catch (Exception) { MessageBox.Show("Error: someone is on mob in preattack"); }

                if (ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
                {
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Went into preattacks but mob is on ingnore list. " + tar.Name);
                    return;
                }
                if (isranged == true) getinrange();
                lblstatus.Text = "Status: Pre-Attacking..";
                //start preattacks
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);

                //LOOP this until mob targets me
                if (preattacks.Count != 0)
                {
                    if (antistuck == true)
                    {
                        stuckcounter = 0;
                        returncounter = 0;
                        tmrstuck.Start();
                    }
                    while ((tar.HasTarget != true || tar.TargetID == tar.ID) && tar.IsDead != true)
                    {
                        if (findpcstance() == eStance.Resting)
                        {
                            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                            keyenumerator("UP");//get up
                            PauseForMilliSeconds(1200);
                        }

                        foreach (string item in preattacks)
                        {
                            //if ((tar.TargetID != pc.ID || tar.TargetID != tar.ID) && targetstar.Type == eType.Player) return;
                            if (ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
                            {
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Went into preattacks but mob is on ingnore list. " + tar.Name);
                                return;
                            }
                            if (pc.Health != 0) usehppot();
                            else return;

                            string[] parseitem = item.Split(':');
                            string key;
                            double delayholder;
                            int delay = 0;
                            if (btnstop.Visible == false || tar.Name == "") return;
                            key = parseitem[0];
                            try
                            {
                                if (Double.Parse(parseitem[1], CultureInfo.InvariantCulture) > .1)
                                {
                                    delayholder = Double.Parse(parseitem[1], CultureInfo.InvariantCulture) * 1000;
                                    delay = Convert.ToInt32(delayholder);
                                }
                                else
                                {
                                    delay = 100;
                                }  //no delay
                            }
                            catch (Exception e) { MessageBox.Show("Error in converting attack delay. " + e); }
                            if (ignorelist.Contains(tar.ID.ToString()) == true) { tmrstuck.Stop(); return; }
                            if (tar.IsDead == true) { return; }
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Preattacking with: " + key + " MOBHP: " + tar.Health);

                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                        }
                        tar.UpdateID(); //fix doing preattacks twice

                    }
                    tmrstuck.Stop();
                }
                lblstatus.Text = "Status: Done Pre-Attacking..";
                if (savelog == true) tw.WriteLine(DateTime.Now + " Preattacks finished");
                //textBox1.Text += "Mainattackloop from pre" + Environment.NewLine;
                mainattackloop();
                kills += 1;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Killed mob. Kills: " + kills);
            }
        }
        */
        private void buffload()
        {
            int counter = 0;
            buffdelays.Clear();
            bufftimes.Clear();
            buffbtns.Clear();
            
            foreach (string buff in buffs)
            {
                string[] parseitem = buff.Split(':');
                string btn;
                double delayholder;
                int delay = 0;
                btn = parseitem[0];
                try
                {
                    if (Double.Parse(parseitem[1], CultureInfo.InvariantCulture) > .1)
                    {
                        delayholder = Double.Parse(parseitem[1], CultureInfo.InvariantCulture) * 60000; //MIN
                        delay = Convert.ToInt32(delayholder);
                    }
                    else
                    {
                        delay = 100;
                    }  //no delay
                }
                catch (Exception e) { MessageBox.Show("Error with converting delay in buffload." + e); }
                buffdelays.Add(new TimeSpan(0, 0, 0, 0, delay + 1000));
                bufftimes.Add(DateTime.Now);
                buffbtns.Add(btn.Trim());
                if (savelog == true) tw.WriteLine(DateTime.Now + " Buff Loaded: " + counter);
                counter++;
            }
            numbuffs = counter;
        }

        /*private void preattackload()
        {
            int counter = 0;
            preattacksequence.Clear();

            foreach (string preattack in preattacks)
            {
                string[] parseitem = preattack.Split(':');
                string btn;
                double delayholder;
                int delay = 0;
                btn = parseitem[0];
                try
                {
                    if (Double.Parse(parseitem[1], CultureInfo.InvariantCulture) > .1)
                    {
                        delayholder = Double.Parse(parseitem[1], CultureInfo.InvariantCulture) * 1000; //Seconds
                        delay = Convert.ToInt32(delayholder);
                    }
                    else
                    {
                        delay = 100;
                    }  //no delay
                }
                catch (Exception e) { MessageBox.Show("Error with converting delay in preattackload." + e); }
                preattacksequence.Add(new KeyValuePair<string, int>(btn, delay));
                if (savelog == true) tw.WriteLine(DateTime.Now + " Preattack Loaded: " + counter);
                counter++;
            }
            numpreattacks = counter;
        }
        */
        /*private void attackload()
        {
            int counter = 0;
            attacksequence.Clear();
            foreach (string attack in attacks)
            {
                string[] parseitem = attack.Split(':');
                string btn;
                double delayholder;
                int delay = 0;
                btn = parseitem[0];
                try
                {
                    if (Double.Parse(parseitem[1], CultureInfo.InvariantCulture) > .1)
                    {
                        delayholder = Double.Parse(parseitem[1], CultureInfo.InvariantCulture) * 1000; //Seconds
                        delay = Convert.ToInt32(delayholder);
                    }
                    else
                    {
                        delay = 100;
                    }  //no delay
                }
                catch (Exception e) { MessageBox.Show("Error with converting delay in attackload." + e); }
                attacksequence.Add(new KeyValuePair<string, int>(btn, delay));
                if (savelog == true) tw.WriteLine(DateTime.Now + " Attack Loaded: " + counter);
                counter++;
            }
            numattacks = counter;
        }*/

        /*private void healload()
        {
            int counter = 0;
            healdelays.Clear();
            healcasts.Clear();
            healtimes.Clear();
            healpercentages.Clear();
            healbtns.Clear();

            foreach (string heal in heals)
            {
                string[] parseitem = heal.Split(':');
                string btn;
                double delayholder;
                int delay = 0;
                int castingtime = 0;
                btn = parseitem[0];

                // Get Casting Time
                try
                {
                    if (Double.Parse(parseitem[2], CultureInfo.InvariantCulture) > .1)
                    {
                        delayholder = Double.Parse(parseitem[2], CultureInfo.InvariantCulture) * 1000; //Seconds
                        castingtime = Convert.ToInt32(delayholder);
                    }
                    else
                    {
                        castingtime = 500;
                    }  //no delay
                }
                catch (Exception e) { MessageBox.Show("Error with converting casting time in healload." + e); }

                // Get Delay
                try
                {
                    if (Double.Parse(parseitem[3], CultureInfo.InvariantCulture) > .1)
                    {
                        delayholder = Double.Parse(parseitem[3], CultureInfo.InvariantCulture) * 1000; //Seconds
                        delay = Convert.ToInt32(delayholder);
                    }
                    else
                    {
                        delay = 100;
                    }  //no delay
                }
                catch (Exception e) { MessageBox.Show("Error with converting delay in healload." + e); }

                healdelays.Add(new TimeSpan(0, 0, 0, 0, delay + 200));
                healcasts.Add(castingtime + 200);
                healtimes.Add(DateTime.Now);
                healpercentages.Add(Int32.Parse(parseitem[1]));
                healbtns.Add(btn.Trim());
                counter++;
            }
            numheals = counter;
        }
        */

        public string findgatherable()
        {
            elist.Update();

            string closestname = "";
            double closestdistance = 0;

            try
            {
                foreach (Entity ents in elist)
                {
                    if (ents.Type == eType.Player && ents.Distance2D(pc.X, pc.Y) < gatherdistance && ents.Name != pc.Name)
                        return "";
                    if ((gatherlist.Count > 0 && gatherlist.Contains(ents.Name)) || gatherlist.Count == 0)
                    {
                        if ((ents.Type == eType.Gatherable || ents.Type == eType.GatherableL) && ents.Distance2D(pc.X, pc.Y) < gatherdistance)
                        {
                            if ((ents.Distance2D(pc.X, pc.Y) < closestdistance) || closestdistance == 0)
                            {
                                if (gatherignorelist.Contains(ents.ID.ToString()) == false)
                                {
                                    closestname = ents.Name; //add check to see if someone is already targetting it.
                                    justignored = ents.ID;
                                    closestdistance = ents.Distance2D(pc.X, pc.Y);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return closestname;
        }

        public bool adddetect()
        {

            //EntityList elist = new EntityList(aionOffsets.entityInfoAddress);
            //elist.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
            elist.Update();

            foreach (Entity ents in elist)
            {
                if (ents.Type == eType.AttackableNPC && ents.Name != "")
                {
                    if (ents.TargetID == pc.ID && ents.Health > 0 && ents.Distance3D(pc.X, pc.Y, pc.Z) < 40)
                    {

                        lblstatus.Text = "Status: ADD found in ADD detection: " + ents.Name;
                        int trycount = 0;
                        if (tar.ID == ents.ID) return true;
                        if (tar.TargetID != pc.ID && tar.ID != 0 && keypressing == false) keyenumerator("ESC");
                        if (savelog == true) tw.WriteLine(DateTime.Now + " ADD found in ADD detection: " + ents.Name + " Distance: " + pc.Distance3D(ents));
                        tar.UpdateID();

                        while (tar.ID != ents.ID)
                        {
                            if (keypressing == false) keyenumerator(keytarget);
                            if (pc.Health == 0) return false;
                            if (trycount >= 5) { keyenumerator("ESC"); return false; }
                            PauseForMilliSeconds(300);
                            tar.UpdateID();
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Trying to target ADD: " + ents.Name + " ID:" + ents.ID + " MobTarget: " + tar.Name + " ID:" + tar.ID);
                            trycount++;
                        }
                        //mainattackloop(0);
                        return true; //if a hostile has you targeted
                    }
                }
            }
            return false;
        }

        public void getpcptr()
        {
            //EntityList elist = new EntityList(aionOffsets.entityInfoAddress);
            //elist.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
            elist.Update();
            //lblstatus.Text = "Status: Getting Player Entity";
            Application.DoEvents();
            foreach (Entity thing in elist)
            {
                if (thing.Name == pc.Name)
                {
                    if (thing._PtrEntity != 0)
                    {
                        myselfptr = thing._PtrEntity;
                        pc.SelfPtr = thing._PtrEntity;
                    }
                    else myselfptr = thing.PtrEntity;
                }
            }
            //lblstatus.Text = "Status: Got Player Entity";
            if (savelog == true) tw.WriteLine(DateTime.Now + " Got player entity");

            Application.DoEvents();
            PauseForMilliSeconds(200);
        }

        public eStance findpcstance()
        {
            eStance mystance = new eStance();
            try
            {

                mystance = (eStance)Memory.ReadInt(Process.handle, (uint)(myselfptr + 0x258)); //use pc entity
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with pcstance." + e);
            }
            return mystance;
        }

        private void rest()
        {
            if (pc.Health == 0) return;
            double perresthp, perrestmana;
            try
            {
                isresting = false;
                if (adddetect() == true) return; //Found add targeting you
                if (pc.Health < pc.MaxHealth) doheal();
                if ((pc.Health < pc.MaxHealth) || (pc.MP < pc.MaxMP)) useoocheal();
                pc.Update();
                PauseForMilliSeconds(50);
                perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;

                if (pc.Name == tar.Name && keypressing == false) keyenumerator("ESC");


                if ((perresthp <= resthp || perrestmana <= restmana) && tar.Name == "")
                {

                    lblstatus.Text = "Status: Resting..";
                    isresting = true;

                    if (perresthp <= resthp)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " HP Resting");
                        getpcptr();
                        if (pc.Class == eClass.Spiritmaster) Get_Petid();
                        while (perresthp < 99 && (tar.Name == "" || tar.Type == eType.Player)) //HP
                        {
                            if (findpcstance() != eStance.Resting)
                                keyenumerator(keyrest);
                            perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            if (pc.Health == 0) { isresting = false; return; } //you clicked stop button
                            if (perresthp > 99) break;
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            if (perresthp > 99) break;
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            if (perresthp > 99) break;
                            PauseForMilliSeconds(600);

                        }
                    }
                    if (perrestmana <= restmana)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Mana Resting");
                        getpcptr();
                        if (pc.Class == eClass.Spiritmaster) Get_Petid();
                        while (perrestmana < 99 && (tar.Name == "" || tar.Type == eType.Player)) //MANA
                        {
                            if (findpcstance() != eStance.Resting) keyenumerator(keyrest);
                            perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            if (pc.Health == 0) { isresting = false; return; } //you clicked stop button

                            if (perrestmana > 99) break;
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            if (perrestmana > 99) break;
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            if (perrestmana > 99) break;
                            PauseForMilliSeconds(600);
                        }
                    }
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Done resting");

                    lblstatus.Text = "Status: Done Resting..";
                    if (findpcstance() == eStance.Resting)
                    {
                        keyenumerator("UP");//get up
                        PauseForMilliSeconds(1400);
                    }
                    isresting = false;
                }
                isresting = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with rest. " + e);
            }
        }

        public void startbtn()
        {
            PauseForMilliSeconds(100);
            if (tar.Type == eType.AttackableNPC || tar.Type == eType.Gatherable || tar.Type == eType.GatherableL)
            {
                SetForegroundWindow(hwndAion);
                getpcptr();
                if (pc.Class == eClass.Spiritmaster)
                    Get_Petid();
                PauseForMilliSeconds(50);
                button1.Visible = false;
                btnstop.Visible = true;
                if (tar.TargetID == pc.ID || tar.TargetID == 0 || killsteal == false)
                {
                    dpstimer.Enabled = true;
                    dps = 0;
                    dpscounter = 0;
                    if (pc.Class == eClass.Spiritmaster && tar.TargetID == petid)
                    {
                        mainattackloop();
                    }
                    else
                    {
                        attackmob();
                    }
                    dpstimer.Enabled = false;
                }
                else
                {
                    dpstimer.Enabled = true;
                    dps = 0;
                    dpscounter = 0;
                    mainattackloop();
                    if (savelog == true) tw.WriteLine(DateTime.Now + " DPS: " + dps);
                    dpstimer.Enabled = false;
                }
                stopbtn();
            }
            if (waypointlist.Count >= 2)
            {
                attackflag = false;
                btnwaypoint.Enabled = false;
                btnautoway.Enabled = false;
                istabbing = false;
                SetForegroundWindow(hwndAion);
                getpcptr();
                PauseForMilliSeconds(50);
                button1.Visible = false;
                btnstop.Visible = true;
                deathrun = false;
                venderrun = false;
                vendercounter = 0;
                deathcounter = 0;
                istabbing = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Start Pressed");
                potready = true;
                Application.DoEvents();
                running = true;
                //timer2.Start();
                if (btnstop.Visible == true)
                {
                    if (shutofftime > 0) tmrshutoff.Start();
                    combat();
                }
            }
            else lblstatus.Text = "Status: Needs more waypoints";
            //combat();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (pc.Level > 30)
            {
                MessageBox.Show("This is a Demo for levels 30 and under. Please donate at http://angelbot.forumbuild.com");
                Process.Close();
                Environment.Exit(0);
            }*/
            //pc.UpdateAP();
            //MessageBox.Show("AP: " + pc.AP );
            startbtn();
        }

        public void stopbtn()
        {
            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(100);
            if (shutofftime > 0) tmrshutoff.Stop();
            btnwaypoint.Enabled = true;
            btnautoway.Enabled = true;
            tmrstuck.Stop();
            running = false;
            stop();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Stop pressed");

            istabbing = false;
        }

        private void btnstop_Click_1(object sender, EventArgs e)
        {
            stopbtn();
        }

        public void deathsystem()
        {
            if (pc.Health == 0)
            {
                //button1.Visible = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " You died like a noob from: " + tar.Name);
                keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
                keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);

                lblstatus.Text = "Status: Resurrecting";
                resurrect();
                if (savelog == true) tw.WriteLine(DateTime.Now + " Resurrect finished");

                //btndeathstop.Visible = true;
                if (deathpointlist.Count >= 1)
                {
                    getpcptr();
                    if (pc.Class == eClass.Spiritmaster) Get_Petid();
                    lblstatus.Text = "Status: Resting " + deathresttime + " sec";
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Resting for " + deathresttime + " seconds");

                    PauseForMilliSeconds(2000);
                    Application.DoEvents();
                    if (btnstop.Visible == false) { button1.Visible = true; deathrun = false; }


                    int sitcounter = 0;
                    while (sitcounter < deathresttime)
                    {

                        Application.DoEvents();

                        if (findpcstance() == eStance.Normal)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Pressing Rest key.");
                            keyenumerator(keyrest);
                        }
                        if (btnstop.Visible == false) break;
                        PauseForMilliSeconds(1500);
                        Application.DoEvents();
                        PauseForMilliSeconds(1500);
                        sitcounter = sitcounter + 3;
                    }
                    sitcounter = 0;

                    keyenumerator("UP"); //get up
                    if (btnstop.Visible != false) lblstatus.Text = "Status: Buffing";
                    if (btnstop.Visible != false) PauseForMilliSeconds(1200);
                    int buffcount = 0;
                    foreach (string buff in buffs)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Buff reset: " + buffcount);
                        bufftimes[buffcount] = DateTime.Now; //Reset buffs
                        buffcount++;
                    }
                    if (btnstop.Visible == false) { button1.Visible = true; deathrun = false; }
                    if (btnstop.Visible != false) buffcast();
                    deathcounter = 0;
                    deathrun = true;
                    attackflag = false;
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Finished death sequence");

                    combat();

                }
            }
        }
        public void vendersystem()
        {
            if (pc.CubeFull() == true)
            {
                //btndeathstop.Visible = true;
                if ((venderpointlist.Count >= 1) && (deathpointlist.Count >= 1))
                {
                    getpcptr();
                    if (pc.Class == eClass.Spiritmaster) Get_Petid();

                    lblstatus.Text = "Status: Cubes are full, running to vender...";
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Cubes are full, running to vender...");

                    Application.DoEvents();
                    if (btnstop.Visible == false) { button1.Visible = true; venderrun = false; }

                    if (findpcstance() == eStance.Resting) keyenumerator("UP"); //if we are resting (also if ooc)
                    if (btnstop.Visible != false) PauseForMilliSeconds(1200);
                    vendercounter = venderpointlist.Count - 1; //start at the end of list.
                    venderrun = true;
                    deathrun = false;
                    forward = false; //run backwards on the list.
                    attackflag = false;

                    if (savelog == true) tw.WriteLine(DateTime.Now + " Attempting to release to town...");

                    //if (skillready("Return") == true) //used in 2.0
                    release();
                    changewaypoint();
                    combat();
                }
                else { if (savelog == true) tw.WriteLine(DateTime.Now + " Either not enough death or vender waypoints to follow!"); }
            }
        }

        public void stop()
        {
            lblstatus.Text = "Status: Stopped";
            if (savelog == true) tw.WriteLine(DateTime.Now + " Stop function called");

            attackflag = false;
            //timer2.Stop(); //combattimer2.Stop();
            tmrtabby.Stop();
            tmrstuck.Stop();
            tmrpot.Stop();
            //tmrheal.Stop();

            btnstop.Visible = false;
            button1.Visible = true;
        }


        //public static DateTime PauseForMilliSeconds(int MilliSecondsToPauseFor)
        public void PauseForMilliSeconds(int MilliSecondsToPauseFor)
        {
            Application.DoEvents();
            Thread.Sleep(MilliSecondsToPauseFor);
        }

        private void btnignore_Click(object sender, EventArgs e)
        {
            if (tar.Name != "")
            {
                ignorelist.Add(tar.Name);
                if (savelog == true) tw.WriteLine(DateTime.Now + " Ignored: " + tar.Name);

            }
        }
        private void btnuningnore_Click(object sender, EventArgs e)
        {
            if (tar.Name != "")
            {
                ignorelist.Remove(tar.Name);
                if (savelog == true) tw.WriteLine(DateTime.Now + " Unignored: " + tar.Name);

            }
        }
        private void keypressWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (keypresswindow == false)
            {
                this.Width = 710;
                this.showkp.Text = "Hide <-";
                keypresswindow = true;
            }
            else
            {
                this.Width = 541;        //490normal
                this.showkp.Text = "Show ->";
                keypresswindow = false;
            }
        }

        private void loadWaypointsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";
            openFileDialog1.DefaultExt = "nwp";
            openFileDialog1.AddExtension = true;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Waypoint Files (*.nwp)|*.nwp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                this.Text = "Using: " + openFileDialog1.SafeFileName;
                string wtype = "";
                //if (sr.EndOfStream != true) type = sr.ReadLine().Trim(); //eat first line 
                while (sr.EndOfStream != true)
                {
                    string waypointline = sr.ReadLine();
                    if (waypointline.Contains('|'))
                    {
                        string[] coords = waypointline.Split('|');
                        cwaypoint here = new cwaypoint();
                        here.X = Convert.ToSingle(coords[1], CultureInfo.InvariantCulture);
                        here.Y = Convert.ToSingle(coords[0], CultureInfo.InvariantCulture);
                        here.Z = Convert.ToSingle(coords[2], CultureInfo.InvariantCulture);
                        if (wtype == "0")
                        {
                            loopit = false;
                            waypointlist.Add(here);
                        }
                        if (wtype == "1")
                        {
                            loopit = true;
                            waypointlist.Add(here);
                        }
                        if (wtype == "3")
                            deathpointlist.Add(here);
                        if (wtype == "4")
                            venderpointlist.Add(here);

                    }
                    if (IsNumber(waypointline) && waypointline.Length == 1)
                        wtype = waypointline.Trim();
                    if (wtype == "8" && waypointline != "8")
                        gatherlist.Add(waypointline.Trim());
                    if (wtype == "9" && waypointline != "9")
                        ignorelist.Add(waypointline.Trim());

                }
                sr.Close();
                if (savelog == true) tw.WriteLine(DateTime.Now + " Loaded waypoints file: " + openFileDialog1.FileName);

                lblstatus.Text = "Status: Waypoints loaded";
            }

        }
        bool IsNumber(string text)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }

        private void saveWaypointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog DialogSave = new SaveFileDialog();

            // Default file extension
            DialogSave.DefaultExt = "nwp";
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.Filter = "Waypoint Files (*.nwp)|*.nwp";
            DialogSave.Title = "Where do you want to save the file?";

            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                TextWriter sw = new StreamWriter(DialogSave.FileName);

                if (loopit == false)
                {
                    sw.WriteLine("0");
                }
                else { sw.WriteLine("1"); }
                string awaypoint;
                foreach (cwaypoint item in waypointlist)
                {
                    double tempx, tempy, tempz;
                    tempx = Double.Parse(item.X.ToString());
                    tempy = Double.Parse(item.Y.ToString());
                    tempz = Double.Parse(item.Z.ToString());

                    awaypoint = string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempy) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempx) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempz);
                    sw.WriteLine(awaypoint);
                }
                awaypoint = "";
                sw.WriteLine("3");
                foreach (cwaypoint item in deathpointlist)
                {
                    double tempx, tempy, tempz;
                    tempx = Double.Parse(item.X.ToString());
                    tempy = Double.Parse(item.Y.ToString());
                    tempz = Double.Parse(item.Z.ToString());

                    awaypoint = string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempy) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempx) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempz);
                    sw.WriteLine(awaypoint);
                }
                sw.WriteLine("4");
                foreach (cwaypoint item in venderpointlist)
                {
                    double tempx, tempy, tempz;
                    tempx = Double.Parse(item.X.ToString());
                    tempy = Double.Parse(item.Y.ToString());
                    tempz = Double.Parse(item.Z.ToString());

                    awaypoint = string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempy) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempx) + "|" + string.Format(CultureInfo.InvariantCulture, "{0:f3}", tempz);
                    sw.WriteLine(awaypoint);
                }
                if (gatherlist.Count != 0)
                {
                    sw.WriteLine("8"); //IGNORELIST
                    foreach (string gather in gatherlist)
                    {
                        if (IsNumber(gather) != true)
                            sw.WriteLine(gather);
                    }
                }
                if (ignorelist.Count != 0)
                {
                    sw.WriteLine("9"); //IGNORELIST
                    foreach (string mob in ignorelist)
                    {
                        if (IsNumber(mob) != true)
                            sw.WriteLine(mob);
                    }
                }
                sw.Close();
                if (savelog == true) tw.WriteLine(DateTime.Now + " Saved waypointfile: " + DialogSave.FileName);

                lblstatus.Text = "Status: Waypoints/IgnoreList saved";
            }

        }

        private void btnwaypoint_Click(object sender, EventArgs e)
        {
            addwaypoint();
            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(600);

        }

        private void btnautoway_Click(object sender, EventArgs e)
        {
            Map map1 = new Map();
            if (autoway == true)
            {
                autoway = false;
                btnautoway.Text = "Auto Waypoints";
                timer3.Stop();
                button1.Enabled = true;
                map1.timer1.Enabled = false;
            }
            else
            {

                if (autoway == false) addwaypoint(); //first waypoint add
                autoway = true;
                btnautoway.Text = "Stop";
                button1.Enabled = false;
                timer3.Start();

                map1.timer1.Enabled = true;
            }

        }

        public void addwaypoint()
        {
            cwaypoint here = new cwaypoint();
            here.X = pc.X;
            here.Y = pc.Y;
            here.Z = pc.Z;
            if ((checkBox1.Checked == false) && (checkBox2.Checked == false))
            {
                waypointlist.Add(here);
                lblstatus.Text = "Status: Added Waypoint " + waypointlist.Count;
            }
            if ((checkBox1.Checked == true) && (checkBox2.Checked == false)) //EDIT, add deathwaypoint ONLY
            {
                deathpointlist.Add(here);
                lblstatus.Text = "Status: Added Death Waypoint " + deathpointlist.Count;
            }
            if ((checkBox1.Checked == false) && (checkBox2.Checked == true)) //Add venderwaypoint ONLY
            {
                venderpointlist.Add(here);
                lblstatus.Text = "Status: Added Vender Waypoint " + venderpointlist.Count;
            }

        }

        private void timer3_Tick(object sender, EventArgs e) //autowaypoint timer
        {
            //ADD DEATH WAYPOINTS HERE
            addwaypoint();
        }

        private void clearWaypointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            waypointlist.Clear();
            ignorelist.Clear();
            deathpointlist.Clear();
            venderpointlist.Clear();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Cleared waypoints");
            btnwaypoint.Enabled = true;
            btnautoway.Enabled = true;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        public void sellmisc()
        {
            soldcrap = true; //we did (or will have) sold our crap
            this.checkBox2.Checked = false;

            TakeOver aion = new TakeOver((int)hwndAion);
            RECT aionscreen = new RECT();

            PauseForMilliSeconds(300);
            if (savelog == true) tw.WriteLine(DateTime.Now + " Attempting to click to sell ");
            lblstatus.Text = "Status: Attempting to click to sell";

            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(800);
            GetWindowRect(hwndAion, ref aionscreen);

            object[,] nControlList = new object[90, 7];
            for (int b = 0; b < 90; b++)
            {
                nControlList[b, 0] = 0; //parent
                nControlList[b, 1] = 0; //address
                nControlList[b, 2] = 0; //name of slot
                nControlList[b, 3] = 0; //ready
                nControlList[b, 4] = 0; //skill number
                nControlList[b, 5] = 0; //PosX
                nControlList[b, 6] = 0; //PosY
            }

            nControlList = AddControl(Memory.ReadInt(Process.handle, (Process.Modules.Game + aionOffsets.vendorInfoAddress)), -1, nControlList); //set control list initial npc dialog
            int i = 0;
            for (i = 0; i <= 30 - 1; i++)
            {
                if (Convert.ToInt32(nControlList[i, 1]) != 0)
                {
                    //int baseoffset = Memory.ReadInt(Process.handle, (Convert.ToUInt32(nControlList[i, 1])));
                    int baseoffset = Convert.ToInt32(nControlList[i, 1]);
                    int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                    int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                    int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                    for (int j = 0; j <= childsize; j++)
                    {
                        thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                        int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                        nControlList = AddControl(final, i, nControlList);
                    }
                    if (Convert.ToString(nControlList[i, 2]) == "NpcFunc2") break;
                }
            }

            int okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + 65 + Convert.ToInt32(nControlList[i, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
            int okbuttony = Convert.ToInt32(aionscreen.Top + Convert.ToDouble(nControlList[0, 6])) + 15 + 120 + Convert.ToInt32(nControlList[i, 6]); //+ Convert.ToInt32(nControlList[2, 6]));
            aion.MoveMouse(okbuttonx, okbuttony);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);
            aion.MoveMouse(okbuttonx, okbuttony - 40);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);

            //object[,] nControlList = new object[90, 7];
            for (int b = 0; b < 90; b++)
            {
                nControlList[b, 0] = 0; //parent
                nControlList[b, 1] = 0; //address
                nControlList[b, 2] = 0; //name of slot
                nControlList[b, 3] = 0; //ready
                nControlList[b, 4] = 0; //skill number
                nControlList[b, 5] = 0; //PosX
                nControlList[b, 6] = 0; //PosY
            }

            nControlList = AddControl(Memory.ReadInt(Process.handle, (Process.Modules.Game + aionOffsets.sellInfoAddress)), -1, nControlList); //set control list for vender.
            //int i = 0;
            for (i = 0; i <= 30 - 1; i++)
            {
                if (Convert.ToInt32(nControlList[i, 1]) != 0)
                {
                    //int baseoffset = Memory.ReadInt(Process.handle, (Convert.ToUInt32(nControlList[i, 1])));
                    int baseoffset = Convert.ToInt32(nControlList[i, 1]);
                    int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                    int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                    int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                    for (int j = 0; j <= childsize; j++)
                    {
                        thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                        int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                        nControlList = AddControl(final, i, nControlList);
                    }
                    if (Convert.ToString(nControlList[i, 2]) == "sell_junk") break;
                }
            }

            okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + 35 + Convert.ToInt32(nControlList[i, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
            okbuttony = Convert.ToInt32(aionscreen.Top + Convert.ToDouble(nControlList[0, 6])) + 15 + 85 + Convert.ToInt32(nControlList[i, 6]); //+ Convert.ToInt32(nControlList[2, 6]));
            aion.MoveMouse(okbuttonx, okbuttony);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);
            aion.MoveMouse(okbuttonx, okbuttony - 25);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);

            for (i = 0; i <= 30 - 1; i++)
            {
                if (Convert.ToInt32(nControlList[i, 1]) != 0)
                {
                    //int baseoffset = Memory.ReadInt(Process.handle, (Convert.ToUInt32(nControlList[i, 1])));
                    int baseoffset = Convert.ToInt32(nControlList[i, 1]);
                    int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                    int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                    int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                    for (int j = 0; j <= childsize; j++)
                    {
                        thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                        int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                        nControlList = AddControl(final, i, nControlList);
                    }
                    if ((Convert.ToInt32(nControlList[i, 0]) == 6) && (Convert.ToString(nControlList[i, 2]) == "ok")) lblstatus.Text = "Status: break on OK "; break;
                }
            }
            if ((Convert.ToInt32(nControlList[17, 0]) == 6) && (Convert.ToString(nControlList[17, 2]) == "ok")) i = 17;

            okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + 35 + Convert.ToInt32(nControlList[i, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
            okbuttony = Convert.ToInt32(aionscreen.Top + Convert.ToDouble(nControlList[0, 6])) + 15 + 85 + Convert.ToInt32(nControlList[i, 6]); //+ Convert.ToInt32(nControlList[2, 6]));
            aion.MoveMouse(okbuttonx, okbuttony);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);
            aion.MoveMouse(okbuttonx, okbuttony - 25);
            PauseForMilliSeconds(60);
            aion.MouseLeftClick();
            PauseForMilliSeconds(300);
        }

        public void resurrect()
        {
            TakeOver aion = new TakeOver((int)hwndAion);
            RECT aionscreen = new RECT();

            PauseForMilliSeconds(1300); //wait for the death
            diedcount++;
            //int halfx = 0;
            //int halfy = 0;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Resurrect clicking started");

            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(800);
            GetWindowRect(hwndAion, ref aionscreen);
            //halfx = Convert.ToInt32((aionscreen.Right + aionscreen.Left) * .58);
            //halfy = Convert.ToInt32((aionscreen.Bottom + aionscreen.Top) * .57);

            object[,] nControlList = new object[12, 7];
            for (int b = 0; b < 9; b++)
            {
                nControlList[b, 0] = 0; //parent
                nControlList[b, 1] = 0; //address
                nControlList[b, 2] = 0; //name of slot
                nControlList[b, 3] = 0; //ready
                nControlList[b, 4] = 0; //skill number
                nControlList[b, 5] = 0; //PosX
                nControlList[b, 6] = 0; //PosY
            }
            nControlList = AddControl(Memory.ReadInt(Process.handle, (Process.Modules.Game + aionOffsets.resInfoAddress)), -1, nControlList); //Resurrection


            int i = 0;

            if (Convert.ToInt32(nControlList[i, 1]) != 0)
            {
                //int baseoffset = Memory.ReadInt(Process.handle, (Convert.ToInt32(nControlList[i, 1])));
                int baseoffset = Convert.ToInt32(nControlList[i, 1]);
                int childlist = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C0));
                int childsize = (int)Memory.ReadUInt(Process.handle, (uint)(baseoffset + 0x1C4));

                int thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(childlist + 0x4));
                for (int j = 1; j <= childsize - 1; j++)
                {
                    thiscontrol = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x4));
                    int final = (int)Memory.ReadUInt(Process.handle, (uint)(thiscontrol + 0x8));
                    nControlList = AddControl(final, i, nControlList);
                }
            }
            int offsety = 3;
            int okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + 35 + Convert.ToInt32(nControlList[2, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
            if (leftres == true)
                okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + -35 + Convert.ToInt32(nControlList[2, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
            int offsetx = 0;

            while (pc.Health == 0)
            {

                int okbuttony = Convert.ToInt32(aionscreen.Top + Convert.ToDouble(nControlList[0, 6])) + offsety + 32 + Convert.ToInt32(nControlList[2, 6]); //+ Convert.ToInt32(nControlList[2, 6]));
                aion.MoveMouse(okbuttonx, okbuttony);
                PauseForMilliSeconds(100);
                aion.MouseLeftClick();
                //aion.ControlClickWindow("left", okbuttonx, okbuttony, false);
                offsety += 3;
                if (offsety > 30)
                {
                    offsety = -5;
                    offsetx += 3;
                    if (offsetx > 82) offsetx = -10;
                    //keyenumerator("ENTER");
                    okbuttonx = Convert.ToInt32(aionscreen.Left + Convert.ToDouble(nControlList[0, 5])) + 32 + offsetx + Convert.ToInt32(nControlList[2, 5]); //+ Convert.ToInt32(nControlList[2, 5]));
                }
            }

            /*if (leftres == true)
            {
                for (int x = -220; x < 220; x = x + 15)
                {
                    for (int y = -210; y < 131; y = y + 15)
                    {
                        if (pc.Health > 0) return;
                        aion.MoveMouse(halfx + x, halfy + y);
                        PauseForMilliSeconds(20);
                        aion.MouseLeftClick();
                    }
                }
            }
            else
            {
                for (int x = 131; x > -220; x = x - 15)
                {
                    for (int y = -210; y < 131; y = y + 15)
                    {
                        if (pc.Health > 0) return;
                        aion.MoveMouse(halfx + x, halfy + y);
                        PauseForMilliSeconds(25);
                        aion.MouseLeftClick();
                    }
                }
            }*/
        }

        private void buffcast()
        {
            if (findpcstance() == eStance.Resting)
            {
                keyenumerator("UP");//get up
                PauseForMilliSeconds(1650);
            }
            for (int i = 0; i < numbuffs; i++)
            {
                if (bufftimes[i] <= DateTime.Now)
                {
                    PauseForMilliSeconds(500);
                    bufftimes[i] = DateTime.Now + buffdelays[i];
                    lblstatus.Text = "Status: Casting Buff: " + i;
                    keyenumerator(buffbtns[i]);
                    PauseForMilliSeconds(1000);
                    Application.DoEvents();
                    PauseForMilliSeconds(1300);
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Buff cast: " + i);

                    lblstatus.Text = "Status: Finished Buff: " + i;

                }
            }
            if (pc.Class == eClass.Spiritmaster)
            {
                Get_Petid();
                Entity pet = new Entity(petptr);
                label1.Visible = true;
                label1.Text = "PetHP: " + pet.Health + "%";
                if (pet.Health == 0)
                {
                    try
                    {
                        Attacklist.SelectedNode = Attacklist.Nodes[4].FirstNode;
                        string skillname = Attacklist.SelectedNode.Text;
                        if (skillready(skillname) == true)
                        {
                            string[] info = Convert.ToString(Attacklist.SelectedNode.Tag).Split('|');
                            string key = info[1];
                            if (key == "") MessageBox.Show(skillname + " has no key bound");
                            int delay = abilities[Attacklist.SelectedNode.Text].CastTime;
                            delay = delay + Convert.ToInt32(info[0]);
                            if (delay < 200) delay = 300;

                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                        }
                    }
                    catch (Exception) { MessageBox.Show("SM: You are missing your pet in the skills editor!"); }
                }
            }
            else label1.Visible = false;
        }

        private void looting()
        {
            lootcounter = 0;
            while (tar.Type == eType.DeadwLoot && ignorelist.Contains(tar.ID.ToString()) == false) //ADD INVENTORY FULL CHECK
            {
                lblstatus.Text = "Status: Looting..: " + lootcounter;
                pc.Updateafterkill();
                tmrstuck.Stop();

                Application.DoEvents();
                //textBox1.Text += "Going into 1st looting" + Environment.NewLine;
                if ((tar.IsDead == true || tar.HealthHP == 0) && tar.Name != "")
                {
                    lootcounter++;
                    keyenumerator(keyloot);
                }
                PauseForMilliSeconds(lootdelay);
                if (pc.CubeFull() == true && invcheck == true && lootcounter >= 2)
                    lootcounter = 5;
                if (lootcounter == 4) keyenumerator("Enter");
                if (lootcounter >= 5) { lootcounter = 0; ignorelist.Add(tar.ID.ToString()); keyenumerator("SPACE"); PauseForMilliSeconds(250); keyenumerator("ESC"); break; }
                if (savelog == true) tw.WriteLine(DateTime.Now + " Looting finished");
                if (savelog == true) tw.WriteLine(DateTime.Now + " Total Exp gained: " + lblxpgain.Text.ToString());
                killLabel.Text = "Kills: " + kills;
                lblstatus.Text = "Status: Finished Looting..: " + lootcounter;
            }
            //if (pc.CubeFull() == true) { lootcounter = 0; ignorelist.Add(tar.ID.ToString());}
            if (savelog == true && pc.CubeFull() == true) tw.WriteLine(DateTime.Now + " Inventory Full");
        }

        private bool checkreturn()
        {
            foreach (Ability item in abilities)
            {
                if (item.AbilityName.Contains("Return")) return skillready("Return");
                if (item.AbilityName.Contains("Rückkehr")) return skillready("Rückkehr");
                if (item.AbilityName.Contains("Retour")) return skillready("Retour");
                if (item.AbilityName.Contains("Возвращение")) return skillready("Возвращение");
            }
            return false;
        }

        private void combat()
        {
            bool incombatloop = true;

            if (attackflag == false)
            {
                while (incombatloop == true)
                {
                    //while (incombatloop == true)
                    //{
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                    combatcounting++;
                    attackflag = true;
                    if (combatcounting >= 1000000) combatcounting = 0;
                    Application.DoEvents();
                    //textBox1.Text += "Top of combat! "+ combatcounting + Environment.NewLine;
                    //lblstatus.Text = "Status: Starting combat loop! " + combatcounting;
                    if (btnstop.Visible == false) { lblstatus.Text = "Status: Stopping"; incombatloop = false; return; }
                    if (pc.Health == 0) { deathsystem(); return; }

                    if (pc.ID == tar.ID && keypressing == false) { keyenumerator("ESC"); PauseForMilliSeconds(200); }

                    if (tar.ID != 0) looting();
                    if (btnstop.Visible == true) { rest(); }//RESTtextBox1.Text += "Going into rest" + Environment.NewLine;
                    //tar.TargetID != pc.ID
                    //Application.DoEvents();
                    if ((venderrun == false) && (deathrun == false) && (usevender == true) && (checkreturn() == true) && (pc.CubeFull() == true) && venderpointlist.Count > 0) { vendersystem(); return; }

                    if ((tar.IsDead != true) && tar.TargetID == pc.ID && tar.Type == eType.AttackableNPC) //ADD DETECTION/FIGHT
                    {
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                        lblstatus.Text = "Status: Attacking ADD..";
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Attacking ADD:" + tar.Name);
                        tmrtabby.Stop();
                        tmrstuck.Stop();
                        dps = 0;
                        dpscounter = 0;
                        dpstimer.Enabled = true;

                        mainattackloop();
                        dpstimer.Enabled = false;
                        PauseForMilliSeconds(100);
                    }

                    if (tar.ID != 0) looting();

                    if (btnstop.Visible == true && adddetect() == false) buffcast();//ACTIVATE BUFFS HERE

                    if (btnstop.Visible == true && adddetect() == false) { rest(); }//RESTtextBox1.Text += "Going into rest" + Environment.NewLine;
                    //Application.DoEvents();
                    try
                    {
                        //textBox1.Text += "Tar.TargetID: " + tar.TargetID.ToString() + " Pc.ID: " + pc.ID + Environment.NewLine;
                        if (btnstop.Visible == true && (tar.TargetID != pc.ID || pc.CubeFull() == true || tar.Type == eType.Player) && adddetect() == false) //someone heals ya here and fucks up
                        {
                            findclosestwaypoint();
                            if (deathrun == false)
                            {
                                //movecounter = waypointlist.IndexOf(findclosestwaypoint()); //BUG

                                if (movecounter < 0)
                                {
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " Closest waypoint error < 0 " + movecounter);
                                    movecounter = 0;
                                }
                                if (movecounter > waypointlist.Count - 1)
                                {
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " Closest waypoint error > max " + movecounter);
                                    movecounter = waypointlist.Count - 1;
                                }

                                if (savelog == true) tw.WriteLine(DateTime.Now + " Closest waypoint is " + movecounter);
                            }
                            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                            //waymovement(false);
                            //Application.DoEvents();
                            //textBox1.Text += "Going into findmob" + Environment.NewLine;
                            doheal();
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Waypoint system started");
                            istabbing = false; //Fix issue with running waypoint without targeting 
                            findmob();
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Waypoint system finished");

                            tmrtabby.Stop();
                            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                            //textBox1.Text += "Out of findmob" + Environment.NewLine;
                        }//SEARCH FOR MOB
                    }
                    catch (Exception eb) { MessageBox.Show("Error in closewaypoint + findmob " + eb); }

                    try
                    {
                        if (btnstop.Visible == true && tar.Name != "" && tar.Type == eType.AttackableNPC)
                        {
                            int tarlevel;
                            tarlevel = Convert.ToInt32(tar.Level);
                            if (tarlevel > ignorelevel && ignorelist.Contains(tar.ID.ToString()) == false && ignorelist.Contains(tar.Name) == false)
                            {
                                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                                tmrtabby.Stop();
                                tmrstuck.Stop();
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Going into Preattacks");
                                if (pc.Health < pc.MaxHealth) doheal();
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Fighting " + tar.Name);
                                //textBox1.Text += "Going into preattacks" + Environment.NewLine;
                                attackmob();

                            }
                        }
                    }
                    catch (Exception ea) { MessageBox.Show("Error in stoptimers and attack " + ea); }

                    attackflag = false;
                    //PauseForMilliSeconds(30);
                    //Application.DoEvents();
                    //lblstatus.Text = "Status: Finished combat loop! Flag=" + incombatloop + " C:" + combatcounting;
                    //textBox1.Text += " Finished combat loop! counter: " + combatcounting + Environment.NewLine;
                    //Application.DoEvents();
                    //}
                    if (btnstop.Visible == false) { lblstatus.Text = "Status: Stopping"; incombatloop = false; return; }
                }
                //lblstatus.Text = "Exited combat loop no:" + combatcounting;
                //if (savelog == true) tw.WriteLine(DateTime.Now + " Exited combat loop");
                //PauseForMilliSeconds(20);
            }
        }

        private void findmob()
        {
            bool hasplayer;
            ismoving = false;
            if (tar.Type == eType.AttackableNPC && tar.HasTarget == true && tar.TargetID != pc.ID && tar.TargetID != tar.ID)
            {
                hasplayer = true;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Has Player = True ");
            }
            else hasplayer = false;
            if (adddetect() == true) { if (savelog == true) tw.WriteLine(DateTime.Now + " ADD FOUND! " + tar.Name); return; }

            if (isresting == false)
            {
                if (adddetect() == true) { lblstatus.Text = "Status: Found ADD!"; if (savelog == true) tw.WriteLine(DateTime.Now + " ADD FOUND! " + tar.Name); return; }

                while ((deathrun == true && defenddeath == false) || tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString())))
                {
                    if (deathrun == true) lblstatus.Text = "Status: Running death waypoints.. " + deathcounter;
                    if (venderrun == true) lblstatus.Text = "Status: Running vender waypoints..." + vendercounter;
                    if ((deathrun == false) && (venderrun == false)) lblstatus.Text = "Status: Searching.. " + movecounter;

                    if (findpcstance() == eStance.Resting) keyenumerator("UP"); //get up
                    if (pc.Health == 0) return;

                    if (pc.ID == tar.ID && keypressing == false) { PauseForMilliSeconds(200); keyenumerator("ESC"); }
                    Application.DoEvents();
                    if (deathrun == false || (deathrun == true && defenddeath == true))
                    {
                        if (tar.TargetID == pc.ID && tar.ID != pc.ID && tar.Health > 0 && tar.Type != eType.Player)
                        {
                            return;
                        }
                    }

                    waymovement(true);
                    //PauseForMilliSeconds(30);

                    if (gatherenabled == true && deathrun == false) gather(); //See if you can gather!

                    if ((istabbing == false) && (deathrun == false) && (venderrun == false) && (ignorelevel < 50))
                    {
                        PauseForMilliSeconds(100);
                        istabbing = true;
                        keyenumerator(keytarget);
                        tmrtabby.Start();
                    }
                    Application.DoEvents();

                    if (ismoving == false)
                    {
                        ismoving = true;
                        PauseForMilliSeconds(200);
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
                    }


                    if (tmrstuck.Enabled == false)
                    {
                        stuckcounter = 0;
                        returncounter = 0; tmrstuck.Start();
                    }
                    if (tar.Type == eType.AttackableNPC && tar.HasTarget == true && tar.TargetID != pc.ID && tar.TargetID != tar.ID) hasplayer = true; else hasplayer = false;
                    if (btnstop.Visible == false)
                    {
                        if (ismoving == true) keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0); //stops ya
                        istabbing = false;
                        ismoving = false;
                        return;
                    }

                }
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                istabbing = false;
                tmrstuck.Stop();
                tmrtabby.Stop();
                stuckcounter = 0;
                lblstatus.Text = "Status: Finished Finding Mob!";
            }
        }//FINDMOB

        public void changewaypoint()
        {
            if (deathrun == true)
            {
                if (pc.Distance3D(awaypoint.X, awaypoint.Y, awaypoint.Z) < 2.5) //hit waypoint CHANGE THIS
                {
                    deathcounter++;
                }

                if (deathcounter <= 0) { deathcounter = 0; forward = true; }
                if (deathcounter >= deathpointlist.Count - 1)
                {
                    deathrun = false; deathcounter = 0; soldcrap = false; return;
                }
                awaypoint = deathpointlist[deathcounter];
            }
            else if (venderrun != true)
            {

                if (pc.Distance3D(awaypoint.X, awaypoint.Y, awaypoint.Z) < 2.5) //hit waypoint CHANGE THIS
                {
                    if (movecounter <= 0)
                    {
                        forward = true;
                        movecounter = 0;
                    } //switch from backwards to forward
                    if (movecounter >= waypointlist.Count - 1)
                    {
                        if (loopit == false)
                        {
                            forward = false;
                        }
                        else
                        {
                            movecounter = -1;
                        }

                    }
                    if (forward == true && movecounter < waypointlist.Count - 1)
                        movecounter++;
                    if (forward == false && movecounter > 0)
                        movecounter--;


                    lblstatus.Text = "Status: Going to waypoint: " + movecounter;
                }

                //if (movecounter >= waypointlist.Count - 1 ) { movecounter = waypointlist.Count - 1; forward = false; }

                //if (movecounter <= waypointlist.Count - 1 && movecounter >= 0)
                awaypoint = waypointlist[movecounter];
                /*else
                {
                    MessageBox.Show("Error with changewaypoint. " + movecounter);
                    //movecounter = waypointlist.IndexOf(findclosestwaypoint());
                    awaypoint = waypointlist[movecounter];
                }*/

            }
            if ((venderrun == true) && (usevender == true))
            {
                if (pc.Distance3D(awaypoint.X, awaypoint.Y, awaypoint.Z) < 2.5)
                {
                    if ((vendercounter <= 0) && (soldcrap == false) && (findVender(venderName) == true))
                    {
                        curkinah = pc.Kinah;
                        sellmisc();

                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
                        PauseForMilliSeconds(600);
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);

                        forward = true;
                        vendercounter = 0;
                    }
                    if ((vendercounter >= venderpointlist.Count - 1) && (soldcrap == false))
                    {
                        forward = false;
                    }
                    if ((vendercounter >= venderpointlist.Count - 1) && (soldcrap == true))
                    {
                        if (curkinah == pc.Kinah) usevender = false;
                        venderrun = false; vendercounter = venderpointlist.Count - 1; deathrun = true; return;
                    }
                    if (forward == true && vendercounter < venderpointlist.Count - 1)
                        vendercounter++;
                    if (forward == false && vendercounter > 0)
                        vendercounter--;

                    lblstatus.Text = "Status: Going to venderpoint: " + vendercounter;

                }

                awaypoint = venderpointlist[vendercounter];
            }

            lblwprange.Text = "WPRange: " + string.Format("{0:f2}", pc.Distance3D(awaypoint.X, awaypoint.Y, awaypoint.Z));
        }

        public void findclosestwaypoint()
        {
            cwaypoint closestpoint = new cwaypoint();
            closestpoint = waypointlist[0];
            int dcounter = 0;
            int mcounter = 0;
            int vcounter = 0;
            double distclosest = 1300;

            try
            {
                foreach (cwaypoint currentpoint in waypointlist)
                {
                    double newwp = pc.Distance3D(currentpoint.X, currentpoint.Y, currentpoint.Z);
                    if (distclosest > newwp)
                    {
                        closestpoint = currentpoint;
                        distclosest = newwp;
                        movecounter = mcounter;
                        deathrun = false;
                    }
                    mcounter++;
                }
                //ADD CHECKS FOR DEATH POINT SYSTEM IF CLOSER
                if (pc.Distance3D(closestpoint.X, closestpoint.Y, closestpoint.Z) > 60)
                {
                    foreach (cwaypoint currentpoint in deathpointlist)
                    {
                        double newwp = pc.Distance3D(currentpoint.X, currentpoint.Y, currentpoint.Z);
                        if (distclosest > newwp)
                        {
                            closestpoint = currentpoint;
                            distclosest = newwp;
                            deathcounter = dcounter;
                            deathrun = true;
                        }
                        dcounter++;
                    }
                }
                //OMG this little check here was screwing it all up, I HAD IT BACKWARDS
                if ((pc.Distance3D(closestpoint.X, closestpoint.Y, closestpoint.Z) > 60) && (soldcrap == false) && (venderrun = true) && (usevender == true))
                {
                    foreach (cwaypoint currentpoint in venderpointlist)
                    {
                        double newwp = pc.Distance3D(currentpoint.X, currentpoint.Y, currentpoint.Z);
                        if (distclosest > newwp)
                        {
                            closestpoint = currentpoint;
                            distclosest = newwp;
                            deathcounter = dcounter;
                            venderrun = true;
                        }
                        vcounter--; //go down list rather than up...
                    }
                }
            }
            catch (Exception e) { MessageBox.Show("Error in finding closest waypoint. " + e); }
            if (savelog == true) tw.WriteLine(DateTime.Now + " Closest waypoint found! Dist: " + distclosest);
            if (distclosest > 200)
            {
                if (savelog == true) tw.WriteLine(DateTime.Now + " Nearest waypoint was > 200! ERROR WP: " + mcounter);
                keyenumerator(keyreturn);
                PauseForMilliSeconds(10000);
            }
        }

        public void waymovement(bool ontrack)
        {
            if (ontrack == true) changewaypoint();

            Application.DoEvents();
            pc.UpdateRot();
            //pc.WriteRot((float)awaypoint.Face(pc.Y, pc.X));
            float x = pc.Rotation;
            float temp = (float)awaypoint.Face(pc.Y, pc.X);
            PauseForMilliSeconds(10);

            while (Math.Abs((x) - (temp)) > 3 )
            {
                if (pc.Health == 0 || btnstop.Visible == false) return;
                
                pc.UpdateRot();
                x = pc.Rotation;
                //if (x > 180) x = -179;
                changewaypoint();
                temp = (float)awaypoint.Face(pc.Y, pc.X);
                
                //label11.Text = x.ToString() + " : " + temp.ToString();
                if (temp > 0)//x = -179;
                { 
                    if (x >= (temp - 180) && x <= temp -3) x += (float)2.2;
                    else if ((-180 < (temp - 180) && x < (temp -180)) || x > temp +3) x -= (float)2.2;
                }
                else
                {
                    if (x > temp +3 && x < (temp + 180)) x -= (float)2.2;
                    else if ((-180 < temp && x < temp -3) || x > (temp + 180)) x += (float)2.2;
                }
                
                if (x > 180) x =  -180;
                if (x < -180) x = 179;
                pc.WriteRot((float)(x));
                Application.DoEvents();
            }
            if(Math.Abs((x) - (temp)) <= 3 && Math.Abs((x) - (temp)) > 0)
                pc.WriteRot((float)awaypoint.Face(pc.Y, pc.X));
            //label11.Text = x.ToString() + " : " + temp.ToString();

            /* (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 2) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180)))
            if (awaypoint.Face(pc.Y, pc.X) > 0)
            {
                if ((pc.Rotation > (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) - 2)) //left within 1.5 degrees
                {
                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while ((pc.Rotation >= (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation <= (awaypoint.Face(pc.Y, pc.X)) - 2))  //LEFT
                    {
                        if (pc.Health == 0 || btnstop.Visible == false) { keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); return; }
                        if (ontrack == true) changewaypoint();
                        Application.DoEvents();
                        pc.UpdateRot();
                        changewaypoint();
                    }
                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
                else if (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 2)) //right within 1.5 degrees
                {

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 2)) //RIGHT
                    {
                        if (ontrack == true) changewaypoint();
                        if (pc.Health == 0 || btnstop.Visible == false) { keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        changewaypoint();
                    }

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); //  Up
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
            }//end of positive rot point

            else //NEG POINT
            {
                if ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 2) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180)) //left within 1.5 degrees
                {

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 2) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180)) //RIGHT
                    {
                        if (ontrack == true) changewaypoint();
                        if (pc.Health == 0 || btnstop.Visible == false) { keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        changewaypoint();
                    }

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
                else if (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 2) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180))) //right within 1.5 degrees
                {

                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 2) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180)))//LEFT
                    {
                        if (ontrack == true) changewaypoint();
                        if (pc.Health == 0 || btnstop.Visible == false) { keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        changewaypoint();
                    }

                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
            
            }
*/
        }

        private void tmrstuck_Tick(object sender, EventArgs e)
        {

        }

        private void waypointEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            waypointeditor f3 = new waypointeditor();
            f3.Show();
        }

        private void waypointEditorToolStripMenuItem_Click_1()
        {

        }


        private void tmrpot_Tick(object sender, EventArgs e)
        {
            potready = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Pot cooldown finished");
            tmrpot.Stop();
        }


        /*private void tmrtabby_Tick(object sender, EventArgs e)
        {
            bool hasplayer;
            if (tar.Health < 100 && tar.HasTarget == true) hasplayer = true;
            else hasplayer = false;

            if (tar.Type != eType.AttackableNPC || ((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
            {
                if (istabbing == true)
                {
                    keyenumerator(keytarget);
                }
            }
            else
            {
                tmrtabby.Stop();
                istabbing = false;
                //keyenumerator("s");
            }
            if (btnstop.Visible == false) tmrtabby.Stop();
        }*/

        /*private void tmrheal_Tick_1(object sender, EventArgs e)
        {
            healready = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Heal cooldown finished");

            tmrheal.Stop();
        }*/

        private void waypointEditorToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            waypointeditor f3 = new waypointeditor();
            f3.Show();
        }
        /* do
      {
            pid = 0;
            // get the window handle
            h = FindWindowEx(IntPtr.Zero, h, null, null);
            // get the process id (and thread id)
            tid = GetWindowThreadProcessId(h, out pid);
            // Console.WriteLine("h = {0} pid = {1}", h.ToInt32(), pid);
            // test for a match of our saved pID
            if (pid == procID)
            {
                  // restore the window
                  ShowWindow(h, SW_RESTORE);
                  break;
            }
      } while( !h.Equals(IntPtr.Zero) );*/

        private void tmrtabby_Tick_1(object sender, EventArgs e)
        {

            bool hasplayer;
            tar.UpdateID();//force update
            if (tmrtabby.Enabled == true)
            {
                if (tar.Health < 100 && tar.HasTarget == true && tar.TargetID != pc.ID && tar.TargetID != tar.ID) hasplayer = true;
                else hasplayer = false;
                if (btnstop.Visible == false) { tmrtabby.Stop(); lblstatus.Text = " Stopped bot in tab"; return; }
                if (savelog == true) tw.WriteLine(DateTime.Now + " In targetting timer");
                if (tar.Type != eType.AttackableNPC || ((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
                {
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Need new target");
                    if (istabbing == true && keypressing == false)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Pressing target key: " + keytarget + " Old tar: " + tar.Name);
                        keyenumerator(keytarget);
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Target key pressed. NewTar: " + tar.Name);
                    }
                }
                else
                {
                    istabbing = false;
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Stopped targetting. Tar: " + tar.Name);

                    tmrtabby.Stop();
                    //istabbing = false;

                }
            }
            if (btnstop.Visible == false) tmrtabby.Stop();
        }

        private void tmrstuck_Tick_1(object sender, EventArgs e)
        {
            if (antistuck == false) tmrstuck.Stop();
            if (antistuck == true)
            {
                if (btnstop.Visible == false || pc.Health == 0) { tmrstuck.Stop(); tmrstuck.Enabled = false; return; }

                if (previouspoint.X == 0 & previouspoint.Y == 0) { previouspoint.Set(pc.X, pc.Y, pc.Z); PauseForMilliSeconds(300); }
                double distance = pc.Distance3D(previouspoint.X, previouspoint.Y, previouspoint.Z);
                //FOR MOB STUCK CODE
                //if (pc.IsCasting == true) { stuckcounter = 0; returncounter = 0; }
                if (tar.Name != "" && tar.TargetID != pc.ID && pc.Spell == 0)
                {
                    stuckcounter++;
                    if (distance < 1)
                    {
                        if (stuckcounter == 3)
                        {
                            keyenumerator("SPACE");
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: Jump!");
                        }
                        if (stuckcounter == 4 || stuckcounter == 5)
                        {
                            if (pc.Spell == 0)
                            {
                                //keyenumerator("SPACE");

                                returncounter++;
                                if (returncounter == 1)
                                {
                                    keyenumerator(keystrafeL);
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: StrafeL");
                                }
                                if (returncounter == 2)
                                {
                                    keyenumerator(keystrafeR);
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: StrafeR");
                                }
                            }//stuckcounter = 0;
                        }
                    }

                    if (stuckcounter >= 6)
                    {
                        if (ignorelist.Contains(tar.ID.ToString()) == false)
                        {
                            ignorelist.Add(tar.ID.ToString());
                            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                            lblstatus.Text = "Status: Ignoring mob ID: " + tar.ID.ToString();
                            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: Ignored mob:" + tar.ID.ToString());
                        }
                        //if (movecounter <= waypointlist.Count - 1) awaypoint = findclosestwaypoint();
                        //PauseForMilliSeconds(100);
                        //keyenumerator("ESC");
                        stuckcounter = 0;
                        returncounter = 0;
                        ismoving = false;
                        if (keypressing == false) keyenumerator("ESC");
                        PauseForMilliSeconds(100);
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                        if (tmrunstuck.Enabled == false) tmrunstuck.Start();
                    }
                }

                //FOR JUST WAYPOINT WALKING, NO MOB TARGETTED
                if (pc.Spell == 0 && tar.Name == "")//(tar.HasTarget != true || tar.Name == ""))
                {
                    if (distance < 1.5)
                    {
                        stuckcounter++;
                        keyenumerator("SPACE");
                        ismoving = false;
                        if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: Jump");
                    }
                    else { stuckcounter = 0; returncounter = 0; }
                    if (stuckcounter >= 2)
                    {
                        //keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0); //just in case
                        ismoving = false;
                        returncounter++;

                        if (returncounter == 1)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: StrafeL");
                            //keyenumerator(keystrafeL);
                        }
                        if (returncounter == 2)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: StrafeR");
                            keyenumerator(keystrafeR);
                        }
                        stuckcounter = 0;
                    }
                    if (returncounter >= 2)
                    {
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
                        if (forward == false)
                        {
                            forward = true;
                        } //switch from backwards to forward
                        else
                        {
                            forward = false;
                        }
                        /*if (forward == true && movecounter < waypointlist.Count - 1) movecounter++;
                        if (forward == false && movecounter > 0) movecounter--;
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Stuck: Going back to WP: " + movecounter);
                        */
                        findclosestwaypoint();
                        //if (movecounter <= waypointlist.Count - 1 && movecounter >= 0) awaypoint = waypointlist[movecounter];
                        stuckcounter = 0;
                        returncounter = 0;


                    }
                    //prevdistance = distance;
                    //previouspoint.Set(pc.X, pc.Y, pc.Z);
                    if (btnstop.Visible == false) tmrstuck.Stop();
                }
                previouspoint.Set(pc.X, pc.Y, pc.Z);
            }
        }

        private void tmrunstuck_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ignorelist.Count != 0)
                {
                    for (int i = ignorelist.Count - 1; i >= 0; i--)
                    {
                        string currentValue = (string)ignorelist[i];

                        if (IsNumber(currentValue) == true)
                        {
                            ignorelist.Remove(currentValue);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Unstuck: Removing mob from Ignore: " + currentValue);

                        }
                    }
                }
                if (gatherignorelist.Count != 0)
                {
                    for (int i = gatherignorelist.Count - 1; i >= 0; i--)
                    {
                        string currentValue = (string)gatherignorelist[i];
                        gatherignorelist.Remove(currentValue);
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Unstuck: Removing gather node from Ignore: " + currentValue);
                    }

                }
            }

            catch (Exception e1) { MessageBox.Show("Error with unstucktmr: " + e1); }

        }
        private void release()
        {
            keyenumerator(keyreturn);
            PauseForMilliSeconds(6000);
        }
        private void tmrshutoff_Tick(object sender, EventArgs e)
        {
            while (tar.ID != 0)
            {
                Application.DoEvents();
                PauseForMilliSeconds(100);
            }

            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(100);
            if (shutofftime > 0) tmrshutoff.Stop();
            btnwaypoint.Enabled = true;
            btnautoway.Enabled = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Shutoff timer reached");

            tmrstuck.Stop();
            stop();
            istabbing = false;
            release();
        }
        public bool AionFocused()
        {
            IntPtr activeWindowHandle = GetForegroundWindow();
            if (hwndAion == activeWindowHandle)
            {
                return true;
            }
            else
                return false;
        }
        private void showkp_Click(object sender, EventArgs e)
        {
            if (keypresswindow == false)
            {
                this.Width = 735;
                this.showkp.Text = "Hide <-";
                keypresswindow = true;
            }
            else
            {
                this.Width = 541;        //490normal
                this.showkp.Text = "Show ->";
                keypresswindow = false;
            }
        }

        public bool findVender(string venderName)
        {
            int trynpc = 0;
            //send up keys since we are at our target
            keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
            keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);

            while ((tar.Name != venderName) && trynpc < 20) //if it doesnt find the vender in 20 tabs, give up and go back
            {
                keyenumerator("F7");
                PauseForMilliSeconds(800);
                lblstatus.Text = "Status: NPC ID: " + tar.Name + " we need : " + venderName;
                trynpc++;
            }
            if (tar.Name == venderName)
            {
                keyenumerator(keyautoatk);
                PauseForMilliSeconds(6000);
                return true;
                //sellmisc();
            }
            else { return false; }

        }

        private void SetVender_Click(object sender, EventArgs e)
        {
            if (tar.Name != "")
            {
                venderName = tar.Name;
                lblstatus.Text = "NPC Name: " + venderName;
            }
            else lblstatus.Text = "NPC name is invalid! ";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked == true) this.checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true) this.checkBox2.Checked = false;
        }

        private void lblwprange_Click(object sender, EventArgs e)
        {
            changewaypoint();
        }

        private void skillEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkillEditor f4 = new SkillEditor();
            f4.Show();
        }

        private void AttackLooplist1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void playerExp_Click(object sender, EventArgs e)
        {
            sellmisc();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
            stop();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Angelbot Exiting");
            tw.Close();
            Process.Close();
            Environment.Exit(0);
        }

        private void dpstimer_Tick(object sender, EventArgs e)
        {
            if (dpscounter == 0)
            {
                dpscounter++;
                lasttarHP = tar.HealthHP;
            }
            else
            {
                dpscounter++;
                int tempdps = lasttarHP - tar.HealthHP;
                dps = (dps + tempdps);
                lasttarHP = tar.HealthHP;
                label11.Text = "DPS: " + dps / dpscounter;
            }
        }
        private int FindKey(string k)
        {
            switch (k)
            {
                case "1":
                    return KeyConstants.VK_1;
                case "2":
                    return KeyConstants.VK_2;
                case "3":
                    return KeyConstants.VK_3;
                case "4":
                    return KeyConstants.VK_4;
                case "5":
                    return KeyConstants.VK_5;
                case "6":
                    return KeyConstants.VK_6;
                case "7":
                    return KeyConstants.VK_7;
                case "8":
                    return KeyConstants.VK_8;
                case "9":
                    return KeyConstants.VK_9;
                case "0":
                    return KeyConstants.VK_0;
                case "-":
                    return KeyConstants.VK_SUBTRACT;
                case "+":
                    return KeyConstants.VK_ADD;
                default:
                    return KeyConstants.VK_1;
            }
        }
        private void playerExp_Click_1(object sender, EventArgs e)
        {
            SKeys keys = new SKeys(hwndAion);

            PauseForMilliSeconds(2000);
            //keys.SendKey(KeyConstants.VK_MENU);
            keys.SendKey(KeyConstants.VK_6);
            //keys.SendKey(KeyConstants.VK_MENU);
            //keys.ControlClickWindow("left", 300, 300, false);
            //keys.ControlClickWindow("right", 300, 300, false);

            //keys.SendUp();
            /*
            List<int> buffedlist = new List<int>();
            List<string> buffedlist2 = new List<string>();
            BuffList.Update();

            foreach (Buff item in BuffList)
            { 
               buffedlist.Add(item.AbilityID);
            }
            foreach (Ability item in abilities)
            {

                if (buffedlist.Contains(item.AbilityID))
                {
                    buffedlist2.Add(item.AbilityName);
                }
                
            }*/

        }
        public void gather()
        {
            string gathername = findgatherable();
            uint newgatheroffset = Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + gatheroffset));
            uint chatinput = Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + gatheroffset + 0x38));
            TakeOver aion = new TakeOver((int)hwndAion);
            //PauseForMilliSeconds(3000);


            if ((gathername != "" && adddetect() == false) && (!gatherignorelist.Contains(gathername)))
            {
                tmrtabby.Enabled = false;
                istabbing = false;
                ismoving = false;
                //antistuck = false;
                keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
                keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
                lblstatus.Text = "Status: Gathering: " + gathername;

                while (Memory.ReadByte(Process.handle, (uint)(chatinput + 0x24)) == 7)
                {
                    keyenumerator("ENTER");
                    PauseForMilliSeconds(1000);
                }


                keyenumerator("ENTER");
                keyenumerator("/");
                PauseForMilliSeconds(35);
                aion.SendKeyboardText(gatherselect + " " + gathername);
                PauseForMilliSeconds(200);
                keyenumerator("ENTER");
                PauseForMilliSeconds(200);
                while (Memory.ReadByte(Process.handle, (uint)(chatinput + 0x24)) == 7)
                {
                    keyenumerator("ENTER");
                    PauseForMilliSeconds(1000);
                }
                if (antistuck == true)
                {
                    stuckcounter = -1;
                    returncounter = -1;
                    tmrstuck.Start();
                }
                keyenumerator(keyautoatk); //heads towards target
                if (savelog == true) tw.WriteLine(DateTime.Now + " Gathering: " + gathername);
                //antistuck = true;
                int gatimer = 0;

                while (tar.Distance2D(pc.X, pc.Y) > 5.6) //wait loop while getting in range
                {

                    PauseForMilliSeconds(700);
                    if (adddetect() == true || gatimer >= 14)
                    {
                        gatherignorelist.Add(gathername);
                        if (tmrunstuck.Enabled == false) tmrunstuck.Start();
                        return;
                    }
                    keyenumerator(keyautoatk); //heads towards target
                    gatimer++;
                }
                tmrstuck.Stop();
                PauseForMilliSeconds(600);
                while (gathername != "")
                {
                    while (Memory.ReadByte(Process.handle, (uint)(chatinput + 0x24)) == 7)
                    {
                        keyenumerator("ENTER");
                        PauseForMilliSeconds(1000);
                    }
                    if (tar.Type == eType.Gatherable || tar.Type == eType.GatherableL)
                    {
                        PauseForMilliSeconds(100);
                        keyenumerator(keyautoatk); //gather!
                        PauseForMilliSeconds(600);
                        while (Memory.ReadByte(Process.handle, (uint)(newgatheroffset + 0x24)) == 7)
                        {
                            PauseForMilliSeconds(500);//Gathering node, wait
                        }
                        if (adddetect() == true) return;
                        PauseForMilliSeconds(200);
                    }
                    PauseForMilliSeconds(2300);
                    gathername = findgatherable();
                    while (Memory.ReadByte(Process.handle, (uint)(chatinput + 0x24)) == 7)
                    {
                        keyenumerator("ENTER");
                        PauseForMilliSeconds(1000);
                    }
                    if (gathername != "")
                    {
                        keyenumerator("ENTER");
                        keyenumerator("/");
                        PauseForMilliSeconds(35);
                        aion.SendKeyboardText(gatherselect + " " + gathername);
                        PauseForMilliSeconds(150);
                        keyenumerator("ENTER");
                    }
                    PauseForMilliSeconds(1400);
                    gathername = findgatherable();
                    tar.Update();
                    PauseForMilliSeconds(100);
                    if (button1.Visible == true) return;
                }
            }
            while (Memory.ReadByte(Process.handle, (uint)(chatinput + 0x24)) == 7)
            {
                keyenumerator("ENTER");
                PauseForMilliSeconds(1000);
            }
        }


        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lbltimelvl_Click(object sender, EventArgs e)
        {
            gather();
        }

        private void lbldistance_Click(object sender, EventArgs e)
        {

        }

        private void killLabel_Click(object sender, EventArgs e)
        {
            pc.UpdateRot();
            float x = pc.Rotation;
            while (pc.Rotation > 91 || pc.Rotation < 89)
            {
                x += (float)1.4;
                if (x > 180) x = -179;
                pc.WriteRot((float)(x));
                pc.UpdateRot();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tmrheart_Tick(object sender, EventArgs e)
        {
            if (heartbeat >= 30)
            {
                Login f11 = (Login)Application.OpenForms["Login"];
                if (f11.heartbeat() == false)
                {
                    stop();
                    MessageBox.Show("Someone else has logged in");
                    Environment.Exit(99);
                }
                else
                {
                    heartbeat = 0;
                }
            }

            heartbeat++;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
                this.TopMost = true;
            else this.TopMost = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Form1 f1 = new Form1();
            Map map1 = new Map();
            map1.Show();
        }

       

    }//FORMCLASS



    public class WindowsShell
    {
        #region fields
        public static int MOD_ALT = 0x1;
        public static int MOD_CONTROL = 0x2;
        public static int MOD_SHIFT = 0x4;
        public static int MOD_WIN = 0x8;
        public static int WM_HOTKEY = 0x312;
        #endregion

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int keyId;
        public static void RegisterHotKey(Form f, Keys key)
        {
            int modifiers = 0;

            if ((key & Keys.Alt) == Keys.Alt)
                modifiers = modifiers | WindowsShell.MOD_ALT;

            if ((key & Keys.Control) == Keys.Control)
                modifiers = modifiers | WindowsShell.MOD_CONTROL;

            if ((key & Keys.Shift) == Keys.Shift)
                modifiers = modifiers | WindowsShell.MOD_SHIFT;

            Keys k = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

            Func ff = delegate()
            {
                keyId = f.GetHashCode(); // this should be a key unique ID, modify this if you want more than one hotkey
                RegisterHotKey((IntPtr)f.Handle, keyId, modifiers, (int)k);
            };

            f.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
        }

        private delegate void Func();

        public static void UnregisterHotKey(Form f)
        {
            try
            {
                Func ff = delegate()
                {
                    UnregisterHotKey(f.Handle, keyId); // modify this if you want more than one hotkey
                };

                f.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
            }
            catch (Exception)
            {

            }
        }
    }

    public class cwaypoint
    {
        public float X;
        public float Y;
        public float Z;
        public void Clear()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public double Face(float pcy, float pcx)
        {
            double faceangle = Math.Atan2((Y - pcy), (X - pcx));
            faceangle = faceangle * 180 / Math.PI;
            faceangle -= 90;
            if (faceangle > 180) faceangle -= 360;
            faceangle -= 180;
            if (faceangle < -180 && faceangle > -360) faceangle += 360;
            if (faceangle > 180) faceangle -= 360;
            if (faceangle < -360) faceangle += 360;
            return (faceangle);
        }
    }

}//NAMESPACE