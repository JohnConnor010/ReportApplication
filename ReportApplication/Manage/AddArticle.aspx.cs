using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{

    public partial class AddArticle : System.Web.UI.Page
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
            
            BindPaperCategory();
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue),int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
            InitRepeater2();
        }
        #region SelectMethod
        public void ddlCustomerCategory_GetData()
        {
            using (var context = new ReportContext())
            {
                var items = context.CustomerCategories.Select(c => new
                {
                    text = c.Name,
                    value = c.ID.ToString()
                });
                this.ddlCustomerCategory.Items.Clear();
                this.ddlCustomerCategory.Items.Add(new ListItem { Text = "选择分类", Value = "0" });
                items.ForEach(item =>
                {
                    this.ddlCustomerCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlCustomer_GetData(int categoryId)
        {
            using (var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                this.ddlCustomer.Items.Clear();
                this.ddlCustomer.Items.Add(new ListItem { Text = "选择客户", Value = "0" });
                var items = context.Customers.Where(c => customerIds.Contains(c.ID)).Where(c => c.CategoryID == categoryId).Select(c => new
                {
                    text = c.Name,
                    value = c.ID.ToString()
                });
                items.ForEach(item =>
                    {
                        this.ddlCustomer.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
            }
        }
        #endregion
        private void InitRepeater1(int currentPageIndex = 1, int customerId = 0,int customerCategoryId = 0,int paperCategoryId = 0)
        {
            using (var context = new PaperContext())
            {
                var papers = context.Papers.OrderByDescending(a => a.PaperID).ToList();
                if(customerCategoryId != 0)
                {
                    papers = papers.Where(p => p.CustomerCategoryID == customerCategoryId).ToList();
                }
                if (customerId != 0)
                {
                    papers = papers.Where(p => p.CustomerID == customerId).ToList();
                }
                if(paperCategoryId != 0)
                {
                    papers = papers.Where(p => p.CategoryID == paperCategoryId).ToList();
                }
                this.AspNetPager1.RecordCount = papers.Count;
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = papers.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }
        private void InitRepeater2(int currentPageIndex = 1)
        {
            using (var context = new PaperContext())
            {
                var categories = context.PaperCategories.Select(c => c).OrderByDescending(p => p.CategoryID).ToList();
                this.AspNetPager2.RecordCount = categories.Count;
                this.Repeater2.DataSource = categories.Skip((currentPageIndex - 1) * this.AspNetPager2.PageSize).Take(this.AspNetPager2.PageSize);
                this.Repeater2.DataBind();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string customerCategoryId = this.ddlCustomerCategory.SelectedValue;
            string customerId = this.ddlCustomer.SelectedValue;
            string categoryId = this.ddlPaperCategory.SelectedValue;
            string title = this.txtTitle.Text.Trim();
            string publishedDate = this.txtPublishedDate.Text.Trim();
            string reprintCount = this.txtReprintCount.Text.Trim();
            string firstSite = this.txtFirstSite.Text.Trim();
            string reprintSite = this.txtReprintSite.Text.Trim();
            string url = this.txtUrl.Text.Trim();
            string summary = this.txtSummary.Text.Trim();
            using(var context = new PaperContext())
            {
                if(this.hidePaperID.Value == "0")
                {
                    var entity = new Paper
                    {
                        CustomerID = int.Parse(customerId),
                        CategoryID = int.Parse(categoryId),
                        CustomerCategoryID = int.Parse(customerCategoryId),
                        Title = title,
                        PaperPublishedDate = publishedDate,
                        ReprintCount = int.Parse(reprintCount),
                        FirstSite = firstSite,
                        ReprintSite = reprintSite,
                        Url = url,
                        Summary = summary,
                        AddDate = DateTime.Now
                    };
                    context.Papers.Add(entity);                    
                }
                else if(this.hidePaperID.Value != "0")
                {
                    int paperId = int.Parse(this.hidePaperID.Value);
                    var entity = context.Papers.Find(paperId);
                    entity.CustomerID = int.Parse(customerId);
                    entity.CategoryID = int.Parse(categoryId);
                    entity.CustomerCategoryID = int.Parse(customerCategoryId);
                    entity.Title = title;
                    entity.PaperPublishedDate = publishedDate;
                    entity.ReprintCount = int.Parse(reprintCount);
                    entity.FirstSite = firstSite;
                    entity.ReprintSite = reprintSite;
                    entity.Url = url;
                    entity.Summary = summary;
                    entity.AddDate = DateTime.Now;
                    this.ddlCustomer.SelectedIndex = -1;
                }
                context.SaveChanges();
                this.txtTitle.Text = string.Empty;
                this.txtPublishedDate.Text = string.Empty;
                this.txtReprintCount.Text = "0";
                this.txtFirstSite.Text = string.Empty;
                this.txtReprintSite.Text = string.Empty;
                this.txtUrl.Text = string.Empty;
                this.txtSummary.Text = string.Empty;
                this.hidePaperID.Value = "0";
                InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue),int.Parse(this.ddlPaperCategory.SelectedValue));
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            int paperId = 0;
            int.TryParse(e.CommandArgument.ToString(), out paperId);
            using(var context = new PaperContext())
            {
                var entity = context.Papers.Find(paperId);
                if(entity != null)
                {
                    this.ddlCustomerCategory.SelectedValue = entity.CustomerCategoryID.ToString();
                    if(entity.CustomerCategoryID != 0)
                    {
                        using (var db = new ReportContext())
                        {
                            var items = db.Customers.Where(c => c.CategoryID == entity.CustomerCategoryID).Select(c => new
                            {
                                text = c.Name,
                                value = c.ID.ToString()
                            });
                            this.ddlCustomer.Items.Clear();
                            this.ddlCustomer.Items.Add(new ListItem { Text = "选择分类", Value = "0" });
                            items.ForEach(item =>
                            {
                                this.ddlCustomer.Items.Add(new ListItem { Text = item.text, Value = item.value });
                            });
                        }
                    }
                    this.ddlCustomer.SelectedValue = entity.CustomerID.ToString();
                    this.ddlPaperCategory.SelectedValue = entity.CategoryID.ToString();
                    this.txtTitle.Text = entity.Title;
                    this.txtPublishedDate.Text = entity.PaperPublishedDate;
                    this.txtReprintCount.Text = entity.ReprintCount.ToString();
                    this.txtFirstSite.Text = entity.FirstSite;
                    this.txtReprintSite.Text = entity.ReprintSite;
                    this.txtUrl.Text = entity.Url;
                    this.txtSummary.Text = entity.Summary;
                    this.hidePaperID.Value = entity.PaperID.ToString();
                }
            }
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int paperId = 0;
            int.TryParse(e.CommandArgument.ToString(), out paperId);
            using (var context = new PaperContext())
            {
                var entity = context.Papers.Find(paperId);
                if (entity != null)
                {
                    context.Papers.Remove(entity);
                    try
                    {
                        context.SaveChanges();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "alert", "alert('文章删除成功')", true);
                        InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "alert", "alert('文章删除失败')", true);
                    }
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
        }

        protected void btnSavePaperCategory_Click(object sender, EventArgs e)
        {
            string categoryName = this.txtPaperCategoryName.Text.Trim();
            string categorySummary = this.txtPaperCategorySummary.Text.Trim();
            string customerCategoryId = this.ddlCustomerCategoryID.SelectedValue;
            using (var context = new PaperContext())
            {
                if (this.hidePaperCategoryAction.Value == "add")
                {
                    var entity = new PaperCategory
                    {
                        CategoryName = categoryName,
                        CategorySummary = categorySummary,
                        CustomerCategoryID = int.Parse(customerCategoryId)
                    };
                    context.PaperCategories.Add(entity);
                    context.SaveChanges();
                    this.hidePaperCategoryAction.Value = "add";
                }
                else
                {
                    var id = this.hidePaperCategoryID.Value;
                    int categoryId = int.Parse(id);
                    var entity = context.PaperCategories.Find(categoryId);
                    if (entity != null)
                    {
                        entity.CategoryName = categoryName;
                        entity.CategorySummary = categorySummary;
                        entity.CustomerCategoryID = int.Parse(customerCategoryId);
                        context.SaveChanges();
                        this.hidePaperCategoryID.Value = "0";
                        this.hidePaperCategoryAction.Value = "add";
                    }
                }
                InitRepeater2();
                this.txtPaperCategoryName.Text = string.Empty;
                this.txtPaperCategorySummary.Text = string.Empty;
                BindPaperCategory();


            }
        }
        public void BindPaperCategory(int customerId = 0)
        {
            using (var context = new PaperContext())
            {
                var entities = context.PaperCategories.ToList();
                if (customerId != 0)
                {
                    entities = entities.Where(a => a.CustomerCategoryID == 0 || a.CustomerCategoryID.Value == customerId).ToList();
                }
                var items = entities.Select(a => new
                {
                    text = a.CategoryName,
                    value = a.CategoryID.ToString()
                });
                this.ddlPaperCategory.Items.Clear();
                this.ddlPaperCategory.Items.Add(new ListItem { Text = "选择类型", Value = "0" });
                items.ForEach(item =>
                {
                    this.ddlPaperCategory.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }

        protected void lnkPaperCategoryEdit_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            using (var context = new PaperContext())
            {
                int categoryId = int.Parse(id);
                var entity = context.PaperCategories.Find(categoryId);
                if (entity != null)
                {
                    this.txtPaperCategoryName.Text = entity.CategoryName;
                    this.ddlCustomerCategoryID.SelectedValue = entity.CustomerCategoryID == null ? "0" : entity.CustomerCategoryID.Value.ToString();
                    this.txtPaperCategorySummary.Text = entity.CategorySummary;
                    this.hidePaperCategoryID.Value = entity.CategoryID.ToString();
                    this.hidePaperCategoryAction.Value = "edit";
                }
            }
        }

        protected void lnkPaperCategoryDelete_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            using (var context = new PaperContext())
            {
                int categoryId = int.Parse(id);
                var entity = context.PaperCategories.Find(categoryId);
                if (entity != null)
                {
                    var entities = context.Papers.Where(p => p.CategoryID == categoryId);
                    context.Papers.RemoveRange(entities);
                    context.PaperCategories.Remove(entity);
                    context.SaveChanges();
                    this.hidePaperCategoryID.Value = "0";
                    this.hidePaperCategoryAction.Value = "add";
                    InitRepeater2();
                    this.txtPaperCategoryName.Text = string.Empty;
                    this.txtPaperCategorySummary.Text = string.Empty;
                    BindPaperCategory();
                    InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
                }
            }
        }

        protected void AspNetPager2_PageChanged(object sender, EventArgs e)
        {
            InitRepeater2(this.AspNetPager2.CurrentPageIndex);
        }

        protected void ddlCustomerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCustomer_GetData(int.Parse(this.ddlCustomerCategory.SelectedValue));
            //BindPaperCategory(int.Parse(this.ddlCustomerCategory.SelectedValue));
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue),int.Parse(this.ddlPaperCategory.SelectedValue));
        }

        protected void ddlPaperCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), int.Parse(this.ddlCustomerCategory.SelectedValue), int.Parse(this.ddlPaperCategory.SelectedValue));
        }
        public IQueryable ddlCustomerCategoryID_GetData()
        {
            var context = new ReportContext();            
            var items = context.CustomerCategories.Select(c => new
            {
                text = c.Name,
                value = c.ID.ToString()
            });
            return items;
        }
    }
}