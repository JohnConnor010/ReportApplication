// ***********************************************************************
// Copyright (c) 2015 JohnConnor,All rights reserved.
// Project:
// Assembly:WXOpinionApp.ReportManage
// Author:JohnConnor
// Created:4/24/2015 2:49:59 PM
// Description:
// ***********************************************************************
// Last Modified By:JOHNCONNOR-PC
// Last Modified On:4/24/2015 2:49:59 PM
// ***********************************************************************
using Aspose.Words;
using Aspose.Words.Saving;
using iNethinkCMS.Command;
using Newtonsoft.Json;
using ReportApplication;
using ReportApplication.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;
using System.Drawing;
using Aspose.Words.Drawing;
using System.Web.UI;
using System.Diagnostics;
using Aspose.Words.Tables;
using ReportApplication.Utilities;
using System.Data.Entity;

namespace ReportApplication.Manage
{
    public partial class GenerateReport : System.Web.UI.Page
    {
        public int hudong_number = 0;
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

        public void InitReportTypeDropDownList()
        {
            var items = CommonUtility.GetReportTypeByCategoryName("常规服务类型");
            this.ddlReportType.Items.Clear();
            this.ddlReportType.Items.Add(new ListItem { Text = "选择类型", Value = "0" });
            items.ForEach(item =>
            {
                this.ddlReportType.Items.Add(new ListItem { Text = item.Text, Value = item.Value });
            });
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
                this.ddlCustomerID.Items.Add(new ListItem { Text = "选择客户", Value = "0" });
                customers.ForEach(item =>
                {
                    ListItem listItem = new ListItem { Text = item.text, Value = item.value };
                    this.ddlCustomerID.Items.Add(listItem);
                });
            }
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            ListItem customerItem = this.ddlCustomerID.SelectedItem;
            ListItem reportItem = this.ddlReportType.SelectedItem;
            int customerId = int.Parse(customerItem.Value);
            int currentNumber = 0;
            int totalNumber = 0;
            string reportType = this.ddlReportType.SelectedValue;
            string news_ids = this.hideSelectedIDs.Value;
            using (var context = new ReportContext())
            {
                int reportTypeId = int.Parse(reportType);
                var entity = context.ReportNumbers.FirstOrDefault(a => a.CustomerID == customerId && a.ReportTypeId == reportTypeId);
                if (entity != null)
                {
                    currentNumber = entity.CurrentNumber.Value;
                    totalNumber = entity.TotalNumber.Value;
                }
            }
            switch (reportItem.Value)
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
            string reportType1 = string.Empty;
            switch(reportItem.Text)
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
            string Subsidiary = string.Empty;
            using (var context = new ReportContext())
            {
                var yesExists = context.Subsidiaries.Any(s => s.CustomerID == customerId);
                if (yesExists == true)
                {
                    Subsidiary = "及其权属企业";
                }

            }
            StringBuilder areaBuilder = new StringBuilder();
            using (var context = new ChartContext())
            {
                string pngPath = this.txtPath0.Text.Trim();
                if(!string.IsNullOrEmpty(pngPath))
                {
                    var gather = context.Gathers.FirstOrDefault(a => a.PngFilePath == pngPath);
                    if(gather != null)
                    {
                        var entities = gather.GatherQuantities;
                        var sensitives = entities.Where(a => a.SensitiveNumber > 0).Select(a => a.ChannelName);
                        int i = 0;
                        foreach(var entity in sensitives)
                        {
                            i++;
                            if(i == sensitives.Count())
                            {
                                areaBuilder.AppendFormat("和{0}", entity);
                            }
                            else
                            {
                                areaBuilder.AppendFormat("、{0}", entity);
                            }
                        }
                    }
                }
            }
            string area = areaBuilder.ToString().TrimStart('、');
            Document document = new Document(Server.MapPath("~/template_files/week_report.doc"));
            DocumentBuilder docBuilder = new DocumentBuilder(document);
            document.Range.Replace("{Company}", customerItem.Text, false, false);
            document.Range.Replace("{Subsidiary}", Subsidiary, false, false);
            document.Range.Replace("{ReportType}", reportItem.Text, false, false);
            document.Range.Replace("{area}", area, false, false);
            document.Range.Replace("{year}", DateTime.Now.ToString("yyyy"), false, false);
            document.Range.Replace("{month}", DateTime.Now.ToString("MM"), false, false);
            document.Range.Replace("{day}", DateTime.Now.ToString("dd"), false, false);
            document.Range.Replace("{number1}", currentNumber.ToString(), false, false);
            document.Range.Replace("{number2}", totalNumber.ToString(), false, false);
            document.Range.Replace("{ReportType1}", reportType1, false, false);
            document.Range.Replace("{TypeString}", CommonUtility.ReplaceReportType(reportItem.Text), false, false);
            document.Range.Replace("{aTypeString}", CommonUtility.ReplaceReportType(reportItem.Text).Replace("本", ""), false, false);
            int? totalNewsQuanity = 0;
            int? sensitiveNumber = 0;
            int? totalHudongCount = 0;
            int? totalMinganCount = 0;
            string strText = "均为正面或中性报道";
            using (var context = new ChartContext())
            {
                var entity = context.Gathers.FirstOrDefault(a => a.PngFilePath == this.txtPath0.Text);
                if (entity != null)
                {
                    var entities = entity.GatherQuantities;
                    totalNewsQuanity = entities.FirstOrDefault(a => a.ChannelName == "新闻").TotalNumber;
                    document.Range.Replace("{news_number}", totalNewsQuanity.ToString(), false, false);
                    sensitiveNumber = entities.FirstOrDefault(a => a.ChannelName == "新闻").SensitiveNumber;

                    if (sensitiveNumber > 0)
                    {
                        strText = String.Format("其中有{0}篇敏感报道。", sensitiveNumber);
                    }
                    document.Range.Replace("{strText}", strText, false, false);
                    totalHudongCount = entities.Where(a => a.ChannelName != "新闻").Select(a => a.TotalNumber).Sum();
                    document.Range.Replace("{hudong_number}", totalHudongCount.ToString(), false, false);
                    totalMinganCount = entities.Where(a => a.ChannelName != "新闻").Select(a => a.SensitiveNumber).Sum();
                    document.Range.Replace("{mingan_number}", totalMinganCount.ToString(), false, false);
                }
            }

