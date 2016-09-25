using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWiki.Api.Filters
{
    public class EWikiAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        public EWikiAuthorizeAttribute()
            : base()
        {
        }

        public EWikiAuthorizeAttribute(string roles)
            : base()
        {
            Roles = roles;
        }

        /*
        protected virtual EWikiPrincipal CurrrentUser
        {
            get { return HttpContext.User as EWikiPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrrentUser != null)
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                    var authorizeRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                    Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                    Roles = String.IsNullOrEmpty(Roles) ? authorizeRoles : Roles;
                }

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!CurrrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Error",
                            action = "AccessDenied"
                        }));
                    }
                }

                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrrentUser.StaffUserName))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Error",
                            action = "AccessDenied"
                        }));
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                }));
            }
        }
        */
    }
}
