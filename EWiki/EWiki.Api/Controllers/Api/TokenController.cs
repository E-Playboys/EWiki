using EWiki.Api.DataAccess;
using EWiki.Api.Identity;
using EWiki.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EWiki.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions tokenOptions;
        private readonly IUserRepository userRepository;

        public TokenController(
            TokenAuthOptions tokenOptions,
            IUserRepository userRepository)
        {
            this.tokenOptions = tokenOptions;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Authorize("TokenAuth")]
        public dynamic Get()
        {
            /* 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            THIS METHOD SHOULD BE REMOVED IN PRODUCTION USE-CASES - IT ALLOWS A USER WITH 
            A VALID TOKEN TO REMAIN LOGGED IN FOREVER, WITH NO WAY OF EVER EXPIRING THEIR
            RIGHT TO USE THE APPLICATION.
            Refresh Tokens (see https://auth0.com/docs/refresh-token) should be used to 
            retrieve new tokens. 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            */
            bool authenticated = false;
            string user = null;
            int entityId = -1;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    foreach (Claim c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                    tokenExpires = DateTime.UtcNow.AddMinutes(2);
                    token = GetToken(currentUser.Identity.Name, tokenExpires);
                }
            }
            return new { authenticated = authenticated, user = user, entityId = entityId, token = token, tokenExpires = tokenExpires };
        }

        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic Post([FromBody] AuthRequest request)
        {
            User user = userRepository.FindBy(u => u.UserName == request.username).FirstOrDefault();
            if (request.password == user.Password)
            {
                DateTime? expires = DateTime.UtcNow.AddMinutes(2);
                var token = GetToken(request.username, expires);
                return new { authenticated = true, entityId = 1, token = token, tokenExpires = expires };
            }
            return new { authenticated = false };
        }

        private string GetToken(string user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth"), new[] { new Claim("UserName", user, ClaimValueTypes.String) });

            var securityToken = handler.CreateToken( new SecurityTokenDescriptor
            {
                Issuer = tokenOptions.Issuer,
                Audience = tokenOptions.Audience,
                SigningCredentials = tokenOptions.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }

        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
