using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TitanTechTask.Data.Providers;
using TitanTechTask.Domain.Account;
using TitanTechTask.Domain.Books;

namespace TitanTechTask.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDataModule(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IBookProvider, BookProvider>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IRoleProvider, RoleProvider>();

            return services;
        }
    }
}
