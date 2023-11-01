using Application.Interfaces.Case;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Case
{
    public class CaseRepository : RepositoryAsync<Domain.Entities.Case.Case, long>, ICaseRepository
    {
        public CaseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
