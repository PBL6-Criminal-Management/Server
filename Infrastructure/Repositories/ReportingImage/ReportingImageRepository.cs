using Application.Interfaces.ReportingImage;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.ReportingImage
{
    public class ReportingImageRepository : RepositoryAsync<Domain.Entities.ReportingImage.ReportingImage, long>, IReportingImageRepository
    {
        public ReportingImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}