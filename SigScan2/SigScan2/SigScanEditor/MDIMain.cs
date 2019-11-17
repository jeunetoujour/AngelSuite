using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SigScanEditor
{
    public partial class MDIMain : Form
    {
        public MDIMain()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FormSigEditor.Instance.MdiParent = this;
            FormSigEditor.Instance.Show();
            FormSigEditor.Instance.BringToFront();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormScan.Instance.MdiParent = this;
            FormScan.Instance.Show();
            FormScan.Instance.BringToFront();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FormOffsetEditor.Instance.MdiParent = this;
            FormOffsetEditor.Instance.Show();
            FormOffsetEditor.Instance.BringToFront();
        }

        private void MDIMain_Load(object sender, EventArgs e)
        {

        }
    }
}
