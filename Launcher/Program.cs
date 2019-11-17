using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoginForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                AutoUpdate.RemotePath = "http://www.angelsuite.net/updates/";
                AutoUpdate.UpdateFileName = "AutoUpdate.txt";

                switch (AutoUpdate.UpdateFiles(AutoUpdate.RemotePath, true))
                {
                    case AutoUpdate.UpdateStatus.NothingToUpdate:
                    case AutoUpdate.UpdateStatus.ErrorInAutoUpdate:
                    case AutoUpdate.UpdateStatus.ErrorAtServer:
                    case AutoUpdate.UpdateStatus.UpdateMismatch:
                    default:
                        StartAngelSuite();
                        break;

                    case AutoUpdate.UpdateStatus.NoConnectionToServer:
                        MessageBox.Show("AutoUpdate could not connect to the remote server to\r\nmake sure you are running the latest version.\r\n\r\nPlease check that your internet connection is working.", "AutoUpdate Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        StartAngelSuite();
                        break;

                    case AutoUpdate.UpdateStatus.UpdatedSuccessfully:
                        Utils.DoGarbageCollection();
                        Application.Restart();
                        return;
                }
            }
            catch
            {
                StartAngelSuite();
            }
        }

        static void StartAngelSuite()
        {
            Application.Run(new AngelSuite.Launcher());
        }

    }
}

