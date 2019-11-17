using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using SigScanLib;

namespace SigScanEditor
{
    public partial class FormScan : Form
    {
        public FormScan()
        {
            InitializeComponent();

            // create Scanner
            Scanner = new SigScanner();

            // handle scanner progress events
            Scanner.ScanSignatureProgressChanged += new SigScanner.ScanSignatureProgressChangedEventHandler(scanner_ScanSignatureProgressChanged);
            Scanner.ScanTotalProgressChanged += new SigScanner.ScanTotalProgressChangedEventHandler(scanner_ScanTotalProgressChanged);
            Scanner.ScanComplete += new SigScanner.ScanCompleteEventHandler(scanner_ScanComplete);
        }

        #region PROPERTIES
        SigScanner Scanner { get; set; }
        #endregion

        #region Static Form Instance
        private static FormScan instance;
        public static FormScan Instance
        {
            get
            {
                if (instance == null)
                    instance = new FormScan();

                return instance;
            }
            set
            {
                instance = value;
            }

        }
        #endregion

        #region FormScan Events
        private void FormScan_Load(object sender, EventArgs e)
        {

        }

        private void FormScan_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }
        #endregion

        private void btnRefreshProcs_Click(object sender, EventArgs e)
        {
            RefreshProcs();
        }

        private void RefreshProcs()
        {
            listProcs.Items.Clear();
            listMods.Items.Clear();

            Process[] list = Process.GetProcesses();

            // create list of SSProcesses
            List<SSProcess> procs = new List<SSProcess>();
            foreach (Process p in list)
            {
                procs.Add(new SSProcess(p));
            }

            // sort procs
            procs.Sort(delegate(SSProcess a, SSProcess b)
            {
                return a.ToString().CompareTo(b.ToString());
            });

            foreach (SSProcess p in procs)
            {
                listProcs.Items.Add(p);
            }
        }

        private void RefreshMods()
        {
            listMods.Items.Clear();

            try
            {

                // get selected process
                SSProcess sel = ((SSProcess)listProcs.SelectedItem);

                // list of modules
                List<SSModule> mods = new List<SSModule>();

                // print modules
                foreach (ProcessModule mod in sel.Proc.Modules)
                {
                    mods.Add(new SSModule(mod));
                }

                // sort modules
                mods.Sort(delegate(SSModule a, SSModule b)
                {
                    return a.ToString().CompareTo(b.ToString());
                });

                // display modules
                foreach (SSModule mod in mods)
                {
                    listMods.Items.Add(mod);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void listProcs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listProcs.SelectedIndex > -1)
            {
                RefreshMods();
            }
            else
            {
                listMods.Items.Clear();
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            // create thread to execute scan in
            Thread scanThread = new Thread(ScanFunction);
            scanThread.Start();
        }

        private void ScanFunction()
        {
            // scan for the signature list loaded in FormSigEditor
            Scanner.SignatureListScan(FormSigEditor.Instance.SignatureList,
                                      ((SSProcess)listProcs.SelectedItem).Proc,
                                      ((SSModule)listMods.SelectedItem).Mod);
        }

        void scanner_ScanComplete(object sender, List<ScanResultData> results)
        {
            // scan is complete
            string resultText = string.Empty;
            foreach (ScanResultData result in results)
            {
                string thisResult = result.Name + ": ";
                foreach (string val in result.Values)
                {
                    thisResult += val + " ";
                }
                resultText += thisResult + Environment.NewLine;
            }

            if (txtResultsText.InvokeRequired)
            {
                txtResultsText.BeginInvoke(new MethodInvoker(delegate()
                    {
                        txtResultsText.Text = resultText;
                    }));
            }
            else
            {
                txtResultsText.Text = resultText;
            }
        }

        void scanner_ScanTotalProgressChanged(object sender, float progress)
        {
           // iupdate progress
            if (progressBarScan.InvokeRequired)
            {
                progressBarScan.BeginInvoke(new MethodInvoker(delegate()
                {
                    progressBarScan.Value = Convert.ToInt32(progress);
                }));
            }
            else
            {
                progressBarScan.Value = Convert.ToInt32(progress);
            }
        }

        void scanner_ScanSignatureProgressChanged(object sender, float progress)
        {
            // iupdate progress
            if (progressBarSignature.InvokeRequired)
            {
                progressBarSignature.BeginInvoke(new MethodInvoker(delegate()
                {
                    progressBarSignature.Value = Convert.ToInt32(progress);
                }));
            }
            else
            {
                progressBarSignature.Value = Convert.ToInt32(progress);
            }
        }

        private void btnOutputOffsets_Click(object sender, EventArgs e)
        {
            FormOffsetEditor.Instance.OffsetManager.Offsets = new List<Offset>();

            // go through scan results, create offsets
            foreach (ScanResultData result in Scanner.Results)
            {
                Offset o = new Offset();
                o.Name = result.Name;
                o.HexValueString = result.Values[0];
                FormOffsetEditor.Instance.OffsetManager.Offsets.Add(o);
            }

            FormOffsetEditor.Instance.Show();
            FormOffsetEditor.Instance.BringToFront();
            FormOffsetEditor.Instance.DisplayList();
        }


    }
}
