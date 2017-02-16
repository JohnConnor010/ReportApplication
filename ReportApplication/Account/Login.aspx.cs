using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using ReportApplication.Utilities;
using iNethinkCMS.Command;

namespace ReportApplication.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtPassword.Attributes.Add("value", "111111");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // 验证用户密码
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // 这不会计入到为执行帐户锁定而统计的登录失败次数中
                // 若要在多次输入错误密码的情况下触发锁定，请更改为 shouldLockout: true
                var result = signinManager.PasswordSignIn(this.txtUserName.Text, this.txtPassword.Text, this.chkRememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        CookieUtilities.SetCookie("username", this.txtUserName.Text.Trim(),DateTime.Now.AddDays(15));
                        IdentityHelper.RedirectToReturnUrl("/Manage/HomePage.aspx", Response);
                        break;
                    case SignInStatus.LockedOut:
                        MessageBox.Show(this, "用户被锁定，请联系管理员解除锁定");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        this.chkRememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        MessageBox.Show(this, "登录失败，请确认用户名和密码是否正确");
                        break;
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}