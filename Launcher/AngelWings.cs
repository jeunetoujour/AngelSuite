using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using AngelRead;

namespace AngelSuite
{
    public partial class AngelWings : Form
    {
        public Offsets aionOffsets;
        public Player pc = new Player();

        int pcptr = 0;
        Entity myself = new Entity();
        bool uppressed = false;
        bool registered = false;
        int heartbeat=0;

        public AngelWings()
        {
            InitializeComponent();
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrheart.Enabled = true;

        }
        private void pressedR()
        {
            //if(pc.Stance == eStance.Flying)
            int temp = 0;
            uppressed = true;
            do
            {
                temp++;
                //pc.Update();
                float newhight = (float)(myself.Zchangeable + 0.07);
                //pc.WriteZ(newhight);
                myself.Zchangeable = newhight;
                
                Thread.Sleep(1);
            } while (temp < 40);
           
            uppressed = false;
            if (checkBox1.Checked == true) 
                Application.DoEvents();
        }

        private void pressedF()
        {
            //if(pc.Stance == eStance.Flying)
            int temp = 0;
            uppressed = true;
            do
            {
                temp++;
                //pc.Update();
                float newhight = (float)(myself.Zchangeable - 0.08);
                //pc.WriteZ(newhight);
                myself.Zchangeable = newhight;
                
                Thread.Sleep(1);
                
            } while (temp < 40);
            
            uppressed = false;
            if (checkBox1.Checked == true) 
                Application.DoEvents();
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            //if (m.Msg == WindowsShell.WM_HOTKEY && uppressed == false) label1.Text = "LParm: " + m.LParam.ToString();
            /*private Keys GetKey(IntPtr LParam)
2	{
3	    return (Keys)((LParam.ToInt32()) >> 16); // not all of the parenthesis are needed, I just found it easier to see what's happening
4	}
             * 
             * 
             * protected override void WndProc(ref Message m)
02	        {
03	            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
04	            {
05	                switch (GetKey(m.lParam))
06	                {
07	                    case Keys.A:
08	                        // the hotkey key is A
09	                        break;
10	            }
11	            base.WndProc(ref m);
12	        }
            }*/

            //if (pc.Stance == eStance.Flying || pc.Stance == eStance.FlyingCombat || pc.Stance == eStance.Gliding || pc.Stance == eStance.GlidingFlyArea)
            if (pc.isFlying > 0)
            {
                if (m.Msg == WindowsShell.WM_HOTKEY && uppressed == false && m.LParam == (IntPtr)4587520)
                { //F
                    pressedF();
                }
                else
                {
                    if(m.Msg == WindowsShell.WM_HOTKEY)
                    pressedR();
                }
            }
        }

        public void getpcptr()
        {
            EntityList elist = new EntityList(aionOffsets.entityInfoAddress);
            //elist.ENTITYLIST_OFFSET = aionOffsets.entityInfoAddress;
            elist.Update();
            //lblstatus.Text = "Status: Getting Player Entity";
            Application.DoEvents();
            foreach (Entity thing in elist)
            {
                if (thing.Name == pc.Name)
                {
                    if (thing._PtrEntity != 0)
                    {
                        pcptr = thing._PtrEntity;
                        pc.SelfPtr = thing._PtrEntity;
                        myself = thing;
                    }
                    else pcptr = thing.PtrEntity;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Process.Open();
            aionOffsets = new Offsets();
            pc = new Player();
            aionOffsets.Update();
            //pc.PLAYER_INFOADDRESS_OFFSET = aionOffsets.playerInfoAddress;
            //MessageBox.Show("Pc offset: " + pc.PLAYER_INFOADDRESS_OFFSET.ToString("X"));
            //pc.PLAYER_GUID_OFFSET = aionOffsets.pGUIDInfoAddress;
            //pc.ENTITY_OFFSET = aionOffsets.entityInfoAddress;
            pc.Updatenamelvl();
            //pc.getpcptr();
            pc.Update();
            getpcptr();
           
            //pc.WriteRot(90);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pc.UpdateFlying();
            if (pc.isFlying > 0 && registered == false)
            {
                Keys r = Keys.R;
                WindowsShell.RegisterHotKey(this, r);
                Keys f = Keys.F;
                WindowsShell.RegisterHotKey(this, f);
                registered = true;
            }
            else if(registered == true)
            {
                WindowsShell.UnregisterHotKey(this);
                WindowsShell.UnregisterHotKey(this);
                registered = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            WindowsShell.UnregisterHotKey(this);
            WindowsShell.UnregisterHotKey(this);
        }

        private void tmrheart_Tick(object sender, EventArgs e)
        {
            if (heartbeat >= 30)
            {
                Login f11 = (Login)Application.OpenForms["Login"];
                f11.theversion = "6";
                f11.program = "Wings";
                f11.heartbeat();
                heartbeat = 0;
            }

            heartbeat++;
        }

    }
    public class WindowsShell
    {
        #region fields
        public static int MOD_ALT = 0x1;
        public static int MOD_CONTROL = 0x2;
        public static int MOD_SHIFT = 0x4;
        public static int MOD_WIN = 0x8;
        public static int WM_HOTKEY = 0x312;
        #endregion

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int keyId;
        public static void RegisterHotKey(Form f, Keys key)
        {
            int modifiers = 0;

            if ((key & Keys.Alt) == Keys.Alt)
                modifiers = modifiers | WindowsShell.MOD_ALT;

            if ((key & Keys.Control) == Keys.Control)
                modifiers = modifiers | WindowsShell.MOD_CONTROL;

            if ((key & Keys.Shift) == Keys.Shift)
                modifiers = modifiers | WindowsShell.MOD_SHIFT;

            Keys k = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;

            Func ff = delegate()
            {
                keyId = f.GetHashCode(); // this should be a key unique ID, modify this if you want more than one hotkey
                RegisterHotKey((IntPtr)f.Handle, keyId, modifiers, (int)k);
            };

            f.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
        }

        private delegate void Func();

        public static void UnregisterHotKey(Form f)
        {
            try
            {
                Func ff = delegate()
                {
                    UnregisterHotKey(f.Handle, keyId); // modify this if you want more than one hotkey
                };

                f.Invoke(ff); // this should be checked if we really need it (InvokeRequired), but it's faster this way
            }
            catch (Exception)
            {

            }
        }
    }

}
