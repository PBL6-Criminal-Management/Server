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
        public string? MurderWeapon { get; set; }
    }
    internal class GetAllWantedCriminalHandler : IRequestHandler<GetAllWantedCriminalQuery, PaginatedResult<GetAllWantedCriminalResponse>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IUploadService _uploadService;

        public GetAllWantedCriminalHandler(
            ICriminalRepository criminalRepository, 
            IWantedCriminalRepository wantedCriminalRepository, 
            ICaseRepository caseRepository, 
            ICaseCriminalRepository caseCriminalRepository,
            IUploadService uploadService
        )
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _uploadService = uploadService;
        }
        public async Task<PaginatedResult<GetAllWantedCriminalResponse>> Handle(GetAllWantedCriminalQuery request, CancellationToken cancellationToken)
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
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.Charge,
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.MurderWeapon
                                  };

            var wantedOfCriminals = from wantedCriminal in _wantedCriminalRepository.Entities
                                  where !wantedCriminal.IsDeleted
                                  group wantedCriminal by wantedCriminal.CriminalId into g
                                   select new
                                  {
                                      CriminalId = g.Key,
                                      g.OrderByDescending(c => c.WantedDecisionDay).FirstOrDefault()!.WantedType,
                                      g.OrderByDescending(c => c.WantedDecisionDay).FirstOrDefault()!.DecisionMakingUnit
                                  };

            var query = _criminalRepository.Entities
                            .Include(c => c.CriminalImages)
                            .AsEnumerable()
                            .Join(caseOfCriminals, 
                                  criminal => criminal.Id,
                                  caseOfCriminal => caseOfCriminal.CriminalId,
                                  (criminal, caseOfCriminals) => new { criminal, caseOfCriminals })
                            .Join(wantedOfCriminals, 
                                  gr => gr.criminal.Id,
                                  wantedOfCriminals => wantedOfCriminals.CriminalId,
                                  (gr, wantedOfCriminals) => new { gr.criminal, gr.caseOfCriminals, wantedOfCriminals })
                            .Where(o => !o.criminal.IsDeleted
                                    && (string.IsNullOrEmpty(request.Keyword)
                                                                || StringHelper.Contains(o.criminal.Name, request.Keyword)
                                                                || o.criminal.Birthday.Year.ToString().StartsWith(request.Keyword)
                                                                || StringHelper.Contains(o.caseOfCriminals.Charge, request.Keyword)
                                                                || StringHelper.Contains(o.criminal.Characteristics, request.Keyword)
                                                                || StringHelper.Contains(o.caseOfCriminals.MurderWeapon, request.Keyword)
                                                                || StringHelper.Contains(o.wantedOfCriminals.WantedType.ToDescriptionString(), request.Keyword)
                                                                )
                                    && (string.IsNullOrWhiteSpace(request.Name) || StringHelper.Contains(o.criminal.Name, request.Name))
                                    && (string.IsNullOrWhiteSpace(request.Charge) || StringHelper.Contains(o.caseOfCriminals.Charge, request.Charge))
                                    && (string.IsNullOrWhiteSpace(request.Characteristics) || StringHelper.Contains(o.criminal.Characteristics, request.Characteristics))
                                    && (string.IsNullOrWhiteSpace(request.DecisionMakingUnit) || StringHelper.Contains(o.wantedOfCriminals.DecisionMakingUnit, request.DecisionMakingUnit))
                                    && (string.IsNullOrWhiteSpace(request.PermanentResidence) || StringHelper.Contains(o.criminal.PermanentResidence, request.PermanentResidence))
                                    && (string.IsNullOrWhiteSpace(request.MurderWeapon) || StringHelper.Contains(o.caseOfCriminals.MurderWeapon, request.MurderWeapon))
                                    && (!request.WantedType.HasValue || o.wantedOfCriminals.WantedType == request.WantedType)
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
                                Charge = o.caseOfCriminals.Charge,
                                WantedType = o.wantedOfCriminals.WantedType,
                                Avatar = _uploadService.GetFullUrl(_uploadService.IsFileExists(o.criminal.CriminalImages?.FirstOrDefault()?.FilePath) ? o.criminal.CriminalImages!.FirstOrDefault()!.FilePath : "Files/Avatar/NotFound/notFoundAvatar.jpg"),
                                MurderWeapon = o.caseOfCriminals.MurderWeapon,
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
