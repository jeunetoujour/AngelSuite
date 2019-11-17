namespace Auto_Skill
{
    partial class FormMain
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
            this.skilllist = new System.Windows.Forms.ListView();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.skilltree = new System.Windows.Forms.TreeView();
            this.addskill = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.clearnode = new System.Windows.Forms.Button();
            this.loadnodes = new System.Windows.Forms.Button();
            this.savenodes = new System.Windows.Forms.Button();
            this.delnode = new System.Windows.Forms.Button();
            this.newchain = new System.Windows.Forms.Button();
            this.BExit = new System.Windows.Forms.Button();
            this.BStart = new System.Windows.Forms.Button();
            this.delskill = new System.Windows.Forms.Button();
            this.PlayerTimer = new System.Windows.Forms.Timer(this.components);
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // skilllist
            // 
            this.skilllist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader14,
            this.columnHeader15});
            this.skilllist.FullRowSelect = true;
            this.skilllist.Location = new System.Drawing.Point(12, 15);
            this.skilllist.Name = "skilllist";
            this.skilllist.Size = new System.Drawing.Size(188, 153);
            this.skilllist.TabIndex = 3;
            this.skilllist.UseCompatibleStateImageBehavior = false;
            this.skilllist.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Skill ID";
            this.columnHeader14.Width = 48;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Skill Name";
            this.columnHeader15.Width = 141;
            // 
            // skilltree
            // 
            this.skilltree.LabelEdit = true;
            this.skilltree.Location = new System.Drawing.Point(221, 13);
            this.skilltree.Name = "skilltree";
            this.skilltree.Size = new System.Drawing.Size(163, 126);
            this.skilltree.TabIndex = 4;
            // 
            // addskill
            // 
            this.addskill.Location = new System.Drawing.Point(12, 174);
            this.addskill.Name = "addskill";
            this.addskill.Size = new System.Drawing.Size(93, 23);
            this.addskill.TabIndex = 5;
            this.addskill.Text = "Get Skills";
            this.addskill.UseVisualStyleBackColor = true;
            this.addskill.Click += new System.EventHandler(this.addskill_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(368, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Slots 1-9 Only! You Can Use Upper Slots! Just Put % or ^ Infront Of Your Key";
            // 
            // clearnode
            // 
            this.clearnode.Location = new System.Drawing.Point(340, 145);
            this.clearnode.Name = "clearnode";
            this.clearnode.Size = new System.Drawing.Size(47, 23);
            this.clearnode.TabIndex = 19;
            this.clearnode.Text = "Clear";
            this.clearnode.UseVisualStyleBackColor = true;
            this.clearnode.Click += new System.EventHandler(this.clearnode_Click);
            // 
            // loadnodes
            // 
            this.loadnodes.Location = new System.Drawing.Point(312, 174);
            this.loadnodes.Name = "loadnodes";
            this.loadnodes.Size = new System.Drawing.Size(75, 23);
            this.loadnodes.TabIndex = 18;
            this.loadnodes.Text = "Load";
            this.loadnodes.UseVisualStyleBackColor = true;
            this.loadnodes.Click += new System.EventHandler(this.loadnodes_Click);
            // 
            // savenodes
            // 
            this.savenodes.Location = new System.Drawing.Point(224, 174);
            this.savenodes.Name = "savenodes";
            this.savenodes.Size = new System.Drawing.Size(75, 23);
            this.savenodes.TabIndex = 17;
            this.savenodes.Text = "Save";
            this.savenodes.UseVisualStyleBackColor = true;
            this.savenodes.Click += new System.EventHandler(this.savenodes_Click);
            // 
            // delnode
            // 
            this.delnode.Location = new System.Drawing.Point(287, 145);
            this.delnode.Name = "delnode";
            this.delnode.Size = new System.Drawing.Size(47, 23);
            this.delnode.TabIndex = 16;
            this.delnode.Text = "Del";
            this.delnode.UseVisualStyleBackColor = true;
            this.delnode.Click += new System.EventHandler(this.delnode_Click);
            // 
            // newchain
            // 
            this.newchain.Location = new System.Drawing.Point(224, 145);
            this.newchain.Name = "newchain";
            this.newchain.Size = new System.Drawing.Size(57, 23);
            this.newchain.TabIndex = 15;
            this.newchain.Text = "Chain";
            this.newchain.UseVisualStyleBackColor = true;
            this.newchain.Click += new System.EventHandler(this.newchain_Click);
            // 
            // BExit
            // 
            this.BExit.Location = new System.Drawing.Point(308, 300);
            this.BExit.Name = "BExit";
            this.BExit.Size = new System.Drawing.Size(75, 23);
            this.BExit.TabIndex = 22;
            this.BExit.Text = "Exit";
            this.BExit.UseVisualStyleBackColor = true;
            this.BExit.Click += new System.EventHandler(this.BExit_Click);
            // 
            // BStart
            // 
            this.BStart.Location = new System.Drawing.Point(224, 300);
            this.BStart.Name = "BStart";
            this.BStart.Size = new System.Drawing.Size(75, 23);
            this.BStart.TabIndex = 21;
            this.BStart.Text = "Start";
            this.BStart.UseVisualStyleBackColor = true;
            this.BStart.Click += new System.EventHandler(this.BStart_Click);
            // 
            // delskill
            // 
            this.delskill.Location = new System.Drawing.Point(115, 174);
            this.delskill.Name = "delskill";
            this.delskill.Size = new System.Drawing.Size(85, 23);
            this.delskill.TabIndex = 23;
            this.delskill.Text = ">>";
            this.delskill.UseVisualStyleBackColor = true;
            this.delskill.Click += new System.EventHandler(this.delskill_Click);
            // 
            // PlayerTimer
            // 
            this.PlayerTimer.Tick += new System.EventHandler(this.PlayerTimer_Tick);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Location = new System.Drawing.Point(18, 227);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(365, 67);
            this.textBoxMsg.TabIndex = 24;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 329);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.delskill);
            this.Controls.Add(this.BExit);
            this.Controls.Add(this.BStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clearnode);
            this.Controls.Add(this.loadnodes);
            this.Controls.Add(this.savenodes);
            this.Controls.Add(this.delnode);
            this.Controls.Add(this.newchain);
            this.Controls.Add(this.addskill);
            this.Controls.Add(this.skilltree);
            this.Controls.Add(this.skilllist);
            this.Name = "FormMain";
            this.Text = "IceJunkie Is Junk Again!";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView skilllist;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        public System.Windows.Forms.TreeView skilltree;
        private System.Windows.Forms.Button addskill;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button clearnode;
        private System.Windows.Forms.Button loadnodes;
        private System.Windows.Forms.Button savenodes;
        private System.Windows.Forms.Button delnode;
        private System.Windows.Forms.Button newchain;
        private System.Windows.Forms.Button BExit;
        private System.Windows.Forms.Button BStart;
        private System.Windows.Forms.Button delskill;
        private System.Windows.Forms.Timer PlayerTimer;
        private System.Windows.Forms.TextBox textBoxMsg;
    }
}

