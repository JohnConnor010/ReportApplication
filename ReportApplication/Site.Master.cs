using Aspose.Email.Mail;
using Aspose.Email.Pop3;
using Newtonsoft.Json;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using ReportApplication.Models;
using ReportApplication.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportApplication
{
    public partial class SiteMaster : MasterPage
    {
        public string MenuString { get; set; }
        public string RoleId
        {
            get
            {
                string userName = CookieUtilities.GetCookieValue("username");
                return MemberUtilities.GetRoleId(userName);
            }
        }
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ltlUserName.Text = CookieUtilities.GetCookieValue("username");
            using (var context = new MemberContext())
            {
                VelocityEngine ve = new VelocityEngine();
                ve.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                ve.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Server.MapPath("~/html_template"));
                ve.Init();
                VelocityContext vContext = new VelocityContext();
                var entities = context.RoleFunctions.Where(a => a.RoleId == RoleId && a.ParentID.EndsWith("_0")).Select(a => a).ToList();
                List<Item> items = new List<Item>();
                foreach (var entity in entities)
                {
                    var item = new Item();
                    item.Name = entity.Name;
                    item.IconClass = entity.IconClass;
                    item.Url = entity.Url;
                    item.Items = context.RoleFunctions.Where(a => a.RoleId == RoleId && a.ParentID.StartsWith(entity.ParentID.Substring(0, 2))).Where(a => a.Name != entity.Name).Select(a => new Item
                    {
                        Name = a.Name,
                        Url = a.Url
                    }).ToList<Item>();
                    items.Add(item);
                }
                vContext.Put("items", items);
                Template template = ve.GetTemplate("menu.html");
                StringWriter writer = new StringWriter();
                template.Merge(vContext, writer);
                MenuString = writer.GetStringBuilder().ToString();
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
        public class Item
        {
            public string Name { get; set; }
            public string IconClass { get; set; }
            public string Url { get; set; }
            public IEnumerable<Item> Items { get; set; }
        }
        
        protected void ext_system_ServerClick(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
            Response.Redirect("/Account/Login.aspx");
        }
        
    }
    

}