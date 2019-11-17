using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace SigScanLiteAion
{
    public class Offset
    {
        public Offset()
        {
            Name = "New Offset";
            HexValueString = "0";
        }

        public string Name { get; set; }

        public string HexValueString { get; set; }

        [XmlIgnore]
        public uint ToUInt
        {
            get
            {
                return UInt32.Parse(HexValueString, System.Globalization.NumberStyles.HexNumber);
            }
        }

        [XmlIgnore]
        public int ToInt32
        {
            get
            {
                return Int32.Parse(HexValueString, System.Globalization.NumberStyles.HexNumber);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
