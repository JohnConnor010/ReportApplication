using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public partial class ChooseNews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            int customerId = 1;
            int.TryParse(Request.QueryString["customerId"], out customerId);
            string reportType = Request.QueryString["reportType"];
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            if (reportType != "0")
            {
                switch (reportType)
                {
                    case "1": //日报
                        startDate = DateTime.Now;
                        break;
                    case "2": //周报
                        startDate = DateTime.Now.AddDays(-7);
                        break;
                    case "3": //旬报
                        startDate = DateTime.Now.AddDays(-10);
                        break;
                    case "4": //半月报
                        startDate = DateTime.Now.AddDays(-15);
                        break;
                    case "5": //月报
                        startDate = DateTime.Now.AddDays(-30);
                        break;
                    case "6": //季度报
                        startDate = DateTime.Now.AddDays(-90);
                        break;
                    case "7": //年报
                        startDate = DateTime.Now.AddYears(-1);
                        break;
                }
            }
            this.txtStartDate.Text = startDate.ToString("yyyy-MM-dd 00:00");
            this.txtEndDate.Text = endDate.ToString("yyyy-MM-dd 00:00");
            InitRepeater1(customerId, reportType, startDate, endDate, this.ddlFavorite.SelectedValue,this.AspNetPager1.CurrentPageIndex,int.Parse(this.ddlRating.SelectedValue));
        }
        public void ddlRating_GetData()
        {
            this.ddlRating.Items.Clear();
            this.ddlRating.Items.Add(new ListItem { Text = "选择星级", Value = "0" });
            Enumerable.Range(1, 10).ForEach(item =>
             {
                 ListItem listItem = new ListItem { Value = item.ToString(), Text = '★'.Repeat(item) };
                 this.ddlRating.Items.Add(listItem);
             });
        }

        private void InitRepeater1(int customerId,string reportType, DateTime startDate, DateTime endDate,string favorite = "", int currentPageIndex = 1,int rating = 0)
        {
            
            using (var context = new ReportContext())
            {
                var entities = context.Articles.Where(a => a.CustomerID == customerId && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate)).OrderByDescending(a => a.ID).ToList();
                if(favorite != "")
                {
                    entities = entities.Where(a => a.Favorite == favorite).ToList();
                }
                if(rating != 0)
                {
                    entities = entities.Where(a => a.Rating == rating).ToList();
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.Skip((this.AspNetPager1.CurrentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }
        public static string GetChannelName(string channelType)
        {
            switch (channelType)
            {
                case "tieba":
                    return "贴吧";
                case "weibo":
                    return "微博";
                case "weixin":
                    return "微信";
                case "bbs":
                    return "论坛";
                case "video":
                    return "视频";
                case "blog":
                    return "博客";
                default:
                    return "新闻";
            }
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            int customerId = 1;
            int.TryParse(Request.QueryString["customerId"], out customerId);
            string reportType = Request.QueryString["reportType"];
            DateTime startDate = Convert.ToDateTime(this.txtStartDate.Text);
            Debug.WriteLine(startDate);
            DateTime endDate = Convert.ToDateTime(this.txtEndDate.Text);
            InitRepeater1(customerId, reportType, startDate, endDate, this.ddlFavorite.SelectedValue, this.AspNetPager1.CurrentPageIndex, int.Parse(ddlRating.SelectedValue));
        }       
        
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            int customerId = 1;
            int.TryParse(Request.QueryString["customerId"], out customerId);
            string reportType = Request.QueryString["reportType"];
            DateTime startDate = Convert.ToDateTime(this.txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(this.txtEndDate.Text);
            InitRepeater1(customerId, reportType, startDate, endDate, this.ddlFavorite.SelectedValue, this.AspNetPager1.CurrentPageIndex, int.Parse(ddlRating.SelectedValue));
        }
    }
}