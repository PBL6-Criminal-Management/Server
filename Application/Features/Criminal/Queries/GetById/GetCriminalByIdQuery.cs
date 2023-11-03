using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.WantedCriminal;
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

        public GetCriminalByIdQueryHandler(ICriminalRepository criminalRepository, IWantedCriminalRepository wantedCriminalRepository, ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository, ICriminalImageRepository criminalImageRepository, IUploadService uploadService)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
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
                                 where criminalImage.CriminalId == request.Id
                                 select criminalImage.FilePath).FirstOrDefault();

            var query = await  (from criminal in _criminalRepository.Entities
                                from wantedCriminal in _wantedCriminalRepository.Entities.Where(wantedCriminal => wantedCriminal.CriminalId == criminal.Id).DefaultIfEmpty()
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
                                    CMND_CCCD = criminal.CMND_CCCD,
                                    CareerAndWorkplace = criminal.CareerAndWorkplace,
                                    PermanentResidence = criminal.PermanentResidence,
                                    CurrentAccommodation = criminal.CurrentAccommodation,
                                    FatherName = criminal.FatherName,
                                    FatherBirthday = criminal.FatherBirthday,
                                    Father_CMND_CCCD = criminal.Father_CMND_CCCD,
                                    MotherName = criminal.MotherName,
                                    MotherBirthday = criminal.MotherBirthday,
                                    Mother_CMND_CCCD = criminal.Mother_CMND_CCCD,
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
                                    IsWantedCriminal = wantedCriminal != null,
                                    WantedType = wantedCriminal.WantedType,
                                    CurrentActivity = wantedCriminal.CurrentActivity,
                                    WantedDecisionNo = wantedCriminal.WantedDecisionNo,
                                    WantedDecisionDay = wantedCriminal.WantedDecisionDay,
                                    DecisionMakingUnit = wantedCriminal.DecisionMakingUnit,
                                    Image = imageOfCriminal,
                                    ImageLink = _uploadService.GetFullUrl(imageOfCriminal),
                                }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (query == null)
                return await Result<GetCriminalByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            return await Result<GetCriminalByIdResponse>.SuccessAsync(query);
        }
    }
}