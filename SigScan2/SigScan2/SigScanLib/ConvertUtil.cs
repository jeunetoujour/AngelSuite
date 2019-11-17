using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SigScanLib
{
    public static class ConvertUtil
    {

        public static int ByteToInt32(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        public static float ByteToFloat(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        public static string ByteToString(byte[] bytes)
        {
            Encoding unicode = Encoding.Unicode;
            Encoding ascii = Encoding.ASCII;

            // convert from unicode byte[] to ascii byte[]
            byte[] ascii_bytes = Encoding.Convert(unicode, ascii, bytes);

            // convert ascii char[] into string
            return Encoding.ASCII.GetString(ascii_bytes, 0, ascii_bytes.Length);
        }

        public static short ByteToShort(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes, 0);
        }

        public static Int32 HexStringToInt(string hex)
        {
            return Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        public static string IntToHexString(int val)
        {
            return Convert.ToInt32(val.ToString(), 16).ToString();
        }

        public static string BytesToHexString(byte[] bytes)
        {
            string temp = string.Empty;
            foreach(byte b in bytes)
            {
                temp += b.ToString("X2");
            }
            return temp;
        }
    }
}
