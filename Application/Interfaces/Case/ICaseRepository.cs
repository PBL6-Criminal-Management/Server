using Application.Interfaces.Repositories;

namespace Application.Interfaces.Case
{
    public interface ICaseRepository : IRepositoryAsync<Domain.Entities.Case.Case, long>
    {
    }
}