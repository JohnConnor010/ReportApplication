using iNethinkCMS.Command;
using ReportApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public partial class UploadTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitComponent();
            }
        }

        private void InitComponent()
        {
            BingGridView1Data();
        }

        private void BingGridView1Data(int currentPageIndex = 1)
        {
            using (var context = new PaperContext())
            {
                var entities = context.PPTTemplates.OrderByDescending(p => p.ID).Select(p => p).ToList();
                this.AspNetPager1.RecordCount = entities.Count();
                this.AspNetPager1.CurrentPageIndex = currentPageIndex;
                this.GridView1.DataSource = entities.Skip((currentPageIndex - 1) * this.AspNetPager1.PageSize).Take(this.AspNetPager1.PageSize);
                this.GridView1.DataBind();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            bool fileOK = false;
            if(this.FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                String[] allowedExtensions = { ".pptx",};
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if(fileOK)
                {
                    string fileName = this.FileUpload1.FileName;
                    string suffix = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + "." + suffix;
                    string savePath = "/template_files/tmpls/" + newFileName;
                    this.FileUpload1.SaveAs(Server.MapPath(savePath));
                    this.txtFilePath.Text = savePath;
                }
                else
                {
                    MessageBox.Show(this, "只允许上传PPTX格式的文件");
                    return;
                }
            }
            else
            {
                MessageBox.Show(this, "上选择要上传的文件");
                return;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string title = this.txtTitle.Text.Trim();
            string filePath = this.txtFilePath.Text.Trim();
            string summary = this.txtSummary.Text.Trim();
            try
            {
                using (var context = new PaperContext())
                {
                    var entity = new PPTTemplate
                    {
                        Title = title,
                        Path = filePath,
                        Summary = summary,
                        AddDate = DateTime.Now
                    };
                    context.PPTTemplates.Add(entity);
                    context.SaveChanges();
                    this.txtTitle.Text = string.Empty;
                    this.txtFilePath.Text = string.Empty;
                    this.txtSummary.Text = string.Empty;
                    BingGridView1Data();
                }
            }
            catch (DbEntityValidationException ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GridView1.EditIndex = e.NewEditIndex;
            BingGridView1Data();
        }

        protected void btnUpload_Click1(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((Button)sender).NamingContainer);
            int index = row.RowIndex;
            var id = this.GridView1.DataKeys[index].Values[0];
            FileUpload fileUpload = ((FileUpload)this.GridView1.Rows[index].FindControl("FileUpload2"));
            bool fileOK = false;
            if (fileUpload.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(fileUpload.FileName).ToLower();
                String[] allowedExtensions = { ".pptx", };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if (fileOK)
                {
                    string fileName = fileUpload.FileName;
                    string suffix = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + "." + suffix;
                    string savePath = "/template_files/tmpls/" + newFileName;
                    fileUpload.SaveAs(Server.MapPath(savePath));
                    TextBox txtPath = ((TextBox)this.GridView1.Rows[index].FindControl("txtPath"));
                    if (txtPath != null)
                    {
                        txtPath.Text = savePath;
                    }
                }
                else
                {
                    MessageBox.Show(this, "只允许上传PPTX格式的文件");
                    return;
                }                
            }
            else
            {
                MessageBox.Show(this, "上选择要上传的文件");
                return;
            }
            Button button = ((Button)this.GridView1.Rows[index].FindControl("btnUpload"));
            HtmlTableRow tr1 = ((HtmlTableRow)this.GridView1.Rows[index].FindControl("tr1"));
            HtmlTableRow tr2 = ((HtmlTableRow)this.GridView1.Rows[index].FindControl("tr2"));
            tr1.Visible = false;
            tr2.Visible = true;
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            BingGridView1Data();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = e.RowIndex;
            var id = this.GridView1.DataKeys[index].Values[0];
            TextBox txtTitle = ((TextBox)this.GridView1.Rows[index].FindControl("txtTitle"));
            TextBox txtPath = ((TextBox)this.GridView1.Rows[index].FindControl("txtPath"));
            TextBox txtSummary = ((TextBox)this.GridView1.Rows[index].FindControl("txtSummary"));
            using (var context = new PaperContext())
            {
                var entity = context.PPTTemplates.Find(id);
                if(entity != null)
                {
                    entity.Title = txtTitle.Text;
                    entity.Path = txtPath.Text;
                    entity.Summary = txtSummary.Text;
                    context.SaveChanges();
                }
            }
            this.GridView1.EditIndex = -1;
            BingGridView1Data();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BingGridView1Data(this.AspNetPager1.CurrentPageIndex);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;
            var id = this.GridView1.DataKeys[index].Values[0];
            using (var context = new PaperContext())
            {
                var entity = context.PPTTemplates.Find(id);
                if (entity != null)
                {
                    string path = entity.Path;
                    context.PPTTemplates.Remove(entity);
                    try
                    {
                        context.SaveChanges();
                        if(!string.IsNullOrEmpty(path))
                        {
                            FileInfo fileInfo = new FileInfo(Server.MapPath(path));
                            if(fileInfo.Exists)
                            {
                                fileInfo.Delete();
                                MessageBox.Show(this, "模版文件删除成功");
                                BingGridView1Data(this.AspNetPager1.CurrentPageIndex);
                            }
                            else
                            {
                                MessageBox.Show(this, "模版文件不存在");
                            }
                        }
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this, "模版文件删除失败");
                    }
                }
            }
        }
    }
}