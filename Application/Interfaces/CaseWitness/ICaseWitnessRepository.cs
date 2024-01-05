using Application.Interfaces.Repositories;

namespace Application.Interfaces.CaseWitness
{
    public interface ICaseWitnessRepository : IRepositoryAsync<Domain.Entities.CaseWitness.CaseWitness, long>
    {
    }
}