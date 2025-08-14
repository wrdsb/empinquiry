using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using System.Web.Security;
using System.Net;
using System.Configuration;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;


namespace empinquiry
{
    public class Email
    {
        /// <summary>
        /// Sends a POST to the mailgun API which sends out email
        /// </summary>
        /// <param name="to">The recipient of the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The body content of the email, html encoding accepted</param>
        public void SendEmail(string to, string subject, string message)
        {
            try
            {
                //Configure the email template
                //Plug in the passed in message into the template
                string emailMessage =   "<div id='email' style='display: block;'>" +
                                        "<div id='logo' style='display: flex; align-items: center; margin-top: 14px;'>" +
                                        "<a href='https://www.wrdsb.ca' style='text-decoration: none; color: #000; display: flex; align-items: center;'>" +
                                        "<img src='cid:wrdsb_logo_medallion.gif' width='150' height='136' alt='Logo' style='border: none; display: block;'>" +
                                        "<h1 style='font-size:30px;font-weight:bold;margin:0;margin-left:10px;color: #005daa;'>" + @System.Web.Configuration.WebConfigurationManager.AppSettings["loginTitle"].ToString() + "</h1>" +
                                        "</a>" +
                                        "</div>" +
                                        "<div id='greenbar' style='background-color:#7ac143; height:6px;'></div>" +
                                        "<br />" +
                                        "<div id='body' style='display: block;'>" +
                                        message +
                                        "</div>" +
                                        "<div id='legal' style='display: block;'>" +
                                        "<p style='font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif;font-size: 12px;line-height: 130%;color: #333;background-color: #fff;margin: 0 10%;'>" +
                                        "Confidentiality Warning: ~This message and any attachments are intended only for the use of the intended recipient(s) and may contain confidential or personal information that may be subject to the provisions of the Municipal Freedom of Information and Protection of Privacy Act. ~If you are not the intended recipient or an authorized representative of the intended recipient, you are notified that any dissemination of this communication is strictly prohibited.~ If you have received this communication in error, please notify the sender immediately and delete the message and any attachments." +
                                        "</p></div></div>";

                //For more information on configuring this code see the c# api documentation
                //https://documentation.mailgun.com/api-sending.html#examples
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;                             
                RestClient client = new RestClient(new RestClientOptions("https://api.mailgun.net/v3") 
                { Authenticator = new HttpBasicAuthenticator("api", System.Web.Configuration.WebConfigurationManager.AppSettings["mailgunKey"].ToString()) });
                RestRequest request = new RestRequest();
                request.AddParameter("domain", System.Web.Configuration.WebConfigurationManager.AppSettings["mailgunDomain"].ToString(), ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "WRDSB (do not reply) <noreply@wrdsb.ca>");
                request.AddParameter("to", to);
                request.AddParameter("subject", subject);
                request.AddParameter("html", emailMessage);
                request.AddFile("inline", HttpContext.Current.Server.MapPath("./images/wrdsb_logo_medallion.gif"), "image/gif");
                request.Method = Method.Post;
                //RestResponse response = client.Execute(request);
                //var content = response.Content;
            }
            catch (Exception ex)
            {
                // Error Logged into the table “hd_empinquiry_error_log” 
                ErrorProvider empinquiryLoginError = new ErrorProvider(ex, "Exception occured attempting to send email.");
                empinquiryLoginError.LogError(ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString);

                Error error = new Error();
                error.handleError(ex, "Exception occured attempting to send email.");
            }
        }
        /*
        /// <summary>
        /// Emails all administrators of the application
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The body content of the email, html encoding accepted</param>
        public void EmailAdministrators(string subject, string message)
        {
            try
            {
                ApplicationUserManager UserManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                //Get a list of administrator usernames from the board users table
                var roleUsers = roleManager.Roles.Single(x => x.Name.Equals("Administrators")).Users;
                var users = UserManager.Users.Where(x => !x.UserName.Contains("@")).ToList();
                users = (from r in roleUsers join u in users on r.UserId equals u.Id select u).Distinct().ToList();

                //For each username, find the identity account and use it's email column in the call to SendMail function
                foreach (var admin in users)
                {
                    //Call Email Function
                    SendEmail(admin.Email, subject, message);
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                //Do not change this message, the error class looks for this exact message to prevent a loop
                error.handleError(ex, "Exception occured attempting to send email.");
            }
        }

        /// <summary>
        /// Emails all super admins of the application
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The body content of the email, html encoding accepted</param>
        public void EmailSuperAdmins(string subject, string message)
        {
            try
            {
                ApplicationUserManager UserManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                //Get a list of super administrator usernames from the board users table
                var roleUsers = roleManager.Roles.Single(x => x.Name.Equals("SuperAdmin")).Users;
                var users = UserManager.Users.Where(x => !x.UserName.Contains("@")).ToList();
                users = (from r in roleUsers join u in users on r.UserId equals u.Id select u).Distinct().ToList();

                //For each username, find the identity account and use it's email column in the call to SendMail function
                foreach (var admin in users)
                {
                    //Call Email Function
                    SendEmail(admin.Email, subject, message);
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                //Do not change this message, the error class looks for this exact message to prevent a loop
                error.handleError(ex, "Exception occured attempting to send email.");
            }
        }

        /// <summary>
        /// Emails all users of a specified role
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="message">The body content of the email, html encoding accepted</param>
        public void EmailSpecifiedRole(string role, string subject, string message)
        {
            try
            {
                ApplicationUserManager UserManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                //Get a list of administrator usernames from the board users table
                var roleUsers = roleManager.Roles.Single(x => x.Name.Equals(role)).Users;
                var users = UserManager.Users.Where(x => !x.UserName.Contains("@")).ToList();
                users = (from r in roleUsers join u in users on r.UserId equals u.Id select u).Distinct().ToList();

                //For each username, find the identity account and use it's email column in the call to SendMail function
                foreach (var user in users)
                {
                    //Call Email Function
                    SendEmail(user.Email, subject, message);
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                //Do not change this message, the error class looks for this exact message to prevent a loop
                error.handleError(ex, "Exception occured attempting to send email.");
            }
        }*/
    }
}