using Application.Interfaces.CaseWitness;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CaseWitness
{
    public class CaseWitnessRepository : RepositoryAsync<Domain.Entities.CaseWitness.CaseWitness, long>, ICaseWitnessRepository
    {
        public CaseWitnessRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}