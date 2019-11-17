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

namespace HPandDPS
{
    public partial class Form1 : Form
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
        public Player pc = new Player();
        private bool isDragging = false;
        private int clickOffsetX, clickOffsetY;
        private int apgain = 0;
        private int aploss = 0;
        private int lastap = 0;

        public Form1()
        {
            InitializeComponent();
            aionOffsets = new Offsets();
            aionOffsets.Update();
            pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;

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
            pc.Updatenamelvl();
            pc.UpdateAP();
            lastap = pc.AP;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pc.UpdateAP();
            if (pc.AP > lastap) //gained ap
            {
                apgain = apgain + (pc.AP - lastap);
                lastap = pc.AP;
                label1.Text = "APGain: " + apgain;
            }
            else if (pc.AP < lastap) //Lost ap
            {
                aploss = aploss + (lastap - pc.AP);
                lastap = pc.AP;
                label3.Text = "APLoss: " + aploss;
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

    }
}
