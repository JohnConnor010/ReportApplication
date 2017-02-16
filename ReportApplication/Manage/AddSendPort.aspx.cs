// ***********************************************************************
// Copyright (c) 2015 JohnConnor,All rights reserved.
// Project:
// Assembly:WXOpinionApp.ReportManage
// Author:JohnConnor
// Created:4/21/2015 2:36:47 PM
// Description:
// ***********************************************************************
// Last Modified By:JOHNCONNOR-PC
// Last Modified On:4/21/2015 2:36:47 PM
// ***********************************************************************
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication.Manage
{
    public partial class AddSendPort : System.Web.UI.Page
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
                InitRepeater();
            }
        }

        private void InitRepeater(int currentPageIndex = 1,int customerId = 0)
        {
            using (var context = new ReportContext())
            {
                var entities = context.SendPorts.Select(a => a);
                if(customerId != 0)
                {
                    entities = entities.Where(s => s.CustomerID == customerId);
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.OrderByDescending(a => a.ID).Skip((this.AspNetPager1.CurrentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize).ToList();
                this.Repeater1.DataBind();
            }
        }

        public void Customer_GetData()
        {
            using (var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(a => int.Parse(a)).ToArray();
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string customerId = this.ddlCustomer.SelectedValue;
            string handphone = this.txtHandPhone.Text;
            string weixin = this.txtWeixin.Text;
            string qq = this.txtQQ.Text;
            string email = this.txtEmail.Text;
            if(this.hidePortID.Value == "0")
            {
                using (var context = new ReportContext())
                {
                    var entity = new SendPort
                    {
                        CustomerID = int.Parse(customerId),
                        HandPhone = handphone,
                        Weixin = weixin,
                        QQ = qq,
                        Email = email,
                        AddDate = DateTime.Now
                    };
                    context.SendPorts.Add(entity);
                    context.SaveChanges();
                    this.ddlCustomer.SelectedIndex = -1;
                    this.txtHandPhone.Text = string.Empty;
                    this.txtWeixin.Text = string.Empty;
                    this.txtQQ.Text = string.Empty;
                    this.txtEmail.Text = string.Empty;
                    InitRepeater(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue));
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('发送端口设置成功')", true);
                }
            }
            else
            {
                string id = this.hidePortID.Value;
                using (var context = new ReportContext())
                {
                    var entity = context.SendPorts.Find(int.Parse(id));
                    if (entity != null)
                    {
                        entity.CustomerID = int.Parse(customerId);
                        entity.HandPhone = handphone;
                        entity.Weixin = weixin;
                        entity.QQ = qq;
                        entity.Email = email;
                        entity.AddDate = DateTime.Now;
                        context.SaveChanges();
                        this.ddlCustomer.SelectedIndex = -1;
                        this.txtHandPhone.Text = string.Empty;
                        this.txtWeixin.Text = string.Empty;
                        this.txtQQ.Text = string.Empty;
                        this.txtEmail.Text = string.Empty;
                        this.hidePortID.Value = "0";
                        InitRepeater(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue));
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('发送端口修改成功')", true);
                    }
                }
            }
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.SendPorts.Find(int.Parse(id));
                if(entity != null)
                {
                    this.hidePortID.Value = id;
                    this.ddlCustomer.SelectedValue = entity.CustomerID.ToString();
                    this.txtHandPhone.Text = entity.HandPhone;
                    this.txtWeixin.Text = entity.Weixin;
                    this.txtQQ.Text = entity.QQ;
                    this.txtEmail.Text = entity.Email;
                }
            }
        }

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            using (var context = new ReportContext())
            {
                var entity = context.SendPorts.Find(int.Parse(id));
                if (entity != null)
                {
                    context.SendPorts.Remove(entity);
                    context.SaveChanges();
                    InitRepeater(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue));
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('发送端口删除成功')", true);
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue));
        }
    }
}