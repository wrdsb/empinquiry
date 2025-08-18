using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;


namespace empinquiry
{
    public partial class _default : System.Web.UI.Page
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if(!Page.User.Identity.IsAuthenticated)
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                FormsAuthentication.RedirectToLoginPage();
            }

            // if both session variables are null, redirect to login.aspx page
            if (Session["surname"] == null || Session["firstname"] == null)
            {
                Session.Clear();
                Session.Abandon();
                
                Response.Redirect("login.aspx");
            }

            
            if (!Page.IsPostBack)
            {
                //lbl_name.Text = Session["firstname"].ToString();
                Session["auditComplete"] = false;
            }
             
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            var purpose = tb_purpose.Text;
            //var ticket = tb_ticket.Text;

            Session["auditComplete"] = true;
            Response.Redirect("reports.aspx");

        }
    }
}