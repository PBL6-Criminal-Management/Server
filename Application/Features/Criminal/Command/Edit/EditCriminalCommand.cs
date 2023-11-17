using Application.Dtos.Requests.Image;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using MediatR;
using Domain.Wrappers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using AutoMapper;
using Domain.Constants.Enum;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities.CriminalImage;

namespace Application.Features.Criminal.Command.Edit
{
    public class EditCriminalCommand : IRequest<Result<EditCriminalCommand>>
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ANOTHER_NAME)]
        public string AnotherName { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        [JsonPropertyName("citizen_id")]
        public string CitizenID { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_PHONE_MODEL)]
        public string PhoneModel { get; set; } = null!;
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_CAREER_AND_WORKPLACE)]
        public string CareerAndWorkplace { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_CHRACTERISTICS)]
        public string Characteristics { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_HOME_TOWN)]
        public string HomeTown { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_ETHNICITY)]
        public string Ethnicity { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_RELIGION)]
        public string? Religion { get; set; }
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_NATIONALITY)]
        public string Nationality { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FATHER_NAME)]
        public string FatherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_FATHER_CITIZEN_ID)]
        [JsonPropertyName("father_citizen_id")]
        public string FatherCitizenID { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly FatherBirthday { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MOTHER_NAME)]
        public string MotherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_MOTHER_CITIZEN_ID)]
        [JsonPropertyName("mother_citizen_id")]
        public string MotherCitizenID { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly MotherBirthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_PERMANENT_RESIDENCE)]
        public string PermanentResidence { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CURRENT_ACCOMMODATION)]
        public string CurrentAccommodation { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_ENTRY_AND_EXITINFORMATION)]
        public string? EntryAndExitInformation { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FACEBOOK)]
        public string? Facebook { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ZALO)]
        public string? Zalo { get; set; }
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_OTHER_SOCIAL_NETWORKS)]
        public string? OtherSocialNetworks { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_GAME_ACCOUNT)]
        public string? GameAccount { get; set; }
        [MaxLength(30, ErrorMessage = StaticVariable.LIMIT_BANK_ACCOUNT)]
        public string? BankAccount { get; set; }
        public string? Research { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_VEHICLES)]
        public string? Vehicles { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_DANGEROUS_LEVEL)]
        public string? DangerousLevel { get; set; }
        public string? ApproachArrange { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly DateOfMostRecentCrime { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? ReleaseDate { get; set; }
        public CriminalStatus Status { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_OTHER_INFORMATION)]
        public string? OtherInformation { get; set; }
        public List<ImageRequest>? CriminalImages { get; set; }
    }
    internal class EditCriminalCommandHandler : IRequestHandler<EditCriminalCommand, Result<EditCriminalCommand>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICriminalRepository _criminalRepository;
        private readonly ICriminalImageRepository _criminalImageRepository;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        public EditCriminalCommandHandler(IUnitOfWork<long> unitOfWork, ICriminalRepository criminalRepository,
            ICriminalImageRepository criminalImageRepository, IMapper mapper, IUploadService uploadService) {
            _criminalImageRepository = criminalImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _criminalRepository = criminalRepository;
            _uploadService = uploadService;
        }

        public async Task<Result<EditCriminalCommand>> Handle(EditCriminalCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == 0)
            {
                return await Result<EditCriminalCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var editCriminal = await _criminalRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (editCriminal == null) return await Result<EditCriminalCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            _mapper.Map(request, editCriminal);
            await _criminalRepository.UpdateAsync(editCriminal);
            if(request.CriminalImages != null)
            {
                List<string?> listNewFile = request.CriminalImages.Select(_ => _.FilePath).ToList();
                var requestImages = await _criminalImageRepository.Entities.Where(_ => _.CriminalId == editCriminal.Id).ToListAsync(cancellationToken);
                if (requestImages.Any())
                {
                    foreach(var image in requestImages)
                    {
                        if(!listNewFile.Contains(image.FilePath)) await _uploadService.DeleteAsync(image.FilePath);
                    }
                    await _criminalImageRepository.RemoveRangeAsync(requestImages);
                    await _unitOfWork.Commit(cancellationToken);
                }
                var images = _mapper.Map<List<CriminalImage>>(request.CriminalImages);
                var requestImage = images.Select(x =>
                {
                    x.Id = 0;
                    x.CriminalId = editCriminal.Id;
                    return x;
                }).ToList();
                await _criminalImageRepository.AddRangeAsync(requestImage);
            }
            return await Result<EditCriminalCommand>.SuccessAsync(request);
        }
    }
}
