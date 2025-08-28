using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Owin;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Amazon.Runtime.Internal.Util;
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
            //if (Request.IsAuthenticated == false)
            //{
            //    //Unauthenticated. Prompt for Azure Auth
            //    HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "login.aspx" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            //    return;
            //}
            //else
            {
                //Authenticated. Compare Azure Login Email with email from the DB
                string emailAddress = User.Identity.Name;
                emailAddress = "meenakshi_durairaj@wrdsb.ca"; //For testing purpose, hardcoded email address
                if (emailAddress != null)
                {                 
                        authenticateWithDBtable(emailAddress);
                 
                }               
            }

        }

       
        public void Logout(object sender, EventArgs e)
        {
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
                    query = string.Format("SELECT username, employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code FROM hd_wrdsb_employee_view WHERE email_address='{0}' AND home_loc_ind='Y' AND activity_code IN ('ACTIVE','ONLEAVE')", email);
                    string connString = ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString;
                    SqlConnection con = new SqlConnection(connString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Session["ein"] = reader["employee_id"].ToString().Trim();
                            Session["surname"] = reader["surname"].ToString().Trim();
                            Session["firstname"] = reader["first_name"].ToString().Trim();
                            Session["location_code"] = reader["location_code"].ToString().Trim();
                            Session["school_code"] = reader["school_code"].ToString().Trim();
                            Session["location_desc"] = reader["location_desc"].ToString().Trim();
                            Session["email"] = reader["email_address"].ToString().ToLower().Trim();
                            Session["group_code"] = reader["emp_group_code"].ToString().Trim();
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

                    string admin = string.Empty;
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
                            admin = reader["admin"].ToString().Trim();
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

                    if (admin == "True") 
                    {
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
                        throw new Exception("The user is not authorized to access the Employee Inquiry application. Please contact administrator for assistance.");
                    }

                }
                catch (Exception ex)
                {
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