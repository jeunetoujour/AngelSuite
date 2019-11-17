
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AionMemory;

namespace ABHeal
{
    public partial class Form1 : Form
    {
        #region Properties
        // Setup process object and SKeys for sending key strokes to the background.
        System.Diagnostics.Process proc;
        SKeys keys;

        // AionMemory objectes and PEntity array to hold our monitored players
        Player player;
        Target target;
        EntityList eList;
        PEntity[] players;

        // One clock to rule them all
        Timer clock;

        // Misc Bools to help keep track of what we are doing
        bool flying = false;
        bool following =  false;
        bool lost = false;
        bool casting = false; // May be depreciated
        double castTime = 0;  // May be depreciated

        // Various DateTime stamps so we know when we can do things again.
        DateTime castStart;
        DateTime flightWarn;
        DateTime chaseTime;
        DateTime manageTime;

        // DateTime stamps to keep track of Heal cool downs.

        DateTime hRCTime;
        DateTime hHLTime;
        DateTime hFRTime;
        DateTime hLRTime;
        #endregion


        public Form1()
        {
            InitializeComponent();

            LoadSettings();     // Load our user settings into the UI.

            // Set all our time stamps to now.
            castStart = hRCTime = hHLTime = hFRTime = hLRTime = 
                flightWarn = chaseTime = manageTime = DateTime.Now;

            // Setup our clock.
            clock = new Timer();
            clock.Interval = 100; // 100ms or 1/10th of a second
            clock.Tick += new EventHandler(clock_Tick);
            
        }

        /// <summary>
        /// Every tick of the clock we need to go through our routine
        /// so the bot can choose the best course of action.
        /// </summary>
        void clock_Tick(object sender, EventArgs e)
        {
            // Update all players.
            PlayerUpdate();
            
            // Every second or 10 cycles we need to look and make sure we have not
            // lost a player due to being out of range or zoned.  If we did then
            // we look for them in the entity list.
            if (ElapsedTime(manageTime) > 1000)
            {
                eList.Update();
                ManagePlayers();
                manageTime = DateTime.Now;
            }

            // See who needs healing and if no one needed healing we will continue
            // on.  We want to loop back to this if someone did need healing incase
            // it has gotten hectic and people are dying.
            if (Heal())
            {
                return;
            }

            // Everyone is healed up so we can look to see if anyone is dead and in
            // range.  If they are we will resurect them.
            if (Resurect())
            {
                return;
            }

            // No one is dead and no one is hurt, lets see if people need buffs.
            Buff();

            // And last but not least, lets make sure we are following our primary
            // player.
            Follow();
        }

        /// <summary>
        /// Look through our players and figure out who needs buffs.
        /// </summary>
        public void Buff()
        {
            for (int i = 0; i < 5; i++)
            {
                // Make sure there is an object/initialized player or well throw exceptions.
                if (players[i] != null)
                {
                    // Only buff if they are close enough, no running around to buff.
                    if (players[i].Distance2D(player.X,player.Y) < 15)
                    {
                        // If either of their buff timers is up we should rebuff
                        // This can be consolidated and made cleaner.
                        if ((ElapsedTime(players[i].bBOHTime) > (double)BlessingOfHealth.Duration * 1000 && bBOHCheck.Checked) ||
                            (ElapsedTime(players[i].bBORTime) > (double)BlessingOfRock.Duration * 1000 && bBORCheck.Checked))
                        {
                            // Set our selves to not following so we restart after were done.
                            following = false;

                            // Select the player
                            PSelect(players[i].Name.ToString());

                            // Buff as needed and reset timers.
                            if (bBOHCheck.Checked)
                            {
                                players[i].bBOHTime = DateTime.Now;
                                keys.SendKey(FindKey(bBOHKey.Text));
                                System.Threading.Thread.Sleep((int)BlessingOfHealth.Cast);
                            }
                            if (bBORCheck.Checked)
                            {
                                players[i].bBORTime = DateTime.Now;
                                keys.SendKey(FindKey(bBORKey.Text));
                                System.Threading.Thread.Sleep((int)BlessingOfRock.Cast);
                            }
                        }
                    }
                }
            }
        }

                        


