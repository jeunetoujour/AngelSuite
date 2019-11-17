using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AngelRead;
using System.Runtime.InteropServices;

namespace AngelSuite
{
    public partial class AngelDPS : Form
    {
        #region Form Dragging API Support
        //The SendMessage function sends a message to a window or windows.
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        //ReleaseCapture releases a mouse capture
        [DllImportAttribute("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern bool ReleaseCapture();
        #endregion

        public Offsets aionOffsets;
        public Target tar = new Target();
        private bool isDragging = false;
        private int clickOffsetX, clickOffsetY;
        private int dpscounter = 0;
        private int lasttarHP = 0;
        private float dps = 0;
        int heartbeat = 0;

        public AngelDPS()
        {
            InitializeComponent();
            aionOffsets = new Offsets();
            aionOffsets.Update();
            //pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;
            //tar.TARGETPTR_OFFSET = aionOffsets.targetInfoAddress;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Process.Open();

            }
            catch (Exception g)
            {
                MessageBox.Show("Aion process not found! Exiting " + g);//Doesnt work!

                Environment.Exit(1);
            }
            tar.Update();
            tmrheart.Enabled = true;

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tar.Update();
            label1.Text = "(" + tar.Level + ")" + tar.HealthHP + "/" + tar.HealthHPMax + " " + tar.Health + "%";
            if (tar.Type == eType.AttackableNPC && tar.HealthHP > 0)
            {
                timer2.Enabled = true;
            }
            else
            {
               // timer2.Enabled = false;
            }
        }
        
        private void label1_MouseDown(System.Object sender,
                    System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = true;
            clickOffsetX = e.X;
            clickOffsetY = e.Y;
        }

        private void label1_MouseUp(System.Object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = false;
        }

        // Move the control (during dragging).
        private void label1_MouseMove(System.Object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if (isDragging == true)
            {
                // The control coordinates are converted into form coordinates
                // by adding the label position offset.
                // The offset where the user clicked in the control is also
                // accounted for. Otherwise, it looks like the top-left corner
                // of the label is attached to the mouse.
                //lblDragger.Left = e.X + lblDragger.Left - clickOffsetX;
                this.Left = this.Left + e.X;
                this.Top = this.Top + e.Y;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (tar.HealthHP == tar.HealthHPMax || tar.Name == "" || tar.Type != eType.AttackableNPC || tar.HealthHP == 0)
            {
                dpscounter = 0;
                dps = 0;
            }
            else
            {
                if (dpscounter == 0)
                {
                    dpscounter++;
                    lasttarHP = tar.HealthHP;
                }
                else
                {
                    dpscounter++;
                    int tempdps = lasttarHP - tar.HealthHP;
                    dps = (dps + tempdps);
                    lasttarHP = tar.HealthHP;
                    label2.Text = "DPS: " + String.Format("{0:0.00}",(dps / dpscounter));

                    int temptime = 0;
                    temptime = (int)(tar.HealthHP / (dps / dpscounter)); 
                    label3.Text = "TimeL: " + temptime + "sec";
                    label4.Text = "Time: " + dpscounter + "sec";
                }
            }
        }

        private void tmrheart_Tick(object sender, EventArgs e)
        {
            if (heartbeat >= 30)
            {
                Login f11 = (Login)Application.OpenForms["Login"];
                f11.theversion = "2";
                f11.program = "DPS";
                f11.heartbeat();
                heartbeat = 0;
            }

            heartbeat++;
        }
  
    }
}
