using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public partial class ManageNews : System.Web.UI.Page
    {
        public string CustomerID
        {
            get
            {
                string userName = CookieUtilities.GetCookieValue("username");
                return MemberUtilities.GetCustomerID(userName);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            
            InitRepeater(this.AspNetPager1.CurrentPageIndex, "0", this.ddlFavorite.SelectedValue, this.ddlChannel.SelectedValue, int.Parse(this.ddlRating.SelectedValue));
        }

        private void InitRepeater(int currentPageIndex = 1, string customerId = "0", string favorite = "",string channel = "",int rating = 0)
        {
            using(var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                var entities = context.Articles.Where(a => customerIds.Contains(a.CustomerID.Value)).ToList();
                if(customerId != "0")
                {
                    entities = entities.Where(a => a.CustomerID == int.Parse(customerId)).ToList();
                }
                if(favorite != "")
                {
                    entities = entities.Where(a => a.Favorite == favorite).ToList();
                }
                if(channel != "")
                {
                    entities = entities.Where(a => a.ChannelType == channel).ToList();
                }
                if(rating != 0)
                {
                    entities = entities.Where(a => a.Rating == rating).ToList();
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.OrderByDescending(a => a.ID).Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater(this.AspNetPager1.CurrentPageIndex,this.ddlCustomer.SelectedValue,this.ddlFavorite.SelectedValue,this.ddlChannel.SelectedValue,int.Parse(this.ddlRating.SelectedValue));
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            InitRepeater(this.AspNetPager1.CurrentPageIndex, this.ddlCustomer.SelectedValue, this.ddlFavorite.SelectedValue, this.ddlChannel.SelectedValue, int.Parse(this.ddlRating.SelectedValue));
        }
        public void Customer_GetData()
        {
            using(var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                var items = context.Customers.Where(c => customerIds.Contains(c.ID)).Select(c => new
                    {
                        value = c.ID.ToString(),
                        text = c.Name
                    });
                this.ddlCustomer.Items.Clear();
                this.ddlCustomer.Items.Add(new ListItem{Text = "选择客户",Value = "0"});
                items.ForEach(item =>
                    {
                        this.ddlCustomer.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
            }
        }
        public void Channel_GetData()
        {
            using(var context = new ReportContext())
            {
                var items = context.Channels.Select(c => new
                    {
                        value = c.ChannelValue,
                        text = c.ChannelText
                    });
                this.ddlChannel.Items.Clear();
                this.ddlChannel.Items.Add(new ListItem { Text = "选择渠道", Value = "" });
                items.ForEach(item =>
                    {
                        this.ddlChannel.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
            }
        }
        public void Rating_GetData()
        {
            this.ddlRating.Items.Clear();
            this.ddlRating.Items.Add(new ListItem{Text = "选择星级",Value = "0"});
            Enumerable.Range(1, 10).ForEach(item =>
                {
                    this.ddlRating.Items.Add(new ListItem { Text = "★".Repeat(item), Value = item.ToString() });
                });
        }
        public string GetCustomerNameById(int? customerId)
        {
            using(var context = new ReportContext())
            {
                var customerName = context.Customers.FirstOrDefault(c => c.ID == customerId).Name ?? "";
                return customerName;
            }
        }
        public string GetChannelName(string channelType)
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

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            using(var context = new ReportContext())
            {
                var entity = context.Articles.Find(id);
                if(entity != null)
                {
                    context.Articles.Remove(entity);
                    context.SaveChanges();
                    InitRepeater(this.AspNetPager1.CurrentPageIndex, this.ddlCustomer.SelectedValue, this.ddlFavorite.SelectedValue, this.ddlChannel.SelectedValue, int.Parse(this.ddlRating.SelectedValue));
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('文章删除成功')", true);
                }
            }
        }
    }
}