using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Security.WebAppLib.IdentityCore
{
    public class MyUserStore : 
        IUserStore<IMyUser>, 
        IUserPasswordStore<IMyUser>, 
        IUserSecurityStampStore<IMyUser>,
        IUserEmailStore<IMyUser>,
        IRoleStore<IMyRole>,
        IUserClaimStore<IMyUser>
    {

        #region Not Implemented Members

        public virtual Task AddClaimsAsync(IMyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> CreateAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> CreateAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedEmailAsync(IMyUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedRoleNameAsync(IMyUser role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedRoleNameAsync(IMyRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedUserNameAsync(IMyUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task RemoveClaimsAsync(IMyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ReplaceClaimAsync(IMyUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetEmailAsync(IMyUser user, string? email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetEmailConfirmedAsync(IMyUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetPasswordHashAsync(IMyUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetRoleNameAsync(IMyRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetSecurityStampAsync(IMyUser user, string? stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetUserNameAsync(IMyUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> UpdateAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> UpdateAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMyUser?> FindByEmailAsync(string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IMyUser?> IUserStore<IMyUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> GetEmailConfirmedAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetPasswordHashAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMyUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IMyRole?> IRoleStore<IMyRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IMyRole?> IRoleStore<IMyRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            
        }

        public virtual Task<string?> GetEmailAsync(IMyUser user, CancellationToken cancellationToken)
        {
            string? email = null;
            if (user is MyUser u)
            {
                email = u.Email;
            }
            return Task.FromResult(email);
        }

        public virtual Task<string?> GetNormalizedEmailAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<IMyUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        public virtual Task<IList<Claim>> GetClaimsAsync(IMyUser user, CancellationToken cancellationToken)
        {
            IList<Claim> claims = [];
            return Task.FromResult(claims);
        }

        public virtual Task<string?> GetNormalizedRoleNameAsync(IMyRole role, CancellationToken cancellationToken)
        {
            if (role is MyRole r)
                return Task.FromResult((string?)r.Name.ToLower());  
            return Task.FromResult((string?)null);
        }

        public virtual Task<string?> GetNormalizedUserNameAsync(IMyUser user, CancellationToken cancellationToken)
        {
            if (user is MyUser u)
                return Task.FromResult((string?)u.UserName.ToLower());
            return Task.FromResult((string?)null);
        }
        public virtual Task<string> GetRoleIdAsync(IMyRole role, CancellationToken cancellationToken)
        {
            if (role is MyRole r)
                return Task.FromResult(r.ID);
            else throw new Exception();
        }

        public virtual Task<string?> GetRoleNameAsync(IMyRole role, CancellationToken cancellationToken)
        {
            if (role is MyRole r)
                return Task.FromResult((string?)r.Name);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<string?> GetSecurityStampAsync(IMyUser user, CancellationToken cancellationToken)
        {
            if (user is MyUser u)
                return Task.FromResult(u.SecurityStamp);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<string> GetUserIdAsync(IMyUser user, CancellationToken cancellationToken)
        {
            if (user is MyUser u)
                return Task.FromResult(u.ID);
            else throw new Exception();
        }

        public virtual Task<string?> GetUserNameAsync(IMyUser user, CancellationToken cancellationToken)
        {
            if (user is MyUser u)
                return Task.FromResult((string?)u.UserName);
            else return Task.FromResult((string?)null);
        }

        public virtual Task<bool> HasPasswordAsync(IMyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
    }
}
