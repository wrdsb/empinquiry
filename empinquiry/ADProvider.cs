using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.DirectoryServices;

namespace empinquiry
{
    public class ADProvider
    {
        private PrincipalContext Context;
        private string Domain;
        private string DomainController;
        private string DomainRoot;

        public ADProvider()
        {
            //default contructor every instance gets our domain context
            this.Domain = ConfigurationManager.AppSettings["Domain"].ToString();
            this.DomainController = ConfigurationManager.AppSettings["DomainController"].ToString();
            this.DomainRoot = ConfigurationManager.AppSettings["DomainRoot"].ToString();
        }

        public PrincipalContext GetDomainContext()
        {
            return this.Context;
        }

        public UserPrincipal GetUserPrincipal(string username)
        {
            return UserPrincipal.FindByIdentity(this.Context, username);
        }

        public GroupPrincipal GetGroupPrincipal(string groupname)
        {
            return GroupPrincipal.FindByIdentity(this.Context, groupname);
        }

        public string GetUserEmail(string username)
        {
            UserPrincipal user = GetUserPrincipal(username);
            return user.EmailAddress;
        }

        public bool UserIsMemberOfGroup(string username, string groupname)
        {
            UserPrincipal user = GetUserPrincipal(username);
            GroupPrincipal group = GetGroupPrincipal(groupname);

            if (user.IsMemberOf(group))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetGroupMembers(string adGroupName)
        {
            List<string> accounts = new List<string>();

            GroupPrincipal group = GetGroupPrincipal(adGroupName);

            if (group != null)
            {
                foreach (Principal p in group.GetMembers())
                {
                    //Make sure p is a user and not a group within a group
                    if (p != null && p is UserPrincipal)
                    {
                        //Filter out non character names as well
                        if (p.SamAccountName.All(Char.IsLetter))
                        {
                            accounts.Add(p.SamAccountName.ToLower());
                        }
                    }
                }
            }
            return accounts;
        }

        public bool Authenticate(string username, string password)
        {
            DirectoryEntry deEntry = new DirectoryEntry();
            try
            {
                deEntry = new DirectoryEntry("LDAP://" + this.DomainController, this.Domain + "\\" + username, password, AuthenticationTypes.ServerBind);

                //Bind to the native AdsObject to force authentication.			
                Object obj = deEntry.NativeObject;
                deEntry.Dispose();

                return true;
            }
            catch (DirectoryServicesCOMException exception)
            {
                //Invalid password/username throws this exception
                deEntry.Dispose();
                return false;
            }
            catch (Exception ex)
            {

                ICollection<KeyValuePair<string, string>> empinquiryInfo = new Dictionary<string, string>();
                empinquiryInfo.Add(new KeyValuePair<string, string>("Error Date", DateTime.Now.ToString()));
                ErrorProvider empinquirySubmitError = new ErrorProvider(ex, "AD authentication error", empinquiryInfo);
                empinquirySubmitError.LogError(ConfigurationManager.ConnectionStrings["SQLDB"].ConnectionString);

                Error error = new Error();
                error.handleError(ex, "AD authentication error");
                deEntry.Dispose();
                return false;
            }
        }
    }
}