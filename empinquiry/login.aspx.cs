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
                    try
                    {
                        string query = "";                       
                        query = string.Format("SELECT username, employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code FROM hd_wrdsb_employee_view WHERE email_address='{0}' AND home_loc_ind='Y' AND activity_code IN ('ACTIVE','ONLEAVE')", emailAddress);
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
                                Session["username"] =  reader["username"].ToString().Trim();
                            }
                        }
                        else
                        {
                            throw new Exception("The entered email address: " + emailAddress + " cannot be found. Please contact administrator for assistance.");
                        }
                        reader.Close();
                        con.Close();



                        //list_job_desc_values.Add("dummy");

                        //bool admin = false;
                        //bool job_desc_found = false;
                        //List<Users> users = new List<Users>();
                        //List<string> list_job_desc_values = new List<string>();

                        //DataTable dt = new DataTable();
                        //SQLProvider sqlp = new SQLProvider(connString);
                        //bool Success = false;

                        ////read admin usernames
                        //string query_admin = "SELECT username, admin FROM users";
                        //dt = sqlp.GetDataTable(query_admin, out Success);

                        //if (Success == false)
                        //{
                        //    Email email = new Email();
                        //    email.SendEmail(WebConfigurationManager.AppSettings["EmailAddressNotify"].ToString(), "empinquiry Error", "Query: query_admin failed.");
                        //}

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    users.Add(new Users { username = row["username"].ToString().ToLower(), admin = Convert.ToBoolean(row["admin"]) });
                        //}

                        ////read allowed job descriptions
                        //string query_job_desc_values = "SELECT value FROM job_descriptions";
                        //dt = sqlp.GetDataTable(query_job_desc_values, out Success);

                        //if (Success == false)
                        //{
                        //    Email email = new Email();
                        //    email.SendEmail(WebConfigurationManager.AppSettings["EmailAddressNotify"].ToString(), "empinquiry Error", "Query: query_job_desc_values failed.");
                        //}

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    list_job_desc_values.Add(row["value"].ToString().ToLower());
                        //}

                        ////if list of admin contains username, set admin to true to display settings tab
                        //foreach (Users i in users)
                        //{
                        //    if (i.username.Equals(Session["username"].ToString().ToLower()))
                        //    {
                        //        admin = true;
                        //        Session["admin"] = true;
                        //        Session["trillium"] = Convert.ToBoolean(i.admin);
                        //    }
                        //}

                        ////authenticate user with active directory
                        //ADProvider ad_provider = new ADProvider();


                        //string query_trillium_job_desc = string.Format("SELECT surname, first_name, job_desc FROM hd_wrdsb_employee_view WHERE username='{0}'", Session["username"].ToString().ToUpper());

                        //string surname = "";
                        //string firstname = "";
                        //List<string> depts = new List<string>();
                        //List<string> job_desc = new List<string>();

                        //SQLProvider sqlp1 = new SQLProvider(connString);

                        //dt = sqlp1.GetDataTable(query_trillium_job_desc, out Success);

                        //if (Success == false)
                        //{
                        //    Email email = new Email();
                        //    email.SendEmail(WebConfigurationManager.AppSettings["EmailAddressNotify"].ToString(), "empinquiry Error", "Query: query_trillium_job_desc failed.");
                        //}

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    if (admin || list_job_desc_values.Contains(row["job_desc"].ToString().ToLower()))
                        //    {
                        //        surname = row["surname"].ToString();
                        //        firstname = row["first_name"].ToString();
                        //        job_desc.Add(row["job_desc"].ToString().ToLower());
                        //    }
                        //}

                        //job_desc = job_desc.Distinct().ToList();

                        //Use School Code from hr view
                        //if (!admin)
                        //{
                        //    depts.Clear();

                        //    string query_school_codes = "SELECT DISTINCT(school_code) FROM hd_wrdsb_employee_view WHERE username = '" + Session["username"].ToString().ToUpper() + "'";

                        //    SQLProvider sqlp2 = new SQLProvider(connString);

                        //    dt = sqlp2.GetDataTable(query_school_codes, out Success);

                        //    if (Success == false)
                        //    {
                        //        Email email = new Email();
                        //        email.SendEmail(WebConfigurationManager.AppSettings["EmailAddressNotify"].ToString(), "empinquiry Error", "Query: query_school_codes failed.");
                        //    }
                        //    foreach (DataRow row in dt.Rows)
                        //    {
                        //        string temp = row["SCHOOL_CODE"].ToString();
                        //        if (temp.All(char.IsLetter))
                        //        {
                        //            depts.Add(row["SCHOOL_CODE"].ToString());
                        //        }
                        //    }
                        //}

                        //if no value returned or numeric value returned assume Eduction centre Location
