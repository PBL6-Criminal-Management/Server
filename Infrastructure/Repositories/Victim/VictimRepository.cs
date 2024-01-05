using Application.Interfaces.Victim;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Victim
{
    public class VictimRepository : RepositoryAsync<Domain.Entities.Victim.Victim, long>, IVictimRepository
    {
        public VictimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}