        /// <summary>
        /// Look for lost players and get their new Ptrs when they re-appear on the entity list.
        /// </summary>
        void ManagePlayers()
        {
            string[] names = new string[5] { p1Name.Text, p2Name.Text, p3Name.Text, p4Name.Text, p5Name.Text };
            for (int i = 0; i < 5; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].Distance2D(player.X,player.Y) > 200)
                    {
                        foreach (Entity p in eList)
                        {
                            if (p.Name.ToString() == names[i])
                            {
                                players[i].PtrEntity = p.PtrEntity;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Follow the main player around.
        /// </summary>
        void Follow()
        {
            // This will eventually switch who it is following if the main player
            // is out of range or dead.
            PEntity p1 = players[0];
            
            if (p1 != null)
            {
                // May not need this, just being safe.
                if (p1.PtrEntity != 0)
                {
                    // Find our flight time
                    double flighttime = ((double)player.FlightTime / (double)player.MaxFlightTime) * 100;

                    // Find out if the player we are following is flying
                    bool mainflight = p1.Stance == eStance.Flying || p1.Stance == eStance.FlyingCombat;

                    // If they are flying and we have more than half our flight time lets fly as well
                    if (mainflight && !flying && flighttime > 50)
                    {
                        flying = true;
                        following = false;
                        keys.SendKey(KeyConstants.VK_PRIOR); // hit the page up key
                        System.Threading.Thread.Sleep(3000); // allow time to get airborne
                       
                    }
                    // If they stopped flying so should we
                    else if (!mainflight && flying)
                    {
                        flying = false;
                        following = false;
                        keys.SendKey(KeyConstants.VK_NEXT);
                        System.Threading.Thread.Sleep(1000);
                        
                    }
                    // Were still flying and running out time, oh crap!
                    else if (flighttime < 25 && flying && ElapsedTime(flightWarn) > 10000)
                    {
                        flightWarn = DateTime.Now;
                        keys.SendLine("/w " + players[0].Name.ToString() + " I need to land soon.");
                    }
                    // If we hit this were probably in trouble and will want to try to re-open
                    // our wings on the next cycle if they are still flying and we have fallen.
                    else if (flighttime < 1 && flying)
                    {
                        flying = false;
                    }
                        

                    // find range
                    double range = player.Distance2D(p1.X,p1.Y);

                    // See if we are close enough to use /follow and not already doing so.
                    if (range < 15 && !following)
                    {
                        PSelectMain();
                        following = true;
                        lost = false;
                    }
                    // Target is to far for /follow so lets try to catch up if were not lost.
                    else if (range > 15 && range < 100 && !lost)
                    {
                        if (ElapsedTime(chaseTime) > 500)
                        {
                            keys.SendKey(KeyConstants.VK_C);
                            chaseTime = DateTime.Now;
                        }
                        
                        following = false;
                        lost = false;
                    }
                    // Target is way out there and no way we can follow so let them know and become lost.
                    else if (range > 200 && !lost)
                    {   
                        lost = true;
                        following = false;
                        keys.SendLine("/w " + players[0].Name.ToString() + " You lost me.");
                    }
                }
            }

        }

        #region Heals

        /// <summary>
        /// Cycle through players and find out who need healing the most.
        /// </summary>
        /// <returns>True if we needed to heal someone.</returns>
        public bool Heal()
        {
            // Figure out our health in percentage so we can compare with Entitys
            double myhealth = (double)player.Health / (double)player.MaxHealth * 100;

            bool group = false;

            // Loop through players array and see if anyone needs a heal.
            for (int i = 0; i < 5; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].Health > 0 && players[i].Health < 90
                        && players[i].PtrEntity != 0
                        && player.Distance3D(players[i].X, players[i].Y, players[i].Z) < 35)
                    {
                        group = true;
                        break;
                    }
                }
            }

            int[] priority = new int[5] { 0, 0, 0, 0, 0 };

            // If someone in the group needs a heal and im not to bad off lets heal them.
            if (group && myhealth > 80)
            {
                int x = 0;
                foreach (PEntity p in players)
                {
                    if (p != null)
                    {
                        // We are using a weighted healing system that will give
                        // higher priority to certain classes at different levels
                        // of health.
                        switch (p.type)
                        {
                            case PType.MainTank:
                                if (p.PtrEntity > 0)
                                {
                                    if (p.Health < 30)
                                    {
                                        priority[x] = 6;
                                    }
                                    else if (p.Health < 50)
                                    {
                                        priority[x] = 5;
                                    }
                                    else if (p.Health < 75)
                                    {
                                        priority[x] = 4;
                                    }
                                    else if (p.Health < 85)
                                    {
                                        priority[x] = 1;
                                    }
                                }
                                x++;
                                break;
                            case PType.Mage:
                                if (p.PtrEntity > 0)
                                {
                                    if (p.Health < 40)
                                    {
                                        priority[x] = 5;
                                    }
                                    else if (p.Health < 60)
                                    {
                                        priority[x] = 4;
                                    }
                                    else if (p.Health < 90)
                                    {
                                        priority[x] = 3;
                                    }
                                }
                                x++;
                                break;
                            case PType.Scout:
                                if (p.PtrEntity > 0)
                                {
                                    if (p.Health < 40)
                                    {
                                        priority[x] = 3;
                                    }
                                    else if (p.Health < 50)
                                    {
                                        priority[x] = 2;
                                    }
                                    else if (p.Health < 85)
                                    {
                                        priority[x] = 1;
                                    }
                                }
                                x++;
                                break;
                            case PType.Healer:
                                if (p.PtrEntity > 0)
                                {
                                    if (p.Health < 40)
                                    {
                                        priority[x] = 4;
                                    }
                                    else if (p.Health < 60)
                                    {
                                        priority[x] = 3;
                                    }
                                    else if (p.Health < 90)
                                    {
                                        priority[x] = 2;
                                    }
                                }
                                x++;
                                break;
                            case PType.Warrior:
                                if (p.PtrEntity > 0)
                                {
                                    if (p.Health < 20)
                                    {
                                        priority[x] = 3;
                                    }
                                    else if (p.Health < 75)
                                    {
                                        priority[x] = 2;
                                    }
                                    else if (p.Health < 90)
                                    {
                                        priority[x] = 1;
                                    }
                                }
                                x++;
                                break;
                        }
                    }
                    else
                    {
                        x++;
                    }
                }

                int target = -1;
                int max = 0;

                // Find the person who has the highest healing priority
                for (int i = 0; i < 5; i++)
                {
                    if (priority[i] > 0 && priority[i] > max)
                    {
                        max = priority[i];
                        target = i;
                    }
                }

                // If no one needed it well continue on. Otherwise heal them with
                // the right routine.
                if (target != -1)
                {
                    switch (players[target].type)
                    {
                        case PType.MainTank:
                            TankHeal(players[target]);
                            return true;
                        case PType.Mage:
                            MageHeal(players[target]);
                            return true;
                        default:
                            BasicHeal(players[target]);
                            return true;
                    }
                }
            }
            // No one needed a heal so if I'm hurt we better top off.
            else if (myhealth < 85)
            {
                SelfHeal((int)myhealth);
                return true;
            }

            // No heals required all around lets let the loop continue.
            return false;
        }


