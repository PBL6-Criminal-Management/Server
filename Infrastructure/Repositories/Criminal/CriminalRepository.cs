using Application.Interfaces.Criminal;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Criminal
{
    public class CriminalRepository : RepositoryAsync<Domain.Entities.Criminal.Criminal, long>, ICriminalRepository
    {
        public CriminalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
