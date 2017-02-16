using Aspose.Slides;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Word = Microsoft.Office.Interop.Word;
using System.Web.ModelBinding;
using System.Web;

namespace ReportApplication
{
    public partial class GenerateReference : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            this.txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            List<string> listPaperId1 = new List<string>();
            ViewState["listPaperId1"] = listPaperId1;

            List<string> listPaperId2 = new List<string>();
            ViewState["listPaperId2"] = listPaperId2;

            List<string> listPaperId3 = new List<string>();
            ViewState["listPaperId3"] = listPaperId3;

            List<string> listPaperId4 = new List<string>();
            ViewState["listPaperId4"] = listPaperId4;

            List<string> listPaperId5 = new List<string>();
            ViewState["listPaperId5"] = listPaperId5;

           

            List<string> listPaperId7 = new List<string>();
            ViewState["listPaperId7"] = listPaperId7;
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
        public void ddlCustomer_GetData([System.Web.ModelBinding.Control("ddlCustomerCategory","SelectedValue")]int? categoryId)
        {
            using (var context = new ReportContext())
            {
                int[] customerIds = CustomerID.Split(',').Select(c => int.Parse(c)).ToArray();
                var items = context.Customers.Where(c => customerIds.Contains(c.ID)).Where(c => c.CategoryID == categoryId).Select(c => new
                {
                    text = c.Name,
                    value = c.ID.ToString()
                });
                this.ddlCustomer.Items.Clear();
                this.ddlCustomer.Items.Add(new ListItem { Text = "选择客户", Value = "0" });
                items.ForEach(item =>
                {
                    this.ddlCustomer.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            };
        }
        public void ddlTopic1Title_GetData()
        {
            using (var context = new PaperContext())
            {
                var entities = context.PaperCategories.Select(c => new
                {
                    text = c.CategoryName,
                    value = c.CategoryID.ToString()
                });
                this.ddlTopic1Title.Items.Clear();
                this.ddlTopic1Title.Items.Add(new ListItem { Text = "选择分类", Value = "0" });
                entities.ForEach(entity =>
                {
                    ListItem item = new ListItem();
                    item.Text = entity.text;
                    item.Value = entity.value;
                    if (entity.value == "3")
                    {
                        item.Selected = true;
                    }
                    this.ddlTopic1Title.Items.Add(item);
                });
            }
        }
        public void ddlTopic2Title_GetData()
        {
            using (var context = new PaperContext())
            {
                var entities = context.PaperCategories.Select(c => new
                {
                    text = c.CategoryName,
                    value = c.CategoryID.ToString()
                });
                this.ddlTopic2Title.Items.Clear();
                this.ddlTopic2Title.Items.Add(new ListItem { Text = "选择分类", Value = "0" });
                entities.ForEach(entity =>
                {
                    ListItem item = new ListItem();
                    item.Text = entity.text;
                    item.Value = entity.value;
                    if (entity.value == "4")
                    {
                        item.Selected = true;
                    }
                    this.ddlTopic2Title.Items.Add(item);
                });
            }
        }
        public void ddlTopic3Title_GetData()
        {
            using (var context = new PaperContext())
            {
                var entities = context.PaperCategories.Select(c => new
                {
                    text = c.CategoryName,
                    value = c.CategoryID.ToString()
                });
                this.ddlTopic3Title.Items.Clear();
                this.ddlTopic3Title.Items.Add(new ListItem { Text = "选择分类", Value = "0" });
                entities.ForEach(entity =>
                {
                    ListItem item = new ListItem();
                    item.Text = entity.text;
                    item.Value = entity.value;
                    if (entity.value == "5")
                    {
                        item.Selected = true;
                    }
                    this.ddlTopic3Title.Items.Add(item);
                });
            }
        }
        public void ddlPictureTemplate_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate.Items.Clear();
                this.ddlPictureTemplate.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlPictureTemplate1_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate1.Items.Clear();
                this.ddlPictureTemplate1.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate1.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlPictureTemplate2_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate2.Items.Clear();
                this.ddlPictureTemplate2.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate2.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlPictureTemplate4_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate4.Items.Clear();
                this.ddlPictureTemplate4.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate4.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlPictureTemplate5_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate5.Items.Clear();
                this.ddlPictureTemplate5.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate5.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlPictureTemplate6_GetData()
        {
            using (var context = new PaperContext())
            {
                var items = context.PPTTemplates.Select(p => new
                {
                    text = p.Title,
                    value = p.Path
                });
                this.ddlPictureTemplate6.Items.Clear();
                this.ddlPictureTemplate6.Items.Add(new ListItem { Text = "选择模版", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlPictureTemplate6.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        #endregion

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = this.ddlCustomer.SelectedValue;
            ClearList();
            this.Image1.Visible = false;
            this.ddlPictureTemplate.SelectedIndex = -1;
            if (selectedValue != "0")
            {
                int Str_CustomerID = int.Parse(selectedValue);
                
            }
        }

        private void ClearList()
        {
            List<string> listPaperId2 = (List<string>)ViewState["listPaperId2"];
            listPaperId2.Clear();
            List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
            listPaperId3.Clear();
            List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
            listPaperId4.Clear();
            List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
            listPaperId5.Clear();
            List<string> listPaperId7 = (List<string>)ViewState["listPaperId7"];
            listPaperId7.Clear();
        }

        
        
        

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView1.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView1.Rows[index].FindControl("CheckBox1"));
            List<string> listPaperId1 = (List<string>)ViewState["listPaperId1"];
            if (cb1.Checked == true)
            {                
                if(!listPaperId1.Contains(cb1.ToolTip))
                {
                    listPaperId1.Add(cb1.ToolTip);
                }
            }
            else
            {
                if(listPaperId1.Contains(cb1.ToolTip))
                {
                    listPaperId1.Remove(cb1.ToolTip);
                }
            }
        }
        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView2.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView2.Rows[index].FindControl("CheckBox2"));
            List<string> listPaperId2 = (List<string>)ViewState["listPaperId2"];
            if (cb1.Checked == true)
            {
                if (!listPaperId2.Contains(cb1.ToolTip))
                {
                    listPaperId2.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (listPaperId2.Contains(cb1.ToolTip))
                {
                    listPaperId2.Remove(cb1.ToolTip);
                }
            }
        }
        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView3.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView3.Rows[index].FindControl("CheckBox3"));
            List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
            if (cb1.Checked == true)
            {
                if (!listPaperId3.Contains(cb1.ToolTip))
                {
                    listPaperId3.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (listPaperId3.Contains(cb1.ToolTip))
                {
                    listPaperId3.Remove(cb1.ToolTip);
                }
            }
        }
        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView4.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView4.Rows[index].FindControl("CheckBox4"));
            List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
            if (cb1.Checked == true)
            {
                if (!listPaperId4.Contains(cb1.ToolTip))
                {
                    listPaperId4.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (listPaperId4.Contains(cb1.ToolTip))
                {
                    listPaperId4.Remove(cb1.ToolTip);
                }
            }
        }
        protected void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView5.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView5.Rows[index].FindControl("CheckBox5"));
            List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
            if (cb1.Checked == true)
            {
                if (!listPaperId5.Contains(cb1.ToolTip))
                {
                    listPaperId5.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (listPaperId5.Contains(cb1.ToolTip))
                {
                    listPaperId5.Remove(cb1.ToolTip);
                }
            }
        }
        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var paperId = this.GridView7.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView7.Rows[index].FindControl("CheckBox7"));
            List<string> listPaperId7 = (List<string>)ViewState["listPaperId7"];
            if (cb1.Checked == true)
            {
                if (!listPaperId7.Contains(cb1.ToolTip))
                {
                    listPaperId7.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (listPaperId7.Contains(cb1.ToolTip))
                {
                    listPaperId7.Remove(cb1.ToolTip);
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId1 = (List<string>)ViewState["listPaperId1"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox1");
                if(listPaperId1.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId2 = (List<string>)ViewState["listPaperId2"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox2");
                if (listPaperId2.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox3");
                if (listPaperId3.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox4");
                if (listPaperId4.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
        protected void GridView5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox5");
                if (listPaperId5.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
        protected void GridView6_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }        
        protected void GridView7_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> listPaperId7 = (List<string>)ViewState["listPaperId7"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox7");
                if (listPaperId7.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string categoryId = this.ddlCustomerCategory.SelectedValue;
            string customerId = this.ddlCustomer.SelectedValue;
            string reportTitle = this.txtTitle.Text.Trim();
            string attachmentFolder = Server.MapPath("~/Download/附件二/");
            string customer = this.ddlCustomer.SelectedItem.Text;
            string saveFolder = Path.Combine(attachmentFolder, customer);
            DirectoryInfo dirInfo = new DirectoryInfo(saveFolder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fileExt = ".doc";
            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            string savePath = Path.Combine(saveFolder, newFileName);
            string hotTitle = string.Empty;
            string comeFrom = string.Empty;
            string hotContent = string.Empty;
            using (var context = new PaperContext())
            {
                if(Request.Form["MyRadioButton"] != null)
                {
                    var paperId = Request.Form["MyRadioButton"];
                    var entity = context.Papers.Find(int.Parse(paperId));
                    if (entity != null)
                    {
                        hotTitle = entity.Title;
                        comeFrom = entity.FirstSite;
                        hotContent = entity.Summary;
                    }
                }
                
            }

            string filePath = Server.MapPath("~/template_files/附件二企业一周舆情参考.doc");
            object missing = Type.Missing;
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            Word.Document document = null;
            try
            {
                document = wordApplication.Documents.Open(
                filePath, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing);
                Word.MailMerge wrdMailMerge = document.MailMerge;
                foreach (Word.MailMergeField f in wrdMailMerge.Fields)
                {
                    if(f.Code.Text.IndexOf("ReportTitle") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.txtTitle.Text.Trim());
                    }
                    else if(f.Code.Text.IndexOf("Customer") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlCustomer.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Picture1") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image1.ImageUrl))
                        {
                            var lineShape = wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image1.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content1") > -1)
                    {
                        List<string> listPaperId1 = (List<string>)ViewState["listPaperId1"];
                        f.Select();
                        if (listPaperId1.Count > 0)
                        {
                            var ids = listPaperId1.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;

                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("Picture2") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image2.ImageUrl))
                        {
                            wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image2.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content2") > -1)
                    {
                        f.Select();
                        List<string> listPaperId2 = (List<string>)ViewState["listPaperId2"];
                        f.Select();
                        if (listPaperId2.Count > 0)
                        {
                            var ids = listPaperId2.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("Topic1Title") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlTopic1Title.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Picture3") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image3.ImageUrl))
                        {
                            wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image3.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content3") > -1)
                    {
                        f.Select();
                        List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
                        f.Select();
                        if (listPaperId3.Count > 0)
                        {
                            var ids = listPaperId3.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("Topic2Title") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlTopic2Title.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Picture4") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image4.ImageUrl))
                        {
                            wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image4.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content4") > -1)
                    {
                        f.Select();
                        List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
                        f.Select();
                        if (listPaperId4.Count > 0)
                        {
                            var ids = listPaperId4.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("Topic3Title") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlTopic3Title.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Picture5") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image5.ImageUrl))
                        {
                            wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image5.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content5") > -1)
                    {
                        f.Select();
                        List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
                        f.Select();
                        if (listPaperId5.Count > 0)
                        {
                            var ids = listPaperId5.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("Picture6") > -1)
                    {
                        f.Select();
                        if (!string.IsNullOrEmpty(this.Image6.ImageUrl))
                        {
                            wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(this.Image6.ImageUrl), missing, missing, missing);
                        }
                    }
                    else if (f.Code.Text.IndexOf("Content7") > -1)
                    {
                        f.Select();
                        List<string> listPaperId7 = (List<string>)ViewState["listPaperId7"];
                        f.Select();
                        if (listPaperId7.Count > 0)
                        {
                            var ids = listPaperId7.Select(s => int.Parse(s));
                            using (var context = new PaperContext())
                            {
                                var entities = context.Papers.Where(p => ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                                {
                                    p.Title,
                                    p.PaperPublishedDate,
                                    p.FirstSite,
                                    p.ReprintSite,
                                    p.ReprintCount,
                                    p.Url,
                                    p.Summary
                                });
                                int i = 0;
                                foreach (var entity in entities)
                                {
                                    i++;
                                    string title = String.Format("{0}.{1}", i, entity.Title);

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "楷体_GB2312";
                                    wordApplication.Selection.TypeText(title);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";

                                    wordApplication.Selection.TypeText("文章网址：");
                                    wordApplication.Selection.Font.UnderlineColor = Word.WdColor.wdColorBlue;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
                                    wordApplication.Selection.Font.Size = 10;
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                                    var range = wordApplication.Selection.Range;
                                    wordApplication.Selection.Document.Hyperlinks.Add(range, entity.Url, missing, missing, entity.Url, missing);
                                    wordApplication.Selection.TypeParagraph();

                                    wordApplication.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                                    wordApplication.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                                    wordApplication.Selection.Font.Size = 16;
                                    wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                    wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                    wordApplication.Selection.TypeText(entity.Summary);
                                    if (i != entities.Count())
                                    {
                                        wordApplication.Selection.TypeParagraph();
                                    }

                                }
                            }
                        }
                    }
                    else if (f.Code.Text.IndexOf("HotTitle") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(hotTitle);
                    }
                    else if (f.Code.Text.IndexOf("ComeFrom") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(comeFrom);
                    }
                    else if (f.Code.Text.IndexOf("HotContent") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(hotContent);
                    }
                }
                
                document.SaveAs2(savePath, Word.WdSaveFormat.wdFormatDocument, missing, missing,
                    missing, missing, false, missing, missing, missing, missing, missing, missing, missing, missing,
                    missing, missing);
                using (var context = new ReportContext())
                {
                    var entity = new ReferenceReport
                    {
                        CategoryID = int.Parse(categoryId),
                        CustomerID = int.Parse(customerId),
                        ReportTitle = reportTitle,
                        FilePath = savePath,
                        AddDate = DateTime.Now
                    };
                    context.ReferenceReports.Add(entity);
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (document != null)
                {
                    document.Close(Word.WdSaveOptions.wdDoNotSaveChanges, ref missing, ref missing);
                    document = null;
                }
                if (wordApplication != null)
                {
                    wordApplication.Quit(ref missing, ref missing, ref missing);
                    wordApplication = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            CommonUtility.PrintFile(savePath, newFileName);
        }

        protected void ddlTopic1Title_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
            listPaperId3.Clear();
            
        }
        protected void ddlTopic2Title_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
            listPaperId4.Clear();
            
        }
        protected void ddlTopic3Title_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
            listPaperId5.Clear();
            
        }

        protected void btnCreatePicture1_Click(object sender, EventArgs e)
        {
            List<string> listPaperId1 = (List<string>)ViewState["listPaperId1"];
            if(listPaperId1.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if(this.ddlPictureTemplate.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId1.Count - 1;
                int slidesCount = pres.Slides.Count;
                if(index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId1.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach(var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image1.ImageUrl = imageUrl;
                    this.Image1.Visible = true;

                }
            }
        }
        protected void btnCreatePicture2_Click(object sender, EventArgs e)
        {
            List<string> listPaperId2 = (List<string>)ViewState["listPaperId2"];
            if (listPaperId2.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if (this.ddlPictureTemplate1.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate1.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId2.Count - 1;
                int slidesCount = pres.Slides.Count;
                if (index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId2.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach (var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image2.ImageUrl = imageUrl;
                    this.Image2.Visible = true;

                }
            }
        }
        protected void btnCreatePicture3_Click(object sender, EventArgs e)
        {
            List<string> listPaperId3 = (List<string>)ViewState["listPaperId3"];
            if (listPaperId3.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if (this.ddlPictureTemplate2.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate2.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId3.Count - 1;
                int slidesCount = pres.Slides.Count;
                if (index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId3.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach (var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image3.ImageUrl = imageUrl;
                    this.Image3.Visible = true;

                }
            }
        }
        protected void btnCreatePicture4_Click(object sender, EventArgs e)
        {
            List<string> listPaperId4 = (List<string>)ViewState["listPaperId4"];
            if (listPaperId4.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if (this.ddlPictureTemplate4.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate4.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId4.Count - 1;
                int slidesCount = pres.Slides.Count;
                if (index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId4.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach (var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image4.ImageUrl = imageUrl;
                    this.Image4.Visible = true;

                }
            }
        }
        protected void btnCreatePicture5_Click(object sender, EventArgs e)
        {
            List<string> listPaperId5 = (List<string>)ViewState["listPaperId5"];
            if (listPaperId5.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if (this.ddlPictureTemplate5.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate5.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId5.Count - 1;
                int slidesCount = pres.Slides.Count;
                if (index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId5.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach (var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image5.ImageUrl = imageUrl;
                    this.Image5.Visible = true;

                }
            }
        }
        protected void btnCreatePicture6_Click(object sender, EventArgs e)
        {
            List<string> listPaperId7 = (List<string>)ViewState["listPaperId7"];
            if (listPaperId7.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的文章')", true);
                return;
            }
            if (this.ddlPictureTemplate6.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('请选择要生成图片的模版')", true);
                return;
            }
            string template_Path = this.ddlPictureTemplate6.SelectedValue;
            string filePath = Server.MapPath(template_Path);
            using (Presentation pres = new Presentation(filePath))
            {
                int index = listPaperId7.Count - 1;
                int slidesCount = pres.Slides.Count;
                if (index >= slidesCount)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "alert", "alert('选择本模版最多只能选择" + slidesCount + "条文章')", true);
                    return;
                }
                using (var context = new PaperContext())
                {
                    int i = 0;
                    int[] Ids = listPaperId7.Select(a => Convert.ToInt32(a)).ToArray();
                    var items = context.Papers.Where(p => Ids.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).Select(p => new
                    {
                        key = "title",
                        Title = p.Title
                    });
                    foreach (var item in items)
                    {
                        i++;
                        AsposeUtilities.PPTFindAndReplace(pres, item.key + i, item.Title);
                    }
                    ISlide slide = pres.Slides[index];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".png";
                    string imageUrl = "/Download/PicTmpls/" + newFileName;
                    bitmap.Save(Server.MapPath(imageUrl), ImageFormat.Png);
                    this.Image6.ImageUrl = imageUrl;
                    this.Image6.Visible = true;

                }
            }
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView1_GetData([Control("ddlCustomer","SelectedValue")] int customerId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate,[Control("AspNetPager1","CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerID == customerId && p.CategoryID == 1).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            AspNetPager1.RecordCount = entities.Count();
            this.AspNetPager1.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
            return entities;

        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView2_GetData([Control("ddlCustomerCategory","SelectedValue")] int? customerCategoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate,[Control("AspNetPager2","CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            
            this.GridView1.DataSource = null;
            this.GridView1.DataBind();
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == 2).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager2.RecordCount = entities.Count();
            this.AspNetPager2.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager2.PageSize).Take(this.AspNetPager2.PageSize);
            return entities;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView3_GetData([Control("ddlCustomerCategory", "SelectedValue")] int? customerCategoryId,[Control("ddlTopic1Title","SelectedValue")] int? categoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate, [Control("AspNetPager3", "CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == categoryId).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager3.RecordCount = entities.Count();
            this.AspNetPager3.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager3.PageSize).Take(this.AspNetPager3.PageSize);
            return entities;
        }

        protected void ddlCustomerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearList();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView4_GetData([Control("ddlCustomerCategory", "SelectedValue")] int? customerCategoryId, [Control("ddlTopic2Title", "SelectedValue")] int? categoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate, [Control("AspNetPager4", "CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == categoryId).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager4.RecordCount = entities.Count();
            this.AspNetPager4.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager4.PageSize).Take(this.AspNetPager4.PageSize);
            return entities;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView5_GetData([Control("ddlCustomerCategory", "SelectedValue")] int? customerCategoryId, [Control("ddlTopic3Title", "SelectedValue")] int? categoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate, [Control("AspNetPager5", "CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == categoryId).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager5.RecordCount = entities.Count();
            this.AspNetPager5.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager5.PageSize).Take(this.AspNetPager5.PageSize);
            return entities;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView7_GetData([Control("ddlCustomerCategory", "SelectedValue")] int? customerCategoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate, [Control("AspNetPager7", "CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == 7).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager7.RecordCount = entities.Count();
            this.AspNetPager7.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager7.PageSize).Take(this.AspNetPager7.PageSize);
            return entities;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridView6_GetData([Control("ddlCustomerCategory", "SelectedValue")] int? customerCategoryId, [Control("txtStartDate", "Text")] string startDate, [Control("txtEndDate", "Text")] string endDate, [Control("AspNetPager6", "CurrentPageIndex")] int currentPageIndex = 1)
        {
            DateTime beginDate = DateTime.Parse(startDate);
            DateTime finishDate = DateTime.Parse(endDate);
            var context = new PaperContext();
            var entities = context.Papers.Where(p => p.CustomerCategoryID == customerCategoryId && p.CategoryID == 8).Where(p => p.AddDate >= beginDate && p.AddDate <= finishDate);
            this.AspNetPager6.RecordCount = entities.Count();
            this.AspNetPager6.CurrentPageIndex = currentPageIndex;
            entities = entities.OrderByDescending(p => p.PaperID).Skip((currentPageIndex - 1) * this.AspNetPager6.PageSize).Take(this.AspNetPager6.PageSize);
            return entities;
        }
    }
}