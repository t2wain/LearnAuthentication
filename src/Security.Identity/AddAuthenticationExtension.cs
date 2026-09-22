using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Security.Identity.IdentityCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Security.Identity
{
    public static class AddAuthenticationExtension
    {
        public static void AddAppAuthentication(this IServiceCollection services)
        {

            services.AddScoped<UserStore>();
            services.AddScoped<IUserStore<IAppUser>, UserStore>();

            services.AddScoped<RoleStore>();
            services.AddScoped<IRoleStore<IAppRole>, RoleStore>();

            // Identity services
            IdentityBuilder identityBuilder = services
                //.AddIdentityCore<IAppUser>()
                .AddIdentity<IAppUser, IAppRole>((IdentityOptions setupOption) => {  })
                .AddSignInManager()
                .AddRoleManager<IAppRole>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();

            // Cookie authentication
            AuthenticationBuilder authBuilder = services
                //.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddAuthentication((AuthenticationOptions configureOptions) =>
                {
                    configureOptions.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                })
                //.AddCookie(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, (CookieAuthenticationOptions configureOptions) => { });

            services.AddAuthorizationCore((AuthorizationOptions configure) =>
            {

            });
        }
    }
}
