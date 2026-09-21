using Microsoft.AspNetCore.Identity;

namespace Security.Identity.IdentityCore
{
    public class RoleStore : IRoleStore<IAppRole>
    {
        public Task<IdentityResult> CreateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IAppRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IAppRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IAppRole role, CancellationToken cancellationToken) => 
            Task.FromResult(role.ID);

        public Task<string?> GetRoleNameAsync(IAppRole role, CancellationToken cancellationToken)
            => Task.FromResult((string?)role.Name);

        public Task SetNormalizedRoleNameAsync(IAppRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IAppRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IAppRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
