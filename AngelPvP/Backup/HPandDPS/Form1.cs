﻿using System;
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
        EntityList elist = new EntityList();
        private bool isDragging = false;
        private int clickOffsetX, clickOffsetY;
        private int playerGUID = 0;

        public Form1()
        {
            InitializeComponent();
            aionOffsets = new Offsets();
            aionOffsets.Update();
            pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;
            elist.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
            pc.ENTITY_OFFSET = aionOffsets.entityInfoAddress;
            playerGUID = pc.ID;
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
            
            //pc.Updatenamelvl();
            //pc.Update();
            //elist.Update();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            pc.Updatenamelvl();
            pc.Update();
            elist.Update();
            foreach (Entity ents in elist)
            {
                if (ents.Type == eType.AttackableNPC && ents.Name != "")
                {
                    if (ents.TargetID == pc.ID && ents.Health > 0)
                    {
                        listBox1.Items.Add("(" + ents.Level + ")" + ents.Name + " D:" + string.Format("{0:f2}",ents.Distance2D(pc.X,pc.Y)));
                    }
                }
                
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
                this.Left = this.Left + e.X;
                this.Top = this.Top + e.Y;
            }
        } 

        private void label1_Click(object sender, EventArgs e)
        {

        }
  
    }
}
