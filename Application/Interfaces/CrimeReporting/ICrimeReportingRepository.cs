using Application.Interfaces.Repositories;

namespace Application.Interfaces.CrimeReporting
{
    public interface ICrimeReportingRepository : IRepositoryAsync<Domain.Entities.CrimeReporting.CrimeReporting, long>
    {
    }
}