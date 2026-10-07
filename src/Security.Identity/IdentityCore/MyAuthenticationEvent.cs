using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Security.Identity.IdentityCore
{
    public class MyAuthenticationEvent : CookieAuthenticationEvents
    {
        #region Redirect

        override public Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToAccessDenied(context);
        }

        override public Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToLogin(context);
        }

        override public Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToLogout(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToReturnUrl(context);
        }

        #endregion

        public override Task SignedIn(CookieSignedInContext context)
        {
            ExploreContext(context);
            return base.SignedIn(context);
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            ExploreContext(context);
            return base.SigningIn(context);
        }

        public override Task SigningOut(CookieSigningOutContext context)
        {
            ExploreContext(context);
            return base.SigningOut(context);
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            ExploreContext(context);
            return base.ValidatePrincipal(context);
        }

        #region ExploreContext

        protected void ExploreContext(CookieSigningOutContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var scheme = context.Scheme;
            var properties = context.Properties;
        }

        protected void ExploreContext(CookieValidatePrincipalContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var scheme = context.Scheme;
            var principal = context.Principal;
            var properties = context.Properties;
        }

        protected void ExploreContext(CookieSigningInContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var scheme = context.Scheme;
            var principal = context.Principal;
            var properties = context.Properties;
        }

        protected void ExploreContext(CookieSignedInContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var scheme = context.Scheme;
            var principal = context.Principal;
            var properties = context.Properties;
        }

        #endregion
    }
}
