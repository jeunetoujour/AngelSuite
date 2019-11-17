using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SigScanLib
{
    public class ScanResultData
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public ScanResult Result { get; set; }

        public ScanResultData()
        {
            Values = new List<string>();
            Name = string.Empty;
            Result = ScanResult.Failure;
        }
    }

    public enum ScanResult
    {
        Success, 
        Failure
    }
}
