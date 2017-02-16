using System.IO;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Sample.CommonService;
using Senparc.Weixin.MP.Entities.BaiduMap;
using System.Collections.Generic;
using Senparc.Weixin.MP.Helpers;
using System;

namespace ReportApplication
{
    public class MyCustomHandler : MessageHandler<CustomMessageContext>
    {
        string accessToken = CommonApi.GetToken(WeiXin.appId, WeiXin.secret).access_token;
        private readonly string openId = "oPWfHt3_ReEKaAT9W9wB-oVdMdaQ";
        public MyCustomHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            WeixinContext.ExpireMinutes = 3;
        }
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase responseMessage = null;
            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {
                if (requestMessage.EventKey == "price")
                {
                    var strongResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
                    responseMessage = strongResponseMessage;
                    strongResponseMessage.Content = "Price";
                }
                if (requestMessage.EventKey == "hotPad")
                {
                    var strongResponseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                    responseMessage = strongResponseMessage;
                    var article = new Article
                    {
                        Title = "马化腾对“互联网+”的最新阐释：能激活更多信息能源",
                        Description = "4月29日，为进一步探讨“互联网+”的发展趋势与前景，促进城市管理创新与产业转型升级，腾讯下午在北京钓鱼台国宾馆举办“势在·必行——2015‘互联网+中国’峰会”，与500位政府官员、各地领导一起，共同就“互联网+”主题展开深入探讨。",
                        PicUrl = "http://mmbiz.qpic.cn/mmbiz/ow6przZuPIFibQ9lW3uNp3xiaibxeFwDbiaNibc2nVVciajmzPytCbPQ2SGSxScTBVu4iarXoUj6huPaXgeC4ia3XmPicQQ/0?wx_fmt=jpeg&wxfrom=5",
                        Url = "http://mp.weixin.qq.com/s?__biz=Mjc1NjM3MjY2MA==&mid=215477879&idx=1&sn=d93719b8570eb0b771e6e22c735f4cf1&key=1936e2bc22c2ceb58b906cc018c3f3d81c21bb23aac1bd3760ac5d88e5693d458d91aea494184af172c86cd57edd922d&ascene=1&uin=MzQ2NzE1&devicetype=Windows+8&version=61000721&pass_ticket=jX95BjyK8RT0TYTIT%2BNDh5FGQFDry5NRWDEip4K5i6Q%3D"
                    };
                    strongResponseMessage.Articles.Add(article);
                }
                //using (var context = new WeiXinDataContext())
                //{
                //    var entity = context.WeiXinMenus.FirstOrDefault(m => m.Key == requestMessage.EventKey);
                //    if (entity.ClickType == "text")
                //    {
                //        var strongResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
                //        responseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = entity.Content;
                //    }
                //    else if (entity.ClickType == "news")
                //    {
                //        var strongResponseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                //        responseMessage = strongResponseMessage;
                //        var articles = context.WeiXinArticles.OrderByDescending(m => m.ID).Where(a => a.MenuID == entity.ID).Select(a => new
                //            Article
                //        {
                //            Title = a.Title,
                //            Description = a.Description,
                //            PicUrl = "http://123.233.246.196" + a.PictureUrl,
                //            Url = "http://123.233.246.196/ShowArticle.aspx?id=" + a.ID
                //        }).Take(5).ToList<Article>();
                //        articles.ForEach(a =>
                //        {
                //            strongResponseMessage.Articles.Add(a);
                //        });
                //    }
                //}
            }


            return responseMessage;
        }
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您的OpenId是：" + requestMessage.FromUserName + "。\r\n您发送的文本是：" + requestMessage.Content;
            return responseMessage;
        }
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
            var locationService = new LocationService();
            var markersList = new List<BaiduMarkers>();
            markersList.Add(new BaiduMarkers
            {
                Color = "0xFFFF00",
                Label = "S",
                Latitude = requestMessage.Location_Y,
                Longitude = requestMessage.Location_X,
                Size = BaiduMarkerSize.Default

            });
            var mapUrl = BaiduMapHelper.GetBaiduStaticMap(requestMessage.Location_X, requestMessage.Location_Y, 16, 12, markersList);
            responseMessage.Articles.Add(new Article
            {
                Title = "使用百度地图定位地点周边地图",
                Description = String.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}", requestMessage.Location_X, requestMessage.Location_Y, requestMessage.Scale, requestMessage.Label),
                PicUrl = mapUrl,
                Url = mapUrl
            });
            return responseMessage;
        }
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件";
            return responseMessage;
        }
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件带等待";
            return responseMessage;
        }
        public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "系统拍照发图,发送的图片信息：" + requestMessage.SendPicsInfo;
            return responseMessage;
        }
        public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出微信相册发图器";
            return responseMessage;
        }
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = "http://weixin.senparc.com"
            });
            return responseMessage;
        }
        public override Senparc.Weixin.MP.Entities.IResponseMessageBase DefaultResponseMessage(Senparc.Weixin.MP.Entities.IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "欢迎关注酷伴成长通，我们将竭诚为您服务";
            return responseMessage;
        }
    }
}