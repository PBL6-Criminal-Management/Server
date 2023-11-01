using Application.Interfaces.Repositories;

namespace Application.Interfaces.Criminal
{
    public interface ICriminalRepository : IRepositoryAsync<Domain.Entities.Criminal.Criminal, long> 
    {
    }
}
