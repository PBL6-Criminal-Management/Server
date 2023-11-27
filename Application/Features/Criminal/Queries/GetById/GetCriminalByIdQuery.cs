using Application.Dtos.Requests.WantedCriminal;
using Application.Dtos.Responses.File;
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
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;

        public GetCriminalByIdQueryHandler(ICriminalRepository criminalRepository, IWantedCriminalRepository wantedCriminalRepository, ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository, ICriminalImageRepository criminalImageRepository, IUploadService uploadService, IMapper mapper)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _mapper = mapper;
        }

        public async Task<Result<GetCriminalByIdResponse>> Handle(GetCriminalByIdQuery request, CancellationToken cancellationToken)
        {
            var caseOfCriminal = (from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == request.Id
                                  group _case by caseCriminal.CriminalId into g
                                  select new { 
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.Charge,
                                      RelatedCases = string.Join(", ",g.Select(_case => _case.Id))
                                  }).FirstOrDefault();

            if(caseOfCriminal == null)
                return await Result<GetCriminalByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var imageOfCriminal = (from criminalImage in _criminalImageRepository.Entities
                                 where criminalImage.CriminalId == request.Id && !criminalImage.IsDeleted
                                 select criminalImage.FilePath).FirstOrDefault();

            var criminalImages = from criminalImage in _criminalImageRepository.Entities
                                 where criminalImage.CriminalId == request.Id && !criminalImage.IsDeleted
                                 select criminalImage;

            var query = await  (from criminal in _criminalRepository.Entities
                                where criminal.Id == request.Id && !criminal.IsDeleted
                                select new GetCriminalByIdResponse()
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
                                    RelatedCases = caseOfCriminal.RelatedCases,
                                    DangerousLevel = criminal.DangerousLevel,
                                    Charge = caseOfCriminal.Charge,
                                    DateOfMostRecentCrime = criminal.DateOfMostRecentCrime,
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
                                    Image = imageOfCriminal,
                                    ImageLink = _uploadService.GetFullUrl(imageOfCriminal),
                                }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (query == null)
                return await Result<GetCriminalByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var fileResponse = criminalImages.Select(i => new FileResponse
            {
                FileName = i.FileName,
                FileUrl = _uploadService.GetFullUrl(i.FilePath)
            });

            query.CriminalImages = fileResponse.ToList();

            var wantedCriminals = _wantedCriminalRepository.Entities.Where(wantedCriminal => wantedCriminal.CriminalId == request.Id);
            query.IsWantedCriminal = wantedCriminals.Any();
            query.WantedCriminals = _mapper.Map<List<WantedCriminalRequest>>(wantedCriminals);

            return await Result<GetCriminalByIdResponse>.SuccessAsync(query);
        }
    }
}