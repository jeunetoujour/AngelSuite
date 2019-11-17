using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AngelSuite
{

    public partial class Login : Form
    {
        public string program = "";
        public string theversion = "";
        private int retrys = 0;
        public string MyConString = "Persist Security Info=False;SERVER=angelsuite.net;" +
               "DATABASE=angelew3_phbb;" +
               "UID=angelew3_phbb1;" +
               "PASSWORD=[(Lb~BJ[tKnk;;Allow User Variables=True;Encrypt=True; Compress=True;Connect Timeout=25;pooling=false"; //4e38f6c
        string mymac;
        int uid;
        string l_ip;
        string ex_ip;

        private void StartProg()
        {
            if (program == "PVP")
            {
                AngelPvP pvpfrm = new AngelPvP();
                pvpfrm.Show();
            }
            if (program == "DPS")
            {
                AngelDPS dpsfrm = new AngelDPS();
                dpsfrm.Show();
            }
            if (program == "Radar")
            {
                Radar radarfrm = new Radar();
                radarfrm.Show();
            }
            if (program == "List")
            {
                AngelList listfrm = new AngelList();
                listfrm.Show();
            }
            if (program == "AP")
            {
                AngelAP apfrm = new AngelAP();
                apfrm.Show();
            }
            if (program == "Wings")
            {
                AngelWings wingsfrm = new AngelWings();
                wingsfrm.Show();
            }
        }

        public Login()
        {

            InitializeComponent();
        }


        public long UnixTimeNow()
        {
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_TimeSpan.TotalSeconds;
        }

        public void GetExternalIp()
        {
            string whatIsMyIp = "http://www.whatismyip.com/automation/n09230945.asp";
            WebClient wc = new WebClient();
            UTF8Encoding utf8 = new UTF8Encoding();
            string requestHtml = "";
            try
            {
                requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));
            }
            catch (WebException we)
            {
                // do something with exception
                MessageBox.Show("Something fucked up yo! " + we.ToString());
            }
            try
            {
                IPAddress externalIp = IPAddress.Parse(requestHtml);
                ex_ip = externalIp.ToString();
            }
            catch (Exception) { ex_ip = "Error"; };
        }

        public bool heartbeat()
        {
            MySqlConnection connection;
            MySqlDataReader Info1;
            connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlCommand infocommand = connection.CreateCommand();
            connection.Open();
            try
            {
               
                infocommand.CommandText = "SET @a :=''; SET @b :=''; CALL AngelInfo('" + uid.ToString() + "',@a,@b);SELECT @a,@b;";
                Info1 = infocommand.ExecuteReader();
                if (Info1.Read() != false)
                {
                    string lastmac = Info1.GetString("@a");
                    if (lastmac == mymac) // Success
                    {
                        Info1.Close();
                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 8 + ");";
                        Info1 = infocommand.ExecuteReader();
                        Info1.Close();
                        connection.Close();
                        retrys = 0;
                        return true;
                    }
                    else
                    {
                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 99 + ");";
                        Info1 = infocommand.ExecuteReader();
                        Info1.Close();
                        connection.Close();
                        return false;

                    }
                }
                Info1.Close();
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
                if (retrys > 2) 
                {                     
                    return false; 
                }
                retrys++;
            };
            
            return true;
        }

        void auth(string pword, string remotehash)
        {
            // Initialize the class
            phpBBCryptoServiceProvider cPhpBB = new phpBBCryptoServiceProvider();
            // Incase your curious. =)
            //string localHash = cPhpBB.phpbb_hash("myPassword");
            // remoteHash is a hash from your SQL database.
            // getSQLhash is a function to query the database and retrieve that hash.
            // Set the result to see if the password matches or not.
            bool result = cPhpBB.phpbbCheckHash(pword, remotehash);
            MySqlConnection connection;
            MySqlDataReader Info;
            connection = new MySqlConnection(MyConString);
            MySqlCommand infocommand = connection.CreateCommand();
            IPHostEntry hostInfo = Dns.GetHostEntry("angelsuite.net");
            IPAddress[] address = hostInfo.AddressList;
            if (address[0].ToString() != "69.4.229.168")
            {
                connection.Open();
                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 5 + ");";
                Info = infocommand.ExecuteReader();
                Info.Close();
                connection.Close();
                Environment.Exit(0);
            }
            GetExternalIp();
            MySqlDataReader Checkgroup;
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            if (result)
            {// If true, the password matches!
                command.CommandText = "SET @a :='';CALL AngelCGroup('" + uid.ToString() + "',@a);SELECT @a;";
                Checkgroup = command.ExecuteReader();

                if (Checkgroup.Read() != false)
                {
                    if (!Checkgroup.IsDBNull(0)) // Yay! They have a record matching group_id = 99
                    {
                        if (Checkgroup.GetString("@a") == "9")
                        {
                            Checkgroup.Close();

                            Utils.SetAppSetting("username", username.Text);

                            infocommand.CommandText = "SET @a :=''; SET @b :=''; CALL AngelInfo('" + uid.ToString() + "',@a,@b);SELECT @a,@b;";
                            Info = infocommand.ExecuteReader();
                            if (Info.Read() != false) // Yay! They have a record matching group_id = 99
                            {
                                if (!Info.IsDBNull(0))
                                {
                                    long toffset = 666;//UnixTimeNow() - Convert.ToInt32(Info.GetString("@b"));
                                    string lastmac = Info.GetString("@a");
                                    if (lastmac == mymac) // Success
                                    {
                                        Info.Close();
                                        infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                                        Info = infocommand.ExecuteReader();
                                        Info.Close();

                                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 1 + ");";
                                        Info = infocommand.ExecuteReader();
                                        connection.Close();

                                        StartProg();
                                    }

                                    /*else if (lastmac != mymac && toffset < 60) // Failure
                                    {
                                        MessageBox.Show("Already Logged in!\nPlease wait 1 min before retrying.");
                                        connection.Close();
                                    }*/
                                    else if (lastmac != mymac && toffset >= 60) // Success
                                    {
                                        Info.Close();
                                        infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                                        Info = infocommand.ExecuteReader();
                                        Info.Close();

                                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 1 + ");";
                                        Info = infocommand.ExecuteReader();
                                        connection.Close();
                                        StartProg();

                                    }

                                }
                                else
                                {
                                    Info.Close();
                                    infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                                    Info = infocommand.ExecuteReader();
                                    Info.Close();

                                    infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 0 + ");";
                                    Info = infocommand.ExecuteReader();
                                    connection.Close();
                                    MessageBox.Show("New User!");
                                    StartProg();
                                }
                            }
                            else
                            {

                                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                                Info = infocommand.ExecuteReader();
                                connection.Close();
                                MessageBox.Show("You do not have a valid license for this bot.");
                            }


                        }
                        else
                        {
                            Checkgroup.Close();
                            infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                            Info = infocommand.ExecuteReader();
                            connection.Close();
                            MessageBox.Show("You do not have a valid license for this bot.");
                        }

                    }
                    else
                    {
                        Checkgroup.Close();
                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                        Info = infocommand.ExecuteReader();
                        connection.Close();
                        MessageBox.Show("You do not have a valid license for this bot.");
                    }
                }
                else // They weren't part of the Customer group - BAD BAD PERSON
                {
                    Checkgroup.Close();
                    infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                    Info = infocommand.ExecuteReader();
                    connection.Close();
                    MessageBox.Show("You do not have a valid license for this bot.");
                }
            }
            else // If false, the password does not!
            {
                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 9 + ");";
                Info = infocommand.ExecuteReader();
                connection.Close();
                MessageBox.Show("Authentication failed.");

            }
            
        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                // Then Enter key was pressed
                loginclick();
            }
        }

        private void loginclick()
        {
            MySqlConnection connection;
            MySqlDataReader Reader;
            connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Info;
            MySqlCommand infocommand = connection.CreateCommand();

            command.CommandText = "SET @a := ''; SET @b := '';CALL AngelLogin2('" + username.Text + "',@a,@b);SELECT @a,@b;";
            connection.Open();
            Reader = command.ExecuteReader();
            if (Reader.Read() != false)
            {
                if (!Reader.IsDBNull(0))
                {

                    uid = Convert.ToInt32(Reader.GetString("@b"));
                    string tempy = Reader.GetString("@a");

                    Reader.Close();
                    connection.Close();
                    connection.Dispose();
                    auth(password.Text, tempy);

                }
                else
                {
                    Reader.Close();

                    infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 9 + ");";
                    Info = infocommand.ExecuteReader();
                    Info.Close();
                    connection.Close();
                    MessageBox.Show("Authentication Failure");
                }

            }
            else // If false, the password does not!
            {
                connection.Close();
                MessageBox.Show("Authentication failed.");
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

            loginclick();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            ManagementObjectSearcher query = null;
            ManagementObjectCollection queryCollection = null;
            //Form1 f1 = new Form1();
            //f1.Show();
            //this.Hide();
#if (DEBUG)
            StartProg();
#endif

            password.Focus();
            
            username.Text = Utils.GetAppSetting("username");
            password.Focus();
            
            try
            {
                query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");

                queryCollection = query.Get();

                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["MacAddress"] != null)
                    {
                        if (mo["MacAddress"].ToString() != "")
                        {
                            mymac = mo["MacAddress"].ToString();
                        }
                    }
                    if (mo["IPAddress"] != null)
                    {
                        if (mo["IPAddress"].ToString() != "")
                        {
                            string[] addresses = (string[])mo["IPAddress"];
                            l_ip = addresses[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex);
            }


        }

      
    }
}
