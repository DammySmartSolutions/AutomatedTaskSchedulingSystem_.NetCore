using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomatedTaskSchedulingSystem.Utility
{
    public class Utilities
    {
      
        public string message;
        public string Encryption(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(text));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();




        }

        public string Encrypt(string text)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }


        public string Decrypt(string text)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }




            return text;
        }


        //public void SendEmail(string EmpID,  string email)
        //{





        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<b>Dear Employee with EmployeeID as " + " " + EmpID + "</b> <br/>");
        //    sb.Append("<b> You have been setup as a System User on</b> <br/>");
        //    sb.Append("<b> the Automated Task Scheduling System.</b><br/>");

        //    sb.Append("<b>Enjoy the use of our robust  Automated Task Scheduling System. </b> <br/> <br/><br/><br/><br/>");

        //    sb.Append("<b><a href=#> Powered by ENGI 9874 _9839 SE TEAM GREAT </a> </b> <br/>");


        //    var config = (from s in Db.tblConfig
        //                  select new
        //                  {

        //                      Email = s.Email,

        //                      Password = s.Password,


        //                      EmailHost = s.EmailHost,
        //                      Port = s.Port,
        //                      EnableSSL = s.EnableSSL,
        //                      BodyHtml = s.BodyHtml,



        //                  }).ToArray();


        //    string Senderemail = config[0].Email;

        //    string password = config[0].Password;

        //    string host = config[0].EmailHost;

        //    int port = config[0].Port;

        //    bool ssl = config[0].EnableSSL;

        //    bool bodyhtml = config[0].BodyHtml;



        //    MailMessage message = new System.Net.Mail.MailMessage(Senderemail, email.Trim(), "ATSS", sb.ToString());

        //    SmtpClient smtp = new SmtpClient();

        //    smtp.Host = host;


        //    smtp.Port = port;

        //    smtp.Credentials = new System.Net.NetworkCredential(Senderemail, password);

        //    smtp.EnableSsl = ssl;


        //    message.IsBodyHtml = bodyhtml;

        //    smtp.Send(message);

        //}




        public void SendEmail(string EmpID, string email)
        {
            try
            {
                // Build the email body
                StringBuilder sb = new StringBuilder();
                sb.Append($"<b>Dear Employee with EmployeeID: {EmpID}</b><br/>");
                sb.Append("<b>You have been set up as a System User on</b><br/>");
                sb.Append("<b>the Automated Task Scheduling System.</b><br/><br/>");
                sb.Append("<b>Enjoy the use of our robust Automated Task Scheduling System.</b><br/><br/><br/>");
                sb.Append("<b><a href='#'>Powered by ENGI 9874 & 9839 SE TEAM GREAT</a></b><br/>");

                // Fetch config
                //var config = Db.tblConfig.FirstOrDefault();
                //if (config == null)
                //    throw new Exception("Email configuration not found.");

                //// Setup mail message
                //MailMessage message = new MailMessage
                //{
                //    From = new MailAddress(config.Email),
                //    Subject = "ATSS System User Registration",
                //    Body = sb.ToString(),
                //    IsBodyHtml = config.BodyHtml
                //};

                //message.To.Add(email.Trim());

                //// Configure SMTP client
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = config.EmailHost,
                //    Port = config.Port,
                //    EnableSsl = config.EnableSSL,
                //    Credentials = new System.Net.NetworkCredential(config.Email, config.Password)
                //};

                //smtp.Send(message);
            }
            catch (Exception ex)
            {
                // Optional: log this exception to a file or table
                System.Diagnostics.Debug.WriteLine("Email send failed: " + ex.Message);
                throw; // rethrow if you want to see the failure
            }
        }





        public void SendEmailforUpdate(string EmpID, string email)
        {
            try
            {
                // Build the email body for user update
                StringBuilder sb = new StringBuilder();
                sb.Append($"<b>Dear Employee with EmployeeID: {EmpID}</b><br/>");
                sb.Append("<b>Your system user details on the</b><br/>");
                sb.Append("<b>Automated Task Scheduling System (ATSS) have been successfully updated.</b><br/><br/>");
                sb.Append("<b>Please log in to the system to review the changes or continue your tasks.</b><br/><br/><br/>");
                sb.Append("<b><a href='#'>Powered by ENGI 9874 & 9839 SE TEAM GREAT</a></b><br/>");

                // Fetch configuration from database
                //var config = Db.tblConfig.FirstOrDefault();
                //if (config == null)
                //    throw new Exception("Email configuration not found.");

                //// Prepare mail message
                //MailMessage message = new MailMessage
                //{
                //    From = new MailAddress(config.Email),
                //    Subject = "ATSS User Details Updated",
                //    Body = sb.ToString(),
                //    IsBodyHtml = config.BodyHtml
                //};

                //message.To.Add(email.Trim());

                //// Configure SMTP
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = config.EmailHost,
                //    Port = config.Port,
                //    EnableSsl = config.EnableSSL,
                //    Credentials = new System.Net.NetworkCredential(config.Email, config.Password)
                //};

                //smtp.Send(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email send failed: " + ex.Message);
                throw;
            }
        }







        public string GeneratePassword()
        {
            string PasswordLength = "8";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";


            char[] sep = {
            ','
        };
            string[] arr = allowedChars.Split(sep);


            string IDString = "";
            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;

            }
            return NewPassword;
        }


        public string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "Please Enter  Email Address!";
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (!match.Success)
            {
                return "Invalid Email Address";
            }

            return "Valid"; // Signal for a valid email
        }








        //public void SendEmailUpdate(string fullname, string staffno, string email)
        //{





        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<b>Dear" + " " + fullname + "</b> <br/>");
        //    sb.Append("<b> Your account details with staff number:" + staffno + " on ISODEV Banking Application,</b> <br/>");
        //    sb.Append("<b> was recently updated. If you are not the one who initiated this action,</b><br/>");
        //    sb.Append("<b>kindly visit System Administrator to lodge complaints</b> <br/>");
        //    sb.Append("<b> You are strongly advised not to share your password or </b>");
        //    sb.Append("<b> enable remember password on shared system.  </b> <br/>");
        //    sb.Append("<b>Enjoy the use of our robust ISODEV Banking Application. </b> <br/> <br/><br/><br/><br/>");
        //    sb.Append("<b><a href=# > Powered by  ICT</a> </b> <br/>");


        //    var config = (from s in Db.tblConfig
        //                  select new
        //                  {

        //                      Email = s.Email,

        //                      Password = s.Password,


        //                      EmailHost = s.EmailHost,
        //                      Port = s.Port,
        //                      EnableSSL = s.EnableSSL,
        //                      BodyHtml = s.BodyHtml,



        //                  }).ToArray();


        //    string Senderemail = config[0].Email;

        //    string password = config[0].Password;

        //    string host = config[0].EmailHost;

        //    int port = config[0].Port;

        //    bool ssl = config[0].EnableSSL;

        //    bool bodyhtml = config[0].BodyHtml;



        //    MailMessage message = new System.Net.Mail.MailMessage(Senderemail, email.Trim(), "ISODEV", sb.ToString());

        //    SmtpClient smtp = new SmtpClient();

        //    smtp.Host = host;


        //    smtp.Port = port;

        //    smtp.Credentials = new System.Net.NetworkCredential(Senderemail, password);

        //    smtp.EnableSsl = ssl;


        //    message.IsBodyHtml = bodyhtml;

        //    smtp.Send(message);


        //}

        public string ToSentenceCase(string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }



        //public void SendLoginMail(string fullname, string StaffNumber, string email)
        //{
        //    try
        //    {

        //        DateTime LoginTime = DateTime.Now;

        //        string name = Environment.MachineName;
        //        string hostname = System.Net.Dns.GetHostName();

        //        string Computername = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

        //        string myIP = Dns.GetHostByName(hostname).AddressList[0].ToString();



        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<b>Dear" + " " + fullname + "," + "</b> <br/>");
        //        sb.Append("<b> You're Welcome to ISODEV Banking Application </b> <br/>");
        //        sb.Append("<b> Your Login details are given below:</b><br/>");
        //        sb.Append("<b>Username: " + StaffNumber + "</b> <br/>");
        //        sb.Append("<b>Login Time: " + LoginTime + "</b> <br/>");
        //        sb.Append("<b> System Name: " + name + "</b> <br/>");
        //        sb.Append("<b> System IP Address: " + myIP + "</b> <br/>");
        //        sb.Append("<b> You are strongly advised not to share your password or </b>");
        //        sb.Append("<b> enable remember password on shared system.  </b> <br/>");
        //        sb.Append("<b>Enjoy the use of our robust ISODEV Banking Application.. </b> <br/> <br/><br/><br/><br/>");
        //        sb.Append("<b><a href=# > Powered by  ICT</a> </b> <br/>");


        //        var config = (from s in Db.tblConfig
        //                      select new
        //                      {

        //                          Email = s.Email,

        //                          Password = s.Password,


        //                          EmailHost = s.EmailHost,
        //                          Port = s.Port,
        //                          EnableSSL = s.EnableSSL,
        //                          BodyHtml = s.BodyHtml,



        //                      }).ToArray();


        //        string Senderemail = config[0].Email;

        //        string password = config[0].Password;

        //        string host = config[0].EmailHost;

        //        int port = config[0].Port;

        //        bool ssl = config[0].EnableSSL;

        //        bool bodyhtml = config[0].BodyHtml;



        //        MailMessage message = new System.Net.Mail.MailMessage(Senderemail, email.Trim(), "CVS", sb.ToString());

        //        SmtpClient smtp = new SmtpClient();

        //        smtp.Host = host;


        //        smtp.Port = port;

        //        smtp.Credentials = new System.Net.NetworkCredential(Senderemail, password);

        //        smtp.EnableSsl = ssl;


        //        message.IsBodyHtml = bodyhtml;

        //        smtp.Send(message);

        //    }

        //    catch (Exception ex)
        //    {


        //    }

        //}



        public string GenerateRandomNumber()
        {
            var random = new Random();

            // Generate a random number less than 1 billion (9 digits)
            int randomNumber = random.Next(1000000000);

            // Ensure at least one leading digit (if the first random number is 0)
            if (randomNumber == 0)
            {
                randomNumber = random.Next(1, 1000000000);
            }

            // Convert the random number to a string and format to 13 digits
            return randomNumber.ToString("D13");
        }



    }
}
