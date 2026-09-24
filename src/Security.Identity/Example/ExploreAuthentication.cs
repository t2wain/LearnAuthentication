using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Security.Identity.IdentityCore;

namespace Security.Identity.Example
{
    public class ExploreAuthentication
    {

        #region Init

        IServiceProvider _provider;
        IAuthenticationService _authenticationService;
        IAuthenticationSchemeProvider _authenticationSchemeProvider;
        IOptionsMonitor<CookieAuthenticationOptions> _cookieOptions;
        UserManager<IAppUser> _userManager;
        SignInManager<IAppUser> _signInManager;

        public ExploreAuthentication(
            IServiceProvider provider,
            IAuthenticationService authenticationService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IOptionsMonitor<CookieAuthenticationOptions> cookieOptions,
            UserManager<IAppUser> userManager,
            SignInManager<IAppUser> signInManager
        )
        {
            _provider = provider;
            this._authenticationService = authenticationService;
            this._authenticationSchemeProvider = authenticationSchemeProvider;
            this._cookieOptions = cookieOptions;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        #endregion

        public async Task Run(int testNo = 0)
        {
            int? o = 2;
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
            }
        }

        public HttpContext? GetHttpContext(object o)
        {
            HttpContext? c = null;
            try
            {
                c = o switch
                {
                    SignInManager<IAppUser> s => s.Context,
                    _ => null
                };
            }
            catch { }
            return c;
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
                ExploreAuthenticationScheme(scheme);
        }

        public void ExploreAuthenticationScheme(AuthenticationScheme scheme)
        {
            string n = scheme.DisplayName;
            string n1 = scheme.Name;
            string t = scheme.HandlerType.Name;
            object? o = _provider.GetService(scheme.HandlerType);
            if (o is AuthenticationHandler<CookieAuthenticationOptions> h)
            {
                CookieAuthenticationOptions op = h.Options;
                AuthenticationScheme s = h.Scheme;
                if (op != null)
                    ExploreCookieOptions(op);
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
            HttpContext? context = GetHttpContext(_signInManager);
            IdentityOptions options = _signInManager.Options;
            UserManager<IAppUser> userManager = _signInManager.UserManager;
            IUserClaimsPrincipalFactory<IAppUser> claimFact = _signInManager.ClaimsFactory;
        }

        public async Task SignIn(string userName)
        {
            IAppUser? user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                await _signInManager.SignInAsync(user, true);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion
    }
}