//                        if ((depts.Any() != true || depts[0].All(char.IsDigit)) && !admin)
//                        {
//                            depts.Add("Education Centre");
//                        }

//                        //Just add anything so that the session object has a value
//                        //This will make it so that there isn't a default school auto loaded for admins
//                        //and they also wont have 120+ radio boxes if they are not excluded from the above school code query
//                        if (admin)
//                        {
//                            depts.Add("111");
//                        }

//                        //if admin or authorize, continue
//                        var inCommon = list_job_desc_values.Intersect(job_desc).Any();

//                        if (inCommon == true)
//                            job_desc_found = true;
//                        else
//                            job_desc_found = false;
//#if DEBUG
//                        //For testing purpose all authenticated users are authorized to access the app
//                        //if (true)
//                        if (admin || job_desc_found)
//#else
//                        if (admin || job_desc_found)
//#endif
                        {
                            //Session["school_code"] = depts[0].ToString();
                            //Session["department"] = depts;
                            //Session["surname"] = surname;
                            //Session["firstname"] = firstname;

                            //app.Use<EnrichIdentityWithAppUserClaims>();
                            //Create the ticket, and add the groups.
                            //bool isCookiePersistent = cb_persist.Checked;
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

                            FormsAuthentication.SetAuthCookie(emailAddress, true);

                            //if (!Convert.ToBoolean(Session["admin"]))
                            //    Response.Redirect("default.aspx");
                            //else if (Session["access_type"].ToString() == "ADMIN")
                            //    Response.Redirect("default_admin.aspx");
                            //else if (Session["access_type"].ToString() == "NONE")
                            //    throw new Exception("Insufficient Access. Please contact administrator for assistance.");

                            //FormsAuthentication.SetAuthCookie(txtUsername.Text.ToLower(), true);
                            //AuthenticationContext ac = new AuthenticationContext("https://login.windows.net/cloudidentity.net");
                            //AuthenticationResult ar = ac.AcquireToken("https://cloudidentity.net/WindowsAzureADWebAPITest", "b68f29b1-3f11-4dd0-a2a9-0f998f6189ca", new Uri("https://cloudidentity.net/myWebAPItestclient"));
                            //// Call Web API
                            //string authHeader = ar.CreateAuthorizationHeader();
                            //HttpClient client = new HttpClient();
                            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44353/api/Values");
                            //request.Headers.TryAddWithoutValidation("Authorization", authHeader);
                            //HttpResponseMessage response = await client.SendAsync(request);
                            //string responseString = await response.Content.ReadAsStringAsync();
                            //Response.Redirect("default.aspx");

                            Response.Redirect(FormsAuthentication.GetRedirectUrl(Session["username"].ToString().ToLower(), false), false);

                            Context.ApplicationInstance.CompleteRequest();

                        }
                        //else
                        //{
                        //    Session["admin"] = false;
                        //    loginErrors.InnerHtml = "You are not authorized to access this web application. " +
                        //        "Please contact your supervisor if this is an error.";
                        //    loginErrors.Visible = true;
                        //    logout2.Text = "Logout Session";
                        //    logout2.Visible = true;     
                        //}

                     }
                    catch (System.Threading.ThreadAbortException)
                    {
                        //This error is expected when using Response.Redirect() but not with the false parameter
                        //https://msdn.microsoft.com/en-us/library/a8wa7sdt.aspx
                    }
                    catch (Exception ex)
                    {
                        loginErrors.InnerHtml = ex.Message;
                        
                        // Error Logged into the table “hd_empinquiry_error_log” 
                        ErrorProvider empinquiryLoginError = new ErrorProvider(ex, "empinquiry Login Error");
                        empinquiryLoginError.LogError(ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString);

                        //Generic Error message to display in the web form
                        loginErrors.InnerText = "Login attempt failed. Please contact supervisor.";

                        loginErrors.Visible = true;
                        logout2.Text = "Logout from existing Session";
                        logout2.Visible = true;
                    }


                    //if (Session["location_code"].ToString() == "370" || Session["location_code"].ToString() == "371" || Session["group_code"].ToString() == "5100")
                    //    Session["access_type"] = "ADMIN";
                    //else if (Session["group_code"].ToString() == "5107A" || Session["group_code"].ToString() == "5108A" || Session["group_code"].ToString() == "PRINSUPE" || Session["group_code"].ToString() == "PRINSUBS" || Session["group_code"].ToString() == "6691" || Session["group_code"].ToString() == "6601")
                    //    Session["access_type"] = "USER";
                    //else
                    //    Session["access_type"] = "NONE";



                    //You can redirect now.
                    //Session["authenticated"] = true;



                }

                /*
                if(email == null || email == "")
                {
                    lblMessage.Text = "Null value or blank";
                }
                else
                {
                    lblMessage.Text = email;
                }

                lblMessage.CssClass = "red1";
                */
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


    }
}