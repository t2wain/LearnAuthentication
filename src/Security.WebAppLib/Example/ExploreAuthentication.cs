using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Security.WebAppLib.IdentityCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Security.WebAppLib.Example
{
    public class ExploreAuthentication
    {

        #region Init

        IServiceProvider _provider;
        IAuthenticationService _authenticationService;
        IAuthenticationSchemeProvider _authenticationSchemeProvider;
        IOptionsMonitor<CookieAuthenticationOptions> _cookieOptions;
        UserManager<IMyUser> _userManager;
        SignInManager<IMyUser> _signInManager;
        IHttpContextAccessor _httpContextAccessor;

        public ExploreAuthentication(
            IServiceProvider provider,
            IAuthenticationService authenticationService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IOptionsMonitor<CookieAuthenticationOptions> cookieOptions,
            UserManager<IMyUser> userManager,
            SignInManager<IMyUser> signInManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _provider = provider;
            this._authenticationService = authenticationService;
            this._authenticationSchemeProvider = authenticationSchemeProvider;
            this._cookieOptions = cookieOptions;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion

        public async Task Run(int testNo = 0)
        {
            int? o = 4;
            var t = o ?? testNo;
            switch(t)
            {
                case 0:
                    ExploreSignInManager();
                    break;
                case 1:
                    await ExploreAuthenticationSchemes();
                    break;
                case 2:
                    ExploreCookieAuthenticationOptions();
                    break;
                case 3:
                    await ExploreAuthenticationService();
                    break;
                case 4:
                    var identity = CreateIdentity();
                    await SignInWithHttpContext(identity);
                    break;
            }
        }

        #region ExploreAuthenticationSchemes

        public async Task ExploreAuthenticationSchemes()
        {
            // default configred authentication shemes
            string s = IdentityConstants.ApplicationScheme;
            s = IdentityConstants.ExternalScheme;
            s = IdentityConstants.TwoFactorUserIdScheme;
            s = IdentityConstants.TwoFactorRememberMeScheme;

            AuthenticationScheme defaultScheme = 
                await _authenticationSchemeProvider.GetDefaultAuthenticateSchemeAsync();
            ExploreAuthenticationScheme(defaultScheme);

            IEnumerable<AuthenticationScheme> schemes = 
                await this._authenticationSchemeProvider.GetAllSchemesAsync();
            foreach (var scheme in schemes)
            {
                string n1 = scheme.Name;
                string t = scheme.HandlerType.Name;
                ExploreAuthenticationScheme(scheme);
            }
        }

        #endregion

        #region ExploreAuthenticationService

        public async Task ExploreAuthenticationService()
        {
            if (_authenticationService is AuthenticationService auth)
            {
                IAuthenticationSchemeProvider schemes = auth.Schemes;
                await ExploreAuthenticationSchemeProvider(schemes);

                IAuthenticationHandlerProvider handlers = auth.Handlers;
                await ExploreAuthenticationHandlerProvider(handlers);

                IClaimsTransformation transform =  auth.Transform;
            }
        }

        public async Task ExploreAuthenticationHandlerProvider(IAuthenticationHandlerProvider handlers)
        {
            if (GetHttpContext() is HttpContext httpContext 
                && handlers is AuthenticationHandlerProvider provider)
            {
                await ExploreAuthenticationSchemeProvider(provider.Schemes);

                IAuthenticationHandler handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.ApplicationScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.ExternalScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.TwoFactorUserIdScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.TwoFactorRememberMeScheme);
            }
        }

        public async Task ExploreAuthenticationSchemeProvider(IAuthenticationSchemeProvider schemes)
        {
            if (schemes is AuthenticationSchemeProvider schemeProvider)
            {
                AuthenticationScheme signInScheme = 
                    await schemeProvider.GetDefaultSignInSchemeAsync();

                AuthenticationScheme signOutScheme = 
                    await schemeProvider.GetDefaultSignOutSchemeAsync();

                AuthenticationScheme challengeScheme = 
                    await schemeProvider.GetDefaultChallengeSchemeAsync();

                AuthenticationScheme forbidScheme = 
                    await schemeProvider.GetDefaultForbidSchemeAsync();

                AuthenticationScheme authenticateScheme = 
                    await schemeProvider.GetDefaultAuthenticateSchemeAsync();

                IEnumerable<AuthenticationScheme> allSchemes = 
                    await schemeProvider.GetAllSchemesAsync();

                foreach (var scheme in allSchemes)
                {
                    string n = scheme.Name;
                    ExploreAuthenticationScheme(scheme);
                }
            }
        }

        #endregion

        #region CookieAuthenticationOptions

        public void ExploreCookieAuthenticationOptions()
        {

            PathString ps = CookieAuthenticationDefaults.LogoutPath;
            ps = CookieAuthenticationDefaults.LoginPath;
            string s = CookieAuthenticationDefaults.ReturnUrlParameter;
            s = CookieAuthenticationDefaults.CookiePrefix;
            s = CookieAuthenticationDefaults.AuthenticationScheme;

            CookieAuthenticationOptions options = _cookieOptions.Get(IdentityConstants.ApplicationScheme);
            if (options != null)
                ExploreCookieOptions(options);

            options = _cookieOptions.Get(IdentityConstants.ExternalScheme);
            if (options != null)
                ExploreCookieOptions(options);

            options = _cookieOptions.Get(IdentityConstants.TwoFactorUserIdScheme);
            if (options != null)
                ExploreCookieOptions(options);

            options = _cookieOptions.Get(IdentityConstants.TwoFactorRememberMeScheme);
            if (options != null)
                ExploreCookieOptions(options);
        }

        public void ExploreCookieOptions(CookieAuthenticationOptions options)
        {
            AuthenticationSchemeOptions o1 = options;

            string? s = o1.ClaimsIssuer;
            s = o1.ForwardAuthenticate;
            s = o1.ForwardChallenge;
            s = o1.ForwardDefault;
            s = o1.ForwardForbid;
            s = o1.ForwardSignIn;
            s = o1.ForwardSignOut;
            object? e = o1.Events;
            s = o1.EventsType?.Name;

            s = options.AccessDeniedPath;
            s = options.ReturnUrlParameter;
            CookieBuilder b = options.Cookie;
            ICookieManager cm = options.CookieManager;
            IDataProtectionProvider dp = options.DataProtectionProvider;
            CookieAuthenticationEvents ev = options.Events;
            TimeSpan ex = options.ExpireTimeSpan;
            PathString ps = options.LoginPath;
            ps = options.LogoutPath;
            ITicketStore ts = options.SessionStore;
            bool b2 = options.SlidingExpiration;
            ISecureDataFormat<AuthenticationTicket> f = options.TicketDataFormat;
        }

        #endregion

        #region SignInManager

        public void ExploreSignInManager()
        {
            if (GetHttpContext(_signInManager) is HttpContext context)
            {
                ExploreHttpContext(context);
                var b = _signInManager.IsSignedIn(context.User);

                var identity = CreateIdentity(IdentityConstants.ApplicationScheme);
                context.User = new ClaimsPrincipal(identity);

                ExploreHttpContext(context);
                // signInManager only recognizes identity with
                // authentication type of IdentityConstants.ApplicationScheme
                b = _signInManager.IsSignedIn(context.User);
            }

            IdentityOptions options = _signInManager.Options;
            UserManager<IMyUser> userManager = _signInManager.UserManager;
            IUserClaimsPrincipalFactory<IMyUser> claimFact = _signInManager.ClaimsFactory;
        }

        public async Task SignInWithManager(IMyUser user)
        {
            try { await _signInManager.SignInAsync(user, true); }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            if (GetHttpContext(_signInManager) is HttpContext httpContext)
                ExploreHttpContext(httpContext);
        }

        public async Task SignInWithHttpContext(ClaimsIdentity identity)
        {
            var u = new ClaimsPrincipal(identity);
            if (GetHttpContext(_signInManager) is HttpContext httpContext)
            {
                try 
                { 
                    await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, u); 
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        public async Task SignInWithAuthenticationService(ClaimsIdentity identity)
        {
            var u = new ClaimsPrincipal(identity);
            if (GetHttpContext(_signInManager) is HttpContext httpContext
                && _authenticationService is AuthenticationService authService)
            {
                try { 
                    await authService.SignInAsync(
                        httpContext, 
                        IdentityConstants.ApplicationScheme, 
                        u, 
                        new()); 
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        public async Task SignInWithCookieHandler(ClaimsIdentity identity)
        {
            var u = new ClaimsPrincipal(identity);
            if (_authenticationService is AuthenticationService authService)
            {
                AuthenticationScheme signInScheme = 
                    await authService.Schemes.GetDefaultSignInSchemeAsync();
                string n = signInScheme.Name;

                object? o = _provider.GetService(signInScheme.HandlerType);
                if (o is CookieAuthenticationHandler cookieHandler)
                {
                    try
                    {
                        await cookieHandler.SignInAsync(u, new());
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }
            }
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Explore

        public void ExploreHttpContext(HttpContext httpContext)
        {
            ClaimsPrincipal user = httpContext.User;
            IEnumerable<Claim> claims = user.Claims;
            var cnt = claims.Count();

            IEnumerable<IIdentity> identities = httpContext.User.Identities;
            cnt = user.Identities.Count();
            if (user.Identity is IIdentity identity)
            {
                bool b = identity.IsAuthenticated;
                string? name = identity.Name;
                string? scheme = identity.AuthenticationType;
            }

        }

        public void ExploreAuthenticationScheme(AuthenticationScheme scheme)
        {
            string n = scheme.DisplayName;
            string n1 = scheme.Name;
            string t = scheme.HandlerType.Name;
            object? o = _provider.GetService(scheme.HandlerType);

            bool b = o is IAuthenticationSignInHandler;
            b = o is IAuthenticationSignOutHandler;

            if (o is IAuthenticationHandler handler)
            {
                ExploreAuthenticationHandler(handler);
            }
        }

        public void ExploreAuthenticationHandler(IAuthenticationHandler handler)
        {
            bool b = handler is IAuthenticationSignInHandler;
            b = handler is IAuthenticationSignOutHandler;
            if (handler is CookieAuthenticationHandler cookieHandler)
            {
                ExploreCookieAuthenticationHandler(cookieHandler);
            };
        }

        public void ExploreCookieAuthenticationHandler(CookieAuthenticationHandler handler)
        {
            AuthenticationScheme s = handler.Scheme;
            if (handler.Options is CookieAuthenticationOptions op)
                ExploreCookieOptions(op);

            if (handler is AuthenticationHandler<CookieAuthenticationOptions> authHandler)
            {
                bool b = authHandler is IAuthenticationSignInHandler;
                b = authHandler is IAuthenticationSignOutHandler;
            }

            if (handler is SignInAuthenticationHandler<CookieAuthenticationOptions> signInHandler)
            {

            }

            if (handler is SignOutAuthenticationHandler<CookieAuthenticationOptions> sigOutnHandler)
            {

            }
        }

        #endregion

        #region Test data

        public HttpContext? GetHttpContext(object? o = null)
        {
            HttpContext? c = null;
            try
            {
                c = o switch
                {
                    SignInManager<IMyUser> s => s.Context,
                    _ => _httpContextAccessor.HttpContext
                };
            }
            catch { }
            return c;
        }

        public IMyUser CreateUser()
        {
            var user = new MyUser()
            {
                Name = "Joe Smith",
                UserName = "U123A",
                ID = "123",
                Email = "joe.smith@example.com",
                SecurityStamp = "123"
            };
            return user;
        }

        public ClaimsIdentity CreateIdentity(string? authenticationType = null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "Joe Smith"),
            };
            var identity = new ClaimsIdentity(claims, authenticationType);
            return identity;
        }

        #endregion
    }
}
