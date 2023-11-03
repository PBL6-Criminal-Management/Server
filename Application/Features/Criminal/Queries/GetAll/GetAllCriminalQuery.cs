using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Criminal.Queries.GetAll
{
    public class GetAllCriminalQuery : GetAllCriminalParameter, IRequest<PaginatedResult<GetAllCriminalResponse>>
    {
    }
    internal class GetAllCriminalQueryHandler : IRequestHandler<GetAllCriminalQuery, PaginatedResult<GetAllCriminalResponse>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;

        public GetAllCriminalQueryHandler(ICriminalRepository criminalRepository, ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository)
        {
            _criminalRepository = criminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
        }
        public async Task<PaginatedResult<GetAllCriminalResponse>> Handle(GetAllCriminalQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();

            var caseOfCriminals = from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted
                                  group _case by caseCriminal.CriminalId into g
                                  select new
                                  {
                                      CriminalId = g.Key,
                                      //g.MaxBy(c => c.StartDate)!.Charge,
                                      //g.MaxBy(c => c.StartDate)!.TypeOfViolation,
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.Charge,
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.TypeOfViolation,
                                      DateOfMostRecentCrime = DateOnly.FromDateTime(g.Max(c => c.StartDate))
                                  };

            var query = _criminalRepository.Entities.AsEnumerable()
                            .Join(caseOfCriminals, criminal => criminal.Id,
                            caseOfCriminal => caseOfCriminal.CriminalId,
                            (criminal, caseOfCriminals) => new { criminal, caseOfCriminals })
                            .Where(o => !o.criminal.IsDeleted
                                    && (string.IsNullOrEmpty(request.Keyword) || StringHelper.Contains(o.criminal.Name, request.Keyword)
                                                                || StringHelper.Contains(o.criminal.AnotherName, request.Keyword)
                                                                || StringHelper.Contains(o.criminal.HomeTown, request.Keyword))
                                    && (request.Status.HasValue || o.criminal.Status == request.Status)
                                    && (request.YearOfBirth.HasValue || o.criminal.Birthday.Year == request.YearOfBirth)
                                    && (request.Gender.HasValue || o.criminal.Gender == request.Gender)
                                    && (string.IsNullOrEmpty(request.Characteristics) || StringHelper.Contains(o.criminal.Characteristics, request.Characteristics))
                                    && (request.TypeOfViolation.HasValue || o.caseOfCriminals.TypeOfViolation == request.TypeOfViolation)
                            && (string.IsNullOrEmpty(request.Area) || StringHelper.Contains(o.criminal.HomeTown, request.Area))
                            && (string.IsNullOrEmpty(request.Charge) || StringHelper.Contains(o.caseOfCriminals.Charge, request.Charge))
                            )
                            .Select(o => new GetAllCriminalResponse
                            {
                                Id = o.criminal.Id,
                                Name = o.criminal.Name,
                                YearOfBirth = o.criminal.Birthday.Year,
                                PermanentResidence = o.criminal.PermanentResidence,
                                Status = o.criminal.Status,
                                Charge = o.caseOfCriminals.Charge,
                                DateOfMostRecentCrime = o.caseOfCriminals.DateOfMostRecentCrime,
                                CreatedAt = o.criminal.CreatedAt
                            });

            var data = query.AsQueryable().OrderBy(request.OrderBy);
            List<GetAllCriminalResponse> result;

            //Pagination
            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            var totalRecord = result.Count();
            return PaginatedResult<GetAllCriminalResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);

        }
    }
}
