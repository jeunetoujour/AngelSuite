using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using SigScanLib;

namespace SigScanEditor
{
    public class OffsetManager
    {
        public OffsetManager()
        {
            offsetList = new List<Offset>();
        }

        private List<Offset> offsetList;
        public List<Offset> Offsets
        {
            get
            {
                if (offsetList == null)
                    offsetList = new List<Offset>();

                return offsetList;
            }
            set
            {
                offsetList = value;
            }
        }

        public Dictionary<string, Offset> OffsetDictionary
        {
            get
            {
                Dictionary<string, Offset> dict = new Dictionary<string, Offset>();

                foreach (Offset o in offsetList)
                {
                    try
                    {
                        dict.Add(o.Name, o);
                    }
                    catch { }
                }

                return dict;
            }
        }

        public void Add(Offset offset)
        {
            offsetList.Add(offset);
        }

        public bool LoadOffsetsXML(string file)
        {
            // try to deserialize
            try
            {
                // get the list of offsets
                offsetList = LoadObjectFromXML<List<Offset>>(file);
                return true;
            }
            catch { }
            return false;
        }

        public bool SaveOffsetsXML(string file)
        {
            try
            {
                // save to xml
                SaveObjectToXML<List<Offset>>(offsetList, file);

                return true;
            }
            catch { }
            return false;
        }

        private void SaveObjectToXML<T>(object obj, string file)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextWriter w = new StreamWriter(file);

            try
            {
                x.Serialize(w, obj);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                w.Close();
            }
        }

        private T LoadObjectFromXML<T>(string file)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextReader r = new StreamReader(file);

            T returnObj;

            try
            {
                returnObj = (T)x.Deserialize(r);
                r.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                r.Close();
            }

            return returnObj;
        }

        public void SaveAsEncryptedXML(string file, string key)
        {
            XmlIO.SaveObjectToEncryptedXML<List<Offset>>(Offsets, file, key);
        }

        public void OpenEncryptedXML(string file, string key)
        {
            // serialize
            Offsets = XmlIO.LoadObjectFromEncryptedXML<List<Offset>>(file, key);
        }

        public int Count
        {
            get
            {
                return offsetList.Count;
            }
        }

        public void Remove(int index)
        {
            offsetList.RemoveAt(index);
        }

        public void Remove(Offset item)
        {
            offsetList.Remove(item);
        }

        public void Sort()
        {
            offsetList.Sort(delegate(Offset a, Offset b)
            {
                return a.Name.CompareTo(b.Name);
            });
        }
    }
}
