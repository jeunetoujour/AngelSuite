using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AngelRead;

namespace AngelSuite
{
    public partial class AngelTarget : Form
    {

        EntityList elist = new EntityList();
        Player pc = new Player();
        Target tar = new Target();
        KeyEnumer keye = new KeyEnumer();
        Entity myself = new Entity();

        int pcptr = 0;

        public AngelTarget()
        {
            InitializeComponent();
        }

        public void getpcptr()
        {

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

        private void AngelTarget_Load(object sender, EventArgs e)
        {
            Process.Open();
            pc.Updatenamelvl();
            pc.Update();
           
            elist.Update();
            getpcptr();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tar.UpdateID();
            elist.Update();
            textBox2.Text = tar.ID.ToString();
            label1.Text = pc.ID.ToString();
            label2.Text = myself.TargetID.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keye.PauseForMilliSeconds(1400);
            getpcptr();
            try
            {
                int newid = int.Parse(textBox1.Text);
                //keye.keyenumerator("ESC");
                tar.Update();
                if (tar.Name == pc.Name || (tar.ID != newid && tar.Name != pc.Name && tar.Name != "" && tar.Name != null)) { keye.keyenumerator("ESC"); keye.PauseForMilliSeconds(100); }
                tar.Update();
                
                while (tar.TargetID != newid && myself.TargetID != newid && (tar.Name == pc.Name|| tar.Name == "" || tar.Name == null) )
                {
                    if (tar.Name == pc.Name) { keye.keyenumerator("ESC"); keye.PauseForMilliSeconds(100); }
                    //myself.Update();
                    //if (tar.TargetID == newid && myself.TargetID == newid && tar.Name != pc.Name)
                     //   break;
                    myself.WriteID(newid);
                    
                    keye.PauseForMilliSeconds(40);
                    keye.keyenumerator("F1");
                  
                    keye.PauseForMilliSeconds(200);
                    label1.Text = myself.ID.ToString();
                    label2.Text = myself.TargetID.ToString();
                    textBox2.Text = tar.ID.ToString();
                    myself.WriteID(pc.ID);
                    //pc.TargetID = 0;
                    
                    //tar.Update();
                   // myself.Update();
                    /*tar.UpdateID();
                    if (tar.TargetID == newid && myself.TargetID == newid) 
                        break;
                    */
                    keye.PauseForMilliSeconds(400);
                    tar.UpdateID();
                    tar.Update();
                    if (tar.Name == pc.Name) { keye.keyenumerator("ESC"); keye.PauseForMilliSeconds(100); }
                    myself.Update();
                    //if (tar.TargetID == newid && myself.TargetID == newid && tar.Name != pc.Name) 
                    //    break;
                    
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.ToString()); 
            }
            
        }
    }
}
