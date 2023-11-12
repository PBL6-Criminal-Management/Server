using Application.Interfaces.CaseInvestigator;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CaseInvestigator
{
    public class CaseInvestigatorRepository : RepositoryAsync<Domain.Entities.CaseInvestigator.CaseInvestigator, long>, ICaseInvestigatorRepository
    {
        public CaseInvestigatorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}