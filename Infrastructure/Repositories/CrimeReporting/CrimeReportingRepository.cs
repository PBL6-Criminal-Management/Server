using Application.Interfaces.CrimeReporting;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CrimeReporting
{
    public class CrimeReportingRepository : RepositoryAsync<Domain.Entities.CrimeReporting.CrimeReporting, long>, ICrimeReportingRepository
    {
        public CrimeReportingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}