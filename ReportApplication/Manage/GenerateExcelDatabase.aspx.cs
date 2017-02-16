using Ivony.Data;
using Ivony.Data.SqlClient;
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication.Manage
{
    public partial class GenerateExcelDatabase : System.Web.UI.Page
    {
        private static string connectionString = WebConfigurationManager.ConnectionStrings["PaperConnectionString"].ConnectionString;
        SqlDbExecutor db = SqlServer.Connect(connectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            ViewState["tempTable"] = CreateTempTable();
            this.txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        #region SelectMethod
        public IEnumerable<dynamic> ddlCustomer_GetData()
        {
            var context = new ReportContext();
            var items = context.Customers.Select(c => new
            {
                text = c.Name,
                value = c.ID
            });
            return items;
        }
        #endregion

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            string customer = this.ddlCustomer.SelectedValue;
            var startDate = this.txtStartDate.Text.ToDateTime();
            var endDate = this.txtEndDate.Text.ToDateTimeOrDefault();
            
            InitRepeater1(db,1,this.ddlCustomer.SelectedValue,this.txtStartDate.Text.Trim(),this.txtEndDate.Text.Trim());


        }
        private void InitRepeater1(SqlDbExecutor db,int currentPageIndex = 1,string customerId = "0",string startDate = "",string endDate = "")
        {
            //using (var context = new ReportContext())
            //{
            //    var items = context.Articles.ToList();
            //    if(customerId != "0")
            //    {
            //        int _customerId = int.Parse(customerId);
            //        items = items.Where(a => a.CustomerID == _customerId).ToList();
            //    }
            //    if(startDate != "" && endDate != "")
            //    {
            //        DateTime _startDate = DateTime.Parse(startDate);
            //        DateTime _endDate = DateTime.Parse(endDate);
            //        items = items.Where(a => a.AddDate >= _startDate && a.AddDate <= _endDate).ToList();
            //    }
            //    DataTable tempTable = (DataTable)ViewState["tempTable"];
            //    IEnumerable<DataRow> rows = new List<DataRow>();
            //    items.ForEach(item =>
            //    {
            //        DataRow row = tempTable.NewRow();
            //        row["ID"] = item.ID.ToString();
            //        row["Title"] = item.Title;
            //        row["ChannelType"] = item.ChannelType.ToString();
            //        row["ReplyCount"] = item.ReplyCount.ToString();
            //        row["Rating"] = item.Rating.ToString();
            //        row["AddDate"] = item.AddDate.ToString();
            //        row["Url"] = item.Url;
            //        row["CustomerID"] = item.CustomerID.ToString();
            //        row["RelateQuestion"] = item.RelateQuestion;
            //        tempTable.Rows.Add(row);
            //    });
            //    this.AspNetPager1.RecordCount = tempTable.Rows.Count;
            //    this.AspNetPager1.CurrentPageIndex = currentPageIndex;
            //    PagedDataSource dataSource = new PagedDataSource();

            //    //this.Repeater1.DataSource = tempTable.Rows.
            //    //this.Repeater1.DataBind();
            //}
            string con = "WHERE 1=1";
            if (customerId != "0")
            {
                con += " AND CustomerID = " + customerId;
            }
            if (startDate != "" && endDate != "")
            {
                con += String.Format(" AND AddDate >= '{0}' AND AddDate <= '{1}'", startDate + " 00:00:00", endDate + " 00:00:00");
            }
            string sql = String.Format("SELECT TOP {0} ID,Title,ChannelType,ReplyCount,Rating,AddDate,Url,CustomerID,RelateQuestion FROM ARTICLE WHERE ID NOT IN(SELECT TOP {1} ID FROM Article {2}  ORDER BY  ID DESC) {3} ORDER BY ID DESC", this.AspNetPager1.PageSize, (((currentPageIndex - 1) * this.AspNetPager1.PageSize)), con, con.Replace("WHERE", "AND"));
            string countSql = String.Format("SELECT COUNT(ID) FROM Article {0}", con);
            var count = db.T(countSql).ExecuteScalar<int>();
            DataTable table = db.T(sql).ExecuteDataTable();
            List<DataRow> rows = new List<DataRow>();
            foreach(DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            this.AspNetPager1.RecordCount = count;
            this.AspNetPager1.CurrentPageIndex = currentPageIndex;
            this.Repeater1.DataSource = table;
            this.Repeater1.DataBind();
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            InitRepeater1(db, this.AspNetPager1.CurrentPageIndex, this.ddlCustomer.SelectedValue, this.txtStartDate.Text.Trim(), this.txtEndDate.Text.Trim());
        }

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {
            //var id = e.CommandArgument.ToString();
            //table.Rows.RemoveAt(1);
            //InitRepeater1(db, this.AspNetPager1.CurrentPageIndex, this.ddlCustomer.SelectedValue, this.txtStartDate.Text.Trim(), this.txtEndDate.Text.Trim());
            //DataRow row = table.Rows.Find(int.Parse(id));
            //if(row != null)
            //{
            //    table.Rows.Remove(row);
            //    InitRepeater1(db, this.AspNetPager1.CurrentPageIndex, this.ddlCustomer.SelectedValue, this.txtStartDate.Text.Trim(), this.txtEndDate.Text.Trim());
            //}
        }
        private DataTable CreateTempTable()
        {
            DataTable table = new DataTable();
            DataColumn IDColumn = new DataColumn("ID", typeof(string));
            DataColumn[] keys = new DataColumn[1];
            keys[0] = IDColumn;
            table.Columns.Add(IDColumn);
            table.Columns.Add(new DataColumn ("Title", typeof(string)));
            table.Columns.Add(new DataColumn("ChannelType", typeof(string)));
            table.Columns.Add(new DataColumn("ReplyCount", typeof(string)));
            table.Columns.Add(new DataColumn("Rating", typeof(string)));
            table.Columns.Add(new DataColumn("AddDate", typeof(string)));
            table.Columns.Add(new DataColumn("Url", typeof(string)));
            table.Columns.Add(new DataColumn("CustomerID", typeof(string)));
            table.Columns.Add(new DataColumn("RelateQuestion", typeof(string)));

            return table;
        }
    }
}