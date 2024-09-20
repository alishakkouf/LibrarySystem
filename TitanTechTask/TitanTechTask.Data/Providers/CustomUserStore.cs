using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TitanTechTask.Domain.Account;

namespace TitanTechTask.Data.Providers
{
    public class CustomUserStore : IUserPasswordStore<UserDomain>
    {
        private readonly IUserProvider _userProvider;

        public CustomUserStore(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public async Task<IdentityResult> CreateAsync(UserDomain user, CancellationToken cancellationToken)
        {
            return await _userProvider.CreateUserAsync(user);
        }

        public Task<IdentityResult> DeleteAsync(UserDomain user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<UserDomain> FindByEmailAsync(string normalizedEmail)
        {
            return await _userProvider.FindByEmailAsync(normalizedEmail);
        }

        public async Task<UserDomain?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _userProvider.FindByIdAsync(userId);
        }

        public Task<UserDomain?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedUserNameAsync(UserDomain user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetPasswordHashAsync(UserDomain user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserIdAsync(UserDomain user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserId.ToString());
        }

        public async Task<string?> GetUserNameAsync(UserDomain user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName.ToString());
        }

        public Task<bool> HasPasswordAsync(UserDomain user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(UserDomain user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(UserDomain user, string? passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(UserDomain user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserDomain user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Other required methods of IUserPasswordStore interface
    }

}
