namespace ABHeal
{
    using ABHeal.Properties;
    using System.Diagnostics;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using AngelRead;

    public class Form1 : Form
    {
        private CheckBox bBOHCheck;
        private ComboBox bBOHKey;
        private CheckBox bBORCheck;
        private ComboBox bBORKey;
        private Button bStart;
        private Button bStop;
        private bool casting;
        private DateTime castStart;
        private double castTime;
        private DateTime chaseTime;
        private System.Windows.Forms.Timer clock;
        private IContainer components;
        //private EntityList eList;
        private DateTime flightWarn;
        private bool flying;
        private bool following;
        private CheckBox hFRCheck;
        private ComboBox hFRKey;
        private DateTime hFRTime;
        private CheckBox hHLCheck;
        private ComboBox hHLKey;
        private DateTime hHLTime;
        private CheckBox hLRCheck;
        private ComboBox hLRKey;
        private DateTime hLRTime;
        private CheckBox hRCCheck;
        private ComboBox hRCKey;
        private DateTime hRCTime;
        private CheckBox hResCheck;
        private ComboBox hResKey;
        private SKeys keys;
        private Label label1;
        private Label label10;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblStatus;
        private bool lost;
        private DateTime manageTime;
        private PictureBox p1Ico;
        private TextBox p1Name;
        private ComboBox p1Type;
        private PictureBox p2Ico;
        private TextBox p2Name;
        private ComboBox p2Type;
        private PictureBox p3Ico;
        private TextBox p3Name;
        private ComboBox p3Type;
        private PictureBox p4Ico;
        private TextBox p4Name;
        private ComboBox p4Type;
        private PictureBox p5Ico;
        private TextBox p5Name;
        private ComboBox p5Type;
        //private Player player;
        private PEntity[] players;
        private System.Diagnostics.Process proc;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        //private Target target;
        private Label label11;
        private CheckBox checkBox1;
        private CheckBox uiCheckGroup;
        public Offsets aionOffsets;

        public EntityList eList = new EntityList();
        public Player pc = new Player();
        private ComboBox pidBox;
        public Target tar = new Target();
        public uint gamedll;
        public uint currentPID;

        public Form1()
        {
            this.InitializeComponent();
            this.LoadSettings();
            this.castStart = this.hRCTime = this.hHLTime = this.hFRTime = this.hLRTime = this.flightWarn = this.chaseTime = this.manageTime = DateTime.Now;
            this.clock = new System.Windows.Forms.Timer();
            this.clock.Interval = 100;
            this.clock.Tick += new EventHandler(this.clock_Tick);
            System.Diagnostics.Process[] ProcessList = System.Diagnostics.Process.GetProcesses();
            String ProcessName;

            pidBox.Items.Clear();
            for (int i = 0; i < ProcessList.Length; i++)
            {
                ProcessName = ProcessList[i].ProcessName;

                if (ProcessName == "aion.bin")
                {
                    if ((ProcessList[i].MainWindowTitle.IndexOf("Aion Client") > -1))
                    {

                    }
                    else
                    {
                        pidBox.Items.Add("aion.bin" + " - " + ProcessList[i].Id);
                        pidBox.SelectedIndex = 0;
                        System.Diagnostics.Process HandleP = System.Diagnostics.Process.GetProcessById(ProcessList[i].Id);

                        foreach (System.Diagnostics.ProcessModule Module in HandleP.Modules)
                        {
                            if ("Game.dll" == Module.ModuleName)
                            {
                                gamedll = (uint)Module.BaseAddress.ToInt32();
                            }
                        }
                    }
                }
            }
        }

