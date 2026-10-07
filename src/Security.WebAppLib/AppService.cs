using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Security.WebAppLib.Example;
using Security.WebAppLib.IdentityCore;

namespace Security.WebAppLib
{
    public class AppService
    {
        IServiceCollection _services;
        IConfiguration _config;

        public AppService(IServiceCollection services, IConfiguration config)
        {
            this._services = services;
            this._config = config;
        }

        public virtual void AddMyIdentity()
        {

            _services.AddScoped<MyUserStore>();
            _services.AddScoped<IUserStore<IMyUser>, MyUserStore>();

            _services.AddScoped<MyRoleStore>();
            _services.AddScoped<IRoleStore<IMyRole>, MyRoleStore>();

            //_services.AddDataProtection();

            // Identity services with authentication
            IdentityBuilder b1 = _services
                //.AddIdentityCore<IMyUser>()
                .AddIdentity<IMyUser, IMyRole>((IdentityOptions setupOption) =>
                {
                    _config.Bind("IdentityOptions", setupOption);
                })
                .AddSignInManager()
                .AddUserStore<MyUserStore>()
                .AddRoleStore<MyRoleStore>()
                .AddDefaultTokenProviders();

            AuthenticationBuilder b2 = _services.AddAuthentication((AuthenticationOptions options) =>
            {
                _config.Bind("AuthenticationOptions", options);
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            });

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

        public virtual void AddMyOtherServices()
        {
            _services.AddScoped<ExploreIdentity>();
            _services.AddScoped<ExploreAuthentication>();
            _services.AddScoped<ExploreAuthorization>();
        }

        public void AddTestHttpContextAccessor()
        {
            _services.AddSingleton<IHttpContextAccessor, MyHttpContextAccessor>();
        }
    }
}
