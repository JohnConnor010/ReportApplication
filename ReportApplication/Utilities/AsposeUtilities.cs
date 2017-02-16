using Aspose.Email.Mail;
using Aspose.Slides;
using Aspose.Slides.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;

namespace ReportApplication.Utilities
{
    public class AsposeUtilities
    {
        public static void PPTFindAndReplace(Presentation pres, string findString, string replaceString, int index = 0)
        {
            ISlide slide = pres.Slides[index];
            ITextFrame[] cb = SlideUtil.GetAllTextFrames(pres, true);
            for (int i = 1; i < cb.Length; i++)
            {
                foreach (Paragraph para in cb[i].Paragraphs)
                {
                    foreach (Portion port in para.Portions)
                    {
                        if (port.Text.Contains(findString))
                        {
                            string str = port.Text;
                            int idx = str.IndexOf(findString);
                            string strStartText = str.Substring(0, idx);
                            string strEndText = str.Substring(idx + findString.Length, str.Length - 1 - (idx + findString.Length - 1));
                            port.Text = strStartText + replaceString + strEndText;
                        }
                    }
                }
            }
        }
        public static SmtpClient GetSmtpClient(string path)
        {
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration(path);
            MailSettingsSectionGroup smtpGroup = (MailSettingsSectionGroup)configuration.GetSectionGroup("system.net/mailSettings");
            string host = smtpGroup.Smtp.Network.Host;
            string userName = smtpGroup.Smtp.Network.UserName;
            string password = smtpGroup.Smtp.Network.Password;
            int port = smtpGroup.Smtp.Network.Port;
            string fromEmail = smtpGroup.Smtp.From;
            SmtpClient client = new SmtpClient(host, port, userName, password);
            client.SecurityMode = SmtpSslSecurityMode.Implicit;
            return client;
        }
    }
}