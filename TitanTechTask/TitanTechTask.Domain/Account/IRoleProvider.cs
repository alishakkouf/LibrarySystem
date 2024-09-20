using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TitanTechTask.Domain.Account
{
    public interface IRoleProvider
    {
        Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken);
    }
}
