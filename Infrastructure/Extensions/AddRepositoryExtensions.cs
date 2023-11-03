using Application.Interfaces.Account;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Infrastructure.Repositories.Account;
using Infrastructure.Repositories.Criminal;
using Infrastructure.Repositories.CriminalImage;
using Infrastructure.Repositories.Case;
using Infrastructure.Repositories.CaseCriminal;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.WantedCriminal;
using Infrastructure.Repositories.WantedCriminal;

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
        public static void AddCaseRepository(this IServiceCollection services)
        {
            services.AddScoped<ICaseRepository, CaseRepository>();
        }
        public static void AddCaseCriminalRepository(this IServiceCollection services)
        {
            services.AddScoped<ICaseCriminalRepository, CaseCriminalRepository>();
        }
        public static void AddWantedCriminalRepository(this IServiceCollection services)
        {
            services.AddScoped<IWantedCriminalRepository, WantedCriminalRepository>();
        }
    }
}
