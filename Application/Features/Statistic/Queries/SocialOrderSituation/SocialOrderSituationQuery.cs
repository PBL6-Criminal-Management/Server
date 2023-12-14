using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Statistic.Queries.CriminalStructure
{
    public class SocialOrderSituationQuery : IRequest<Result<List<SocialOrderSituationResponse>>>
    {
        public List<int> Year { get; set; } = null!;
        public int Month { get; set; }
    }
    internal class SocialOrderSituationQueryHandler : IRequestHandler<SocialOrderSituationQuery, Result<List<SocialOrderSituationResponse>>>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICriminalRepository _criminalRepository;

        public SocialOrderSituationQueryHandler(
            ICaseRepository caseRepository, 
            ICaseCriminalRepository caseCriminalRepository,
            ICriminalRepository criminalRepository
            )
        {
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalRepository = criminalRepository;
        }
        public async Task<Result<List<SocialOrderSituationResponse>>> Handle(SocialOrderSituationQuery request, CancellationToken cancellationToken)
        {
            foreach(var year in request.Year)
                if (year < 0)
                    return await Result<List<SocialOrderSituationResponse>>.FailAsync(StaticVariable.YEAR_MUST_NOT_BE_NEGATIVE);

            if(request.Month < 1 || request.Month > 12)
                return await Result<List<SocialOrderSituationResponse>>.FailAsync(StaticVariable.INVALID_MONTH);

            var listResponse = new List<SocialOrderSituationResponse>();

            foreach(var year in request.Year)
            {
                var casesInRequestTime = _caseRepository.Entities.Where(c => c.StartDate.Year == year && c.StartDate.Month == request.Month).ToList();

                var caseCount = casesInRequestTime.Count();

                var triedCaseCount = casesInRequestTime.Where(c => c.Status == Domain.Constants.Enum.CaseStatus.Tried).Count();

                var arrestedOrHandledCriminalCount = casesInRequestTime
                                    .Join(_caseCriminalRepository.Entities,
                                            caseInRequestYear => caseInRequestYear.Id,
                                            caseCriminal => caseCriminal.CaseId,
                                            (caseInRequestYear, caseCriminal) => new
                                            {
                                                caseId = caseCriminal.CaseId,
                                                criminalId = caseCriminal.CriminalId
                                            })
                                    .GroupBy(gr => gr.criminalId,
                                            gr => gr,
                                            (criminalId, gr) => criminalId)
                                    .Join(_criminalRepository.Entities,
                                        criminalId => criminalId,
                                        c => c.Id,
                                        (criminalId, c) => c.Status)
                                    .Where(s => s != Domain.Constants.Enum.CriminalStatus.Wanted)
                                    .Count();

                listResponse.Add(new SocialOrderSituationResponse
                {
                    Month = request.Month,
                    Year = year,
                    CaseCount = caseCount,
                    TriedCaseCount = triedCaseCount,
                    ArrestedOrHandledCriminalCount = arrestedOrHandledCriminalCount
                });
            }            

            return await Result<List<SocialOrderSituationResponse>>.SuccessAsync(listResponse);

        }
    }
}
