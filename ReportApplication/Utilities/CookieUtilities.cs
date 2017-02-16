using Microsoft.AspNet.Identity.Owin;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace ReportApplication.Utilities
{
    public class CookieUtilities
    {
        #region Plain cookie

        public static void SetCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        public static void SetCookie(String key, String valueString)
        {
            HttpServerUtility serverUtility = HttpContext.Current.Server;

            key = serverUtility.UrlEncode(key);
            valueString = serverUtility.UrlEncode(valueString);

            HttpCookie cookie = new HttpCookie(key, valueString);

            SetCookie(cookie);
        }

        public static void SetCookie(String key, String valueString, DateTime expires)
        {
            HttpServerUtility serverUtility = HttpContext.Current.Server;

            key = serverUtility.UrlEncode(key);
            valueString = serverUtility.UrlEncode(valueString);

            HttpCookie cookie = new HttpCookie(key, valueString);
            cookie.Expires = expires;

            SetCookie(cookie);
        }

        public static HttpCookie GetCookie(String key)
        {
            key = HttpContext.Current.Server.UrlEncode(key);
            var httpCookie = HttpContext.Current.Request.Cookies.Get(key);
            if(httpCookie == null)
            {
                HttpContext.Current.Response.Redirect("/Account/Login");
                return null;
            }
            else
            {
                return (HttpContext.Current.Request.Cookies.Get(key));
            }
            
        }

        public static String GetCookieValue(String key)
        {
            String valueString = GetCookie(key).Value;

            valueString = HttpContext.Current.Server.UrlDecode(valueString);

            return (valueString);
        }

        public static void RemoveCookie(String key)
        {
            key = HttpContext.Current.Server.UrlEncode(key);
            HttpCookie cookie = new HttpCookie(key, "anything");
            cookie.Expires = DateTime.Now.AddYears(-10);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        #endregion
    }
    public class MemberUtilities
    {
        static string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static string GetCustomerID(string userName)
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdText = "SELECT CustomerID FROM AspNetUsers WHERE UserName='" + userName + "'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                return command.ExecuteScalar().ToString(); 
            }
        }
        public static string GetRoleId(string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdText = "SELECT id FROM AspNetUsers WHERE UserName='" + userName + "'";
                SqlCommand command = new SqlCommand(cmdText, connection);
                string userId = command.ExecuteScalar().ToString();
                string query = "SELECT RoleId FROM AspNetUserRoles WHERE UserId='" + userId + "'";
                SqlCommand command1 = new SqlCommand(query, connection);
                return command1.ExecuteScalar().ToString();
            }
        }
    }
}