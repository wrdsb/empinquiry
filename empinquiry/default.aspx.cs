using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
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
                Session["auditComplete"] = false;
            }

        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString;            
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    var query = "INSERT INTO hd_empinquiry_audit (employee_id, firstname, surname, emailaddress, userid, Purpose, inquiry_date) " +
                                "VALUES (@empId, @firstName, @surName, @email, @userId, @purpose, @currenDate)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        string currenDate = DateTime.Now.ToString("MMM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                        cmd.Parameters.AddWithValue("@empId", Session["ein"]);
                        cmd.Parameters.AddWithValue("@firstName", Session["firstname"]);
                        cmd.Parameters.AddWithValue("@surName", Session["surname"]);
                        cmd.Parameters.AddWithValue("@email", Session["email"]);
                        cmd.Parameters.AddWithValue("@userId", Session["username"]);
                        cmd.Parameters.AddWithValue("@purpose", tb_purpose.Text);   
                        cmd.Parameters.AddWithValue("@currenDate", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Loggers.Log("Error inserting audit record: " + ex.Message);
                throw new Exception("Error inserting audit record: " + ex.Message);
            }
            Session["auditComplete"] = true;
            Response.Redirect("reports.aspx");

        }
    }
}