using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace SigScanLib
{
    public static class XmlIO
    {
        public static void SaveObjectToXML<T>(object obj, string file)
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

        public static T LoadObjectFromXML<T>(string file)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextReader r = new StreamReader(file);

            T returnObj = default(T);

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

        public static void SaveObjectToEncryptedXML<T>(object obj, string file, string encKey)
        {
            // create xml serializer
            XmlSerializer x = new XmlSerializer(typeof(T));

            // create string writer to serialize to
            StringWriter output = new StringWriter(new StringBuilder());

            // serialize
            x.Serialize(output, obj);

            // get xml string
            string xml = output.ToString();

            // create text writer
            TextWriter writer = null;
            try
            {
                // create an instance of the text writer
                writer = new StreamWriter(file);

                // encrypt
                string enc = EncryptString(xml, encKey);

                // write to file
                writer.Write(enc);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // close writer
                writer.Close();
            }
        }

        public static T LoadObjectFromEncryptedXML<T>(string file, string encKey)
        {
            // return val
            T retVal = default(T);

            // create xml serializer
            XmlSerializer x = new XmlSerializer(typeof(T));

            // open file
            TextReader reader = null;
            string data = string.Empty;
            try
            {
                reader = new StreamReader(file);

                // read data from file
                data = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // close
                reader.Close();
            }

            // decrypt
            string dec = DecryptString(data, encKey);

            // take off any \0
            dec = dec.Replace("\0", string.Empty);

            // create text reader so we can deserializer from it
            TextReader input = null;
            try
            {
                input = new StringReader(dec);

                // serialize
                retVal = (T)x.Deserialize(input);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // close input reader
                input.Close();
            }

            return retVal;
        }

        private static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        private static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }
    }
}
