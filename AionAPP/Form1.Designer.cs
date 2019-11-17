namespace AionAPP
{
    partial class AionAPP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AionAPP));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.playerNameDisplay = new System.Windows.Forms.Label();
            this.currentXPDisplay = new System.Windows.Forms.Label();
            this.xpHourDisplay = new System.Windows.Forms.Label();
            this.xpGainedDisplay = new System.Windows.Forms.Label();
            this.timerTickDisplay = new System.Windows.Forms.Label();
            this.avgFightTimeDisplay = new System.Windows.Forms.Label();
            this.avgExpGainDisplay = new System.Windows.Forms.Label();
            this.totalFights = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label18 = new System.Windows.Forms.Label();
            this.fastestKillDisplay = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.killsHourDisplay = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.killsMinuteDisplay = new System.Windows.Forms.Label();
            this.statusBox = new System.Windows.Forms.Label();
            this.levelsGainedDisplay = new System.Windows.Forms.Label();
            this.fightsToLvDisplay = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.seperator3 = new System.Windows.Forms.Label();
            this.expPerMinuteDisplay = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.seperator2 = new System.Windows.Forms.Label();
            this.resetbutton = new System.Windows.Forms.Button();
            this.onoffbutton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.expHrText = new System.Windows.Forms.Label();
            this.expGainedText = new System.Windows.Forms.Label();
            this.seperator1 = new System.Windows.Forms.Label();
            this.expPercentNumDisplay = new System.Windows.Forms.Label();
            this.expProgressDisplay = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.requireXPDisplay = new System.Windows.Forms.Label();
            this.levelInDisplay = new System.Windows.Forms.Label();
            this.expPerSecondDisplay = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.stayOnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.highlowexpDisplay = new System.Windows.Forms.Label();
            this.deathDisplay = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // playerNameDisplay
            // 
            this.playerNameDisplay.AutoSize = true;
            this.playerNameDisplay.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerNameDisplay.ForeColor = System.Drawing.Color.Green;
            this.playerNameDisplay.Location = new System.Drawing.Point(8, 8);
            this.playerNameDisplay.Name = "playerNameDisplay";
            this.playerNameDisplay.Size = new System.Drawing.Size(190, 24);
            this.playerNameDisplay.TabIndex = 0;
            this.playerNameDisplay.Text = "playerNameDisplay";
            // 
            // currentXPDisplay
            // 
            this.currentXPDisplay.AutoSize = true;
            this.currentXPDisplay.BackColor = System.Drawing.Color.Transparent;
            this.currentXPDisplay.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentXPDisplay.Location = new System.Drawing.Point(8, 38);
            this.currentXPDisplay.Name = "currentXPDisplay";
            this.currentXPDisplay.Size = new System.Drawing.Size(27, 14);
            this.currentXPDisplay.TabIndex = 1;
            this.currentXPDisplay.Text = "EXP";
            // 
            // xpHourDisplay
            // 
            this.xpHourDisplay.AutoSize = true;
            this.xpHourDisplay.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpHourDisplay.ForeColor = System.Drawing.Color.DarkGreen;
            this.xpHourDisplay.Location = new System.Drawing.Point(7, 95);
            this.xpHourDisplay.Name = "xpHourDisplay";
            this.xpHourDisplay.Size = new System.Drawing.Size(132, 22);
            this.xpHourDisplay.TabIndex = 2;
            this.xpHourDisplay.Text = "xpHourDisplay";
            // 
            // xpGainedDisplay
            // 
            this.xpGainedDisplay.AutoSize = true;
            this.xpGainedDisplay.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpGainedDisplay.ForeColor = System.Drawing.Color.DarkGreen;
            this.xpGainedDisplay.Location = new System.Drawing.Point(158, 95);
            this.xpGainedDisplay.Name = "xpGainedDisplay";
            this.xpGainedDisplay.Size = new System.Drawing.Size(153, 22);
            this.xpGainedDisplay.TabIndex = 3;
            this.xpGainedDisplay.Text = "xpGainedDisplay";
            // 
            // timerTickDisplay
            // 
            this.timerTickDisplay.AutoSize = true;
            this.timerTickDisplay.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerTickDisplay.ForeColor = System.Drawing.Color.SlateGray;
            this.timerTickDisplay.Location = new System.Drawing.Point(258, 414);
            this.timerTickDisplay.Name = "timerTickDisplay";
            this.timerTickDisplay.Size = new System.Drawing.Size(19, 14);
            this.timerTickDisplay.TabIndex = 4;
            this.timerTickDisplay.Text = "00";
            // 
            // avgFightTimeDisplay
            // 
            this.avgFightTimeDisplay.AutoSize = true;
            this.avgFightTimeDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.avgFightTimeDisplay.ForeColor = System.Drawing.Color.Green;
            this.avgFightTimeDisplay.Location = new System.Drawing.Point(10, 186);
            this.avgFightTimeDisplay.Name = "avgFightTimeDisplay";
            this.avgFightTimeDisplay.Size = new System.Drawing.Size(139, 17);
            this.avgFightTimeDisplay.TabIndex = 5;
            this.avgFightTimeDisplay.Text = "avgFightTimeDisplay";
            // 
            // avgExpGainDisplay
            // 
            this.avgExpGainDisplay.AutoSize = true;
            this.avgExpGainDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.avgExpGainDisplay.ForeColor = System.Drawing.Color.Green;
            this.avgExpGainDisplay.Location = new System.Drawing.Point(158, 186);
            this.avgExpGainDisplay.Name = "avgExpGainDisplay";
            this.avgExpGainDisplay.Size = new System.Drawing.Size(130, 17);
            this.avgExpGainDisplay.TabIndex = 6;
            this.avgExpGainDisplay.Text = "avgExpGainDisplay";
            // 
            // totalFights
            // 
            this.totalFights.AutoSize = true;
            this.totalFights.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.totalFights.ForeColor = System.Drawing.Color.Green;
            this.totalFights.Location = new System.Drawing.Point(10, 305);
            this.totalFights.Name = "totalFights";
            this.totalFights.Size = new System.Drawing.Size(73, 17);
            this.totalFights.TabIndex = 7;
            this.totalFights.Text = "totalFights";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tabControl1.Location = new System.Drawing.Point(-2, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(294, 462);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.deathDisplay);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.highlowexpDisplay);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.fastestKillDisplay);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.killsHourDisplay);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.killsMinuteDisplay);
            this.tabPage1.Controls.Add(this.statusBox);
            this.tabPage1.Controls.Add(this.levelsGainedDisplay);
            this.tabPage1.Controls.Add(this.fightsToLvDisplay);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.seperator3);
            this.tabPage1.Controls.Add(this.expPerMinuteDisplay);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.seperator2);
            this.tabPage1.Controls.Add(this.resetbutton);
            this.tabPage1.Controls.Add(this.onoffbutton);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.expHrText);
            this.tabPage1.Controls.Add(this.expGainedText);
            this.tabPage1.Controls.Add(this.seperator1);
            this.tabPage1.Controls.Add(this.expPercentNumDisplay);
            this.tabPage1.Controls.Add(this.currentXPDisplay);
            this.tabPage1.Controls.Add(this.expProgressDisplay);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.requireXPDisplay);
            this.tabPage1.Controls.Add(this.levelInDisplay);
            this.tabPage1.Controls.Add(this.expPerSecondDisplay);
            this.tabPage1.Controls.Add(this.totalFights);
            this.tabPage1.Controls.Add(this.avgExpGainDisplay);
            this.tabPage1.Controls.Add(this.xpHourDisplay);
            this.tabPage1.Controls.Add(this.avgFightTimeDisplay);
            this.tabPage1.Controls.Add(this.xpGainedDisplay);
            this.tabPage1.Controls.Add(this.timerTickDisplay);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(286, 433);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EXP Guage";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(9, 246);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(85, 14);
            this.label18.TabIndex = 37;
            this.label18.Text = "Fast / Slow Kill";
            // 
            // fastestKillDisplay
            // 
            this.fastestKillDisplay.AutoSize = true;
            this.fastestKillDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.fastestKillDisplay.ForeColor = System.Drawing.Color.Green;
            this.fastestKillDisplay.Location = new System.Drawing.Point(10, 260);
            this.fastestKillDisplay.Name = "fastestKillDisplay";
            this.fastestKillDisplay.Size = new System.Drawing.Size(114, 17);
            this.fastestKillDisplay.TabIndex = 36;
            this.fastestKillDisplay.Text = "fastestKillDisplay";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(159, 327);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 14);
            this.label19.TabIndex = 35;
            this.label19.Text = "Kills/Hour";
            // 
            // killsHourDisplay
            // 
            this.killsHourDisplay.AutoSize = true;
            this.killsHourDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.killsHourDisplay.ForeColor = System.Drawing.Color.Green;
            this.killsHourDisplay.Location = new System.Drawing.Point(159, 341);
            this.killsHourDisplay.Name = "killsHourDisplay";
            this.killsHourDisplay.Size = new System.Drawing.Size(62, 17);
            this.killsHourDisplay.TabIndex = 34;
            this.killsHourDisplay.Text = "killsHour";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(10, 327);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 14);
            this.label17.TabIndex = 33;
            this.label17.Text = "Kills/Minute";
            // 
            // killsMinuteDisplay
            // 
            this.killsMinuteDisplay.AutoSize = true;
            this.killsMinuteDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.killsMinuteDisplay.ForeColor = System.Drawing.Color.Green;
            this.killsMinuteDisplay.Location = new System.Drawing.Point(10, 341);
            this.killsMinuteDisplay.Name = "killsMinuteDisplay";
            this.killsMinuteDisplay.Size = new System.Drawing.Size(73, 17);
            this.killsMinuteDisplay.TabIndex = 32;
            this.killsMinuteDisplay.Text = "killsMinute";
            // 
            // statusBox
            // 
            this.statusBox.AutoSize = true;
            this.statusBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.ForeColor = System.Drawing.Color.SteelBlue;
            this.statusBox.Location = new System.Drawing.Point(193, 414);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(62, 14);
            this.statusBox.TabIndex = 31;
            this.statusBox.Text = "statusBox";
            // 
            // levelsGainedDisplay
            // 
            this.levelsGainedDisplay.AutoSize = true;
            this.levelsGainedDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelsGainedDisplay.ForeColor = System.Drawing.Color.Blue;
            this.levelsGainedDisplay.Location = new System.Drawing.Point(8, 385);
            this.levelsGainedDisplay.Name = "levelsGainedDisplay";
            this.levelsGainedDisplay.Size = new System.Drawing.Size(72, 15);
            this.levelsGainedDisplay.TabIndex = 30;
            this.levelsGainedDisplay.Text = "Total Fights";
            // 
            // fightsToLvDisplay
            // 
            this.fightsToLvDisplay.AutoSize = true;
            this.fightsToLvDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.fightsToLvDisplay.ForeColor = System.Drawing.Color.Green;
            this.fightsToLvDisplay.Location = new System.Drawing.Point(159, 305);
            this.fightsToLvDisplay.Name = "fightsToLvDisplay";
            this.fightsToLvDisplay.Size = new System.Drawing.Size(74, 17);
            this.fightsToLvDisplay.TabIndex = 29;
            this.fightsToLvDisplay.Text = "fightsToLv";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(158, 291);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 14);
            this.label16.TabIndex = 28;
            this.label16.Text = "Mobs To Level";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(9, 291);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 14);
            this.label15.TabIndex = 27;
            this.label15.Text = "Total Kills";
            // 
            // seperator3
            // 
            this.seperator3.Location = new System.Drawing.Point(11, 284);
            this.seperator3.Name = "seperator3";
            this.seperator3.Size = new System.Drawing.Size(266, 12);
            this.seperator3.TabIndex = 26;
            this.seperator3.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯";
            // 
            // expPerMinuteDisplay
            // 
            this.expPerMinuteDisplay.AutoSize = true;
            this.expPerMinuteDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.expPerMinuteDisplay.ForeColor = System.Drawing.Color.Green;
            this.expPerMinuteDisplay.Location = new System.Drawing.Point(159, 222);
            this.expPerMinuteDisplay.Name = "expPerMinuteDisplay";
            this.expPerMinuteDisplay.Size = new System.Drawing.Size(140, 17);
            this.expPerMinuteDisplay.TabIndex = 25;
            this.expPerMinuteDisplay.Text = "expPerMinuteDisplay";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(159, 208);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 14);
            this.label14.TabIndex = 24;
            this.label14.Text = "EXP/Minute";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(9, 208);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 14);
            this.label13.TabIndex = 23;
            this.label13.Text = "EXP/Second";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(159, 172);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 14);
            this.label12.TabIndex = 22;
            this.label12.Text = "Avg EXP Gain";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(9, 172);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 14);
            this.label11.TabIndex = 21;
            this.label11.Text = "Avg Fight Time";
            // 
            // seperator2
            // 
            this.seperator2.Location = new System.Drawing.Point(10, 166);
            this.seperator2.Name = "seperator2";
            this.seperator2.Size = new System.Drawing.Size(267, 12);
            this.seperator2.TabIndex = 20;
            this.seperator2.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯";
            // 
            // resetbutton
            // 
            this.resetbutton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetbutton.ForeColor = System.Drawing.Color.Red;
            this.resetbutton.Location = new System.Drawing.Point(76, 403);
            this.resetbutton.Name = "resetbutton";
            this.resetbutton.Size = new System.Drawing.Size(62, 25);
            this.resetbutton.TabIndex = 19;
            this.resetbutton.Text = "Reset";
            this.resetbutton.UseVisualStyleBackColor = true;
            this.resetbutton.Click += new System.EventHandler(this.resetbutton_Click);
            // 
            // onoffbutton
            // 
            this.onoffbutton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onoffbutton.Location = new System.Drawing.Point(11, 403);
            this.onoffbutton.Name = "onoffbutton";
            this.onoffbutton.Size = new System.Drawing.Size(59, 25);
            this.onoffbutton.TabIndex = 18;
            this.onoffbutton.Text = "Start";
            this.onoffbutton.UseVisualStyleBackColor = true;
            this.onoffbutton.Click += new System.EventHandler(this.onoffbutton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 14);
            this.label10.TabIndex = 17;
            this.label10.Text = "Estimated Time to Level";
            // 
            // expHrText
            // 
            this.expHrText.AutoSize = true;
            this.expHrText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expHrText.Location = new System.Drawing.Point(7, 81);
            this.expHrText.Name = "expHrText";
            this.expHrText.Size = new System.Drawing.Size(56, 14);
            this.expHrText.TabIndex = 15;
            this.expHrText.Text = "EXP/Hour";
            // 
            // expGainedText
            // 
            this.expGainedText.AutoSize = true;
            this.expGainedText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expGainedText.Location = new System.Drawing.Point(158, 81);
            this.expGainedText.Name = "expGainedText";
            this.expGainedText.Size = new System.Drawing.Size(68, 14);
            this.expGainedText.TabIndex = 16;
            this.expGainedText.Text = "EXP Gained";
            // 
            // seperator1
            // 
            this.seperator1.Location = new System.Drawing.Point(9, 75);
            this.seperator1.Name = "seperator1";
            this.seperator1.Size = new System.Drawing.Size(264, 12);
            this.seperator1.TabIndex = 14;
            this.seperator1.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯";
            // 
            // expPercentNumDisplay
            // 
            this.expPercentNumDisplay.AutoSize = true;
            this.expPercentNumDisplay.BackColor = System.Drawing.Color.Transparent;
            this.expPercentNumDisplay.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expPercentNumDisplay.ForeColor = System.Drawing.Color.Gray;
            this.expPercentNumDisplay.Location = new System.Drawing.Point(250, 38);
            this.expPercentNumDisplay.Name = "expPercentNumDisplay";
            this.expPercentNumDisplay.Size = new System.Drawing.Size(27, 14);
            this.expPercentNumDisplay.TabIndex = 13;
            this.expPercentNumDisplay.Text = "EXP";
            // 
            // expProgressDisplay
            // 
            this.expProgressDisplay.Location = new System.Drawing.Point(90, 38);
            this.expProgressDisplay.Name = "expProgressDisplay";
            this.expProgressDisplay.Size = new System.Drawing.Size(156, 15);
            this.expProgressDisplay.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.expProgressDisplay.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "The EXP Guage XP/Hr updates in realtime, the rest updates per in-game exp gain..";
            // 
            // requireXPDisplay
            // 
            this.requireXPDisplay.AutoSize = true;
            this.requireXPDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requireXPDisplay.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.requireXPDisplay.Location = new System.Drawing.Point(8, 56);
            this.requireXPDisplay.Name = "requireXPDisplay";
            this.requireXPDisplay.Size = new System.Drawing.Size(105, 15);
            this.requireXPDisplay.TabIndex = 10;
            this.requireXPDisplay.Text = "requireXPDisplay";
            // 
            // levelInDisplay
            // 
            this.levelInDisplay.AutoSize = true;
            this.levelInDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelInDisplay.ForeColor = System.Drawing.Color.Blue;
            this.levelInDisplay.Location = new System.Drawing.Point(6, 138);
            this.levelInDisplay.Name = "levelInDisplay";
            this.levelInDisplay.Size = new System.Drawing.Size(124, 24);
            this.levelInDisplay.TabIndex = 9;
            this.levelInDisplay.Text = "levelInDisplay";
            // 
            // expPerSecondDisplay
            // 
            this.expPerSecondDisplay.AutoSize = true;
            this.expPerSecondDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.expPerSecondDisplay.ForeColor = System.Drawing.Color.Green;
            this.expPerSecondDisplay.Location = new System.Drawing.Point(9, 222);
            this.expPerSecondDisplay.Name = "expPerSecondDisplay";
            this.expPerSecondDisplay.Size = new System.Drawing.Size(146, 17);
            this.expPerSecondDisplay.TabIndex = 8;
            this.expPerSecondDisplay.Text = "expPerSecondDisplay";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(286, 433);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "About";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(13, 302);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(251, 71);
            this.label8.TabIndex = 6;
            this.label8.Text = "NOTE: Some data may seem inaccurate if you\'ve only done a few fights, I would rec" +
                "ommend fighting for at least 15-20 minutes before getting a realistic representa" +
                "tion.";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(210, 384);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "version 0.9";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(13, 260);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 32);
            this.label6.TabIndex = 4;
            this.label6.Text = "Email: Josh@viion.co.uk";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(14, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(251, 32);
            this.label5.TabIndex = 3;
            this.label5.Text = "If you have any issues please let me know right away, this is still in BETA.";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(251, 102);
            this.label4.TabIndex = 2;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(13, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Viion";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Developed by";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label9.Location = new System.Drawing.Point(257, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 14);
            this.label9.TabIndex = 17;
            this.label9.Text = "BETA";
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(65, 502);
            this.hScrollBar1.Minimum = 30;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(227, 18);
            this.hScrollBar1.TabIndex = 18;
            this.hScrollBar1.Tag = "gh";
            this.hScrollBar1.Value = 100;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // stayOnTopCheckBox
            // 
            this.stayOnTopCheckBox.AutoSize = true;
            this.stayOnTopCheckBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stayOnTopCheckBox.ForeColor = System.Drawing.Color.Gray;
            this.stayOnTopCheckBox.Location = new System.Drawing.Point(2, 502);
            this.stayOnTopCheckBox.Name = "stayOnTopCheckBox";
            this.stayOnTopCheckBox.Size = new System.Drawing.Size(65, 18);
            this.stayOnTopCheckBox.TabIndex = 19;
            this.stayOnTopCheckBox.Text = "On Top";
            this.stayOnTopCheckBox.UseVisualStyleBackColor = true;
            this.stayOnTopCheckBox.CheckedChanged += new System.EventHandler(this.alwaysOnTop_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(158, 246);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(87, 14);
            this.label20.TabIndex = 39;
            this.label20.Text = "High / Low EXP";
            // 
            // highlowexpDisplay
            // 
            this.highlowexpDisplay.AutoSize = true;
            this.highlowexpDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.highlowexpDisplay.ForeColor = System.Drawing.Color.Green;
            this.highlowexpDisplay.Location = new System.Drawing.Point(159, 260);
            this.highlowexpDisplay.Name = "highlowexpDisplay";
            this.highlowexpDisplay.Size = new System.Drawing.Size(123, 17);
            this.highlowexpDisplay.TabIndex = 38;
            this.highlowexpDisplay.Text = "highlowexpDisplay";
            // 
            // deathDisplay
            // 
            this.deathDisplay.AutoSize = true;
            this.deathDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deathDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.deathDisplay.Location = new System.Drawing.Point(8, 370);
            this.deathDisplay.Name = "deathDisplay";
            this.deathDisplay.Size = new System.Drawing.Size(78, 15);
            this.deathDisplay.TabIndex = 40;
            this.deathDisplay.Text = "Total Deaths";
            // 
            // AionAPP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 519);
            this.Controls.Add(this.stayOnTopCheckBox);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.playerNameDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AionAPP";
            this.Text = "AionAPP *beta";
            this.Load += new System.EventHandler(this.AionAPP_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label playerNameDisplay;
        private System.Windows.Forms.Label currentXPDisplay;
        private System.Windows.Forms.Label xpHourDisplay;
        private System.Windows.Forms.Label xpGainedDisplay;
        private System.Windows.Forms.Label timerTickDisplay;
        private System.Windows.Forms.Label avgFightTimeDisplay;
        private System.Windows.Forms.Label avgExpGainDisplay;
        private System.Windows.Forms.Label totalFights;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label expPerSecondDisplay;
        private System.Windows.Forms.Label levelInDisplay;
        private System.Windows.Forms.Label requireXPDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar expProgressDisplay;
        private System.Windows.Forms.Label expPercentNumDisplay;
        private System.Windows.Forms.Label seperator1;
        private System.Windows.Forms.Label expHrText;
        private System.Windows.Forms.Label expGainedText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button onoffbutton;
        private System.Windows.Forms.Button resetbutton;
        private System.Windows.Forms.Label seperator2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label expPerMinuteDisplay;
        private System.Windows.Forms.Label seperator3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label fightsToLvDisplay;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label levelsGainedDisplay;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.CheckBox stayOnTopCheckBox;
        private System.Windows.Forms.Label statusBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label killsHourDisplay;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label killsMinuteDisplay;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label fastestKillDisplay;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label highlowexpDisplay;
        private System.Windows.Forms.Label deathDisplay;
    }
}

