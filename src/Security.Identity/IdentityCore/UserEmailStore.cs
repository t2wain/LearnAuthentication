using Microsoft.AspNetCore.Identity;

namespace Security.Identity.IdentityCore
{
    public class UserEmailStore : IUserEmailStore<IAppUser>
    {
        public virtual Task<IdentityResult> CreateAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IdentityResult> DeleteAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IAppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IAppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> GetEmailConfirmedAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedEmailAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetNormalizedUserNameAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> GetUserIdAsync(IAppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string?> GetUserNameAsync(IAppUser user, CancellationToken cancellationToken)
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

        public virtual Task SetNormalizedEmailAsync(IAppUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task SetNormalizedUserNameAsync(IAppUser user, string? normalizedName, CancellationToken cancellationToken)
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
    }
}
