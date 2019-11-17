using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SigScanEditor
{
    public class SSModule
    {
        public ProcessModule Mod { get; set; }
        public SSModule(ProcessModule mod)
        {
            Mod = mod;
        }
        public override string ToString()
        {
            return Mod.ModuleName;
        }
    }
}
