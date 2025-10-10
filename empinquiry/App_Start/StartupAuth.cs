using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.Owin.Security;
using System.Globalization;
using System.Linq;
using Microsoft.Owin.Extensions;
namespace empinquiry

{

    public partial class ad_group_roles
    {
        public int id { get; set; }
        public string group_name { get; set; }
        public string role_name { get; set; }
    }
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, "Cookies");

            // Add custom user claims here


            return userIdentity;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SQLForms", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ad_group_roles> ad_group_roles { get; set; }
    }
 
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        // Concatenate aadInstance, tenant to form authority value       
        private string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        // ConfigureAuth method  
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);



            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //{
            //    AuthenticationType = "Cookies",
            //    LoginPath = new PathString("/login.aspx"),
            //    CookieSecure = CookieSecureOption.Never,
            //    CookieManager = new Microsoft.Owin.Host.SystemWeb.SystemWebChunkingCookieManager(),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //              validateInterval: TimeSpan.FromMinutes(30),
            //              regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});



            app.UseOpenIdConnectAuthentication(

                              new OpenIdConnectAuthenticationOptions
                              {
                                  ClientId = clientId,
                                  Authority = authority,
                                  PostLogoutRedirectUri = postLogoutRedirectUri,
                                  Notifications = new OpenIdConnectAuthenticationNotifications()
                                  {
                                      AuthenticationFailed = (context) =>
                                      {
                                          return System.Threading.Tasks.Task.FromResult(0);
                                      },
                                      SecurityTokenValidated = (context) =>
                                      {
                                          var claims = context.AuthenticationTicket.Identity.Claims;
                                          var groups = from c in claims
                                                       where c.Type == "groups"
                                                       select c;
                                          foreach (var group in groups)
                                          {
                                              context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, group.Value));
                                              context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email, group.Value));
                                              context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, group.Value));
                                              //context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes., group.Value));
                                          }

                                          return Task.FromResult(0);
                                      }
                                  }
                              });
            // This makes any middleware defined above this line run before the Authorization rule is applied in web.config
            app.UseStageMarker(PipelineStage.Authenticate);

            //      app.Use<EnrichIdentityWithAppUserClaims>();

        } // end - ConfigureAuth method  

        public bool AuthenticateAD(string username, string password)
        {
            //using (var context = new PrincipalContext(ContextType.Domain, System.Web.Configuration.WebConfigurationManager.AppSettings["adAuthURL"].ToString(), username, password))
            using (var context = new PrincipalContext(ContextType.Domain, "ec-dc1.wrdsb.ca", username, password))
            {
                return context.ValidateCredentials(username, password);
            }
        }
        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
