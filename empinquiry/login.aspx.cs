using System;
using System.Web;
using System.Web.Security;
using System.Configuration;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System.Data.SqlClient;
using Microsoft.Owin.Security.Cookies;

namespace empinquiry
{

    public partial class login : System.Web.UI.Page
    {
        private class Users
        {
            public string username { get; set; }
            public bool admin { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Lets try redirecting to a new login page which then verifies the authentication and then handles the redirection there.            
            if (Request.IsAuthenticated == false)
            {
                //Unauthenticated. Prompt for Azure Auth
                HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "login.aspx" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
                return;
            }
            else
            {
                //Authenticated. Compare Azure Login Email with email from the DB
                //Logger.Log("User authenticated via Azure AD. Proceeding with application login.");
                string emailAddress = User.Identity.Name;
                //emailAddress = "meenakshi_durairaj@wrdsb.ca"; //For testing purpose, hardcoded email address
                //Loggers.Log("User authenticated via email: " + emailAddress);
                if (emailAddress != null)
                {
                    authenticateWithDBtable(emailAddress);
                }
            }
        }


        public void Logout(object sender, EventArgs e)
        {
            //Loggers.Log("User initiated logout.");
            Session.Clear();
            Session.Abandon();

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Request.GetOwinContext().Authentication.SignOut();
            HttpContext.Current.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            Response.Redirect("https://login.microsoftonline.com/common/oauth2/v2.0/logout?");
        }



        protected void authenticateWithDBtable(string email)
        {
            if (email != null)
            {
                try
                {
                    string query = "";
                    query = string.Format("SELECT username, employee_id, surname, first_name, email_address FROM hd_wrdsb_employee_view WHERE email_address='{0}' AND home_loc_ind='Y' AND activity_code IN ('ACTIVE','ONLEAVE')", email);
                    string connString = ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString;
                    SqlConnection con = new SqlConnection(connString);
                    //Loggers.Log("ConnectionString ....." + connString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    Loggers.Log("Trying to open database connectionstring");
                    con.Open();
                    Loggers.Log("Executing query to fetch user details for email: " + email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Session["ein"] = reader["employee_id"].ToString().Trim();
                            Session["surname"] = reader["surname"].ToString().Trim();
                            Session["firstname"] = reader["first_name"].ToString().Trim();
                            Session["email"] = reader["email_address"].ToString().ToLower().Trim();
                            Session["username"] = reader["username"].ToString().Trim();
                        }
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        throw new Exception("The entered email address: " + email + " cannot be found. Please contact administrator for assistance.");
                    }
                    reader.Close();
                    con.Close();


                    // employee inquiry searcher needs to be a admin so validate if user is admin

                    bool admin = false;
                    string empId = Session["ein"].ToString().Trim();
                    query = string.Format("SELECT admin FROM hd_empinquiry_user WHERE employee_id = '{0}'", empId);
                    connString = ConfigurationManager.ConnectionStrings["SQLDB_HDHRP"].ConnectionString;
                    con = new SqlConnection(connString);
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            admin = Convert.ToBoolean(reader["admin"]);
                        }
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        throw new Exception("The user is not authorized to access the Employee Inquiry application. Please contact administrator for assistance.");
                    }
                    reader.Close();
                    con.Close();


                    // Only admin users are allowed to access the application 

                    if (admin)
                    {
                        //Loggers.Log("User is verified as admin. Setting up Forms Authentication ticket.");
                        bool isCookiePersistent = false;
                        System.Web.Configuration.AuthenticationSection authSection = (System.Web.Configuration.AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

                        System.Web.Configuration.FormsAuthenticationConfiguration
                            formsAuthenticationSection = authSection.Forms;

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, Session["username"].ToString(),
                            DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, "groups");

                        //Encrypt the ticket.
                        String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        //Create a cookie, and then add the encrypted ticket to the cookie as data.
                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        if (true == isCookiePersistent)
                            authCookie.Expires = authTicket.Expiration;

                        //Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);

                        FormsAuthentication.SetAuthCookie(email, true);
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(Session["username"].ToString().ToLower(), false), false);

                        Context.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {
                        Loggers.Log("User is not an admin. Access denied.");
                        throw new Exception("The user is not authorized to access the Employee Inquiry application. Please contact administrator for assistance.");
                    }

                }
                catch (Exception ex)
                {
                    Loggers.Log("Exception Errors: " + ex.Message);
                    loginErrors.InnerHtml = ex.Message;
                    loginErrors.InnerText = ex.Message;
                    loginErrors.Visible = true;
                    logout2.Text = "Logout from existing Session";
                    logout2.Visible = true;
                }
            }
        }
    }
}