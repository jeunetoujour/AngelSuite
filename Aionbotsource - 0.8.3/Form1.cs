using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using AionMemory;
using MemoryLib;
using Ini;



namespace AngelBot
{
    public partial class Form1 : Form
    {
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

        IntPtr hwndAion;

        bool keypresswindow = false;
        Player pc;
        Target tar;

        int tabfind = 2;
        int myselfptr;
        double xpStart;
        int kinahStart;
        int kills;
        int tarID;
        //int tarLastKillID;
        DateTime xpStartTime;
        DateTime xpCurTime;
        TimeSpan elapsed;
        int resthp;
        int restmana;
        int healhp;
        int pothp;
        int potmp;
        int ignorelevel;
        int ignoretime;
        int oochealper;
        string ooctype;
        bool ishealer;
        bool isranged;
        bool isresting = false;
        bool potready = true;
        int rangedist;
        List<string> preattacks = new List<string>();
        List<string> attacks = new List<string>();
        List<string> buffs = new List<string>();
        List<string> ignorelist = new List<string>();
        public List<cwaypoint> waypointlist = new List<cwaypoint>();
        public List<cwaypoint> deathpointlist = new List<cwaypoint>();
        public int movecounter = 0;
        cwaypoint awaypoint = new cwaypoint();

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

        bool attackflag;
        bool autoway;
        bool forward = true;

