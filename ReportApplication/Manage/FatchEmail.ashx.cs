using Aspose.Email.Mail;
using Aspose.Email.Pop3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportApplication.Manage
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class FatchEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "json";
            Pop3Client client = new Pop3Client();
            client.Host = "pop.126.com";
            client.Username = "johnconnor@126.com";
            client.Password = "0105310532";
            client.Port = 110;
            client.SecurityMode = Pop3SslSecurityMode.Implicit;
            string result = string.Empty;
            try
            {
                int messageCount = client.GetMessageCount();
                MailMessage msg;
                string mailSender = string.Empty;
                List<EmailItem> items = new List<EmailItem>();
                EmailItem emailItem = new EmailItem();
                for (int i = messageCount; i >= messageCount - 2; i--)
                {
                    msg = client.FetchMessage(i);
                    if (!string.IsNullOrEmpty(msg.From.DisplayName))
                    {
                        mailSender = msg.From.DisplayName;
                    }
                    else
                    {
                        mailSender = msg.From.Address.Substring(0, msg.From.Address.LastIndexOf('@'));
                    }
                    
                    items.Add(new EmailItem
                    {
                        Subject = msg.Subject,
                        SendDate = msg.Date,
                        Sender = mailSender
                    });
                    emailItem.Items = items;

                }
                result = JsonConvert.SerializeObject(emailItem,Formatting.Indented);
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            finally
            {
                client.Disconnect();
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class EmailItem
        {
            public string Subject { get; set; }
            public string Sender { get; set; }
            public DateTime SendDate { get; set; }
            public string StrDate { get; set; }
            public IEnumerable<EmailItem> Items { get; set; }
        }
        
    }
}