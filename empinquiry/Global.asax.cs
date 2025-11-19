using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Security.Principal;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;
using System.Configuration;

namespace empinquiry
{
    public class Global : System.Web.HttpApplication
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString;
        public static bool log = false;
        protected void Application_Start(object sender, EventArgs e)
        {
            log = Convert.ToBoolean(WebConfigurationManager.AppSettings["Log"]);
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        void Application_Error(object sender, EventArgs e)
        {
            Loggers.Log("Application Error Occurred");
            HttpUnhandledException httpUnhandledException = new HttpUnhandledException(Server.GetLastError().Message, Server.GetLastError());
            Loggers.Log("Application Error Occurred : " + httpUnhandledException.InnerException.InnerException.Source + "\n" + httpUnhandledException.GetHtmlErrorMessage());
            Loggers.Log("Application Error Occurred : " + httpUnhandledException.InnerException.InnerException.Message);
            Loggers.Log("Application Error Occurred : " + httpUnhandledException.InnerException.InnerException.StackTrace);                     
        }

        void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            String cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {//There is no authentication cookie.
                return;
            }

            FormsAuthenticationTicket authTicket = null;

            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                Loggers.Log("Application_AuthenticateRequest: " + ex.Message);
                Loggers.Log("Application_AuthenticateRequest: " + ex.StackTrace);
                Loggers.Log("Application_AuthenticateRequest: Cookie Value: " + authCookie.Value);
                return;
            }

            if (null == authTicket)
            {//Cookie failed to decrypt.
                return;
            }

            //When the ticket was created, the UserData property was assigned a
            //pipe-delimited string of group names.
            String[] groups = authTicket.UserData.Split(new char[] { '|' });

            //Create an Identity.
            GenericIdentity id = new GenericIdentity(authTicket.Name, "LdapAuthentication");

            //This principal flows throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, groups);

            Context.User = principal;
        }
    }
}