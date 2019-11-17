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
    public partial class FormOffsetEditor : Form
    {
        public FormOffsetEditor()
        {
            InitializeComponent();
        }

#region Properties
        /// <summary>
        /// OffsetManager object
        /// </summary>
        private OffsetManager offsetMgr;
        public OffsetManager OffsetManager
        {
            get
            {
                if (offsetMgr == null)
                    offsetMgr = new OffsetManager();

                return offsetMgr;
            }
        }

        #region Static Form Instance
        private static FormOffsetEditor instance;
        public static FormOffsetEditor Instance
        {
            get
            {
                if (instance == null)
                    instance = new FormOffsetEditor();

                return instance;
            }
        }
        #endregion

        #region Selected Offset
        private Offset SelectedOffset
        {
            get
            {
                return (Offset)listOffsets.SelectedItem;
            }
            set
            {
                listOffsets.SelectedItem = value;
            }
        }
        #endregion
#endregion

        #region FormOffsetEditor Events
        private void FormOffsetEditor_Load(object sender, EventArgs e)
        {

        }

        private void FormOffsetEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // set the static form instance to null
            instance = null;
        }
#endregion

#region Offset Editor Methods
        /// <summary>
        /// Displays the list of offsets in the listbox.
        /// </summary>
        public void DisplayList()
        {
            listOffsets.Items.Clear();
            foreach (Offset o in OffsetManager.Offsets)
            {
                listOffsets.Items.Add(o);
            }
        }

        /// <summary>
        /// Displays the selected offset in the offset details view
        /// </summary>
        private void DisplaySelectedOffset()
        {
            if (listOffsets.SelectedIndex > -1)
            {
                txtOffsetName.Text = SelectedOffset.Name;
                txtOffsetHex.Text = SelectedOffset.HexValueString;
            }
            else
            {
                txtOffsetHex.Text = "";
                txtOffsetName.Text = "";
            }
        }
        private void SelectOffsetIndex(int index)
        {
            listOffsets.SelectedIndex = index;
        }

        private void ClearDetails()
        {
            txtOffsetHex.Text = string.Empty;
            txtOffsetName.Text = string.Empty;
        }
#endregion

        #region listOffsets methods
        /// <summary>
        /// Displays the selected offset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOffsets_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectedOffset();
            txtOffsetHex.SelectAll();
            txtOffsetHex.Focus();
        }

        #endregion

        // btnNewOffset
        private void btnNewOffset_Click(object sender, EventArgs e)
        {
            Offset o = new Offset();
            try
            {
                OffsetManager.Add(o);
                DisplayList();
                SelectOffsetIndex(listOffsets.Items.Count - 1);
                txtOffsetName.SelectAll();
                txtOffsetName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // btnDeleteOffset
        private void btnDeleteOffset_Click(object sender, EventArgs e)
        {
            int i = listOffsets.SelectedIndex;

            if (i > -1)
            {
                OffsetManager.Remove(((Offset)(listOffsets.SelectedItem)));
                DisplayList();

                int i2 = i - 1;
                if (i2 < 0)
                    i2 = 0;

                // select previous offset
                if (listOffsets.Items.Count > 0)
                    SelectOffsetIndex(i2);
            }
        }

        private void txtOffsetName_TextChanged(object sender, EventArgs e)
        {
            if (SelectedOffset != null)
            {
                int i = listOffsets.SelectedIndex;
                SelectedOffset.Name = txtOffsetName.Text;
                DisplayList();
                SelectOffsetIndex(i);
            }
        }

        private void txtOffsetHex_TextChanged(object sender, EventArgs e)
        {
            if (SelectedOffset != null)
                SelectedOffset.HexValueString = txtOffsetHex.Text;
        }

        #region SAVE FILE
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.DefaultExt = "xml";
            d.Filter = "XML Files|*.xml";

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.SaveOffsetsXML(d.FileName);
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.DefaultExt = "xml";
            d.Filter = "XML Files|*.xml";

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.SaveOffsetsXML(d.FileName);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.DefaultExt = "xml";
            d.Filter = "XML Files|*.xml";

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.SaveOffsetsXML(d.FileName);
            }
        }
        #endregion

        #region NEW FILE
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            OffsetManager.Offsets = new List<Offset>();
            DisplayList();
            ClearDetails();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OffsetManager.Offsets = new List<Offset>();
            DisplayList();
            ClearDetails();
        }
        #endregion

        #region OPEN FILE
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "xml";
            d.Filter = "XML Files|*.xml";

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.LoadOffsetsXML(d.FileName);
                DisplayList();
                ClearDetails();
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "xml";
            d.Filter = "XML Files|*.xml";

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.LoadOffsetsXML(d.FileName);
                DisplayList();
                ClearDetails();
            }
        }
        #endregion

        #region EXIT
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        // sort list
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OffsetManager.Sort();
            DisplayList();
        }

        private void mnuOpenEncrypted_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.OpenEncryptedXML(d.FileName, txtEncryptionKey.Text);
                DisplayList();
                ClearDetails();
            }
        }

        private void mnuSaveEncrypted_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.SaveAsEncryptedXML(d.FileName, txtEncryptionKey.Text);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.OpenEncryptedXML(d.FileName, txtEncryptionKey.Text);
                DisplayList();
                ClearDetails();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                OffsetManager.SaveAsEncryptedXML(d.FileName, txtEncryptionKey.Text);
            }
        }
    }
}
