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

namespace WXOpinionApp.ReportManage
{
    public partial class EditReport : System.Web.UI.Page
    {
        public int news_r = 0;  //新闻正面或中性
        public int news_n = 0;  //新闻负面
        public int tieba_r = 0;
        public int tieba_n = 0;
        public int bbs_r = 0;
        public int bbs_n = 0;
        public int weibo_r = 0;
        public int weibo_n = 0;
        public int weixin_r = 0;
        public int weixin_n = 0;
        public int video_r = 0;
        public int video_n = 0;
        public int blog_r = 0;
        public int blog_n = 0;
        public int hudong_number = 0;
        protected string array = string.Empty;
        protected string array1 = string.Empty;
        public int ReportId
        {
            get
            {
                return Request.QueryString["id"].ToInt32();
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
            using(var context = new ReportContext())
            {
                var entity = context.Reports.Find(ReportId);
                if (entity != null)
                {
                    var customerId = context.Customers.FirstOrDefault(c => c.Name == entity.Company).ID;
                    this.ddlCustomer.SelectedValue = customerId.ToString();
                    this.ddlReportType.SelectedValue = entity.ReportType;
                    this.txtPath0.Text = entity.Info_total_pic;

                    this.hideSelectedIDs.Value = entity.PriorityArticles;
                    array = entity.PriorityArticles;
                    if(!string.IsNullOrEmpty(entity.PriorityArticles))
                    {
                        var articles = context.Articles.ToList().OrderByDescending(a => a.ID).Where(a => entity.PriorityArticles.Split(',').Contains(a.ID.ToString())).Select(a => new
                            {
                                a.ID,
                                a.Title,
                                a.Site,
                                ChannelType = CommonUtility.GetChannelName(a.ChannelType),
                                a.ReplyCount,
                                AddDate = a.AddDate.Value.ToString("MM月dd日")
                            });
                        this.Repeater1.DataSource = articles;
                        this.Repeater1.DataBind();
                    }
                    this.txtDistributeSummary.Text = entity.DistributeSummary;
                    this.txtPath10.Text = entity.Distribute_Pic;
                    this.txtDistributeContent.Text = entity.DistributeContent;
                    this.txtPath11.Text = entity.NextWeekWarning_Pic;
                    this.ddlNextWeekMinganValue.SelectedValue = entity.NextWeekMinganValue;
                    this.txtNextWeekSustainNumber.Text = entity.NextWeekSustainNumber.Value.ToString();
                    foreach(var item in entity.AttentionQuestions.Split('|'))
                    {
                        this.lstAttentionQuestions.Items.Add(new ListItem { Text = item, Value = item });
                    }
                    this.txtPath1.Text = entity.Week_Pic;
                    this.prev_pic1.Src = entity.Week_Pic;
                    this.txtLowExponent.Value = entity.LowExponent.Value.ToString();
                    this.txtHeightExponent.Value = entity.HeightExponent.Value.ToString();
                    this.txtAvgExponent.Value = entity.AvgExponent.Value.ToString();
                    this.txtStartMonth.Value = entity.StartMonth.Value.ToString();
                    this.txtStartdDay.Value = entity.StartDay.Value.ToString();
                    this.txtLowNumber.Value = entity.LowNumber.Value.ToString();
                    this.txtEndMonth.Value = entity.EndMonth.Value.ToString();
                    this.txtEndDay.Value = entity.EndDay.Value.ToString();
                    this.txtHeightNumber.Value = entity.HeightNumber.Value.ToString();
                    this.ddlTrend.Value = entity.Tend;

                    this.hideNewsId.Value = entity.ReportsExcerpt;
                    array1 = entity.ReportsExcerpt;
                    if(!string.IsNullOrEmpty(entity.ReportsExcerpt))
                    {
                        var excerpts = context.Articles.OrderByDescending(a => a.ID).ToList().Where(a => entity.ReportsExcerpt.Split(',').Contains(a.ID.ToString())).Select(a => new
                            {
                                a.ID,
                                a.Title,
                                a.Site,
                                ChannelType = CommonUtility.GetChannelName(a.ChannelType),
                                a.ReplyCount,
                                AddDate = a.AddDate.Value.ToString("MM月dd日")
                            });
                        this.Repeater2.DataSource = excerpts;
                        this.Repeater2.DataBind();
                    }
                    this.txtPath2.Text = entity.Attention_Picture;
                    this.prev_pic2.Src = entity.Attention_Picture;
                    this.txtTotal_AttentionNumber.Value = entity.Total_AttentionNumber.Value.ToString();
                    this.txtMobile_AttentionNumber.Value = entity.Mobile_AttentNumber.Value.ToString();
                    this.ddlTotal_FloatingType.Value = entity.Total_FloatingType;
                    this.txtTotal_AttentionPercent.Value = entity.Total_AttentionPercent;
                    this.ddlMobile_FloatingType.Value = entity.Mobile_FloatingType;
                    this.txtMobile_AttentionPercent.Value = entity.Mobile_AttentionPercent;
                    foreach(var item in entity.AttentionCities.Split('、'))
                    {
                        this.lstAttentionCities.Items.Add(new ListItem { Text = item, Value = item });
                    }
                    this.txtPath3.Text = entity.Police_pic;
                    this.txtTopic1Guide.Text = entity.Topic1Guide;
                    this.txtPath4.Text = entity.Topic1_pic;
                    this.txtTopic2Guide.Text = entity.Topic2Guide;
                    this.txtPath5.Text = entity.Topic2_pic;
                    this.txtTopic3Guide.Text = entity.Topic3Guide;
                    this.txtPath6.Text = entity.Topic3_pic;
                    this.txtPath7.Text = entity.Investment_pic;
                    this.txtPath8.Text = entity.Technology_pic;
                    this.txtPath9.Text = entity.Reference_pic;

                }
            }
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
                var customers = context.Customers.Select(c => new
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
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            hudong_number = int.Parse(this.ltlTieba_r.Text) + int.Parse(this.ltlBBS_r.Text) + int.Parse(this.ltlWeibo_r.Text) + int.Parse(this.ltlWeixin_r.Text) + int.Parse(this.ltlVideo_r.Text) + int.Parse(this.ltlBlog_r.Text);
            int mingan_number = int.Parse(this.ltlNews_n.Text) + int.Parse(this.ltlTieba_n.Text) + int.Parse(this.ltlBBS_n.Text) + int.Parse(this.ltlWeibo_n.Text) + int.Parse(this.ltlWeixin_n.Text) + int.Parse(this.ltlVideo_n.Text) + int.Parse(this.ltlBlog_n.Text);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            ListItem customerItem = this.ddlCustomer.SelectedItem;
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
                if(entity != null)
                {
                    currentNumber = entity.CurrentNumber.Value;
                    totalNumber = entity.TotalNumber.Value;
                }
            }
            switch(reportItem.Value)
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
            Document document = new Document(Server.MapPath("~/template_files/week_report.doc"));
            DocumentBuilder docBuilder = new DocumentBuilder(document);
            document.Range.Replace("{Company}", customerItem.Text, false, false);
            document.Range.Replace("{ReportType}", reportItem.Text, false, false);
            document.Range.Replace("{year}", DateTime.Now.ToString("yyyy"), false, false);
            document.Range.Replace("{month}", DateTime.Now.ToString("MM"), false, false);
            document.Range.Replace("{day}", DateTime.Now.ToString("dd"), false, false);
            document.Range.Replace("{number1}", currentNumber.ToString(), false, false);
            document.Range.Replace("{number2}", totalNumber.ToString(), false, false);
            document.Range.Replace("{TypeString}", CommonUtility.ReplaceReportType(reportItem.Text),false,false);
            
            document.Range.Replace("{news_number}", this.ltlNews_r.Text, false, false);
            document.Range.Replace("{hudong_number}", hudong_number.ToString(), false, false);
            document.Range.Replace("{mingan_number}", mingan_number.ToString(), false, false);
            document.Range.Replace(new Regex("{info_total_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath0.Text)), false);
            //本周重点敏感舆情、风险分析研判与处置建议
            using (var context = new ReportContext())
            {
                if(!string.IsNullOrEmpty(news_ids))
                {
                    var articleIds = news_ids.Split(',').Select(a => int.Parse(a));
                    var entities = context.Articles.Where(a => a.CustomerID == customerId && articleIds.Contains(a.ID)).OrderByDescending(a => a.ID).Select(a => a);
                    int i = 0;
                    docBuilder.MoveToMergeField("Content");
                    foreach (var entity in entities)
                    {
                        i++;
                        //插入标题
                        docBuilder.Writeln();
                        docBuilder.Font.Name = "楷体_GB2312";
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Bold = true;
                        docBuilder.Writeln(String.Format("{0}.{1}",i,entity.Title));
                        docBuilder.Font.ClearFormatting();

                        //插入内容
                        docBuilder.Font.Size = 16;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Writeln(entity.Content);
                        docBuilder.Font.ClearFormatting();

                        //插入图片
                        docBuilder.InsertImage(Server.MapPath(entity.ScreenshotsPath), RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, 430, 220, WrapType.Square);

                        //插入链接
                        docBuilder.Font.Color = Color.Blue;
                        docBuilder.Font.Size = 11;
                        docBuilder.Font.Underline = Underline.Single;
                        docBuilder.InsertHyperlink(entity.Url, entity.Url, false);
                        docBuilder.Font.ClearFormatting();

                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Font.Size = 16;
                        docBuilder.Writeln();
                        docBuilder.Writeln(String.Format("风险分析研判：{0}",entity.JudgeContent));
                        docBuilder.Write("舆情星级：");
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
                        docBuilder.Write(String.Format("处置建议：{0}", entity.SuggestContent)); ;
                        docBuilder.Font.ClearFormatting();

                        
                    }
                }
                
            }
            document.Range.Replace("{DistributeSummary}", this.txtDistributeSummary.Text.Replace("\r\n","").Replace("#", ""), false, false);
            document.Range.Replace(new Regex("{Distribute_Pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath10.Text)), false);
            document.Range.Replace("{DistributeContent}", this.txtDistributeContent.Text.Replace("\r\n",""), false, false);

            document.Range.Replace(new Regex("{NextWeekWarning_Pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath11.Text)), false);
            document.Range.Replace("{NextWeekMinganValue}", this.ddlNextWeekMinganValue.SelectedValue, false, false);
            document.Range.Replace("{NextWeekSustainNumber}", this.txtNextWeekSustainNumber.Text, false, false);
            docBuilder.MoveToMergeField("Questions");
            int b = 0;            
            docBuilder.Font.Size = 16;
            docBuilder.Font.Name = "仿宋_GB2312";
            docBuilder.Writeln();
            foreach(ListItem item in this.lstAttentionQuestions.Items)
            {
                b++;
                string str = String.Format("{0}.{1}", b, item.Value);
                docBuilder.Writeln(str);
            }
            docBuilder.Font.ClearFormatting();

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

            //-----------------部分新闻报道摘录---------------------------------
            docBuilder.MoveToMergeField("ReportsExcerpt");

            Aspose.Words.Tables.Table table = docBuilder.StartTable();

            docBuilder.CellFormat.Borders.LineStyle = LineStyle.Single;
            docBuilder.CellFormat.Shading.BackgroundPatternColor = Color.FromArgb(217, 217, 217);
            docBuilder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            docBuilder.Font.Name = "仿宋_GB2312";
            docBuilder.Font.Size = 10;
            docBuilder.Font.Bold = true;

            docBuilder.InsertCell();
            docBuilder.CellFormat.Width = 85.0;
            docBuilder.Write("序号");

            docBuilder.InsertCell();
            docBuilder.CellFormat.Width = 86.0;
            docBuilder.Write("来源");

            docBuilder.InsertCell();
            docBuilder.CellFormat.Width = 76.0;
            docBuilder.Write("时间");

            docBuilder.InsertCell();
            docBuilder.CellFormat.Width = 273.0;
            docBuilder.Write("文章标题");

            docBuilder.InsertCell();
            docBuilder.CellFormat.Width = 95.0;
            docBuilder.Write("转载量");
            docBuilder.EndRow();
            docBuilder.Font.ClearFormatting();

            using(var context = new ReportContext())
            {
                if(!string.IsNullOrEmpty(this.hideNewsId.Value))
                {
                    string newsIds = this.hideNewsId.Value;
                    var articleIds = newsIds.Split(',').Select(a => int.Parse(a));
                    var entities = context.Articles.Where(a => a.CustomerID == customerId && articleIds.Contains(a.ID)).OrderByDescending(a => a.ID).Select(a => a);
                    int c = 0;
                    foreach(var entity in entities)
                    {
                        c++;
                        docBuilder.Font.Name = "仿宋_GB2312";
                        docBuilder.Font.Size = 10;

                        docBuilder.InsertCell();
                        docBuilder.CellFormat.Width = 85.0;
                        docBuilder.Write(c.ToString());

                        docBuilder.InsertCell();
                        docBuilder.CellFormat.Width = 86.0;
                        docBuilder.Write(entity.Site);

                        docBuilder.InsertCell();
                        docBuilder.CellFormat.Width = 76.0;
                        docBuilder.Write(entity.AddDate.Value.ToString("MM月dd日"));

                        docBuilder.InsertCell();
                        docBuilder.CellFormat.Width = 273.0;
                        docBuilder.Write(entity.Title);

                        docBuilder.InsertCell();
                        docBuilder.CellFormat.Width = 95.0;
                        docBuilder.Write(entity.ReplyCount.ToString());
                        docBuilder.EndRow();
                        docBuilder.Font.ClearFormatting();
                        
                    }
                }
            }
            docBuilder.EndTable();

            document.Range.Replace(new Regex("{attention_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath2.Text)), false);
            document.Range.Replace("{total_AttentionNumber}", this.txtTotal_AttentionNumber.Value, false, false);
            document.Range.Replace("{mobile_AttentNumber}", this.txtMobile_AttentionNumber.Value, false, false);
            document.Range.Replace("{total_FloatingType}", this.ddlTotal_FloatingType.Value, false, false);
            document.Range.Replace("{total_AttentionPercent}", this.txtTotal_AttentionPercent.Value, false, false);
            document.Range.Replace("{mobile_FloatingType}", this.ddlMobile_FloatingType.Value, false, false);
            document.Range.Replace("{mobile_AttentionPercent}", this.txtMobile_AttentionPercent.Value, false, false);
            StringBuilder cityBuilder = new StringBuilder();
            foreach(ListItem item in this.lstAttentionCities.Items)
            {
                cityBuilder.AppendFormat("{0}、", item.Value);
            }
            string cities = cityBuilder.ToString().TrimEnd('、');
            document.Range.Replace("{cities}", cities, false, false);

            document.Range.Replace(new Regex("{police_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath3.Text)), false);
            document.Range.Replace("{Topic1Guide}", this.txtTopic1Guide.Text.Replace("\r\n", ""), false, false);
            document.Range.Replace(new Regex("{Topic1_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath4.Text)), false);
            document.Range.Replace("{Topic2Guide}", this.txtTopic2Guide.Text.Replace("\r\n", ""), false, false);
            document.Range.Replace(new Regex("{Topic2_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath5.Text)), false);
            document.Range.Replace("{Topic3Guide}", this.txtTopic3Guide.Text.Replace("\r\n",""), false, false);
            document.Range.Replace(new Regex("{Topic3_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath6.Text)), false);

            document.Range.Replace(new Regex("{Investment_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath7.Text)), false);
            document.Range.Replace(new Regex("{Technology_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath8.Text)), false);
            document.Range.Replace(new Regex("{Reference_pic}"), new ReplaceAndInsertImage(Server.MapPath(this.txtPath9.Text)), false);
            string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", customerItem.Text, reportItem.Text, currentNumber.ToString());

            StringBuilder questionsBuilder = new StringBuilder();
            foreach(ListItem item in this.lstAttentionQuestions.Items)
            {
                questionsBuilder.AppendFormat("{0}|", item.Value);
            }
            string questions = questionsBuilder.ToString().TrimEnd('|');
            using (var context = new ReportContext())
            {
                var report = new Report
                {
                    ReportName = fileName,
                    Company = customerItem.Text,
                    ReportType = reportType,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM"),
                    Day = DateTime.Now.ToString("dd"),
                    CurrentNumber = currentNumber.ToString(),
                    TotalNumber = totalNumber.ToString(),
                    News_number = int.Parse(this.ltlNews_r.Text),
                    Hudong_number = hudong_number,
                    Mingan_number = mingan_number,
                    Info_total_pic = this.txtPath0.Text,
                    PriorityArticles = this.hideSelectedIDs.Value,                    
                    DistributeSummary = this.txtDistributeSummary.Text.Replace("#",""),
                    Distribute_Pic = this.txtPath10.Text,
                    DistributeContent = this.txtDistributeContent.Text,
                    NextWeekWarning_Pic = this.txtPath11.Text,
                    NextWeekMinganValue = this.ddlNextWeekMinganValue.SelectedValue,
                    NextWeekSustainNumber = int.Parse(this.txtNextWeekSustainNumber.Text),
                    AttentionQuestions = questions,
                    ReportsExcerpt = this.hideNewsId.Value,
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
                    Police_pic = this.txtPath3.Text,
                    AttentionCities = cities,
                    Topic1Guide = this.txtTopic1Guide.Text,
                    Topic1_pic = this.txtPath4.Text,
                    Topic2Guide = this.txtTopic2Guide.Text,
                    Topic2_pic = this.txtPath5.Text,
                    Topic3Guide = this.txtTopic3Guide.Text,
                    Topic3_pic = this.txtPath6.Text,
                    Investment_pic = this.txtPath7.Text,
                    Technology_pic = this.txtPath8.Text,
                    Reference_pic = this.txtPath9.Text,
                    AddDate = DateTime.Now
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

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTableData(this.ddlReportType.SelectedValue,this.ddlCustomer.SelectedValue);            
            
        }

        private void InitTableData(string selectedValue = "0",string custId = "0")
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            if (selectedValue != "0" && custId != "0")
            {
                switch (selectedValue)
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
                    int customerId = int.Parse(custId);
                    news_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "news" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    news_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "news" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    tieba_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "tieba" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    tieba_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "tieba" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    bbs_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "bbs" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    bbs_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "bbs" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    weibo_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "weibo" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    weibo_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "weibo" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    weixin_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "weixin" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    weixin_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "weixin" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    video_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "video" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    video_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "video" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    blog_r = context.Articles.Count(a => (a.Favorite == "正面" || a.Favorite == "中性") && a.ChannelType == "blog" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    blog_n = context.Articles.Count(a => a.Favorite == "负面" && a.ChannelType == "blog" && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate) && a.CustomerID == customerId);
                    this.ltlNews_r.Text = news_r.ToString();
                    this.ltlNews_n.Text = news_n.ToString();
                    this.ltlTieba_r.Text = tieba_r.ToString();
                    this.ltlTieba_n.Text = tieba_n.ToString();
                    this.ltlBBS_r.Text = bbs_r.ToString();
                    this.ltlBBS_n.Text = bbs_n.ToString();
                    this.ltlWeibo_r.Text = weibo_r.ToString();
                    this.ltlWeibo_n.Text = weibo_n.ToString();
                    this.ltlWeixin_r.Text = weixin_r.ToString();
                    this.ltlWeixin_n.Text = weixin_n.ToString();
                    this.ltlVideo_r.Text = video_r.ToString();
                    this.ltlVideo_n.Text = video_n.ToString();
                    this.ltlBlog_r.Text = blog_r.ToString();
                    this.ltlBlog_n.Text = blog_n.ToString();

                    var results = context.Articles.Where(a => a.Favorite == "负面" && a.CustomerID == customerId && (a.AddDate.Value >= startDate && a.AddDate.Value <= endDate)).GroupBy(g => g.ChannelType).Select(g => new
                    {
                        ChannelName = context.Channels.FirstOrDefault(c => c.ChannelValue == g.Key).ChannelText,
                        Count = g.Count()
                    }).OrderByDescending(o => o.Count);
                    StringBuilder sBuilder = new StringBuilder();
                    foreach (var result in results)
                    {
                        //与微博（1条）
                        sBuilder.AppendFormat("{0}（{1}条），", result.ChannelName, result.Count);
                    }
                    string str = sBuilder.ToString().TrimEnd('，');
                    int mingan_number = news_n + tieba_n + bbs_n + weibo_n + weixin_n + video_n + blog_n;
                    string text = String.Format("{0}，{1}敏感舆情数量共计{2}条，均出现在#互动类媒体平台#，分布在#{3}#。", CommonUtility.ReplaceReportType(this.ddlReportType.SelectedItem.Text), this.ddlCustomer.SelectedItem.Text, mingan_number, str);
                    this.txtDistributeSummary.Text = text;
                }
            }
            else
            {
                this.ltlNews_r.Text = "0";
                this.ltlNews_n.Text = "0";
                this.ltlTieba_r.Text = "0";
                this.ltlTieba_n.Text = "0";
                this.ltlBBS_r.Text = "0";
                this.ltlBBS_n.Text = "0";
                this.ltlWeibo_r.Text = "0";
                this.ltlWeibo_n.Text = "0";
                this.ltlWeixin_r.Text = "0";
                this.ltlWeixin_n.Text = "0";
                this.ltlVideo_r.Text = "0";
                this.ltlVideo_n.Text = "0";
                this.ltlBlog_r.Text = "0";
                this.ltlBlog_n.Text = "0";
                this.txtDistributeSummary.Text = string.Empty;
            }
        }
        [WebMethod]
        public static string GetNewsById(string ids)
        {
            string json = string.Empty;
            if(!string.IsNullOrEmpty(ids))
            {
                var articleIds = ids.Split(',').Select(a => int.Parse(a));
                using (var context = new ReportContext())
                {
                    var articles = context.Articles.Where(a => articleIds.Contains(a.ID)).OrderByDescending(a => a.ID).ToList().Select(a => new
                    {
                        a.ID,
                        a.Title,
                        a.Site,
                        ChannelType = CommonUtility.GetChannelName(a.ChannelType),
                        a.ReplyCount,
                        AddDate = a.AddDate.Value.ToString("MM月dd日")
                    });
                    json = JsonConvert.SerializeObject(articles, Formatting.Indented);
                }
            }
            else
            {
                json = string.Empty;
            }
            return json;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTableData(this.ddlReportType.SelectedValue, this.ddlCustomer.SelectedValue);
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

        protected void btnAddQuestion_ServerClick(object sender, EventArgs e)
        {
            string question = this.txtAttentionQuestion.Text;
            if(!string.IsNullOrEmpty(question))
            {
                var item = this.lstAttentionQuestions.Items.FindByValue(question);
                if (item == null)
                {
                    item = new ListItem { Text = question, Value = question };
                    this.lstAttentionQuestions.Items.Add(item);
                    this.lstAttentionQuestions.SelectedIndex = -1;
                    this.txtAttentionQuestion.Text = string.Empty;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "a", "alert('此重点关注问题已经存在');$('#txtAttentionQuestion').focus();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "a", "alert('请输入重点关注问题');$('#txtAttentionQuestion').focus();", true);
            }
        }

        protected void btnDeleteQuestion_ServerClick(object sender, EventArgs e)
        {
            string question = this.txtAttentionQuestion.Text;
            var item = this.lstAttentionQuestions.Items.FindByValue(question);
            if (item != null)
            {;
                this.lstAttentionQuestions.Items.Remove(item);
                this.lstAttentionQuestions.SelectedIndex = -1;
                this.txtAttentionQuestion.Text = string.Empty;
            }
        }

        protected void lstAttentionQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = this.lstAttentionQuestions.SelectedValue;
            this.txtAttentionQuestion.Text = selectedValue;
        }

        

    }
}