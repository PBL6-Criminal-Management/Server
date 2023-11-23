using Application.Interfaces.Repositories;

namespace Application.Interfaces.ReportingImage
{
    public interface IReportingImageRepository : IRepositoryAsync<Domain.Entities.ReportingImage.ReportingImage, long>
    {
    }
}