using Application.Interfaces.Repositories;

namespace Application.Interfaces.Account
{
    public interface IAccountRepository : IRepositoryAsync<Domain.Entities.User.User, long>
    {
    }
}