        public Form1()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                MessageBox.Show("Aion process not found!! Exiting");
                Environment.Exit(1);
            }
        }

        private void clearAll()
        {
            pc = new Player();
            tar = new Target();
            pc.Update();
            playerName.Text = pc.Name;
            xpStart = pc.XP;
            kills = 0;
            tarID = 0;
            killLabel.Text = "Kills\n" + kills;
            kinahStart = pc.Kinah;
            xpStartTime = DateTime.Now;
            xpCurTime = xpStartTime;
            expHRLabel.Text = "Exp/Hr: 0";
            kinahLabel.Text = "Kinah: " + pc.Kinah;
            elapsedLabel.Text = "Running\n00:00:00";
            playerProg.Maximum = pc.MaxXP;
            playerProg.Value = pc.XP;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Process.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Aion process not found! Exiting");//Doesnt work!

                Environment.Exit(1);
            }
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("aion.bin");
            hwndAion = proc[0].MainWindowHandle;

            clearAll();
            pc.Update();
            tar.Update();
            xpStart = pc.XP;
            timer1.Start();
            this.TopMost = true;
            this.Opacity = .9;

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

            pc.Update();
            tar.Update();
            elapsed = DateTime.Now - xpStartTime;
            elapsedLabel.Text = "Running\n" + (elapsed.ToString()).Substring(0, 8);
            elapsed = DateTime.Now - xpCurTime;

            if (elapsed.TotalSeconds > .3)
            {
                
                label1.Text = "X: " + string.Format("{0:f3}", pcx);
                label2.Text = "Y: " + string.Format("{0:f3}", pcy);
                label3.Text = "Z: " + string.Format("{0:f3}", (pcz + 1.1)); //note the 1.1 added
                lblrot.Text = pc.Rotation.ToString();

                tarID = tar.ID;

                if (ignorelist.Contains(tar.Name) == true)//ignore button
                {
                    btnuningnore.Enabled = true;
                    btnignore.Enabled = false;

                }
                if (ignorelist.Contains(tar.Name) == false)
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
                tarhealth.Value = tar.Health;

                /*if(tar.Name == "")
                {
                     tarhealth.CreateGraphics().DrawString("No Target", new Font("Arial", (float)8.25, FontStyle.Bold), Brushes.WhiteSmoke, new PointF(tarhealth.Width / 2 - 25, tarhealth.Height / 2 - 7));
                }
                else*/
                tarhealth.CreateGraphics().DrawString(tarhealth.Value.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Bold), Brushes.WhiteSmoke, new PointF(tarhealth.Width / 2 - 15, tarhealth.Height / 2 - 7));

                healthbar.Maximum = pc.MaxHealth;
                if ((int)pc.Health <= healthbar.Maximum) healthbar.Value = (int)pc.Health;
                healthbar.CreateGraphics().DrawString(pc.Health + "/" + pc.MaxHealth + " " + string.Format("{0:F1}", (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.WhiteSmoke, new PointF(healthbar.Width / 2 - 40, healthbar.Height / 2 - 7));
                manabar.Maximum = pc.MaxMP;
                if ((int)pc.MP <= manabar.Maximum) manabar.Value = (int)pc.MP;
                manabar.CreateGraphics().DrawString(pc.MP + "/" + pc.MaxMP + " " + string.Format("{0:F1}", (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100) + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.WhiteSmoke, new PointF(manabar.Width / 2 - 40, manabar.Height / 2 - 7));
                displayrange();


                if (elapsed.TotalSeconds > 5) //updates screen stats
                {
                    pcXP = pc.XP;
                    pcMaxXP = pc.MaxXP;
                    playerExp.Text = "Exp: " + string.Format("{0:f3}", ((pcXP / pcMaxXP) * 100)) + "%";//string.Format("{0:n0}", pcXP) + "/" + string.Format("{0:n0}", pcMaxXP);
                    playerProg.Maximum = pcMaxXP;
                    playerProg.Value = (int)pcXP;
                    xpCurTime = DateTime.Now;
                    elapsed = xpCurTime - xpStartTime;
                    totalSeconds = elapsed.TotalSeconds;
                    xpperhr = Math.Round((pcXP - xpStart) * 3600 / totalSeconds / 1000, 2);
                    lblxpgain.Text = "XP: " + (pcXP - xpStart);
                    if (xpperhr < 0) clearAll();
                    expHRLabel.Text = "Exp/Hr: " + string.Format("{0:n2}", xpperhr) + "k";
                    kinahLabel.Text = "Kinah " + (pc.Kinah - kinahStart);
                    killLabel.Text = "Kills: " + kills;
                }
            }
        }

        private void resetAll_Click(object sender, EventArgs e)
        {
            clearAll();
            //stopmovement = true;
        }

        private void Form1_OnClosing(System.ComponentModel.CancelEventArgs e)
        {
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
            healhp = 0;
            pothp = 0;
            potmp = 0;
            ignorelevel = 0;
            ignoretime = 0;
            oochealper = 0;
            ishealer = false;
            isranged = false;
            rangedist = 0;
            preattacks.Clear();
            attacks.Clear();
            buffs.Clear();
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

        public void loadsettings()
        {
            clearsettings();
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\settings.ini");
            //string temp;
            //temp = ini.IniReadValue("limits", "RestHP").ToString();
            resthp = Int32.Parse(ini.IniReadValue("limits", "RestHP"));
            restmana = Int32.Parse(ini.IniReadValue("limits", "RestMana"));
            healhp = Int32.Parse(ini.IniReadValue("limits", "HealHP"));
            pothp = Int32.Parse(ini.IniReadValue("limits", "PotHP"));
            potmp = Int32.Parse(ini.IniReadValue("limits", "PotMP").TrimEnd(' '));
            ignorelevel = Int32.Parse(ini.IniReadValue("limits", "IgnoreLevel").TrimEnd(' '));
            ignoretime = Int32.Parse(ini.IniReadValue("limits", "IgnoreTime").TrimEnd(' '));
            oochealper = Int32.Parse(ini.IniReadValue("limits", "OOCHeal").TrimEnd(' '));
            oochealper = Int32.Parse(ini.IniReadValue("limits", "OOCHeal").TrimEnd(' '));
            ooctype = ini.IniReadValue("limits", "OOCType");

            ishealer = Convert.ToBoolean(ini.IniReadValue("character", "Healer").TrimEnd(' '));
            isranged = Convert.ToBoolean(ini.IniReadValue("character", "Ranged").TrimEnd(' '));
            rangedist = Int32.Parse(ini.IniReadValue("character", "RangeDist").TrimEnd(' '));

            string pretemp = ini.IniReadValue("preattacks", "PreAttacks");
            if (pretemp != "")
            {
                if (pretemp.StartsWith("|")) pretemp = pretemp.Substring(1, pretemp.Length);
                if (pretemp.Contains('\0').ToString() == "True")
                {
                    pretemp = pretemp.Substring(0, pretemp.LastIndexOf('\0') - 0);
                }
                string[] listpreattack = pretemp.Split('|');
                preattacks.AddRange(listpreattack);
            }
            string attacktemp = ini.IniReadValue("attacks", "Attacks");
            if (attacktemp != "")
            {
                if (attacktemp.StartsWith("|")) attacktemp = attacktemp.Substring(1, attacktemp.Length);
                if (attacktemp.Contains('\0').ToString() == "True")
                {
                    attacktemp = attacktemp.Substring(0, attacktemp.LastIndexOf('\0') - 0);
                }
                string[] listattack = attacktemp.Split('|');
                attacks.AddRange(listattack);
            }
            string bufftemp = ini.IniReadValue("buffs", "Buffs");
            if (bufftemp != "")
            {
                if (bufftemp.StartsWith("|")) bufftemp = bufftemp.Substring(1, bufftemp.Length);
                if (attacktemp.Contains('\0').ToString() == "True")
                {
                    attacktemp = attacktemp.Substring(0, attacktemp.LastIndexOf('\0') - 0);
                }
                string[] listbuff = bufftemp.Split('|');
                buffs.AddRange(listbuff);
            }
            keyloot = Convert.ToString(ini.IniReadValue("keybinds", "LootBtn"));
            keyrest = Convert.ToString(ini.IniReadValue("keybinds", "RestBtn"));
            keyhppot = Convert.ToString(ini.IniReadValue("keybinds", "Healthpot"));
            keymppot = Convert.ToString(ini.IniReadValue("keybinds", "Manapot"));
            keytarget = Convert.ToString(ini.IniReadValue("keybinds", "TargetBtn"));
            keyself = Convert.ToString(ini.IniReadValue("keybinds", "SelfTarget"));
            keyturn = Convert.ToString(ini.IniReadValue("keybinds", "TurnAround"));
            keyautoatk = Convert.ToString(ini.IniReadValue("keybinds", "Autoattack"));
            keyheal = Convert.ToString(ini.IniReadValue("keybinds", "Heal"));
            keyooch = Convert.ToString(ini.IniReadValue("keybinds", "OOCH"));

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {

                if (Process.Open() == false)
                {

                    MessageBox.Show("Aion process not found! Exiting");//Doesnt work!
                    Environment.Exit(1);
                }
                pc = new Player();
                tar = new Target();
            }
            catch (Exception)
            {
                MessageBox.Show("Problem found on startup! Exiting");//Doesnt work!
                Environment.Exit(1);
            }

            clearAll();
            pc.Update();
            xpStart = pc.XP;
            this.TopMost = true;
            this.Opacity = .9;
            loadsettings();
        }

        public void usehppot()
        {
            if (potready == true){
                if (((Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) < (double)pothp)
                {
                    lblstatus.Text = "Status: Using HP Pot";
                    keyenumerator(keyhppot);
                    potready = false;
                    tmrpot.Start();
                    lblstatus.Text = "Status: HP Pot used";
                    PauseForMilliSeconds(400);
                }
            }
        }
        public void useoocheal()
        {
            double hp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
            double mp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;

            if (ooctype == "Heal")
            {
                if (hp < oochealper && tar.Name == "")
                {
                    lblstatus.Text = "Status: Using OOCHeal";
                    keyenumerator(keyooch);
                    lblstatus.Text = "Status: OOCHeal used";
                    PauseForMilliSeconds(4000);
                }
            }
            if (ooctype == "Mana")
            {
                if (mp < oochealper && tar.Name == "")
                {
                    lblstatus.Text = "Status: Using OOCMana";
                    keyenumerator(keyooch);
                    lblstatus.Text = "Status: OOCMana used";
                    PauseForMilliSeconds(4000);
                }
            }
        }
        public void useheal()
        {

            if (pc.Health == 0) { stop(); return; }
            if (ishealer == true)
            {
                if (((Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) < healhp)
                {
                    lblstatus.Text = "Status: Using Heal";
                    keyenumerator(keyheal);
                    lblstatus.Text = "Status:Heal used";
                    PauseForMilliSeconds(200);

                }
            }
        }
        public void usemanapot()
        {
            if (potready == true){
                if (((Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100) < potmp)
                {
                    lblstatus.Text = "Status: Using Mana Pot";
                    keyenumerator(keyhppot);
                    potready = false;
                    tmrpot.Start();
                    lblstatus.Text = "Status: Mana Pot used";
                    PauseForMilliSeconds(300);
                }
            }
        }

        private void displayrange()
        {
            double distance = 0;
            if (tar.Name != "")
            {
                distance = pc.Distance3D(tar) - 2;
                lbldistance.Text = "Range: " + string.Format("{0:f2}", distance);
            }
        }

        private void mainattackloop()
        {
            int firstattack = 0;
            if (tar.TargetID != pc.ID)
            {
                do
                {
                    foreach (string item in attacks)
                    {
                        firstattack++;


                        if (tar.Stance == eStance.Dead) return;
                        if (tar.Name == "") return; //exits the attack loop
                        Entity targetstar = new Entity(tar.PtrTarget);

                        if (tar.TargetID != pc.ID && targetstar.Type == eType.Player) //if mob doesnt have me targeted
                        {
                            lblstatus.Text = "Status: Someone is already on mob";
                            return;
                        }
                        lblstatus.Text = "Status: Begin Attacking..";
                        string[] parseitem = item.Split(':');
                        string key;
                        double delayholder;
                        int delay = 0;
                        key = parseitem[0];
                        Application.DoEvents();
                        try
                        {
                            if (double.Parse(parseitem[1]) > .1)
                            {
                                delayholder = Convert.ToDouble(parseitem[1]) * 1000;
                                delay = Convert.ToInt32(delayholder);
                            }
                            else
                            {
                                delay = 100;
                            }  //no delay
                        }
                        catch (Exception) { MessageBox.Show("Error in converting attack delay"); }

                        if (tar.IsDead == true || tar.Health == 0 || btnstop.Visible == false) //tar.HasTarget == false ||
                        {
                            lblstatus.Text = "Status: Exiting attack seq..";
                            return;
                        }
                        if (tar.Health != 0) usehppot();
                        if (tar.Health != 0) usemanapot();
                        useheal();
                        if (firstattack == 1) //first attack seq, keep trying till it fires
                        {
                            do
                            {
                                lblstatus.Text = "Status: First attack sequence..";
                                keyenumerator(key);
                                PauseForMilliSeconds(delay);
                                if (btnstop.Visible == false) return;
                            } while (tar.HasTarget != true);

                        }
                        else //attack seq. continues
                        {
                            lblstatus.Text = "Status: Attacking..";
                            if (tar.IsDead == true || tar.Health == 0 || tar.HasTarget == false)
                            {
                                lblstatus.Text = "Status: Exiting attack";
                                return;
                            }
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                        }
                        PauseForMilliSeconds(100); //try to fix invalid target
                        if (btnstop.Visible == false) return;
                    }

                } while ((tar.IsDead != true || tar.Name != "") || tar.Health != 0 && btnstop.Visible == true);

                lblstatus.Text = "Status: Exiting attack..";
            }
        }

        private void getinrange()
        {
            lblstatus.Text = "Status: Getting in range..";
            double distancetotar = 0;
            distancetotar = (pc.Distance2D(tar) - 2);
            if (distancetotar >= rangedist)
            {
                keyenumerator(keyautoatk);//make bindable
                //Application.DoEvents();
                PauseForMilliSeconds(2200);
                if (btnstop.Visible == false) return;
                keyenumerator("s");
            }
            //stops the bot in range

        }

        private void attackmob()
        {
            Entity targetstar = new Entity(tar.PtrTarget);
            if (tar.Type == eType.AttackableNPC && (tar.Health == 100 || targetstar.Type == eType.FriendlyNPC) && tar.HasTarget != true)//can I fight it?
            {
                if (isranged == true) getinrange();
                lblstatus.Text = "Status: Pre-Attacking..";
                //start preattacks

                //LOOP this until mob targets me
                if (preattacks.Count != 0)
                {
                    do
                    {
                        foreach (string item in preattacks)
                        {
                            usehppot();
                            string[] parseitem = item.Split(':');
                            string key;
                            double delayholder;
                            int delay;
                            if (btnstop.Visible == false || tar.Name == "") return;
                            key = parseitem[0];
                            delayholder = Convert.ToDouble(parseitem[1]) * 1000;
                            delay = Convert.ToInt32(delayholder); //optimize this
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                        }

                    } while (tar.HasTarget != true);
                }
                mainattackloop();
            }
        }

        private void buffloop()
        {

        }

        public void getpcptr()
        {
            try
            {
                lblstatus.Text = "Status: Getting Player Entity";
                while (tar.Name != pc.Name && btnstop.Visible == true)
                {
                    keyenumerator(keyself);
                    PauseForMilliSeconds(300);
                }

                myselfptr = tar.PtrEntity; //gets pc entity ptr
                if (tar.Name == pc.Name)
                    keyenumerator("ESC");
                //PauseForMilliSeconds(300);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        public eStance findpcstance()
        {
            eStance mystance = new eStance();
            try
            {
                mystance = (eStance)Memory.ReadInt(Process.handle, (myselfptr + 0x20C)); //use pc entity
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return mystance;
        }

        private void rest()
        {
            double perresthp, perrestmana;
            try
            {
                isresting = false;
                perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;

                if (pc.Name == tar.Name) keyenumerator("ESC");
                useheal();
                useoocheal();
                if ((perresthp <= resthp || perrestmana <= restmana) && tar.Name == "")
                {

                    lblstatus.Text = "Status: Resting..";
                    isresting = true;

                    if (perresthp <= resthp)
                    {
                        while (perresthp < 99 && tar.Name == "") //HP
                        {
                            if (findpcstance() == eStance.Normal || findpcstance() == eStance.Combat) keyenumerator(keyrest);
                            perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            PauseForMilliSeconds(1500);
                        }  //bug on mana
                    }
                    if (perrestmana <= restmana)
                    {
                        while (perrestmana < 99 && tar.Name == "") //MANA
                        {
                            if (findpcstance() == eStance.Normal || findpcstance() == eStance.Combat) keyenumerator(keyrest);
                            perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            PauseForMilliSeconds(1500);
                        }  //bug on mana
                    }

                    lblstatus.Text = "Status: Done Resting..";
                    if (findpcstance() == eStance.Resting)
                    {
                        keyenumerator(keyrest);//get up
                        PauseForMilliSeconds(1300);
                    }
                    isresting = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (waypointlist.Count >= 2)
            {
                button1.Visible = false;
                btnstop.Visible = true;
                SetForegroundWindow(hwndAion);
                getpcptr();
                //PauseForMilliSeconds(30);  
                timer2.Start();
            }
            else lblstatus.Text = "Status: Needs more waypoints";
            //combat();
        }
        private void btnstop_Click_1(object sender, EventArgs e)
        {
            stop();
        }
        public void stop()
        {
            lblstatus.Text = "Status: Stopped";
            attackflag = false;
            timer2.Stop();
            btnstop.Visible = false;
            button1.Visible = true;
        }
        private void keyenumerator(string key)
        {
            if (key.Contains('\0').ToString() == "True")
            {
                key = key.Substring(0, key.LastIndexOf('\0') - 0);
            }

            switch (key)
            {
                case "NUMLOCK":
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 2, 0); //  Up 
                    break;
                case "F1":
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 2, 0); //  Up 
                    break;
                case "a":
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 0, 0); //  Down
                    PauseForMilliSeconds(20);
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 2, 0); //  Up 
                    break;
                case "b":
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 2, 0); //  Up 
                    break;
                case "c":
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 2, 0); //  Up 
                    break;
                case "d":
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 2, 0); //  Up 
                    break;
                case "e":
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 2, 0); //  Up 
                    break;
                case "f":
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 2, 0); //  Up 
                    break;
                case "g":
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 2, 0); //  Up 
                    break;
                case "h":
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 2, 0); //  Up 
                    break;
                case "i":
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 2, 0); //  Up 
                    break;
                case "j":
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 2, 0); //  Up 
                    break;
                case "k":
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 2, 0); //  Up 
                    break;
                case "l":
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 2, 0); //  Up 
                    break;
                case "m":
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 2, 0); //  Up 
                    break;
                case "n":
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 2, 0); //  Up 
                    break;
                case "o":
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 2, 0); //  Up 
                    break;
                case "p":
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 2, 0); //  Up 
                    break;
                case "q":
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 2, 0); //  Up 
                    break;
                case "r":
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 2, 0); //  Up 
                    break;
                case "s":
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 2, 0); //  Up 
                    break;
                case "t":
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 2, 0); //  Up 
                    break;
                case "u":
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 2, 0); //  Up 
                    break;
                case "v":
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 2, 0); //  Up 
                    break;
                case "w":
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0); //  Up 
                    break;
                case "x":
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 2, 0); //  Up 
                    break;
                case "y":
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 2, 0); //  Up 
                    break;
                case "z":
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 2, 0); //  Up 
                    break;
                case "ESC":
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 2, 0); //  Up 
                    break;
                case "1":

                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    break;
                case "2":

                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    break;
                case "3":

                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    break;
                case "4":

                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    break;
                case "5":

                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    break;
                case "6":

                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    break;
                case "7":

                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    break;
                case "8":

                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    break;
                case "9":

                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    break;
                case "0":

                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    break;
                case "-":
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    break;
                case "=":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case "+":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case ",":
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 2, 0); //  Up 
                    break;
                case "TAB":
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 0, 0); //  Down
                    PauseForMilliSeconds(1);
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 2, 0); //  Up 
                    break;

                case "Alt,1":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,2":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,3":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,4":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,5":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,6":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,7":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,8":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,9":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,0":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,-":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,+":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,=":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,1":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,2":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,3":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,4":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,5":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,6":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,7":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,8":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,9":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,0":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,+":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,-":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
            }
            if (keypresswindow == true)
            {
                if (listkeypress.Items.Count >= 19) listkeypress.Items.Clear();
                listkeypress.Items.Add(key + " MobHP:" + tar.Health);
            }
        }
        //public static DateTime PauseForMilliSeconds(int MilliSecondsToPauseFor)
        public void PauseForMilliSeconds(int MilliSecondsToPauseFor)
        {
            Application.DoEvents();

            Thread.Sleep(MilliSecondsToPauseFor);
            /*    if (MilliSecondsToPauseFor < 0) MilliSecondsToPauseFor = 50;
            
                    System.DateTime ThisMoment = System.DateTime.Now;
                    System.TimeSpan duration = new System.TimeSpan(0, 0, 0, 0, MilliSecondsToPauseFor);
                    System.DateTime AfterWards = ThisMoment.Add(duration);


                    while (AfterWards >= ThisMoment)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        ThisMoment = System.DateTime.Now;
                    }

            
                return System.DateTime.Now;
        
             */
        }

        private void btnignore_Click(object sender, EventArgs e)
        {
            if (tar.Name != "")
            {
                ignorelist.Add(tar.Name);
            }
        }
        private void btnuningnore_Click(object sender, EventArgs e)
        {
            if (tar.Name != "")
            {
                ignorelist.Remove(tar.Name);
            }
        }
        private void keypressWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (keypresswindow == false)
            {
                this.Width = 566;
                keypresswindow = true;
            }
            else
            {
                this.Width = 414;        //414normal
                keypresswindow = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (btnstop.Visible == false) timer2.Stop();
            if (attackflag == false)
                combat();
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
                string wtype = "";
                //if (sr.EndOfStream != true) type = sr.ReadLine().Trim(); //eat first line 
                while (sr.EndOfStream != true)
                {
                    string waypointline = sr.ReadLine();
                    if (waypointline.Contains('|'))
                    {
                        string[] coords = waypointline.Split('|');
                        cwaypoint here = new cwaypoint();
                        here.X = Convert.ToDouble(coords[1]);
                        here.Y = Convert.ToDouble(coords[0]);
                        here.Z = Convert.ToDouble(coords[2]);
                        if (wtype == "0" || wtype == "1")
                            waypointlist.Add(here);
                        if (wtype == "3")
                            deathpointlist.Add(here);
                    }
                    else
                        wtype = waypointline.Trim();
                }
                sr.Close();
                lblstatus.Text = "Status: Waypoints loaded";
            }

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
                TextWriter tw = new StreamWriter(DialogSave.FileName);

                tw.WriteLine("0");
                string awaypoint;
                foreach (cwaypoint item in waypointlist)
                {
                    awaypoint = string.Format("{0:f3}", item.Y) + "|" + string.Format("{0:f3}", item.X) + "|" + string.Format("{0:f3}", item.Z);
                    tw.WriteLine(awaypoint);
                }
                tw.Close();
                lblstatus.Text = "Status: Waypoints saved";
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
            if (autoway == true)
            {
                autoway = false;
                btnautoway.Text = "Auto Waypoints";
                timer3.Stop();
            }
            else
            {
                if (autoway == false) addwaypoint(); //first waypoint add
                autoway = true;
                btnautoway.Text = "Stop";
                timer3.Start();
            }

        }

        public void addwaypoint()
        {
            cwaypoint here = new cwaypoint();
            here.X = pc.X;
            here.Y = pc.Y;
            here.Z = pc.Z;
            if (checkBox1.Checked == false)
            {
                waypointlist.Add(here);
                lblstatus.Text = "Status: Added Waypoint " + waypointlist.Count;
            }
            else
            {
                deathpointlist.Add(here);
                lblstatus.Text = "Status: Added Death Waypoint " + deathpointlist.Count;
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
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(700);
            if (waypointlist.Count > 0) { waymovement(); };
        }

        private void combat()
        {
            if (btnstop.Visible == false) { lblstatus.Text = "Status: Stopping"; return; }
            if (pc.Health == 0) stop();
            Application.DoEvents();
            attackflag = true;
            while ((tar.IsDead == true || tar.Health == 0) && tar.Name != "") //ADD INVENTORY FULL CHECK
            {
                lblstatus.Text = "Status: Looting..:";
                PauseForMilliSeconds(1400);
                if ((tar.IsDead == true || tar.Health == 0) && tar.Name != "")
                    keyenumerator(keyloot);
                PauseForMilliSeconds(300);
                kills += 1;
                killLabel.Text = "Kills: " + kills;
            }

            if (btnstop.Visible == true) rest();//REST
            //tar.TargetID != pc.ID
            if ((tar.IsDead != true && tar.Name != "") || tar.TargetID == pc.ID) //ADD DETECTION/FIGHT
            {
                int tarlevel1;
                tarlevel1 = Convert.ToInt32(tar.Level);
                if (tarlevel1 > ignorelevel)
                {
                    lblstatus.Text = "Status: Attacking ADD..";
                    mainattackloop();
                    PauseForMilliSeconds(200);
                }
            }

            while ((tar.IsDead == true || tar.Health == 0) && tar.Name != "") //ADD INVENTORY FULL CHECK
            {
                lblstatus.Text = "Status: Looting..:";
                PauseForMilliSeconds(1400);
                if ((tar.IsDead == true || tar.Health == 0) && tar.Name != "")
                    keyenumerator(keyloot);
                PauseForMilliSeconds(300);
                kills += 1;
                killLabel.Text = "Kills: " + kills;
            }
            //if (btnstop.Visible == true) buffloop();//ACTIVATE BUFFS HERE
            if (btnstop.Visible == true) rest();//REST

            if (btnstop.Visible == true && tar.TargetID != pc.ID) findmob(); //SEARCH FOR MOB

            if (btnstop.Visible == true && tar.Name != "")
            {
                int tarlevel;
                tarlevel = Convert.ToInt32(tar.Level);
                if (tarlevel > ignorelevel)
                { attackmob(); }
            }
            attackflag = false;

        }

        private void findmob()
        {
            bool hasplayer;
            if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true;
            else hasplayer = false;

            while (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name)))
            {
                if (isresting == false)
                {
                    lblstatus.Text = "Status: Searching..";
                    if (findpcstance() == eStance.Resting) keyenumerator("w"); //get up

                    if (pc.Name == tar.Name) keyenumerator("ESC");
                    Application.DoEvents();
                    if (tar.TargetID == pc.ID && tar.ID != pc.ID) return; //another player attacks it

                    if (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name)))
                    {
                        tabfind++;
                        if (tabfind >= 3)
                        {
                            keyenumerator(keytarget); //tab
                            tabfind = 0;
                        }
                    }
                    waymovement();
                    if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true; else hasplayer = false;
                    if (btnstop.Visible == false) return;
                }
            }
        }//FINDMOB

        public void waymovement()
        {

            if (movecounter <= waypointlist.Count - 1) awaypoint = waypointlist[movecounter];

            if (pc.Distance3D((float)awaypoint.X, (float)awaypoint.Y, (float)awaypoint.Z) < 5) //hit waypoint
            {
                if ((movecounter == 0) && forward == false)
                {
                    forward = true;
                } //switch from backwards to forward
                if ((movecounter == waypointlist.Count - 1) && forward == true)
                {
                    forward = false;
                }
                if (forward == true) movecounter++;
                if (forward == false)
                {
                    movecounter--;
                }
                lblstatus.Text = "Status: Going to waypoint: " + movecounter;
            }

            //Application.DoEvents();

            if (awaypoint.Face(pc.Y, pc.X) > 0)
            {
                if ((pc.Rotation > (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) - 5)) //left within 1.5 degrees
                {
                    do //LEFT
                    {
                        keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 0, 0); //  Down
                        PauseForMilliSeconds(10);
                        keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 2, 0); //  Up 
                    } while ((pc.Rotation >= (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation <= (awaypoint.Face(pc.Y, pc.X)) - 5));
                }
                else if (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 5)) //right within 1.5 degrees
                {
                    do //RIGHT
                    {
                        keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 0, 0); //  Down
                        PauseForMilliSeconds(10);
                        keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 2, 0); //  Up 
                    } while (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 5));
                }
            }//end of positive rot point

            else //NEG POINT
            {
                if ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 5) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180)) //left within 1.5 degrees
                {
                    do //RIGHT
                    {
                        keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 0, 0); //  Down
                        PauseForMilliSeconds(10);
                        keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 2, 0); //  Up 
                    } while ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 5) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180));
                }
                else if (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 5) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180))) //right within 1.5 degrees
                {
                    do //LEFT
                    {
                        keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 0, 0); //  Down
                        PauseForMilliSeconds(10);
                        keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 2, 0); //  Up 
                    } while (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 5) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180)));
                }
            }
            Application.DoEvents();
            keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 0, 0); //  Down
            PauseForMilliSeconds(700);
            keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0); //  Up 

            //}
        }

        private void tmrstuck_Tick(object sender, EventArgs e)
        {

        }

        private void waypointEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            waypointeditor f3 = new waypointeditor();
            f3.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tmrpot_Tick(object sender, EventArgs e)
        {
            potready = true;
            tmrpot.Stop();
        }

    }//FORMCLASS

    public class cwaypoint
    {
        public double X;
        public double Y;
        public double Z;
        public void Clear()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public double Face(double pcy, double pcx)
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