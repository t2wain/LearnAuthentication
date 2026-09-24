using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Identity.Example;
using Security.Identity.IdentityCore;

namespace Security.Identity
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

        public virtual void AddIdentity()
        {

            _services.AddScoped<IdentityCoreStore>();
            _services.AddScoped<IUserStore<IAppUser>, IdentityCoreStore>();

            _services.AddScoped<RoleStore>();
            _services.AddScoped<IRoleStore<IAppRole>, RoleStore>();

            // Identity services with authentication
            IdentityBuilder identityBuilder = _services
                //.AddIdentityCore<IAppUser>()
                .AddIdentity<IAppUser, IAppRole>((IdentityOptions setupOption) => 
                { 
                    _config.Bind("IdentityOptions", setupOption);
                })
                .AddSignInManager()
                .AddUserStore<IdentityCoreStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();

            _services.ConfigureApplicationCookie((CookieAuthenticationOptions configureOptions) =>
            {
                _config.Bind("CookieAuthenticationOptions", configureOptions);
            });
        }

        public virtual void AddAuthorization()
        {
            _services.AddAuthorizationCore((AuthorizationOptions configure) =>
            {
                _config.Bind("AuthorizationOptions", configure);
            });
        }

        public virtual void AddOtherServices()
        {
            _services.AddScoped<ExploreIdentity>();
            _services.AddScoped<ExploreAuthentication>();
            _services.AddScoped<ExploreAuthorization>();
        }
    }
}
