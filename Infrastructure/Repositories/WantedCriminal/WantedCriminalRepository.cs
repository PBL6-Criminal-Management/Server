using Application.Interfaces.WantedCriminal;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.WantedCriminal
{
    public class WantedCriminalRepository : RepositoryAsync<Domain.Entities.WantedCriminal.WantedCriminal, long>, IWantedCriminalRepository
    {
        public WantedCriminalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
