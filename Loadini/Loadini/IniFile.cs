using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
namespace Ini
{
    public class IniFile
    {
        public string path;

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
          SetLastError = true,
          CharSet = CharSet.Unicode, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          string lpReturnString,
          int nSize,
          string lpFilename);
        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
          SetLastError = true,
          CharSet = CharSet.Unicode, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFilename);

        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        public string IniReadValue(string Section, string Key)
        {
            string result = new string(' ', 255);
            GetPrivateProfileString(Section, Key, "", result, 255, this.path);
            return result;

        }
        public List<string> IniGetCategories()
        {
            string returnString = new string(' ', 65536);
            GetPrivateProfileString(null, null, null, returnString, 65536, this.path);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }
        public List<string> IniGetKeys(string category)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(category, null, null, returnString, 32768, this.path);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }
    }
}