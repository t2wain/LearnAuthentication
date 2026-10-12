using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Security.WebAppLib.Example;
using Security.WebAppLib.IdentityCore;

namespace Security.WebAppLib
{
    /// <summary>
    /// Configures and registers services for ASP.NET Core Identity, 
    /// authentication, and authorization.
    /// </summary>
    public class AppService
    {
        IServiceCollection _services;
        IConfiguration _config;

        public AppService(IServiceCollection services, IConfiguration config)
        {
            this._services = services;
            this._config = config;
        }

        /// <summary>
        /// Registers ASP.NET Core Identity services with custom user 
        /// and role stores and authentication configurations.
        /// </summary>
        public virtual void AddMyIdentity()
        {

            _services.AddScoped<MyUserStore>();
            _services.AddScoped<IUserStore<IMyUser>, MyUserStore>();

            _services.AddScoped<MyRoleStore>();
            _services.AddScoped<IRoleStore<IMyRole>, MyRoleStore>();

            // Register Identity services with 4 cookie authentication schemes with names
            // from the IdentityConstants enum : ApplicationScheme, ExternalScheme,
            // TwoFactorRememberMeScheme, TwoFactorUserIdScheme
            IdentityBuilder b1 = _services
                .AddIdentity<IMyUser, IMyRole>((IdentityOptions setupOption) =>
                {
                    _config.Bind("IdentityOptions", setupOption);
                })
                .AddSignInManager()
                .AddUserStore<MyUserStore>()
                .AddRoleStore<MyRoleStore>()
                .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Registers authentication schemes and configurations.
        /// </summary>
        public virtual void AddMyAuthentication()
        {
            // Configure default authentication schemes for various sign-in scenarios.
            AuthenticationBuilder b2 = _services.AddAuthentication((AuthenticationOptions options) =>
            {
                _config.Bind("AuthenticationOptions", options);
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            });

            // Configure default options for the cookie authentication scheme
            // as IOptionsMonitor<CookieAuthenticationOptions>.
            _services.ConfigureApplicationCookie((CookieAuthenticationOptions configureOptions) =>
            {
                _config.Bind("CookieAuthenticationOptions", configureOptions);
                configureOptions.Events = new MyAuthenticationEvent();
            });
        }

        public virtual void AddMyAuthorization()
        {
            _services.AddAuthorizationCore((AuthorizationOptions configure) =>
            {
                _config.Bind("AuthorizationOptions", configure);
            });
        }

        /// <summary>
        /// Registers examples
        /// </summary>
        public virtual void AddMyExamples()
        {
            _services.AddScoped<ExploreIdentity>();
            _services.AddScoped<ExploreAuthentication>();
            _services.AddScoped<ExploreAuthorization>();
        }

        /// <summary>
        /// Registers a custom implementation of 
        /// IHttpContextAccessor for unit testing purposes.
        /// </summary>
        public virtual void AddMyHttpContextAccessor()
        {
            _services.AddSingleton<IHttpContextAccessor, MyHttpContextAccessor>();
        }
    }
}
