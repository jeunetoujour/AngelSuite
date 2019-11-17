namespace ABHeal
{
    using ABHeal.Properties;
    using AionMemory;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

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
        private EntityList eList;
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
        private Player player;
        private PEntity[] players;
        private System.Diagnostics.Process proc;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Target target;
        private CheckBox uiCheckGroup;

        public Form1()
        {
            this.InitializeComponent();
            this.LoadSettings();
            this.castStart = this.hRCTime = this.hHLTime = this.hFRTime = this.hLRTime = this.flightWarn = this.chaseTime = this.manageTime = DateTime.Now;
            this.clock = new System.Windows.Forms.Timer();
            this.clock.Interval = 100;
            this.clock.Tick += new EventHandler(this.clock_Tick);
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
            if (AionMemory.Process.Open())
            {
                this.lblStatus.Text = "Status: Started!";
                this.proc = System.Diagnostics.Process.GetProcessesByName("aion.bin")[0];
                this.keys = new SKeys(this.proc);
                this.eList = new EntityList();
                this.player = new Player();
                this.player.Updatenamelvl();
                this.target = new Target();
                this.players = new PEntity[5];
                this.eList.Update();
                this.LoadPlayers();
                if ((this.players[0] == null) || (this.players[0].PtrEntity == 0))
                {
                    MessageBox.Show("Main not found!");
                    AionMemory.Process.Close();
                    this.lblStatus.Text = "Status: Stopped...";
                }
                else
                {
                    this.PlayerUpdate();
                    this.player.Updatenamelvl();
                    this.player.UpdateRot();
                    this.player.Updateafterkill();
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
            AionMemory.Process.Close();
            this.following = false;
            this.lost = false;
            this.lblStatus.Text = "Status: Stopped.";
        }

        public void Buff()
        {
            for (int i = 0; i < 5; i++)
            {
                if (((this.players[i] != null) && (this.players[i].Distance2D(this.player.X, this.player.Y) < 15.0)) && (((this.ElapsedTime(this.players[i].bBOHTime) > 3600000.0) && this.bBOHCheck.Checked) || ((this.ElapsedTime(this.players[i].bBORTime) > 3600000.0) && this.bBORCheck.Checked)))
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
            TimeSpan span = (TimeSpan) (DateTime.Now - time);
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
                    return (uint) entity.PtrEntity;
                }
            }
            return 0;
        }

        private void Follow()
        {
            PEntity entity = this.players[0];
            if ((entity != null) && (entity.PtrEntity != 0))
            {
                double num = (((double) this.player.FlightTime) / ((double) this.player.MaxFlightTime)) * 100.0;
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
                double num2 = this.player.Distance2D(entity.X, entity.Y);
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
            double num = (((double) this.player.Health) / ((double) this.player.MaxHealth)) * 100.0;
            bool flag = false;
            for (int i = 0; i < 5; i++)
            {
                if ((((this.players[i] != null) && (this.players[i].Health > 0)) && ((this.players[i].Health < 90) && (this.players[i].PtrEntity != 0))) && (this.player.Distance3D(this.players[i].X, this.players[i].Y, this.players[i].Z) < 35.0))
                {
                    flag = true;
                    break;
                }
            }
            int[] numArray = new int[5];
            if (!flag || (num <= 80.0))
            {
                if (num < 85.0)
                {
                    this.SelfHeal((int) num);
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
                            if (entity.PtrEntity > 0)
                            {
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
                Label_0299:;
                }
                int num4 = -1;
                int num5 = 0;
                for (int j = 0; j < 5; j++)
                {
                    if ((numArray[j] > 0) && (numArray[j] > num5))
                    {
                        num5 = numArray[j];
                        num4 = j;
                    }
                }
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
            this.tabPage2 = new TabPage();
            this.label10 = new Label();
            this.label6 = new Label();
            this.hLRKey = new ComboBox();
            this.hRCCheck = new CheckBox();
            this.label9 = new Label();
            this.hRCKey = new ComboBox();
            this.hLRCheck = new CheckBox();
            this.hFRCheck = new CheckBox();
            this.hHLKey = new ComboBox();
            this.label7 = new Label();
            this.label8 = new Label();
            this.hFRKey = new ComboBox();
            this.hHLCheck = new CheckBox();
            this.label12 = new Label();
            this.bBORKey = new ComboBox();
            this.bBOHKey = new ComboBox();
            this.bBORCheck = new CheckBox();
            this.bBOHCheck = new CheckBox();
            this.hResKey = new ComboBox();
            this.hResCheck = new CheckBox();
            this.tabPage1 = new TabPage();
            this.lblStatus = new Label();
            this.bStop = new Button();
            this.bStart = new Button();
            this.p5Type = new ComboBox();
            this.label5 = new Label();
            this.p5Name = new TextBox();
            this.p4Name = new TextBox();
            this.p3Name = new TextBox();
            this.p2Name = new TextBox();
            this.p1Name = new TextBox();
            this.p5Ico = new PictureBox();
            this.p4Type = new ComboBox();
            this.label4 = new Label();
            this.p4Ico = new PictureBox();
            this.p3Type = new ComboBox();
            this.label3 = new Label();
            this.p3Ico = new PictureBox();
            this.p2Type = new ComboBox();
            this.label2 = new Label();
            this.p2Ico = new PictureBox();
            this.p1Type = new ComboBox();
            this.label1 = new Label();
            this.p1Ico = new PictureBox();
            this.uiCheckGroup = new CheckBox();
            this.tabControl1 = new TabControl();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((ISupportInitialize) this.p5Ico).BeginInit();
            ((ISupportInitialize) this.p4Ico).BeginInit();
            ((ISupportInitialize) this.p3Ico).BeginInit();
            ((ISupportInitialize) this.p2Ico).BeginInit();
            ((ISupportInitialize) this.p1Ico).BeginInit();
            this.tabControl1.SuspendLayout();
            base.SuspendLayout();
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
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0x113, 0xb8);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Heals";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x81, 3);
            this.label10.Name = "label10";
            this.label10.Size = new Size(30, 13);
            this.label10.TabIndex = 0x22;
            this.label10.Text = "Keys";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x1d, 20);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x45, 13);
            this.label6.TabIndex = 0x17;
            this.label6.Text = "Radiant Cure";
            this.hLRKey.FormattingEnabled = true;
            this.hLRKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.hLRKey.Location = new Point(0x84, 0x3e);
            this.hLRKey.Name = "hLRKey";
            this.hLRKey.Size = new Size(0x23, 0x15);
            this.hLRKey.TabIndex = 0x21;
            this.hRCCheck.AutoSize = true;
            this.hRCCheck.Location = new Point(12, 0x15);
            this.hRCCheck.Name = "hRCCheck";
            this.hRCCheck.Size = new Size(15, 14);
            this.hRCCheck.TabIndex = 0x16;
            this.hRCCheck.UseVisualStyleBackColor = true;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x1d, 0x41);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x5b, 13);
            this.label9.TabIndex = 0x20;
            this.label9.Text = "Light of Recovery";
            this.hRCKey.FormattingEnabled = true;
            this.hRCKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.hRCKey.Location = new Point(0x84, 0x11);
            this.hRCKey.Name = "hRCKey";
            this.hRCKey.Size = new Size(0x23, 0x15);
            this.hRCKey.TabIndex = 0x18;
            this.hLRCheck.AutoSize = true;
            this.hLRCheck.Location = new Point(12, 0x42);
            this.hLRCheck.Name = "hLRCheck";
            this.hLRCheck.Size = new Size(15, 14);
            this.hLRCheck.TabIndex = 0x1f;
            this.hLRCheck.UseVisualStyleBackColor = true;
            this.hFRCheck.AutoSize = true;
            this.hFRCheck.Location = new Point(12, 0x59);
            this.hFRCheck.Name = "hFRCheck";
            this.hFRCheck.Size = new Size(15, 14);
            this.hFRCheck.TabIndex = 0x19;
            this.hFRCheck.UseVisualStyleBackColor = true;
            this.hHLKey.FormattingEnabled = true;
            this.hHLKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.hHLKey.Location = new Point(0x84, 40);
            this.hHLKey.Name = "hHLKey";
            this.hHLKey.Size = new Size(0x23, 0x15);
            this.hHLKey.TabIndex = 30;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x1d, 0x59);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x5d, 13);
            this.label7.TabIndex = 0x1a;
            this.label7.Text = "Flash of Recovery";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x1d, 0x2b);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x45, 13);
            this.label8.TabIndex = 0x1d;
            this.label8.Text = "Healing Light";
            this.hFRKey.FormattingEnabled = true;
            this.hFRKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.hFRKey.Location = new Point(0x84, 0x56);
            this.hFRKey.Name = "hFRKey";
            this.hFRKey.Size = new Size(0x23, 0x15);
            this.hFRKey.TabIndex = 0x1b;
            this.hHLCheck.AutoSize = true;
            this.hHLCheck.Location = new Point(12, 0x2c);
            this.hHLCheck.Name = "hHLCheck";
            this.hHLCheck.Size = new Size(15, 14);
            this.hHLCheck.TabIndex = 0x1c;
            this.hHLCheck.UseVisualStyleBackColor = true;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x81, 0xbb);
            this.label12.Name = "label12";
            this.label12.Size = new Size(30, 13);
            this.label12.TabIndex = 0x15;
            this.label12.Text = "Keys";
            this.bBORKey.FormattingEnabled = true;
            this.bBORKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.bBORKey.Location = new Point(0x84, 0x9a);
            this.bBORKey.Name = "bBORKey";
            this.bBORKey.Size = new Size(0x23, 0x15);
            this.bBORKey.TabIndex = 0x13;
            this.bBOHKey.FormattingEnabled = true;
            this.bBOHKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.bBOHKey.Location = new Point(0x84, 0x83);
            this.bBOHKey.Name = "bBOHKey";
            this.bBOHKey.Size = new Size(0x23, 0x15);
            this.bBOHKey.TabIndex = 0x12;
            this.bBORCheck.AutoSize = true;
            this.bBORCheck.Location = new Point(12, 0x9e);
            this.bBORCheck.Name = "bBORCheck";
            this.bBORCheck.Size = new Size(0x6a, 0x11);
            this.bBORCheck.TabIndex = 0x11;
            this.bBORCheck.Text = "Blessing of Rock";
            this.bBORCheck.UseVisualStyleBackColor = true;
            this.bBOHCheck.AutoSize = true;
            this.bBOHCheck.Location = new Point(12, 0x87);
            this.bBOHCheck.Name = "bBOHCheck";
            this.bBOHCheck.Size = new Size(0x6f, 0x11);
            this.bBOHCheck.TabIndex = 0x10;
            this.bBOHCheck.Text = "Blessing of Health";
            this.bBOHCheck.UseVisualStyleBackColor = true;
            this.hResKey.FormattingEnabled = true;
            this.hResKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" });
            this.hResKey.Location = new Point(0x84, 110);
            this.hResKey.Name = "hResKey";
            this.hResKey.Size = new Size(0x23, 0x15);
            this.hResKey.TabIndex = 15;
            this.hResKey.SelectedIndexChanged += new EventHandler(this.hResKey_SelectedIndexChanged);
            this.hResCheck.AutoSize = true;
            this.hResCheck.Location = new Point(12, 0x70);
            this.hResCheck.Name = "hResCheck";
            this.hResCheck.Size = new Size(0x48, 0x11);
            this.hResCheck.TabIndex = 14;
            this.hResCheck.Text = "Resurrect";
            this.hResCheck.UseVisualStyleBackColor = true;
            this.hResCheck.CheckedChanged += new EventHandler(this.hResCheck_CheckedChanged);
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
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0x113, 0xb8);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblStatus.Location = new Point(3, 0x67);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(70, 0x18);
            this.lblStatus.TabIndex = 0x18;
            this.lblStatus.Text = "Status: ";
            this.bStop.Location = new Point(0x63, 6);
            this.bStop.Name = "bStop";
            this.bStop.Size = new Size(0x56, 0x21);
            this.bStop.TabIndex = 0x16;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new EventHandler(this.bStop_Click);
            this.bStart.Location = new Point(7, 6);
            this.bStart.Name = "bStart";
            this.bStart.Size = new Size(0x56, 0x21);
            this.bStart.TabIndex = 0x15;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new EventHandler(this.bStart_Click);
            this.p5Type.FormattingEnabled = true;
            this.p5Type.Items.AddRange(new object[] { "MainTank", "Warrior", "Healer", "Scout", "Mage" });
            this.p5Type.Location = new Point(0x106, 0x184);
            this.p5Type.Name = "p5Type";
            this.p5Type.Size = new Size(0x7f, 0x15);
            this.p5Type.TabIndex = 20;
            this.p5Type.Visible = false;
            this.p5Type.SelectedIndexChanged += new EventHandler(this.p5Type_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Uighur", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0x11, 0x173);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x34, 20);
            this.label5.TabIndex = 0x13;
            this.label5.Text = "Member 5";
            this.label5.Visible = false;
            this.p5Name.Location = new Point(20, 0x185);
            this.p5Name.Name = "p5Name";
            this.p5Name.Size = new Size(0xea, 20);
            this.p5Name.TabIndex = 0x12;
            this.p5Name.Visible = false;
            this.p5Name.TextChanged += new EventHandler(this.p5Name_TextChanged);
            this.p4Name.Location = new Point(20, 350);
            this.p4Name.Name = "p4Name";
            this.p4Name.Size = new Size(0xea, 20);
            this.p4Name.TabIndex = 14;
            this.p4Name.Visible = false;
            this.p4Name.TextChanged += new EventHandler(this.p4Name_TextChanged);
            this.p3Name.Location = new Point(20, 0x135);
            this.p3Name.Name = "p3Name";
            this.p3Name.Size = new Size(0xea, 20);
            this.p3Name.TabIndex = 10;
            this.p3Name.Visible = false;
            this.p3Name.TextChanged += new EventHandler(this.p3Name_TextChanged);
            this.p2Name.Location = new Point(20, 0x10d);
            this.p2Name.Name = "p2Name";
            this.p2Name.Size = new Size(0xea, 20);
            this.p2Name.TabIndex = 6;
            this.p2Name.Visible = false;
            this.p2Name.TextChanged += new EventHandler(this.p2Name_TextChanged);
            this.p1Name.Location = new Point(8, 0x47);
            this.p1Name.Name = "p1Name";
            this.p1Name.Size = new Size(0xea, 20);
            this.p1Name.TabIndex = 2;
            this.p1Name.TextChanged += new EventHandler(this.p1Name_TextChanged);
            this.p5Ico.Location = new Point(0, 0);
            this.p5Ico.Name = "p5Ico";
            this.p5Ico.Size = new Size(100, 50);
            this.p5Ico.TabIndex = 0x19;
            this.p5Ico.TabStop = false;
            this.p4Type.FormattingEnabled = true;
            this.p4Type.Items.AddRange(new object[] { "MainTank", "Warrior", "Healer", "Scout", "Mage" });
            this.p4Type.Location = new Point(0x106, 0x15d);
            this.p4Type.Name = "p4Type";
            this.p4Type.Size = new Size(0x7f, 0x15);
            this.p4Type.TabIndex = 0x10;
            this.p4Type.Visible = false;
            this.p4Type.SelectedIndexChanged += new EventHandler(this.p4Type_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Uighur", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x11, 0x14c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Member 4";
            this.label4.Visible = false;
            this.p4Ico.Location = new Point(0, 0);
            this.p4Ico.Name = "p4Ico";
            this.p4Ico.Size = new Size(100, 50);
            this.p4Ico.TabIndex = 0x1a;
            this.p4Ico.TabStop = false;
            this.p3Type.FormattingEnabled = true;
            this.p3Type.Items.AddRange(new object[] { "MainTank", "Warrior", "Healer", "Scout", "Mage" });
            this.p3Type.Location = new Point(0x106, 0x134);
            this.p3Type.Name = "p3Type";
            this.p3Type.Size = new Size(0x7f, 0x15);
            this.p3Type.TabIndex = 12;
            this.p3Type.Visible = false;
            this.p3Type.SelectedIndexChanged += new EventHandler(this.p3Type_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Uighur", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x11, 0x123);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x34, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Member 3";
            this.label3.Visible = false;
            this.p3Ico.Location = new Point(0, 0);
            this.p3Ico.Name = "p3Ico";
            this.p3Ico.Size = new Size(100, 50);
            this.p3Ico.TabIndex = 0x1b;
            this.p3Ico.TabStop = false;
            this.p2Type.FormattingEnabled = true;
            this.p2Type.Items.AddRange(new object[] { "MainTank", "Warrior", "Healer", "Scout", "Mage" });
            this.p2Type.Location = new Point(0x106, 0x10c);
            this.p2Type.Name = "p2Type";
            this.p2Type.Size = new Size(0x7f, 0x15);
            this.p2Type.TabIndex = 8;
            this.p2Type.Visible = false;
            this.p2Type.SelectedIndexChanged += new EventHandler(this.p2Type_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Uighur", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x11, 0xfb);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Member 2";
            this.label2.Visible = false;
            this.p2Ico.Location = new Point(0, 0);
            this.p2Ico.Name = "p2Ico";
            this.p2Ico.Size = new Size(100, 50);
            this.p2Ico.TabIndex = 0x1c;
            this.p2Ico.TabStop = false;
            this.p1Type.FormattingEnabled = true;
            this.p1Type.Items.AddRange(new object[] { "MainTank", "Warrior", "Healer", "Scout", "Mage" });
            this.p1Type.Location = new Point(0x106, 0xe5);
            this.p1Type.Name = "p1Type";
            this.p1Type.Size = new Size(0x7f, 0x15);
            this.p1Type.TabIndex = 4;
            this.p1Type.Visible = false;
            this.p1Type.SelectedIndexChanged += new EventHandler(this.p1Type_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Times New Roman", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(5, 0x30);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x69, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Other Char Name";
            this.p1Ico.Location = new Point(0, 0);
            this.p1Ico.Name = "p1Ico";
            this.p1Ico.Size = new Size(100, 50);
            this.p1Ico.TabIndex = 0x1d;
            this.p1Ico.TabStop = false;
            this.uiCheckGroup.AutoSize = true;
            this.uiCheckGroup.Location = new Point(-3, 0x19f);
            this.uiCheckGroup.Name = "uiCheckGroup";
            this.uiCheckGroup.Size = new Size(0x55, 0x11);
            this.uiCheckGroup.TabIndex = 0;
            this.uiCheckGroup.Text = "Group Mode";
            this.uiCheckGroup.UseVisualStyleBackColor = true;
            this.uiCheckGroup.Visible = false;
            this.uiCheckGroup.CheckedChanged += new EventHandler(this.uiCheckGroup_CheckedChanged);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x11b, 210);
            this.tabControl1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11b, 210);
            base.Controls.Add(this.tabControl1);
            base.Name = "Form1";
            this.Text = "Angelbot Healbot Prototype 0.3";
            base.TopMost = true;
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((ISupportInitialize) this.p5Ico).EndInit();
            ((ISupportInitialize) this.p4Ico).EndInit();
            ((ISupportInitialize) this.p3Ico).EndInit();
            ((ISupportInitialize) this.p2Ico).EndInit();
            ((ISupportInitialize) this.p1Ico).EndInit();
            this.tabControl1.ResumeLayout(false);
            base.ResumeLayout(false);
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
            for (int i = 0; i < 5; i++)
            {
                if ((this.players[i] != null) && (this.players[i].Distance2D(this.player.X, this.player.Y) > 200.0))
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
            this.player.Update();
            this.target.Update();
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
            if (this.target.Name.ToString() != name)
            {
                this.keys.SendKey(0x71);
            }
        }

        private void PSelectMain()
        {
            if (this.target.Name.ToString() != this.players[0].Name.ToString())
            {
                this.keys.SendKey(0x71);
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
                for (int i = 0; i < 5; i++)
                {
                    if (((this.players[i] != null) && (this.players[i].Health <= 0)) && (this.players[i].Distance2D(this.player.X, this.player.Y) < 25.0))
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
            if (((p.Health < 50) && (this.ElapsedTime(this.hFRTime) > 30000.0)) && this.hFRCheck.Checked)
            {
                this.lblStatus.Text = "Status: Heal FlashofRecovery";
                this.keys.SendKey(this.FindKey(this.hFRKey.Text));
                Thread.Sleep(100);
                this.hFRTime = DateTime.Now;
            }
            else if (((p.Health < 0x4f) && (this.ElapsedTime(this.hRCTime) > 6000.0)) && this.hRCCheck.Checked)
            {
                this.lblStatus.Text = "Status: Heal RadiantCure";
                this.keys.SendKey(this.FindKey(this.hRCKey.Text));
                Thread.Sleep(0xbb8);
                this.hRCTime = DateTime.Now;
            }
            else if ((((p.Health < 0x55) && (this.ElapsedTime(this.hHLTime) > 100.0)) && ((this.ElapsedTime(this.hRCTime) < 6000.0) || !this.hRCCheck.Checked)) && this.hHLCheck.Checked)
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
    }
}

