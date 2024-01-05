using Application.Interfaces.CaseCriminal;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CaseCriminal
{
    public class CaseCriminalRepository : RepositoryAsync<Domain.Entities.CaseCriminal.CaseCriminal, long>, ICaseCriminalRepository
    {
        public CaseCriminalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
