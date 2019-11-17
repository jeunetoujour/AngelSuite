using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

using SigScanLib;

namespace SigScanLiteAion
{
    public partial class FormMain : Form
    {
        #region Private Members
        /// <summary>
        /// SigScanner object
        /// </summary>
        private SigScanner scanner;

        /// <summary>
        /// Signatures
        /// </summary>
        private List<ByteSignature> sigs;

        /// <summary>
        /// Old offsets file, new values will be inserted
        /// </summary>
        private List<Offset> offsets;

        /// <summary>
        /// Aion Process
        /// </summary>
        private Process aionProc;

        /// <summary>
        /// Game.dll
        /// </summary>
        private ProcessModule gameDll;
        #endregion

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // load offsets
            try
            {
                offsets = XmlIO.LoadObjectFromEncryptedXML<List<Offset>>(Application.StartupPath + "\\Offsets.xml", "nboffsets");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            // load signatures
            try
            {
                sigs = XmlIO.LoadObjectFromEncryptedXML<List<ByteSignature>>(Application.StartupPath + "\\EncSigs.xml", "nbsigs");
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Error loading signatures: " + ex2.ToString());
                Application.Exit();
            }
        }

        // searches for aion process and game.dll module
        private bool AionFound()
        {
            Process[] aionProcess = Process.GetProcessesByName("AION.bin");
            if (aionProcess.Count() == 0)
            {
                MessageBox.Show("Could not find AION.bin!");
                return false;
            }
            else
            {
                // grab process
                aionProc = aionProcess[0];

                // find game.dll
                foreach (ProcessModule mod in aionProc.Modules)
                {
                    if (mod.ModuleName.ToLower().Equals("game.dll"))
                    {
                        gameDll = mod;
                        btnFindAion.Enabled = false;
                        btnStartScan.Enabled = true;
                        return true;
                    }
                }

                // couldnt find game.dll
                MessageBox.Show("Could not find Game.dll!");
                return false;
            }
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            // create scanner object
            scanner = new SigScanner();

            // add event handlers
            scanner.ScanSignatureProgressChanged += new SigScanner.ScanSignatureProgressChangedEventHandler(scanner_ScanSignatureProgressChanged);
            scanner.ScanTotalProgressChanged += new SigScanner.ScanTotalProgressChangedEventHandler(scanner_ScanTotalProgressChanged);
            scanner.ScanComplete += new SigScanner.ScanCompleteEventHandler(scanner_ScanComplete);
        
            // createa  scan thread
            Thread scanThread = new Thread(Scan);
            scanThread.Start();
        }

        private void Scan()
        {
            scanner.SignatureListScan(sigs, aionProc, gameDll);
        }

        void scanner_ScanComplete(object sender, List<ScanResultData> results)
        {
            // update the offset values
            // go through each result, see if there is an offset with matching name, 
            //    and update the value of the offset.

            // enable save offsets button
            if (btnSaveOffsets.InvokeRequired)
            {
                btnSaveOffsets.BeginInvoke(new MethodInvoker(delegate()
                {
                    btnSaveOffsets.Enabled = true;
                }));
            }
            else
            {
                btnSaveOffsets.Enabled = true;
            }
        }

        void scanner_ScanTotalProgressChanged(object sender, float progress)
        {
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
            if (progressBarSig.InvokeRequired)
            {
                progressBarSig.BeginInvoke(new MethodInvoker(delegate()
                {
                    progressBarSig.Value = Convert.ToInt32(progress);
                }));
            }
            else
            {
                progressBarSig.Value = Convert.ToInt32(progress);
            }
        }
    }
}
