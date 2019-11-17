using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SigScanLib
{
    // format:
    // ?? = wildcard
    // prefix/suffix bytes
    // XX = value we're after
    public class ByteSignature
    {
        public string Name { get; set; }
        public string Signature { get; set; }
        public ByteSignature()
        {
            Name = "New Signature";
            Signature = string.Empty;
        }

        [XmlIgnore]
        public string RegExpStr
        {
            get
            {
                string result = Signature.Trim().Replace(" ", string.Empty);
                result = result.Replace("\n", string.Empty);
                result = result.Replace("X", "([0-9|A-F])");
                result = result.Replace("?", "([0-9|A-F])");
                return result;
            }
        }

        public int TargetByteCount
        {
            get
            {
                // return the number of XX bytes in our signature
                int index = SignatureTrimmed.IndexOf('X');
                int stopIndex = SignatureTrimmed.LastIndexOf('X') + 1;
                return stopIndex - index;
            }
        }

        public int TargetByteStartIndex
        {
            get
            {
                return SignatureTrimmed.IndexOf('X');
            }
        }

        public string SignatureTrimmed
        {
            get
            {
                string val = Signature.Replace(" ", string.Empty);
                val = val.Replace("\n", string.Empty);
                return val;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