        public void BasicHeal(PEntity p)
        {
            this.PSelect(p.Name.ToString());
            if (((p.Health < 40) && (this.ElapsedTime(this.hFRTime) > 30000.0)) && this.hFRCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hFRKey.Text));
                Thread.Sleep(100);
                this.hFRTime = DateTime.Now;
            }
            else if (((p.Health < 60) && (this.ElapsedTime(this.hLRTime) > 2000.0)) && this.hLRCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hLRKey.Text));
                Thread.Sleep(0x3e8);
                this.hLRTime = DateTime.Now;
            }
            else if (((p.Health < 90) && (this.ElapsedTime(this.hHLTime) > 100.0)) && this.hHLCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hHLKey.Text));
                Thread.Sleep(0x7d0);
                this.hHLTime = DateTime.Now;
            }
            this.following = false;
        }

        private void bBOHCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.bBOHCheck = this.bBOHCheck.Checked;
            Settings.Default.Save();
        }

        private void bBOHKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.bBOHKey = this.bBOHKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void bBORCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.bBORCheck = this.bBORCheck.Checked;
            Settings.Default.Save();
        }

        private void bBORKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.bBORKey = this.bBORKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            string AionID = pidBox.Text;
            int AionIDPosition = AionID.IndexOf(" - ");
            AionID = AionID.Remove(0, AionIDPosition + 3);
            currentPID = (uint)Convert.ToUInt32(AionID); 

            if (AngelRead.Process.Open((int)currentPID))
            {
                this.lblStatus.Text = "Status: Started!";
                this.proc = System.Diagnostics.Process.GetProcessById((int)currentPID);
                aionOffsets = new Offsets();
                aionOffsets.Update();

                pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;
                pc.PLAYER_GUID_OFFSET = aionOffsets.pGUIDInfoAddress;
                pc.ENTITY_OFFSET = aionOffsets.entityInfoAddress;
                pc.PLAYER_INVENTORY_OFFSET = aionOffsets.inventoryInfoAddress;

                tar.TARGETPTR_OFFSET = aionOffsets.targetInfoAddress;
                eList.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;

                this.players = new PEntity[2];
                this.keys = new SKeys(this.proc);
                this.pc.Updatenamelvl();

                this.eList.Update();
                this.LoadPlayers();
                if ((this.players[0] == null) || (this.players[0].PtrEntity == 0))
                {
                    MessageBox.Show("Main not found!");
                    AngelRead.Process.Close();
                    this.lblStatus.Text = "Status: Stopped...";
                }
                else
                {
                    this.PlayerUpdate();
                    this.pc.Updatenamelvl();
                    this.pc.UpdateRot();
                    this.pc.Updateafterkill();
                    this.clock.Start();
                }
            }
            else
            {
                MessageBox.Show("Aion Not Found!");
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            this.clock.Stop();
            AngelRead.Process.Close();
            this.following = false;
            this.lost = false;
            this.lblStatus.Text = "Status: Stopped.";
        }

        public void Buff()
        {
            for (int i = 0; i < 2; i++)
            {
                if (((this.players[i] != null) && (this.players[i].Distance2D(this.pc.X, this.pc.Y) < 15.0)) && (((this.ElapsedTime(this.players[i].bBOHTime) > 3600000.0) && this.bBOHCheck.Checked) || ((this.ElapsedTime(this.players[i].bBORTime) > 3600000.0) && this.bBORCheck.Checked)))
                {
                    this.following = false;
                    this.PSelect(this.players[i].Name.ToString());
                    if (this.bBOHCheck.Checked)
                    {
                        this.players[i].bBOHTime = DateTime.Now;
                        this.keys.SendKey(this.FindKey(this.bBOHKey.Text));
                        Thread.Sleep(0x3e8);
                    }
                    if (this.bBORCheck.Checked)
                    {
                        this.players[i].bBORTime = DateTime.Now;
                        this.keys.SendKey(this.FindKey(this.bBORKey.Text));
                        Thread.Sleep(0x3e8);
                    }
                }
            }
        }

        private void clock_Tick(object sender, EventArgs e)
        {
            this.PlayerUpdate();
            if (this.ElapsedTime(this.manageTime) > 1000.0)
            {
                this.eList.Update();
                this.ManagePlayers();
                this.manageTime = DateTime.Now;
            }
            if (!this.Heal() && !this.Resurect())
            {
                this.Buff();
                this.Follow();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public double ElapsedTime(DateTime time)
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - time);
            return span.TotalMilliseconds;
        }

        private int FindKey(string k)
        {
            switch (k)
            {
                case "1":
                    return 0x31;

                case "2":
                    return 50;

                case "3":
                    return 0x33;

                case "4":
                    return 0x34;

                case "5":
                    return 0x35;

                case "6":
                    return 0x36;

                case "7":
                    return 0x37;

                case "8":
                    return 0x38;

                case "9":
                    return 0x39;

                case "0":
                    return 0x30;

                case "-":
                    return 0x6d;

                case "+":
                    return 0x6b;
            }
            return 0x31;
        }

        private uint FindPlayer(string name)
        {
            foreach (Entity entity in this.eList)
            {
                if (entity.Name.ToString() == name)
                {
                    return (uint)entity.PtrEntity;
                }
            }
            return 0;
        }

        private void Follow()
        {
            PEntity entity = this.players[0];
            if ((entity != null) && (entity.PtrEntity != 0))
            {
                double num = 5;//(((double)this.pc.FlightTime) / ((double)this.pc.MaxFlightTime)) * 100.0;
                bool flag = (entity.Stance == eStance.Flying) || (entity.Stance == eStance.FlyingCombat);
                if ((flag && !this.flying) && (num > 50.0))
                {
                    this.flying = true;
                    this.following = false;
                    this.keys.SendKey(0x21);
                    Thread.Sleep(0xbb8);
                }
                else if (!flag && this.flying)
                {
                    this.flying = false;
                    this.following = false;
                    this.keys.SendKey(0x22);
                    Thread.Sleep(0x3e8);
                }
                else if (((num < 25.0) && this.flying) && (this.ElapsedTime(this.flightWarn) > 10000.0))
                {
                    this.flightWarn = DateTime.Now;
                    this.keys.SendLine("/w " + this.players[0].Name.ToString() + " I need to land soon.");
                }
                else if ((num < 1.0) && this.flying)
                {
                    this.flying = false;
                }
                double num2 = this.pc.Distance2D(entity.X, entity.Y);
                if ((num2 < 15.0) && !this.following)
                {
                    this.PSelectMain();
                    this.following = true;
                    this.lost = false;
                }
                else if (((num2 > 15.0) && (num2 < 100.0)) && !this.lost)
                {
                    if (this.ElapsedTime(this.chaseTime) > 500.0)
                    {
                        this.keys.SendKey(0x43);
                        this.chaseTime = DateTime.Now;
                    }
                    this.following = false;
                    this.lost = false;
                }
                else if ((num2 > 200.0) && !this.lost)
                {
                    this.lost = true;
                    this.following = false;
                    this.keys.SendLine("/w " + this.players[0].Name.ToString() + " You lost me.");
                }
            }
        }

        public bool Heal()
        {
            double num = (((double)this.pc.Health) / ((double)this.pc.MaxHealth)) * 100.0;
            bool flag = false;
            for (int i = 0; i < 2; i++)
            {
                if ((((this.players[i] != null) && (this.players[i].Health > 0)) && ((this.players[i].Health < 90) && (this.players[i].PtrEntity != 0))) && (this.pc.Distance3D(this.players[i].X, this.players[i].Y, this.players[i].Z) < 35.0))
                {
                    flag = true;
                    break;
                }
            }
            int[] numArray = new int[2];
            if (!flag || (num <= 80.0))
            {
                if (num < 85.0)
                {
                    this.SelfHeal((int)num);
                    return true;
                }
            }
            else
            {
                int index = 0;
                foreach (PEntity entity in this.players)
                {
                    if (entity == null)
                    {
                        goto Label_0293;
                    }
                    switch (entity.type)
                    {
                        case PType.MainTank:
                            if (entity.PtrEntity != 0)
                            {
                                label11.Text = entity.Health.ToString();
                                if (entity.Health >= 30)
                                {

                                    break;
                                }
                                numArray[index] = 6;
                            }
                            goto Label_016A;

                        case PType.Warrior:
                            if (entity.PtrEntity > 0)
                            {
                                if (entity.Health >= 20)
                                {
                                    goto Label_0269;
                                }
                                numArray[index] = 3;
                            }
                            goto Label_028B;

                        case PType.Healer:
                            if (entity.PtrEntity > 0)
                            {
                                if (entity.Health >= 40)
                                {
                                    goto Label_0223;
                                }
                                numArray[index] = 4;
                            }
                            goto Label_0245;

                        case PType.Scout:
                            if (entity.PtrEntity > 0)
                            {
                                if (entity.Health >= 40)
                                {
                                    goto Label_01DA;
                                }
                                numArray[index] = 3;
                            }
                            goto Label_01FC;

                        case PType.Mage:
                            if (entity.PtrEntity > 0)
                            {
                                if (entity.Health >= 40)
                                {
                                    goto Label_0191;
                                }
                                numArray[index] = 5;
                            }
                            goto Label_01B3;

                        default:
                            goto Label_0299;
                    }

                    if (entity.Health < 50)
                    {
                        numArray[index] = 5;
                    }
                    else if (entity.Health < 0x4b)
                    {
                        numArray[index] = 4;
                    }
                    else if (entity.Health < 0x55)
                    {
                        numArray[index] = 1;
                    }
                Label_016A:
                    index++;
                    goto Label_0299;
                Label_0191:
                    if (entity.Health < 60)
                    {
                        numArray[index] = 4;
                    }
                    else if (entity.Health < 90)
                    {
                        numArray[index] = 3;
                    }
                Label_01B3:
                    index++;
                    goto Label_0299;
                Label_01DA:
                    if (entity.Health < 50)
                    {
                        numArray[index] = 2;
                    }
                    else if (entity.Health < 0x55)
                    {
                        numArray[index] = 1;
                    }
                Label_01FC:
                    index++;
                    goto Label_0299;
                Label_0223:
                    if (entity.Health < 60)
                    {
                        numArray[index] = 3;
                    }
                    else if (entity.Health < 90)
                    {
                        numArray[index] = 2;
                    }
                Label_0245:
                    index++;
                    goto Label_0299;
                Label_0269:
                    if (entity.Health < 0x4b)
                    {
                        numArray[index] = 2;
                    }
                    else if (entity.Health < 90)
                    {
                        numArray[index] = 1;
                    }
                Label_028B:
                    index++;
                    goto Label_0299;
                Label_0293:
                    index++;
                Label_0299: ;
                }
                int num4 = -1;
                int num5 = 0;
                for (int j = 0; j < 2; j++)
                {
                    if ((numArray[j] > 0) && (numArray[j] > num5))
                    {
                        num5 = numArray[j];
                        num4 = j;
                    }
                }
                //label11.Text = 
                if (num4 != -1)
                {
                    PType type = this.players[num4].type;
                    if (type != PType.MainTank)
                    {
                        if (type == PType.Mage)
                        {
                            this.MageHeal(this.players[num4]);
                            return true;
                        }
                        this.BasicHeal(this.players[num4]);
                        return true;
                    }
                    this.TankHeal(this.players[num4]);
                    return true;
                }
            }
            return false;
        }

        private void hFRCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hFRCheck = this.hFRCheck.Checked;
            Settings.Default.Save();
        }

        private void hFRKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.hFRKey = this.hFRKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void hHLCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hHLCheck = this.hHLCheck.Checked;
            Settings.Default.Save();
        }

        private void hHLKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.hHLKey = this.hHLKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void hLRCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hLRCheck = this.hLRCheck.Checked;
            Settings.Default.Save();
        }

        private void hLRKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.hLRKey = this.hLRKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void hRCCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hRCCheck = this.hRCCheck.Checked;
            Settings.Default.Save();
        }

        private void hRCKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.hRCKey = this.hRCKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void hResCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hResCheck = this.hResCheck.Checked;
            Settings.Default.Save();
        }

        private void hResKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.hResKey = this.hResKey.SelectedIndex;
            Settings.Default.Save();
        }

        private void InitializeComponent()
        {
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.hLRKey = new System.Windows.Forms.ComboBox();
            this.hRCCheck = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.hRCKey = new System.Windows.Forms.ComboBox();
            this.hLRCheck = new System.Windows.Forms.CheckBox();
            this.hFRCheck = new System.Windows.Forms.CheckBox();
            this.hHLKey = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.hFRKey = new System.Windows.Forms.ComboBox();
            this.hHLCheck = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bBORKey = new System.Windows.Forms.ComboBox();
            this.bBOHKey = new System.Windows.Forms.ComboBox();
            this.bBORCheck = new System.Windows.Forms.CheckBox();
            this.bBOHCheck = new System.Windows.Forms.CheckBox();
            this.hResKey = new System.Windows.Forms.ComboBox();
            this.hResCheck = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bStop = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.p5Type = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.p5Name = new System.Windows.Forms.TextBox();
            this.p4Name = new System.Windows.Forms.TextBox();
            this.p3Name = new System.Windows.Forms.TextBox();
            this.p2Name = new System.Windows.Forms.TextBox();
            this.p1Name = new System.Windows.Forms.TextBox();
            this.p5Ico = new System.Windows.Forms.PictureBox();
            this.p4Type = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.p4Ico = new System.Windows.Forms.PictureBox();
            this.p3Type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.p3Ico = new System.Windows.Forms.PictureBox();
            this.p2Type = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.p2Ico = new System.Windows.Forms.PictureBox();
            this.p1Type = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.p1Ico = new System.Windows.Forms.PictureBox();
            this.uiCheckGroup = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pidBox = new System.Windows.Forms.ComboBox();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.p5Ico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4Ico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3Ico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2Ico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1Ico)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.hLRKey);
            this.tabPage2.Controls.Add(this.hRCCheck);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.hRCKey);
            this.tabPage2.Controls.Add(this.hLRCheck);
            this.tabPage2.Controls.Add(this.hFRCheck);
            this.tabPage2.Controls.Add(this.hHLKey);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.hFRKey);
            this.tabPage2.Controls.Add(this.hHLCheck);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.bBORKey);
            this.tabPage2.Controls.Add(this.bBOHKey);
            this.tabPage2.Controls.Add(this.bBORCheck);
            this.tabPage2.Controls.Add(this.bBOHCheck);
            this.tabPage2.Controls.Add(this.hResKey);
            this.tabPage2.Controls.Add(this.hResCheck);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(275, 184);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Heals";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(129, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Keys";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Radiant Cure";
            // 
            // hLRKey
            // 
            this.hLRKey.FormattingEnabled = true;
            this.hLRKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.hLRKey.Location = new System.Drawing.Point(132, 62);
            this.hLRKey.Name = "hLRKey";
            this.hLRKey.Size = new System.Drawing.Size(35, 21);
            this.hLRKey.TabIndex = 33;
            // 
            // hRCCheck
            // 
            this.hRCCheck.AutoSize = true;
            this.hRCCheck.Location = new System.Drawing.Point(12, 21);
            this.hRCCheck.Name = "hRCCheck";
            this.hRCCheck.Size = new System.Drawing.Size(15, 14);
            this.hRCCheck.TabIndex = 22;
            this.hRCCheck.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Light of Recovery";
            // 
            // hRCKey
            // 
            this.hRCKey.FormattingEnabled = true;
            this.hRCKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.hRCKey.Location = new System.Drawing.Point(132, 17);
            this.hRCKey.Name = "hRCKey";
            this.hRCKey.Size = new System.Drawing.Size(35, 21);
            this.hRCKey.TabIndex = 24;
            // 
            // hLRCheck
            // 
            this.hLRCheck.AutoSize = true;
            this.hLRCheck.Location = new System.Drawing.Point(12, 66);
            this.hLRCheck.Name = "hLRCheck";
            this.hLRCheck.Size = new System.Drawing.Size(15, 14);
            this.hLRCheck.TabIndex = 31;
            this.hLRCheck.UseVisualStyleBackColor = true;
            // 
            // hFRCheck
            // 
            this.hFRCheck.AutoSize = true;
            this.hFRCheck.Location = new System.Drawing.Point(12, 89);
            this.hFRCheck.Name = "hFRCheck";
            this.hFRCheck.Size = new System.Drawing.Size(15, 14);
            this.hFRCheck.TabIndex = 25;
            this.hFRCheck.UseVisualStyleBackColor = true;
            // 
            // hHLKey
            // 
            this.hHLKey.FormattingEnabled = true;
            this.hHLKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.hHLKey.Location = new System.Drawing.Point(132, 40);
            this.hHLKey.Name = "hHLKey";
            this.hHLKey.Size = new System.Drawing.Size(35, 21);
            this.hHLKey.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Flash of Recovery";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Healing Light";
            // 
            // hFRKey
            // 
            this.hFRKey.FormattingEnabled = true;
            this.hFRKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.hFRKey.Location = new System.Drawing.Point(132, 86);
            this.hFRKey.Name = "hFRKey";
            this.hFRKey.Size = new System.Drawing.Size(35, 21);
            this.hFRKey.TabIndex = 27;
            // 
            // hHLCheck
            // 
            this.hHLCheck.AutoSize = true;
            this.hHLCheck.Location = new System.Drawing.Point(12, 44);
            this.hHLCheck.Name = "hHLCheck";
            this.hHLCheck.Size = new System.Drawing.Size(15, 14);
            this.hHLCheck.TabIndex = 28;
            this.hHLCheck.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(129, 187);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Keys";
            // 
            // bBORKey
            // 
            this.bBORKey.FormattingEnabled = true;
            this.bBORKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.bBORKey.Location = new System.Drawing.Point(132, 154);
            this.bBORKey.Name = "bBORKey";
            this.bBORKey.Size = new System.Drawing.Size(35, 21);
            this.bBORKey.TabIndex = 19;
            // 
            // bBOHKey
            // 
            this.bBOHKey.FormattingEnabled = true;
            this.bBOHKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.bBOHKey.Location = new System.Drawing.Point(132, 131);
            this.bBOHKey.Name = "bBOHKey";
            this.bBOHKey.Size = new System.Drawing.Size(35, 21);
            this.bBOHKey.TabIndex = 18;
            // 
            // bBORCheck
            // 
            this.bBORCheck.AutoSize = true;
            this.bBORCheck.Location = new System.Drawing.Point(12, 158);
            this.bBORCheck.Name = "bBORCheck";
            this.bBORCheck.Size = new System.Drawing.Size(106, 17);
            this.bBORCheck.TabIndex = 17;
            this.bBORCheck.Text = "Blessing of Rock";
            this.bBORCheck.UseVisualStyleBackColor = true;
            // 
            // bBOHCheck
            // 
            this.bBOHCheck.AutoSize = true;
            this.bBOHCheck.Location = new System.Drawing.Point(12, 135);
            this.bBOHCheck.Name = "bBOHCheck";
            this.bBOHCheck.Size = new System.Drawing.Size(111, 17);
            this.bBOHCheck.TabIndex = 16;
            this.bBOHCheck.Text = "Blessing of Health";
            this.bBOHCheck.UseVisualStyleBackColor = true;
            // 
            // hResKey
            // 
            this.hResKey.FormattingEnabled = true;
            this.hResKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "-",
            "+"});
            this.hResKey.Location = new System.Drawing.Point(132, 110);
            this.hResKey.Name = "hResKey";
            this.hResKey.Size = new System.Drawing.Size(35, 21);
            this.hResKey.TabIndex = 15;
            this.hResKey.SelectedIndexChanged += new System.EventHandler(this.hResKey_SelectedIndexChanged);
            // 
            // hResCheck
            // 
            this.hResCheck.AutoSize = true;
            this.hResCheck.Location = new System.Drawing.Point(12, 112);
            this.hResCheck.Name = "hResCheck";
            this.hResCheck.Size = new System.Drawing.Size(72, 17);
            this.hResCheck.TabIndex = 14;
            this.hResCheck.Text = "Resurrect";
            this.hResCheck.UseVisualStyleBackColor = true;
            this.hResCheck.CheckedChanged += new System.EventHandler(this.hResCheck_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pidBox);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.lblStatus);
            this.tabPage1.Controls.Add(this.bStop);
            this.tabPage1.Controls.Add(this.bStart);
            this.tabPage1.Controls.Add(this.p5Type);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.p5Name);
            this.tabPage1.Controls.Add(this.p4Name);
            this.tabPage1.Controls.Add(this.p3Name);
            this.tabPage1.Controls.Add(this.p2Name);
            this.tabPage1.Controls.Add(this.p1Name);
            this.tabPage1.Controls.Add(this.p5Ico);
            this.tabPage1.Controls.Add(this.p4Type);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.p4Ico);
            this.tabPage1.Controls.Add(this.p3Type);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.p3Ico);
            this.tabPage1.Controls.Add(this.p2Type);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.p2Ico);
            this.tabPage1.Controls.Add(this.p1Type);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.p1Ico);
            this.tabPage1.Controls.Add(this.uiCheckGroup);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(275, 184);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(173, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.Text = "inGroup?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "label11";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 103);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(70, 24);
            this.lblStatus.TabIndex = 24;
            this.lblStatus.Text = "Status: ";
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(87, 6);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(74, 33);
            this.bStop.TabIndex = 22;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(7, 6);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(74, 33);
            this.bStart.TabIndex = 21;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // p5Type
            // 
            this.p5Type.FormattingEnabled = true;
            this.p5Type.Items.AddRange(new object[] {
            "MainTank",
            "Warrior",
            "Healer",
            "Scout",
            "Mage"});
            this.p5Type.Location = new System.Drawing.Point(262, 388);
            this.p5Type.Name = "p5Type";
            this.p5Type.Size = new System.Drawing.Size(127, 21);
            this.p5Type.TabIndex = 20;
            this.p5Type.Visible = false;
            this.p5Type.SelectedIndexChanged += new System.EventHandler(this.p5Type_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Uighur", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 371);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "Member 5";
            this.label5.Visible = false;
            // 
            // p5Name
            // 
            this.p5Name.Location = new System.Drawing.Point(20, 389);
            this.p5Name.Name = "p5Name";
            this.p5Name.Size = new System.Drawing.Size(234, 20);
            this.p5Name.TabIndex = 18;
            this.p5Name.Visible = false;
            this.p5Name.TextChanged += new System.EventHandler(this.p5Name_TextChanged);
            // 
            // p4Name
            // 
            this.p4Name.Location = new System.Drawing.Point(20, 350);
            this.p4Name.Name = "p4Name";
            this.p4Name.Size = new System.Drawing.Size(234, 20);
            this.p4Name.TabIndex = 14;
            this.p4Name.Visible = false;
            this.p4Name.TextChanged += new System.EventHandler(this.p4Name_TextChanged);
            // 
            // p3Name
            // 
            this.p3Name.Location = new System.Drawing.Point(20, 309);
            this.p3Name.Name = "p3Name";
            this.p3Name.Size = new System.Drawing.Size(234, 20);
            this.p3Name.TabIndex = 10;
            this.p3Name.Visible = false;
            this.p3Name.TextChanged += new System.EventHandler(this.p3Name_TextChanged);
            // 
            // p2Name
            // 
            this.p2Name.Location = new System.Drawing.Point(20, 269);
            this.p2Name.Name = "p2Name";
            this.p2Name.Size = new System.Drawing.Size(234, 20);
            this.p2Name.TabIndex = 6;
            this.p2Name.Visible = false;
            this.p2Name.TextChanged += new System.EventHandler(this.p2Name_TextChanged);
            // 
            // p1Name
            // 
            this.p1Name.Location = new System.Drawing.Point(8, 71);
            this.p1Name.Name = "p1Name";
            this.p1Name.Size = new System.Drawing.Size(234, 20);
            this.p1Name.TabIndex = 2;
            this.p1Name.TextChanged += new System.EventHandler(this.p1Name_TextChanged);
            // 
            // p5Ico
            // 
            this.p5Ico.Location = new System.Drawing.Point(0, 0);
            this.p5Ico.Name = "p5Ico";
            this.p5Ico.Size = new System.Drawing.Size(100, 50);
            this.p5Ico.TabIndex = 25;
            this.p5Ico.TabStop = false;
            // 
            // p4Type
            // 
            this.p4Type.FormattingEnabled = true;
            this.p4Type.Items.AddRange(new object[] {
            "MainTank",
            "Warrior",
            "Healer",
            "Scout",
            "Mage"});
            this.p4Type.Location = new System.Drawing.Point(262, 349);
            this.p4Type.Name = "p4Type";
            this.p4Type.Size = new System.Drawing.Size(127, 21);
            this.p4Type.TabIndex = 16;
            this.p4Type.Visible = false;
            this.p4Type.SelectedIndexChanged += new System.EventHandler(this.p4Type_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Uighur", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Member 4";
            this.label4.Visible = false;
            // 
            // p4Ico
            // 
            this.p4Ico.Location = new System.Drawing.Point(0, 0);
            this.p4Ico.Name = "p4Ico";
            this.p4Ico.Size = new System.Drawing.Size(100, 50);
            this.p4Ico.TabIndex = 26;
            this.p4Ico.TabStop = false;
            // 
            // p3Type
            // 
            this.p3Type.FormattingEnabled = true;
            this.p3Type.Items.AddRange(new object[] {
            "MainTank",
            "Warrior",
            "Healer",
            "Scout",
            "Mage"});
            this.p3Type.Location = new System.Drawing.Point(262, 308);
            this.p3Type.Name = "p3Type";
            this.p3Type.Size = new System.Drawing.Size(127, 21);
            this.p3Type.TabIndex = 12;
            this.p3Type.Visible = false;
            this.p3Type.SelectedIndexChanged += new System.EventHandler(this.p3Type_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Uighur", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Member 3";
            this.label3.Visible = false;
            // 
            // p3Ico
            // 
            this.p3Ico.Location = new System.Drawing.Point(0, 0);
            this.p3Ico.Name = "p3Ico";
            this.p3Ico.Size = new System.Drawing.Size(100, 50);
            this.p3Ico.TabIndex = 27;
            this.p3Ico.TabStop = false;
            // 
            // p2Type
            // 
            this.p2Type.FormattingEnabled = true;
            this.p2Type.Items.AddRange(new object[] {
            "MainTank",
            "Warrior",
            "Healer",
            "Scout",
            "Mage"});
            this.p2Type.Location = new System.Drawing.Point(262, 268);
            this.p2Type.Name = "p2Type";
            this.p2Type.Size = new System.Drawing.Size(127, 21);
            this.p2Type.TabIndex = 8;
            this.p2Type.Visible = false;
            this.p2Type.SelectedIndexChanged += new System.EventHandler(this.p2Type_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Uighur", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Member 2";
            this.label2.Visible = false;
            // 
            // p2Ico
            // 
            this.p2Ico.Location = new System.Drawing.Point(0, 0);
            this.p2Ico.Name = "p2Ico";
            this.p2Ico.Size = new System.Drawing.Size(100, 50);
            this.p2Ico.TabIndex = 28;
            this.p2Ico.TabStop = false;
            // 
            // p1Type
            // 
            this.p1Type.FormattingEnabled = true;
            this.p1Type.Items.AddRange(new object[] {
            "MainTank",
            "Warrior",
            "Healer",
            "Scout",
            "Mage"});
            this.p1Type.Location = new System.Drawing.Point(262, 229);
            this.p1Type.Name = "p1Type";
            this.p1Type.Size = new System.Drawing.Size(127, 21);
            this.p1Type.TabIndex = 4;
            this.p1Type.Visible = false;
            this.p1Type.SelectedIndexChanged += new System.EventHandler(this.p1Type_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Other Char Name";
            // 
            // p1Ico
            // 
            this.p1Ico.Location = new System.Drawing.Point(0, 0);
            this.p1Ico.Name = "p1Ico";
            this.p1Ico.Size = new System.Drawing.Size(100, 50);
            this.p1Ico.TabIndex = 29;
            this.p1Ico.TabStop = false;
            // 
            // uiCheckGroup
            // 
            this.uiCheckGroup.AutoSize = true;
            this.uiCheckGroup.Location = new System.Drawing.Point(-3, 415);
            this.uiCheckGroup.Name = "uiCheckGroup";
            this.uiCheckGroup.Size = new System.Drawing.Size(85, 17);
            this.uiCheckGroup.TabIndex = 0;
            this.uiCheckGroup.Text = "Group Mode";
            this.uiCheckGroup.UseVisualStyleBackColor = true;
            this.uiCheckGroup.Visible = false;
            this.uiCheckGroup.CheckedChanged += new System.EventHandler(this.uiCheckGroup_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(283, 210);
            this.tabControl1.TabIndex = 0;
            // 
            // pidBox
            // 
            this.pidBox.FormattingEnabled = true;
            this.pidBox.Location = new System.Drawing.Point(167, 6);
            this.pidBox.Name = "pidBox";
            this.pidBox.Size = new System.Drawing.Size(96, 21);
            this.pidBox.TabIndex = 32;
            this.pidBox.Text = "Choose PID";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(283, 210);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Angelbot Healbot Prototype 0.3";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.p5Ico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4Ico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3Ico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2Ico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1Ico)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void LoadPlayers()
        {
            if (this.p1Name.Text != "")
            {
                this.players[0] = new PEntity(this.FindPlayer(this.p1Name.Text), this.p1Type.Text);
                if (this.players[0].PtrEntity > 0)
                {
                    this.p1Ico.Show();
                }
                else
                {
                    this.p1Ico.Hide();
                }
            }
            if (this.p2Name.Text != "")
            {
                this.players[1] = new PEntity(this.FindPlayer(this.p2Name.Text), this.p2Type.Text);
                if (this.players[1].PtrEntity > 0)
                {
                    this.p2Ico.Show();
                }
                else
                {
                    this.p2Ico.Hide();
                }
            }
            if (this.p3Name.Text != "")
            {
                this.players[2] = new PEntity(this.FindPlayer(this.p3Name.Text), this.p3Type.Text);
                if (this.players[2].PtrEntity > 0)
                {
                    this.p3Ico.Show();
                }
                else
                {
                    this.p3Ico.Hide();
                }
            }
            if (this.p4Name.Text != "")
            {
                this.players[3] = new PEntity(this.FindPlayer(this.p4Name.Text), this.p4Type.Text);
                if (this.players[3].PtrEntity > 0)
                {
                    this.p4Ico.Show();
                }
                else
                {
                    this.p4Ico.Hide();
                }
            }
            if (this.p5Name.Text != "")
            {
                this.players[4] = new PEntity(this.FindPlayer(this.p5Name.Text), this.p5Type.Text);
                if (this.players[4].PtrEntity > 0)
                {
                    this.p5Ico.Show();
                }
                else
                {
                    this.p5Ico.Hide();
                }
            }
        }

        private void LoadSettings()
        {
            this.p5Type.SelectedIndex = Settings.Default.p5type;
            this.p4Type.SelectedIndex = Settings.Default.p4type;
            this.p3Type.SelectedIndex = Settings.Default.p3type;
            this.p2Type.SelectedIndex = Settings.Default.p2type;
            this.p1Type.SelectedIndex = Settings.Default.p1type;
            this.p5Name.Text = Settings.Default.player5;
            this.p4Name.Text = Settings.Default.player4;
            this.p3Name.Text = Settings.Default.player3;
            this.p2Name.Text = Settings.Default.player2;
            this.p1Name.Text = Settings.Default.player1;
            this.hFRCheck.Checked = Settings.Default.hFRCheck;
            this.hLRCheck.Checked = Settings.Default.hLRCheck;
            this.hRCCheck.Checked = Settings.Default.hRCCheck;
            this.hHLCheck.Checked = Settings.Default.hHLCheck;
            this.hFRKey.SelectedIndex = Settings.Default.hFRKey;
            this.hLRKey.SelectedIndex = Settings.Default.hLRKey;
            this.hRCKey.SelectedIndex = Settings.Default.hRCKey;
            this.hHLKey.SelectedIndex = Settings.Default.hHLKey;
            this.bBORCheck.Checked = Settings.Default.bBORCheck;
            this.bBOHCheck.Checked = Settings.Default.bBOHCheck;
            this.bBOHKey.SelectedIndex = Settings.Default.bBOHKey;
            this.bBORKey.SelectedIndex = Settings.Default.bBORKey;
            this.hResCheck.Checked = Settings.Default.hResCheck;
            this.hResKey.SelectedIndex = Settings.Default.hResKey;
        }

        public void MageHeal(PEntity p)
        {
            this.PSelect(p.Name.ToString());
            if (((p.Health < 40) && (this.ElapsedTime(this.hFRTime) > 30000.0)) && this.hFRCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hFRKey.Text));
                Thread.Sleep(100);
                this.hFRTime = DateTime.Now;
            }
            else if (((p.Health < 60) && (this.ElapsedTime(this.hLRTime) > 2000.0)) && this.hLRCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hLRKey.Text));
                Thread.Sleep(0x3e8);
                this.hLRTime = DateTime.Now;
            }
            else if (((p.Health < 80) && (this.ElapsedTime(this.hHLTime) > 100.0)) && this.hHLCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hHLKey.Text));
                Thread.Sleep(0x7d0);
                this.hHLTime = DateTime.Now;
            }
            this.following = false;
        }

        private void ManagePlayers()
        {
            string[] strArray = new string[] { this.p1Name.Text, this.p2Name.Text, this.p3Name.Text, this.p4Name.Text, this.p5Name.Text };
            for (int i = 0; i < 2; i++)
            {
                if ((this.players[i] != null) && (this.players[i].Distance2D(this.pc.X, this.pc.Y) > 200.0))
                {
                    foreach (Entity entity in this.eList)
                    {
                        if (entity.Name.ToString() == strArray[i])
                        {
                            this.players[i].PtrEntity = entity.PtrEntity;
                            break;
                        }
                    }
                }
            }
        }

        private void p1Name_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.player1 = this.p1Name.Text;
            Settings.Default.Save();
        }

        private void p1Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.p1type = this.p1Type.SelectedIndex;
            Settings.Default.Save();
        }

        private void p2Name_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.player2 = this.p2Name.Text;
            Settings.Default.Save();
        }

        private void p2Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.p2type = this.p2Type.SelectedIndex;
            Settings.Default.Save();
        }

        private void p3Name_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.player3 = this.p3Name.Text;
            Settings.Default.Save();
        }

        private void p3Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.p3type = this.p3Type.SelectedIndex;
            Settings.Default.Save();
        }

        private void p4Name_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.player4 = this.p4Name.Text;
            Settings.Default.Save();
        }

        private void p4Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.p4type = this.p4Type.SelectedIndex;
            Settings.Default.Save();
        }

        private void p5Name_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.player5 = this.p5Name.Text;
            Settings.Default.Save();
        }

        private void p5Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.p5type = this.p5Type.SelectedIndex;
            Settings.Default.Save();
        }

        private void PlayerUpdate()
        {
            this.pc.Update();
            this.tar.Update();
            foreach (PEntity entity in this.players)
            {
                if (entity != null)
                {
                    entity.Update();
                }
            }
        }

        private void PSelect(string name)
        {
            if (this.tar.Name.ToString() != name)
            {

                if (checkBox1.Checked == true) this.keys.SendKey(0x71);
                else this.keys.SendLine("/select " + name);
            }
        }

        private void PSelectMain()
        {
            if (this.tar.Name.ToString() != this.players[0].Name.ToString())
            {
                if (checkBox1.Checked == true) this.keys.SendKey(0x71);
                else this.keys.SendLine("/select " + this.players[0].Name.ToString());
                this.keys.SendKey(0x38);
                this.following = true;
            }
            else
            {
                this.keys.SendKey(0x38);
                this.following = true;
            }
        }

        private bool Resurect()
        {
            if (this.hResCheck.Checked)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (((this.players[i] != null) && (this.players[i].Health <= 0)) && (this.players[i].Distance2D(this.pc.X, this.pc.Y) < 25.0))
                    {
                        this.following = false;
                        this.lblStatus.Text = "Status: Resurrecting";
                        this.keys.SendKey(this.FindKey(this.hResKey.Text));
                        Thread.Sleep(0x1770);
                        return true;
                    }
                }
            }
            return false;
        }

        public void SelfHeal(int myhealth)
        {
            this.keys.SendKey(0x70);
            if (((myhealth < 40) && (this.ElapsedTime(this.hFRTime) > 30000.0)) && this.hFRCheck.Checked)
            {
                this.lblStatus.Text = "Status: SelfHeal FlashofRecovery";
                this.keys.SendKey(this.FindKey(this.hFRKey.Text));
                Thread.Sleep(100);
                this.hFRTime = DateTime.Now;
            }
            else if (((myhealth < 50) && (this.ElapsedTime(this.hLRTime) > 2000.0)) && this.hLRCheck.Checked)
            {
                this.lblStatus.Text = "Status: SelfHeal LightofRecovery";
                this.keys.SendKey(this.FindKey(this.hLRKey.Text));
                Thread.Sleep(0x3e8);
                this.hLRTime = DateTime.Now;
            }
            else if (((myhealth < 0x4b) && (this.ElapsedTime(this.hHLTime) > 100.0)) && this.hHLCheck.Checked)
            {
                this.lblStatus.Text = "Status: SelfHeal HealingLight";
                this.keys.SendKey(this.FindKey(this.hHLKey.Text));
                Thread.Sleep(0x7d0);
                this.hHLTime = DateTime.Now;
            }
            this.following = false;
        }

        public void TankHeal(PEntity p)
        {
            this.PSelect(p.Name.ToString());
            if (((p.Health < 35) && (this.ElapsedTime(this.hFRTime) > 30000.0)) && this.hFRCheck.Checked)
            {
                this.lblStatus.Text = "Status: Heal FlashofRecovery";
                this.keys.SendKey(this.FindKey(this.hFRKey.Text));
                Thread.Sleep(100);
                this.hFRTime = DateTime.Now;
            }
            else if (((p.Health < 55) && (this.ElapsedTime(this.hRCTime) > 6000.0)) && this.hRCCheck.Checked)
            {
                this.lblStatus.Text = "Status: Heal RadiantCure";
                this.keys.SendKey(this.FindKey(this.hRCKey.Text));
                Thread.Sleep(0xbb8);
                this.hRCTime = DateTime.Now;
            }
            else if ((((p.Health < 75) && (this.ElapsedTime(this.hHLTime) > 100.0)) && ((this.ElapsedTime(this.hRCTime) < 6000.0) || !this.hRCCheck.Checked)) && this.hHLCheck.Checked)
            {
                this.lblStatus.Text = "Status: Heal HealingLight";
                this.keys.SendKey(this.FindKey(this.hHLKey.Text));
                Thread.Sleep(0x7d0);
                this.hHLTime = DateTime.Now;
            }
            else if (((p.Health < 90) && (this.ElapsedTime(this.hLRTime) > 2000.0)) && this.hLRCheck.Checked)
            {
                this.keys.SendKey(this.FindKey(this.hLRKey.Text));
                Thread.Sleep(0x3e8);
                this.hLRTime = DateTime.Now;
            }
            this.following = false;
        }

        private void uiCheckGroup_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Group Mode Is Currently Not Functioning");
            this.uiCheckGroup.Checked = false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

