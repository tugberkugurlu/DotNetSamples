using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SinglePageRc.Models;
using SinglePageRc.Providers;
using SinglePageRc.Results;

namespace SinglePageRc.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

        public AccountController()
            : this(Startup.IdentityManagerFactory.CreateStoreManager(), Startup.OAuthOptions, Startup.CookieOptions)
        {
        }

        public AccountController(IdentityManager identityManager, OAuthAuthorizationServerOptions oAuthOptions,
            CookieAuthenticationOptions cookieOptions)
        {
            IdentityManager = identityManager;
            OAuthOptions = oAuthOptions;
            CookieOptions = cookieOptions;
        }

        public IdentityManager IdentityManager { get; private set; }
        public OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public CookieAuthenticationOptions CookieOptions { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(Startup.ExternalOAuthAuthenticationType)]
        [HttpGet("UserInfo")]
        public UserInfoViewModel UserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        [HttpPost("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieOptions.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [HttpGet("ManageInfo")]
        public async Task<ManageInfoViewModel> ManageInfo(string returnUrl, bool generateState = false)
        {
            IEnumerable<IUserLogin> linkedAccounts = await IdentityManager.Logins.GetLoginsAsync(
                User.Identity.GetUserId());
            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IUserLogin linkedAccount in linkedAccounts)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = IdentityManager.Settings.GetIdentityOptions().LocalLoginProvider,
                UserName = User.Identity.GetUserName(),
                Logins = logins,
                ExternalLoginProviders = ExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [HttpPost("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await IdentityManager.Passwords.ChangePasswordAsync(User.Identity.GetUserName(),
                model.OldPassword, model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [HttpPost("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await IdentityManager.Logins.AddLocalLoginAsync(User.Identity.GetUserId(),
                User.Identity.GetUserName(), model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [HttpPost("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthenticationTicket ticket = OAuthOptions.AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await IdentityManager.Logins.AddLoginAsync(User.Identity.GetUserId(),
                externalData.LoginProvider, externalData.ProviderKey);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [HttpPost("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await IdentityManager.Logins.RemoveLoginAsync(User.Identity.GetUserId(),
                model.LoginProvider, model.ProviderKey);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(Startup.ExternalCookieAuthenticationType)]
        [AllowAnonymous]
        [HttpGet("ExternalLogin", RouteName = "ExternalLogin")]
        public async Task<IHttpActionResult> ExternalLogin(string provider)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            string userId = await IdentityManager.Logins.GetUserIdForLoginAsync(externalLogin.LoginProvider,
                externalLogin.ProviderKey);

            bool hasRegistered = userId != null;

            if (hasRegistered)
            {
                Authentication.SignOut(Startup.ExternalCookieAuthenticationType);
                IEnumerable<Claim> claims = await ApplicationOAuthProvider.GetClaimsAsync(IdentityManager, userId);
                ClaimsIdentity oAuthIdentity = ApplicationOAuthProvider.CreateIdentity(IdentityManager, claims,
                    OAuthOptions.AuthenticationType);
                ClaimsIdentity cookieIdentity = ApplicationOAuthProvider.CreateIdentity(IdentityManager, claims,
                    CookieOptions.AuthenticationType);
                AuthenticationProperties properties = await ApplicationOAuthProvider.CreatePropertiesAsync(
                    IdentityManager, userId);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = ApplicationOAuthProvider.CreateIdentity(IdentityManager, claims,
                    OAuthOptions.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [HttpGet("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> ExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                state = GenerateAntiForgeryState();
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = returnUrl,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User(model.UserName);
            IdentityResult result = await IdentityManager.Users.CreateLocalUserAsync(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(Startup.ExternalOAuthAuthenticationType)]
        [HttpPost("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            User user = new User(model.UserName);
            IdentityResult result = await IdentityManager.Users.CreateExternalUserAsync(user,
                externalLogin.LoginProvider, externalLogin.ProviderKey);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IdentityManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private string GenerateAntiForgeryState()
        {
            const int strengthInBits = 256;
            const int strengthInBytes = strengthInBits / 8;
            byte[] data = new byte[strengthInBytes];
            _random.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Success)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        #endregion
    }
}
