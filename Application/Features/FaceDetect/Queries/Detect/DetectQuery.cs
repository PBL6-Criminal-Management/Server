using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Services;
using Application.Interfaces.WantedCriminal;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FaceDetect.Queries.Detect
{
    public class DetectQuery : IRequest<Result<DetectResponse>>
    {
        public IFormFile CriminalImage { get; set; } = default!;
    }

    internal class DetectQueryHandler : IRequestHandler<DetectQuery, Result<DetectResponse>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICriminalImageRepository _criminalImageRepository;
        private readonly IUploadService _uploadService;
        private readonly IFaceDetectService _faceDetectService;
        private readonly ICheckFileType _checkFileType;
        private readonly ICheckSizeFile _checkSizeFile;

        public DetectQueryHandler(
            ICriminalRepository criminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICaseRepository caseRepository,
            ICaseCriminalRepository caseCriminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IUploadService uploadService,
            IFaceDetectService faceDetectService,
            ICheckFileType checkFileType,
            ICheckSizeFile checkSizeFile)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _faceDetectService = faceDetectService;
            _checkFileType = checkFileType;
            _checkSizeFile = checkSizeFile;
        }

        public async Task<Result<DetectResponse>> Handle(DetectQuery request, CancellationToken cancellationToken)
        {
            var isFileImage = _checkFileType.CheckFilesIsImage(new Dtos.Requests.CheckImagesTypeRequest
            {
                Files = new List<IFormFile>() { request.CriminalImage }
            });

            if (isFileImage != "")
                return await Result<DetectResponse>.FailAsync(isFileImage);

            var isValidFileSize = _checkSizeFile.CheckImageSize(new Dtos.Requests.CheckImageSizeRequest
            {
                Files = new List<IFormFile> { request.CriminalImage }
            });

            if (isValidFileSize != "")
                return await Result<DetectResponse>.FailAsync(isValidFileSize);

            var detectResult = _faceDetectService.FaceDetect(request.CriminalImage, false);

            if (detectResult.Message != null)
            {
                if(detectResult.Message.Equals(StaticVariable.UNKNOWN))
                    return await Result<DetectResponse>.SuccessAsync(new DetectResponse
                    {
                        CanPredict = false,
                        ResultFile = detectResult.DetectResultFile,
                        DetectConfidence = null,
                        FoundCriminal = null
                    });
                else
                    return await Result<DetectResponse>.FailAsync(detectResult.Message);
            }

            var criminalId = detectResult.CriminalId;

            var caseOfCriminal = (from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == criminalId
                                  group _case by caseCriminal.CriminalId into g
                                  select new
                                  {
                                      g.OrderByDescending(c => c.StartDate).FirstOrDefault()!.Charge,
                                      RelatedCases = string.Join(", ", g.Select(_case => _case.Id))
                                  }).FirstOrDefault();

            var imageOfCriminal = (from criminalImage in _criminalImageRepository.Entities
                                   where criminalImage.CriminalId == criminalId
                                   select criminalImage.FilePath).FirstOrDefault();

            var query = await (from criminal in _criminalRepository.Entities
                               from wantedCriminal in _wantedCriminalRepository.Entities.Where(wantedCriminal => wantedCriminal.CriminalId == criminal.Id).DefaultIfEmpty()
                               where criminal.Id == criminalId && !criminal.IsDeleted
                               select new DetectResponse()
                               {
                                   CanPredict = true,
                                   FoundCriminal = new FoundCriminal
                                   {
                                       Id = criminal.Id,
                                       Name = criminal.Name,
                                       Birthday = criminal.Birthday,
                                       Gender = criminal.Gender,
                                       AnotherName = criminal.AnotherName,
                                       PhoneNumber = criminal.PhoneNumber,
                                       HomeTown = criminal.HomeTown,
                                       Nationality = criminal.Nationality,
                                       Ethnicity = criminal.Ethnicity,
                                       Religion = criminal.Religion,
                                       CitizenID = criminal.CitizenID,
                                       CareerAndWorkplace = criminal.CareerAndWorkplace,
                                       PermanentResidence = criminal.PermanentResidence,
                                       CurrentAccommodation = criminal.CurrentAccommodation,
                                       FatherName = criminal.FatherName,
                                       FatherBirthday = criminal.FatherBirthday,
                                       FatherCitizenID = criminal.FatherCitizenID,
                                       MotherName = criminal.MotherName,
                                       MotherBirthday = criminal.MotherBirthday,
                                       MotherCitizenID = criminal.MotherCitizenID,
                                       Characteristics = criminal.Characteristics,
                                       Status = criminal.Status,
                                       RelatedCases = caseOfCriminal != null ? caseOfCriminal.RelatedCases : null,
                                       DangerousLevel = criminal.DangerousLevel,
                                       Charge = caseOfCriminal != null ? caseOfCriminal.Charge : null,
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
                                       ImageLink = _uploadService.GetFullUrl(imageOfCriminal)
                                   }
                               }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (query == null)
                return await Result<DetectResponse>.SuccessAsync(new DetectResponse
                {
                    CanPredict = true,
                    ResultFile = detectResult.DetectResultFile,
                    DetectConfidence = detectResult.DetectConfidence,
                    FoundCriminal = null
                });

            query.ResultFile = detectResult.DetectResultFile;
            query.DetectConfidence = detectResult.DetectConfidence;

            return await Result<DetectResponse>.SuccessAsync(query);
        }
    }
}
