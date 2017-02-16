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
using Microsoft.AspNet.Identity.Owin;
using ReportApplication;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WXOpinionApp.ReportManage
{
    public partial class AddNew : System.Web.UI.Page
    {
        public int NewId
        {
            get
            {
                return 1;
            }
        }
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
            this.txtContent.Text = GetChannelContent();
            InitRepeater1();
        }

        private void InitRepeater1(int currentPageIndex = 1)
        {
            using (var context = new ReportContext())
            {
                var entities = context.QuestionCategories.OrderByDescending(q => q.ID).Select(q => q).ToList();
                this.AspNetPager1.RecordCount = entities.Count;
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                
                this.Repeater1.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
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
                this.ddlChannelValue.Items.Clear();
                items.ForEach(item =>
                {
                    ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                    this.ddlChannelValue.Items.Add(listItem);
                });
            }
        }
        public void Customer_GetData()
        {
            int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
            using(var context = new ReportContext())
            {
                var customers = context.Customers.Where(c => customerIds.Contains(c.ID)).Select(c => new
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
            int replyCount = 0;
            int.TryParse(this.txtReplyCount.Text, out replyCount);
            string relateQuestion = this.ddlRelateQuestion.SelectedValue;
            string favorite = this.ddlFavorite.SelectedValue;
            string content = this.txtContent.Text;
            string screenshotsPath = this.txtScreenshotsPath.Text;

            string savePath = string.Empty;
            if(!string.IsNullOrEmpty(screenshotsPath))
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
                var entity = context.Articles.FirstOrDefault(a => a.Title == title);
                if (entity == null)
                {
                    entity = new Article
                    {
                        CustomerID = int.Parse(customerId),
                        Title = title,
                        CategoryID = int.Parse(categoryId),
                        MainCategoryID = int.Parse(mainCategoryId),
                        MediumCategoryID = int.Parse(mediumCategoryId),
                        SmallCategoryID = int.Parse(smallCategoryId),
                        ClickCount = 0,
                        ReplyCount = replyCount,
                        Url = url,
                        Favorite = favorite,
                        Site = website,
                        ChannelType = channelType,
                        Content = content,
                        ScreenshotsPath = savePath,
                        Rating = rating,
                        JudgeContent = judgeContent,
                        SuggestContent = suggestContent,
                        AddDate = DateTime.Now,
                        RelateQuestion = relateQuestion
                    };
                    context.Articles.Add(entity);
                    context.SaveChanges();
                    MessageBox.ShowAndRedirect(this, "文章添加成功", "ManageNews.aspx");
                }
                else
                {
                    MessageBox.Show(this, "此舆情已经添加，不能请勿重复添加！");
                }

            }
        }

        protected void ddlChannelValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = this.ddlChannelValue.SelectedValue;
            string content = GetChannelContent(selectedValue);
            this.txtContent.Text = content;
        }
        public string GetChannelContent(string channelValue = "news")
        {
            string result = string.Empty;
            using (var context = new ReportContext())
            {
                var entity = context.ChannelContents.FirstOrDefault(c => c.ChannelValue == channelValue);
                if(entity != null)
                {
                    result = entity.Content;
                }
            }
            return result;
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            string questionName = this.txtQuestionName.Text.Trim();
            string questionSummary = this.txtQuestionSummary.Text.Trim();
            using (var context = new ReportContext())
            {
                if(this.hideAction.Value == "add")
                {
                    var entity = new QuestionCategory
                    {
                        Name = questionName,
                        Summary = questionSummary
                    };
                    context.QuestionCategories.Add(entity);
                }
                if(this.hideAction.Value == "edit")
                {
                    string id = this.hideQuestionID.Value;
                    var entity = context.QuestionCategories.Find(int.Parse(id));
                    if(entity != null)
                    {
                        entity.Name = questionName;
                        entity.Summary = questionSummary;
                    }
                }
                await context.SaveChangesAsync();
                InitRepeater1(this.AspNetPager1.CurrentPageIndex);
                this.ddlRelateQuestion.DataBind();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex);
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.QuestionCategories.Find(int.Parse(id));
                if(entity != null)
                {
                    this.txtQuestionName.Text = entity.Name;
                    this.txtQuestionSummary.Text = entity.Summary;
                    this.hideAction.Value = "edit";
                    this.hideQuestionID.Value = id;
                }
            }
        }

        protected async void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.QuestionCategories.Find(int.Parse(id));
                if (entity != null)
                {
                    context.QuestionCategories.Remove(entity);
                    this.hideAction.Value = "add";
                    this.hideQuestionID.Value = "0";
                    await context.SaveChangesAsync();
                    InitRepeater1(this.AspNetPager1.CurrentPageIndex);
                    this.ddlRelateQuestion.DataBind();
                }
            }
        }
    }
}