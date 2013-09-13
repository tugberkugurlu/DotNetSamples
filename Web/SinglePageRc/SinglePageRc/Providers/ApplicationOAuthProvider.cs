using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace SinglePageRc.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IIdentityManagerFactory _identityManagerFactory;
        private readonly CookieAuthenticationOptions _cookieOptions;

        public ApplicationOAuthProvider(string publicClientId, IIdentityManagerFactory identityManagerFactory,
            CookieAuthenticationOptions cookieOptions)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (identityManagerFactory == null)
            {
                throw new ArgumentNullException("_identityManagerFactory");
            }

            if (cookieOptions == null)
            {
                throw new ArgumentNullException("cookieOptions");
            }

            _publicClientId = publicClientId;
            _identityManagerFactory = identityManagerFactory;
            _cookieOptions = cookieOptions;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (IdentityManager identityManager = _identityManagerFactory.CreateStoreManager())
            {
                if (!await identityManager.Passwords.CheckPasswordAsync(context.UserName, context.Password))
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                string userId = await identityManager.Logins.GetUserIdForLocalLoginAsync(context.UserName);
                IEnumerable<Claim> claims = await GetClaimsAsync(identityManager, userId);
                ClaimsIdentity oAuthIdentity = CreateIdentity(identityManager, claims,
                    context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = CreateIdentity(identityManager, claims,
                    _cookieOptions.AuthenticationType);
                AuthenticationProperties properties = await CreatePropertiesAsync(identityManager, userId);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                if (RequestExtensions.IsUrlLocalToHost(null, context.RedirectUri))
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static ClaimsIdentity CreateIdentity(IdentityManager identityManager, IEnumerable<Claim> claims,
            string authenticationType)
        {
            if (identityManager == null)
            {
                throw new ArgumentNullException("identityManager");
            }

            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }

            IdentityAuthenticationOptions options = identityManager.Settings.GetAuthenticationOptions();
            return new ClaimsIdentity(claims, authenticationType, options.UserNameClaimType, options.RoleClaimType);
        }

        public static async Task<AuthenticationProperties> CreatePropertiesAsync(IdentityManager identityManager,
            string userId)
        {
            if (identityManager == null)
            {
                throw new ArgumentNullException("identityStore");
            }

            IUser user = await identityManager.Store.Users.FindAsync(userId, CancellationToken.None);
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName }
            };
            return new AuthenticationProperties(data);
        }

        public static Task<IList<Claim>> GetClaimsAsync(IdentityManager identityManager, string userId)
        {
            if (identityManager == null)
            {
                throw new ArgumentNullException("identityManager");
            }

            AuthenticationManager authenticationManager = new AuthenticationManager(
                identityManager.Settings.GetAuthenticationOptions(), identityManager);

            return authenticationManager.GetUserIdentityClaimsAsync(userId, new Claim[0], CancellationToken.None);
        }
    }
}