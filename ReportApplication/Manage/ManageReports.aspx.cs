using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Saving;
using Aspose.Words.Tables;
using iNethinkCMS.Command;
using Microsoft.AspNet.Identity.EntityFramework;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public partial class ManageReports : System.Web.UI.Page
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
            
            
        }

        private void InitRepeater1(int currentPageIndex = 1,int customerId = 0,string reportType = "0",string addDate = "")
        {
            using(var context = new ReportContext())
            {
                var entities = context.Reports.OrderByDescending(a => a.ID).ToList();
                if(customerId != 0)
                {
                    entities = entities.Where(a => a.CustomerID == customerId).ToList();
                }
                if(reportType != "0")
                {
                    entities = entities.Where(a => a.ReportType == reportType).ToList();
                }
                if(!string.IsNullOrEmpty(addDate))
                {
                    entities = entities.Where(a => a.AddDate == DateTime.Parse(addDate)).ToList();
                }
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.Repeater1.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.Repeater1.DataBind();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), this.ddlReportType.SelectedValue, this.txtDate.Value);
            
        }
        public string GetTypeNameById(int id)
        {
            using(var context = new DataSettingContext())
            {
                var entity = context.DataServiceCategories.Find(id);
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
            var items = CommonUtility.GetReportTypeByCategoryName("常规服务类型");
            this.ddlReportType.Items.Clear();
            this.ddlReportType.Items.Add(new ListItem { Text = "选择类型", Value = "0" });
            items.ForEach(item =>
                {
                    this.ddlReportType.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
                });
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), this.ddlReportType.SelectedValue, this.txtDate.Value);
        }

        protected void lnkGenerateReport_Command(object sender, CommandEventArgs e)
        {
            CreateWordDocument(e);
        }

        private void CreateWordDocument(CommandEventArgs e)
        {
            var id = int.Parse(e.CommandArgument.ToString());
            using (var context = new ReportContext())
            {
                var entity = context.Reports.Find(id);
                if (entity != null)
                {
                    var customerId = context.Customers.FirstOrDefault(c => c.Name == entity.Company).ID;
                    Document document = new Document(Server.MapPath("~/template_files/week_report.doc"));
                    DocumentBuilder docBuilder = new DocumentBuilder(document);                    
                    document.Range.Replace("{Company}", entity.Company, false, false);
                    document.Range.Replace("{ReportType}", GetTypeNameById(entity.ReportType.ToInt32()), false, false);
                    document.Range.Replace("{year}", entity.Year.Trim(), false, false);
                    string reportType1 = string.Empty;
                    switch (GetTypeNameById(entity.ReportType.ToInt32()))
                    {
                        case "日报":
                            reportType1 = "一日";
                            break;
                        case "周报":
                            reportType1 = "一周";
                            break;
                        case "旬报":
                            reportType1 = "旬度";
                            break;
                        case "半月报":
                            reportType1 = "半月";
                            break;
                        case "月报":
                            reportType1 = "月度";
                            break;
                    }
                    document.Range.Replace("{month}", entity.Month.Trim(), false, false);
                    document.Range.Replace("{Subsidiary}", entity.Subsidiary ?? "", false, false);
                    document.Range.Replace("{area}", entity.Area ?? "", false, false);
                    document.Range.Replace("{day}", entity.Day.Trim(), false, false);
                    document.Range.Replace("{number1}", entity.CurrentNumber.Trim(), false, false);
                    document.Range.Replace("{number2}", entity.TotalNumber.Trim(), false, false);
                    document.Range.Replace("{ReportType1}", reportType1, false, false);
                    document.Range.Replace("{TypeString}", CommonUtility.ReplaceReportType(GetTypeNameById(entity.ReportType.ToInt32())), false, false);
                    document.Range.Replace("{aTypeString}", CommonUtility.ReplaceReportType(GetTypeNameById(entity.ReportType.ToInt32())).Replace("本", ""), false, false);

                    document.Range.Replace("{news_number}", entity.News_number.ToString(), false, false);
                    document.Range.Replace("{hudong_number}", entity.Hudong_number.ToString(), false, false);
                    document.Range.Replace("{mingan_number}", entity.Mingan_number.ToString(), false, false);
                    document.Range.Replace("{strText}", entity.StrText, false, false);
                    document.Range.Replace(new Regex("{info_total_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Info_total_pic)), false);
                    //本周重点敏感舆情、风险分析研判与处置建议
                    if (!string.IsNullOrEmpty(entity.PriorityArticles))
                    {
                        var articleIds = entity.PriorityArticles.Split(',').Select(a => int.Parse(a));
                        var entities = context.Articles.Where(a => a.CustomerID == customerId && articleIds.Contains(a.ID)).OrderByDescending(a => a.ID).Select(a => a);
                        int i = 0;
                        docBuilder.MoveToMergeField("Content");
                        foreach (var article in entities)
                        {
                            i++;
                            //插入标题
                            docBuilder.Writeln();
                            docBuilder.Font.Name = "楷体_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln(String.Format("{0}.{1}", i, article.Title));
                            docBuilder.Font.ClearFormatting();

                            //插入内容
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Writeln(article.Content);
                            docBuilder.Font.ClearFormatting();

                            //插入图片
                            if (!string.IsNullOrEmpty(article.ScreenshotsPath))
                            {
                                FileInfo fileInfo = new FileInfo(Server.MapPath(article.ScreenshotsPath));
                                if (fileInfo.Exists)
                                {
                                    docBuilder.InsertImage(Server.MapPath(article.ScreenshotsPath), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);
                                }
                            }

                            //插入链接
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Write("网址链接：");
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Color = Color.Blue;
                            docBuilder.Font.Size = 10;
                            docBuilder.Font.Name = "仿宋";
                            docBuilder.Font.Underline = Underline.Single;
                            docBuilder.InsertHyperlink(article.Url, article.Url, false);
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Writeln();
                            docBuilder.Writeln(String.Format("风险分析研判：{0}", article.JudgeContent));
                            docBuilder.Write("舆情星级：");
                            docBuilder.Font.ClearFormatting();

                            //插入舆情星级
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Color = Color.Red;
                            docBuilder.Writeln("★".Repeat(article.Rating.Value));
                            docBuilder.Font.ClearFormatting();

                            //插入处置建议
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Write(String.Format("处置建议：{0}", article.SuggestContent)); ;
                            docBuilder.Font.ClearFormatting();


                        }
                    }

                    docBuilder.MoveToMergeField("DistributeContent");
                    docBuilder.Write(entity.DistributeSummary.Replace("#", ""));
                    if (!string.IsNullOrEmpty(entity.Distribute_Pic))
                    {
                        docBuilder.InsertImage(Server.MapPath(entity.Distribute_Pic), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);
                    }

                    document.Range.Replace(new Regex("{NextWeekWarning_Pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.NextWeekWarning_Pic)), false);
                    document.Range.Replace("{NextWeekMinganValue}", entity.NextWeekMinganValue, false, false);
                    document.Range.Replace("{NextWeekSustainNumber}", entity.NextWeekSustainNumber.Value.ToString(), false, false);
                    docBuilder.MoveToMergeField("Questions");
                    int b = 0;
                    docBuilder.Writeln();
                    foreach (var item in entity.AttentionQuestions.Split('|'))
                    {
                        b++;
                        if (item.Split(',')[0] != item.Split(',')[1])
                        {
                            string str = String.Format("{0}.{1}", b, item.Split(',')[0]);
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln(str);
                            docBuilder.Font.ClearFormatting();
                            if (b == entity.AttentionQuestions.Split('|').Count())
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Write(item.Split(',')[1].Trim());
                                docBuilder.Font.ClearFormatting();
                            }
                            else
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Writeln(item.Split(',')[1].Trim());
                                docBuilder.Font.ClearFormatting();
                            }
                        }
                        else
                        {
                            if (b == entity.AttentionQuestions.Split('|').Count())
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Write(String.Format("{0}.{1}", b, item.Split(',')[1]));
                                docBuilder.Font.ClearFormatting();
                            }
                            else
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Writeln(String.Format("{0}.{1}", b, item.Split(',')[1]));
                                docBuilder.Font.ClearFormatting();
                            }
                        }
                    }

                    document.Range.Replace(new Regex("{week_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Week_Pic)), false);
                    document.Range.Replace("{lowExponent}", entity.LowExponent.ToString(), false, false);
                    document.Range.Replace("{heightExponent}", entity.HeightExponent.Value.ToString(), false, false);
                    document.Range.Replace("{avgExponent}", entity.AvgExponent.Value.ToString(), false, false);
                    document.Range.Replace("{startMonth}", entity.StartMonth.Value.ToString(), false, false);
                    document.Range.Replace("{startDay}", entity.StartDay.Value.ToString(), false, false);
                    document.Range.Replace("{lowNumber}", entity.LowNumber.Value.ToString(), false, false);
                    document.Range.Replace("{endMonth}", entity.EndMonth.Value.ToString(), false, false);
                    document.Range.Replace("{endDay}", entity.EndDay.Value.ToString(), false, false);
                    document.Range.Replace("{heightNumber}", entity.HeightNumber.Value.ToString(), false, false);
                    document.Range.Replace("{trend}", entity.Tend, false, false);

                    
                    
                    document.Range.Replace(new Regex("{attention_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Attention_Picture)), false);
                    document.Range.Replace("{total_AttentionNumber}", entity.Total_AttentionNumber.Value.ToString(), false, false);
                    document.Range.Replace("{mobile_AttentNumber}", entity.Mobile_AttentNumber.Value.ToString(), false, false);
                    document.Range.Replace("{total_FloatingType}", entity.Total_FloatingType, false, false);
                    document.Range.Replace("{total_AttentionPercent}", entity.Total_AttentionPercent, false, false);
                    document.Range.Replace("{mobile_FloatingType}", entity.Mobile_FloatingType, false, false);
                    document.Range.Replace("{mobile_AttentionPercent}", entity.Mobile_AttentionPercent, false, false);
                    StringBuilder cityBuilder = new StringBuilder();
                    foreach (var item in entity.AttentionCities.Split("、"))
                    {
                        cityBuilder.AppendFormat("{0}、", item);
                    }
                    string cities = cityBuilder.ToString().TrimEnd('、');
                    document.Range.Replace("{cities}", cities, false, false);

                    
                    string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", entity.Company, GetTypeNameById(entity.ReportType.ToInt32()), entity.CurrentNumber);


                    using (MemoryStream ms = new MemoryStream())
                    {
                        document.Save(ms, SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                        Response.ContentType = "application/msword";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.BinaryWrite(ms.ToArray());
                        Response.End();
                    }
                }
            }
        }
        private void CreatePDFDocument(CommandEventArgs e)
        {
            var id = int.Parse(e.CommandArgument.ToString());
            using (var context = new ReportContext())
            {
                var entity = context.Reports.Find(id);
                if (entity != null)
                {
                    var customerId = context.Customers.FirstOrDefault(c => c.Name == entity.Company).ID;
                    Document document = new Document(Server.MapPath("~/template_files/week_report.doc"));
                    DocumentBuilder docBuilder = new DocumentBuilder(document);
                    document.Range.Replace("{Company}", entity.Company, false, false);
                    document.Range.Replace("{ReportType}", GetTypeNameById(entity.ReportType.ToInt32()), false, false);
                    string reportType1 = string.Empty;
                    switch (GetTypeNameById(entity.ReportType.ToInt32()))
                    {
                        case "日报":
                            reportType1 = "一日";
                            break;
                        case "周报":
                            reportType1 = "一周";
                            break;
                        case "旬报":
                            reportType1 = "旬度";
                            break;
                        case "半月报":
                            reportType1 = "半月";
                            break;
                        case "月报":
                            reportType1 = "月度";
                            break;
                    }
                    
                    document.Range.Replace("{year}", entity.Year.Trim(), false, false);
                    document.Range.Replace("{month}", entity.Month.Trim(), false, false);
                    document.Range.Replace("{Subsidiary}", entity.Subsidiary ?? "", false, false);
                    document.Range.Replace("{area}", entity.Area ?? "", false, false);
                    document.Range.Replace("{day}", entity.Day.Trim(), false, false);
                    document.Range.Replace("{number1}", entity.CurrentNumber.Trim(), false, false);
                    document.Range.Replace("{number2}", entity.TotalNumber.Trim(), false, false);
                    document.Range.Replace("{ReportType1}", reportType1, false, false);
                    document.Range.Replace("{TypeString}", CommonUtility.ReplaceReportType(GetTypeNameById(entity.ReportType.ToInt32())), false, false);
                    document.Range.Replace("{aTypeString}", CommonUtility.ReplaceReportType(GetTypeNameById(entity.ReportType.ToInt32())).Replace("本", ""), false, false);

                    document.Range.Replace("{news_number}", entity.News_number.ToString(), false, false);
                    document.Range.Replace("{hudong_number}", entity.Hudong_number.ToString(), false, false);
                    document.Range.Replace("{mingan_number}", entity.Mingan_number.ToString(), false, false);
                    document.Range.Replace("{strText}", entity.StrText, false, false);
                    document.Range.Replace(new Regex("{info_total_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Info_total_pic)), false);
                    //本周重点敏感舆情、风险分析研判与处置建议
                    if (!string.IsNullOrEmpty(entity.PriorityArticles))
                    {
                        var articleIds = entity.PriorityArticles.Split(',').Select(a => int.Parse(a));
                        var entities = context.Articles.Where(a => a.CustomerID == customerId && articleIds.Contains(a.ID)).OrderByDescending(a => a.ID).Select(a => a);
                        int i = 0;
                        docBuilder.MoveToMergeField("Content");
                        foreach (var article in entities)
                        {
                            i++;
                            //插入标题
                            docBuilder.Writeln();
                            docBuilder.Font.Name = "楷体_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln(String.Format("{0}.{1}", i, article.Title));
                            docBuilder.Font.ClearFormatting();

                            //插入内容
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Writeln(article.Content);
                            docBuilder.Font.ClearFormatting();

                            //插入图片
                            if (!string.IsNullOrEmpty(article.ScreenshotsPath))
                            {
                                FileInfo fileInfo = new FileInfo(Server.MapPath(article.ScreenshotsPath));
                                if (fileInfo.Exists)
                                {
                                    docBuilder.InsertImage(Server.MapPath(article.ScreenshotsPath), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);
                                }
                            }

                            //插入链接
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Write("网址链接：");
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Color = Color.Blue;
                            docBuilder.Font.Size = 10;
                            docBuilder.Font.Name = "仿宋";
                            docBuilder.Font.Underline = Underline.Single;
                            docBuilder.InsertHyperlink(article.Url, article.Url, false);
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Writeln();
                            docBuilder.Writeln(String.Format("风险分析研判：{0}", article.JudgeContent));
                            docBuilder.Write("舆情星级：");
                            docBuilder.Font.ClearFormatting();

                            //插入舆情星级
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Color = Color.Red;
                            docBuilder.Writeln("★".Repeat(article.Rating.Value));
                            docBuilder.Font.ClearFormatting();

                            //插入处置建议
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Write(String.Format("处置建议：{0}", article.SuggestContent)); ;
                            docBuilder.Font.ClearFormatting();


                        }
                    }

                    docBuilder.MoveToMergeField("DistributeContent");
                    docBuilder.Write(entity.DistributeSummary.Replace("#", ""));
                    if (!string.IsNullOrEmpty(entity.Distribute_Pic))
                    {
                        docBuilder.InsertImage(Server.MapPath(entity.Distribute_Pic), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);
                    }

                    document.Range.Replace(new Regex("{NextWeekWarning_Pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.NextWeekWarning_Pic)), false);
                    document.Range.Replace("{NextWeekMinganValue}", entity.NextWeekMinganValue, false, false);
                    document.Range.Replace("{NextWeekSustainNumber}", entity.NextWeekSustainNumber.Value.ToString(), false, false);
                    docBuilder.MoveToMergeField("Questions");
                    int b = 0;
                    docBuilder.Writeln();
                    foreach (var item in entity.AttentionQuestions.Split('|'))
                    {
                        b++;
                        if (item.Split(',')[0] != item.Split(',')[1])
                        {
                            string str = String.Format("{0}.{1}", b, item.Split(',')[0]);
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln(str);
                            docBuilder.Font.ClearFormatting();
                            if (b == entity.AttentionQuestions.Split('|').Count())
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Write(item.Split(',')[1].Trim());
                                docBuilder.Font.ClearFormatting();
                            }
                            else
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Writeln(item.Split(',')[1].Trim());
                                docBuilder.Font.ClearFormatting();
                            }
                        }
                        else
                        {
                            if (b == entity.AttentionQuestions.Split('|').Count())
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Write(String.Format("{0}.{1}", b, item.Split(',')[1]));
                                docBuilder.Font.ClearFormatting();
                            }
                            else
                            {
                                docBuilder.Font.Size = 16;
                                docBuilder.Font.Name = "仿宋_GB2312";
                                docBuilder.Writeln(String.Format("{0}.{1}", b, item.Split(',')[1]));
                                docBuilder.Font.ClearFormatting();
                            }
                        }
                    }

                    document.Range.Replace(new Regex("{week_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Week_Pic)), false);
                    document.Range.Replace("{lowExponent}", entity.LowExponent.ToString(), false, false);
                    document.Range.Replace("{heightExponent}", entity.HeightExponent.Value.ToString(), false, false);
                    document.Range.Replace("{avgExponent}", entity.AvgExponent.Value.ToString(), false, false);
                    document.Range.Replace("{startMonth}", entity.StartMonth.Value.ToString(), false, false);
                    document.Range.Replace("{startDay}", entity.StartDay.Value.ToString(), false, false);
                    document.Range.Replace("{lowNumber}", entity.LowNumber.Value.ToString(), false, false);
                    document.Range.Replace("{endMonth}", entity.EndMonth.Value.ToString(), false, false);
                    document.Range.Replace("{endDay}", entity.EndDay.Value.ToString(), false, false);
                    document.Range.Replace("{heightNumber}", entity.HeightNumber.Value.ToString(), false, false);
                    document.Range.Replace("{trend}", entity.Tend, false, false);

                    
                    
                    document.Range.Replace(new Regex("{attention_pic}"), new ReplaceAndInsertImage(Server.MapPath(entity.Attention_Picture)), false);
                    document.Range.Replace("{total_AttentionNumber}", entity.Total_AttentionNumber.Value.ToString(), false, false);
                    document.Range.Replace("{mobile_AttentNumber}", entity.Mobile_AttentNumber.Value.ToString(), false, false);
                    document.Range.Replace("{total_FloatingType}", entity.Total_FloatingType, false, false);
                    document.Range.Replace("{total_AttentionPercent}", entity.Total_AttentionPercent, false, false);
                    document.Range.Replace("{mobile_FloatingType}", entity.Mobile_FloatingType, false, false);
                    document.Range.Replace("{mobile_AttentionPercent}", entity.Mobile_AttentionPercent, false, false);
                    StringBuilder cityBuilder = new StringBuilder();
                    foreach (var item in entity.AttentionCities.Split("、"))
                    {
                        cityBuilder.AppendFormat("{0}、", item);
                    }
                    string cities = cityBuilder.ToString().TrimEnd('、');
                    document.Range.Replace("{cities}", cities, false, false);

                    
                    string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", entity.Company, GetTypeNameById(entity.ReportType.ToInt32()), entity.CurrentNumber);
                    DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/Download/专报/" + entity.Company));
                    if(!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    string savePath = directoryInfo.FullName;
                    string saveName = Path.Combine(savePath,fileName);
                    document.Save(saveName);
                    
                    string pdfFileName = String.Format("{0}网络舆情{1}第{2}期.pdf", entity.Company, GetTypeNameById(entity.ReportType.ToInt32()), entity.CurrentNumber);
                    string pdfSavePath = Path.Combine(savePath, pdfFileName);
                    OfficeUtilities.DOC2PDF(saveName, pdfSavePath);
                    using(FileStream fileStream = new FileStream(pdfSavePath,FileMode.Open))
                    {
                        byte[] buffer = fileStream.ToByteArray();
                        Response.Clear();
                        Response.AddHeader("Content-Length", fileStream.Length.ToString());
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "inline;FileName=" + pdfFileName);
                        fileStream.Close();
                        fileStream.Dispose();
                        Response.BinaryWrite(buffer);
                        Response.End();
                        
                    }

                    
                }
            }
        }
        protected void lnkDeleteReport_Command(object sender, CommandEventArgs e)
        {
            int id = e.CommandArgument.ToInt32();
            using(var context = new ReportContext())
            {
                var entity = context.Reports.Find(id);
                if(entity != null)
                {
                    context.Reports.Remove(entity);
                    context.SaveChanges();
                    InitRepeater1(this.AspNetPager1.CurrentPageIndex, int.Parse(this.ddlCustomer.SelectedValue), this.ddlReportType.SelectedValue, this.txtDate.Value);
                    MessageBox.ShowAndRedirect(this, "alert('专报删除成功')", Request.RawUrl);
                }
            }
        }

        protected void lnkExportPDF_Command(object sender, CommandEventArgs e)
        {
            CreatePDFDocument(e);
        }
    }
    
}