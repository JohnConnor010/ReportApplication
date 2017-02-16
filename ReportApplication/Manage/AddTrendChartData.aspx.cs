using iNethinkCMS.Command;
using Microsoft.Office.Core;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReportApplication
{
    public partial class AddTrendChartData : System.Web.UI.Page
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
            
            this.txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            InitGridView1("1", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),DateTime.Now.ToString("yyyy-MM-dd"));
            InitRepeater3();
        }
        private void InitGridView1(string chartType,string startDateStr,string endDateStr)
        {
            List<string> columns = new List<string>();
            columns.Add("Date");
            columns.Add("Quantity");
            DataTable table = CreateTempTable(columns);
            DateTime startDate = DateTime.Now;
            DateTime.TryParse(startDateStr, out startDate);
            DateTime endDate = DateTime.Now;
            DateTime.TryParse(endDateStr, out endDate);
            for (DateTime dt = startDate; dt <= endDate;dt = dt.AddDays(1))
            {
                DataRow row = table.NewRow();
                row["Date"] = dt.ToString("yyyy年MM月dd日");
                row["Quantity"] = "0";
                table.Rows.Add(row);
            }
            ViewState["table"] = table;
            BindGridView1();
        }
        private void InitGridView2()
        {
            List<string> columns = new List<string>();
            columns.Add("Issue");
            columns.Add("Quantity");
            DataTable table1 = CreateTempTable(columns);
            ViewState["table1"] = table1;
            BindGridView2();
        }
        private void BindGridView1()
        {
            DataTable table = (DataTable)ViewState["table"];
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
        }
        private void BindGridView2()
        {
            DataTable table = (DataTable)ViewState["table1"];
            this.GridView2.DataSource = table;
            this.GridView2.DataBind();
            
        }
        public void ddlCustomer_GetData()
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
        private DataTable CreateTempTable(List<string> columns)
        {
            DataTable tempTable = new DataTable();
            DataColumn[] keys = new DataColumn[2];
            DataColumn _IDColumn = new DataColumn("ID", typeof(int));
            _IDColumn.AutoIncrement = true;
            _IDColumn.AutoIncrementSeed = 1;
            _IDColumn.AutoIncrementStep = 1;
            keys[0] = _IDColumn;
            tempTable.Columns.Add(_IDColumn);
            tempTable.PrimaryKey = keys;
            foreach (var column in columns)
            {
                tempTable.Columns.Add(new DataColumn(column, typeof(string)));
            }
            return tempTable;
        }

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.ddlChartType.SelectedValue == "0")
            {
                this.Panel1_1.Visible = false;
                this.Panel1_2.Visible = false;
                this.Panel1_3.Visible = false;
                this.Panel2_1.Visible = false;
                this.Panel2_2.Visible = false;
            }
            if (this.ddlChartType.SelectedValue == "1")
            {
                this.Panel1_1.Visible = true;
                this.Panel1_2.Visible = true;
                this.Panel1_3.Visible = true;
                this.Panel2_1.Visible = false;
                this.Panel2_2.Visible = false;
                InitGridView1(this.ddlChartType.SelectedValue, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),DateTime.Now.ToString("yyyy-MM-dd"));
            }
            if (this.ddlChartType.SelectedValue == "2")
            {
                InitGridView2();
                this.txtIssueNumber.Text = string.Empty;
                this.txtQuantity.Text = string.Empty;
                this.Panel2_1.Visible = true;
                this.Panel2_2.Visible = true;
                this.Panel1_1.Visible = false;
                this.Panel1_2.Visible = false;
                this.Panel1_3.Visible = false;
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView1.EditIndex = e.NewEditIndex;
            BindGridView1();
            CommonUtility.GridViewSetFocus(this.GridView1.Rows[e.NewEditIndex], "txtQuantity");
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var id = this.GridView1.DataKeys[e.RowIndex].Values[0].ToInt32();
            string date = ((TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtDate")).Text;
            string number = ((TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtQuantity")).Text;
            if(number == "0")
            {
                MessageBox.Show(this, "数量不能为0");
                return;
            }
            DataTable table = (DataTable)ViewState["table"];
            DataRow row = table.Rows.Find(id);
            if(row != null)
            {
                row["Date"] = date;
                row["Quantity"] = number;
            }
            this.GridView1.EditIndex = -1;
            BindGridView1();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            BindGridView1();
        }

        protected void btnAddIssue_Click(object sender, EventArgs e)
        {
            DataTable table = (DataTable)ViewState["table1"];
            string issueNumber = this.txtIssueNumber.Text.Trim();
            int quantity = 0;
            int.TryParse(this.txtQuantity.Text.Trim(), out quantity);
            string issue = String.Format("第{0}期", issueNumber);
            DataRow row = table.NewRow();
            row["Issue"] = issue;
            row["Quantity"] = quantity.ToString();
            table.Rows.Add(row);

            this.txtIssueNumber.Text = string.Empty;
            this.txtQuantity.Text = string.Empty;
            this.GridView2.EditIndex = -1;
            BindGridView2();

        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView2.EditIndex = e.NewEditIndex;
            BindGridView2();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable table = (DataTable)ViewState["table1"];
            var id = this.GridView2.DataKeys[e.RowIndex].Values[0].ToInt32();
            string issue = ((TextBox)this.GridView2.Rows[e.RowIndex].FindControl("txtIssue")).Text;
            string quantity = ((TextBox)this.GridView2.Rows[e.RowIndex].FindControl("txtQuantityNumber")).Text;
            DataRow row = table.Rows.Find(id);
            if(row != null)
            {
                row["Issue"] = issue;
                row["Quantity"] = quantity;
            }
            this.GridView2.EditIndex = -1;
            BindGridView2();
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView2.EditIndex = -1;
            BindGridView2();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable table = (DataTable)ViewState["table1"];
            table.Rows.RemoveAt(e.RowIndex);
            e.Cancel = true;
            BindGridView2();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            string customerId = this.ddlCustomer.SelectedValue;
            string filePath = Server.MapPath("~/template_files/template_workbook.xlsx");
            string chartTitle = this.txtChartTitle.Text.Replace("#", "");
            using (var context = new ChartContext())
            {
                DataTable table = (DataTable)ViewState["table"];
                DataTable table1 = (DataTable)ViewState["table1"];
                if(this.ddlChartType.SelectedValue == "1")
                {
                    var rows = table.Select("Quantity <> '0'");
                    if (rows.Count() == 0)
                    {
                        MessageBox.Show(this, "图表数据不能为空");
                        return;
                    }
                }
                if(this.ddlChartType.SelectedValue == "2")
                {
                    var rows = table1.Rows;
                    if(rows.Count == 0)
                    {
                        MessageBox.Show(this, "图表数据不能为空");
                        return;
                    }
                }
                
                try
                {
                    var missing = Type.Missing;
                    string fileName = Server.MapPath("~/template_files/template_workbook.xlsx");
                    Excel.Application application = new Excel.Application();
                    application.Visible = false;
                    application.DisplayAlerts = false;

                    Excel.Workbook workbook = application.Workbooks.Open(fileName);
                    string sheetname = "sheet1";
                    Excel._Worksheet worksheet = workbook.Sheets[sheetname];
                    worksheet.Activate();
                    List<GatherTrend> gatherTrends = new List<GatherTrend>();
                    List<int> listElements = new List<int>();
                    int i = 0;
                    string chartCategory = this.ddlChartType.SelectedValue;
                    switch (chartCategory)
                    {
                        case "1":
                            table = (DataTable)ViewState["table"];
                            string dateString = string.Empty;
                            foreach(DataRow row in table.Rows)
                            {
                                if(row["Quantity"].ToString() != "0")
                                {
                                    if (this.rbYearMonth.Checked == true)
                                    {
                                        dateString = DateTime.Parse(row["Date"].ToString()).ToString("yyyy年M月");
                                    }
                                    else if (this.rbMonthDay.Checked == true)
                                    {
                                        dateString = DateTime.Parse(row["Date"].ToString()).ToString("M月d日");
                                    }
                                    i++;
                                    Excel.Range rangeA = worksheet.get_Range("A" + i);
                                    rangeA.Value = dateString;
                                    Excel.Range rangeB = worksheet.get_Range("B" + i);
                                    rangeB.Value = row["Quantity"].ToString();
                                    listElements.Add(row["Quantity"].ToInt32());
                                    GatherTrend trend = new GatherTrend
                                    {
                                        Name = dateString,
                                        Number = row["Quantity"].ToInt32()
                                    };
                                    gatherTrends.Add(trend);
                                }
                                
                            }
                            break;
                        case "2":
                            table1 = (DataTable)ViewState["table1"];
                            foreach(DataRow row in table1.Rows)
                            {
                                i++;
                                Excel.Range rangeA = worksheet.get_Range("A" + i);
                                rangeA.Value = row["Issue"];
                                Excel.Range rangeB = worksheet.get_Range("B" + i);
                                rangeB.Value = row["Quantity"].ToString();
                                listElements.Add(row["Quantity"].ToInt32());
                            }
                            break;
                    }

                    Excel.Range dataSourceRange = worksheet.get_Range("A1", "B" + i);
                    Excel.Shape shape1 = worksheet.Shapes.AddChart(Excel.XlChartType.xlLineMarkers, missing, missing, missing, missing);

                    //设置数据源
                    shape1.Chart.SetSourceData(dataSourceRange, missing);

                    //设置标题
                    shape1.Chart.HasTitle = true;
                    shape1.Chart.ChartTitle.Text = chartTitle.Replace("#", "");
                    shape1.Chart.ChartTitle.Format.TextFrame2.TextRange.Font.Size = 16;

                    //设置Legend
                    if(this.rblDataLabels.SelectedValue == "1")
                    {
                        shape1.Chart.ApplyDataLabels();
                    }
                    else if(this.rblDataLabels.SelectedValue == "2")
                    {
                        int maxIndex = listElements.IndexOf(listElements.Max()) + 1;
                        int minIndex = listElements.IndexOf(listElements.Min()) + 1;
                        shape1.Chart.SeriesCollection(1).Points(maxIndex).ApplyDataLabels();
                        shape1.Chart.SeriesCollection(1).Points(minIndex).ApplyDataLabels();
                    }                  


                    shape1.Left = (float)worksheet.get_Range("G1").Left;
                    shape1.Top = (float)worksheet.get_Range("G6").Top;
                    shape1.Fill.Visible = MsoTriState.msoTrue;
                    shape1.Fill.ForeColor.RGB = Color.FromArgb(225, 236, 238).ToArgb();
                    shape1.Fill.Transparency = 0;
                    shape1.Fill.Solid();
                    shape1.Chart.PlotArea.Format.Fill.Visible = MsoTriState.msoTrue;
                    shape1.Chart.PlotArea.Format.Fill.ForeColor.RGB = Color.FromArgb(218, 234, 253).ToArgb();
                    shape1.Chart.PlotArea.Format.Fill.Transparency = 0;
                    shape1.Chart.PlotArea.Format.Fill.Solid();
                    //shape1.Chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;
                    shape1.Chart.HasLegend = false;

                    DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/Download/" + this.ddlCustomer.SelectedItem.Text));
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    String ymd = DateTime.Now.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
                    string saveExcelFileName = String.Format("{0}_{1}.xlsx", chartTitle, ymd);
                    string saveExcelFilePath = Path.Combine(directoryInfo.FullName, saveExcelFileName);
                    string savePngFileName = String.Format("{0}_{1}.png", chartTitle, ymd);
                    string savePngFilePath = Path.Combine(directoryInfo.FullName, savePngFileName);
                    string excelFileName = "/Download/" + this.ddlCustomer.SelectedItem.Text + "/" + saveExcelFileName;
                    string pngFileName = "/Download/" + this.ddlCustomer.SelectedItem.Text + "/" + savePngFileName;
                    worksheet.SaveAs(saveExcelFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    shape1.Chart.Export(savePngFilePath, missing, missing);
                    application.Quit();
                    worksheet = null;
                    workbook = null;
                    application = null;
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);

                    var entity = new Gather
                    {
                        CustomerID = int.Parse(customerId),
                        TimeRange = "",
                        ChartTitle = chartTitle,
                        ExcelFilePath = excelFileName,
                        PngFilePath = pngFileName,
                        AddDate = DateTime.Now,
                        Flag = "trend"
                    };
                    gatherTrends.ForEach(item =>
                    {
                        entity.GatherTrends.Add(item);
                    });
                    context.Gathers.Add(entity);
                    context.SaveChanges();
                    using (FileStream fileStream = new FileStream(saveExcelFilePath, FileMode.Open))
                    {
                        byte[] bytes = fileStream.ToByteArray();
                        Response.Clear();
                        Response.AddHeader("Content-Length", fileStream.Length.ToString());
                        Response.ContentType = "application/ms-excel";
                        Response.AddHeader("Content-Disposition", "inline;FileName=" + saveExcelFileName);
                        fileStream.Close();
                        fileStream.Dispose();
                        Response.BinaryWrite(bytes);
                        Response.End();
                    }
                    InitRepeater3();
                }
                catch (Exception ex)
                {
                    MessageBox.ShowAndRedirect(this, "生成图表是出错：" + ex.Message, Request.RawUrl);
                }
            }
        }
        private void InitRepeater3(int currentPageIndex = 1)
        {
            using (var context = new ChartContext())
            {
                var entities = context.Gathers.OrderByDescending(a => a.ID).ToList().Where(a => a.Flag == "trend").Select(a => new
                {
                    a.ID,
                    CustomerName = new AddChartData().GetCustomerNameByID(a.CustomerID),
                    FileName = Path.GetFileName(a.ExcelFilePath),
                    a.ExcelFilePath,
                    a.PngFilePath,
                });
                this.AspNetPager1.RecordCount = entities.Count();
                this.Repeater3.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater3.DataBind();
            }
        }
        protected void lnkTrendDeleteFile_Command(object sender, CommandEventArgs e)
        {
            string filePath = e.CommandArgument.ToString().Split('|')[0];
            string id = e.CommandArgument.ToString().Split('|')[1];
            using (var context = new ChartContext())
            {
                int gatherId = int.Parse(id);
                var entity = context.Gathers.Find(gatherId);
                if (entity != null)
                {
                    var entities = context.GatherTrends.Where(a => a.GatherID == gatherId);
                    context.GatherTrends.RemoveRange(entities);
                    context.Gathers.Remove(entity);
                    try
                    {
                        context.SaveChanges();
                        string excelPath = Server.MapPath(filePath);
                        if (File.Exists(excelPath))
                        {
                            File.Delete(excelPath);
                        }
                        string pngFileName = filePath.Replace("xlsx", "png");
                        string pngPath = Server.MapPath(pngFileName);
                        if (File.Exists(pngPath))
                        {
                            File.Delete(pngPath);
                        }
                        MessageBox.ShowAndRedirect(this, "文件删除成功", "AddChartData.aspx?tab=trend");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ShowAndRedirect(this, "文件删除失败:" + ex.Message, "AddChartData.aspx?tab=trend");
                    }
                }
            }
        }
        protected void lnlDownloadPicture_Command(object sender, CommandEventArgs e)
        {
            string pictureFilePath = e.CommandArgument.ToString();
            if (File.Exists(Server.MapPath(pictureFilePath)))
            {
                using (FileStream fileStream = new FileStream(Server.MapPath(pictureFilePath), FileMode.Open))
                {
                    byte[] bytes = fileStream.ToByteArray();
                    Response.Clear();
                    Response.AddHeader("Content-Length", fileStream.Length.ToString());
                    Response.ContentType = "image/png";
                    Response.AddHeader("Content-Disposition", "inline;FileName=" + Path.GetFileName(pictureFilePath));
                    fileStream.Close();
                    fileStream.Dispose();
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
            }
            else
            {
                MessageBox.Show(this, "文件不存在！");
                return;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime.TryParse(this.txtStartDate.Text.Trim(), out startDate);
            DateTime.TryParse(this.txtEndDate.Text.Trim(), out endDate);
            InitGridView1(this.ddlChartType.SelectedValue, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater3(this.AspNetPager1.CurrentPageIndex);
        }
    }
}