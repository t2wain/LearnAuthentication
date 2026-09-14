using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Security.Identity.IdentityCore
{
    public class IdentityCoreStore : 
        IUserStore<IAppUser>, 
        IUserPasswordStore<IAppUser>, 
        IUserSecurityStampStore<IAppUser>,
        IUserEmailStore<IAppUser>,
        IRoleStore<IAppRole>,
        IUserClaimStore<IAppUser>
    {

        #region Not Implemented Members

        public virtual Task AddClaimsAsync(IAppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> CreateAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> CreateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedEmailAsync(IAppUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedRoleNameAsync(IAppUser role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedRoleNameAsync(IAppRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedUserNameAsync(IAppUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task RemoveClaimsAsync(IAppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ReplaceClaimAsync(IAppUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetEmailAsync(IAppUser user, string? email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetEmailConfirmedAsync(IAppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetPasswordHashAsync(IAppUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetRoleNameAsync(IAppRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetSecurityStampAsync(IAppUser user, string? stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetUserNameAsync(IAppUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> UpdateAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> UpdateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IAppUser?> FindByEmailAsync(string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IAppUser?> IUserStore<IAppUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> GetEmailConfirmedAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetPasswordHashAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IAppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IAppRole?> IRoleStore<IAppRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IAppRole?> IRoleStore<IAppRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<IAppUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        public virtual Task<IList<Claim>> GetClaimsAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
        {
            if (role is AppRole r)
                return Task.FromResult((string?)r.Name.ToLower());  
            return Task.FromResult((string?)null);
        }

        public virtual Task<string?> GetNormalizedUserNameAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user is AppUser u)
                return Task.FromResult((string?)u.UserName.ToLower());
            return Task.FromResult((string?)null);
        }
        public virtual Task<string> GetRoleIdAsync(IAppRole role, CancellationToken cancellationToken)
        {
            if (role is AppRole r)
                return Task.FromResult(r.ID);
            else throw new Exception();
        }

        public virtual Task<string?> GetRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
        {
            if (role is AppRole r)
                return Task.FromResult((string?)r.Name);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<string?> GetSecurityStampAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user is AppUser u)
                return Task.FromResult(u.SecurityStamp);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<string> GetUserIdAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user is AppUser u)
                return Task.FromResult(u.ID);
            else throw new Exception();
        }

        public virtual Task<string?> GetUserNameAsync(IAppUser user, CancellationToken cancellationToken)
        {
            if (user is AppUser u)
                return Task.FromResult((string?)u.UserName);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<bool> HasPasswordAsync(IAppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
    }
}
