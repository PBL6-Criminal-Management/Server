using Application.Dtos.Requests.Image;
using Application.Interfaces;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities.CriminalImage;
using Domain.Wrappers;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Criminal.Command.Add
{
    public class AddCriminalCommand : IRequest<Result<AddCriminalCommand>>
    {
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; } = null!;
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.ANOTHER_NAME_CONTAINS_VALID_CHARACTER)]
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ANOTHER_NAME)]
        public string AnotherName { get; set; } = null!;
        public string? Avatar { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.CITIZEN_ID_VALID_CHARACTER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        public string CitizenId { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_PHONE_MODEL)]
        [RegularExpression(@"^[\p{L}0-9 ]+$", ErrorMessage = StaticVariable.PHONE_MODE_VALID_CHARACTER)]
        public string PhoneModel { get; set; } = null!;
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_CAREER_AND_WORKPLACE)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.CAREER_AND_WORKPLACE_VALID_CHARACTER)]
        public string CareerAndWorkplace { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_CHARACTERISTICS)]
        [RegularExpression(@"^[\p{L}, ]+$", ErrorMessage = StaticVariable.CHARACTERISTICS_VALID_CHARACTER)]
        public string Characteristics { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_HOME_TOWN)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.HOME_TOWN_VALID_CHARACTER)]
        public string HomeTown { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_ETHNICITY)]
        [RegularExpression(@"^[\p{L}]+$", ErrorMessage = StaticVariable.ETHNICITY_VALID_CHARACTER)]
        public string Ethnicity { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_RELIGION)]
        [RegularExpression(@"^[\p{L}]+$", ErrorMessage = StaticVariable.RELIGION_VALID_CHARACTER)]
        public string? Religion { get; set; }
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_NATIONALITY)]
        [RegularExpression(@"^[\p{L}]+$", ErrorMessage = StaticVariable.NATIONALITY_VALID_CHARACTER)]
        public string Nationality { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FATHER_NAME)]
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        public string FatherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_FATHER_CITIZEN_ID)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.CITIZEN_ID_VALID_CHARACTER)]
        public string FatherCitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public DateOnly FatherBirthday { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MOTHER_NAME)]
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        public string MotherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_MOTHER_CITIZEN_ID)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.CITIZEN_ID_VALID_CHARACTER)]
        public string MotherCitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly MotherBirthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_PERMANENT_RESIDENCE)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.PERMANENT_RESIDENCE_VALID_CHARACTER)]
        public string PermanentResidence { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CURRENT_ACCOMMODATION)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.CURRENT_ACCOMMODATION_VALID_CHARACTER)]
        public string CurrentAccommodation { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_ENTRY_AND_EXIT_INFORMATION)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.ENTRY_AND_EXIT_INFORMATION_VALID_CHARACTER)]
        public string? EntryAndExitInformation { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FACEBOOK)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.FACEBOOK_VALID_CHARACTER)]
        public string? Facebook { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ZALO)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.ZALO_VALID_CHARACTER)]
        public string? Zalo { get; set; }
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_OTHER_SOCIAL_NETWORKS)]
        public string? OtherSocialNetworks { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_GAME_ACCOUNT)]
        public string? GameAccount { get; set; }
        [MaxLength(30, ErrorMessage = StaticVariable.LIMIT_BANK_ACCOUNT)]
        [RegularExpression(@"^[\p{L}0-9 ]+$", ErrorMessage = StaticVariable.BANK_ACCOUNT_VALID_CHARACTER)]
        public string? BankAccount { get; set; }
        public string? Research { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_VEHICLES)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.VEHICLES_VALID_CHARACTER)]
        public string? Vehicles { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_DANGEROUS_LEVEL)]
        [RegularExpression(@"^[\p{L}0-9,.: ]+$", ErrorMessage = StaticVariable.DANGEROUS_LEVEL_VALID_CHARACTER)]
        public string? DangerousLevel { get; set; }
        public string? ApproachArrange { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? ReleaseDate { get; set; }
        public CriminalStatus Status { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_OTHER_INFORMATION)]
        public string? OtherInformation { get; set; }
        public List<ImageRequest>? CriminalImages { get; set; }
    }
    internal class AddCriminalCommandHandler : IRequestHandler<AddCriminalCommand, Result<AddCriminalCommand>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICriminalImageRepository _criminalImageRepository;
        private readonly IUploadService _uploadService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public AddCriminalCommandHandler(
            ICriminalRepository criminalRepository,
            IUnitOfWork<long> unitOfWork,
            IMapper mapper,
            ICriminalImageRepository criminalImageRepository,
            IUploadService uploadService,
            IBackgroundJobClient backgroundJobClient
        )
        {
            _criminalRepository = criminalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Result<AddCriminalCommand>> Handle(AddCriminalCommand request, CancellationToken cancellationToken)
        {
            var isCitizenIdExists = await _criminalRepository.FindAsync(_ => _.CitizenId.Equals(request.CitizenId));
            if (isCitizenIdExists != null)
            {
                return await Result<AddCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }
            var isPhoneNumberExists = await _criminalRepository.FindAsync(_ => _.PhoneNumber.Equals(request.PhoneNumber) && !_.IsDeleted);
            if (isPhoneNumberExists != null)
            {
                return await Result<AddCriminalCommand>.FailAsync(StaticVariable.PHONE_NUMBER_EXISTS_MSG);
            }
            if (!string.IsNullOrEmpty(request.FatherCitizenId))
            {
                if (request.FatherCitizenId.Equals(request.CitizenId) || request.FatherCitizenId.Equals(request.MotherCitizenId))
                {
                    return await Result<AddCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_REPEAT);
                }
            }
            if (!string.IsNullOrEmpty(request.MotherCitizenId))
            {
                if (request.MotherCitizenId.Equals(request.CitizenId) || request.MotherCitizenId.Equals(request.FatherCitizenId))
                {
                    return await Result<AddCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_REPEAT);
                }
            }
            if (!string.IsNullOrEmpty(request.Facebook))
            {
                var isFacebookExists = await _criminalRepository.FindAsync(_ => !string.IsNullOrEmpty(_.Facebook) && _.Facebook.Equals(request.Facebook) && !_.IsDeleted);
                if (isFacebookExists != null)
                {
                    return await Result<AddCriminalCommand>.FailAsync(StaticVariable.FACEBOOK_EXISTS_MSG);
                }
            }
            var addCriminal = _mapper.Map<Domain.Entities.Criminal.Criminal>(request);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _criminalRepository.AddAsync(addCriminal);
                    await _unitOfWork.Commit(cancellationToken);

                    if (request.CriminalImages != null)
                    {
                        var addImage = _mapper.Map<List<CriminalImage>>(request.CriminalImages);
                        addImage.ForEach(x => x.CriminalId = addCriminal.Id);
                        await _criminalImageRepository.AddRangeAsync(addImage);
                        await _unitOfWork.Commit(cancellationToken);

                        var uploadList = request.CriminalImages.Select(image => image.FilePath).ToList();
                        if (request.Avatar != null) uploadList.Add(request.Avatar);
                        _backgroundJobClient.Enqueue(() => _uploadService.UploadToGGDrive(addCriminal.Id, uploadList));
                    }

                    await transaction.CommitAsync(cancellationToken);
                    return await Result<AddCriminalCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<AddCriminalCommand>.FailAsync(ex.Message);
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });

            return result;
        }
    }
}
