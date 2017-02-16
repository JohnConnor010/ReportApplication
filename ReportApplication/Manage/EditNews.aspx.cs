// ***********************************************************************
// Copyright (c) 2015 JohnConnor,All rights reserved.
// Project:
// Assembly:WXOpinionApp.ReportManage
// Author:JohnConnor
// Created:4/28/2015 4:16:03 PM
// Description:
// ***********************************************************************
// Last Modified By:JOHNCONNOR-PC
// Last Modified On:4/28/2015 4:16:03 PM
// ***********************************************************************
using iNethinkCMS.Command;
using ReportApplication;
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WXOpinionApp.ReportManage
{
    public partial class EditNews : System.Web.UI.Page
    {
        public int NewId
        {
            get
            {
                return int.Parse(Request.QueryString["id"]);
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
            using(var context = new ReportContext())
            {
                var entity = context.Articles.Find(NewId);
                if(entity != null)
                {
                    this.ddlCustomer.SelectedValue = entity.CustomerID.ToString();
                    this.txtTitle.Text = entity.Title;
                    this.txtUrl.Text = entity.Url;
                    this.ddlCategory.SelectedValue = entity.CategoryID.ToString();

                    var mainCategoryItems = CommonUtility.GetMainCategoryById(entity.CategoryID.Value);
                    this.ddlMainCategory.Items.Clear();                    
                    this.ddlMainCategory.DataValueField = "Value";
                    this.ddlMainCategory.DataTextField = "Text";
                    this.ddlMainCategory.DataSource = mainCategoryItems;
                    this.ddlMainCategory.DataBind();
                    this.ddlMainCategory.Items.Insert(0, new ListItem { Text = "选择大类", Value = "0" });
                    this.ddlMainCategory.SelectedValue = entity.MainCategoryID.ToString();

                    var mediumCategoryItems = CommonUtility.GetMediumCategoryById(entity.MainCategoryID.Value);
                    this.ddlMediumCategory.Items.Clear();
                    this.ddlMediumCategory.DataValueField = "Value";
                    this.ddlMediumCategory.DataTextField = "Text";
                    this.ddlMediumCategory.DataSource = mediumCategoryItems;
                    this.ddlMediumCategory.DataBind();
                    this.ddlMediumCategory.Items.Insert(0, new ListItem { Text = "选择中类", Value = "0" });
                    this.ddlMediumCategory.SelectedValue = entity.MediumCategoryID.ToString();

                    var smallCategoryItems = CommonUtility.GetSmallCategoryById(entity.MediumCategoryID.Value);
                    this.ddlSmallCategory.Items.Clear();
                    this.ddlSmallCategory.DataValueField = "Value";
                    this.ddlSmallCategory.DataTextField = "Text";
                    this.ddlSmallCategory.DataSource = smallCategoryItems;
                    this.ddlSmallCategory.DataBind();
                    this.ddlSmallCategory.Items.Insert(0, new ListItem { Text = "选择小类", Value = "0" });
                    this.ddlSmallCategory.SelectedValue = entity.SmallCategoryID.ToString();

                    //this.txtClickCount.Text = entity.ClickCount.Value.ToString();
                    this.txtReplyCount.Text = entity.ReplyCount.Value.ToString();
                    this.ddlRelateQuestion.SelectedValue = entity.RelateQuestion;
                    this.ddlFavorite.SelectedValue = entity.Favorite;
                    this.txtWebSite.Text = entity.Site;
                    this.ddlChannelValue.SelectedValue = entity.ChannelType;
                    this.txtContent.Text = entity.Content;
                    this.txtScreenshotsPath.Text = entity.ScreenshotsPath;
                    this.txtReplyCount.Text = entity.ReplyCount.Value.ToString();
                    this.input_star.Value = entity.Rating.Value.ToString();
                    this.hideRatingNumber.Value = entity.Rating.Value.ToString();
                    this.txtJudgeContent.Text = entity.JudgeContent;
                    this.txtSuggestContent.Text = entity.SuggestContent;
                }
            }
        }
        public void InitCategoryDropDownList()
        {

            var entities = CommonUtility.GetAllCategory();
            this.ddlCategory.Items.Clear();
            this.ddlCategory.Items.Add(new ListItem { Text = "选择门类", Value = "0" });
            entities.ForEach(item =>
            {
                this.ddlCategory.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
            });
        }
        public void ChannelValue_GetData()
        {
            using (var context = new ReportContext())
            {
                var items = context.Channels.Select(c => new
                {
                    value = c.ChannelValue,
                    text = c.ChannelText
                });
                items.ForEach(item =>
                {
                    ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                    this.ddlChannelValue.Items.Add(listItem);
                });
            }
        }
        public void Customer_GetData()
        {
            using(var context = new ReportContext())
            {
                var customers = context.Customers.Select(c => new
                    {
                        text = c.Name,
                        value = c.ID.ToString()
                    });
                this.ddlCustomer.Items.Add(new ListItem { Text = "选择客户", Value = "0" });
                customers.ForEach(item =>
                    {
                        ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                        this.ddlCustomer.Items.Add(listItem);
                    });
            }
        }
        public void ddlRelateQuestion_GetData()
        {
            var context = new ReportContext();
            var items = context.QuestionCategories.Select(q => new
            {
                text = q.Name,
                value = q.Name
            }).ToList();
            this.ddlRelateQuestion.Items.Clear();
            this.ddlRelateQuestion.Items.Add(new ListItem { Text = "选择涉及问题", Value = "" });
            items.ForEach(item =>
            {
                this.ddlRelateQuestion.Items.Add(new ListItem { Text = item.text, Value = item.value });
            });
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = this.ddlCategory.SelectedValue;
            if (selectedId != "0")
            {
                int categoryId = int.Parse(selectedId);
                var entities = CommonUtility.GetMainCategoryById(categoryId);
                this.ddlMainCategory.Items.Clear();
                this.ddlMainCategory.Items.Add(new ListItem { Text = "选择大类", Value = "0" });
                entities.ForEach(item =>
                {
                    this.ddlMainCategory.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
                });
                this.ddlMediumCategory.Items.Clear();
                this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
            else
            {
                this.ddlMainCategory.Items.Clear();
                this.ddlMainCategory.Items.Add(new ListItem { Text = "选择大类", Value = "0" });
                this.ddlMediumCategory.Items.Clear();
                this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
        }

        protected void ddlMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = this.ddlMainCategory.SelectedValue;
            if (selectedId != "0")
            {
                int mainCategoryId = int.Parse(selectedId);
                var entities = CommonUtility.GetMediumCategoryById(mainCategoryId);
                this.ddlMediumCategory.Items.Clear();
                this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                entities.ForEach(item =>
                {
                    this.ddlMediumCategory.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
                });
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
            else
            {
                this.ddlMediumCategory.Items.Clear();
                this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
        }

        protected void ddlMediumCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = this.ddlMediumCategory.SelectedValue;
            if (selectedId != "0")
            {
                int mediumCategoryId = int.Parse(selectedId);
                var entities = CommonUtility.GetSmallCategoryById(mediumCategoryId);
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
                entities.ForEach(item =>
                {
                    this.ddlSmallCategory.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
                });
            }
            else
            {
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string customerId = this.ddlCustomer.SelectedValue;
            string title = this.txtTitle.Text.Trim();
            string categoryId = this.ddlCategory.SelectedValue;
            string mainCategoryId = this.ddlMainCategory.SelectedValue;
            string mediumCategoryId = this.ddlMediumCategory.SelectedValue;
            string smallCategoryId = this.ddlSmallCategory.SelectedValue;
            //int clickCount = 0;
            //int.TryParse(this.txtClickCount.Text, out clickCount);
            int replyCount = 0;
            int.TryParse(this.txtReplyCount.Text, out replyCount);
            string favorite = this.ddlFavorite.SelectedValue;
            string content = this.txtContent.Text;
            string screenshotsPath = this.txtScreenshotsPath.Text;
            string relateQuestion = this.ddlRelateQuestion.SelectedValue;
            string savePath = string.Empty;
            if (!string.IsNullOrEmpty(screenshotsPath))
            {                
                string fileExt = Path.GetExtension(screenshotsPath);
                string fileName = Path.GetFileNameWithoutExtension(screenshotsPath) + "_1" + fileExt;
                string dirPath = Path.GetDirectoryName(screenshotsPath);
                Path.GetPathRoot(screenshotsPath);
                savePath = Path.Combine(dirPath, fileName);
                CommonUtility.SetPictureBorder(Server.MapPath(screenshotsPath), Server.MapPath(savePath));
            }
            

            int rating = 0;
            int.TryParse(this.hideRatingNumber.Value, out rating);
            string judgeContent = this.txtJudgeContent.Text;
            string suggestContent = this.txtSuggestContent.Text;
            string url = this.txtUrl.Text;
            string channelType = this.ddlChannelValue.SelectedValue;
            string website = this.txtWebSite.Text;
            using (var context = new ReportContext())
            {
                var entity = context.Articles.Find(NewId);
                if(entity != null)
                {
                    entity.CustomerID = int.Parse(customerId);
                    entity.Title = title;
                    entity.CategoryID = int.Parse(categoryId);
                    entity.MainCategoryID = int.Parse(mainCategoryId);
                    entity.MediumCategoryID = int.Parse(mediumCategoryId);
                    entity.SmallCategoryID = int.Parse(smallCategoryId);
                    entity.ClickCount = 0;
                    entity.ReplyCount = replyCount;
                    entity.Url = url;
                    entity.Favorite = favorite;
                    entity.Site = website;
                    entity.ChannelType = channelType;
                    entity.Content = content;
                    entity.ScreenshotsPath = savePath;
                    entity.Rating = rating;
                    entity.JudgeContent = judgeContent;
                    entity.SuggestContent = suggestContent;
                    entity.RelateQuestion = relateQuestion;
                }
                context.SaveChanges();
                MessageBox.ShowAndRedirect(this, "文章修改成功", "ManageNews.aspx");
            }
        }
    }
}