            //-------------------
            document.Range.Replace(new Regex("{info_total_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath0.Text)), false);
            //本周重点敏感舆情、风险分析研判与处置建议
            using (var context = new ReportContext())
            {
                if (!string.IsNullOrEmpty(news_ids))
                {
                    var articleIds = news_ids.Split(',').Select(a => int.Parse(a));
                    var entities = context.Articles.Where(a => a.CustomerID == customerId && articleIds.Contains(a.ID)).OrderByDescending(a => a.Rating).Select(a => a);
                    int i = 0;
                    docBuilder.MoveToMergeField("Content");
                    foreach (var entity in entities)
                    {
                        i++;
                        if (i == 1)
                        {
                            //插入标题
                            docBuilder.Writeln();
                            docBuilder.Font.Name = "楷体_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln(String.Format("{0}.{1}", i, entity.Title));
                            docBuilder.Font.ClearFormatting();

                            //插入内容
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Writeln(entity.Content);
                            docBuilder.Font.ClearFormatting();


                            //插入链接
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Write("    网址链接：");
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Color = Color.Blue;
                            docBuilder.Font.Size = 10;
                            docBuilder.Font.Name = "仿宋";
                            docBuilder.Font.Underline = Underline.Single;
                            ParagraphFormat paragraphFormat = docBuilder.ParagraphFormat;
                            paragraphFormat.Alignment = ParagraphAlignment.Center;

                            docBuilder.InsertHyperlink(entity.Url, entity.Url, false);
                            paragraphFormat.ClearFormatting();
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Writeln();
                            docBuilder.Writeln(String.Format("    风险分析研判：{0}", entity.JudgeContent));
                            docBuilder.Write("    舆情星级：");
                            docBuilder.Font.ClearFormatting();

                            //插入舆情星级
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Color = Color.Red;
                            docBuilder.Writeln("★".Repeat(entity.Rating.Value));
                            docBuilder.Font.ClearFormatting();

                            //插入处置建议
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Write(String.Format("    处置建议：{0}", entity.SuggestContent)); ;
                            docBuilder.Font.ClearFormatting();
                        }
                        else
                        {
                            //插入标题
                            docBuilder.Writeln();
                            docBuilder.Font.Name = "楷体_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Bold = true;
                            docBuilder.Writeln("    " + String.Format("{0}.{1}", i, entity.Title));
                            docBuilder.Font.ClearFormatting();

                            //插入内容
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Writeln("    " + entity.Content);
                            docBuilder.Font.ClearFormatting();

                            //插入图片
                            if (!string.IsNullOrEmpty(entity.ScreenshotsPath))
                            {
                                FileInfo fileInfo = new FileInfo(Server.MapPath(entity.ScreenshotsPath));
                                if (fileInfo.Exists)
                                {
                                    docBuilder.InsertImage(Server.MapPath(entity.ScreenshotsPath), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);


                                }
                            }

                            //插入链接
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Write("    网址链接：");
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Color = Color.Blue;
                            docBuilder.Font.Size = 10;
                            docBuilder.Font.Name = "仿宋";
                            docBuilder.Font.Underline = Underline.Single;
                            ParagraphFormat paragraphFormat = docBuilder.ParagraphFormat;
                            paragraphFormat.Alignment = ParagraphAlignment.Center;

                            docBuilder.InsertHyperlink(entity.Url, entity.Url, false);
                            paragraphFormat.ClearFormatting();
                            docBuilder.Font.ClearFormatting();

                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Writeln();
                            docBuilder.Writeln(String.Format("    风险分析研判：{0}", entity.JudgeContent));
                            docBuilder.Write("    舆情星级：");
                            docBuilder.Font.ClearFormatting();

                            //插入舆情星级
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Font.Color = Color.Red;
                            docBuilder.Writeln("★".Repeat(entity.Rating.Value));
                            docBuilder.Font.ClearFormatting();

                            //插入处置建议
                            docBuilder.Font.Name = "仿宋_GB2312";
                            docBuilder.Font.Size = 16;
                            docBuilder.Write(String.Format("    处置建议：{0}", entity.SuggestContent)); ;
                            docBuilder.Font.ClearFormatting();
                        }


                    }
                }
            }

            docBuilder.MoveToMergeField("DistributeContent");
            docBuilder.Write(this.txtDistributeSummary.Text.Trim().Replace("#", ""));
            if (!string.IsNullOrEmpty(this.txtPath10.Text.Trim()))
            {
                docBuilder.InsertImage(Server.MapPath(txtPath10.Text.Trim()), RelativeHorizontalPosition.Default, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);
            }

            document.Range.Replace(new Regex("{NextWeekWarning_Pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath11.Text)), false);
            document.Range.Replace("{NextWeekMinganValue}", this.ddlNextWeekMinganValue.SelectedValue, false, false);
            document.Range.Replace("{NextWeekSustainNumber}", this.txtNextWeekSustainNumber.Text, false, false);
            docBuilder.MoveToMergeField("Questions");
            int b = 0;
            docBuilder.Font.Size = 16;
            docBuilder.Font.Name = "仿宋_GB2312";
            docBuilder.Writeln();
            foreach (ListItem item in this.lstWarningList.Items)
            {
                b++;
                if (item.Value != item.Text)
                {
                    string str = String.Format("{0}.{1}", b, item.Value);
                    docBuilder.Font.Size = 16;
                    docBuilder.Font.Name = "仿宋_GB2312";
                    docBuilder.Font.Bold = true;
                    docBuilder.Writeln(str);
                    docBuilder.Font.ClearFormatting();
                    if (b == this.lstWarningList.Items.Count)
                    {
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Write(item.Text.Trim());
                        docBuilder.Font.ClearFormatting();
                    }
                    else
                    {
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Writeln(item.Text.Trim());
                        docBuilder.Font.ClearFormatting();
                    }
                }
                else
                {
                    if (b == this.lstWarningList.Items.Count)
                    {
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Write(String.Format("{0}.{1}", b, item.Value));
                        docBuilder.Font.ClearFormatting();
                    }
                    else
                    {
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Writeln(String.Format("{0}.{1}", b, item.Value));
                        docBuilder.Font.ClearFormatting();
                    }
                }
            }

            document.Range.Replace(new Regex("{week_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath1.Text)), false);
            document.Range.Replace("{lowExponent}", this.txtLowExponent.Value, false, false);
            document.Range.Replace("{heightExponent}", this.txtHeightExponent.Value, false, false);
            document.Range.Replace("{avgExponent}", this.txtAvgExponent.Value, false, false);
            document.Range.Replace("{startMonth}", this.txtStartMonth.Value, false, false);
            document.Range.Replace("{startDay}", this.txtStartdDay.Value, false, false);
            document.Range.Replace("{lowNumber}", this.txtLowNumber.Value, false, false);
            document.Range.Replace("{endMonth}", this.txtEndMonth.Value, false, false);
            document.Range.Replace("{endDay}", this.txtEndDay.Value, false, false);
            document.Range.Replace("{heightNumber}", this.txtHeightNumber.Value, false, false);
            document.Range.Replace("{trend}", this.ddlTrend.Value, false, false);

            document.Range.Replace(new Regex("{attention_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath2.Text), 410, 307), false);
            document.Range.Replace("{total_AttentionNumber}", this.txtTotal_AttentionNumber.Value, false, false);
            document.Range.Replace("{mobile_AttentNumber}", this.txtMobile_AttentionNumber.Value, false, false);
            document.Range.Replace("{total_FloatingType}", this.ddlTotal_FloatingType.Value, false, false);
            document.Range.Replace("{total_AttentionPercent}", this.txtTotal_AttentionPercent.Value, false, false);
            document.Range.Replace("{mobile_FloatingType}", this.ddlMobile_FloatingType.Value, false, false);
            document.Range.Replace("{mobile_AttentionPercent}", this.txtMobile_AttentionPercent.Value, false, false);
            StringBuilder cityBuilder = new StringBuilder();
            foreach (ListItem item in this.lstAttentionCities.Items)
            {
                cityBuilder.AppendFormat("{0}、", item.Value);
            }
            string cities = cityBuilder.ToString().TrimEnd('、');
            document.Range.Replace("{cities}", cities, false, false);


            string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", customerItem.Text, reportItem.Text, currentNumber.ToString());

            StringBuilder questionsBuilder = new StringBuilder();
            foreach (ListItem item in this.lstWarningList.Items)
            {
                questionsBuilder.AppendFormat("{0},{1}|", item.Value, item.Text);
            }
            string questions = questionsBuilder.ToString().TrimEnd('|');
            using (var context = new ReportContext())
            {
                var report = new Report
                {
                    ReportName = fileName,
                    CustomerID = int.Parse(customerItem.Value),
                    Company = customerItem.Text,
                    ReportType = reportType,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM"),
                    Day = DateTime.Now.ToString("dd"),
                    CurrentNumber = currentNumber.ToString(),
                    TotalNumber = totalNumber.ToString(),
                    StrText = strText,
                    News_number = totalNewsQuanity,
                    Hudong_number = totalHudongCount,
                    Mingan_number = totalMinganCount,
                    Info_total_pic = this.txtPath0.Text,
                    PriorityArticles = this.hideSelectedIDs.Value,
                    DistributeSummary = this.txtDistributeSummary.Text.Replace("#", ""),
                    Distribute_Pic = this.txtPath10.Text,
                    NextWeekWarning_Pic = this.txtPath11.Text,
                    NextWeekMinganValue = this.ddlNextWeekMinganValue.SelectedValue,
                    NextWeekSustainNumber = int.Parse(this.txtNextWeekSustainNumber.Text),
                    AttentionQuestions = questions,
                    Week_Pic = this.txtPath1.Text,
                    LowExponent = int.Parse(this.txtLowExponent.Value),
                    HeightExponent = int.Parse(this.txtHeightExponent.Value),
                    AvgExponent = int.Parse(this.txtAvgExponent.Value),
                    StartMonth = int.Parse(this.txtStartMonth.Value),
                    StartDay = int.Parse(this.txtStartdDay.Value),
                    LowNumber = int.Parse(this.txtLowNumber.Value),
                    EndMonth = int.Parse(this.txtEndMonth.Value),
                    EndDay = int.Parse(this.txtEndDay.Value),
                    HeightNumber = int.Parse(this.txtHeightNumber.Value),
                    Tend = this.ddlTrend.Value,
                    Attention_Picture = this.txtPath2.Text,
                    Total_AttentionNumber = int.Parse(this.txtTotal_AttentionNumber.Value),
                    Mobile_AttentNumber = int.Parse(this.txtMobile_AttentionNumber.Value),
                    Total_FloatingType = this.ddlTotal_FloatingType.Value,
                    Mobile_FloatingType = this.ddlMobile_FloatingType.Value,
                    Total_AttentionPercent = this.txtTotal_AttentionPercent.Value,
                    Mobile_AttentionPercent = this.txtMobile_AttentionPercent.Value,
                    AttentionCities = cities,
                    AddDate = DateTime.Now,
                    Area = area,
                    Subsidiary = Subsidiary
                };
                context.Reports.Add(report);
                int reportTypeId = int.Parse(reportType);
                var entity = context.ReportNumbers.FirstOrDefault(a => a.CustomerID == customerId && a.ReportTypeId == reportTypeId);
                if (entity != null)
                {
                    entity.CurrentNumber++;
                    entity.TotalNumber++;
                    context.SaveChanges();
                }
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms, SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                Response.ContentType = "application/msword";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }


        }
        public void InitTrendChartDropDownList(int CustomerID)
        {
            using(var context = new ChartContext())
            {
                this.ddlTrendChart.Items.Clear();
                this.ddlTrendChart.Items.Add(new ListItem { Text = "选择走势图", Value = "" });
                var items = context.Gathers.Where(g => g.Flag == "Trend" && g.CustomerID == CustomerID ).OrderByDescending(g => g.ID).ToList().Select(g => new
                    {
                        text = Path.GetFileName(g.ExcelFilePath),
                        value = g.PngFilePath
                    });
                items.ForEach(item =>
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = item.text;
                        listItem.Value = item.value;
                        this.ddlTrendChart.Items.Add(listItem);
                    });
            }
        }
        public void InitGatherChartDropDownList(int CustomerID)
        {
            using (var context = new ChartContext())
            {
                this.ddlGatherChart.Items.Clear();
                this.ddlGatherChart.Items.Add(new ListItem { Text = "选择汇总图", Value = "" });
                var items = context.Gathers.Where(g => g.Flag == "gather" && g.CustomerID == CustomerID).OrderByDescending(g => g.ID).ToList().Select(g => new
                    {
                        text = Path.GetFileName(g.ExcelFilePath),
                        value = g.PngFilePath
                    });
                items.ForEach(item =>
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = item.text;
                    listItem.Value = item.value;
                    this.ddlGatherChart.Items.Add(listItem);
                });
            }

        }
        
        [WebMethod]
        public static string GetNewsById(string ids)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(ids))
            {
                var articleIds = ids.Split(',').Select(a => int.Parse(a));
                using (var context = new ReportContext())
                {
                    var papers = context.Articles.Where(p => articleIds.Contains(p.ID)).OrderByDescending(p => p.ID).ToList().Select(p => new
                    {
                        p.ID,
                        p.Title,
                        p.Site,
                        ChannelType = CommonUtility.GetChannelName(p.ChannelType)
                    });
                    json = JsonConvert.SerializeObject(papers);
                }
            }
            else
            {
                json = string.Empty;
            }
            return json;
        }

        [WebMethod]
        public static string GetArticleById(string ids)
        {
            string json = string.Empty;
            if(!string.IsNullOrEmpty(ids))
            {
                var articleIds = ids.Split(',').Select(a => int.Parse(a));
                using (var context = new PaperContext())
                {
                    var papers = context.Papers.Where(p => articleIds.Contains(p.PaperID)).OrderByDescending(p => p.PaperID).ToList().Select(p => new
                        {
                            p.PaperID,
                            p.Title,
                            p.FirstSite,
                            p.PaperPublishedDate,
                            p.ReprintCount
                        });
                    json = JsonConvert.SerializeObject(papers);
                }
            }
            else
            {
                json = string.Empty;
            }
            return json;
        }

        protected void ddlCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CustomerID = int.Parse(ddlCustomerID.SelectedValue);
            InitTrendChartDropDownList(CustomerID);
            InitGatherChartDropDownList(CustomerID);
            InitWeekTrendChartDropDownList(CustomerID);
            this.txtPath0.Text = string.Empty;
            this.ddlGatherChart.SelectedIndex = -1;
            this.div_pic0.Visible = false;
        }

        protected void btnAddCity_ServerClick(object sender, EventArgs e)
        {
            string city = this.txtAttentionCity.Text.Trim();
            
            if(!string.IsNullOrEmpty(city))
            {
                var item = this.lstAttentionCities.Items.FindByValue(city);
                if(item == null)
                {
                    item = new ListItem { Text = city, Value = city };
                    this.lstAttentionCities.Items.Add(item);
                    this.lstAttentionCities.SelectedIndex = -1;
                    this.txtAttentionCity.Text = string.Empty;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "a", "alert('此地区已经存在');$('#txtAttentionCity').focus();", true);
                }
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "a", "alert('请输入分布地区');$('#txtAttentionCity').focus();", true);
            }
            
        }

