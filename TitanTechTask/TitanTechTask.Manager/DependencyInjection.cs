using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TitanTechTask.Domain.Account;
using TitanTechTask.Domain.Books;
using TitanTechTask.Manager.Account;
using TitanTechTask.Manager.BooksManager;

namespace TitanTechTask.Manager
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureManagerModule(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IBookManager, BookManager>();
            services.AddTransient<IUserManager, UserManager>();
            return services;
        }
    }
}
