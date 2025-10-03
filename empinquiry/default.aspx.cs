using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


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

            if (!Page.User.Identity.IsAuthenticated)
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
            var employeeId = Session["ein"];
            var surName = Session["surname"];
            var firstName = Session["firstname"];
            var email = Session["email"];
            var userId = Session["username"];
            var purpose = tb_purpose.Text;
            string currenDate = DateTime.Now.ToString("MMM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            try
            {
                var query = "INSERT INTO hd_empinquiry_audit " +
                "VALUES ('" + employeeId + "','" + firstName + "','" + surName + "','" + email + "','" + userId + "','" + purpose + "','" + currenDate + "')";


                string connString = ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString;
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting audit record: " + ex.Message);
            }


            Session["auditComplete"] = true;
            Response.Redirect("reports.aspx");

        }
    }
}