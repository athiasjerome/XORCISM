using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Configuration;

using XORCISMModel;
using System.Xml;
using System.Reflection;

namespace XCommon
{
    public enum Algorithm
    {
        SHA1,
        MD5
    }

    public enum RIGHT
    {
        RELOAD,
        UNLOCK,
        CREATE, 
        MODIFY,
        DELETE,
        VIEW
    };

    public enum STATUS
    {
        FINISHED,
        IDLE,
        TOCANCEL,
        CANCELLING,
        RUNNING,
        ERROR,
        CANCELED
    }

    public class Utils
    {
        static public void Helper_Trace(string module, string message)
        {
            DateTimeOffset dt;
            dt = DateTimeOffset.Now;

            string s="";
            try
            {
                s = string.Format("{0} {1}:{2}:{3}.{4} : {5} : {6}", dt.ToString(), dt.Hour, dt.Minute, dt.Second.ToString("00"), dt.Millisecond.ToString("000"), module, message);
                Trace.WriteLine(s);
            }
            catch(Exception exHelper_Trace)
            {
                Trace.WriteLine("Exception exHelper_Trace " + exHelper_Trace.Message + " " + exHelper_Trace.InnerException);
            }
            
        }

        static public string Helper_Encrypt(string source,Algorithm algo)
        {
            StringBuilder hashed=new StringBuilder();
            byte[] sourceByte=new byte[source.Length];
            for(int i=0;i<source.ToArray().Length;i++)
                sourceByte[i]=(byte)source[i];
            byte[] destByte = null;

            switch (algo)
            {
                case Algorithm.MD5:
                    {
                        destByte = MD5.Create().ComputeHash(sourceByte);
                        
                    }break;
                case Algorithm.SHA1:
                    {
                        destByte = SHA1.Create().ComputeHash(sourceByte);
                    } break;
            }
            foreach (byte dByte in destByte)
                hashed.Append(dByte);
            return hashed.ToString();
        }

        static public bool Helper_SendEmail(string Tos,string Subject, string Message)
        {
            string From = "contact@hackenaton.org"; //HARDCODED

            System.Net.Mail.SmtpClient vSmtpClient;
            vSmtpClient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTP_SERVER"], Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]));

            vSmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            vSmtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTP_USERNAME"], ConfigurationManager.AppSettings["SMTP_PASSWORD"]);
            vSmtpClient.EnableSsl = true;

            try
            {
                //System.Net.Mail.MailMessage vMessage;
                //vMessage = new System.Net.Mail.MailMessage();

                //string[] tab_Tos = Tos.Split(new char[] {','});
                //foreach(string To in tab_Tos)
                //    if(!string.IsNullOrWhiteSpace(To))
                //        vMessage.To.Add(To);
                ////vMessage.To.Add("athiasjerome@gmail.com");  //HARDCODED                
                //vMessage.Bcc.Add("athiasjerome@gmail.com");   //HARDCODED
                ////"contact@hackenaton.org"
                //vMessage.From = new System.Net.Mail.MailAddress(From);
                //vMessage.Subject = Subject;
                //vMessage.IsBodyHtml = true;
                //vMessage.Body = Message;
                //vMessage.Priority = System.Net.Mail.MailPriority.Normal;
                //vSmtpClient.Send(vMessage);

                CDO.Message message = new CDO.Message();
                CDO.IConfiguration configuration = message.Configuration;
                ADODB.Fields fields = configuration.Fields;

                ADODB.Field field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
                field.Value = ConfigurationManager.AppSettings["SMTP_SERVER"];

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
                field.Value = 465;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
                field.Value = CDO.CdoSendUsing.cdoSendUsingPort;

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
                field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
                field.Value = ConfigurationManager.AppSettings["SMTP_USERNAME"];

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
                field.Value = ConfigurationManager.AppSettings["SMTP_PASSWORD"];

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
                field.Value = "true";

                fields.Update();

                Utils.Helper_Trace("Email Service", String.Format("Building CDO Message..."));

                message.From = From;
                string[] tab_Tos = Tos.Split(new char[] { ',' });
                foreach (string To in tab_Tos)
                    if (!string.IsNullOrWhiteSpace(To))
                        message.To=To;  //TODO
                message.Subject = Subject;
                message.TextBody = Message;


                // Send message.
                message.Send();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("Email Service","Error sending email to the administrator : " + ex.Message+" "+ex.InnerException);
                return false;
            }
            
            Utils.Helper_Trace("XORCISM Email Service", "Email sent");
            return true;
        }

