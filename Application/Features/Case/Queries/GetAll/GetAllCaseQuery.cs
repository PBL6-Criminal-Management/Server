using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Domain.Constants;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Case.Queries.GetAll
{
    public class GetAllCaseQuery : GetAllCaseParameter, IRequest<PaginatedResult<GetAllCaseResponse>>
    {
    }
    internal class GetAllCaseQueryHandler : IRequestHandler<GetAllCaseQuery, PaginatedResult<GetAllCaseResponse>>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICriminalRepository _criminalRepository;
        private readonly IDateTimeService _dateTimeService;
        public GetAllCaseQueryHandler(ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository,
            IDateTimeService dateTimeService, ICriminalRepository criminalRepository)
        {
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _dateTimeService = dateTimeService;
            _criminalRepository = criminalRepository;
        }
        public async Task<PaginatedResult<GetAllCaseResponse>> Handle(GetAllCaseQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();

            var query = _caseRepository.Entities.AsEnumerable()
                        .Where(c => !c.IsDeleted &&
                        (string.IsNullOrEmpty(request.Keyword) || StringHelper.Contains(c.Charge, request.Keyword)
                                                                || StringHelper.Contains(c.CrimeScene, request.Keyword)
                                                                || StringHelper.Contains(c.TypeOfViolation.ToDescriptionString(), request.Keyword)
                                                                || (_dateTimeService.ConvertToUtc(c.StartDate) + " - " + (c.EndDate.HasValue ? _dateTimeService.ConvertToUtc(c.EndDate.Value) : "")).Contains(request.Keyword)
                                                                ) &&
                        (!request.Status.HasValue || c.Status == request.Status) &&
                        (!request.TypeOfViolation.HasValue || c.TypeOfViolation == request.TypeOfViolation) &&
                        (string.IsNullOrEmpty(request.Area) || StringHelper.Contains(c.CrimeScene, request.Area)))
                        .AsQueryable()
                        .Select(c => new GetAllCaseResponse
                        {
                            Id = c.Id,
                            Code = StaticVariable.CASE + c.Id.ToString().PadLeft(5, '0'),
                            Charge = c.Charge,
                            TimeTakesPlace = _dateTimeService.ConvertToUtc(c.StartDate) + " - " + (c.EndDate.HasValue ? _dateTimeService.ConvertToUtc(c.EndDate.Value) : ""),
                            TypeOfViolation = c.TypeOfViolation,
                            Status = c.Status,
                            Area = c.CrimeScene,
                            CriminalOfCase = _caseCriminalRepository.Entities.Where(_ => !_.IsDeleted && _.CaseId == c.Id)
                                                .Join(_criminalRepository.Entities,
                                                    cCr => cCr.CriminalId,
                                                    cr => cr.Id,
                                                    (cCr, cr) => new GetAllCaseResponse.CriminalId
                                                    {
                                                        Id = cCr.CriminalId,
                                                        Name = cr.Name
                                                    })
                                                .ToList(),
                            CreatedAt = c.CreatedAt
                        })
                        .ToList();
            if (request.TimeTakesPlace.HasValue)
            {
                var dateCheck = request.TimeTakesPlace.Value.ToString("HH:mm dd/MM/yyyy");
                for (int i = 0; i < query.Count(); i++)
                {
                    var c = query[i];

                    if (c.TimeTakesPlace == null || !_dateTimeService.IsBetween(c.TimeTakesPlace, dateCheck))
                    {
                        query.RemoveAt(i);
                        i--;
                    }
                }
            }
            var data = query.AsQueryable().OrderBy(request.OrderBy);
            List<GetAllCaseResponse> result;

            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            var totalRecord = result.Count();
            return PaginatedResult<GetAllCaseResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);
        }
    }
}