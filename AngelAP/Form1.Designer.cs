using System;
using System.Diagnostics;


namespace AngelBot
{
    partial class Form1
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
            this.button1.Visible = true;
            this.btnstop.Visible = false;
            timer1.Stop();
            //timer2.Stop();
            tmrtabby.Stop();
            tmrstuck.Stop();
            tmrpot.Stop();
            //tmrheal.Stop();
            Environment.Exit(1);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.playerName = new System.Windows.Forms.Label();
            this.playerExp = new System.Windows.Forms.Label();
            this.playerProg = new System.Windows.Forms.ProgressBar();
            this.expHRLabel = new System.Windows.Forms.Label();
            this.kinahLabel = new System.Windows.Forms.Label();
            this.resetAll = new System.Windows.Forms.Button();
            this.elapsedLabel = new System.Windows.Forms.Label();
            this.killLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.healthbar = new System.Windows.Forms.ProgressBar();
            this.manabar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lblxpgain = new System.Windows.Forms.Label();
            this.lbltarget = new System.Windows.Forms.Label();
            this.tarhealth = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.btnignore = new System.Windows.Forms.Button();
            this.btnuningnore = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnstop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnwaypoint = new System.Windows.Forms.Button();
            this.btnautoway = new System.Windows.Forms.Button();
            this.listkeypress = new System.Windows.Forms.ListBox();
            this.lbldistance = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.tmrstuck = new System.Windows.Forms.Timer(this.components);
            this.tmrpot = new System.Windows.Forms.Timer(this.components);
            this.lblwprange = new System.Windows.Forms.Label();
            this.tmrtabby = new System.Windows.Forms.Timer(this.components);
            this.lbltimelvl = new System.Windows.Forms.Label();
            this.tmrunstuck = new System.Windows.Forms.Timer(this.components);
            this.tmrshutoff = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SetVender = new System.Windows.Forms.Button();
            this.tmrtoreturn = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keypressWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waypoint = new System.Windows.Forms.ToolStripMenuItem();
            this.clearWaypointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWaypointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWaypointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waypointEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skillEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.healBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.showkp = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dpstimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tmrheart = new System.Windows.Forms.Timer(this.components);
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // playerName
            // 
            this.playerName.AutoSize = true;
            this.playerName.BackColor = System.Drawing.Color.Gainsboro;
            this.playerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerName.ForeColor = System.Drawing.Color.DarkOrange;
            this.playerName.Location = new System.Drawing.Point(6, 12);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(61, 22);
            this.playerName.TabIndex = 0;
            this.playerName.Text = "Player";
            // 
            // playerExp
            // 
            this.playerExp.AutoSize = true;
            this.playerExp.Location = new System.Drawing.Point(-3, 211);
            this.playerExp.Name = "playerExp";
            this.playerExp.Size = new System.Drawing.Size(28, 13);
            this.playerExp.TabIndex = 2;
            this.playerExp.Text = "Exp:";
            this.playerExp.Click += new System.EventHandler(this.playerExp_Click_1);
            // 
            // playerProg
            // 
            this.playerProg.Location = new System.Drawing.Point(-4, 227);
            this.playerProg.Name = "playerProg";
            this.playerProg.Size = new System.Drawing.Size(477, 15);
            this.playerProg.TabIndex = 3;
            // 
            // expHRLabel
            // 
            this.expHRLabel.AutoSize = true;
            this.expHRLabel.Location = new System.Drawing.Point(6, 120);
            this.expHRLabel.Name = "expHRLabel";
            this.expHRLabel.Size = new System.Drawing.Size(39, 13);
            this.expHRLabel.TabIndex = 4;
            this.expHRLabel.Text = "ExpHr:";
            // 
            // kinahLabel
            // 
            this.kinahLabel.AutoSize = true;
            this.kinahLabel.Location = new System.Drawing.Point(6, 72);
            this.kinahLabel.Name = "kinahLabel";
            this.kinahLabel.Size = new System.Drawing.Size(37, 13);
            this.kinahLabel.TabIndex = 5;
            this.kinahLabel.Text = "Kinah:";
            // 
            // resetAll
            // 
            this.resetAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetAll.Location = new System.Drawing.Point(64, 0);
            this.resetAll.Name = "resetAll";
            this.resetAll.Size = new System.Drawing.Size(46, 20);
            this.resetAll.TabIndex = 6;
            this.resetAll.Text = "Reset";
            this.resetAll.UseVisualStyleBackColor = true;
            this.resetAll.Click += new System.EventHandler(this.resetAll_Click);
            // 
            // elapsedLabel
            // 
            this.elapsedLabel.AutoSize = true;
            this.elapsedLabel.Location = new System.Drawing.Point(5, 20);
            this.elapsedLabel.Name = "elapsedLabel";
            this.elapsedLabel.Size = new System.Drawing.Size(36, 13);
            this.elapsedLabel.TabIndex = 8;
            this.elapsedLabel.Text = "Timer:";
            // 
            // killLabel
            // 
            this.killLabel.AutoSize = true;
            this.killLabel.Location = new System.Drawing.Point(6, 46);
            this.killLabel.Name = "killLabel";
            this.killLabel.Size = new System.Drawing.Size(28, 13);
            this.killLabel.TabIndex = 9;
            this.killLabel.Text = "Kills:";
            this.killLabel.Click += new System.EventHandler(this.killLabel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Health";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Mana";
            // 
            // healthbar
            // 
            this.healthbar.BackColor = System.Drawing.Color.Black;
            this.healthbar.ForeColor = System.Drawing.Color.LightCoral;
            this.healthbar.Location = new System.Drawing.Point(51, 37);
            this.healthbar.Name = "healthbar";
            this.healthbar.Size = new System.Drawing.Size(130, 18);
            this.healthbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.healthbar.TabIndex = 16;
            // 
            // manabar
            // 
            this.manabar.BackColor = System.Drawing.Color.Black;
            this.manabar.ForeColor = System.Drawing.Color.SkyBlue;
            this.manabar.Location = new System.Drawing.Point(51, 61);
            this.manabar.Name = "manabar";
            this.manabar.Size = new System.Drawing.Size(130, 20);
            this.manabar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.manabar.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(22, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 34);
            this.button1.TabIndex = 18;
            this.button1.Text = "Start";
            this.toolTip1.SetToolTip(this.button1, "Ctrl Z");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblxpgain
            // 
            this.lblxpgain.AutoSize = true;
            this.lblxpgain.Location = new System.Drawing.Point(6, 85);
            this.lblxpgain.Name = "lblxpgain";
            this.lblxpgain.Size = new System.Drawing.Size(24, 13);
            this.lblxpgain.TabIndex = 19;
            this.lblxpgain.Text = "XP:";
            // 
            // lbltarget
            // 
            this.lbltarget.AutoSize = true;
            this.lbltarget.BackColor = System.Drawing.Color.Gainsboro;
            this.lbltarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltarget.Location = new System.Drawing.Point(3, 17);
            this.lbltarget.Name = "lbltarget";
            this.lbltarget.Size = new System.Drawing.Size(61, 20);
            this.lbltarget.TabIndex = 20;
            this.lbltarget.Text = "Target";
            // 
            // tarhealth
            // 
            this.tarhealth.BackColor = System.Drawing.Color.Black;
            this.tarhealth.ForeColor = System.Drawing.Color.Lime;
            this.tarhealth.Location = new System.Drawing.Point(7, 55);
            this.tarhealth.Name = "tarhealth";
            this.tarhealth.Size = new System.Drawing.Size(159, 20);
            this.tarhealth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tarhealth.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Health";
            // 
            // btnignore
            // 
            this.btnignore.Location = new System.Drawing.Point(387, 30);
            this.btnignore.Name = "btnignore";
            this.btnignore.Size = new System.Drawing.Size(70, 24);
            this.btnignore.TabIndex = 23;
            this.btnignore.Text = "Ignore NPC";
            this.btnignore.UseVisualStyleBackColor = true;
            this.btnignore.Click += new System.EventHandler(this.btnignore_Click);
            // 
            // btnuningnore
            // 
            this.btnuningnore.Location = new System.Drawing.Point(459, 30);
            this.btnuningnore.Name = "btnuningnore";
            this.btnuningnore.Size = new System.Drawing.Size(62, 24);
            this.btnuningnore.TabIndex = 24;
            this.btnuningnore.Text = "Unignore";
            this.btnuningnore.UseVisualStyleBackColor = true;
            this.btnuningnore.Click += new System.EventHandler(this.btnuningnore_Click);
            // 
            // btnstop
            // 
            this.btnstop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnstop.Location = new System.Drawing.Point(22, 180);
            this.btnstop.Name = "btnstop";
            this.btnstop.Size = new System.Drawing.Size(131, 29);
            this.btnstop.TabIndex = 28;
            this.btnstop.Text = "Stop";
            this.toolTip1.SetToolTip(this.btnstop, "Ctrl Z");
            this.btnstop.UseVisualStyleBackColor = true;
            this.btnstop.Visible = false;
            this.btnstop.Click += new System.EventHandler(this.btnstop_Click_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Stats";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(185, 181);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Death Waypoints";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnwaypoint
            // 
            this.btnwaypoint.Location = new System.Drawing.Point(281, 148);
            this.btnwaypoint.Name = "btnwaypoint";
            this.btnwaypoint.Size = new System.Drawing.Size(90, 26);
            this.btnwaypoint.TabIndex = 30;
            this.btnwaypoint.Text = "Add Waypoint";
            this.btnwaypoint.UseVisualStyleBackColor = true;
            this.btnwaypoint.Click += new System.EventHandler(this.btnwaypoint_Click);
            // 
            // btnautoway
            // 
            this.btnautoway.Location = new System.Drawing.Point(185, 148);
            this.btnautoway.Name = "btnautoway";
            this.btnautoway.Size = new System.Drawing.Size(90, 26);
            this.btnautoway.TabIndex = 31;
            this.btnautoway.Text = "Auto Waypoints";
            this.btnautoway.UseVisualStyleBackColor = true;
            this.btnautoway.Click += new System.EventHandler(this.btnautoway_Click);
            // 
            // listkeypress
            // 
            this.listkeypress.FormattingEnabled = true;
            this.listkeypress.Location = new System.Drawing.Point(539, 42);
            this.listkeypress.Name = "listkeypress";
            this.listkeypress.Size = new System.Drawing.Size(191, 199);
            this.listkeypress.TabIndex = 32;
            // 
            // lbldistance
            // 
            this.lbldistance.AutoSize = true;
            this.lbldistance.Location = new System.Drawing.Point(4, 78);
            this.lbldistance.Name = "lbldistance";
            this.lbldistance.Size = new System.Drawing.Size(45, 13);
            this.lbldistance.TabIndex = 33;
            this.lbldistance.Text = "Range: ";
            this.lbldistance.Click += new System.EventHandler(this.lbldistance_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // timer3
            // 
            this.timer3.Interval = 1600;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // tmrstuck
            // 
            this.tmrstuck.Interval = 2000;
            this.tmrstuck.Tick += new System.EventHandler(this.tmrstuck_Tick_1);
            // 
            // tmrpot
            // 
            this.tmrpot.Interval = 30000;
            this.tmrpot.Tick += new System.EventHandler(this.tmrpot_Tick);
            // 
            // lblwprange
            // 
            this.lblwprange.AutoSize = true;
            this.lblwprange.Location = new System.Drawing.Point(7, 84);
            this.lblwprange.Name = "lblwprange";
            this.lblwprange.Size = new System.Drawing.Size(60, 13);
            this.lblwprange.TabIndex = 36;
            this.lblwprange.Text = "WPRange:";
            // 
            // tmrtabby
            // 
            this.tmrtabby.Interval = 1000;
            this.tmrtabby.Tick += new System.EventHandler(this.tmrtabby_Tick_1);
            // 
            // lbltimelvl
            // 
            this.lbltimelvl.AutoSize = true;
            this.lbltimelvl.Location = new System.Drawing.Point(6, 136);
            this.lbltimelvl.Name = "lbltimelvl";
            this.lbltimelvl.Size = new System.Drawing.Size(48, 13);
            this.lbltimelvl.TabIndex = 37;
            this.lbltimelvl.Text = "Level In:";
            this.lbltimelvl.Click += new System.EventHandler(this.lbltimelvl_Click);
            // 
            // tmrunstuck
            // 
            this.tmrunstuck.Interval = 10000;
            this.tmrunstuck.Tick += new System.EventHandler(this.tmrunstuck_Tick);
            // 
            // tmrshutoff
            // 
            this.tmrshutoff.Interval = 1000;
            this.tmrshutoff.Tick += new System.EventHandler(this.tmrshutoff_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Inv:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Deaths:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.elapsedLabel);
            this.groupBox1.Controls.Add(this.kinahLabel);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbltimelvl);
            this.groupBox1.Controls.Add(this.killLabel);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblxpgain);
            this.groupBox1.Controls.Add(this.expHRLabel);
            this.groupBox1.Controls.Add(this.resetAll);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(387, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 155);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stats";
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(332, 0);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblstatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 241);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(535, 22);
            this.statusStrip1.TabIndex = 45;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // lblstatus
            // 
            this.lblstatus.BackColor = System.Drawing.Color.Transparent;
            this.lblstatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(43, 17);
            this.lblstatus.Text = "status";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(541, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "Keypresses";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(185, 204);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(113, 17);
            this.checkBox2.TabIndex = 47;
            this.checkBox2.Text = "Vender Waypoints";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // SetVender
            // 
            this.SetVender.Location = new System.Drawing.Point(299, 200);
            this.SetVender.Name = "SetVender";
            this.SetVender.Size = new System.Drawing.Size(72, 22);
            this.SetVender.TabIndex = 49;
            this.SetVender.Text = "Set Vender";
            this.SetVender.UseVisualStyleBackColor = true;
            this.SetVender.Click += new System.EventHandler(this.SetVender_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.waypointEditorToolStripMenuItem,
            this.skillEditorToolStripMenuItem,
            this.healBotToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(535, 24);
            this.menuStrip1.TabIndex = 50;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.keypressWindowToolStripMenuItem,
            this.waypoint,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // keypressWindowToolStripMenuItem
            // 
            this.keypressWindowToolStripMenuItem.Name = "keypressWindowToolStripMenuItem";
            this.keypressWindowToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.keypressWindowToolStripMenuItem.Text = "Keypress window";
            this.keypressWindowToolStripMenuItem.Click += new System.EventHandler(this.keypressWindowToolStripMenuItem_Click);
            // 
            // waypoint
            // 
            this.waypoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.waypoint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearWaypointsToolStripMenuItem,
            this.loadWaypointsToolStripMenuItem,
            this.saveWaypointsToolStripMenuItem});
            this.waypoint.Name = "waypoint";
            this.waypoint.Size = new System.Drawing.Size(165, 22);
            this.waypoint.Text = "Waypoints";
            // 
            // clearWaypointsToolStripMenuItem
            // 
            this.clearWaypointsToolStripMenuItem.Name = "clearWaypointsToolStripMenuItem";
            this.clearWaypointsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.clearWaypointsToolStripMenuItem.Text = "Clear Waypoints";
            this.clearWaypointsToolStripMenuItem.Click += new System.EventHandler(this.clearWaypointsToolStripMenuItem_Click);
            // 
            // loadWaypointsToolStripMenuItem
            // 
            this.loadWaypointsToolStripMenuItem.Name = "loadWaypointsToolStripMenuItem";
            this.loadWaypointsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.loadWaypointsToolStripMenuItem.Text = "Load Waypoints";
            this.loadWaypointsToolStripMenuItem.Click += new System.EventHandler(this.loadWaypointsToolStripMenuItem_Click);
            // 
            // saveWaypointsToolStripMenuItem
            // 
            this.saveWaypointsToolStripMenuItem.Name = "saveWaypointsToolStripMenuItem";
            this.saveWaypointsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.saveWaypointsToolStripMenuItem.Text = "Save Waypoints";
            this.saveWaypointsToolStripMenuItem.Click += new System.EventHandler(this.saveWaypointsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // waypointEditorToolStripMenuItem
            // 
            this.waypointEditorToolStripMenuItem.Name = "waypointEditorToolStripMenuItem";
            this.waypointEditorToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.waypointEditorToolStripMenuItem.Text = "Waypoint Editor";
            this.waypointEditorToolStripMenuItem.Click += new System.EventHandler(this.waypointEditorToolStripMenuItem_Click);
            // 
            // skillEditorToolStripMenuItem
            // 
            this.skillEditorToolStripMenuItem.Name = "skillEditorToolStripMenuItem";
            this.skillEditorToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.skillEditorToolStripMenuItem.Text = "Skill Editor";
            this.skillEditorToolStripMenuItem.Click += new System.EventHandler(this.skillEditorToolStripMenuItem_Click);
            // 
            // healBotToolStripMenuItem
            // 
            this.healBotToolStripMenuItem.Name = "healBotToolStripMenuItem";
            this.healBotToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            this.healBotToolStripMenuItem.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblwprange);
            this.groupBox2.Controls.Add(this.manabar);
            this.groupBox2.Controls.Add(this.healthbar);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.playerName);
            this.groupBox2.Location = new System.Drawing.Point(12, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 112);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lbldistance);
            this.groupBox3.Controls.Add(this.tarhealth);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbltarget);
            this.groupBox3.Location = new System.Drawing.Point(210, 30);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(171, 115);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 15);
            this.label11.TabIndex = 34;
            this.label11.Text = "DPS:";
            // 
            // showkp
            // 
            this.showkp.Location = new System.Drawing.Point(479, 221);
            this.showkp.Name = "showkp";
            this.showkp.Size = new System.Drawing.Size(54, 19);
            this.showkp.TabIndex = 44;
            this.showkp.Text = "Show ->";
            this.showkp.UseVisualStyleBackColor = true;
            this.showkp.Click += new System.EventHandler(this.showkp_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Angelbot2";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(93, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem1.Text = "Exit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // dpstimer
            // 
            this.dpstimer.Interval = 1000;
            this.dpstimer.Tick += new System.EventHandler(this.dpstimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "PetHP:";
            this.label1.Visible = false;
            // 
            // tmrheart
            // 
            this.tmrheart.Interval = 60000;
            this.tmrheart.Tick += new System.EventHandler(this.tmrheart_Tick);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.BackColor = System.Drawing.Color.Transparent;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(426, 246);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(92, 17);
            this.checkBox3.TabIndex = 56;
            this.checkBox3.Text = "AlwaysOnTop";
            this.checkBox3.UseVisualStyleBackColor = false;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(299, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 23);
            this.button2.TabIndex = 57;
            this.button2.Text = "Show Map";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(535, 263);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.SetVender);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.showkp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listkeypress);
            this.Controls.Add(this.btnautoway);
            this.Controls.Add(this.btnwaypoint);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnuningnore);
            this.Controls.Add(this.btnignore);
            this.Controls.Add(this.btnstop);
            this.Controls.Add(this.playerProg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.playerExp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label playerName;
        private System.Windows.Forms.Label playerExp;
        private System.Windows.Forms.ProgressBar playerProg;
        private System.Windows.Forms.Label expHRLabel;
        private System.Windows.Forms.Label kinahLabel;
        private System.Windows.Forms.Button resetAll;
        private System.Windows.Forms.Label elapsedLabel;
        private System.Windows.Forms.Label killLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar healthbar;
        private System.Windows.Forms.ProgressBar manabar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblxpgain;
        private System.Windows.Forms.Label lbltarget;
        private System.Windows.Forms.ProgressBar tarhealth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnignore;
        private System.Windows.Forms.Button btnuningnore;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnwaypoint;
        private System.Windows.Forms.Button btnautoway;
        private System.Windows.Forms.ListBox listkeypress;
        private System.Windows.Forms.Label lbldistance;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer tmrstuck;
        private System.Windows.Forms.Timer tmrpot;
        public System.Windows.Forms.Timer tmrtabby;
        private System.Windows.Forms.Label lbltimelvl;
        private System.Windows.Forms.Timer tmrunstuck;
        private System.Windows.Forms.Timer tmrshutoff;
        public System.Windows.Forms.Label lblwprange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblstatus;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button SetVender;
        private System.Windows.Forms.Timer tmrtoreturn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keypressWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waypoint;
        private System.Windows.Forms.ToolStripMenuItem clearWaypointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWaypointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWaypointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waypointEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skillEditorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button showkp;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        public System.Windows.Forms.Timer dpstimer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem healBotToolStripMenuItem;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmrheart;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button2;
    }
}

