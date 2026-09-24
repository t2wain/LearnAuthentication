using Microsoft.AspNetCore.Identity;
using Security.Identity.IdentityCore;

namespace Security.Identity.Example
{
    public class ExploreIdentity
    {
        IUserStore<IAppUser> _userStore;
        IRoleStore<IAppRole> _roleStore;
        UserManager<IAppUser> _userManager;
        RoleManager<IAppRole> _roleManager;

        public ExploreIdentity(
            IUserStore<IAppUser> userStore,
            IRoleStore<IAppRole> roleStore,
            UserManager<IAppUser> userManager,
            RoleManager<IAppRole> roleManager
        )
        {
            this._userStore = userStore;
            this._roleStore = roleStore;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        #region UserManager

        public void ExporeUserManager()
        {
            ILookupNormalizer o = _userManager.KeyNormalizer;
            IdentityOptions o2 = _userManager.Options;
            IPasswordHasher<IAppUser> o3 = _userManager.PasswordHasher;
            IList<IPasswordValidator<IAppUser>> o4 = _userManager.PasswordValidators;
            IList<IUserValidator<IAppUser>> o5 = _userManager.UserValidators;

            bool b = _userManager.SupportsQueryableUsers;
            b = _userManager.SupportsQueryableUsers;
            b = _userManager.SupportsUserAuthenticationTokens;
            b = _userManager.SupportsUserAuthenticatorKey;
            b = _userManager.SupportsUserClaim;
            b = _userManager.SupportsUserEmail;
            b = _userManager.SupportsUserLockout;
            b = _userManager.SupportsUserLogin;
            b = _userManager.SupportsUserPasskey;
            b = _userManager.SupportsUserPassword;
            b = _userManager.SupportsUserPhoneNumber;
            b = _userManager.SupportsUserRole;
            b = _userManager.SupportsUserSecurityStamp;
            b = _userManager.SupportsUserTwoFactor;
            b = _userManager.SupportsUserTwoFactorRecoveryCodes;
        }

        #endregion

    }
}
