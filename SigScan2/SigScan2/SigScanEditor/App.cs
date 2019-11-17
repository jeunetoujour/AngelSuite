using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SigScanEditor
{
    public static class App
    {
        #region Single Instance Forms
        /// <summary>
        /// FormEditor single instance
        /// </summary>
        private static FormSigEditor formEditor;
        public static FormSigEditor FormEditor
        {
            get
            {
                if (formEditor == null)
                    formEditor = new FormSigEditor();

                return formEditor;
            }
        }

        /// <summary>
        /// FormScan single instance
        /// </summary>
        private static FormScan formScan;
        public static FormScan FormScan
        {
            get
            {
                if (formScan == null)
                    formScan = new FormScan();

                return formScan;
            }
        }
        #endregion
    }
}
