using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
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

        public readonly string AuthCookieName = 
            $"{CookieAuthenticationDefaults.CookiePrefix}{IdentityConstants.ApplicationScheme}";

        /// <summary>
        /// Services available from DI container
        /// </summary>
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
            int? o = 5;
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
                    var identity = CreateIdentity(IdentityConstants.ApplicationScheme);
                    await SignInWithHttpContext(identity);
                    break;
                case 5:
                    await ExploreJwtScheme();
                    break;
            }
        }

        #region ExploreAuthenticationSchemes

        /// <summary>
        /// List all the registered authentication shemes
        /// </summary>
        public async Task ExploreAuthenticationSchemes()
        {
            // default configured authentication shemes
            // when adding the Identity framework
            string s = IdentityConstants.ApplicationScheme;
            s = IdentityConstants.ExternalScheme;
            s = IdentityConstants.TwoFactorUserIdScheme;
            s = IdentityConstants.TwoFactorRememberMeScheme;

            AuthenticationScheme? defaultAuthenticateScheme = 
                await _authenticationSchemeProvider.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticateScheme != null)
                ExploreAuthenticationScheme(defaultAuthenticateScheme);

            AuthenticationScheme? defaultChallengeScheme =
                await _authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
            AuthenticationScheme? defaultForbidScheme =
                await _authenticationSchemeProvider.GetDefaultForbidSchemeAsync();
            AuthenticationScheme? defaultSignInScheme =
                await _authenticationSchemeProvider.GetDefaultSignInSchemeAsync();
            AuthenticationScheme? defaultSignOutScheme =
                await _authenticationSchemeProvider.GetDefaultSignOutSchemeAsync();

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

                // authentication logic is implemented by a specific handler
                // for each authentication scheme
                IAuthenticationHandler? handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.ApplicationScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.ExternalScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.TwoFactorUserIdScheme);

                handler = await provider.GetHandlerAsync(
                    httpContext, IdentityConstants.TwoFactorRememberMeScheme);
            }
        }

        public record DefaultSchemeNames(
            string? SignInScheme, 
            string? SignOutScheme, 
            string? ChallengeScheme, 
            string? ForbidScheme, 
            string? AuthenticateScheme);

        /// <summary>
        /// Explore the configuration of various default authentication schemes
        /// for various authentication behaviors
        /// </summary>
        public async Task<DefaultSchemeNames?> ExploreAuthenticationSchemeProvider(
            IAuthenticationSchemeProvider schemes)
        {
            DefaultSchemeNames? defaultSchemes = null;

            if (schemes is AuthenticationSchemeProvider schemeProvider)
            {
                // handler reponsible for sign-in logic
                AuthenticationScheme? signInScheme = 
                    await schemeProvider.GetDefaultSignInSchemeAsync();

                // handler reponsible for sign-out logic
                AuthenticationScheme? signOutScheme = 
                    await schemeProvider.GetDefaultSignOutSchemeAsync();

                // redirect ot log-in page
                AuthenticationScheme? challengeScheme = 
                    await schemeProvider.GetDefaultChallengeSchemeAsync();

                // redirect to unauthorize page
                AuthenticationScheme? forbidScheme = 
                    await schemeProvider.GetDefaultForbidSchemeAsync();

                // reconstruct PrincipalClaims from cookie/token
                AuthenticationScheme? authenticateScheme = 
                    await schemeProvider.GetDefaultAuthenticateSchemeAsync();

                defaultSchemes = new(signInScheme?.Name, 
                    signOutScheme?.Name, 
                    challengeScheme?.Name, 
                    forbidScheme?.Name, 
                    authenticateScheme?.Name);

                IEnumerable<AuthenticationScheme> allSchemes = 
                    await schemeProvider.GetAllSchemesAsync();

                foreach (var scheme in allSchemes)
                {
                    string n = scheme.Name;
                    ExploreAuthenticationScheme(scheme);
                }
            }

            return defaultSchemes;
        }

        #endregion

        #region CookieAuthenticationOptions

        /// <summary>
        /// Configuration for authentication cookie
        /// </summary>
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
            IDataProtectionProvider? dp = options.DataProtectionProvider;
            CookieAuthenticationEvents ev = options.Events;
            TimeSpan ex = options.ExpireTimeSpan;
            PathString ps = options.LoginPath;
            ps = options.LogoutPath;
            ITicketStore? ts = options.SessionStore;
            bool b2 = options.SlidingExpiration;
            ISecureDataFormat<AuthenticationTicket> f = options.TicketDataFormat;
        }

        #endregion

        #region Explore SignInManager

        /// <summary>
        /// Sign-In logic
        /// </summary>
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

        #endregion

        #region Test SignIn

        public DefaultSchemeNames? DefaultSchemes 
        { 
            get
            {
                if (field == null)
                    field = ExploreAuthenticationSchemeProvider(_authenticationSchemeProvider).Result;
                return field;
            }
            protected set; 
        }

        /// <summary>
        /// Various sign-In logic
        /// </summary>
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

        /// <summary>
        /// Various sign-In logic
        /// </summary>
        public async Task SignInWithHttpContext(ClaimsIdentity identity)
        {
            var u = new ClaimsPrincipal(identity);
            if (GetHttpContext(_signInManager) is HttpContext httpContext)
            {
                try 
                {
                    string signInScheme = DefaultSchemes?.SignInScheme ?? IdentityConstants.ApplicationScheme;
                    await httpContext.SignInAsync(signInScheme, u);
                    ValidateSignIn(signInScheme, httpContext.Response);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        /// <summary>
        /// Various sign-In logic
        /// </summary>
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

        /// <summary>
        /// Various sign-In logic
        /// </summary>
        public async Task SignInWithCookieHandler(ClaimsIdentity identity)
        {
            var u = new ClaimsPrincipal(identity);
            if (_authenticationService is AuthenticationService authService)
            {
                AuthenticationScheme? signInScheme = 
                    await authService.Schemes.GetDefaultSignInSchemeAsync();

                object? o = null;
                if (signInScheme != null && signInScheme.HandlerType != null)
                {
                    o = _provider.GetService(signInScheme.HandlerType);
                    string n = signInScheme.Name;
                }

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

        public void ValidateSignIn(string signInScheme, HttpResponse response)
        {
            SetCookieHeaderValue? authCookie = ExploreAuthCookies(response);
            if (authCookie != null)
            {
                AuthenticationTicket? ticket =
                    ParseAuthCookie(signInScheme, authCookie.Value.ToString());
                if (ticket != null)
                    ExploreAuthenticationTicket(ticket);
            }
        }

        public AuthenticationTicket? ParseAuthCookie(string scheme, string cookieValue)
        {
            CookieAuthenticationOptions options = _cookieOptions.Get(scheme);
            AuthenticationTicket? ticket = options.TicketDataFormat.Unprotect(cookieValue);
            return ticket;
        }

        #endregion

        #region JWT

        public async Task ExploreJwtScheme()
        {
            if (await GetJwtBearerHandler() is JwtBearerHandler jwtHandler)
            {
                JwtBearerOptions options = 
                    _provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().CurrentValue;
                TokenValidationParameters validation = options.TokenValidationParameters;
            }
        }

        public async Task<JwtBearerHandler?> GetJwtBearerHandler()
        {
            AuthenticationScheme? scheme =
                await _authenticationSchemeProvider.GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme);
            if (scheme != null
                && _provider.GetService(scheme.HandlerType) is JwtBearerHandler jwtHandler)
            {
                return jwtHandler;
            }
            return null;
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
            string? n = scheme.DisplayName;
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

        public SetCookieHeaderValue? ExploreAuthCookies(HttpResponse response)
        {
            SetCookieHeaderValue? authCookie = null;
            if (response.Headers.TryGetValue(HeaderNames.SetCookie, out StringValues vals))
            {
                foreach (string? val in vals)
                {
                    if (SetCookieHeaderValue.TryParse(val, out SetCookieHeaderValue? cookieVal))
                    {
                        StringSegment s = cookieVal.Domain;
                        DateTimeOffset? d = cookieVal.Expires;
                        IList<StringSegment> e = cookieVal.Extensions;
                        bool b = cookieVal.HttpOnly;
                        TimeSpan? t = cookieVal.MaxAge;
                        s = cookieVal.Name;
                        s = cookieVal.Path;
                        var m = cookieVal.SameSite;
                        b = cookieVal.Secure;
                        s = cookieVal.Value;

                        if (cookieVal.Name == AuthCookieName)
                        {
                            authCookie = cookieVal;
                        }
                    }
                }
            }
            return authCookie;
        }

        public string? ExploreAuthCookies(HttpRequest request)
        {
            string? authCookie = null;
            foreach (KeyValuePair<string, string> cookie in request.Cookies)
            {
                string name = cookie.Key;
                string value = cookie.Value;
                if (name == AuthCookieName)
                {
                    authCookie = $"{name}={value}";
                }
            }
            return authCookie;
        }

        public void ExploreAuthenticationTicket(AuthenticationTicket ticket)
        {
            ClaimsPrincipal principal = ticket.Principal;
            AuthenticationProperties properties = ticket.Properties;
            DateTimeOffset? issuedUtc = properties.IssuedUtc;
            DateTimeOffset? expiresUtc = properties.ExpiresUtc;
            bool? allowRefresh = properties.AllowRefresh;
            string? redirectUri = properties.RedirectUri;
            IDictionary<string, string?> items = properties.Items;
            IList<string> list = properties.Parameters.Keys.ToList();
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
