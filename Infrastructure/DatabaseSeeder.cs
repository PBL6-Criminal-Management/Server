using Application.Interfaces.Services;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DatabaseSeeder(ILogger<DatabaseSeeder> logger, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            AddAdministrator();
            _context.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                var adminRole = new AppRole()
                {
                    Name = RoleConstants.AdministratorRole,
                    Description = "Administrator role with full permission"
                };
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }

                var officerRole = new AppRole()
                {
                    Name = RoleConstants.OfficerRole,
                    Description = "Officer role with custom permission"
                };
                var officerRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.OfficerRole);
                if (officerRoleInDb == null)
                {
                    await _roleManager.CreateAsync(officerRole);
                    _logger.LogInformation("Seeded Officer Role.");
                }

                var investigatorRole = new AppRole()
                {
                    Name = RoleConstants.InvestigatorRole,
                    Description = "Investigator role with custom permission"
                };
                var investigatorRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.InvestigatorRole);
                if (investigatorRoleInDb == null)
                {
                    await _roleManager.CreateAsync(investigatorRole);
                    _logger.LogInformation("Seeded Investigator Role.");
                }

                //Check if User Exists
                var superUser = new AppUser()
                {
                    FullName = "Superadmin",
                    Email = "noreply.criminalmanagement@gmail.com",
                    UserName = UserConstants.DefaultUsername,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByNameAsync(superUser.UserName);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}