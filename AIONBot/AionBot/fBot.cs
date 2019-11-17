using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Collections;

namespace AionBot
{
    public partial class fBot : Form
    {
        public fBot()
        {
            InitializeComponent();
            MessageBox.Show("You must load a profile before you do anything else");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
        bool flag = false;
        int num = 0x780;
        int num2 = 0x438;
        int num3 = aionUtil.getBaseAddr();
        string[] actionArray = this.pullSeq.Text.Split(new char[] { ',' });
        string[] strArray2 = this.fightSeq1.Text.Split(new char[] { ',' });
        string[] strArray3 = this.fightSeq2.Text.Split(new char[] { ',' });
        string[] strArray4 = this.healSeq.Text.Split(new char[] { ',' });
        string[] strArray5 = this.afterDeathSeq.Text.Split(new char[] { ',' });
        string[] strArray6 = this.lootKey.Text.Split(new char[] { ',' });
        int delay = int.Parse(this.pullDelay.Text);
        int num5 = int.Parse(this.fight1Delay.Text);
        int millisecondsTimeout = int.Parse(this.fight2Delay.Text);
        int num7 = int.Parse(this.healDelay.Text);
        int num8 = int.Parse(this.afterDeathDelay.Text);
        int num9 = int.Parse(this.minHealth.Text);
        int num10 = int.Parse(this.fight_file_name.Text.Split(new char[] { 'x' })[0]);
        int num11 = int.Parse(this.fight_file_name.Text.Split(new char[] { 'x' })[1]);
        int x = (num10 * 0x453) / num;
        int y = (num11 * 0x264) / num2;
        keyEvent event2 = new keyEvent();
        mouseEvent event3 = new mouseEvent();
        Thread.Sleep(0x1388);
        while (this.fightCount == 0)
        {
            if (((aionUtil.hasTarget(num3) == 0) || (aionUtil.getTargetStatus(num3) == 8)) || flag)
            {
                event2.tab();
                Thread.Sleep(0x3e8);
                flag = false;
            }
            if (aionUtil.getTargetStatus(num3) != 9)
            {
                event2.pressLeft(400);
                Thread.Sleep(0x7d0);
                continue;
            }
            aionUtil.actionSeq(actionArray, delay);
            StartTimerCallBack method = new StartTimerCallBack(this.StartTimer);
            base.Invoke(method);
            int num14 = aionUtil.getPlayerHealth(num3);
            int num15 = aionUtil.getTargetHealth(num3);
            while ((aionUtil.getTargetStatus(num3) == 9) && (aionUtil.getPlayerHealth(num3) > 0))
            {
                aionUtil.actionSeq(strArray2, num5);
                if (aionUtil.getTargetStatus(num3) != 9)
                {
                    break;
                }
                Thread.Sleep(millisecondsTimeout);
                aionUtil.actionSeq(strArray3, num5);
                if (aionUtil.getTargetStatus(num3) != 9)
                {
                    break;
                }
                Thread.Sleep(millisecondsTimeout);
                if (((this.timer > 400) && (aionUtil.getPlayerHealth(num3) >= num14)) && (aionUtil.getTargetHealth(num3) >= num15))
                {
                    event2.pressRight(0x640);
                    Thread.Sleep(0x3e8);
                    event2.tab();
                    flag = true;
                    break;
                }
                StopTimerCallBack back2 = new StopTimerCallBack(this.StopTimer);
                base.Invoke(back2);
            }
            StopTimerCallBack back3 = new StopTimerCallBack(this.StopTimer);
            base.Invoke(back3);
            Thread.Sleep(500);
            aionUtil.actionSeq(strArray6, 0x5dc);
            Thread.Sleep(0x3e8);
            if (aionUtil.getPlayerHealth(num3) == 0)
            {
                Thread.Sleep(0x1388);
                event3.leftClick(x, y);
                Thread.Sleep(0x1388);
                event2.pressKey(this.restKey.Text, 100);
                Thread.Sleep(0x3a980);
                event2.pressKey(this.restKey.Text, 100);
                Thread.Sleep(0xbb8);
                aionUtil.actionSeq(strArray5, num8);
                Thread.Sleep(0x3e8);
                aionUtil.followPath(num3, this.death_file_name.Text);
            }
            else if ((aionUtil.getPlayerHealth(num3) <= num9) && (aionUtil.hasTarget(num3) == 0))
            {
                Thread.Sleep(0x3e8);
                aionUtil.actionSeq(strArray4, num7);
            }
            else
            {
                event2.pressLeft(300);
            }
        }
        this.fightCount = 0;
    }

