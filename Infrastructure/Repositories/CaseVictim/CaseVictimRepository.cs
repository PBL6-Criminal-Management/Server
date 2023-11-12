using Application.Interfaces.CaseVictim;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CaseVictim
{
    public class CaseVictimRepository : RepositoryAsync<Domain.Entities.CaseVictim.CaseVictim, long>, ICaseVictimRepository
    {
        public CaseVictimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}