using Aspose.Email.Mail;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Saving;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Newtonsoft.Json;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ReportApplication.Manage
{
    public partial class HomePage : System.Web.UI.Page
    {
        public static readonly string token = WebConfigurationManager.AppSettings["token"];
        public static readonly string appId = WebConfigurationManager.AppSettings["appId"];
        public static readonly string secret = WebConfigurationManager.AppSettings["secret"];
        public static readonly string encodingAESKey = WebConfigurationManager.AppSettings["encodingAESKey"];
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
            var table = CommonUtility.CreateMemoryTable(columns);
            Session["tempTable"] = table;
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            DateTime endDate = startDate.AddDays(1);
            using (var context = new ReportContext())
            {
                var newsCount = context.Articles.Count(a => a.AddDate >= startDate && a.AddDate <= endDate);
                this.ltlNewsCount.Text = newsCount.ToString();
                var reportsCount = context.Reports.Count();
                this.ltlReportsCount.Text = reportsCount.ToString();
                var referencesCount = context.ReferenceReports.Count();
                this.ltlReferencesCount.Text = referencesCount.ToString();
            }
            using (var context = new PaperContext())
            {
                var articlesCount = context.Papers.Count(p => p.AddDate >= startDate && p.AddDate <= endDate);
                this.ltlArticlesCount.Text = articlesCount.ToString();
            }
            this.ltlBarChart.Text = BasicColumnBarChart();

            string userId = User.Identity.GetUserId();
            if(userId != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(userId);
                if(user.CanWriteEmail == true)
                {
                    this.send_email_div.Visible = true;
                }
                else
                {
                    this.send_email_div.Visible = false;
                }
            }
            InitRepeater1();
            InitRepeater2();
            cblReceiveUser_GetData();
            InitRepeater4();
            ddlParentGroupID_GetData();
            InitRepeater5();
        }
        #region SelectMethod

        public void cblReceiveUser_GetData()
        {
            //List<ListItem> items = new List<ListItem>();
            //var results = Senparc.Weixin.MP.AdvancedAPIs.User.Get(accessToken, null);
            //foreach (var result in results.data.openid)
            //{
            //    ListItem item = new ListItem();
            //    item.Value = result;
            //    item.Text = Senparc.Weixin.MP.AdvancedAPIs.User.Info(accessToken, result).nickname;
            //    items.Add(item);
            //}
            //this.Repeater3.DataSource = items;
            //this.Repeater3.DataBind();

        }
        public void ddlParentGroupID_GetData()
        {
            using (var context = new EmailContext())
            {
                var items = context.EmailGroups.Where(g => g.ParentID == 0).Select(g => new
                {
                    text = g.GroupName,
                    value = g.ID.ToString()
                });
                this.ddlParentGroupID.Items.Clear();
                this.ddlParentGroupID.Items.Add(new ListItem { Text = "做为一级分组", Value = "0" });
                items.ForEach(item =>
                {
                    this.ddlParentGroupID.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlGroupID_GetData()
        {
            using (var context = new EmailContext())
            {
                var items = context.EmailGroups.Where(g => g.ParentID == 0).Select(g => new
                {
                    text = g.GroupName,
                    value = g.ID.ToString()
                });
                this.ddlGroupID.Items.Clear();
                this.ddlGroupID.Items.Add(new ListItem { Text = "选择分组", Value = "-1" });
                items.ForEach(item =>
                {
                    this.ddlGroupID.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        public void ddlSubGroupID_GetData(int parentId = 0)
        {
            using (var context = new EmailContext())
            {
                var items = context.EmailGroups.Where(g => g.ParentID == parentId).Select(g => new
                {
                    text = g.GroupName,
                    value = g.ID.ToString()
                });
                this.ddlSubGroupID.Items.Clear();
                this.ddlSubGroupID.Items.Add(new ListItem { Text = "选择分组", Value = "0" });
                items.ForEach(item =>
                {
                    this.ddlSubGroupID.Items.Add(new ListItem { Text = item.text, Value = item.value });
                });
            }
        }
        #endregion

        private void InitRepeater2()
        {
            using (var context = new PaperContext())
            {
                var entities = context.Papers.OrderByDescending(p => p.PaperID).ToList().Select(p => new
                {
                    p.Title,
                    AddDate = p.AddDate.ToString("yyyy-MM-dd"),
                    p.Url
                }).Take(8);
                this.Repeater2.DataSource = entities;
                this.Repeater2.DataBind();
            }
        }

        private void InitRepeater1()
        {
            using (var context = new ReportContext())
            {
                var entities = context.Reports.OrderByDescending(r => r.ID).Select(r => new
                {
                    r.ID,
                    FileName = r.ReportName
                }).Take(8).ToList();
                this.Repeater1.DataSource = entities;
                this.Repeater1.DataBind();
            }
        }

        public string BasicColumnBarChart()
        {
            using (var context = new PaperContext())
            {
                List<SeriesData> seriesDatas = new List<SeriesData>();
                var entities = context.Papers.GroupBy(p => p.CustomerID).Select(g => new
                {
                    Key = g.Key,
                    Count = g.Count(),
                });
                foreach (var entity in entities)
                {
                    var item = new SeriesData
                    {
                        Y = entity.Count,
                        Name = GetCustomerName(entity.Key)
                    };
                    if(!string.IsNullOrEmpty(item.Name))
                    {
                        seriesDatas.Add(item);
                    }
                }
                Data data = new Data(seriesDatas.ToArray());
                Highcharts chart = new Highcharts("chart1");
                chart.InitChart(new Chart
                {
                    Type = ChartTypes.Column
                });
                chart.SetTitle(new Title
                {
                    Text = "客户文章数量柱状图"
                });
                chart.SetSubtitle(new Subtitle
                {
                    Text = "查看文章数量"
                });
                chart.SetXAxis(new XAxis
                {
                    Type = AxisTypes.Category,
                    Labels = new XAxisLabels
                    {
                        Rotation = -30,
                        Align = HorizontalAligns.Right,
                        Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                    }
                });
                chart.SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "总数量份额" }
                });
                chart.SetLegend(new DotNet.Highcharts.Options.Legend { Enabled = false });
                chart.SetTooltip(new Tooltip
                {
                    HeaderFormat = "<span style=\"font-size:11px\">{series.name}</span><br/>",
                    PointFormat = "<span style=\"color:{point.color}\">{point.name}:<b>{point.y}条</b></span><br/>"
                });
                chart.SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        BorderWidth = 0,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,
                            Format = "{point.y}条"
                        }
                    }
                });
                chart.SetSeries(new Series
                {
                    Name = "文章数量",
                    Data = data,
                    PlotOptionsBar = new PlotOptionsBar
                    {
                        ColorByPoint = true
                    }
                });
                return chart.ToHtmlString();
            }
        }
        public string GetCustomerName(int customerId)
        {
            using (var context = new ReportContext())
            {
                var entity = context.Customers.Find(customerId);
                if (entity != null)
                {
                    return entity.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected void lnkDownloadReport_Command(object sender, CommandEventArgs e)
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
                    document.Range.Replace("{ReportType}", new ManageReports().GetTypeNameById(entity.ReportType.ToInt32()), false, false);
                    document.Range.Replace("{year}", entity.Year.Trim(), false, false);
                    document.Range.Replace("{month}", entity.Month.Trim(), false, false);
                    document.Range.Replace("{Subsidiary}", entity.Subsidiary ?? "", false, false);
                    document.Range.Replace("{area}", entity.Area ?? "", false, false);
                    document.Range.Replace("{day}", entity.Day.Trim(), false, false);
                    document.Range.Replace("{number1}", entity.CurrentNumber.Trim(), false, false);
                    document.Range.Replace("{number2}", entity.TotalNumber.Trim(), false, false);
                    document.Range.Replace("{TypeString}", CommonUtility.ReplaceReportType(new ManageReports().GetTypeNameById(entity.ReportType.ToInt32())), false, false);
                    document.Range.Replace("{aTypeString}", CommonUtility.ReplaceReportType(new ManageReports().GetTypeNameById(entity.ReportType.ToInt32())).Replace("本", ""), false, false);

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


                    string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", entity.Company, new ManageReports().GetTypeNameById(entity.ReportType.ToInt32()), entity.CurrentNumber);

                    //string fileName = String.Format("{0}网络舆情{1}第{2}期.doc", entity.Company, "aaaaaaa", entity.CurrentNumber);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        document.Save(ms, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                        Response.ContentType = "application/msword";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.BinaryWrite(ms.ToArray());
                        Response.End();
                    }
                }
            }
        }

        protected void btnSendWeiXin_Click(object sender, EventArgs e)
        {
            string content = this.txtWeiXinContent.Text;
            try
            {
                int count = 0;
                foreach (RepeaterItem item in this.Repeater3.Items)
                {
                    HtmlInputCheckBox checkBox = (HtmlInputCheckBox)item.FindControl("inlineCheckbox1");
                    if (checkBox.Checked)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('请选择接收用户')", true);
                    return;
                }
                StringBuilder receiverBuilder = new StringBuilder();
                foreach (RepeaterItem item in this.Repeater3.Items)
                {
                    HtmlInputCheckBox checkBox = (HtmlInputCheckBox)item.FindControl("inlineCheckbox1");
                    
                }
                string receiver = receiverBuilder.ToString().TrimEnd(',');
                using (var context = new EmailContext())
                {
                    var entity = new SendLog
                    {
                        Sender = CookieUtilities.GetCookieValue("username"),
                        Receiver = receiver,
                        SendTime = DateTime.Now,
                        SendType = "WeiXin"
                    };
                    context.SendLogs.Add(entity);
                    context.SaveChanges();
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('微信发送成功')", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('微信发送可能，可能部分用户我发收到微信内容！')", true);
            }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            cblReceiveUser_GetData();
        }
        [WebMethod(EnableSession = true)]
        public static string GetInsertFiles(string url)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(url));
                string fileName = fileInfo.Name;
                string filePath = fileInfo.FullName;
                if (HttpContext.Current.Session["tempTable"] != null)
                {
                    var table = (DataTable)HttpContext.Current.Session["tempTable"];
                    DataRow row = table.NewRow();
                    row["FileName"] = fileName;
                    row["FilePath"] = filePath;
                    table.Rows.Add(row);
                    json = JsonConvert.SerializeObject(table);
                    return json;
                }

            }
            return json;
        }
        [WebMethod(EnableSession = true)]
        public static string RemoveFileById(string id)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                if (HttpContext.Current.Session["tempTable"] != null)
                {
                    DataTable table = (DataTable)HttpContext.Current.Session["tempTable"];
                    DataRow row = table.Rows.Find(id);
                    if (row != null)
                    {
                        FileInfo fileInfo = new FileInfo(row["FilePath"].ToString());
                        table.Rows.Remove(row);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                        result = JsonConvert.SerializeObject(table);
                        return result;
                    }
                }
            }
            return result;
        }

        private void InitRepeater4(int currentPageIndex = 1, int parentId = 0)
        {
            using (var context = new EmailContext())
            {
                var entities = context.EmailGroups.OrderByDescending(g => g.ID).Where(g => g.ParentID == parentId).Select(g => g);
                this.AspNetPager4.RecordCount = entities.Count();
                this.AspNetPager4.CurrentPageIndex = currentPageIndex;
                this.Repeater4.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager4.PageSize).Take(this.AspNetPager4.PageSize).ToList();
                this.Repeater4.DataBind();
            }
        }

        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            string groupName = this.txtGroupName.Text.Trim();
            string parentId = this.ddlParentGroupID.SelectedValue;
            string groupDescription = this.txtGroupDescription.Text.Trim();
            using (var context = new EmailContext())
            {
                if (this.hideGroupAction.Value == "add")
                {
                    var entity = context.EmailGroups.FirstOrDefault(g => g.GroupName == groupName);
                    if (entity == null)
                    {
                        entity = new EmailGroup
                        {
                            GroupName = groupName,
                            ParentID = int.Parse(parentId),
                            GroupDescription = groupDescription
                        };
                        context.EmailGroups.Add(entity);
                        context.SaveChanges();
                        this.txtGroupName.Text = string.Empty;
                        this.txtGroupDescription.Text = string.Empty;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.GroupUpdatePanel, GetType(), "a", "alert('此分组名称已经存在')", true);
                        return;
                    }


                }
                if (this.hideGroupAction.Value == "edit")
                {
                    var id = this.hideGroupID.Value;
                    var entity = context.EmailGroups.Find(int.Parse(id));
                    if (entity != null)
                    {
                        entity.GroupName = groupName;
                        entity.ParentID = int.Parse(parentId);
                        entity.GroupDescription = groupDescription;
                    }
                    context.SaveChanges();
                    this.txtGroupName.Text = string.Empty;
                    this.ddlParentGroupID.SelectedIndex = -1;
                    this.txtGroupDescription.Text = string.Empty;
                    this.hideGroupID.Value = "0";
                    this.hideGroupAction.Value = "add";
                }
            }
            InitRepeater4(this.AspNetPager4.CurrentPageIndex);
            ddlParentGroupID_GetData();
        }
        public string GetGroupName(string groupId)
        {
            if (groupId == "0")
            {
                return "做为一级分组";
            }
            else
            {
                using (var context = new EmailContext())
                {
                    var id = int.Parse(groupId);
                    var entity = context.EmailGroups.FirstOrDefault(g => g.ID == id);
                    if (entity != null)
                    {
                        return entity.GroupName;
                    }
                    else
                    {
                        return "做为一级分组";
                    }
                }
            }

        }

        protected void ddlParentGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitRepeater4(this.AspNetPager4.CurrentPageIndex, int.Parse(this.ddlParentGroupID.SelectedValue));
        }

        protected void lnkGroupEdit_Command(object sender, CommandEventArgs e)
        {
            var groupId = e.CommandArgument.ToString();
            using (var context = new EmailContext())
            {
                var entity = context.EmailGroups.Find(int.Parse(groupId));
                if (entity != null)
                {
                    this.txtGroupName.Text = entity.GroupName;
                    this.ddlParentGroupID.SelectedValue = entity.ParentID.ToString();
                    this.txtGroupDescription.Text = entity.GroupDescription;
                    this.hideGroupID.Value = entity.ID.ToString();
                    this.hideGroupAction.Value = "edit";
                }
            }
        }

        protected void lnkGroupDelete_Command(object sender, CommandEventArgs e)
        {
            var groupId = e.CommandArgument.ToString();
            using (var context = new EmailContext())
            {
                var entity = context.EmailGroups.Find(int.Parse(groupId));
                if (entity != null)
                {
                    int parentId = int.Parse(groupId);
                    context.EmailGroups.Remove(entity);
                    var entities = context.EmailGroups.Where(g => g.ParentID == parentId);
                    context.EmailGroups.RemoveRange(entities);
                    context.SaveChanges();
                    this.txtGroupName.Text = string.Empty;
                    this.ddlParentGroupID.SelectedIndex = -1;
                    this.txtGroupDescription.Text = string.Empty;
                    this.hideGroupID.Value = "0";
                    this.hideGroupAction.Value = "add";
                    InitRepeater4(this.AspNetPager4.CurrentPageIndex);
                    ddlParentGroupID_GetData();
                }
            }
        }

        protected void AspNetPager4_PageChanged(object sender, EventArgs e)
        {
            InitRepeater4(this.AspNetPager4.CurrentPageIndex, int.Parse(this.ddlParentGroupID.SelectedValue));
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string groupId = this.ddlGroupID.SelectedValue;
            string subGroupId = this.ddlSubGroupID.SelectedValue;
            string email = this.txtEmail.Text.Trim();
            string phone = this.txtPhone.Text.Trim();
            string summary = this.txtSummary.Text.Trim();
            using (var context = new EmailContext())
            {
                if (this.hideUserAction.Value == "add")
                {
                    var entity = new EmailUser
                    {
                        UserName = userName,
                        GroupID = int.Parse(groupId),
                        SubGroupID = int.Parse(subGroupId),
                        Email = email,
                        Phone = phone,
                        Summary = summary,
                        AddDate = DateTime.Now
                    };
                    context.EmailUsers.Add(entity);
                    context.SaveChanges();
                    this.txtUserName.Text = string.Empty;
                    this.txtEmail.Text = string.Empty;
                    this.txtPhone.Text = string.Empty;
                    this.txtSummary.Text = string.Empty;

                }
                if (this.hideUserAction.Value == "edit")
                {
                    var id = this.hideUserID.Value;
                    var entity = context.EmailUsers.Find(int.Parse(id));
                    if (entity != null)
                    {
                        entity.UserName = userName;
                        entity.GroupID = int.Parse(groupId);
                        entity.SubGroupID = int.Parse(subGroupId);
                        entity.Email = email;
                        entity.Phone = phone;
                        entity.Summary = summary;
                        entity.AddDate = DateTime.Now;
                        context.SaveChanges();
                        this.txtUserName.Text = string.Empty;
                        this.txtEmail.Text = string.Empty;
                        this.txtPhone.Text = string.Empty;
                        this.txtSummary.Text = string.Empty;
                        this.hideUserAction.Value = "add";
                        this.hideUserID.Value = "0";
                    }
                }
                InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
            }
        }

        private void InitRepeater5(int currentPageIndex = 1, int groupId = -1, int subGroupId = 0)
        {
            using (var context = new EmailContext())
            {
                var entities = context.EmailUsers.OrderByDescending(g => g.ID).ToList();
                if (groupId != -1)
                {
                    entities = entities.Where(g => g.GroupID == groupId).ToList();
                }
                if (subGroupId != 0)
                {
                    entities = entities.Where(g => g.SubGroupID == subGroupId).ToList();
                }
                this.AspNetPager5.RecordCount = entities.Count;
                this.AspNetPager5.CurrentPageIndex = currentPageIndex;
                this.Repeater5.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager5.PageSize).Take(this.AspNetPager5.PageSize);
                this.Repeater5.DataBind();
            }
        }

        protected void ddlGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubGroupID_GetData(int.Parse(this.ddlGroupID.SelectedValue));
            InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
        }

        protected void ddlSubGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {

            InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
        }

        protected void AspNetPager5_PageChanged(object sender, EventArgs e)
        {
            InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
        }

        protected void lnkUserEdit_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            using (var context = new EmailContext())
            {
                var entity = context.EmailUsers.Find(int.Parse(id));
                if (entity != null)
                {
                    this.txtUserName.Text = entity.UserName;
                    this.ddlGroupID.SelectedValue = entity.GroupID.ToString();
                    ddlSubGroupID_GetData(entity.GroupID.Value);
                    this.ddlSubGroupID.SelectedValue = entity.SubGroupID.ToString();
                    this.txtEmail.Text = entity.Email;
                    this.txtPhone.Text = entity.Phone;
                    this.txtSummary.Text = entity.Summary;
                    this.hideUserAction.Value = "edit";
                    this.hideUserID.Value = id;
                    InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
                }
            }
        }

        protected void lnkUserDelete_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            using (var context = new EmailContext())
            {
                var entity = context.EmailUsers.Find(int.Parse(id));
                if (entity != null)
                {
                    context.EmailUsers.Remove(entity);
                    context.SaveChanges();
                    InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
                }
            }
        }

        protected void btnUserReset_Click(object sender, EventArgs e)
        {
            this.txtUserName.Text = string.Empty;
            this.ddlGroupID.SelectedIndex = -1;
            this.ddlSubGroupID.SelectedIndex = -1;
            this.txtEmail.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtSummary.Text = string.Empty;
            InitRepeater5(this.AspNetPager5.CurrentPageIndex, int.Parse(this.ddlGroupID.SelectedValue), int.Parse(this.ddlSubGroupID.SelectedValue));
        }
        public IQueryable DropDownList1_GetData()
        {
            var context = new EmailContext();
            var items = context.EmailGroups.Where(g => g.ParentID == 0).Select(g => new
            {
                text = g.GroupName,
                value = g.ID.ToString()
            });
            return items;
        }

        #region WebMethod

        [WebMethod]
        public static string GetGroupByParentId(string parentId)
        {
            using (var context = new EmailContext())
            {
                int id = int.Parse(parentId);
                var items = context.EmailGroups.Where(g => g.ParentID == id).Select(g => new
                {
                    text = g.GroupName,
                    value = g.ID.ToString()
                }).ToList();
                return JsonConvert.SerializeObject(items, Formatting.Indented);
            }
        }
        [WebMethod]
        public static string GetUsersByParentId(string groupId, string subGroupId)
        {
            string result = string.Empty;
            using (var context = new EmailContext())
            {
                var entities = context.EmailUsers.Select(u => new
                {
                    u.ID,
                    u.GroupID,
                    u.SubGroupID,
                    u.UserName,
                    u.Email,
                    u.Phone
                }).ToList();
                if (groupId != "-1")
                {
                    entities = entities.Where(a => a.GroupID == int.Parse(groupId)).ToList();
                }
                if (subGroupId != "0")
                {
                    entities = entities.Where(a => a.SubGroupID == int.Parse(subGroupId)).ToList();
                }
                result = JsonConvert.SerializeObject(entities, Formatting.Indented);
            }
            return result;
        }
        #endregion

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            string receiver = this.txtReceiver.Text.Trim();
            string subject = this.txtSubject.Text.Trim();
            MailMessage mailMessage = new MailMessage();

            string content = this.hideMailContent.Value;
            mailMessage.From = "sdcecn01@126.com";
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.HtmlBody = content;
            string[] users = receiver.Split(',');
            foreach (string user in users)
            {
                int startIndex = user.IndexOf('<') + 1;
                int length = user.Length;
                string email = user.Substring(startIndex, length - startIndex - 1);
                mailMessage.To.Add(email);
            }
            //创建附件
            if (Session["tempTable"] != null)
            {
                DataTable table = (DataTable)Session["tempTable"];
                foreach (DataRow row in table.Rows)
                {
                    mailMessage.Attachments.Add(new Attachment(row["FilePath"].ToString()));
                }
            }
            SmtpClient client = AsposeUtilities.GetSmtpClient(this.Request.ApplicationPath);
            try
            {
                client.Send(mailMessage);
                using (var context = new EmailContext())
                {
                    var entity = new SendLog
                    {
                        Sender = CookieUtilities.GetCookieValue("username"),
                        Receiver = receiver,
                        SendTime = DateTime.Now,
                        SendType = "Email"
                    };
                    context.SendLogs.Add(entity);
                    context.SaveChanges();
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('邮件发送成功')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "a", "alert('邮件发送失败！" + ex.Message + "')", true);
            }
        }
    }
}