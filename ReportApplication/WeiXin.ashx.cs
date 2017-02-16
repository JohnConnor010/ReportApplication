using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ReportApplication
{
    /// <summary>
    /// Summary description for WeiXin
    /// </summary>
    public class WeiXin : IHttpHandler
    {

        public static readonly string token = WebConfigurationManager.AppSettings["token"];
        public static readonly string appId = WebConfigurationManager.AppSettings["appId"];
        public static readonly string secret = WebConfigurationManager.AppSettings["secret"];
        public static readonly string encodingAESKey = WebConfigurationManager.AppSettings["encodingAESKey"];
        private HttpRequest Request = HttpContext.Current.Request;
        private HttpResponse Response = HttpContext.Current.Response;

        public void ProcessRequest(HttpContext context)
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echoStr = Request["echostr"];
            if (Request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    WriteContent(echoStr);
                    
                    
                }
                else
                {
                    WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, token) + "。" +
                                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。" + echoStr);
                }
                Response.End();
            }
            else
            {
                if (!CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    WriteContent("参数错误");
                    Response.End();
                }
                var postModel = new PostModel
                {
                    Signature = Request.QueryString["signature"],
                    Msg_Signature = Request.QueryString["msg_signature"],
                    Timestamp = Request.QueryString["timestamp"],
                    Nonce = Request.QueryString["nonce"],
                    Token = token,
                    EncodingAESKey = encodingAESKey,
                    AppId = appId
                };
                int maxRecordCount = 10;
                var messageHandler = new MyCustomHandler(Request.InputStream, postModel, maxRecordCount);

                try
                {
                    messageHandler.RequestDocument.Save(
                        HttpContext.Current.Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                                       messageHandler.RequestMessage.FromUserName + ".txt"));
                    //执行微信处理过程
                    messageHandler.Execute();
                    //测试时可开启，帮助跟踪数据
                    messageHandler.ResponseDocument.Save(
                        HttpContext.Current.Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" +
                                       messageHandler.ResponseMessage.ToUserName + ".txt"));
                    WriteContent(messageHandler.ResponseDocument.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    WriteContent(ex.Message);
                }
                finally
                {
                    Response.End();
                }
            }
        }
        public void MiniPost(string signature, string timestamp, string nonce, string echostr)
        {
            if (!CheckSignature.Check(signature, timestamp, nonce, token))
            {
                return;
            }

            var messageHandler = new CustomMessageHandler(Request.InputStream, null, 10);

            messageHandler.Execute();//执行微信处理过程
        }

        private void WriteContent(string echoStr)
        {
            Response.Output.WriteLine(echoStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}