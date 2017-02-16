// ***********************************************************************
// Copyright (c) 2015 JohnConnor,All rights reserved.
// Project:
// Assembly:WXOpinionApp.ReportManage
// Author:JohnConnor
// Created:4/28/2015 5:20:37 PM
// Description:
// ***********************************************************************
// Last Modified By:JOHNCONNOR-PC
// Last Modified On:4/28/2015 5:20:37 PM
// ***********************************************************************
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication.Manage
{
    public partial class ChooseAnalysisAndSuggest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        public int Stars
        {
            get
            {
                return Request.QueryString["stars"].ToInt32();
            }
        }
        private void InitComponent()
        {
            this.ddlStars.SelectedValue = Stars.ToString();
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, Stars.ToString(), this.txtKeyword.Text.Trim());
            InitRepeater2(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, Stars.ToString(), this.txtKeyword.Text.Trim());
        }

        private void InitRepeater2(int currentPageIndex = 1, string categoryId = "0", string mainCategoryId = "0", string mediumCategoryId = "0", string smallCategoryId = "0", string stars = "0", string keyword = "")
        {
            using (var context = new DataSettingContext())
            {
                var entities = context.Suggestions.Select(s => s).ToList();
                if (categoryId != "0")
                {
                    entities = entities.Where(a => a.CategoryID == int.Parse(categoryId)).ToList();
                }
                if (mainCategoryId != "0")
                {
                    entities = entities.Where(a => a.MainCategoryID == int.Parse(mainCategoryId)).ToList();
                }
                if (mediumCategoryId != "0")
                {
                    entities = entities.Where(a => a.MediumCategoryID == int.Parse(mediumCategoryId)).ToList();
                }
                if (smallCategoryId != "0")
                {
                    entities = entities.Where(a => a.SmallCategoryID == int.Parse(smallCategoryId)).ToList();
                }
                if (stars != "0")
                {
                    entities = entities.Where(a => a.Star == int.Parse(stars)).ToList();
                }
                if (keyword != "")
                {
                    entities = entities.Where(a => a.Keyword.Contains(keyword)).ToList();
                }
                this.AspNetPager2.RecordCount = entities.Count();
                this.AspNetPager2.PageSize = this.AspNetPager1.PageSize;
                this.AspNetPager2.CurrentPageIndex = currentPageIndex;
                this.Repeater2.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize).ToList();
                this.Repeater2.DataBind();
            }
        }

        private void InitRepeater1(int currentPageIndex = 1,string categoryId = "0",string mainCategoryId = "0",string mediumCategoryId = "0",string smallCategoryId = "0",string stars = "0",string keyword = "")
        {
            using (var context = new DataSettingContext())
            {
                var entities = context.Judges.Select(j => j).ToList();
                if(categoryId != "0")
                {
                    entities = entities.Where(a => a.CategoryID == int.Parse(categoryId)).ToList();
                }
                if(mainCategoryId != "0")
                {
                    entities = entities.Where(a => a.MainCategoryID == int.Parse(mainCategoryId)).ToList();
                }
                if (mediumCategoryId != "0")
                {
                    entities = entities.Where(a => a.MediumCategoryID == int.Parse(mediumCategoryId)).ToList();
                }
                if (smallCategoryId != "0")
                {
                    entities = entities.Where(a => a.SmallCategoryID == int.Parse(smallCategoryId)).ToList();
                }
                if (stars != "0")
                {
                    entities = entities.Where(a => a.Star == int.Parse(stars)).ToList();
                }
                if(keyword != "")
                {
                    entities = entities.Where(a => a.Keyword.Contains(keyword)).ToList();
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.PageSize = this.AspNetPager1.PageSize;
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize).ToList();
                this.Repeater1.DataBind();
            }
        }

        public void InitStarsDropDownList()
        {
            char c = '★';
            Enumerable.Range(1, 10).ForEach(item =>
             {
                 ListItem listItem = new ListItem { Text = c.Repeat(item), Value = item.ToString() };
                 this.ddlStars.Items.Add(listItem);
             });
        }
        public void InitCategoryDropDownList()
        {
            using (var context = new DataSettingContext())
            {
                var entities = context.Categories.Select(c => new
                {
                    text = c.CategoryName,
                    value = c.Id.ToString()
                });
                this.ddlCategory.Items.Clear();
                this.ddlCategory.Items.Add(new ListItem { Text = "选择门类", Value = "0" });
                entities.ForEach(item =>
                {
                    this.ddlCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, this.ddlStars.SelectedValue, this.txtKeyword.Text.Trim());
        }

        protected void AspNetPager2_PageChanged(object sender, EventArgs e)
        {
            InitRepeater2(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, this.ddlStars.SelectedValue, this.txtKeyword.Text.Trim());
        }
        public string Substring(string str,int length)
        {
            if(str.Length <= length)
            {
                return str;
            }
            else
            {
                return str.Substring(0, length) + "...";
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = this.ddlCategory.SelectedValue;
            if(selectedId != "0")
            {
                int categoryId = int.Parse(selectedId);
                using (var context = new DataSettingContext())
                {
                    var entities = context.MainCategories.Where(c => c.CategoryID == categoryId).Select(c => new
                    {
                        text = c.CategoryName,
                        value = c.Id.ToString()
                    });
                    this.ddlMainCategory.Items.Clear();
                    this.ddlMainCategory.Items.Add(new ListItem { Text = "选择大类", Value = "0"});
                    entities.ForEach(item =>
                    {
                        this.ddlMainCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
                    this.ddlMediumCategory.Items.Clear();
                    this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                    this.ddlSmallCategory.Items.Clear();
                    this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
                }
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
            if(selectedId != "0")
            {
                int mainCategoryId = int.Parse(selectedId);
                using (var context = new DataSettingContext())
                {
                    var entities = context.MediumCategories.Where(c => c.MainCategoryID == mainCategoryId).Select(c => new
                    {
                        text = c.CategoryName,
                        value = c.Id.ToString()
                    });
                    this.ddlMediumCategory.Items.Clear();
                    this.ddlMediumCategory.Items.Add(new ListItem { Text = "选择中类", Value = "0" });
                    entities.ForEach(item =>
                    {
                        this.ddlMediumCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
                    this.ddlSmallCategory.Items.Clear();
                    this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
                }
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
            if(selectedId != "0")
            {
                int mediumCategoryId = int.Parse(selectedId);
                using (var context = new DataSettingContext())
                {
                    var entities = context.SmallCategories.Where(s => s.MediumCategoryID == mediumCategoryId).Select(c => new
                    {
                        text = c.CategoryName,
                        value = c.Id.ToString()
                    });
                    this.ddlSmallCategory.Items.Clear();
                    this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
                    entities.ForEach(item =>
                    {
                        this.ddlSmallCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
                }
            }
            else
            {
                this.ddlSmallCategory.Items.Clear();
                this.ddlSmallCategory.Items.Add(new ListItem { Text = "选择小类", Value = "0" });
            }
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, this.ddlStars.SelectedValue, this.txtKeyword.Text.Trim());
            InitRepeater2(this.AspNetPager1.CurrentPageIndex, this.ddlCategory.SelectedValue, this.ddlMainCategory.SelectedValue,
                this.ddlMediumCategory.SelectedValue, this.ddlSmallCategory.SelectedValue, this.ddlStars.SelectedValue, this.txtKeyword.Text.Trim());
        }
    }
}