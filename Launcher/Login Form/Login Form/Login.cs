using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace LoginForm
{

    public partial class Login : Form
    {
        private MySqlConnection sqlConn;
        private string connStr;
        private bool isConnected;
        public Login()
        {

            InitializeComponent();
        }

        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider(); 
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input); 
            bs = x.ComputeHash(bs); 
            System.Text.StringBuilder s = new System.Text.StringBuilder(); 
            foreach (byte b in bs) { s.Append(b.ToString("x2").ToLower()); } 
            string password = s.ToString(); 
            return password;
        }

        private void Login_Load_1(object sender, EventArgs e)
        {
            this.connStr = "Server=" + "gamercreation.co.uk" + ";Database=" + "angelbot_auth" + ";Uid=" + "angelbot" + ";Pwd=" + "chang3d" + "; Encrypt=true; Allow User Variables=true;"; //encryption? Allow User Variables=true;

            try
            {
                sqlConn = new MySqlConnection(this.connStr);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Error connecting you to " +
                    "the my sql server. Internal error message: " + excp.Message, excp);
                throw myExcp;
            }

            Connect();
            this.isConnected = false;
        }

        public void Connect()
        {
            bool success = true;

            if (this.isConnected == false)
            {
                try
                {
                    this.sqlConn.Open();
                }
                catch (Exception excp)
                {
                    this.isConnected = false;
                    success = false;
                    Exception myException = new Exception("Error opening connection" +
                        " to the sql server. Error: " + excp.Message, excp);

                    throw myException;
                }

                if (success)
                {
                    this.isConnected = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rc = 0;
            string returned = "";
            string Query = "SET @a := '';CALL AngelLogin('" + username.Text.Trim() + "','" + GetMD5Hash(password.Text) + "',@a);SELECT @a;";

            MySqlCommand verifyUser = new MySqlCommand(Query, this.sqlConn);

            try
            {
                rc = verifyUser.ExecuteNonQuery();

                MySqlDataReader myReader = verifyUser.ExecuteReader();

                while (myReader.Read() != false)
                {
                    returned = myReader.GetString(0);
                }

                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not verify user. Error: " +
                    excp.Message, excp);
                //throw (myExcp);
            }

            if (returned != username.Text)
            {
                MessageBox.Show("Invalid Login!");

            }
            else
            {
                MessageBox.Show("Login Verified - This is where the new form would load.");
                //this.Hide();

                //Main frmcheck = new Main();

                //frmcheck.Show();
            }
            
        }




    }
}
