using Application.Interfaces.Repositories;

namespace Application.Interfaces.CaseVictim
{
    public interface ICaseVictimRepository : IRepositoryAsync<Domain.Entities.CaseVictim.CaseVictim, long>
    {
    }
}