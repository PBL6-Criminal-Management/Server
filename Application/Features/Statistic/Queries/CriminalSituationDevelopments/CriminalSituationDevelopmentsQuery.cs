using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseVictim;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Statistic.Queries.CriminalStructure
{
    public class CriminalSituationDevelopmentsQuery : IRequest<Result<List<CriminalSituationDevelopmentsResponse>>>
    {
        public int Year { get; set; }
    }
    internal class CriminalSituationDevelopmentsQueryHandler : IRequestHandler<CriminalSituationDevelopmentsQuery, Result<List<CriminalSituationDevelopmentsResponse>>>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICaseVictimRepository _caseVictimRepository;

        public CriminalSituationDevelopmentsQueryHandler(
            ICaseRepository caseRepository, 
            ICaseCriminalRepository caseCriminalRepository,
            ICaseVictimRepository caseVictimRepository
            )
        {
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _caseVictimRepository = caseVictimRepository;
        }
        public async Task<Result<List<CriminalSituationDevelopmentsResponse>>> Handle(CriminalSituationDevelopmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.Year < 0)
                return await Result<List<CriminalSituationDevelopmentsResponse>>.FailAsync(StaticVariable.YEAR_MUST_NOT_BE_NEGATIVE);

            var listResponse = new List<CriminalSituationDevelopmentsResponse>();

            var casesInRequestYear = _caseRepository.Entities.Where(c => c.StartDate.Year == request.Year).ToList();

            var criminalsInMonths = casesInRequestYear
                                .Join(_caseCriminalRepository.Entities,
                                        caseInRequestYear => caseInRequestYear.Id,
                                        caseCriminal => caseCriminal.CaseId,
                                        (caseInRequestYear, caseCriminal) => new
                                        {
                                            month = caseInRequestYear.StartDate.Month,
                                            caseId = caseCriminal.CaseId,
                                            criminalId = caseCriminal.CriminalId
                                        })
                                .GroupBy(gr => gr.month,
                                        gr => gr,
                                        (month, gr) => new { month, gr }).ToList();

            var victimsInMonths = casesInRequestYear
                                .Join(_caseVictimRepository.Entities,
                                        caseInRequestYear => caseInRequestYear.Id,
                                        caseVictim => caseVictim.CaseId,
                                        (caseInRequestYear, caseVictim) => new
                                        {
                                            month = caseInRequestYear.StartDate.Month,
                                            caseId = caseVictim.CaseId,
                                            victimId = caseVictim.VictimId
                                        })
                                .GroupBy(gr => gr.month,
                                        gr => gr,
                                        (month, gr) => new { month, gr }).ToList();

            for (var i = 1; i <= 12; i++)
            {
                listResponse.Add(new CriminalSituationDevelopmentsResponse
                {
                    Month = i,
                    CaseCount = casesInRequestYear.Where(c => c.StartDate.Month == i).Count(),
                    CriminalCount = criminalsInMonths.Where(c => c.month == i).Sum(c => c.gr.Count()),
                    VictimCount = victimsInMonths.Where(v => v.month == i).Sum(v => v.gr.Count())
                });
            }

            return await Result<List<CriminalSituationDevelopmentsResponse>>.SuccessAsync(listResponse);

        }
    }
}