        protected void btnDeleteCity_ServerClick(object sender, EventArgs e)
        {
            string city = this.txtAttentionCity.Text;
            var item = this.lstAttentionCities.Items.FindByValue(city);
            if(item != null)
            {
                this.lstAttentionCities.Items.Remove(item);
                this.lstAttentionCities.SelectedIndex = -1;
            }
        }

        protected void lstAttentionCities_SelectedIndexChanged(object sender, EventArgs e)
        {
            string city = this.lstAttentionCities.SelectedValue;
            this.txtAttentionCity.Text = city;
        }

        

        protected void ddlTrendChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.ddlTrendChart.SelectedValue != "")
            {
                this.txtPath1.Text = this.ddlTrendChart.SelectedValue;
                this.img_TrendChart.ImageUrl = this.ddlTrendChart.SelectedValue;
                this.pic_div1.Visible = true;
                using (var context = new ChartContext())
                {
                    var entity = context.Gathers.FirstOrDefault(g => g.PngFilePath == this.ddlTrendChart.SelectedValue);
                    if (entity != null)
                    {
                        var entities = context.Entry(entity).Collection(x => x.GatherTrends).Query().OrderByDescending(a => a.Number).ToList();
                        var firstElement = entities.First();
                        var lastElement = entities.Last();
                        var avgValue = Math.Round(entities.Average(a => a.Number.Value));
                        this.txtLowExponent.Value = lastElement.Number.ToString();
                        this.txtHeightExponent.Value = firstElement.Number.ToString();
                        this.txtAvgExponent.Value = avgValue.ToString();
                        var startMonth = lastElement.Name.ToDateTime().Month;
                        this.txtStartMonth.Value = startMonth.ToString();
                        var startDay = lastElement.Name.ToDateTime().Day;
                        this.txtStartdDay.Value = startDay.ToString();
                        this.txtLowNumber.Value = lastElement.Number.Value.ToString();
                        var endMonth = firstElement.Name.ToDateTime().Month;
                        this.txtEndMonth.Value = endMonth.ToString();
                        var endDay = firstElement.Name.ToDateTime().Day;
                        this.txtEndDay.Value = endDay.ToString();
                        this.txtHeightNumber.Value = firstElement.Number.Value.ToString();
                        if (avgValue <= 10)
                        {
                            this.ddlTrend.Value = "平稳";
                        }
                        else
                        {
                            this.ddlTrend.Value = "波动";
                        }

                    }
                }
            }
            else
            {
                this.txtPath1.Text = string.Empty;
                this.img_TrendChart.ImageUrl = null;
                this.pic_div1.Visible = false;
                this.txtLowExponent.Value = string.Empty;
                this.txtHeightExponent.Value = string.Empty;
                this.txtAvgExponent.Value = string.Empty;
                this.txtStartMonth.Value = string.Empty;
                this.txtStartdDay.Value = string.Empty;
                this.txtLowNumber.Value = string.Empty;
                this.txtEndMonth.Value = string.Empty;
                this.txtEndDay.Value = string.Empty;
                this.txtHeightNumber.Value = string.Empty;
            }
            
        }

        protected void ddlGatherChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.ddlGatherChart.SelectedValue))
            {
                this.txtPath0.Text = this.ddlGatherChart.SelectedValue;
                this.img_GatherChart.ImageUrl = this.ddlGatherChart.SelectedValue;
                this.div_pic0.Visible = true;
                using (var context = new ChartContext())
                {
                    var entity = context.Gathers.FirstOrDefault(a => a.PngFilePath == this.ddlGatherChart.SelectedValue);
                    if(entity != null)
                    {
                        var entities = entity.GatherQuantities;
                        var totalNewsCount = entities.FirstOrDefault(a => a.ChannelName == "新闻").TotalNumber;
                        var totalMinganSumCount = entities.Select(a => a.SensitiveNumber).Sum();
                        var entities1 = entities.Where(a => a.SensitiveNumber != 0).Select(a => new
                        {
                            a.SensitiveNumber,
                            a.ChannelName
                        });
                        StringBuilder sBuilder = new StringBuilder();
                        foreach(var entity1 in entities1)
                        {
                            sBuilder.AppendFormat("{0}{1}条，", entity1.ChannelName, entity1.SensitiveNumber);
                        }
                        string builderString = sBuilder.ToString().ReplaceLast("，", "。");
                        string text = String.Format("{0}，{1}敏感舆情数量共计{2}条，#均出现在互动类媒体平台，分布在{3}#", CommonUtility.ReplaceReportType(this.ddlReportType.SelectedItem.Text), this.ddlCustomerID.SelectedItem.Text, totalMinganSumCount,builderString);

                        this.txtDistributeSummary.Text = text;
                    }
                }

            }
        }

        protected void btnAddWarning_Click(object sender, EventArgs e)
        {
            string title = this.txtNextWarningTitle.Text.Trim();
            string content = this.txtNextWarningContent.Text.Trim();
            if (this.hideAction.Value == "add")
            {

                ListItem item = new ListItem();
                item.Text = content;
                if(this.chkIsShowTitle.Checked)
                {
                    item.Value = title;
                }
                else
                {
                    item.Value = content;
                }
                if (this.lstWarningList.Items.Contains(item))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel6_5, GetType(), "alert", "alert('此项已经添加')", true);
                    return;
                }
                this.lstWarningList.Items.Add(item);

                this.lstWarningList.SelectedIndex = -1;
                this.txtNextWarningTitle.Text = string.Empty;
                this.txtNextWarningContent.Text = string.Empty;
            }
            if (this.hideAction.Value == "edit")
            {
                ListItem item = this.lstWarningList.SelectedItem;
                item.Text = content;
                if (this.chkIsShowTitle.Checked)
                {
                    item.Value = title;
                }
                else
                {
                    item.Value = content;
                }
                this.lstWarningList.SelectedIndex = -1;
                this.txtNextWarningTitle.Text = string.Empty;
                this.txtNextWarningContent.Text = string.Empty;
            }
        }

        protected void lstWarningList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = this.lstWarningList.SelectedItem;
            this.hideAction.Value = "edit";
            this.txtNextWarningTitle.Text = string.Empty;
            this.txtNextWarningContent.Text = string.Empty;
            if (selectedItem != null)
            {
                this.txtNextWarningContent.Text = selectedItem.Text;
                if (selectedItem.Value != selectedItem.Text)
                {
                    this.chkIsShowTitle.Checked = true;
                    this.txtNextWarningTitle.Text = selectedItem.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel6_5, GetType(), "show", "$('#UpdatePanel6_2').show();", true);
                }
                else
                {
                    this.chkIsShowTitle.Checked = false;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel6_5, GetType(), "show1", "$('#UpdatePanel6_2').hide();", true);
                }
            }
        }

        protected void btnDeleteWarning_Click(object sender, EventArgs e)
        {
            ListItem item = this.lstWarningList.SelectedItem;
            if(item == null)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel6_5, GetType(), "alert", "alert('请选择删除项');$('#lstWarningList').focus();", true);
                return;
            }
            this.lstWarningList.Items.Remove(item);
            this.txtNextWarningTitle.Text = string.Empty;
            this.txtNextWarningContent.Text = string.Empty;
            this.lstWarningList.SelectedIndex = -1;
        }
        public void InitWeekTrendChartDropDownList(int CustomerID)
        {
            using (var context = new ChartContext())
            {
                var items = context.Gathers.Where(g => g.Flag == "Trend" && g.CustomerID == CustomerID).OrderByDescending(g => g.ID).ToList().Select(g => new
                {
                    value = g.PngFilePath,
                    text = Path.GetFileName(g.ExcelFilePath)
                });
                this.ddlWeekTrendChart.Items.Clear();
                this.ddlWeekTrendChart.Items.Add(new ListItem { Text = "选择走势图", Value = "" });
                items.ForEach(item =>
                {
                    this.ddlWeekTrendChart.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }

        protected void ddlWeekTrendChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.ddlWeekTrendChart.SelectedValue != "")
            {
                this.txtPath11.Text = this.ddlWeekTrendChart.SelectedValue;
                this.img_WeekChart.ImageUrl = this.ddlWeekTrendChart.SelectedValue;
                this.prev_weekchart.Visible = true;
            }
            else
            {
                this.txtPath11.Text = string.Empty;
                this.img_WeekChart.ImageUrl = string.Empty;
                this.prev_weekchart.Visible = false;
            }
            
        }

        protected void btnConvertToJpg_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.txtPath2.Text))
            {
                string path = this.txtPath2.Text.Trim(); 
                string filePath = Server.MapPath(path);
                using (Aspose.Slides.Presentation presentation = new Aspose.Slides.Presentation(filePath))
                {
                    Aspose.Slides.ISlide slide = presentation.Slides[0];
                    Bitmap bitmap = slide.GetThumbnail(1f, 1f);
                    string jpgPath = path.Replace(".ppt", "").Replace(".pptx", "") + ".jpg";
                    bitmap.Save(Server.MapPath(jpgPath), System.Drawing.Imaging.ImageFormat.Jpeg);
                    this.prev_pic2.ImageUrl = jpgPath;
                    this.txtPath2.Text = jpgPath;
                    ScriptManager.RegisterStartupScript(this.pic_div2, GetType(), "show", "$('#pic_div2').show();", true);
                }
            }
        }
    }
}