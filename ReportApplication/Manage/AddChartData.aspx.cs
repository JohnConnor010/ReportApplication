using ReportApplication.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;
using System.Globalization;
using iNethinkCMS.Command;
using System.Data.Entity;
using System.Text;
using ReportApplication.Utilities;

namespace ReportApplication
{
    public partial class AddChartData : System.Web.UI.Page
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
            
            Dictionary<string, string> tables = new Dictionary<string, string>();
            ViewState["tables"] = tables;
            Dictionary<string, int> daysTrendData = new Dictionary<string, int>();
            ViewState["daysTrendData"] = daysTrendData;


            InitRepeater2();
        }

        private void InitRepeater2(int currentPageIndex = 1)
        {
            using (var context = new ChartContext())
            {
                var entities = context.Gathers.OrderByDescending(a => a.ID).ToList().Where(a => a.Flag == "gather").Select(a => new
                {
                    a.ID,
                    CustomerName = GetCustomerNameByID(a.CustomerID),
                    FileName = Path.GetFileName(a.ExcelFilePath),
                    a.ExcelFilePath,
                    a.PngFilePath,
                });
                this.AspNetPager1.RecordCount = entities.Count();
                this.Repeater2.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater2.DataBind();
            }
        }
        public string GetCustomerNameByID(int CustomerID)
        {
            using (var context = new ReportContext())
            {
                var entity = context.Customers.FirstOrDefault(c => c.ID == CustomerID);
                if(entity != null)
                {
                    return entity.Name;
                }
                else
                {
                    return "";
                }
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
        public void Channel_GetData()
        {
            using(var context = new ReportContext())
            {
                var channels = context.Channels.Select(c => new
                    {
                        text = c.ChannelText,
                        value = c.ChannelText
                    });
                this.ddlChannel.Items.Clear();
                this.ddlChannel.Items.Add(new ListItem { Text = "选择渠道", Value = "" });
                channels.ForEach(item =>
                    {
                        this.ddlChannel.Items.Add(new ListItem { Text = item.text, Value = item.value });
                    });
            }
        }
        
        protected void btnGatherAdd_Click(object sender, EventArgs e)
        {
                        
            string channelValue = this.ddlChannel.SelectedValue;
            int sensitiveNumber = 0;
            int.TryParse(this.txtSensitiveNumber.Text, out sensitiveNumber);
            int totalNumber = 0;
            int.TryParse(this.txtTotalNumber.Text, out totalNumber);
            Dictionary<string,string> tables = (Dictionary<string, string>)ViewState["tables"];
            if(tables.ContainsKey(channelValue))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "a", "alert('此条数据已经添加！')", true);
                return;
            }
            string value = String.Format("{0},{1}", sensitiveNumber, totalNumber);
            tables.Add(channelValue,value);
            ViewState["tables"] = tables;
            this.Repeater1.DataSource = tables;
            this.Repeater1.DataBind();
            this.txtSensitiveNumber.Text = "0";
            this.txtTotalNumber.Text = "0";
        }

        protected void btnGatherSubmit_Click(object sender, EventArgs e)
        {
            string customerId = this.ddlCustomer.SelectedValue;
            string chartTitle = this.txtChartTitle.Text.Trim().Replace("#", "");
            Dictionary<string,string> tables = (Dictionary<string, string>)ViewState["tables"];
            using (var context = new ChartContext())
            {                
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

                    Excel.Range Range_B1 = worksheet.get_Range("B1");
                    Range_B1.Value2 = "敏感信息量";
                    Excel.Range Range_C1 = worksheet.get_Range("C1");
                    Range_C1.Value2 = "信息总量";
                    int i = 1;
                    foreach (KeyValuePair<string, string> item in tables)
                    {
                        i++;
                        Excel.Range Range_Ai = worksheet.get_Range("A" + i);
                        Range_Ai.Value2 = item.Key;
                        Excel.Range Range_Bi = worksheet.get_Range("B" + i);
                        Range_Bi.Value2 = item.Value.Split(',')[0];
                        Excel.Range Range_Ci = worksheet.get_Range("C" + i);
                        Range_Ci.Value2 = item.Value.Split(',')[1];
                    }
                    Excel.Range dataSourceRange = worksheet.get_Range("A1", "C" + i);
                    Excel.Shape shape1 = worksheet.Shapes.AddChart(Excel.XlChartType.xl3DColumnClustered, missing, missing, missing, missing);

                    //设置数据源
                    shape1.Chart.SetSourceData(dataSourceRange, missing);

                    //设置标题
                    shape1.Chart.HasTitle = true;
                    shape1.Chart.ChartTitle.Text = chartTitle;
                    shape1.Chart.ChartTitle.Format.TextFrame2.TextRange.Font.Size = 16;

                    //设置Legend
                    shape1.Chart.SeriesCollection(1).Name = (string)worksheet.get_Range("B1").Value2;
                    shape1.Chart.SeriesCollection(2).Name = (string)worksheet.get_Range("C1").Value2;
                    shape1.Chart.ApplyDataLabels();

                    shape1.Left = (float)worksheet.get_Range("G1").Left;
                    shape1.Top = (float)worksheet.get_Range("G6").Top;
                    shape1.Fill.Visible = MsoTriState.msoTrue;
                    shape1.Fill.ForeColor.RGB = Color.FromArgb(225, 236, 238).ToArgb();
                    shape1.Fill.Transparency = 0;
                    shape1.Fill.Solid();
                    shape1.Chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;

                    DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/Download/" + this.ddlCustomer.SelectedItem.Text));
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    String ymd = DateTime.Now.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
                    string saveExcelFileName = String.Format("{0}_{1}.xlsx",chartTitle,ymd);
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
                        Flag = "gather"
                    };
                    foreach (KeyValuePair<string, string> item in tables)
                    {
                        var gatherQuantity = new GatherQuantity
                        {
                            ChannelName = item.Key,
                            SensitiveNumber = int.Parse(item.Value.Split(',')[0]),
                            TotalNumber = int.Parse(item.Value.Split(',')[1]),
                        };
                        entity.GatherQuantities.Add(gatherQuantity);
                    }
                    context.Gathers.Add(entity);
                    context.SaveChanges();
                    InitRepeater2();
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
                }
                catch (Exception ex)
                {
                    MessageBox.ShowAndRedirect(this, "生成图表是出错：" + ex.Message,Request.RawUrl);
                }
            }
            
            
        }

        protected void lnkDeleteQuantity_Command(object sender, CommandEventArgs e)
        {
            string key = e.CommandArgument.ToString();
            Dictionary<string, string> tables = (Dictionary<string,string>)ViewState["tables"];
            if(tables.ContainsKey(key))
            {
                tables.Remove(key);
                this.Repeater1.DataSource = tables;
                this.Repeater1.DataBind();
            }
        }

        protected void lnkDeleteFile_Command(object sender, CommandEventArgs e)
        {
            string filePath = e.CommandArgument.ToString().Split('|')[0];
            string id = e.CommandArgument.ToString().Split('|')[1];
            using (var context = new ChartContext())
            {
                int gatherId = int.Parse(id);
                var entity = context.Gathers.Find(gatherId);
                if(entity != null)
                {
                    var entities = context.GatherQuantities.Where(a => a.GatherID == gatherId);
                    context.GatherQuantities.RemoveRange(entities);
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
                        MessageBox.ShowAndRedirect(this, "文件删除成功", "AddChartData.aspx?tab=gather");
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception(ex.Message);
                        MessageBox.ShowAndRedirect(this, "文件删除失败:" + ex.Message, "AddChartData.aspx?tab=gather");
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> tables = (Dictionary<string, string>)ViewState["tables"];
            tables.Clear();
            this.Repeater1.DataSource = tables;
            this.Repeater1.DataBind();
            this.ddlChannel.SelectedIndex = -1;
            this.txtSensitiveNumber.Text = "0";
            this.txtTotalNumber.Text = "0";
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater2(this.AspNetPager1.CurrentPageIndex);
        }
    }
}