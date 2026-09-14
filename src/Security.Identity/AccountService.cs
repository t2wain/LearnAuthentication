using Microsoft.AspNetCore.Identity;
using Security.Identity.IdentityCore;

namespace Security.Identity
{
    public class AccountService
    {
        SignInManager<IAppUser> _signInManager;
        UserManager<IAppUser> _userManager;

        public AccountService(
            SignInManager<IAppUser> signInManager, 
            UserManager<IAppUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public async Task SignIn(string userName)
        {
            IAppUser user = await _userManager.FindByNameAsync(userName);
            await _signInManager.SignInAsync(user, true);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
