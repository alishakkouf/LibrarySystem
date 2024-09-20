using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TitanTechTask.Domain.Account;

namespace TitanTechTask.Manager.Account
{
    public class UserManager : IUserManager
    {
        private readonly IUserProvider _userProvider;

        public UserManager(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public async Task<IdentityResult> CreateUserAsync(UserDomain user)
        {
            return await _userProvider.CreateUserAsync(user);
        }

        public async Task<UserDomain> FindByEmailAsync(string email)
        {
            return await _userProvider.FindByEmailAsync(email);
        }
    }

}
