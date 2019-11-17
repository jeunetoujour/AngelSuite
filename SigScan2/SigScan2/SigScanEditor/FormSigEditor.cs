using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using SigScanLib;

namespace SigScanEditor
{
    public partial class FormSigEditor : Form
    {
        public FormSigEditor()
        {
            InitializeComponent();
        }

        #region PUBLIC PROPERTIES
        #region Static Form instance
        private static FormSigEditor instance;
        public static FormSigEditor Instance
        {
            get
            {
                if (instance == null)
                    instance = new FormSigEditor();

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion

        #region ByteSignature List
        private List<ByteSignature> signatureList;
        public List<ByteSignature> SignatureList
        {
            get
            {
                if (signatureList == null)
                    signatureList = new List<ByteSignature>();

                return signatureList;
            }
            set
            {
                signatureList = value;
            }
        }
        #endregion
        #endregion

        #region PRIVATE PROPERTIES .... penises and vajayjays.
        private ByteSignature SelectedSignature
        {
            get
            {
                return (ByteSignature)listBoxSignatures.SelectedItem;
            }
        }
        #endregion

        #region FormEditor Events
        private void FormEditor_Load(object sender, EventArgs e)
        {

        }

        private void FormSigEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }
        #endregion

        #region MENU EVENTS
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("New file?", "Confirm New File", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SignatureList = new List<ByteSignature>();
                DisplayList();
                ClearSelection();
            }
        }        
        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Load as new signature file?", "Load or Insert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    OpenFile(d.FileName);
                else
                {
                    List<ByteSignature> insert = XmlIO.LoadObjectFromXML<List<ByteSignature>>(d.FileName);
                    foreach (ByteSignature sig in insert)
                    {
                        SignatureList.Add(sig);
                        DisplayList();
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SaveFile(d.FileName);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SaveFile(d.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region TOOLBAR EVENTS
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("New file?", "Confirm New File", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SignatureList = new List<ByteSignature>();
                DisplayList();
                ClearSelection();
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Load as new signature file?", "Load or Insert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    OpenFile(d.FileName);
                else
                {
                    List<ByteSignature> insert = XmlIO.LoadObjectFromXML<List<ByteSignature>>(d.FileName);
                    foreach (ByteSignature sig in insert)
                    {
                        SignatureList.Add(sig);
                        DisplayList();
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SaveFile(d.FileName);
            }
        }

        private void btnNewSig_Click(object sender, EventArgs e)
        {
            AddSignature();
        }

        private void btnDeleteSignature_Click(object sender, EventArgs e)
        {
            DeleteSelectedSig();
        }
        #endregion

        #region Private Methods
        private void NewFile()
        {
            SignatureList = new List<ByteSignature>();
            DisplayList();
            ClearSelection();
        }

        private void SaveFile(string file)
        {
            try
            {
                XmlIO.SaveObjectToXML<List<ByteSignature>>(SignatureList, file);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void OpenFile(string file)
        {
            try
            {
                SignatureList = XmlIO.LoadObjectFromXML<List<ByteSignature>>(file);
                DisplayList();
                ClearSelection();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Adds a new signature to the list.
        /// </summary>
        private void AddSignature()
        {
            SignatureList.Add(new ByteSignature());
            DisplayList();
            SelectSignature(SignatureList.Count - 1);
        }

        /// <summary>
        /// Displays the signature list.
        /// </summary>
        private void DisplayList()
        {
            listBoxSignatures.Items.Clear();
            foreach (ByteSignature sig in SignatureList)
            {
                listBoxSignatures.Items.Add(sig);
            }
        }

        /// <summary>
        /// Displays the selected signature object
        /// </summary>
        /// <param name="sig"></param>
        private void DisplaySignature(ByteSignature sig)
        {
            txtName.Text = sig.Name;
            txtSig.Text = sig.Signature;
        }

        private void SelectSignature(int index)
        {
            if (index > -1)
            {
                listBoxSignatures.SelectedIndex = index;
                DisplaySignature(SelectedSignature);
            }
            else
                ClearDisplay();
        }

        private void DeleteSelectedSig()
        {
            int i = listBoxSignatures.SelectedIndex;

            if (i > -1)
            {
                // remove sig
                SignatureList.Remove(SelectedSignature);

                // update list
                DisplayList();

                // select previous sig
                i--;
                if (i < 0)
                    i = 0;

                if (signatureList.Count == 0)
                    i = -1;

                SelectSignature(i);
            }
        }

        private void ClearSelection()
        {
            if (listBoxSignatures.SelectedIndex > -1)
                listBoxSignatures.SelectedIndex = -1;

            ClearDisplay();
        }

        private void ClearDisplay()
        {
            txtName.Text = string.Empty;
            txtSig.Text = string.Empty;
        }
        #endregion

        private void listBoxSignatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectSignature(listBoxSignatures.SelectedIndex);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = listBoxSignatures.SelectedIndex;
                SelectedSignature.Name = txtName.Text;
                DisplayList();
                SelectSignature(i);
            }
            catch
            {
                ClearDisplay();
            }
        }

        private void txtSig_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedSignature.Signature = txtSig.Text;
            }
            catch
            {
                ClearDisplay();
            }
        }

        private void btnSortSigs_Click(object sender, EventArgs e)
        {
            SignatureList.Sort(delegate(ByteSignature a, ByteSignature b)
            {
                return a.Name.CompareTo(b.Name);
            });

            DisplayList();
            ClearDisplay();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SignatureList = XmlIO.LoadObjectFromEncryptedXML<List<ByteSignature>>(d.FileName, txtEncKey.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlIO.SaveObjectToEncryptedXML<List<ByteSignature>>(SignatureList, d.FileName, txtEncKey.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
