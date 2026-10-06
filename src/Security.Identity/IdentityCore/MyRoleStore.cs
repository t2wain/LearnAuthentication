using Microsoft.AspNetCore.Identity;

namespace Security.Identity.IdentityCore
{
    public class MyRoleStore : IRoleStore<IMyRole>
    {
        public Task<IdentityResult> CreateAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public Task<IMyRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IMyRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedRoleNameAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IMyRole role, CancellationToken cancellationToken) => 
            Task.FromResult(role.ID);

        public Task<string?> GetRoleNameAsync(IMyRole role, CancellationToken cancellationToken)
            => Task.FromResult((string?)role.Name);

        public Task SetNormalizedRoleNameAsync(IMyRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IMyRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IMyRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
