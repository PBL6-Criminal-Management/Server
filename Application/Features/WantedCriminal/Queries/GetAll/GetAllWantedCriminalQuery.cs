using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.WantedCriminal;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.WantedCriminal.Queries.GetAll
{
    public class GetAllWantedCriminalQuery : GetAllWantedCriminalParameter, IRequest<PaginatedResult<GetAllWantedCriminalResponse>>
    {
        public string? Name { get; set; }
        public string? Charge { get; set; }
        public string? Characteristics { get; set; }
        public string? DecisionMakingUnit { get; set; }
        public string? PermanentResidence { get; set; }
        public string? Weapon { get; set; }
    }
    internal class GetAllWantedCriminalHandler : IRequestHandler<GetAllWantedCriminalQuery, PaginatedResult<GetAllWantedCriminalResponse>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly IUploadService _uploadService;
        private readonly ICaseCriminalRepository _caseCriminalRepository;

        public GetAllWantedCriminalHandler(
            ICriminalRepository criminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICaseRepository caseRepository,
            IUploadService uploadService,
            ICaseCriminalRepository caseCriminalRepository
        )
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _uploadService = uploadService;
            _caseCriminalRepository = caseCriminalRepository;
        }
        public async Task<PaginatedResult<GetAllWantedCriminalResponse>> Handle(GetAllWantedCriminalQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();

            var wantedOfCriminals = from wantedCriminal in _wantedCriminalRepository.Entities
                                    where !wantedCriminal.IsDeleted
                                    group wantedCriminal by wantedCriminal.CriminalId into g
                                    select new
                                    {
                                        CriminalId = g.Key,
                                        g.OrderByDescending(c => c.WantedDecisionDay).FirstOrDefault()!.WantedType,
                                        g.OrderByDescending(c => c.WantedDecisionDay).FirstOrDefault()!.DecisionMakingUnit,
                                        g.OrderByDescending(c => c.WantedDecisionDay).FirstOrDefault()!.CaseId,
                                    };

            var wantedAndCaseOfCriminals = from wantedCriminal in wantedOfCriminals
                                           join caseCriminal in _caseCriminalRepository.Entities on new { wantedCriminal.CriminalId, wantedCriminal.CaseId } equals new { caseCriminal.CriminalId, caseCriminal.CaseId }
                                           where !caseCriminal.IsDeleted
                                           select new
                                           {
                                               wantedCriminal.CriminalId,
                                               wantedCriminal.WantedType,
                                               wantedCriminal.DecisionMakingUnit,
                                               caseCriminal.Charge,
                                               caseCriminal.Weapon
                                           };

            var query = _criminalRepository.Entities
                            .Include(c => c.CriminalImages)
                            .AsEnumerable()
                            .Join(wantedAndCaseOfCriminals,
                                  criminal => criminal.Id,
                                  wantedAndCaseOfCriminal => wantedAndCaseOfCriminal.CriminalId,
                                  (criminal, wantedAndCaseOfCriminal) => new { criminal, wantedAndCaseOfCriminal })
                            .Where(o => !o.criminal.IsDeleted && o.criminal.Status == Domain.Constants.Enum.CriminalStatus.Wanted
                                    && (string.IsNullOrEmpty(request.Keyword)
                                                                || StringHelper.Contains(o.criminal.Name, request.Keyword)
                                                                || o.criminal.Birthday.Year.ToString().StartsWith(request.Keyword)
                                                                || StringHelper.Contains(o.wantedAndCaseOfCriminal.Charge, request.Keyword)
                                                                || StringHelper.Contains(o.criminal.Characteristics, request.Keyword)
                                                                || StringHelper.Contains(o.wantedAndCaseOfCriminal.Weapon, request.Keyword)
                                                                || StringHelper.Contains(o.wantedAndCaseOfCriminal.WantedType.ToDescriptionString(), request.Keyword)
                                                                )
                                    && (string.IsNullOrWhiteSpace(request.Name) || StringHelper.Contains(o.criminal.Name, request.Name))
                                    && (string.IsNullOrWhiteSpace(request.Charge) || StringHelper.Contains(o.wantedAndCaseOfCriminal.Charge, request.Charge))
                                    && (string.IsNullOrWhiteSpace(request.Characteristics) || StringHelper.Contains(o.criminal.Characteristics, request.Characteristics))
                                    && (string.IsNullOrWhiteSpace(request.DecisionMakingUnit) || StringHelper.Contains(o.wantedAndCaseOfCriminal.DecisionMakingUnit, request.DecisionMakingUnit))
                                    && (string.IsNullOrWhiteSpace(request.PermanentResidence) || StringHelper.Contains(o.criminal.PermanentResidence, request.PermanentResidence))
                                    && (string.IsNullOrWhiteSpace(request.Weapon) || StringHelper.Contains(o.wantedAndCaseOfCriminal.Weapon, request.Weapon))
                                    && (!request.WantedType.HasValue || o.wantedAndCaseOfCriminal.WantedType == request.WantedType)
                                    && (!request.YearOfBirth.HasValue || o.criminal.Birthday.Year == request.YearOfBirth)
                            )
                            .Select(o => new GetAllWantedCriminalResponse
                            {
                                Id = o.criminal.Id,
                                Name = o.criminal.Name,
                                AnotherName = o.criminal.AnotherName,
                                YearOfBirth = o.criminal.Birthday.Year,
                                PermanentResidence = o.criminal.PermanentResidence,
                                Characteristics = o.criminal.Characteristics,
                                Charge = o.wantedAndCaseOfCriminal.Charge,
                                WantedType = o.wantedAndCaseOfCriminal.WantedType,
                                Avatar = _uploadService.GetFullUrl(_uploadService.IsFileExists(o.criminal.CriminalImages?.FirstOrDefault()?.FilePath) ? o.criminal.CriminalImages!.FirstOrDefault()!.FilePath : "Files/Avatar/NotFound/notFoundAvatar.jpg"),
                                Weapon = o.wantedAndCaseOfCriminal.Weapon,
                                CreatedAt = o.criminal.CreatedAt,
                            });

            var data = query
                .AsQueryable()
                .OrderBy(request.OrderBy);
            List<GetAllWantedCriminalResponse> result;

            //Pagination
            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            var totalRecord = result.Count();
            return PaginatedResult<GetAllWantedCriminalResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);

        }
    }
}
