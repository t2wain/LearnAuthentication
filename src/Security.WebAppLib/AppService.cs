using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

        #region Identity

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

        #endregion

        #region Authentication

        /// <summary>
        /// Registers authentication schemes and configurations.
        /// </summary>
        public virtual AuthenticationBuilder AddMyAuthentication(
            string? cfgAuthenOptions = null, string? cfgAuthenCookieOpt = null)
        {
            // Configure default authentication schemes for various sign-in scenarios.
            AuthenticationBuilder b = _services.AddAuthentication((AuthenticationOptions options) =>
            {
                if (!string.IsNullOrWhiteSpace(cfgAuthenOptions))
                    _config.Bind(cfgAuthenOptions, options);
                else
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                }
            });

            // Configure default options for the cookie authentication scheme
            // as IOptionsMonitor<CookieAuthenticationOptions>.
            _services.ConfigureApplicationCookie((CookieAuthenticationOptions options) =>
            {
                if (!string.IsNullOrWhiteSpace(cfgAuthenCookieOpt))
                    _config.Bind(cfgAuthenCookieOpt, options);
                options.Events = new MyAuthenticationEvent();
            });

            return b;
        }

        #endregion

        #region JWT

        /// <summary>
        /// Configure JWT Authenticaiton Scheme
        /// </summary>
        public virtual AuthenticationBuilder AddMyAuthenticationJwtBearer(
            AuthenticationBuilder builder,
            string cfgJwtOptions,
            string? schemeName = null)
        {
            _services.Configure<JwtConfig>(_config.GetSection(cfgJwtOptions));

            JwtConfig cfg = new();
            _config.Bind(cfgJwtOptions, cfg);
            TokenValidationParameters validation = JwtUtil.GetValidatOption(cfg);

            string scheme = schemeName ?? JwtBearerDefaults.AuthenticationScheme;
            var b = builder.AddJwtBearer(scheme, (JwtBearerOptions options) =>
            {
                options.TokenValidationParameters = validation;
            });
            return b;
        }

        #endregion

        #region Cookie

        /// <summary>
        /// Configure Cookie Authentication Scheme
        /// </summary>
        public virtual AuthenticationBuilder AddMyAuthenticationCookie(
            AuthenticationBuilder builder, 
            string? schemeName = null)
        {
            string scheme = schemeName ?? CookieAuthenticationDefaults.AuthenticationScheme;
            var b = builder.AddCookie(scheme, (CookieAuthenticationOptions options) =>
            {

            });
            return b;
        }

        #endregion

        public virtual void AddMyAuthorization(string? cfgAuthorOptions = null)
        {
            MyAuthorizationService.ConfigureServices(_services, _config);
            _services.AddAuthorization((AuthorizationOptions options) =>
            {
                if (!string.IsNullOrWhiteSpace(cfgAuthorOptions))
                    _config.Bind(cfgAuthorOptions, options);

                // add authorization policies
                MyAuthorizationService.ConfigureAuthorizationOptions(options);
            });
        }

        /// <summary>
        /// Registers examples
        /// </summary>
        public virtual void AddMyAuthExamples()
        {
            _services.AddScoped<ExploreIdentity>();
            _services.AddScoped<ExploreAuthentication>();
            _services.AddScoped<ExploreAuthorization>();
        }

        /// <summary>
        /// Registers a custom implementation of 
        /// IHttpContextAccessor for unit testing purposes.
        /// </summary>
        public virtual void AddMyTestHttpContextAccessor()
        {
            _services.AddSingleton<IHttpContextAccessor, MyHttpContextAccessor>();
        }

        public virtual void AddMyAuthMiddlewares(IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    }
}
