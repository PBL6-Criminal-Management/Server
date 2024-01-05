using Application.Interfaces.Repositories;

namespace Application.Interfaces.CaseCriminal
{
    public interface ICaseCriminalRepository : IRepositoryAsync<Domain.Entities.CaseCriminal.CaseCriminal, long>
    {
    }
}