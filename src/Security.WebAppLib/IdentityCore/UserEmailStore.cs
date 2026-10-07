using Microsoft.AspNetCore.Identity;

namespace Security.WebAppLib.IdentityCore
{
    public class UserEmailStore : IUserEmailStore<IMyUser>
    {
        public virtual Task<IdentityResult> CreateAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMyUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMyUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IMyUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetEmailAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> GetEmailConfirmedAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedEmailAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedUserNameAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> GetUserIdAsync(IMyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetUserNameAsync(IMyUser user, CancellationToken cancellationToken)
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

        public virtual Task SetNormalizedEmailAsync(IMyUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedUserNameAsync(IMyUser user, string? normalizedName, CancellationToken cancellationToken)
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
    }
}
