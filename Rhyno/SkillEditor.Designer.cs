namespace AngelBot
{
    partial class SkillEditor
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
            this.label66 = new System.Windows.Forms.Label();
            this.txtcasttime = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.hotkey = new System.Windows.Forms.TextBox();
            this.button27 = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcasttimemod = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.skilltree = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtcastpercent = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(171, 94);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(100, 13);
            this.label66.TabIndex = 27;
            this.label66.Text = "Base CastTime(ms):";
            // 
            // txtcasttime
            // 
            this.txtcasttime.Enabled = false;
            this.txtcasttime.Location = new System.Drawing.Point(174, 110);
            this.txtcasttime.Name = "txtcasttime";
            this.txtcasttime.Size = new System.Drawing.Size(118, 20);
            this.txtcasttime.TabIndex = 26;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(171, 54);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(44, 13);
            this.label40.TabIndex = 25;
            this.label40.Text = "Hotkey:";
            // 
            // hotkey
            // 
            this.hotkey.Location = new System.Drawing.Point(174, 70);
            this.hotkey.Name = "hotkey";
            this.hotkey.Size = new System.Drawing.Size(118, 20);
            this.hotkey.TabIndex = 24;
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(12, 442);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(103, 41);
            this.button27.TabIndex = 23;
            this.button27.Text = "Populate/Refresh Skills";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(322, 9);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(100, 20);
            this.label50.TabIndex = 60;
            this.label50.Text = "Active Skill Tree:";
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(174, 28);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(118, 20);
            this.textBox20.TabIndex = 16;
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(12, 28);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(150, 394);
            this.listBox4.TabIndex = 15;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(172, 367);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(117, 23);
            this.button6.TabIndex = 35;
            this.button6.Text = "Clear List";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(171, 338);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(118, 23);
            this.button9.TabIndex = 32;
            this.button9.Text = "Remove Selected";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "CastTime Modifer(ms):";
            // 
            // txtcasttimemod
            // 
            this.txtcasttimemod.Location = new System.Drawing.Point(174, 152);
            this.txtcasttimemod.Name = "txtcasttimemod";
            this.txtcasttimemod.Size = new System.Drawing.Size(118, 20);
            this.txtcasttimemod.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "SKILL LIST";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // skilltree
            // 
            this.skilltree.AllowDrop = true;
            this.skilltree.HotTracking = true;
            this.skilltree.Location = new System.Drawing.Point(298, 28);
            this.skilltree.Name = "skilltree";
            this.skilltree.Size = new System.Drawing.Size(248, 394);
            this.skilltree.TabIndex = 53;
            this.skilltree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.skilltree_AfterSelect_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(171, 290);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 42);
            this.button1.TabIndex = 54;
            this.button1.Text = "Add to list =>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(121, 442);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 41);
            this.button2.TabIndex = 55;
            this.button2.Text = "Save ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(230, 442);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 41);
            this.button3.TabIndex = 56;
            this.button3.Text = "Load ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "Castable at HP%:";
            // 
            // txtcastpercent
            // 
            this.txtcastpercent.Location = new System.Drawing.Point(174, 192);
            this.txtcastpercent.Name = "txtcastpercent";
            this.txtcastpercent.Size = new System.Drawing.Size(118, 20);
            this.txtcastpercent.TabIndex = 57;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(375, 428);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 31);
            this.button4.TabIndex = 59;
            this.button4.Text = "Update Selected Skill";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Skill Name:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(175, 220);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 17);
            this.checkBox1.TabIndex = 62;
            this.checkBox1.Text = "Chain Skill";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(552, 87);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(46, 64);
            this.button5.TabIndex = 63;
            this.button5.Text = "Up";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(552, 157);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(46, 64);
            this.button7.TabIndex = 64;
            this.button7.Text = "Down";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // SkillEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 495);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtcastpercent);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.skilltree);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtcasttimemod);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label66);
            this.Controls.Add(this.txtcasttime);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.hotkey);
            this.Controls.Add(this.button27);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.listBox4);
            this.Name = "SkillEditor";
            this.Text = "SkillEditor";
            this.Load += new System.EventHandler(this.SkillEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.TextBox txtcasttime;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox hotkey;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtcasttimemod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView skilltree;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtcastpercent;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
    }
}