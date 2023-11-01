using Application.Interfaces.Repositories;

namespace Application.Interfaces.CriminalImage
{
    public interface ICriminalImageRepository : IRepositoryAsync<Domain.Entities.CriminalImage.CriminalImage, long>
    {
    }
}