        // The following functions heal based on class type, really only mages and main tanks
        // need special healing patterns, but we may deversify this later if needed.
        // NOTE: We will eventually move all these percentages into the UI so they can be
        // customized per user.
        public void TankHeal(PEntity p)
        {
            PSelect(p.Name.ToString());
            

            if (p.Health < 50 && ElapsedTime(hFRTime) > (double)FlashofRecovery.Cool && hFRCheck.Checked)
            {
                lblStatus.Text = "Status: Heal FlashofRecovery";
                keys.SendKey(FindKey(hFRKey.Text));
                System.Threading.Thread.Sleep((int)FlashofRecovery.Cast);
                hFRTime = DateTime.Now;
            }
            else if (p.Health < 79 && ElapsedTime(hRCTime) > (double)RadiantCure.Cool && hRCCheck.Checked)
            {
                lblStatus.Text = "Status: Heal RadiantCure";
                keys.SendKey(FindKey(hRCKey.Text));
                System.Threading.Thread.Sleep((int)RadiantCure.Cast);
                hRCTime = DateTime.Now;
            }
            else if (p.Health < 85 && ElapsedTime(hHLTime) > (double)HealingLight.Cool
                && (ElapsedTime(hRCTime) < (double)RadiantCure.Cool || !hRCCheck.Checked) && hHLCheck.Checked)
            {
                lblStatus.Text = "Status: Heal HealingLight";
                keys.SendKey(FindKey(hHLKey.Text));
                System.Threading.Thread.Sleep((int)HealingLight.Cast);
                hHLTime = DateTime.Now;
            }
            else if (p.Health < 90 && ElapsedTime(hLRTime) > (double)LightofRecovery.Cool && hLRCheck.Checked)
            {
                keys.SendKey(FindKey(hLRKey.Text));
                System.Threading.Thread.Sleep((int)LightofRecovery.Cast);
                hLRTime = DateTime.Now;
            }


            following = false;
        }

