using iNethinkCMS.Command;
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Word = Microsoft.Office.Interop.Word;

namespace ReportApplication
{
    public partial class GenerateSubsidiaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            List<string> columns = new List<string>();
            columns.Add("FileName");
            columns.Add("FilePath");
            columns.Add("Customer");
            columns.Add("Subsidiary");
            columns.Add("AddDate");
            columns.Add("DocPath");
            System.Data.DataTable table = CommonUtility.CreateMemoryTable(columns);
            ViewState["Table"] = table;
            List<string> list_id = new List<string>();
            ViewState["list"] = list_id;
            BindGridView1();
        }

        private void BindGridView1(int customerId = 0, int currentPageIndex = 1)
        {

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            switch (this.ddlReportType.SelectedValue)
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
            using (var context = new ReportContext())
            {
                var articles = context.Articles.Where(c => c.CustomerID == customerId && (c.AddDate >= startDate && c.AddDate <= endDate) ).OrderByDescending(c => c.ID).ToList();
                this.AspNetPager1.RecordCount = articles.Count();
                this.GridView1.DataSource = articles.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.GridView1.DataBind();
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = this.ddlCustomer.SelectedValue;
            if (selectedValue != "0")
            {
                int customerId = int.Parse(selectedValue);
                using (var context = new ReportContext())
                {
                    var customer = context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        this.ddlSubsidiary.Items.Clear();
                        this.ddlSubsidiary.Items.Add(new ListItem { Text = "选择企业", Value = "0" });
                        foreach (var subsidiary in customer.Subsidiaries)
                        {
                            this.ddlSubsidiary.Items.Add(new ListItem { Text = subsidiary.Name, Value = subsidiary.SubsidiaryID.ToString() });
                        }
                    }

                }
                List<string> list_id = (List<string>)ViewState["list"];
                list_id.Clear();
                BindGridView1(customerId);
            }

        }

        #region SelectMethod
        public void ddlCustomer_GetData()
        {
            using (var context = new ReportContext())
            {
                var customers = context.Customers.Select(c => new
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
        public void ddlReportType_GetData()
        {
            var items = CommonUtility.GetReportTypeByCategoryName("常规服务类型");
            this.ddlReportType.Items.Clear();
            this.ddlReportType.Items.Add(new ListItem { Text = "选择类型", Value = "0" });
            items.ForEach(item =>
            {
                this.ddlReportType.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
            });
        }
        #endregion

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindGridView1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> list_id = (List<string>)ViewState["list"];
            List<int> list = list_id.Select(c => Convert.ToInt32(c)).ToList();
            if (list.Count == 0)
            {
                return;
            }
            string attachmentFolder = Server.MapPath("~/Download/附件一/");
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
            string docPath = "/Download/附件一/" + customer + "/" + newFileName;

            string filePath = Server.MapPath("~/template_files/附件一权属企业舆情专报.doc");
            //string filePath = Server.MapPath("~/template_files/111.docx");
            object missing = Type.Missing;
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            Word.Document document = null;
            try
            {
                document = wordApplication.Documents.Open(
                filePath, ref missing, false,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing);
                Word.MailMerge wrdMailMerge = document.MailMerge;
                foreach (Word.MailMergeField f in wrdMailMerge.Fields)
                {
                    if (f.Code.Text.IndexOf("Customer") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlCustomer.SelectedItem.Text.Replace("集团", ""));
                    }
                    else if (f.Code.Text.IndexOf("ReportType") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlReportType.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Subsidiary") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(this.ddlSubsidiary.SelectedItem.Text);
                    }
                    else if (f.Code.Text.IndexOf("Date") > -1)
                    {
                        f.Select();
                        wordApplication.Selection.TypeText(DateTime.Now.ToString("yyyy年MM月dd日"));
                    }
                    else if (f.Code.Text.IndexOf("CurrentNumber") > -1)
                    {
                        f.Select();
                        using (var context = new ReportContext())
                        {
                            int CustomerID = int.Parse(this.ddlCustomer.SelectedValue);
                            int reportTypeId = int.Parse(this.ddlReportType.SelectedValue);
                            int currentNumber = 0;
                            var entity = context.ReportNumbers.FirstOrDefault(a => a.CustomerID == CustomerID && a.ReportTypeId == reportTypeId);
                            if (entity != null)
                            {
                                currentNumber = entity.CurrentNumber.Value;
                            }
                            wordApplication.Selection.TypeText(currentNumber.ToString());

                        }
                    }
                    else if (f.Code.Text.IndexOf("TotalNumber") > -1)
                    {
                        f.Select();
                        using (var context = new ReportContext())
                        {
                            int CustomerID = int.Parse(this.ddlCustomer.SelectedValue);
                            int reportTypeId = int.Parse(this.ddlReportType.SelectedValue);
                            int totalNumber = 0;
                            var entity = context.ReportNumbers.FirstOrDefault(a => a.CustomerID == CustomerID && a.ReportTypeId == reportTypeId);
                            if (entity != null)
                            {
                                totalNumber = entity.TotalNumber.Value;
                            }
                            wordApplication.Selection.TypeText(totalNumber.ToString());

                        }
                    }
                    else if (f.Code.Text.IndexOf("Content") > -1)
                    {
                        f.Select();
                        using (var context = new ReportContext())
                        {
                            var articles = context.Articles.Where(a => list.Contains(a.ID)).OrderByDescending(a => a.Rating);
                            int i = 0;
                            //wordApplication.Selection.ParagraphFormat.IndentFirstLineCharWidth(1);
                            foreach (var article in articles)
                            {
                                i++;

                                wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;
                                wordApplication.Selection.Font.Size = 16;
                                wordApplication.Selection.Font.Name = "楷体_GB2312";
                                string title = String.Format("{0}.{1}", i, article.Title);

                                wordApplication.Selection.TypeText(title);
                                wordApplication.Selection.TypeParagraph();
                                wordApplication.Selection.Font.Bold = (int)Word.WdConstants.wdToggle;

                                //wordApplication.Selection.Font.Size = 16;
                                wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                wordApplication.Selection.TypeText(article.Content);
                                wordApplication.Selection.TypeParagraph();
                                if (!string.IsNullOrEmpty(article.ScreenshotsPath))
                                {
                                    FileInfo fileInfo = new FileInfo(Server.MapPath(article.ScreenshotsPath));
                                    if (fileInfo.Exists)
                                    {
                                        wordApplication.Selection.InlineShapes.AddPicture(Server.MapPath(article.ScreenshotsPath), missing, missing, missing);
                                        wordApplication.Selection.TypeParagraph();
                                    }
                                }


                                wordApplication.Selection.TypeText("网址链接：");

                                wordApplication.Selection.Font.Name = "宋体";
                                Word.Range range = wordApplication.Selection.Range;
                                wordApplication.Selection.Font.Size = 10;
                                wordApplication.Selection.Hyperlinks.Add(range, article.Url, missing, missing, article.Url, missing);
                                wordApplication.Selection.TypeParagraph();




                                wordApplication.Selection.Font.Name = "仿宋_GB2312";
                                wordApplication.Selection.Font.Size = 16;
                                wordApplication.Selection.TypeText("风险分析研判：");
                                wordApplication.Selection.TypeText(article.JudgeContent);
                                wordApplication.Selection.TypeParagraph();
                                wordApplication.Selection.TypeText("舆情星级：");
                                wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
                                wordApplication.Selection.TypeText("★".Repeat(article.Rating.Value));
                                wordApplication.Selection.TypeParagraph();

                                wordApplication.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                                wordApplication.Selection.TypeText("处置建议：");
                                wordApplication.Selection.TypeText(article.SuggestContent);
                                wordApplication.Selection.TypeParagraph();
                            }
                        }
                    }
                }
                document.SaveAs2(savePath, Word.WdSaveFormat.wdFormatDocument, missing, missing,
                    missing, missing, false, missing, missing, missing, missing, missing, missing, missing, missing,
                    missing, missing);
                using (var context = new ReportContext())
                {
                    int customerId = int.Parse(this.ddlCustomer.SelectedValue);
                    int reportTypeId = int.Parse(this.ddlReportType.SelectedValue);
                    var entity = context.ReportNumbers.FirstOrDefault(a => a.CustomerID == customerId && a.ReportTypeId == reportTypeId);
                    if (entity != null)
                    {
                        entity.CurrentNumber++;
                        entity.TotalNumber++;
                        context.SaveChanges();
                    }
                }
                System.Data.DataTable table = (System.Data.DataTable)ViewState["Table"];
                System.Data.DataRow row = table.NewRow();
                row["FileName"] = newFileName;
                row["FilePath"] = savePath;
                row["Customer"] = this.ddlCustomer.SelectedItem.Text;
                row["Subsidiary"] = this.ddlSubsidiary.SelectedItem.Text;
                row["AddDate"] = DateTime.Now.ToString();
                row["DocPath"] = docPath;
                table.Rows.Add(row);
                BindRepeater1(table);
                BindGridView1(int.Parse(this.ddlCustomer.SelectedValue), this.AspNetPager1.CurrentPageIndex);

            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "alert", "alert('" + ex.Message + "')", true);
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
        }

        private void BindRepeater1(System.Data.DataTable table)
        {
            this.Repeater1.DataSource = table;
            this.Repeater1.DataBind();
        }

        protected void btnMergeFile_Click(object sender, EventArgs e)
        {
            System.Data.DataTable table = (System.Data.DataTable)ViewState["Table"];
            
            string attachmentFolder = Server.MapPath("~/Download/附件一/");
            string customer = this.ddlCustomer.SelectedItem.Text;
            string saveFolder = Path.Combine(attachmentFolder, customer);
            DirectoryInfo dirInfo = new DirectoryInfo(saveFolder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fileExt = ".doc";
            String newFileName = String.Format("附件一：权属企业“{0}”网络舆情专报_",this.ddlCustomer.SelectedItem.Text) + DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            
            string savePath = Path.Combine(saveFolder, newFileName);

            Word.Application wordApplication = null;
            Word.Document wordDocument = null;
            object missing = Type.Missing;
            try
            {
                string filePath = Server.MapPath("~/template_files/附件一权属企业舆情专报空白模版.doc");                
                
                wordApplication = new Microsoft.Office.Interop.Word.Application();
                wordDocument = wordApplication.Documents.Open(
                    filePath, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing);
                int i = 0;
                foreach (System.Data.DataRow row in table.Rows)
                {
                    i++;
                    wordApplication.Selection.InsertFile(row["FilePath"].ToString(), missing, missing, missing, missing);
                    if (i < table.Rows.Count)
                    {
                        wordApplication.Selection.InsertBreak(Word.WdBreakType.wdPageBreak);
                    }
                }
                wordDocument.SaveAs2(savePath, Word.WdSaveFormat.wdFormatDocument);
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(false, ref missing, ref missing);
                    wordDocument = null;
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

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> list_id = (List<string>)ViewState["list"];
            list_id.Clear();
            BindGridView1(int.Parse(this.ddlCustomer.SelectedValue));
        }

        protected void lnkDeleteFile_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            System.Data.DataTable table = (System.Data.DataTable)ViewState["Table"];
            var row = table.Rows.Find(int.Parse(id));
            if(row != null)
            {
                string filePath = row["FilePath"].ToString();
                FileInfo fileInfo = new FileInfo(filePath);
                if(fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                table.Rows.Remove(row);
                BindRepeater1(table);
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            var newsId = this.GridView1.DataKeys[index].Values[0];
            CheckBox cb1 = ((CheckBox)this.GridView1.Rows[index].FindControl("CheckBox1"));
            List<string> list_id = (List<string>)ViewState["list"];
            if (cb1.Checked == true)
            {
                if (!list_id.Contains(cb1.ToolTip))
                {
                    list_id.Add(cb1.ToolTip);
                }
            }
            else
            {
                if (list_id.Contains(cb1.ToolTip))
                {
                    list_id.Remove(cb1.ToolTip);
                }
            }
            Debug.WriteLine(list_id);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> list_id = (List<string>)ViewState["list"];
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox1");
                if (list_id.Contains(cb1.ToolTip))
                {
                    cb1.Checked = true;
                }
            }
        }
    }
}