        static public bool Helper_RightEnforcement(Guid userID, string securableType, string securableID, RIGHT right)
        {
            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();

            if (model.USERACCOUNT.FirstOrDefault(o => o.UserID == userID && o.UserAccountTypeID == 1) != null)  //TODO Hardcoded "Administrator"
                return true;

            List<string> listRACI;
            listRACI = new List<string>();

            switch (right)
            {
                case RIGHT.CREATE:
                    listRACI.Add("R");
                    break;

                case RIGHT.MODIFY:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    listRACI.Add("C");
                    break;

                case RIGHT.DELETE:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    break;

                case RIGHT.VIEW:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    listRACI.Add("C");
                    listRACI.Add("I");
                    break;
            }

            foreach (string raci in listRACI)
            {
                //RACISECURABLEINSTANCE raciInstance;
                //raciInstance = model.RACISECURABLEINSTANCE.FirstOrDefault(o => o.UserID == userID && o.SecurableType == securableType && o.SecurableID == securableID && o.RACIValue == raci);

                //if (raciInstance != null)
                    return true;
            }

            return false;
        }

        static public bool Helper_RightEnforcement(Guid userID, string securableType, RIGHT right)
        {
            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();

            if (model.USERACCOUNT.FirstOrDefault(o => o.UserID == userID && o.UserAccountTypeID == 1) != null)  //TODO Hardcoded "Administrator"
                return true;

            List<string> listRACI;
            listRACI = new List<string>();

            switch (right)
            {
                case RIGHT.CREATE:
                    listRACI.Add("R");
                    break;

                case RIGHT.MODIFY:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    listRACI.Add("C");
                    break;

                case RIGHT.DELETE:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    break;

                case RIGHT.VIEW:
                    listRACI.Add("R");
                    listRACI.Add("A");
                    listRACI.Add("C");
                    listRACI.Add("I");
                    break;
            }

            foreach (string raci in listRACI)
            {
                //RACISECURABLE raciSecurable;
                //raciSecurable = model.RACISECURABLE.FirstOrDefault(o => o.UserID == userID && o.SecurableType == securableType && o.RACIValue == raci);

                //if (raciSecurable != null)
                    return true;
            }

            return false;
        }

        //static public void Helper_Notify(Guid userID, string securableType, string securableID, XCommon.RIGHT right)
        static public void Helper_Notify(int userID, string securableType, string securableID, XCommon.RIGHT right)
        {
            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();

            string username="";
            //username = model.USERS.FirstOrDefault(o => o.UserId == userID).UserName;
            username = model.USER.FirstOrDefault(o => o.UserID == userID).UserName;

            USERACCOUNT uia=null;
            //uia = model.USERACCOUNT.FirstOrDefault(o => o.UserID == userID);    //TODO review uniqueidentifier and uncomment

            int accountID;
            accountID = uia.ACCOUNT.AccountID;

            //List<RACISECURABLEINSTANCE> list;
            //list = model.RACISECURABLEINSTANCE.Where(o => o.SecurableType == securableType && o.SecurableID == securableID && o.AccountID == accountID && o.RACIValue == "I").ToList();

            string verb = "";
            switch (right)
            {
                case XCommon.RIGHT.CREATE:
                    verb = "created";
                    break;

                case XCommon.RIGHT.DELETE:
                    verb = "deleted";
                    break;

                case XCommon.RIGHT.MODIFY:
                    verb = "modified";
                    break;

                case XCommon.RIGHT.UNLOCK:
                    verb = "unlocked";
                    break;

                case XCommon.RIGHT.RELOAD:
                    verb = "reloaded";
                    break;
            }

            /*
            foreach (RACISECURABLEINSTANCE instance in list)
            {
                NOTIFICATION notification;
                notification = new XORCISMModel.NOTIFICATION();
                notification.CreatedDate = DateTimeOffset.Now;
                notification.UserID = instance.UserID;

                switch (securableType)
                {
                    case "TASK_ASSET":
                        notification.NotificationMessage = string.Format("The asset #{0} has been {1} by user '{2}'", securableID, verb, username);
                        break;
                    case "TASK_USER":
                        notification.NotificationMessage = string.Format("The user #{0} has been {1} by user '{2}'", securableID, verb, username);
                        break;
                    case "TASK_JOB":
                        notification.NotificationMessage = string.Format("The user #{0} has been {1} by user '{2}'", securableID, verb, username);
                        break;
                }
                notification.timestamp = DateTimeOffset.Now;
                model.NOTIFICATION.Add(notification);
            }

            model.SaveChanges();
            */
        }

        public static int GetMaxPageBySubscriptionLevel(Guid userID, int Service, XmlDocument xDoc)
        {
            //NOTE: code removed
            return 999999;
            
        }

        public static string FindCPE(string rawString)
        {
            string myCPE = string.Empty;
            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();
                       

            // define which character is seperating fields
            char[] splitter = { ' ' };

            string[] words = rawString.Split(splitter);

            for (int x = 0; x < words.Length; x++)
            {
                
                var possiblecpes = from c in model.CPE
                                   where SqlMethods.Like(c.CPEName, "%" + words[x].ToLower()+"%")
                                   select c;
            }

            return myCPE;
        }

    }
}
