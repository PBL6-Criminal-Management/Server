using Application.Dtos.Responses.DetectResult;
using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.WantedCriminal;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        private readonly ICheckFileType _checkFileType;
        private readonly ICheckFileSize _checkFileSize;

        public DetectQueryHandler(
            ICriminalRepository criminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICaseRepository caseRepository,
            ICaseCriminalRepository caseCriminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IUploadService uploadService,
            ICheckFileType checkFileType,
            ICheckFileSize checkFileSize)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _checkFileType = checkFileType;
            _checkFileSize = checkFileSize;
        }

        public async Task<Result<DetectResponse>> Handle(DetectQuery request, CancellationToken cancellationToken)
        {
            var isFileImage = _checkFileType.CheckFileIsImage(request.CriminalImage);

            if (isFileImage != "")
                return await Result<DetectResponse>.FailAsync(isFileImage);

            var isValidFileSize = _checkFileSize.CheckImageSize(request.CriminalImage);

            if (isValidFileSize != "")
                return await Result<DetectResponse>.FailAsync(isValidFileSize);

            DetectResult? detectResult = null;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = Timeout.InfiniteTimeSpan;
                try
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        // Add the image file to the request
                        content.Add(new StreamContent(request.CriminalImage.OpenReadStream()), "CriminalImage", "image.jpg");
                        HttpResponseMessage response = await client.PostAsync(StaticVariable.AI_SERVER_BASE_URL + "/detect", content);

                        // Read and deserialize the JSON content
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        detectResult = JsonConvert.DeserializeObject<DetectResult>(jsonContent);
                        if (detectResult == null)
                            return await Result<DetectResponse>.FailAsync($"Lỗi server");
                    }                    
                }
                catch (Exception ex)
                {
                    return await Result<DetectResponse>.FailAsync($"Lỗi: {ex.Message}");
                }
            }

            if(detectResult.error != null)
                return await Result<DetectResponse>.FailAsync(detectResult.error);

            if(detectResult.message != null)
                return await Result<DetectResponse>.SuccessAsync(detectResult.message);

            if (detectResult.isPredictable == null || !(bool)detectResult.isPredictable)
            {
                return await Result<DetectResponse>.SuccessAsync(new DetectResponse
                {
                    CanPredict = false,
                    ResultFile = detectResult.image,
                    DetectConfidence = null,
                    FoundCriminal = null
                });
            }

            int criminalId = detectResult.label != null ? (int)detectResult.label : - 1;

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
                    ResultFile = detectResult.image,
                    DetectConfidence = detectResult.confidence,
                    FoundCriminal = null
                });

            query.ResultFile = detectResult.image;
            query.DetectConfidence = detectResult.confidence;

            return await Result<DetectResponse>.SuccessAsync(query);
        }
    }
}
