using System.Security.Principal;

namespace EWiki.Api.Filters
{
    public class EWikiPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public int UserRole { get; set; }

        public EWikiPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string roles)
        {
            if (Identity == null || !Identity.IsAuthenticated)
            {
                return false;
            }

            string[] roleList = roles.Split(',');
            bool result = false;

            foreach (string role in roleList)
            {
                switch (role)
                {
                    case "Admin":
                        result = this.UserRole == (int)Roles.Admin;
                        break;
                    case "PremiumUser":
                        result = this.UserRole == (int)Roles.PremiumUser;
                        break;
                    case "User":
                        result = this.UserRole == (int)Roles.User;
                        break;
                    case "Guest":
                        result = this.UserRole == (int)Roles.Guest;
                        break;
                }

                if (result == true)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
