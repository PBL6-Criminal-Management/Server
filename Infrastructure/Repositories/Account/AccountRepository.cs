using Application.Interfaces.Account;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Account
{
    public class AccountRepository : RepositoryAsync<Domain.Entities.User.User, long>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
