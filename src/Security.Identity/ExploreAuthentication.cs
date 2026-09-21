using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Security.Identity.IdentityCore;

namespace Security.Identity
{
    public class ExploreAuthentication
    {
        IAuthenticationService _authenticationService;
        IAuthenticationService _authorizationService;
        IUserStore<IAppUser> _userStore;
        IRoleStore<IAppUser> _roleStore;
        UserManager<IAppUser> _userManager;
        RoleManager<IAppRole> _roleManager;
        SignInManager<IAppUser> _signInManager;

        public ExploreAuthentication(
            IAuthenticationService authenticationService,
            IAuthenticationService authorizationService,
            IUserStore<IAppUser> userStore,
            IRoleStore<IAppUser> roleStore,
            UserManager<IAppUser> userManager,
            RoleManager<IAppRole> roleManager,
            SignInManager<IAppUser> signInManager
        )
        {
            this._authenticationService = authenticationService;
            this._authorizationService = authorizationService;
            this._userStore = userStore;
            this._roleStore = roleStore;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }

        public void ExploreSignInManager()
        {
            HttpContext context = _signInManager.Context;
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

    }
}
