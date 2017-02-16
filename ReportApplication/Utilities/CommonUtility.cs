using Aspose.Words;
using Aspose.Words.Drawing;
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public static class CommonUtility
    {
        public static string GetCheckBoxListSelectedValue(this CheckBoxList checkboxlist)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (ListItem item in checkboxlist.Items)
            {
                if (item.Selected)
                {
                    sBuilder.AppendFormat("{0}+", item.Value);
                }
            }
            return sBuilder.ToString().TrimEnd('+');
        }
        public static string PrintStar(int starCount)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 1; i <= starCount; i++)
            {
                sBuilder.Append("★");
            }
            return sBuilder.ToString();
        }
        public static string Substring(string str, int length)
        {
            if (str.Length <= length)
            {
                return str;
            }
            else
            {
                return str.Substring(0, length);
            }
        }
        public static string GetChannelName(string channelType)
        {
            switch (channelType)
            {
                case "tieba":
                    return "贴吧";
                case "weibo":
                    return "微博";
                case "weixin":
                    return "微信";
                case "bbs":
                    return "论坛";
                case "video":
                    return "视频";
                case "blog":
                    return "博客";
                default:
                    return "新闻";
            }
        }
        public static string ReplaceReportType(string reportType)
        {
            if (!string.IsNullOrEmpty(reportType))
            {
                return "本" + reportType.Replace("报", "");
            }
            else
            {
                return string.Empty;
            }
        }
        public static IEnumerable<Item> GetAllCategory()
        {
            using (var context = new DataSettingContext())
            {
                var entities = context.Categories.Select(c => new Item
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                return entities.ToList();
            }
        }

        public static IEnumerable<Item> GetMainCategoryById(int categoryId)
        {
            using (var context = new DataSettingContext())
            {
                var items = context.MainCategories.Where(c => c.CategoryID == categoryId).Select(c => new Item
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                return items.ToList();
            }
        }

        public static IEnumerable<Item> GetMediumCategoryById(int mainCategoryId)
        {
            using (var context = new DataSettingContext())
            {
                var items = context.MediumCategories.Where(c => c.MainCategoryID == mainCategoryId).Select(c => new Item
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                return items.ToList();
            }
        }
        public static IEnumerable<Item> GetSmallCategoryById(int mediumCategoryId)
        {
            using (var context = new DataSettingContext())
            {
                var items = context.SmallCategories.Where(c => c.MediumCategoryID == mediumCategoryId).Select(c => new Item
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                return items.ToList();
            }
        }
        public static IEnumerable<ReportType> GetReportTypeByCategoryName(string categoryName)
        {
            using (var context = new DataSettingContext())
            {
                var entities = context.DataServiceCategories.Where(c => c.CategoryName == categoryName).Select(c => new ReportType
                {
                    Value = c.ID.ToString(),
                    Text = c.CategoryItem
                });
                return entities.ToList();
            }
        }


        public static string GetTypeNameById(int id)
        {
            using (var context = new DataSettingContext())
            {
                var entity = context.DataServiceCategories.Find(id);
                if (entity != null)
                {
                    return entity.CategoryItem;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //输出26个英文字母大写
        public static List<string> GetLetters()
        {
            List<string> list = new List<string>();
            int iStart = 65;
            int iEnd = 90;
            while(iStart <= iEnd)
            {
                list.Add(((char)iStart).ToString());
                iStart++;
            }
            return list;
        }
        public static void GridViewSetFocus(GridViewRow row, string ControlName)
        {
            bool found = false;
            string control_name_lower = ControlName.ToLower();
            foreach (TableCell cell in row.Cells)
            {
                foreach (Control control in cell.Controls)
                {
                    if (control.ID != null)
                    {
                        if (control.ID.ToLower() == control_name_lower)
                        {
                            found = true;
                            control.Focus();
                            break;
                        }
                    }
                }
                if (found)
                    break;
            }
        }
        public static void PrintFile(string filePath,string fileName)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = fileStream.ToByteArray();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Length", fileStream.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/msword";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;FileName=" + fileName);
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.End();
                fileStream.Close();
                fileStream.Dispose();

            }
        }
        public static void SetPictureBorder(string sourceFile,string outputFile)
        {
            System.Drawing.Image img = Bitmap.FromFile(sourceFile);
            int bordwidth = Convert.ToInt32(img.Width * 0.01);
            int bordheight = Convert.ToInt32(img.Height * 0.01);

            int newheight = img.Height + bordheight;
            int newwidth = img.Width + bordwidth;

            Color bordcolor = Color.FromArgb(191, 191, 191);
            Bitmap bmp = new Bitmap(newwidth, newheight);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            int Style = 0;     //New: 绘制边框的类型, 手动修改0,1,2 可改变边框类型
            if (Style == 0)   //New: 整个边框.
            {
                //Changed: 修改rec区域, 将原图缩放. 适合边框内
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, bordwidth / 2, newwidth - bordwidth / 2, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(new Pen(bordcolor, bordheight), 0, 0, newwidth, newheight);
            }
            else if (Style == 1)   //New: 上下边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(0, bordwidth / 2, newwidth, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, newwidth, 0);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, newheight, newwidth, newheight);
            }
            else if (Style == 2)   //New: 左右边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, 0, newwidth - bordwidth / 2, newheight);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, 0, newheight);
                g.DrawLine(new Pen(bordcolor, bordheight), newwidth, 0, newwidth, newheight);
            }

            bmp.Save(outputFile);
            g.Dispose();
        }
        public static DataTable CreateMemoryTable(List<string> columns)
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

    }
    [DataContract]
    public class ReportType
    {
        public int ID { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
    [DataContract]
    public class Item
    {
        public int Id { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
    public static class SMSUtility
    {

        public static string baseUri = "http://www.yunpian.com";
        public static string version = "v1";
        public static string uri_get_user_info = baseUri + "/" + version + "/user/get.json";
        public static string uri_send_sms = baseUri + "/" + version + "/sms/send.json";
        public static string uri_tpl_send_sms = baseUri + "/" + version + "/sms/tpl_send.json";
        public static string apikey = "e5af98a4ed62015c4322a219aad2b2c1";
        public static string GetUserInfo(string apikey)
        {
            string requestUriString = uri_get_user_info + "?apikey=" + apikey;
            WebRequest request = WebRequest.Create(requestUriString);
            using (WebResponse webResponse = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public static string SendSMS(string text, string mobile, string apikey = "e5af98a4ed62015c4322a219aad2b2c1")
        {
            string parameter = String.Format("apikey={0}&text={1}&mobile={2}", apikey, Uri.EscapeUriString(text), mobile);
            WebRequest request = WebRequest.Create(uri_send_sms);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(parameter);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                using (WebResponse response = request.GetResponse())
                {
                    if (response == null)
                    {
                        return null;
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string sendTplSMS(long tpl_id, string text, string mobile, string apikey = "e5af98a4ed62015c4322a219aad2b2c1")
        {
            string parameter = String.Format("apikey={0}&tpl_id={1}&tpl_value={2}&mobile={3}", apikey, tpl_id, Uri.EscapeUriString(text), mobile);
            WebRequest request = WebRequest.Create(uri_tpl_send_sms);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(parameter);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                using (WebResponse response = request.GetResponse())
                {
                    if (response == null)
                    {
                        return null;
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
    public class ReplaceAndInsertImage : IReplacingCallback
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ReplaceAndInsertImage(string url,int width = 430,int height=220)
        {
            this.Url = url;
            this.Width = width;
            this.Height = height;
        }
        public ReplaceAction Replacing(ReplacingArgs args)
        {
            FileInfo file = new FileInfo(Url);
            var node = args.MatchNode;
            Document document = node.Document as Document;
            DocumentBuilder builder = new DocumentBuilder(document);
            builder.MoveTo(node);
            if (file.Exists)
            {

                builder.InsertImage(Url, RelativeHorizontalPosition.Margin, 1, RelativeVerticalPosition.Default, 10, Width, Height, WrapType.Square);
                return ReplaceAction.Replace;
            }
            else
            {                
                return ReplaceAction.Replace;
            }
        }
        
    }
    public class ReplaceAndInsertHtml : IReplacingCallback
    {
        public string Content { get; set; }
        public ReplaceAndInsertHtml(string content)
        {
            this.Content = content;
        }
        public ReplaceAction Replacing(ReplacingArgs args)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                var node = args.MatchNode;
                Document document = node.Document as Document;
                DocumentBuilder docBuilder = new DocumentBuilder(document);
                docBuilder.MoveTo(node);
                docBuilder.InsertHtml(Content);
                return ReplaceAction.Replace;
            }
            else
            {
                return ReplaceAction.Skip;
            }
        }
    }
}