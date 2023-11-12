using Application.Interfaces.Repositories;

namespace Application.Interfaces.Victim
{
    public interface IVictimRepository : IRepositoryAsync<Domain.Entities.Victim.Victim, long>
    {
    }
}