using iNethinkCMS.Command;
using Microsoft.Office.Core;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReportApplication
{
    public partial class AddChartCompare : System.Web.UI.Page
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
            InitRepeater1();
        }
        
        private void InitRepeater1(int currentPageIndex = 1)
        {
            using(var context = new ChartContext())
            {
                var entities = context.Gathers.ToList().Where(a => a.Flag == "trendCompare").Select(a => new
                    {
                        a.ID,
                        CustomerName = new AddChartData().GetCustomerNameByID(a.CustomerID),
                        FileName = Path.GetFileName(a.ExcelFilePath),
                        a.ExcelFilePath,
                        a.PngFilePath,
                    });
                this.AspNetPager1.RecordCount = entities.Count();
                this.Repeater1.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView1.EditIndex = e.NewEditIndex;            
            BindGridView();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var id = this.GridView1.DataKeys[e.RowIndex].Values[0].ToString();
            DataTable table = (DataTable)ViewState["DataTable"];
            DataTable cloneTable = (DataTable)ViewState["CloneTable"];
            DataRow row = table.Rows.Find(int.Parse(id));
            int columnsCount = table.Columns.Count;
            int j =0;
            for(int i = 0;i < this.GridView1.Rows[e.RowIndex].Cells.Count;i++)
            {
                if(i >= 2)
                {
                    j++;
                    var value = ((TextBox)this.GridView1.Rows[e.RowIndex].Cells[i].Controls[0]).Text;
                    row[j] = value;
                }
            }
            DataRow cloneRow = cloneTable.Rows.Find(id);
            if(cloneRow == null)
            {
                cloneTable.Rows.Add(table.Rows[e.RowIndex].ItemArray);
            }
            else
            {
                cloneRow.Delete();
                cloneTable.Rows.Add(table.Rows[e.RowIndex].ItemArray);
            }
            cloneTable.DefaultView.Sort = "ID";
            this.GridView1.EditIndex = -1;
            BindGridView();
            this.btnSubmit.Enabled = true;
            
        }

        protected void btnAddCategory_ServerClick(object sender, EventArgs e)
        {
            string category = this.txtCategory.Text.Trim();
            ListItem item = new ListItem { Text = category, Value = category };
            if(!this.lstCategories.Items.Contains(item))
            {
                this.lstCategories.Items.Add(item);
            }
            this.txtCategory.Text = string.Empty;
        }

        protected void btnSubmitCategory_Click(object sender, EventArgs e)
        {
            if(this.lstCategories.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "alert", "alert('请输入图例');$('#txtCategory').focus();", true);
                return;
            }
            
            List<string> items = new List<string>();
            foreach(ListItem item in this.lstCategories.Items)
            {
                items.Add(item.Value);
            };
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime.TryParse(this.txtStartDate.Text.Trim(), out startDate);
            DateTime.TryParse(this.txtEndDate.Text.Trim(), out endDate);
            var table = CreateTempTable(items);
            
            

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                string horizontalCategory = dt.ToString("yyyy年MM月dd日");
                DataRow row = table.NewRow();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j != 0)
                    {
                        if (j == 1)
                        {
                            row[table.Columns[1]] = horizontalCategory;
                        }
                        else
                        {
                            row[table.Columns[j]] = "0";
                        }
                    }
                }
                row["水平类别"] = horizontalCategory;
                table.Rows.Add(row);
            }
            ViewState["DataTable"] = table;
            DataTable cloneTable = table.Clone();
            ViewState["CloneTable"] = cloneTable;
            BindGridView();
            this.UpdatePanel6.Visible = true;
            ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "show", "$('#UpdatePanel6').show();$('#UpdatePanel7').show()", true);

        }
        private void BindGridView()
        {
            DataTable table = (DataTable)ViewState["DataTable"];
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
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
            tempTable.Columns.Add(new DataColumn("水平类别", typeof(string)));
            foreach(var column in columns)
            {
                tempTable.Columns.Add(new DataColumn(column, typeof(string)));
            }
            return tempTable;
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            BindGridView();
        }

        protected void btnClearCategory_Click(object sender, EventArgs e)
        {
            this.lstCategories.Items.Clear();
            DataTable table = (DataTable)ViewState["DataTable"];
            table.Clear();
            this.GridView1.EditIndex = -1;
            BindGridView();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable table = (DataTable)ViewState["CloneTable"];
            if(table == null)
            {
                MessageBox.Show(this, "数据表不存在！");
                return;
            }
            if(table.Rows.Count < 3)
            {
                MessageBox.Show(this, "不能低于3条数据！");
                return;
            }
            string customerId = this.ddlCustomer.SelectedValue;
            string chartTitle = this.txtChartTitle.Text.Trim().Replace("#","");
            try
            {
                var missing = Type.Missing;
                string fileName = Server.MapPath("~/template_files/template_workbook.xlsx");
                Excel.Application application = new Excel.Application();
                application.Visible = false;
                application.DisplayAlerts = false;
                Excel.Workbook workbook = application.Workbooks.Open(fileName);

                string SheetName = "sheet1";
                Excel._Worksheet worksheet = workbook.Sheets[SheetName];
                worksheet.Activate();

                //写入Excel列头
                int i = 0;
                int columnsCount = table.Columns.Count;
                for (int j = 0; j < columnsCount; j++)
                {
                    if(j > 1)
                    {
                        i++;
                        var letter = CommonUtility.GetLetters()[i];
                        Excel.Range range = worksheet.get_Range(letter + "1");
                        range.Value2 = table.Columns[j].ColumnName;
                    }
                }
                //写入Excel数据
                int a = 1;
                foreach(DataRow row in table.Rows)
                {
                    a++;
                    for(int b = 1;b < columnsCount;b++)
                    {
                        Excel.Range range = worksheet.get_Range(CommonUtility.GetLetters()[b - 1] + a);
                        range.Value2 = row[b].ToString();
                    }
                    
                }


                Excel.Shape shape1 = worksheet.Shapes.AddChart(Excel.XlChartType.xlLine, missing, missing, missing, missing);

                Excel.Range sourceRange = worksheet.get_Range("A1", CommonUtility.GetLetters()[columnsCount - 2] + a);

                //设置数据源
                shape1.Chart.SetSourceData(sourceRange, missing);

                //设置标题
                shape1.Chart.HasTitle = true;
                shape1.Chart.ChartTitle.Text = chartTitle;
                shape1.Chart.ChartTitle.Format.TextFrame2.TextRange.Font.Size = 16;

                Excel.Series se = shape1.Chart.SeriesCollection(1);
                se.Format.Line.Visible = MsoTriState.msoTrue;
                se.Format.Line.Transparency = 0.0f;
                se.Format.Line.Weight = 2.25f;

                se = shape1.Chart.SeriesCollection(2);
                se.Format.Line.ForeColor.RGB = Color.FromArgb(0, 0, 192).ToArgb();
                se.Format.Line.Visible = MsoTriState.msoTrue;
                se.Format.Line.Transparency = 0.0f;
                se.Format.Line.Weight = 2.25f;

                shape1.Left = (float)worksheet.get_Range("G1").Left;
                shape1.Top = (float)worksheet.get_Range("G8").Top;
                shape1.Fill.Visible = MsoTriState.msoTrue;
                shape1.Fill.ForeColor.RGB = Color.FromArgb(225, 236, 238).ToArgb();
                shape1.Fill.Transparency = 0;
                shape1.Fill.Solid();
                shape1.Chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;
                shape1.Chart.PlotArea.Format.Fill.Visible = MsoTriState.msoTrue;
                shape1.Chart.PlotArea.Format.Fill.ForeColor.RGB = Color.FromArgb(218, 234, 253).ToArgb();
                shape1.Chart.PlotArea.Format.Fill.Transparency = 0;
                shape1.Chart.PlotArea.Format.Fill.Solid();

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
                using(var context = new ChartContext())
                {
                    var entity = new Gather
                    {
                        CustomerID = int.Parse(customerId),
                        TimeRange = "",
                        ChartTitle = chartTitle,
                        ExcelFilePath = excelFileName,
                        PngFilePath = pngFileName,
                        AddDate = DateTime.Now,
                        Flag = "trendCompare"
                    };
                    StringBuilder sBuilder = new StringBuilder();
                    foreach(ListItem item in this.lstCategories.Items)
                    {
                        sBuilder.AppendFormat("{0},", item.Value);
                    }
                    string legendString = sBuilder.ToString().TrimEnd(',');
                    Legend legend = new Legend()
                    {
                        LegendName = legendString
                    };
                    entity.Legends.Add(legend);

                    StringBuilder legendDataBuilder = new StringBuilder();
                    foreach (DataRow row in table.Rows)
                    {                        
                        for (int c = 0; c < table.Columns.Count; c++)
                        {
                            if (c > 0)
                            {
                                if(c == 1)
                                {
                                    legendDataBuilder.AppendFormat("{0}+", row[c].ToString());
                                }
                                else
                                {
                                    legendDataBuilder.AppendFormat("{0},", row[c].ToString());
                                }
                                
                            }
                        }
                        legendDataBuilder.Append("|");

                    }
                    string strValue = legendDataBuilder.ToString().TrimEnd('|');
                    foreach(string str in strValue.Split('|'))
                    {
                        LegendData legendData = new LegendData()
                        {
                            DateString = str.TrimEnd(',').Split('+')[0],
                            LegendValue = str.TrimEnd(',').Split('+')[1]
                        };
                        legend.LegendDatas.Add(legendData);
                    }
                    context.Gathers.Add(entity);
                    context.SaveChanges();

                }
                
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
                throw new Exception(ex.Message);
            }
            
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

        protected void lnkDeleteFile_Command(object sender, CommandEventArgs e)
        {
            string filePath = e.CommandArgument.ToString().Split('|')[0];
            string id = e.CommandArgument.ToString().Split('|')[1];
            using(var context = new ChartContext())
            {
                int gatherId = int.Parse(id);
                var gather = context.Gathers.Find(gatherId);
                var legend = context.Legends.FirstOrDefault(a => a.GatherID == gatherId);
                var legendDatas = context.LegendDatas.Where(a => a.LegendID == legend.ID);
                context.LegendDatas.RemoveRange(legendDatas);
                context.Legends.Remove(legend);
                context.Gathers.Remove(gather);
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
                    MessageBox.ShowAndRedirect(this, "图表删除成功", Request.RawUrl);
                }
                catch (Exception)
                {
                    
                    throw;
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

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex);
        }
    }
}