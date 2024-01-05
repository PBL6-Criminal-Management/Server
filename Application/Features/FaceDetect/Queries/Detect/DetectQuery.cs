using Application.Dtos.Responses.DetectResult;
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
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;

        public DetectQueryHandler(
            ICriminalRepository criminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICaseRepository caseRepository,
            ICaseCriminalRepository caseCriminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IUploadService uploadService,
            ICheckFileType checkFileType,
            ICheckFileSize checkFileSize, 
            IMapper mapper)
        {
            _criminalRepository = criminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _checkFileType = checkFileType;
            _checkFileSize = checkFileSize;
            _mapper = mapper;
        }

        public async Task<Result<DetectResponse>> Handle(DetectQuery request, CancellationToken cancellationToken)
        {
            var isFileImage = _checkFileType.CheckFileIsImage(request.CriminalImage);                

            if (isFileImage != "")
                return await Result<DetectResponse>.FailAsync(isFileImage);

            var isValidFileSize = _checkFileSize.CheckImageSize(request.CriminalImage);

            if (isValidFileSize != "")
                return await Result<DetectResponse>.FailAsync(isValidFileSize);

            var stream = request.CriminalImage.OpenReadStream();

            //Compress image to not be overflow free server's memory
            if (stream.Length >= StaticVariable.MAX_IMAGE_SIZE_FOR_FREE_AISERVER)
            {
                var magicImage = new MagickImage(stream);
                var width = magicImage.Width;
                do
                {
                    width -= 300;
                    var byteArr = ResizeImage(magicImage, width);
                    if (byteArr == null)
                        break;

                    stream = new MemoryStream(byteArr);
                } while (stream.Length >= StaticVariable.MAX_IMAGE_SIZE_FOR_FREE_AISERVER);
            }

            DetectResult? detectResult = null;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = Timeout.InfiniteTimeSpan;
                try
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        // Add the image file to the request
                        content.Add(new StreamContent(stream), "CriminalImage", "image.jpg");
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

            if (detectResult.isPredictable == null || !(bool)detectResult.isPredictable || detectResult.label == null)
            {
                return await Result<DetectResponse>.SuccessAsync(new DetectResponse
                {
                    CanPredict = false,
                    ResultFile = detectResult.image,
                    DetectConfidence = null,
                    FoundCriminal = null
                });
            }

            int criminalId = (int)detectResult.label;

            var criminal = _criminalRepository.Entities.FirstOrDefault(c => c.Id == criminalId);

            if (criminal == null || criminal.IsDeleted)
                return await Result<DetectResponse>.SuccessAsync(new DetectResponse
                {
                    CanPredict = true,
                    ResultFile = detectResult.image,
                    DetectConfidence = detectResult.confidence,
                    FoundCriminal = null
                });

            var caseOfCriminal = (from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == criminal.Id
                                  group _case by caseCriminal.CriminalId into g
                                  select new
                                  {
                                      DateOfMostRecentCrime = DateOnly.FromDateTime(g.Max(c => c.StartDate))
                                  }).FirstOrDefault();

            var query = new DetectResponse()
            {
                CanPredict = true,
                FoundCriminal = new FoundCriminal()
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
                }
            };

            var casesOfCriminal = from caseCriminal in _caseCriminalRepository.Entities
                                  join _case in _caseRepository.Entities on caseCriminal.CaseId equals _case.Id
                                  where !_case.IsDeleted && !caseCriminal.IsDeleted && caseCriminal.CriminalId == criminalId
                                  select _case;

            var criminalImages = from criminalImage in _criminalImageRepository.Entities
                                 where criminalImage.CriminalId == criminalId && !criminalImage.IsDeleted
                                 select criminalImage;

            query.FoundCriminal.CriminalImages = criminalImages.Select(i => new FileResponse
            {
                FileName = i.FileName,
                FilePath = i.FilePath,
                FileUrl = _uploadService.GetFullUrl(i.FilePath)
            }).ToList();

            var wantedCriminals = _wantedCriminalRepository.Entities.Where(wantedCriminal => wantedCriminal.CriminalId == criminalId && !wantedCriminal.IsDeleted).OrderBy(w => w.WantedDecisionDay);
            query.FoundCriminal.IsWantedCriminal = wantedCriminals.Any();
            query.FoundCriminal.WantedCriminals = _mapper.Map<List<WantedCriminalResponse>>(wantedCriminals);

            List<long> listCaseIds = new List<long>();

            if (casesOfCriminal != null)
            {
                foreach (var _case in casesOfCriminal)
                {
                    listCaseIds.Add(_case.Id);

                    query.FoundCriminal.WantedCriminals = query.FoundCriminal.WantedCriminals.Select(criminal =>
                    {
                        if (criminal.CaseId == _case.Id)
                        {
                            criminal.Charge = _case.Charge;
                        }

                        return criminal;
                    }).ToList();
                }
                query.FoundCriminal.RelatedCases = string.Join(", ", listCaseIds);
                query.FoundCriminal.Charge = casesOfCriminal.OrderByDescending(c => c.StartDate).FirstOrDefault()?.Charge;
            }

            query.ResultFile = detectResult.image;
            query.DetectConfidence = detectResult.confidence;

            return await Result<DetectResponse>.SuccessAsync(query);
        }

        byte[]? ResizeImage(MagickImage image, int width)
        {
            if (width > 0)
            {
                var size = new MagickGeometry(width, 0);
                image.Resize(size);

                return image.ToByteArray();
            }
            else
                return null;
        }     
    }
}