        public void MageHeal(PEntity p)
        {
            PSelect(p.Name.ToString());


            if (p.Health < 40 && ElapsedTime(hFRTime) > (double)FlashofRecovery.Cool && hFRCheck.Checked)
            {
                keys.SendKey(FindKey(hFRKey.Text));
                System.Threading.Thread.Sleep((int)FlashofRecovery.Cast);
                hFRTime = DateTime.Now;
            }
            else if (p.Health < 60 && ElapsedTime(hLRTime) > (double)LightofRecovery.Cool && hLRCheck.Checked)
            {
                keys.SendKey(FindKey(hLRKey.Text));
                System.Threading.Thread.Sleep((int)LightofRecovery.Cast);
                hLRTime = DateTime.Now;
            }
            else if (p.Health < 80 && ElapsedTime(hHLTime) > (double)HealingLight.Cool && hHLCheck.Checked)
            {
                keys.SendKey(FindKey(hHLKey.Text));
                System.Threading.Thread.Sleep((int)HealingLight.Cast);
                hHLTime = DateTime.Now;
            }


            following = false;
        }

        public void BasicHeal(PEntity p)
        {
            PSelect(p.Name.ToString());


            if (p.Health < 40 && ElapsedTime(hFRTime) > (double)FlashofRecovery.Cool && hFRCheck.Checked)
            {
                keys.SendKey(FindKey(hFRKey.Text));
                System.Threading.Thread.Sleep((int)FlashofRecovery.Cast);
                hFRTime = DateTime.Now;
            }
            else if (p.Health < 60 && ElapsedTime(hLRTime) > (double)LightofRecovery.Cool && hLRCheck.Checked)
            {
                keys.SendKey(FindKey(hLRKey.Text));
                System.Threading.Thread.Sleep((int)LightofRecovery.Cast);
                hLRTime = DateTime.Now;
            }
            else if (p.Health < 90 && ElapsedTime(hHLTime) > (double)HealingLight.Cool && hHLCheck.Checked)
            {
                keys.SendKey(FindKey(hHLKey.Text));
                System.Threading.Thread.Sleep((int)HealingLight.Cast);
                hHLTime = DateTime.Now;
            }


            following = false;
        }

        public void SelfHeal(int myhealth)
        {
            keys.SendKey(KeyConstants.VK_F1);


            if (myhealth < 40 && ElapsedTime(hFRTime) > (double)FlashofRecovery.Cool && hFRCheck.Checked)
            {
                lblStatus.Text = "Status: SelfHeal FlashofRecovery";
                keys.SendKey(FindKey(hFRKey.Text));
                System.Threading.Thread.Sleep((int)FlashofRecovery.Cast);
                hFRTime = DateTime.Now;
            }
            else if (myhealth < 50 && ElapsedTime(hLRTime) > (double)LightofRecovery.Cool && hLRCheck.Checked)
            {
                lblStatus.Text = "Status: SelfHeal LightofRecovery";
                keys.SendKey(FindKey(hLRKey.Text));
                System.Threading.Thread.Sleep((int)LightofRecovery.Cast);
                hLRTime = DateTime.Now;
            }
            else if (myhealth < 75 && ElapsedTime(hHLTime) > (double)HealingLight.Cool && hHLCheck.Checked)
            {
                lblStatus.Text = "Status: SelfHeal HealingLight";
                keys.SendKey(FindKey(hHLKey.Text));
                System.Threading.Thread.Sleep((int)HealingLight.Cast);
                hHLTime = DateTime.Now;
            }


            following = false;
        }

