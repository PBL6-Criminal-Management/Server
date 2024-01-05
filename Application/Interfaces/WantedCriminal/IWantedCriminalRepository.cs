using Application.Interfaces.Repositories;

namespace Application.Interfaces.WantedCriminal
{
    public interface IWantedCriminalRepository : IRepositoryAsync<Domain.Entities.WantedCriminal.WantedCriminal, long>
    {
    }
}
