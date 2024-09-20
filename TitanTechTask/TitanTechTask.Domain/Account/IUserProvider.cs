using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TitanTechTask.Domain.Account
{
    public interface IUserProvider
    {
        Task<IdentityResult> CreateUserAsync(UserDomain user);

        Task<UserDomain> FindByEmailAsync(string email);

        Task<UserDomain> FindByIdAsync(string userid);
    }
}