    private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
    {
        int num = 5;
        TextWriter writer = new StreamWriter(this.record_file_name.Text, false);
        int num2 = aionUtil.getBaseAddr();
        string str = "Recording in: 5 seconds";
        SetTextCallback method = new SetTextCallback(this.SetText);
        base.Invoke(method, new object[] { str });
        while (this.count == 0)
        {
            for (int i = num; i > 0; i--)
            {
                str = "Recording in: " + i + " seconds";
                SetTextCallback callback2 = new SetTextCallback(this.SetText);
                base.Invoke(callback2, new object[] { str });
                Thread.Sleep(0x3e8);
            }
            str = "Recording location now";
            SetTextCallback callback3 = new SetTextCallback(this.SetText);
            base.Invoke(callback3, new object[] { str });
            Thread.Sleep(0x3e8);
            aionUtil.setPlayerInfo(num2);
            writer.WriteLine("{0},{1},{2}", aionUtil.player.X.ToString(), aionUtil.player.Y.ToString(), aionUtil.player.Z.ToString());
        }
        writer.Close();
        this.count = 0;
        str = "Not Recording";
        SetTextCallback callback4 = new SetTextCallback(this.SetText);
        base.Invoke(callback4, new object[] { str });
    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.backgroundWorker2.RunWorkerAsync();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        this.backgroundWorker2.CancelAsync();
        this.count = 1;
        this.label14.Text = "Not Recording";
    }

    private void button3_Click(object sender, EventArgs e)
    {
        this.backgroundWorker1.CancelAsync();
        this.fightCount = 1;
    }

    private void button4_Click(object sender, EventArgs e)
    {
        this.backgroundWorker1.RunWorkerAsync();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        Thread.Sleep(0x1388);
        aionUtil.followPath(aionUtil.getBaseAddr(), this.record_file_name.Text);
    }

    private void button6_Click(object sender, EventArgs e)
    {
        this.textBox1.Text = aionUtil.hasTarget(aionUtil.getBaseAddr()).ToString();
    }

    private void label14_Click(object sender, EventArgs e)
    {
    }

    private void label7_Click(object sender, EventArgs e)
    {
    }

    private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string str;
        StreamReader reader = new StreamReader("./profile.txt");
        ArrayList list = new ArrayList();
        while ((str = reader.ReadLine()) != null)
        {
            list.Add(str);
        }
        this.pullSeq.Text = list[0].ToString();
        this.pullDelay.Text = list[1].ToString();
        this.fightSeq1.Text = list[2].ToString();
        this.fight1Delay.Text = list[3].ToString();
        this.fightSeq2.Text = list[4].ToString();
        this.fight2Delay.Text = list[5].ToString();
        this.healSeq.Text = list[6].ToString();
        this.healDelay.Text = list[7].ToString();
        this.lootKey.Text = list[8].ToString();
        this.minHealth.Text = list[9].ToString();
        this.restKey.Text = list[10].ToString();
        this.fight_file_name.Text = list[11].ToString();
        this.death_file_name.Text = list[12].ToString();
        this.afterDeathSeq.Text = list[13].ToString();
        this.afterDeathDelay.Text = list[14].ToString();
        reader.Close();
    }

    private void saveProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        TextWriter writer = new StreamWriter("profile.txt", false);
        writer.WriteLine(this.pullSeq.Text);
        writer.WriteLine(this.pullDelay.Text);
        writer.WriteLine(this.fightSeq1.Text);
        writer.WriteLine(this.fight1Delay.Text);
        writer.WriteLine(this.fightSeq2.Text);
        writer.WriteLine(this.fight2Delay.Text);
        writer.WriteLine(this.healSeq.Text);
        writer.WriteLine(this.healDelay.Text);
        writer.WriteLine(this.lootKey.Text);
        writer.WriteLine(this.minHealth.Text);
        writer.WriteLine(this.restKey.Text);
        writer.WriteLine(this.fight_file_name.Text);
        writer.WriteLine(this.death_file_name.Text);
        writer.WriteLine(this.afterDeathSeq.Text);
        writer.WriteLine(this.afterDeathDelay.Text);
        writer.Close();
    }

    private void SetText(string text)
    {
        this.label14.Text = text;
    }

    private void StartTimer()
    {
        this.timer1.Start();
        this.timer1.Tick += new EventHandler(this.timer1_Tick);
    }

    private void StopTimer()
    {
        this.timer1.Stop();
        this.timer = 0;
    }

    private void tabPage1_Click(object sender, EventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void textBox9_TextChanged(object sender, EventArgs e)
    {
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        this.timer++;
        this.label14.Text = this.timer.ToString();
    }

    private void updateStatus(string msg)
    {
        this.label4.Text = msg;
    }

    // Nested Types
    private delegate void SetTextCallback(string text);

    private delegate void StartTimerCallBack();

    private delegate void StopTimerCallBack();

    }
}
