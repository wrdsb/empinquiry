using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using Microsoft.Owin.Security.Cookies;

namespace empinquiry
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbl_username.Text = Context.User.Identity.Name;

                reports_link.Visible = false;
                
            }

            

            if (Session["auditComplete"] != null)
            {
                var auditComplete = Convert.ToBoolean(Session["auditComplete"]);
                if (auditComplete == true)
                {
                    reports_link.Visible = true;
                }
            }


            SetCurrentPage();
        }

        protected void lb_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

            //Request.GetOwinContext().Authentication.SignOut();
            //HttpContext.Current.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            //Response.Redirect("https://login.microsoftonline.com/common/oauth2/v2.0/logout?");
        }

        protected void lb_settings_Click(object sender, EventArgs e)
        {
            Response.Redirect("settings.aspx");
        }
        private void SetCurrentPage()
        {
            var pageName = GetPageName();

            switch (pageName)
            {
                case "default.aspx":
                    home_link.Attributes["class"] = "active";
                    break;
                case "reports.aspx":
                    reports_link.Attributes["class"] = "active";
                    break;                
            }
        }
        private string GetPageName()
        {
            return Request.Url.ToString().Split('/').Last();
        }

        
    }
}