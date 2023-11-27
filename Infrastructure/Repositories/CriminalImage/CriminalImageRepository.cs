using Application.Interfaces.CriminalImage;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.CriminalImage
{
    public class CriminalImageRepository : RepositoryAsync<Domain.Entities.CriminalImage.CriminalImage, long>, ICriminalImageRepository
    {
        public CriminalImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
