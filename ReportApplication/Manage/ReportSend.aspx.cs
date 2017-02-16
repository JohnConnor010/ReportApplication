// ***********************************************************************
// Copyright (c) 2015 JohnConnor,All rights reserved.
// Project:
// Assembly:WXOpinionApp.ReportManage
// Author:JohnConnor
// Created:4/21/2015 4:25:16 PM
// Description:
// ***********************************************************************
// Last Modified By:JOHNCONNOR-PC
// Last Modified On:4/21/2015 4:25:16 PM
// ***********************************************************************
using Aspose.Email.Mail;
using iNethinkCMS.Command;
using ReportApplication;
using ReportApplication.Utilities;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WXOpinionApp.ReportManage
{
    public partial class ReportSend : System.Web.UI.Page
    {
        public static readonly string token = WebConfigurationManager.AppSettings["token"];
        public static readonly string appId = WebConfigurationManager.AppSettings["appId"];
        public static readonly string secret = WebConfigurationManager.AppSettings["secret"];
        public static readonly string encodingAESKey = WebConfigurationManager.AppSettings["encodingAESKey"];
        private readonly string openId = "oJ83Ct8IIhLwQPEqV6djzHI2KPw4";
        private string accessToken = CommonApi.GetToken(appId,secret).access_token;
        
        public FileCompatibilityMode MessageLoadOptions { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            
        }
        public void InitOpenIdRadioButtonList()
        {
            var results = Senparc.Weixin.MP.AdvancedAPIs.User.Get(accessToken, null);
            foreach(var result in results.data.openid)
            {
                ListItem item = new ListItem();
                item.Value = result;
                item.Text = Senparc.Weixin.MP.AdvancedAPIs.User.Info(accessToken, result).nickname;
                item.Attributes.Add("class", "checkbox-inline input-sm");
                this.rblOpenId.Items.Add(item);
            }
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            string emailPath = Request.MapPath("~/ReportManage/template_files/mail.html");
            MailMessage msg = new MailMessage();
            string emails = this.txtEmail.Text.Replace(',', ' ').Replace(';', ' ');
            for (int i = 0; i < emails.Split(' ').Length; i++)
            {
                msg.To.Add(emails.Split(' ')[i]);
            }
            msg.From = "sdcecn01@126.com";
            msg.Subject = this.txtSubject.Text;
            msg.IsBodyHtml = true;
            msg.HtmlBody = this.txtEmailContent.Text;
            if (!String.IsNullOrEmpty(this.txtFilePath1.Text))
            {
                Attachment attachment = new Attachment(Server.MapPath(this.txtFilePath1.Text));
                msg.Attachments.Add(attachment);
            }
            if (!String.IsNullOrEmpty(this.txtFilePath2.Text))
            {
                Attachment attachment = new Attachment(Server.MapPath(this.txtFilePath2.Text));
                msg.Attachments.Add(attachment);
            }
            if (!String.IsNullOrEmpty(this.txtFilePath3.Text))
            {
                Attachment attachment = new Attachment(Server.MapPath(this.txtFilePath3.Text));
                msg.Attachments.Add(attachment);
            }
            SmtpClient client = AsposeUtilities.GetSmtpClient(this.Request.ApplicationPath);
            try
            {
                client.Send(msg);
                MessageBox.ShowAndRedirect(this, "邮件发送成功", this.Request.Path);
            }
            catch (Exception ex)
            {
                MessageBox.ShowAndRedirect(this, "邮件发送失败", this.Request.Path);
            }
        }
        public string GetEmailContent(string emailPath)
        {
            FileInfo fileInfo = new FileInfo(emailPath);
            return fileInfo.ReadToEnd();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //FileInfo fileInfo = new FileInfo(Server.MapPath("~/ReportManage/template/weixin.txt"));

            //string content = fileInfo.ReadToEnd(Encoding.UTF8);
            //var accessToken = CommonApi.GetToken(WeiXin.appId, WeiXin.secret).access_token;
            //Custom.SendText(accessToken, openId, content);

            
            Article article = new Article
            {
                Title = "这是标题",
                Description = "当用户主动发消息给公众号的时候（包括发送信息、点击自定义菜单、订阅事件、扫描二维码事件、支付成功事件、用户维权），微信将会把消息数据推送给开发者，开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。",
                PicUrl = "http://img.cnblogs.com/ad/shilipai_201504.png",
                Url = "http://mp.weixin.qq.com/wiki/7/12a5a320ae96fecdf0e15cb06123de9f.html"

            };
            List<Article> articles = new List<Article>();
            articles.Add(article);
            var sendResult = Custom.SendNews(accessToken, openId, articles);
            
        }

        protected void btnWeiXinSubmit_Click(object sender, EventArgs e)
        {
            string content = this.txtWeiXinContent.Text;
            try
            {
                foreach (ListItem item in this.rblOpenId.Items)
                {
                    if (item.Selected == true)
                    {
                        Custom.SendText(accessToken, item.Value, content);
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('微信发送成功')", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('微信发送可能，可能部分用户我发收到微信内容！')", true);
            }
        }

        protected void btnSendSMS_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.txtMobiles.Text))
            {
                foreach(var mobile in this.txtMobiles.Text.Split(' '))
                {
                    string content = this.txtSMSContent.Text;
                    string url = this.txtUrl.Text;
                    string analyze = this.txtAnalyze.Text;
                    FileInfo fileInfo = new FileInfo(Server.MapPath("~/ReportManage/template_files/sms_template.txt"));
                    string str = fileInfo.ReadAllText();
                    //string value = String.Format(str, DateTime.Now.ToString("yyyy年MM月dd日"), content, url, analyze);
                    string value = "【类聚平台】尊敬的顾客：本公司对您的订单号为：12345的订单进行了支付状态进行了更改并缴费12.23元，如有任何疑意，请您及时联系本公司！";
                    string result = SMSUtility.SendSMS(value, mobile);
                }
            }
            
        }
        
    }
}