        // Lets see if anyone is dead and make sure they are in range.  If they are we
        // will resurect them.
        bool Resurect()
        {
            if (!hResCheck.Checked)
            {
                return false; 
            }

            for (int i = 0; i < 5; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].Health <= 0 && players[i].Distance2D(player.X,player.Y) < 25)
                    {
                        following = false;
                        lblStatus.Text = "Status: Resurrecting";
                        keys.SendKey(FindKey(hResKey.Text));
                        System.Threading.Thread.Sleep(6000);
                        return true;
                    }
                }
            }

            // No one is dead? Onward!
            return false;
        }



        #endregion

        #region Utilities

        /// <summary>
        /// Find out how much time has passed since a certain DateTime
        /// </summary>
        /// <param name="time">DateTime stamp</param>
        /// <returns>How many miliseconds have passed</returns>
        public double ElapsedTime(DateTime time)
        {
            TimeSpan interval;
            interval = DateTime.Now - time;
            return interval.TotalMilliseconds;
        }

        /// <summary>
        /// Select the main player, if we already have them targeted we should follow.
        /// NOTE: We need to refine when and where we select and follow.
        /// </summary>
        private void PSelectMain()
        {
            if (target.Name.ToString() != players[0].Name.ToString())
            {
                keys.SendKey(KeyConstants.VK_F2);
                keys.SendKey(KeyConstants.VK_8);
                following = true;
                //keys.SendLine("/follow " + players[0].Name.ToString());
            }
            else
            {
                keys.SendKey(KeyConstants.VK_8);
                //keys.SendLine("/follow " + players[0].Name.ToString());
                following = true;
            }
        }

        // Select without following a target by name, do nothing if already selected.
        private void PSelect(string name)
        {
            if (target.Name.ToString() != name)
            {
                keys.SendKey(KeyConstants.VK_F2);
                //keys.SendLine("/select " + name);
            }
        }

        // Translate our combobox key choices into KeyConstants
        // NOTE: Eventually add in stuff to do combo keys.
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

        // Find all our players in the list at start up.
        private void LoadPlayers()
        {
            if (p1Name.Text != "")
            {
                players[0] = new PEntity(FindPlayer(p1Name.Text),p1Type.Text);
                if (players[0].PtrEntity > 0)
                {
                    p1Ico.Show();
                }
                else
                {
                    p1Ico.Hide();
                }
            }

            if (p2Name.Text != "")
            {
                players[1] = new PEntity(FindPlayer(p2Name.Text),p2Type.Text);
                if (players[1].PtrEntity > 0)
                {
                    p2Ico.Show();
                }
                else
                {
                    p2Ico.Hide();
                }
            }

            if (p3Name.Text != "")
            {
                players[2] = new PEntity(FindPlayer(p3Name.Text),p3Type.Text);
                if (players[2].PtrEntity > 0)
                {
                    p3Ico.Show();
                }
                else
                {
                    p3Ico.Hide();
                }
            }

            if (p4Name.Text != "")
            {
                players[3] = new PEntity(FindPlayer(p4Name.Text),p4Type.Text);
                if (players[3].PtrEntity > 0)
                {
                    p4Ico.Show();
                }
                else
                {
                    p4Ico.Hide();
                }
            }

            if (p5Name.Text != "")
            {
                players[4] = new PEntity(FindPlayer(p5Name.Text),p5Type.Text);
                if (players[4].PtrEntity > 0)
                {
                    p5Ico.Show();
                }
                else
                {
                    p5Ico.Hide();
                }
            }
        }

        // Find the ptr for the player.
        private uint FindPlayer(string name)
        {
            foreach (Entity e in eList)
            {
                if (e.Name.ToString() == name)
                {
                    return (uint)e.PtrEntity;
                }
            }

            return 0;
        }

        private void PlayerUpdate()
        {
            //eList.Update();
            player.Update();
            target.Update();

            foreach (PEntity p in players)
            {
                if (p != null)
                {
                    p.Update();
                }
            }
        }

        void LoadSettings()
        {
            p5Type.SelectedIndex = Properties.Settings.Default.p5type;
            p4Type.SelectedIndex = Properties.Settings.Default.p4type;
            p3Type.SelectedIndex = Properties.Settings.Default.p3type;
            p2Type.SelectedIndex = Properties.Settings.Default.p2type;
            p1Type.SelectedIndex = Properties.Settings.Default.p1type;
            p5Name.Text = Properties.Settings.Default.player5;
            p4Name.Text = Properties.Settings.Default.player4;
            p3Name.Text = Properties.Settings.Default.player3;
            p2Name.Text = Properties.Settings.Default.player2;
            p1Name.Text = Properties.Settings.Default.player1;
            hFRCheck.Checked = Properties.Settings.Default.hFRCheck;
            hLRCheck.Checked = Properties.Settings.Default.hLRCheck;
            hRCCheck.Checked = Properties.Settings.Default.hRCCheck;
            hHLCheck.Checked = Properties.Settings.Default.hHLCheck;
            hFRKey.SelectedIndex = Properties.Settings.Default.hFRKey;
            hLRKey.SelectedIndex = Properties.Settings.Default.hLRKey;
            hRCKey.SelectedIndex = Properties.Settings.Default.hRCKey;
            hHLKey.SelectedIndex = Properties.Settings.Default.hHLKey;
            bBORCheck.Checked = Properties.Settings.Default.bBORCheck;
            bBOHCheck.Checked = Properties.Settings.Default.bBOHCheck;
            bBOHKey.SelectedIndex = Properties.Settings.Default.bBOHKey;
            bBORKey.SelectedIndex = Properties.Settings.Default.bBORKey;
            hResCheck.Checked = Properties.Settings.Default.hResCheck;
            hResKey.SelectedIndex = Properties.Settings.Default.hResKey;
        }
        #endregion

        #region UIEvents

        private void uiCheckGroup_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Group Mode Is Currently Not Functioning");
            uiCheckGroup.Checked = false;
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if (AionMemory.Process.Open())
            {
                lblStatus.Text = "Status: Started!";

                proc = System.Diagnostics.Process.GetProcessesByName("aion.bin")[0];
                keys = new SKeys(proc);
                eList = new EntityList();
                player = new Player();
                player.Updatenamelvl();

                target = new Target();
                players = new PEntity[5];
                eList.Update();
                LoadPlayers();

                if (players[0] == null || players[0].PtrEntity == 0)
                {
                    MessageBox.Show("Main not found!");
                    
                    AionMemory.Process.Close();
                    lblStatus.Text = "Status: Stopped...";
                    return;

                }

                PlayerUpdate();
                player.Updatenamelvl();
                player.UpdateRot();
                player.Updateafterkill();

                clock.Start();
            }
            else
            {
                MessageBox.Show("Aion Not Found!");
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            clock.Stop();
            AionMemory.Process.Close();
            following = false;
            lost = false;
            lblStatus.Text = "Status: Stopped.";
        }

        private void hRCCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hRCCheck = hRCCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void hRCKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hRCKey = hRCKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void hLRCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hLRCheck = hLRCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void hLRKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hLRKey = hLRKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void hHLCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hHLCheck = hHLCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void hHLKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hHLKey = hHLKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void hFRCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hFRCheck = hFRCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void hFRKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hFRKey = hFRKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void p1Name_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.player1 = p1Name.Text;
            Properties.Settings.Default.Save();
        }

        private void p2Name_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.player2 = p2Name.Text;
            Properties.Settings.Default.Save();
        }

        private void p3Name_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.player3 = p3Name.Text;
            Properties.Settings.Default.Save();
        }

        private void p4Name_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.player4 = p4Name.Text;
            Properties.Settings.Default.Save();
        }

        private void p5Name_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.player5 = p5Name.Text;
            Properties.Settings.Default.Save();
        }

        private void p1Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.p1type = p1Type.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void p2Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.p2type = p2Type.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void p3Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.p3type = p3Type.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void p4Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.p4type = p4Type.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void p5Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.p5type = p5Type.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void bBOHCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bBOHCheck = bBOHCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void bBOHKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bBOHKey = bBOHKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void bBORCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bBORCheck = bBORCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void bBORKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bBORKey = bBORKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void hResCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hResCheck = hResCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void hResKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hResKey = hResKey.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        #endregion
        
    }
}
