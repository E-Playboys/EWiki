using Microsoft.IdentityModel.Tokens;

namespace EWiki.Api.Identity
{
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
