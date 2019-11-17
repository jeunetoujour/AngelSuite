using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AionAPP
{
/*
 *  AionAPP
 *  Developed by:   Viion
 *  Website:        Viion.co.uk
 *  Date Year:      2010
 *  Version         0.9
 *  
 *  If you have this source code, please do not claim as your own.
*/
    public partial class AionAPP : Form
    {
        public AionAPP()
        {
            InitializeComponent();
        }

        private void AionAPP_Load(object sender, EventArgs e)
        {
            //-----------------------------------------------------------------------
            // Begin Application
            //-----------------------------------------------------------------------

            // Open AionMemory and run
            AionMemory.Process.Open();

            // Update all player information
            AionVars.player.Updatenamelvl();
            AionVars.player.Updateafterkill();
            AionVars.player.Update();

            // Register Player Information
            playerName = AionVars.player.Name;
            playerLevel = AionVars.player.Level;
            playerXP = AionVars.player.XP;
            playerXPMAX = AionVars.player.MaxXP;
            playerHP = AionVars.player.Health;
            playerNameDisplay.Text = playerName;

            // Set App Status
            statusBox.Text = "Stopped";

            // Reset App Data
            Reset();

            // Set Startup Fixed Variables
            firststart = 0;
            deathwait = 0;

            // Separators
            seperator1.AutoSize = false;
            seperator1.Height = 2;
            seperator1.BorderStyle = BorderStyle.Fixed3D;
            seperator2.AutoSize = false;
            seperator2.Height = 2;
            seperator2.BorderStyle = BorderStyle.Fixed3D;
            seperator3.AutoSize = false;
            seperator3.Height = 2;
            seperator3.BorderStyle = BorderStyle.Fixed3D;            
        }

        //-----------------------------------------------------------------------
        // Reset (Default)
        // Purpose:     Resets everything back to initialized state. 
        //-----------------------------------------------------------------------
        public void Reset()
        {
            // Update all player information
            AionVars.player.Updatenamelvl();
            AionVars.player.Updateafterkill();
            AionVars.player.Update();

            // Register Player Information
            playerName = AionVars.player.Name;
            playerLevel = AionVars.player.Level;
            playerXP = AionVars.player.XP;
            playerXPMAX = AionVars.player.MaxXP;
            playerHP = AionVars.player.Health;

            // Calculate Require to level
            requiredExpToLevel = playerXPMAX - playerXP;
            
            // Set Default Text Labels
            playerNameDisplay.Text = playerName;
            currentXPDisplay.Text = string.Format("{0:###,###,###}", playerXP);
            timerTickDisplay.Text = timerTick.ToString();
            requireXPDisplay.Text = string.Format("{0:EXP Required: ###,###,###}", requiredExpToLevel);
            xpGainedDisplay.Text = string.Format("{0:###,###,###}", gainedEXP);
            xpHourDisplay.Text = string.Format("{0:###,###,###}", ExpAnHour);

            // Set Progress Bar to match % of Current EXP
            progressBarControl();

            // Clear EXP/Time lists.
            expValues.Clear();
            timeValues.Clear();
            
            // Blank Number Labels
            avgFightTimeDisplay.Text = "";
            avgExpGainDisplay.Text = "";
            totalFights.Text = "";
            expPerSecondDisplay.Text = "";
            expPerMinuteDisplay.Text = "";
            levelInDisplay.Text = "";
            fightsToLvDisplay.Text = "";
            levelsGainedDisplay.Text = "";
            fastestKillDisplay.Text = "";
            killsMinuteDisplay.Text = "";
            killsHourDisplay.Text = "";
            highlowexpDisplay.Text = "";
            deathDisplay.Text = "";
            
            // Store EXP for calculations
            oldEXP = AionVars.player.XP;
            newEXP = 0;
            oldLV = AionVars.player.Level;
            newLV = AionVars.player.Level;

            // Zero common variables
            gainedEXP = 0;
            EXPtoADD = 0;
            timerTick = 0;
            storedTime = 0;
            ExpAnHour = 0;
            levelIn = 0;
            lastexp = 0;
            levelsGained = 0;
            timeValuesTotal = 0;
            expValuesTotal = 0;
            avgFightTime = 0;
            avgExpGain = 0;
            fightsToLv = 0;
            killsMinute = 0;
            killsHour = 0;
            fastestKill = 0;
            slowestKill = 0;
            deathcount = 0;

            // Vars used in calculations only
            timeDividedByOneHour = 0;
            expPerOneSecond = 0;
            expPerOneMinute = 0;
            requiredExpToLevel = 0;
            requiredDivBySecondsAmount = 0;
            requiredDivByMinutesAmount = 0;
            requiredDivByHoursAmount = 0;
            requiredDivByDaysAmount = 0;
            avgFightTimeInMinutes = 0;
            avgFightTimeInHours = 0;
            totalTimeAmount = 0;
            percentProgress = 0;

            // Set pause status on Reset
            paused = 1;
        }

        // Datatypes.
        string playerName;
        double percentProgress, fightsToLv, killsMinute, killsHour;
        int playerXP, playerXPMAX, playerHP, playerLevel, oldEXP, newEXP, oldLV, newLV, gainedEXP, EXPtoADD, timerTick, storedTime, levelIn;
        int ExpAnHour, timeValuesTotal, expValuesTotal, avgFightTime, avgExpGain, paused, firststart, lastexp;
        int timeDividedByOneHour, expPerOneSecond, expPerOneMinute, requiredExpToLevel, levelsGained, totalTimeAmount;
        int requiredDivBySecondsAmount, requiredDivByMinutesAmount, requiredDivByHoursAmount, requiredDivByDaysAmount;
        int avgFightTimeInMinutes, avgFightTimeInHours, fastestKill, slowestKill, lowEXP, highEXP, deathcount;
        int deathwait;

        // Lists to store EXP/Time data.
        List<int> expValues = new List<int>();
        List<int> timeValues = new List<int>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Check if application is paused
            if (paused==0) {
                // If not Add 1 second to the timer
                timerTick++;

                // Update all player information
                AionVars.player.Updatenamelvl();
                AionVars.player.Updateafterkill();
                AionVars.player.Update();

                // Update Level/EXP/HP
                newLV = AionVars.player.Level;
                newEXP = AionVars.player.XP;
                playerHP = AionVars.player.Health;

                // Update Progress Bar for Current EXP
                progressBarControl();

                // If the updated NewLV is higher than that last recorded, player gained a level
                if (newLV > oldLV)
                {
                    // Update Levels Gained.
                    levelsGained++;

                    // Set Old LV to match current level
                    oldLV = AionVars.player.Level;

                    // Reset Old EXP ready for new update
                    oldEXP = 0;

                    // Calculate EXP Required to Level
                    requiredExpToLevel = playerXPMAX - playerXP;
                    requireXPDisplay.Text = string.Format("{0:EXP Required: ###,###,###}", requiredExpToLevel);

                    // Calculate the total of all the values inside the time list
                    foreach (int valC in timeValues)
                    {
                        totalTimeAmount += valC;
                    }

                    // Convert total time (seconds) to an actual time span.
                    TimeSpan lvGain = TimeSpan.FromSeconds(totalTimeAmount);
                    string levelsGainedTime = string.Format("{0:D2}d  {1:D2}h  {2:D2}m  {3:D2}s", lvGain.Days, lvGain.Hours, lvGain.Minutes, lvGain.Seconds);
                    levelsGainedDisplay.Text = "Levels Gained: " + levelsGained + "    in: " + levelsGainedTime;

                    // Fights to level will need resetting as new MaxXP is set
                    fightsToLvDisplay.Text = "Waiting...";
                }

                // If the new updated EXP is higher than last stored, player killed an enemy.
                if (newEXP > oldEXP)
                {
                    // The difference of EXP will be the new - the old
                    EXPtoADD = (newEXP-=oldEXP);

                    // Store the last EXP gained (this var is not used)
                    lastexp = EXPtoADD;

                    // Increase total EXP Gained
                    gainedEXP += EXPtoADD;

                    // Update old EXP with new ready for next check
                    oldEXP = AionVars.player.XP;

                    // Get the value of timer to add to the Time List
                    storedTime = timerTick;

                    // Update Lists with EXP / Time
                    expValues.Add(EXPtoADD);
                    timeValues.Add(storedTime);

                    // Reset the timer
                    timerTick = 0;

                    // Update Player XP
                    playerXP = AionVars.player.XP;

                    // Calculate total Time Values
                    int timeValuesTotal = 0;
                    foreach (int valA in timeValues)
                    {
                        timeValuesTotal += valA;
                    }

                    // Calculate total EXP Values
                    int expValuesTotal = 0;
                    foreach (int valB in expValues)
                    {
                        expValuesTotal += valB;
                    }

                    // Ensure all calculations are above 0 and there is actual data in the lists
                    if ((timeValuesTotal != 0) && (timeValues.Count != 0) && (expValuesTotal != 0) && (expValues.Count != 0))
                    {
                        // Calculate the Average Fight time and convert to a time span format
                        avgFightTime = (timeValuesTotal / timeValues.Count);
                        TimeSpan texp = TimeSpan.FromSeconds(avgFightTime);
                        string avgFightTimeFormatted = string.Format("{0:D2}m  {1:D2}s", texp.Minutes, texp.Seconds);
                        avgFightTimeDisplay.Text = avgFightTimeFormatted.ToString();

                        // Calculate the Average EXP Gain per fight
                        avgExpGain = (expValuesTotal / expValues.Count);
                        avgExpGainDisplay.Text = avgExpGain.ToString();

                        // Calculate EXP/Hour by doing 1hour (in seconds)  /  Avg Fight Time (in seconds)
                        timeDividedByOneHour = (3600 / avgFightTime);
                        ExpAnHour = timeDividedByOneHour * avgExpGain;

                        // Get Count for amount of values inside list, this equals amount of fights
                        totalFights.Text = (expValues.Count()).ToString();

                        // If only one fight has occured, display Waiting... until 2 have.
                        // ! no if statement = incorrect display or calculation of Fights to Level
                        if (expValues.Count() == 1)
                        {
                            fightsToLvDisplay.Text = "Waiting...";
                        }
                        else
                        {
                            fightsToLv = Math.Floor((double)requiredExpToLevel / avgExpGain);
                            fightsToLvDisplay.Text = fightsToLv.ToString();
                        }

                        // Run Kill Statistics function
                        killStats();

                        // Run Kill Speed Function
                        killSpeed();

                        // Run Highest/Lowest EXP Function
                        exphighlow();
                    }
                }
                
                // if EXP/Hour is above 0 then knows we're fighting
                if (ExpAnHour > 0)
                {
                    // Update Player Information
                    AionVars.player.Updatenamelvl();
                    AionVars.player.Updateafterkill();

                    // Calculate Required EXP to Level and Print
                    requiredExpToLevel = playerXPMAX - playerXP;
                    requireXPDisplay.Text = string.Format("{0:EXP Required: ###,###,###}", requiredExpToLevel);

                    // Calculate EXP per second and Minute
                    expPerOneSecond = (ExpAnHour / 3600);
                    expPerOneMinute = (expPerOneSecond * 60);

                    // Each second your technically losing 1second of EXP, so Minus this each second
                    ExpAnHour -= expPerOneSecond;

                    // Print Data to Labels
                    xpHourDisplay.Text = ExpAnHour.ToString();
                    expPerSecondDisplay.Text = expPerOneSecond.ToString();
                    expPerMinuteDisplay.Text = expPerOneMinute.ToString();

                    // Calculate How long it will take to level
                    // Math: Total EXP Required  /  EXP Per 1 second = Total time in seconds
                    requiredDivBySecondsAmount = (requiredExpToLevel / expPerOneSecond);
                    TimeSpan ttolv = TimeSpan.FromSeconds(requiredDivBySecondsAmount);

                    // If the amount of seconds is beyond 1 day (Player shouldnt be grinding there!!!)
                    if (requiredDivBySecondsAmount < 86400)
                    {
                        // No Days format
                        string levelInFormatted = string.Format("{0:D2}h  {1:D2}m  {2:D2}s", ttolv.Hours, ttolv.Minutes, ttolv.Seconds);
                        levelInDisplay.Text = levelInFormatted.ToString();
                    }
                    else
                    {
                        // Day Format
                        string levelInFormatted = string.Format("{0:D2}d  {1:D2}h  {2:D2}m  {3:D2}s", ttolv.Days, ttolv.Hours, ttolv.Minutes, ttolv.Seconds);
                        levelInDisplay.Text = levelInFormatted.ToString();
                    }                   
                }

                // Update Display Labels
                xpGainedDisplay.Text = string.Format("{0:###,###,###}", gainedEXP);
                xpHourDisplay.Text = string.Format("{0:###,###,###}", ExpAnHour);
                currentXPDisplay.Text = string.Format("{0:###,###,###}", playerXP);
                timerTickDisplay.Text = timerTick.ToString();

                // Check if Player is Dead
                deathcheck();
            }
        }

        // Death check
        public void deathcheck()
        {
            // If Players HP = 0 and the deathWait is 0
            if (playerHP == 0 && deathwait == 0)
            {
                // Update Death Count
                deathcount++;

                // Set Death wait so it does not keep adding deathcount per second
                deathwait = 1;

                // Print Death Label
                deathDisplay.Text = "You've died " + deathcount + " times.";
            }

            // If Players HP above 0 (raised up)
            if (playerHP > 0)
            {
                // Reset Death wait until next death
                deathwait = 0;
            }
        }

        // Kill Statistics
        public void killStats()
        {
            // Calculate how many kills per minute
            killsMinute = ((double)60 / avgFightTime);
            killsHour = ((double)killsMinute * 60);

            // Print Kill Label
            killsMinuteDisplay.Text = string.Format("{0:0.00}", killsMinute);
            killsHourDisplay.Text = string.Format("{0:0.00}", killsHour);
        }

        // Control the Progress Bar to avoid Framework errors
        public void progressBarControl()
        {
            // Create progress bar for how much XP Gained
            percentProgress = Math.Floor(((double)playerXP / playerXPMAX) * 100);

            // Ensure EXP Bar does not go out of boundarys
            if (percentProgress > 100)
            {
                percentProgress = 100;
            }
            if (percentProgress < 0)
            {
                percentProgress = 0;
            }

            // Same as above, another precaution
            expProgressDisplay.Maximum = 100;
            expProgressDisplay.Minimum = 0;
            expProgressDisplay.Value = int.Parse(Math.Truncate(percentProgress).ToString());
            expPercentNumDisplay.Text = percentProgress + "%";
        }

        // Kill Statistics
        public void killSpeed()
        {
            // Get the Lowest Value (fastest kill) and highest (slowest kill)
            fastestKill = timeValues.Min();
            slowestKill = timeValues.Max();

            // Convert the Seconds of Fastest/Slowests into a Time Span
            TimeSpan fk = TimeSpan.FromSeconds(fastestKill);
            TimeSpan sk = TimeSpan.FromSeconds(slowestKill);

            // Change Format to Minutes/Seconds
            string fastest = string.Format("{0:D2}:{1:D2}", fk.Minutes, fk.Seconds);
            string slowest = string.Format("{0:D2}:{1:D2}", sk.Minutes, sk.Seconds);

            // Print Fastest/Slowest Label
            fastestKillDisplay.Text = fastest + " / " + slowest;
        }

        // EXP Statistics
        public void exphighlow()
        {
            // Get the Highest / Lowest Value
            lowEXP = expValues.Min();
            highEXP = expValues.Max();

            // Print Highest/Lowest EXP Label
            highlowexpDisplay.Text = lowEXP + " / " + highEXP;
        }


        // -------
        // Private functions default by Visual c#
        // -------

        // Start/Stop Button
        private void onoffbutton_Click(object sender, EventArgs e)
        {
            // Check if the Application is paused.
            if (paused==1)
            {
                // Check if this is first time Application has opened
                if (firststart == 0)
                {
                    // If yes Reset Values 
                    // (this prevents EXP Going in if you start the app, then gain some, then hit Start)
                    firststart = 1;
                    Reset();
                }

                // Set Pause Value
                paused = 0;

                // Change Text Labels
                onoffbutton.Text = "Stop";
                statusBox.Text = "Running";
            }
            else
            {
                // Set Pause Value
                paused = 1;

                // Change Text Labels
                onoffbutton.Text = "Start";
                statusBox.Text = "Stopped";
            }
        }

        // Reset Button
        private void resetbutton_Click(object sender, EventArgs e)
        {
            // If Application is not paused, pause it upon reset
            if (paused == 0)
            {
                paused = 1;
                onoffbutton.Text = "Start";
            }

            // Change Text Labels
            statusBox.Text = "Stopped";

            // Reset Application
            Reset();
        }

        // Opacity Slider
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Opacity = (e.NewValue / 100.0)+0.1;
        }

        // Always On Top checkbox.
        private void alwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (stayOnTopCheckBox.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }
    }
}
