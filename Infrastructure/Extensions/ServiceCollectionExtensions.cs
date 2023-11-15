using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Account;
using Application.Interfaces.Services.Identity;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Infrastructure.Services.FaceDetect;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Ms SQL
            // services.AddDbContext<ApplicationDbContext>(options =>
            // {
            //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), hius =>
            //     {
            //         hius.EnableRetryOnFailure();
            //         hius.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);

            //     });
            //     options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // });

            //MySQL
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(0, 0, 34));
            services.AddDbContext<ApplicationDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion, mySqlOptions =>
                                mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore)
                    .EnableRetryOnFailure())
                    //.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<ITokenService, IdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ICheckFileType, CheckFileType>();
            services.AddScoped<ICheckSizeFile, CheckFileSize>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddSingleton<IFaceDetectService, FaceDetectService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddAccountRepository();
            services.AddCriminalRepository();
            services.AddCriminalImageRepository();
            services.AddCaseRepository();
            services.AddCaseCriminalRepository();
            services.AddWantedCriminalRepository();
            services.AddWitnessRepository();
            services.AddEvidenceRepository();
            services.AddCaseImageRepository();
            services.AddCaseInvestigatorRepository();
            services.AddCaseWitnessRepository();
            services.AddVictimRepository();
            services.AddCaseVictimRepository();
        }
    }
}