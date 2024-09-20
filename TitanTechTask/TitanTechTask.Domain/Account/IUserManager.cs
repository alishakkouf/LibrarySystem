using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TitanTechTask.Domain.Account
{
    public interface IUserManager
    {
        Task<UserDomain> FindByEmailAsync(string email);

        Task<IdentityResult> CreateUserAsync(UserDomain user);
    }
}
