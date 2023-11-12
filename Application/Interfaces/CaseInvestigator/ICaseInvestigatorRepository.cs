using Application.Interfaces.Repositories;

namespace Application.Interfaces.CaseInvestigator
{
    public interface ICaseInvestigatorRepository : IRepositoryAsync<Domain.Entities.CaseInvestigator.CaseInvestigator, long>
    {
    }
}