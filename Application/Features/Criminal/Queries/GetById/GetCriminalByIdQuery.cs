using Application.Dtos.Responses.File;
using Application.Dtos.Responses.WantedCriminal;
using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.WantedCriminal;
using AutoMapper;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;

namespace Application.Features.Criminal.Queries.GetById
{
    public class GetCriminalByIdQuery : IRequest<Result<GetCriminalByIdResponse>>
    {
        public long Id { get; set; }
    }

    internal class GetCriminalByIdQueryHandler : IRequestHandler<GetCriminalByIdQuery, Result<GetCriminalByIdResponse>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICriminalImageRepository _criminalImageRepository;
        private readonly IUploadService _uploadService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetCriminalByIdQueryHandler(ICriminalRepository criminalRepository, IWantedCriminalRepository wantedCriminalRepository, ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository, ICriminalImageRepository criminalImageRepository, IUploadService uploadService,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GetCriminalByIdResponse>> Handle(GetCriminalByIdQuery request, CancellationToken cancellationToken)
        {
            var criminal = _criminalRepository.Entities.FirstOrDefault(c => c.Id == request.Id);

            if (criminal == null || criminal.IsDeleted)
                return await Result<GetCriminalByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            if (_currentUserService.Username == null)
            {
                if (!_wantedCriminalRepository.Entities.Any(w => w.CriminalId == request.Id && !w.IsDeleted) || criminal.Status != Domain.Constants.Enum.CriminalStatus.Wanted)
                    return await Result<GetCriminalByIdResponse>.FailAsync(StaticVariable.NOT_VIEW_CRIMINAL_INFOR_PERMISSION);
            }

            var caseOfCriminal = (from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == criminal.Id
                                  group _case by caseCriminal.CriminalId into g
                                  select new
                                  {
                                      DateOfMostRecentCrime = DateOnly.FromDateTime(g.Max(c => c.StartDate))
                                  }).FirstOrDefault();

            var query = new GetCriminalByIdResponse()
            {
                Name = criminal.Name,
                Birthday = criminal.Birthday,
                Gender = criminal.Gender,
                AnotherName = criminal.AnotherName,
                PhoneNumber = criminal.PhoneNumber,
                HomeTown = criminal.HomeTown,
                Nationality = criminal.Nationality,
                Ethnicity = criminal.Ethnicity,
                Religion = criminal.Religion,
                CitizenId = criminal.CitizenId,
                CareerAndWorkplace = criminal.CareerAndWorkplace,
                PermanentResidence = criminal.PermanentResidence,
                CurrentAccommodation = criminal.CurrentAccommodation,
                FatherName = criminal.FatherName,
                FatherBirthday = criminal.FatherBirthday,
                FatherCitizenId = criminal.FatherCitizenId,
                MotherName = criminal.MotherName,
                MotherBirthday = criminal.MotherBirthday,
                MotherCitizenId = criminal.MotherCitizenId,
                Characteristics = criminal.Characteristics,
                Status = criminal.Status,
                DangerousLevel = criminal.DangerousLevel,
                DateOfMostRecentCrime = caseOfCriminal?.DateOfMostRecentCrime,
                ReleaseDate = criminal.ReleaseDate,
                EntryAndExitInformation = criminal.EntryAndExitInformation,
                BankAccount = criminal.BankAccount,
                GameAccount = criminal.GameAccount,
                Facebook = criminal.Facebook,
                Zalo = criminal.Zalo,
                OtherSocialNetworks = criminal.OtherSocialNetworks,
                PhoneModel = criminal.PhoneModel,
                Research = criminal.Research,
                ApproachArrange = criminal.ApproachArrange,
                OtherInformation = criminal.OtherInformation,
                Vehicles = criminal.Vehicles,
                IsWantedCriminal = true,
                Avatar = criminal.Avatar,
                AvatarLink = _uploadService.GetFullUrl(_uploadService.IsFileExists(criminal.Avatar) ? criminal.Avatar : "Files/Avatar/NotFound/notFoundAvatar.jpg"),
            };

            var casesOfCriminal = from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == request.Id
                                  select _case;

            var criminalImages = from criminalImage in _criminalImageRepository.Entities
                                 where criminalImage.CriminalId == request.Id && !criminalImage.IsDeleted
                                 select criminalImage;

            query.CriminalImages = criminalImages.Select(i => new FileResponse
            {
                FileName = i.FileName,
                FilePath = i.FilePath,
                FileUrl = _uploadService.GetFullUrl(i.FilePath)
            }).ToList();

            var wantedCriminals = _wantedCriminalRepository.Entities.Where(wantedCriminal => wantedCriminal.CriminalId == request.Id && !wantedCriminal.IsDeleted).OrderBy(w => w.WantedDecisionDay);
            query.IsWantedCriminal = wantedCriminals.Any();
            query.WantedCriminals = _mapper.Map<List<WantedCriminalResponse>>(wantedCriminals);

            List<long> listCaseIds = new List<long>();

            if (casesOfCriminal != null)
            {
                foreach (var _case in casesOfCriminal)
                {
                    listCaseIds.Add(_case.Id);

                    foreach (var wantedCriminal in query.WantedCriminals)
                    {
                        if (wantedCriminal.CaseId == _case.Id)
                        {
                            var caseCriminal = await _caseCriminalRepository.FindAsync(_ => _.CaseId == _case.Id && _.CriminalId == request.Id && !_.IsDeleted);
                            wantedCriminal.Weapon = caseCriminal == null || String.IsNullOrEmpty(caseCriminal.Weapon) ? "" : caseCriminal.Weapon;
                            wantedCriminal.Charge = caseCriminal == null || String.IsNullOrEmpty(caseCriminal.Weapon) ? "" : caseCriminal.Weapon;
                        }

                    }
                }
                query.RelatedCases = string.Join(", ", listCaseIds);
                // query.Charge
                var caseIdLast = casesOfCriminal.OrderByDescending(c => c.StartDate).FirstOrDefault()?.Id;
                if (caseIdLast == null)
                {
                    query.Charge = "";
                }
                else
                {
                    var chargeOfCase = await _caseCriminalRepository.FindAsync(_ => _.CaseId == caseIdLast && _.CriminalId == request.Id && !_.IsDeleted);
                    query.Charge = chargeOfCase == null || String.IsNullOrEmpty(chargeOfCase.Charge) ? "" : chargeOfCase.Charge;
                }
            }
            return await Result<GetCriminalByIdResponse>.SuccessAsync(query);
        }
    }
}