using Application.Interfaces.Account;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Infrastructure.Repositories.Account;
using Infrastructure.Repositories.Criminal;
using Infrastructure.Repositories.CriminalImage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class AddRepositoryExtensions
    {
        public static void AddAccountRepository(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
        public static void AddCriminalRepository(this IServiceCollection services)
        {
            services.AddScoped<ICriminalRepository, CriminalRepository>();
        }
        public static void AddCriminalImageRepository(this IServiceCollection services)
        {
            services.AddScoped<ICriminalImageRepository, CriminalImageRepository>();
        }
    }
}
