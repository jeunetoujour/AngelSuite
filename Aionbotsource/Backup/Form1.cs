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
using MemoryLib;
//using TakeOver;
using Ini;

/*//Initially populate your ability list.
var abilities = new AbilityList();
abilities.Update();


//This is how long this length takes to cooldown when used.
abilities["Skill Name"].CooldownLength;
//This is the system Tick time that this is available at.
//Compare against:
[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
static extern int GetTickCount();
//Before accessing an when this is available, it would be a good idea to update it.
abilities["Skill Name"].Update();
abilities["Skill Name"].AvailableAtTick;
var abilityNotInCooldown = abilities["Skill Name"].AvailableAtTick < GetTickCount();*/


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
        public Player pc;
        public Target tar;

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
        int healhp;
        double healdelay;
        double healcool;
        int pothp;
        int potmp;
        int ignorelevel;
        int ignoretime;
        int oochealper;
        int stuckcounter;
        int shutofftime;
        int deathresttime;

        string ooctype;
        bool ishealer;
        bool isranged;
        bool isresting = false;
        bool potready = true;
        bool healready = true;
        bool istabbing = false;
        bool invcheck = true;
        public bool savelog = false;

        public bool antistuck = true;
        //bool largeangle = false;
        int rangedist;
        List<string> preattacks = new List<string>();
        List<string> attacks = new List<string>();
        List<string> buffs = new List<string>();
        List<string> buffbtns = new List<string>();
        List<DateTime> bufftimes = new List<DateTime>();
        List<TimeSpan> buffdelays = new List<TimeSpan>();
        int numbuffs = 0;

        int combatcounting = 0;
        int diedcount = 0;

        List<string> ignorelist = new List<string>();

        public List<cwaypoint> waypointlist = new List<cwaypoint>();
        public List<cwaypoint> deathpointlist = new List<cwaypoint>();
        public cwaypoint previouspoint = new cwaypoint();
        public int movecounter = 0;
        public int deathcounter = 0;
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

        //bool cantattack = false;

        int lootcounter = 0;
        bool attackflag;
        public bool deathrun = false;
        bool autoway;
        bool ismoving = false;
        bool forward = true;
        public TextWriter tw; 

        public Form1()
        {
            try
            {
                
                InitializeComponent();
                bool progname = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToString().Contains("Angel");
                Application.VisualStyleState = VisualStyleState.NoneEnabled;

                if (progname)
                {
                    System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                    process1.StartInfo.FileName = "Launcher.exe";
                    process1.Start();
                    Environment.Exit(1);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error starting Angelbot!! Exiting");
                Environment.Exit(1);
            }
        }

        private void clearAll()
        {
            pc = new Player();
            tar = new Target();
            pc.Updatenamelvl();
            pc.Updateafterkill();
            pc.Update();
            PauseForMilliSeconds(50);
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
            //getpcptr();
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

                label1.Text = "X: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", pcx);
                label2.Text = "Y: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", pcy);
                label3.Text = "Z: " + string.Format(CultureInfo.InvariantCulture, "{0:f3}", (pcz + 1.1)); //note the 1.1 added
                lblrot.Text = pc.Rotation.ToString();

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
                if (elapsed.TotalSeconds > 2 && savelog == true) tw.Flush();

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
                    if (xpperhr < 0) { clearAll(); if (savelog == true) tw.WriteLine(DateTime.Now + " Leveled!"); }
                    expHRLabel.Text = "Exp/Hr: " + string.Format("{0:n2}", xpperhr) + "k";
                    double leveltime = (Convert.ToDouble(pcMaxXP, CultureInfo.InvariantCulture) - Convert.ToDouble(pcXP, CultureInfo.InvariantCulture)) / (Convert.ToDouble(xpperhr, CultureInfo.InvariantCulture) * 1000);
                    lbltimelvl.Text = "Level In: " + string.Format("{0:n2}", leveltime) + "h";
                    kinahLabel.Text = "Kinah " + (pc.Kinah - kinahStart);
                    killLabel.Text = "Kills: " + kills;
                    
                    label6.Text = "Inv: " + pc.GetUsedSpace() + "/" + pc.GetMaxCubes();
                    label9.Text = "Deaths: " + diedcount;
                }
            }
        }

        private void resetAll_Click(object sender, EventArgs e)
        {
            clearAll();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Reset pressed");
            //stopmovement = true;
        }

        private void Form1_OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            stop();
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
            buffdelays = new List<TimeSpan>();
            bufftimes = new List<DateTime>();
            buffbtns = new List<string>();
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
                //character.Add(key, IniSection.ValueCollection);
                character[section][key] = template[section][key];
                character.Save();
                return template[section][key];
            }
            else
            {
                MessageBox.Show("You have no template.ini file or a corrupted copy! Missing in section: " + section + " key: "+ key);
                return "";
            }
        }

        public void loadsettings()
        {
            clearsettings();
            pc.Updatenamelvl();
            PauseForMilliSeconds(200);
            
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + pc.Name + ".ini");
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
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + pc.Name + ".ini");
                ini.Load();
            }
            try
            {
                resthp = Int32.Parse(GetSetting(ini, initemplate, "limits", "RestHP"));
                restmana = Int32.Parse(GetSetting(ini, initemplate, "limits", "RestMana"));
                healhp = Int32.Parse(GetSetting(ini, initemplate, "limits", "HealHP"));
                healdelay = Double.Parse(GetSetting(ini, initemplate, "limits", "HealDelay"), CultureInfo.InvariantCulture);
                healcool = Double.Parse(GetSetting(ini, initemplate, "limits", "HealCD"), CultureInfo.InvariantCulture);
                tmrheal.Interval = Convert.ToInt32(healcool * 1000);
                pothp = Int32.Parse(GetSetting(ini, initemplate, "limits", "PotHP").Trim());
                potmp = Int32.Parse(GetSetting(ini, initemplate, "limits", "PotMP").TrimEnd(' '));
                ignorelevel = Int32.Parse(GetSetting(ini, initemplate, "limits", "IgnoreLevel").TrimEnd(' '));
                ignoretime = Int32.Parse(GetSetting(ini, initemplate, "limits", "IgnoreTime").TrimEnd(' '));
                tmrunstuck.Interval = ignoretime * 60000;
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

                string pretemp = GetSetting(ini, initemplate, "preattacks", "PreAttacks");
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
                string attacktemp = GetSetting(ini, initemplate, "attacks", "Attacks");
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
                string bufftemp = GetSetting(ini, initemplate, "buffs", "Buffs");
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
                keyloot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "LootBtn"));
                keyrest = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "RestBtn"));
                keyhppot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Healthpot"));
                keymppot = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Manapot"));
                keytarget = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "TargetBtn"));
                keyself = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "SelfTarget"));
                keyturn = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "TurnAround"));
                keyautoatk = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Autoattack"));
                keyheal = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "Heal"));
                keyooch = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "OOCH"));
                keystrafeL = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "StrafeL"));
                keystrafeR = Convert.ToString(GetSetting(ini, initemplate, "keybinds", "StrafeR"));
                
            }
            catch (Exception e)
            {
                //antistuck = true;
                //if (savelog == true) tw.WriteLine(DateTime.Now + "  Problem with ini settings");
                MessageBox.Show("Problem with your ini file! " + e);
            }
            if (savelog == true)
            {
                tw = new StreamWriter(Environment.CurrentDirectory + "\\" + pc.Name + ".log");
                tw.WriteLine(DateTime.Now + " Loaded ini settings good");
            }
            buffload();
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
            this.Opacity = .9;
            loadsettings();
            keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0);
            keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0);
        }

        public void usehppot()
        {
            try
            {
                if (potready == true)
                {
                    if (((Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) < pothp)
                    {
                        int prevhealth = pc.Health;
                        //float currhealth = (Convert.ToSingle(pc.Health) / Convert.ToSingle(pc.MaxHealth)) * 100;
                        
                        while (prevhealth >= pc.Health)
                        {
                            lblstatus.Text = "Status: Using HP Pot";
                            PauseForMilliSeconds(600);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Using Pot HP:" + pc.Health + "/" + pc.MaxHealth);
                            keyenumerator(keyhppot);
                            Application.DoEvents();
                            lblstatus.Text = "Status: HP Pot used";
                            PauseForMilliSeconds(200);
                            if (pc.Health == 0) return;
                            if (prevhealth > pc.Health) prevhealth = pc.Health;
                            
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
                    while (prevmp >= pc.MP)
                    {
                        lblstatus.Text = "Status: Using Mana Pot";
                        PauseForMilliSeconds(600);
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Using Mana Pot MP: " + pc.MP + "/" + pc.MaxMP);
                        keyenumerator(keymppot);
                        
                        lblstatus.Text = "Status: Mana Pot used";
                        PauseForMilliSeconds(200);
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
                }
            }
        }
        public void useheal()
        {

            if (pc.Health == 0) { return; }

            if (ishealer == true && healready == true && pc.IsCasting == false)
            {
                if (((Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100) < healhp)
                {
                    int prevhealth = pc.Health;
                    while (prevhealth >= pc.Health)
                    {
                        lblstatus.Text = "Status: Using Heal";
                        PauseForMilliSeconds(350);
                        keyenumerator(keyheal);
                        PauseForMilliSeconds(50);
                        if (pc.Health == 0) return;
                        if (pc.IsCasting == true)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Casting Heal HP: " + pc.Health + "/" + pc.MaxHealth);
                            PauseForMilliSeconds(Convert.ToInt32(healdelay * 1000));
                        }
                            if (prevhealth > pc.Health) prevhealth = pc.Health;
                    }
                    healready = false;
                    //Application.DoEvents;
                    tmrheal.Start(); //cooldown
                    //Application.DoEvents; 
                    lblstatus.Text = "Status:Heal used";
                    //PauseForMilliSeconds(Convert.ToInt32(healdelay * 1000));
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Casting Heal HP: " + pc.Health + "/" + pc.MaxHealth);
                }
            }
        }

        private void displayrange()
        {
            double distance = 0;
            if (tar.Name != "")
            {
                distance = pc.Distance3D(tar);
                lbldistance.Text = "Range: " + string.Format("{0:f2}", distance + 1);
            }
        }

        private void mainattackloop()
        {
            //int firstattack = 0;
            bool started = false;
            //Entity targetstar1 = new Entity(tar.PtrTarget);

            if (pc.TargetID != pc.ID) //&& (targetstar1.ID == pc.ID || targetstar1.Type == eType.FriendlyNPC || tar.HasTarget != true)) //selftarget
            {
                while ((tar.IsDead != true || tar.Name != "") && tar.Health != 0 && btnstop.Visible == true)
                {
                    lblstatus.Text = "Status: Begin Attacking..";
                    tmrtabby.Stop();
                    if (findpcstance() == eStance.Resting)
                    {
                        keyenumerator("UP");//get up
                        PauseForMilliSeconds(1000);
                    }

                    foreach (string item in attacks)
                    {

                        //firstattack++;
                        if (tar.Stance == eStance.Dead) return;
                        if (tar.Name == "") return; //exits the attack loop

                        try
                        {
                            Entity targetstar = new Entity(tar.PtrTarget);
                            if (targetstar.ID != pc.ID && targetstar.Type == eType.Player && tar.Health > 0) //if mob doesnt have me targeted
                            {
                                lblstatus.Text = "Status: Someone is already on mob";
                                ignorelist.Add(tar.ID.ToString());
                                break;
                            }
                        }
                        catch (Exception) { MessageBox.Show("Error: issue with someone on mob in attack"); }
                        string[] parseitem = item.Split(':');
                        string key;
                        double delayholder;
                        int delay = 0;
                        key = parseitem[0];
                        Application.DoEvents();
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
                        catch (Exception e) { if (savelog == true) tw.WriteLine(DateTime.Now + " Error convert attack delay"); MessageBox.Show("Error in converting attack delay. " + e); }

                        if (tar.IsDead == true || tar.Health == 0 || btnstop.Visible == false) //tar.HasTarget == false ||
                        {
                            lblstatus.Text = "Status: Exiting attack seq..";
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack seq..");
                            return;
                        }

                        if (pc.Health != 0) usehppot();
                        if (pc.Health != 0) usemanapot();
                        if (pc.Health == 0) { return; } //you clicked stop button
                        
                        if (ignorelist.Contains(tar.ID.ToString()) == true && tar.TargetID == pc.ID)
                        {
                            if (ignorelist.Count != 0)
                                if (IsNumber(tar.ID.ToString()) == true)
                                {
                                    if (savelog == true) tw.WriteLine(DateTime.Now + " Unignoring in attack: " + tar.ID.ToString());
                                    ignorelist.Remove(tar.ID.ToString());
                                }

                        } //WTF was this for anyways?

                        useheal();
                        Application.DoEvents();
                        if (started == false) //first attack seq, keep trying till it fires
                        {
                            if (antistuck == true)
                            {
                                stuckcounter = 0;
                                returncounter = 0;
                                tmrstuck.Start();
                            }
                            Entity targetstar1 = new Entity(tar.PtrTarget);
                            lblstatus.Text = "Status: First attack sequence..";
                            started = true;
                            do
                            {
                                keyenumerator(key);
                                PauseForMilliSeconds(delay);
                                if (savelog == true) tw.WriteLine(DateTime.Now + " First attack tried");
                                if (ignorelist.Contains(tar.ID.ToString()) == true && targetstar1.ID != pc.ID) { tmrstuck.Stop(); return; }
                                if (btnstop.Visible == false) return;
                            } while (tar.HasTarget != true);

                            tmrstuck.Stop();
                        }
                        else //attack seq. continues
                        {
                            stuckcounter = 0;
                            returncounter = 0;
                            tmrstuck.Stop();
                            lblstatus.Text = "Status: Attacking..";
                            if (tar.IsDead == true || tar.Health == 0 || tar.HasTarget == false)
                            {
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack");
                                lblstatus.Text = "Status: Exiting attack";
                                return;
                            }
                            if (tar.Health != 0) usehppot();
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Attack key pressed: " + key);
                        }
                        tmrstuck.Stop();
                        PauseForMilliSeconds(100); //try to fix invalid target
                        if (btnstop.Visible == false) break;
                    }

                }
                if (savelog == true) tw.WriteLine(DateTime.Now + " Exiting attack loop");
                lblstatus.Text = "Status: Exiting attack..";
            }
        }

        private void getinrange()
        {
            /*lblstatus.Text = "Status: Getting in range..";
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
            keyenumerator(keyautoatk);//make bindable
            
            while (distancetotar >= rangedist)
            {
                distancetotar = (float)(pc.Distance2D(tar) + 1);
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

        private void attackmob()
        {
            Entity targetstar = new Entity(tar.PtrTarget);

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
                
                if (isranged == true) getinrange();
                lblstatus.Text = "Status: Pre-Attacking..";
                //start preattacks

                if (findpcstance() == eStance.Resting)
                {
                    keyenumerator("UP");//get up
                    PauseForMilliSeconds(1100);
                }
                //LOOP this until mob targets me
                if (preattacks.Count != 0)
                {
                    if (antistuck == true)
                    {
                        stuckcounter = 0;
                        returncounter = 0;
                        tmrstuck.Start();
                    }
                    while (tar.HasTarget != true)
                    {
                        foreach (string item in preattacks)
                        {
                            //if ((tar.TargetID != pc.ID || tar.TargetID != tar.ID) && targetstar.Type == eType.Player) return;
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
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Preattacking with: " + key);
                            keyenumerator(key);
                            PauseForMilliSeconds(delay);
                        }

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

        private void buffload()
        {
            int counter = 0;

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

        public bool adddetect()
        {
            EntityList elist = new EntityList();
            elist.Update();
            
            foreach (Entity ents in elist)
            {
                if (ents.Type == eType.AttackableNPC)
                {
                    if (ents.TargetID == pc.ID && ents.Health > 0)
                    {
                        lblstatus.Text = "Status: ADD found in ADD detection: " + ents.Name;
                        if (savelog == true) tw.WriteLine(DateTime.Now + " ADD found in ADD detection: " + ents.Name);
                        return true; //if a hostile has you targeted
                    }
                }
            }
            return false;
        }

        public void getpcptr()
        {
            EntityList elist = new EntityList();
            elist.Update();
            lblstatus.Text = "Status: Getting Player Entity";
            Application.DoEvents();
            foreach (Entity thing in elist)
            {
                if (thing.Name == pc.Name)
                {
                    if (thing._PtrEntity != 0)
                    {
                        myselfptr = thing._PtrEntity;
                    }
                    else myselfptr = thing.PtrEntity;
                }
            }
            lblstatus.Text = "Status: Got Player Entity";
            if (savelog == true) tw.WriteLine(DateTime.Now + " Got player entity");

            Application.DoEvents();
            PauseForMilliSeconds(800);
        }

        public eStance findpcstance()
        {
            eStance mystance = new eStance();
            try
            {

                mystance = (eStance)Memory.ReadInt(Process.handle, (uint)(myselfptr + 0x20C)); //use pc entity
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with pcstance." + e);
            }
            return mystance;
        }

        private void rest()
        {
            double perresthp, perrestmana;
            try
            {
                isresting = false;
                if (adddetect() == true) return; //Found add targeting you
                if (pc.Health > 0) useheal();
                if (pc.Health > 0) useoocheal();
                perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;

                if (pc.Name == tar.Name) keyenumerator("ESC");

                if ((perresthp <= resthp || perrestmana <= restmana) && tar.Name == "")
                {

                    lblstatus.Text = "Status: Resting..";
                    isresting = true;

                    if (perresthp <= resthp)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " HP Resting");
                        
                        while (perresthp < 99 && (tar.Name == "" || tar.Type == eType.Player)) //HP
                        {
                            if (findpcstance() == eStance.Normal || findpcstance() == eStance.Combat)
                                keyenumerator(keyrest);
                            perresthp = (Convert.ToDouble(pc.Health) / Convert.ToDouble(pc.MaxHealth)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            if (pc.Health == 0) { isresting = false; return; } //you clicked stop button

                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            PauseForMilliSeconds(600);

                        }
                    }
                    if (perrestmana <= restmana)
                    {
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Mana Resting");
                        
                        while (perrestmana < 99 && (tar.Name == "" || tar.Type == eType.Player)) //MANA
                        {
                            if (findpcstance() == eStance.Normal || findpcstance() == eStance.Combat) keyenumerator(keyrest);
                            perrestmana = (Convert.ToDouble(pc.MP) / Convert.ToDouble(pc.MaxMP)) * 100;
                            if (tar.TargetID == pc.ID && tar.ID != pc.ID) { isresting = false; lblstatus.Text = "Status: Rest Interrupted"; return; }
                            if (btnstop.Visible == false || tar.Name != "") { isresting = false; return; } //you clicked stop button
                            if (pc.Health == 0) { isresting = false; return; } //you clicked stop button
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
                            PauseForMilliSeconds(500);
                            Application.DoEvents();
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with rest. " + e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (waypointlist.Count >= 2)
            {
                btnwaypoint.Enabled = false;
                btnautoway.Enabled = false;
                istabbing = false;
                SetForegroundWindow(hwndAion);
                getpcptr();
                PauseForMilliSeconds(50);
                button1.Visible = false;
                btnstop.Visible = true;
                deathrun = false;
                deathcounter = 0;
                istabbing = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Start Pressed");

                Application.DoEvents();
                //timer2.Start();
                if (attackflag == false && btnstop.Visible == true)
                {
                    if(shutofftime > 0) tmrshutoff.Start();
                    combat();
                }
            }
            else lblstatus.Text = "Status: Needs more waypoints";
            //combat();
        }
        private void btnstop_Click_1(object sender, EventArgs e)
        {
            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(100);
            if (shutofftime > 0) tmrshutoff.Stop();
            btnwaypoint.Enabled = true;
            btnautoway.Enabled = true;
            tmrstuck.Stop();
            stop();
            if (savelog == true) tw.WriteLine(DateTime.Now + " Stop pressed");
            
            istabbing = false;
        }

        public void deathsystem()
        {
            if (pc.Health == 0)
            {
                //button1.Visible = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " You died like a noob");
                
                lblstatus.Text = "Status: Resurrecting";
                resurrect();
                if (savelog == true) tw.WriteLine(DateTime.Now + " Resurrect finished");

                //btndeathstop.Visible = true;
                if (deathpointlist.Count >= 1)
                {
                    
                    getpcptr();

                    lblstatus.Text = "Status: Resting" + deathresttime + "sec";
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Resting for " + deathresttime + "seconds");

                    PauseForMilliSeconds(2000);
                    Application.DoEvents();
                    if (btnstop.Visible == false) { button1.Visible = true; deathrun = false; }
                    if (findpcstance() == eStance.Normal)
                    {
                        PauseForMilliSeconds(2000);
                        keyenumerator(keyrest);

                    }

                    int sitcounter = 0;
                    while (sitcounter < deathresttime)
                    {
                        PauseForMilliSeconds(1000);
                        Application.DoEvents();
                        if (btnstop.Visible == false) break;
                        sitcounter++;
                    }


                    keyenumerator("UP"); //get up
                    if (btnstop.Visible != false)  lblstatus.Text = "Status: Buffing"; 
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

        public void stop()
        {
            lblstatus.Text = "Status: Stopped";
            if (savelog == true) tw.WriteLine(DateTime.Now + " Stop function called");
            
            attackflag = false;
            //timer2.Stop(); //combattimer2.Stop();
            tmrtabby.Stop();
            tmrstuck.Stop();
            tmrpot.Stop();
            tmrheal.Stop();

            btnstop.Visible = false;
            button1.Visible = true;
        }

        public void keyenumerator(string key)
        {
            if (key.Contains('\0').ToString() == "True")
            {
                key = key.Substring(0, key.LastIndexOf('\0') - 0);
            }
            if (key.Length == 1) key = key.ToLower();

            switch (key)
            {
                case "DOWN":
                    keybd_event((int)Keys.Down, (byte)MapVirtualKey((int)Keys.Down, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Down, (byte)MapVirtualKey((int)Keys.Down, 0), 2, 0); //  Up 
                    break;
                case "SPACE":
                    keybd_event((int)Keys.Space, (byte)MapVirtualKey((int)Keys.Space, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Space, (byte)MapVirtualKey((int)Keys.Space, 0), 2, 0); //  Up 
                    break;
                case "NUMLOCK":
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.NumLock, (byte)MapVirtualKey((int)Keys.NumLock, 0), 2, 0); //  Up 
                    break;
                case "F1":
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F1, (byte)MapVirtualKey((int)Keys.F1, 0), 2, 0); //  Up 
                    break;
                case "F2":
                    keybd_event((int)Keys.F2, (byte)MapVirtualKey((int)Keys.F2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F2, (byte)MapVirtualKey((int)Keys.F2, 0), 2, 0); //  Up 
                    break;
                case "F3":
                    keybd_event((int)Keys.F3, (byte)MapVirtualKey((int)Keys.F3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F3, (byte)MapVirtualKey((int)Keys.F3, 0), 2, 0); //  Up 
                    break;
                case "F4":
                    keybd_event((int)Keys.F4, (byte)MapVirtualKey((int)Keys.F4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F4, (byte)MapVirtualKey((int)Keys.F4, 0), 2, 0); //  Up 
                    break;
                case "F5":
                    keybd_event((int)Keys.F5, (byte)MapVirtualKey((int)Keys.F5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F5, (byte)MapVirtualKey((int)Keys.F5, 0), 2, 0); //  Up 
                    break;
                case "F6":
                    keybd_event((int)Keys.F6, (byte)MapVirtualKey((int)Keys.F6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F6, (byte)MapVirtualKey((int)Keys.F6, 0), 2, 0); //  Up 
                    break;
                case "F7":
                    keybd_event((int)Keys.F7, (byte)MapVirtualKey((int)Keys.F7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F7, (byte)MapVirtualKey((int)Keys.F7, 0), 2, 0); //  Up 
                    break;
                case "F8":
                    keybd_event((int)Keys.F8, (byte)MapVirtualKey((int)Keys.F8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F8, (byte)MapVirtualKey((int)Keys.F8, 0), 2, 0); //  Up 
                    break;
                case "F9":
                    keybd_event((int)Keys.F9, (byte)MapVirtualKey((int)Keys.F9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F9, (byte)MapVirtualKey((int)Keys.F9, 0), 2, 0); //  Up 
                    break;
                case "F10":
                    keybd_event((int)Keys.F10, (byte)MapVirtualKey((int)Keys.F10, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F10, (byte)MapVirtualKey((int)Keys.F10, 0), 2, 0); //  Up 
                    break;
                case "F11":
                    keybd_event((int)Keys.F11, (byte)MapVirtualKey((int)Keys.F11, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F11, (byte)MapVirtualKey((int)Keys.F11, 0), 2, 0); //  Up 
                    break;
                case "F12":
                    keybd_event((int)Keys.F12, (byte)MapVirtualKey((int)Keys.F12, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F12, (byte)MapVirtualKey((int)Keys.F12, 0), 2, 0); //  Up 
                    break;
                case "a":
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 0, 0); //  Down
                    PauseForMilliSeconds(20);
                    keybd_event((int)Keys.A, (byte)MapVirtualKey((int)Keys.A, 0), 2, 0); //  Up 
                    break;
                case "b":
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.B, (byte)MapVirtualKey((int)Keys.B, 0), 2, 0); //  Up 
                    break;
                case "c":
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.C, (byte)MapVirtualKey((int)Keys.C, 0), 2, 0); //  Up 
                    break;
                case "d":
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D, (byte)MapVirtualKey((int)Keys.D, 0), 2, 0); //  Up 
                    break;
                case "e":
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.E, (byte)MapVirtualKey((int)Keys.E, 0), 2, 0); //  Up 
                    break;
                case "f":
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.F, (byte)MapVirtualKey((int)Keys.F, 0), 2, 0); //  Up 
                    break;
                case "g":
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.G, (byte)MapVirtualKey((int)Keys.G, 0), 2, 0); //  Up 
                    break;
                case "h":
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.H, (byte)MapVirtualKey((int)Keys.H, 0), 2, 0); //  Up 
                    break;
                case "i":
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.I, (byte)MapVirtualKey((int)Keys.I, 0), 2, 0); //  Up 
                    break;
                case "j":
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.J, (byte)MapVirtualKey((int)Keys.J, 0), 2, 0); //  Up 
                    break;
                case "k":
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.K, (byte)MapVirtualKey((int)Keys.K, 0), 2, 0); //  Up 
                    break;
                case "l":
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.L, (byte)MapVirtualKey((int)Keys.L, 0), 2, 0); //  Up 
                    break;
                case "m":
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.M, (byte)MapVirtualKey((int)Keys.M, 0), 2, 0); //  Up 
                    break;
                case "n":
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.N, (byte)MapVirtualKey((int)Keys.N, 0), 2, 0); //  Up 
                    break;
                case "o":
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.O, (byte)MapVirtualKey((int)Keys.O, 0), 2, 0); //  Up 
                    break;
                case "p":
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.P, (byte)MapVirtualKey((int)Keys.P, 0), 2, 0); //  Up 
                    break;
                case "q":
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Q, (byte)MapVirtualKey((int)Keys.Q, 0), 2, 0); //  Up 
                    break;
                case "r":
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.R, (byte)MapVirtualKey((int)Keys.R, 0), 2, 0); //  Up 
                    break;
                case "s":
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.S, (byte)MapVirtualKey((int)Keys.S, 0), 2, 0); //  Up 
                    break;
                case "t":
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.T, (byte)MapVirtualKey((int)Keys.T, 0), 2, 0); //  Up 
                    break;
                case "u":
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.U, (byte)MapVirtualKey((int)Keys.U, 0), 2, 0); //  Up 
                    break;
                case "v":
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.V, (byte)MapVirtualKey((int)Keys.V, 0), 2, 0); //  Up 
                    break;
                case "w":
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0); //  Up 
                    break;
                case "x":
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.X, (byte)MapVirtualKey((int)Keys.X, 0), 2, 0); //  Up 
                    break;
                case "y":
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Y, (byte)MapVirtualKey((int)Keys.Y, 0), 2, 0); //  Up 
                    break;
                case "z":
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Z, (byte)MapVirtualKey((int)Keys.Z, 0), 2, 0); //  Up 
                    break;
                case "ESC":
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 0, 0); //  Down
                    PauseForMilliSeconds(250);
                    keybd_event((int)Keys.Escape, (byte)MapVirtualKey((int)Keys.Escape, 0), 2, 0); //  Up 
                    break;
                case "1":

                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    break;
                case "2":

                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    break;
                case "3":

                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    break;
                case "4":

                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    break;
                case "5":

                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    break;
                case "6":

                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    break;
                case "7":

                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    break;
                case "8":

                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    break;
                case "9":

                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    break;
                case "0":

                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    break;
                case "-":
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    break;
                case "=":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case "+":
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    break;
                case ",":
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 0, 0); //  Down
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemcomma, (byte)MapVirtualKey((int)Keys.Oemcomma, 0), 2, 0); //  Up 
                    break;
                case "TAB":
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.Tab, (byte)MapVirtualKey((int)Keys.Tab, 0), 2, 0); //  Up 
                    break;
                case "UP":
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0); //  Down
                    PauseForMilliSeconds(100);
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0); //  Up 
                    break;
                case "Alt,1":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(1);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,2":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(1);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,3":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,4":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,5":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,6":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,7":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,8":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,9":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Menu
                    break;
                case "Alt,0":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  DownPauseForMilliSeconds(10);
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,-":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,+":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Alt,=":

                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 0, 0); //  Menu
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,1":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D1, (byte)MapVirtualKey((int)Keys.D1, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,2":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D2, (byte)MapVirtualKey((int)Keys.D2, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,3":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  Alt
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D3, (byte)MapVirtualKey((int)Keys.D3, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,4":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D4, (byte)MapVirtualKey((int)Keys.D4, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,5":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D5, (byte)MapVirtualKey((int)Keys.D5, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,6":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D6, (byte)MapVirtualKey((int)Keys.D6, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,7":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D7, (byte)MapVirtualKey((int)Keys.D7, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,8":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D8, (byte)MapVirtualKey((int)Keys.D8, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,9":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D9, (byte)MapVirtualKey((int)Keys.D9, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  ControlKey
                    break;
                case "Ctrl,0":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.D0, (byte)MapVirtualKey((int)Keys.D0, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,+":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.Oemplus, (byte)MapVirtualKey((int)Keys.Oemplus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); //  Alt
                    break;
                case "Ctrl,-":
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 0, 0); //  ControlKey
                    PauseForMilliSeconds(50);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 0, 0); //  Down
                    PauseForMilliSeconds(10);
                    keybd_event((int)Keys.OemMinus, (byte)MapVirtualKey((int)Keys.OemMinus, 0), 2, 0); //  Up 
                    PauseForMilliSeconds(50);
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
                this.Width = 566;
                keypresswindow = true;
            }
            else
            {
                this.Width = 414;        //414normal
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
                        if (wtype == "0" || wtype == "1")
                            waypointlist.Add(here);
                        if (wtype == "3")
                            deathpointlist.Add(here);

                    }
                    if (IsNumber(waypointline) && waypointline.Length == 1)
                        wtype = waypointline.Trim();
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

                sw.WriteLine("0");
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
            if (autoway == true)
            {
                autoway = false;
                btnautoway.Text = "Auto Waypoints";
                timer3.Stop();
                button1.Enabled = true;
            }
            else
            {
                if (autoway == false) addwaypoint(); //first waypoint add
                autoway = true;
                btnautoway.Text = "Stop";
                button1.Enabled = false;
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
            ignorelist.Clear();
            deathpointlist.Clear();
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

        public void resurrect()
        {
            TakeOver aion = new TakeOver((int)hwndAion);
            RECT aionscreen = new RECT();
            
            PauseForMilliSeconds(5300); //wait for the death
            diedcount++;
            int halfx = 0;
            int halfy = 0;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Resurrect clicking started");

            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(800);
            GetWindowRect(hwndAion, ref aionscreen);
            halfx = Convert.ToInt32((aionscreen.Right + aionscreen.Left) * .58);
            halfy = Convert.ToInt32((aionscreen.Bottom + aionscreen.Top) * .57);
            for (int y = -210; y < 131; y = y + 20)
            {
                for (int x = 131; x > -220; x = x - 20)
                {
                    aion.MoveMouse(halfx + x, halfy + y);
                    PauseForMilliSeconds(30);
                    aion.MouseLeftClick();
                }
            }
        }

        private void buffcast()
        {
            DateTime current = DateTime.Now;

            if (findpcstance() == eStance.Resting)
            {
                keyenumerator("UP");//get up
                PauseForMilliSeconds(1650);
            }
            for (int i = 0; i < numbuffs; i++)
            {
                if (bufftimes[i] <= current)
                {
                    PauseForMilliSeconds(500);
                    bufftimes[i] = current + buffdelays[i];
                    lblstatus.Text = "Status: Casting Buff: " + i;
                    keyenumerator(buffbtns[i]);
                    PauseForMilliSeconds(1000);
                    Application.DoEvents();
                    PauseForMilliSeconds(1300);
                    if (savelog == true) tw.WriteLine(DateTime.Now + " Buff cast: " + i);

                    lblstatus.Text = "Status: Finished Buff: " + i;

                }
            }
        }

        private void looting()
        {
            lootcounter = 0;
            while ((tar.IsDead == true || tar.Health == 0) && tar.Name != "" && (pc.CubeFull() == false || invcheck == false)) //ADD INVENTORY FULL CHECK
            {
                lblstatus.Text = "Status: Looting..: " + lootcounter;
                pc.Updateafterkill();
                tmrstuck.Stop();
                PauseForMilliSeconds(lootdelay);
                Application.DoEvents();
                //textBox1.Text += "Going into 1st looting" + Environment.NewLine;
                if ((tar.IsDead == true || tar.Health == 0) && tar.Name != "")
                {
                    lootcounter++;
                    keyenumerator(keyloot);
                }

                if (lootcounter >= 5) { lootcounter = 0; ignorelist.Add(tar.ID.ToString()); keyenumerator("ESC"); break; }
                if (savelog == true) tw.WriteLine(DateTime.Now + " Looting finished");
                
                killLabel.Text = "Kills: " + kills;
                lblstatus.Text = "Status: Finished Looting..: " + lootcounter;
            }
            if (pc.CubeFull() == true) { lootcounter = 0; ignorelist.Add(tar.ID.ToString()); keyenumerator("ESC"); }
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
                    combatcounting++;
                    attackflag = true;
                    if (combatcounting >= 1000000) combatcounting = 0;
                    Application.DoEvents();
                    //textBox1.Text += "Top of combat! "+ combatcounting + Environment.NewLine;
                    lblstatus.Text = "Status: Starting combat loop! " + combatcounting;
                    if (btnstop.Visible == false) { lblstatus.Text = "Status: Stopping"; incombatloop = false; return; }
                    if (pc.Health == 0) { deathsystem(); return; }

                    if (pc.ID == tar.ID) { PauseForMilliSeconds(100); keyenumerator("ESC"); }

                    looting();
                    if (btnstop.Visible == true) { rest(); }//RESTtextBox1.Text += "Going into rest" + Environment.NewLine;
                    //tar.TargetID != pc.ID
                    //Application.DoEvents();

                    if ((tar.IsDead != true) && tar.TargetID == pc.ID && tar.Type == eType.AttackableNPC) //ADD DETECTION/FIGHT
                    {
                            lblstatus.Text = "Status: Attacking ADD..";
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Attacking ADD..");
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Fighting " + tar.Name);
                            tmrtabby.Stop();
                            tmrstuck.Stop();
                            //textBox1.Text += "Going into attacking ADD" + Environment.NewLine;
                            mainattackloop();
                            PauseForMilliSeconds(100);
                    }

                    looting();

                    if (btnstop.Visible == true && adddetect() == false) buffcast();//ACTIVATE BUFFS HERE

                    if (btnstop.Visible == true) { rest(); }//RESTtextBox1.Text += "Going into rest" + Environment.NewLine;
                    //Application.DoEvents();
                    try
                    {
                        //textBox1.Text += "Tar.TargetID: " + tar.TargetID.ToString() + " Pc.ID: " + pc.ID + Environment.NewLine;
                        if (btnstop.Visible == true && tar.TargetID != pc.ID && adddetect() == false)
                        {
                            if (deathrun == false) movecounter = waypointlist.IndexOf(findclosestwaypoint());
                            if (movecounter < 0) movecounter = 0;
                            if (movecounter > waypointlist.Count - 1) movecounter = waypointlist.Count - 1;

                            waymovement(false);
                            //Application.DoEvents();
                            //textBox1.Text += "Going into findmob" + Environment.NewLine;
                            useheal();
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Waypoint system started");
                            findmob();
                            if (savelog == true) tw.WriteLine(DateTime.Now + " Waypoint system finished");

                            tmrtabby.Stop();
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
                            if (tarlevel > ignorelevel)
                            {
                                tmrtabby.Stop();
                                tmrstuck.Stop();
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Going into Preattacks");
                                if (savelog == true) tw.WriteLine(DateTime.Now + " Fighting " + tar.Name);
                                //textBox1.Text += "Going into preattacks" + Environment.NewLine;
                                attackmob();
                            }
                        }
                    }
                    catch (Exception ea) { MessageBox.Show("Error in stoptimers and attack " + ea); }

                    attackflag = false;
                    PauseForMilliSeconds(40);
                    //Application.DoEvents();
                    lblstatus.Text = "Status: Finished combat loop! Flag=" + incombatloop + " C:" + combatcounting;
                    //textBox1.Text += " Finished combat loop! counter: " + combatcounting + Environment.NewLine;
                    //Application.DoEvents();
                    //}
                    if (btnstop.Visible == false) { lblstatus.Text = "Status: Stopping"; incombatloop = false; return; }
                }
                lblstatus.Text = "Exited combat loop no:" + combatcounting;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Exited combat loop");

            }
        }

        private void findmob()
        {
            bool hasplayer;
            bool ismoving = false;
            if (tar.Health < 100 && tar.HasTarget == true) hasplayer = true;
            else hasplayer = false;
            if (adddetect() == true)  return;

            if (isresting == false)
            {
                if (adddetect() == true) return;

                while (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString())))
                {
                    if (deathrun == true) lblstatus.Text = "Status: Running death waypoints.. " + deathcounter;
                    else lblstatus.Text = "Status: Searching.. " + movecounter;
                   

                    if (findpcstance() == eStance.Resting) keyenumerator("UP"); //get up
                    if (pc.Health == 0) return;

                    if (pc.ID == tar.ID) { PauseForMilliSeconds(100); keyenumerator("ESC"); }
                    Application.DoEvents();
                    if (tar.TargetID == pc.ID && tar.ID != pc.ID)
                    {
                        return;
                        //textBox1.Text += "In findmob: something attacking me" + Environment.NewLine;
                        //mainattackloop();
                    } //another player attacks it

                    if (istabbing == false && deathrun == false)
                    {
                        istabbing = true;
                        keyenumerator(keytarget);
                        tmrtabby.Start();
                    }
                    Application.DoEvents();

                    waymovement(true);
                    PauseForMilliSeconds(30);

                    if (ismoving == false)
                    {
                        ismoving = true;
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0);
                    }
                    
                    /*changewaypoint();
                    PauseForMilliSeconds(200);
                    //changewaypoint();
                    Application.DoEvents();
                    PauseForMilliSeconds(200);
                    //changewaypoint();
                    Application.DoEvents();
                    PauseForMilliSeconds(200);
                    changewaypoint();
                    keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 2, 0);
                    */

                    if (tmrstuck.Enabled == false)
                    {
                        stuckcounter = 0;
                        returncounter = 0; tmrstuck.Start();
                    }
                    if (tar.Health < 100 && tar.HasTarget == true) hasplayer = true; else hasplayer = false;
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
                    deathrun = false; deathcounter = 0; return;
                }
                awaypoint = deathpointlist[deathcounter];          
            }
            else
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
                        forward = false;
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
            lblwprange.Text = "WPRange: " + string.Format("{0:f2}", pc.Distance3D(awaypoint.X, awaypoint.Y, awaypoint.Z));
        }

        public cwaypoint findclosestwaypoint()
        {
            cwaypoint closestpoint = new cwaypoint();
            closestpoint = waypointlist[0];
            int dcounter = 0;
            double distclosest = 1000;

            try
            {
                foreach (cwaypoint currentpoint in waypointlist)
                {
                    double newwp = pc.Distance3D(currentpoint.X, currentpoint.Y, currentpoint.Z);
                    if (distclosest > newwp)
                    {
                        closestpoint = currentpoint;
                        distclosest = newwp;
                        deathrun = false;
                    }
                }
                //ADD CHECKS FOR DEATH POINT SYSTEM IF CLOSER
                if (pc.Distance3D(closestpoint.X, closestpoint.Y, closestpoint.Z) > 70)
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
            }
            catch (Exception e) { MessageBox.Show("Error in finding closest waypoint. " + e); }
            if (savelog == true) tw.WriteLine(DateTime.Now + " Closest waypoint found!");

            return closestpoint;
        }

        public void waymovement(bool ontrack)
        {
            
            /*double distwp = pc.Distance3D((float)awaypoint.X, (float)awaypoint.Y, (float)awaypoint.Z);

            if ((distwp <= 4 && distwp >= 2) &&
                (Math.Abs(awaypoint.Face(pc.Y, pc.X)) - 90 > 0) && largeangle == false)
            {
                largeangle = true;
                PauseForMilliSeconds(10);
                keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0);
                ismoving = false;
            }
            else largeangle = false;
            */
            //if (ontrack == true)
            //{
                changewaypoint();
            //}
            if (awaypoint.Face(pc.Y, pc.X) > 0)
            {
                if ((pc.Rotation > (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) - 1.5)) //left within 1.5 degrees
                {

                    PauseForMilliSeconds(35);
                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while ((pc.Rotation >= (awaypoint.Face(pc.Y, pc.X) - 180)) && (pc.Rotation <= (awaypoint.Face(pc.Y, pc.X)) - 1.5))  //LEFT
                    {
                        //if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true; else hasplayer = false;
                        //if (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name))) return;
                        if (pc.Health == 0 ) { keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); return; }
                        changewaypoint();
                        Application.DoEvents();
                        pc.UpdateRot();
                        //PauseForMilliSeconds(1);
                    }
                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
                else if (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 1.5)) //right within 1.5 degrees
                {
                    PauseForMilliSeconds(35);
                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while (((-180 < (awaypoint.Face(pc.Y, pc.X) - 180)) && pc.Rotation < (awaypoint.Face(pc.Y, pc.X) - 180)) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X)) + 1.5)) //RIGHT
                    {
                        //if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true; else hasplayer = false;
                        //if (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name))) return;
                        changewaypoint();
                        if (pc.Health == 0 ) { keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        //PauseForMilliSeconds(1);
                        //PauseForMilliSeconds(10);
                    }

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); //  Up
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
            }//end of positive rot point

            else //NEG POINT
            {
                if ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 1.5) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180)) //left within 1.5 degrees
                {
                    PauseForMilliSeconds(35);
                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while ((pc.Rotation > awaypoint.Face(pc.Y, pc.X) + 1.5) && (pc.Rotation < (awaypoint.Face(pc.Y, pc.X)) + 180)) //RIGHT
                    {
                        //if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true; else hasplayer = false;
                        //if (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name))) return;
                        changewaypoint();
                        if (pc.Health == 0 ) { keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        //PauseForMilliSeconds(1);
                        //PauseForMilliSeconds(10);
                    }

                    keybd_event((int)Keys.Right, (byte)MapVirtualKey((int)Keys.Right, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }
                else if (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 1.5) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180))) //right within 1.5 degrees
                {
                    PauseForMilliSeconds(35);
                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 0, 0); //  Down
                    //tmrstuck.Stop();
                    while (((-180 < awaypoint.Face(pc.Y, pc.X)) && pc.Rotation < awaypoint.Face(pc.Y, pc.X) - 1.5) || (pc.Rotation > (awaypoint.Face(pc.Y, pc.X) + 180)))//LEFT
                    {
                        //if (tar.Health < 100 || tar.HasTarget == true) hasplayer = true; else hasplayer = false;
                        //if (tar.Type != eType.AttackableNPC || (((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name))) return;
                        changewaypoint();
                        if (pc.Health == 0 ) { keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); return; }
                        Application.DoEvents();
                        pc.UpdateRot();
                        //PauseForMilliSeconds(1);
                        //PauseForMilliSeconds(10);
                    }

                    keybd_event((int)Keys.Left, (byte)MapVirtualKey((int)Keys.Left, 0), 2, 0); //  Up 
                    stuckcounter = 0;
                    returncounter = 0;
                    //tmrstuck.Start();
                }

            }
            /*
            keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 0, 0); //  Down
            PauseForMilliSeconds(700);
            keybd_event((int)Keys.W, (byte)MapVirtualKey((int)Keys.W, 0), 2, 0); //  Up 
            */
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

        private void waypointEditorToolStripMenuItem_Click_1()
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tmrpot_Tick(object sender, EventArgs e)
        {
            potready = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Pot cooldown finished");
            tmrpot.Stop();
        }

        private void tmrheal_Tick(object sender, EventArgs e)
        {
            healready = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Heal cooldown finished");

            tmrheal.Stop();
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

        private void tmrheal_Tick_1(object sender, EventArgs e)
        {
            healready = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Heal cooldown finished");

            tmrheal.Stop();
        }

        private void waypointEditorToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            waypointeditor f3 = new waypointeditor();
            f3.Show();
        }

        private void tmrtabby_Tick_1(object sender, EventArgs e)
        {
            bool hasplayer;
            if (tar.Health < 100 && tar.HasTarget == true) hasplayer = true;
            else hasplayer = false;
            if (btnstop.Visible == false) { tmrtabby.Stop(); lblstatus.Text = "Stopped bot in tab"; return; }
            if (tar.Type != eType.AttackableNPC || ((int)tar.Level <= ignorelevel) || hasplayer == true || ignorelist.Contains(tar.Name) || ignorelist.Contains(tar.ID.ToString()))
            {
                if (istabbing == true)
                {
                    keyenumerator(keytarget);
                }
            }
            else
            {
                istabbing = false;
                if (savelog == true) tw.WriteLine(DateTime.Now + " Stopped targetting");

                tmrtabby.Stop();
                //istabbing = false;

            }
            if (btnstop.Visible == false) tmrtabby.Stop();
        }

        private void tmrstuck_Tick_1(object sender, EventArgs e)
        {
            if (antistuck == false) tmrstuck.Stop();
            if (antistuck == true)
            {
                if (btnstop.Visible == false || pc.Health == 0) { tmrstuck.Stop(); tmrstuck.Enabled = false; return; }

                if (previouspoint.X == 0 & previouspoint.Y == 0) previouspoint.Set(pc.X, pc.Y, pc.Z);
                double distance = pc.Distance3D(previouspoint.X, previouspoint.Y, previouspoint.Z);
                //FOR MOB STUCK CODE
                if (pc.IsCasting == true) { stuckcounter = 0; returncounter = 0; }
                if (tar.Name != "" && tar.TargetID != pc.ID && pc.IsCasting == false)
                {
                    stuckcounter++;
                    if (distance < 0.4)
                    {
                        if (stuckcounter == 3 || stuckcounter == 4)
                        {
                            if (pc.IsCasting == false)
                            {
                                keyenumerator("SPACE");
                                if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: Jump!");

                                returncounter++;
                                if (returncounter == 1) { 
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

                    if (stuckcounter >= 5)
                    {
                        ignorelist.Add(tar.ID.ToString());
                        lblstatus.Text = "Status: Ignoring mob ID: " + tar.ID.ToString();
                        if (savelog == true) tw.WriteLine(DateTime.Now + " StuckM: Ignored mob:" + tar.ID.ToString());

                        //if (movecounter <= waypointlist.Count - 1) awaypoint = findclosestwaypoint();
                        //PauseForMilliSeconds(100);
                        //keyenumerator("ESC");
                        stuckcounter = 0;
                        returncounter = 0;

                        if (tmrunstuck.Enabled == false) tmrunstuck.Start();
                    }
                }

                //FOR JUST WAYPOINT WALKING, NO MOB TARGETTED
                if (pc.IsCasting == false && tar.Name == "")//(tar.HasTarget != true || tar.Name == ""))
                {
                    if (distance < 0.8)
                    {
                        stuckcounter++;
                    }
                    else { stuckcounter = 0; returncounter = 0; }
                    if (stuckcounter >= 2)
                    {
                        keybd_event((int)Keys.Up, (byte)MapVirtualKey((int)Keys.Up, 0), 0, 0); //just in case
                        keyenumerator("SPACE");
                        returncounter++;
                        if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: Jump");

                        if (returncounter == 1)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: StrafeL");
                            keyenumerator(keystrafeL);
                        }
                        if (returncounter == 2)
                        {
                            if (savelog == true) tw.WriteLine(DateTime.Now + " StuckW: StrafeR");
                            keyenumerator(keystrafeR);
                        } stuckcounter = 0;
                    }
                    if (returncounter >= 2)
                    {
                        if (forward == false)
                        {
                            forward = true;
                        } //switch from backwards to forward
                        else
                        {
                            forward = false;
                        }
                        if (forward == true && movecounter < waypointlist.Count - 1) movecounter++;
                        if (forward == false && movecounter > 0) movecounter--;
                        if (savelog == true) tw.WriteLine(DateTime.Now + " Stuck: Going back to WP: " + movecounter);

                        if (movecounter <= waypointlist.Count - 1 && movecounter >= 0) awaypoint = waypointlist[movecounter];
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
            }
            catch (Exception e1) { MessageBox.Show("Error with unstucktmr: " + e1); }

        }

        private void tmrshutoff_Tick(object sender, EventArgs e)
        {
            SetForegroundWindow(hwndAion);
            PauseForMilliSeconds(100);
            if (shutofftime > 0) tmrshutoff.Stop();
            btnwaypoint.Enabled = true;
            btnautoway.Enabled = true;
            if (savelog == true) tw.WriteLine(DateTime.Now + " Shutoff timer reached");

            tmrstuck.Stop();
            stop();
            istabbing = false;
        }

    }//FORMCLASS

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