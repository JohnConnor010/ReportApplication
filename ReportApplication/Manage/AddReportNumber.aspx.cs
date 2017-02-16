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
    public partial class AddReportNumber : System.Web.UI.Page
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
            InitRepeater1();
        }

        private void InitRepeater1(int customerId = 0,int currentPageIndex = 1)
        {
            using(var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                var entities = context.ReportNumbers.Where(r => customerIds.Contains(r.CustomerID.Value)).Select(r => new
                    {
                        r.ID,
                        CustomerName = context.Customers.FirstOrDefault(c => c.ID == r.CustomerID).Name,
                        r.ReportTypeId,
                        r.CustomerID,
                        r.CurrentNumber,
                        r.TotalNumber
                    }).ToList();
                if(customerId != 0)
                {
                    
                    entities = entities.Where(r => r.CustomerID == customerId).ToList();
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.OrderByDescending(a => a.ID).Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }
        public void Customer_GetData()
        {
            using (var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                var customers = context.Customers.Where(c => customerIds.Contains(c.ID)).Select(c => new
                {
                    text = c.Name,
                    value = c.ID.ToString()
                });
                this.ddlCustomer.Items.Clear();
                this.ddlCustomer.Items.Add(new ListItem { Text = "选择客户", Value = "0" });
                customers.ForEach(item =>
                {
                    ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                    this.ddlCustomer.Items.Add(listItem);
                });
            }
        }
        public void ReportType_GetData()
        {
            using(var context = new DataSettingContext())
            {                
                var items = context.DataServiceCategories.Where(c => c.CategoryName == "常规服务类型").Select(c => new
                    {
                        text = c.CategoryItem,
                        value = c.ID.ToString(),
                    });
                this.ddlReportType.Items.Clear();
                this.ddlReportType.Items.Add(new ListItem { Text = "选择类型", Value = "0" });
                items.ForEach(item =>
                    {
                        ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                        this.ddlReportType.Items.Add(listItem);
                    });
            }
        }
        public string GetReportTypeName(int reportTypeId)
        {
            using(var context = new DataSettingContext())
            {
                var entity = context.DataServiceCategories.FirstOrDefault(c => c.ID == reportTypeId);
                if(entity != null)
                {
                    return entity.CategoryItem;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(int.Parse(ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string customerId = this.ddlCustomer.SelectedValue;
            string reportTypeId = this.ddlReportType.SelectedValue;
            string currentNumber = this.txtCurrentNumber.Text;
            string totalNumber = this.txtTotalNumber.Text;
            if(this.hideID.Value == "0")
            {
                using(var context = new ReportContext())
                {
                    int custId = int.Parse(customerId);
                    int repId = int.Parse(reportTypeId);
                    var entity = context.ReportNumbers.FirstOrDefault(r => r.CustomerID == custId && r.ReportTypeId == repId);
                    if(entity == null)
                    {
                        entity = new ReportNumber
                        {
                            CustomerID = int.Parse(customerId),
                            CurrentNumber = int.Parse(currentNumber),
                            TotalNumber = int.Parse(totalNumber),
                            ReportTypeId = int.Parse(reportTypeId)
                        };
                        context.ReportNumbers.Add(entity);
                        try
                        {
                            context.SaveChanges();
                            InitRepeater1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数添加成功')", true);
                        }
                        catch (Exception ex)
                        {

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数添加失败,原因：'" + ex.Message + ")", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('此客户的专报期数已经添加')", true);
                    }
                    
                }
            }
            else
            {
                var id = this.hideID.Value;
                using (var context = new ReportContext())
                {
                    var entity = context.ReportNumbers.Find(int.Parse(id));
                    if (entity != null)
                    {
                        entity.CustomerID = int.Parse(customerId);
                        entity.CurrentNumber = int.Parse(currentNumber);
                        entity.TotalNumber = int.Parse(totalNumber);
                        entity.ReportTypeId = int.Parse(reportTypeId);
                        try
                        {
                            context.SaveChanges();                            
                            this.ddlReportType.SelectedIndex = -1;
                            this.txtCurrentNumber.Text = string.Empty;
                            this.txtTotalNumber.Text = string.Empty;
                            this.hideID.Value = "0";
                            InitRepeater1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数修改成功')", true);
                        }
                        catch (Exception ex)
                        {

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数修改失败,原因：'" + ex.Message + ")", true);
                        }
                    }
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitRepeater1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.ReportNumbers.Find(int.Parse(id));
                if (entity != null)
                {
                    this.ddlCustomer.SelectedValue = entity.CustomerID.ToString();
                    this.ddlReportType.SelectedValue = entity.ReportTypeId.ToString();
                    this.txtCurrentNumber.Text = entity.CurrentNumber.ToString();
                    this.txtTotalNumber.Text = entity.TotalNumber.ToString();
                    this.hideID.Value = id;
                }
            }
        }

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {            
            var id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.ReportNumbers.Find(int.Parse(id));
                if (entity != null)
                {
                    context.ReportNumbers.Remove(entity);
                    try
                    {
                        context.SaveChanges();
                        this.ddlReportType.SelectedIndex = -1;
                        this.txtCurrentNumber.Text = string.Empty;
                        this.txtTotalNumber.Text = string.Empty;
                        this.hideID.Value = "0";
                        InitRepeater1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数删除成功')", true);
                    }
                    catch (Exception ex)
                    {

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('期数删除失败,原因：'" + ex.Message + ")", true);
                    }
                }
            }
        }
    }
}