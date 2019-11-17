using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SigScanEditor
{
    // overrides process.tostring so i can display in listbox
    public class SSProcess
    {
        public Process Proc { get; set; }
        public SSProcess(Process p)
        {
            Proc = p;
        }

        public override string ToString()
        {
            return this.Proc.ProcessName;
        }
    }
}
