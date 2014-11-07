using System.Collections.Generic;
using System.Security.Claims;

namespace MultipleClaimsSample
{
    class Program
    {
        const string AuthServerName = "urn:myauthserver";
        const string TenantAuthType = "Application";
        const string UserAuthType = "External";

        static void Main(string[] args)
        {
            // NOTE: The below is a sample of how we may construct a ClaimsPrincipal instance over two ClaimsIdentity instances:
            //       one for the tenant identity and the the other for the user idenetity. When a request come to the web server, we can determine the
            //       tenant's identity at the very early stages of the request lifecycle. Then, we can try to authenticate the user based on the 
            //       information passed through the request headers (this could be bearer token, basic auth, etc.).

            const string tenantId = "f35fe69d-7aef-4f1a-b645-0de4176cd441";
            const string tenantName = "bigcompany";
            IEnumerable<Claim> tenantClaims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, tenantId, ClaimValueTypes.String, AuthServerName),
                new Claim(ClaimTypes.Name, tenantName, ClaimValueTypes.String, AuthServerName)
            };

            const string userId = "d4903f71-ca06-4671-a3df-14f7e02a0008";
            const string userName = "tugberk";
            const string twitterToken = "30807826f0d74ed29d69368ea5faee2638b0e931566b4e4092c1aca9b4db04fe";
            const string facebookToken = "35037356a183470691504cd163ce2f835419978ed81c4b7781ae3bbefdea176a";
            IEnumerable<Claim> userClaims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, AuthServerName),
                new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String, AuthServerName),
                new Claim("token", twitterToken, ClaimValueTypes.String, AuthServerName, "Twitter"),
                new Claim("token", facebookToken, ClaimValueTypes.String, AuthServerName, "Facebook")
            };

            ClaimsIdentity tenantIdentity = new ClaimsIdentity(tenantClaims, TenantAuthType, ClaimTypes.Name, ClaimTypes.Role);
            ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims, UserAuthType, ClaimTypes.Name, ClaimTypes.Role);

            ClaimsPrincipal principal = new ClaimsPrincipal(new[] { tenantIdentity, userIdentity });
        }

        public static class ClaimsPrincipalExtensions
        {
        }
    }
}