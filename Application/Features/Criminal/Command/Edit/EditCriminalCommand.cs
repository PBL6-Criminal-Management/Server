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
using Hangfire;

namespace Application.Features.Criminal.Command.Edit
{
    public class EditCriminalCommand : IRequest<Result<EditCriminalCommand>>
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Name { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ANOTHER_NAME)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string AnotherName { get; set; } = null!;
        public string? Avatar { get; set; }
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string CitizenId { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_PHONE_MODEL)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string PhoneModel { get; set; } = null!;
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_CAREER_AND_WORKPLACE)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string CareerAndWorkplace { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_CHARACTERISTICS)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Characteristics { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_HOME_TOWN)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string HomeTown { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_ETHNICITY)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Ethnicity { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_RELIGION)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Religion { get; set; }
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_NATIONALITY)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Nationality { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FATHER_NAME)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string FatherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_FATHER_CITIZEN_ID)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string FatherCitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly FatherBirthday { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MOTHER_NAME)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string MotherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_MOTHER_CITIZEN_ID)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string MotherCitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly MotherBirthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_PERMANENT_RESIDENCE)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string PermanentResidence { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CURRENT_ACCOMMODATION)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string CurrentAccommodation { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_ENTRY_AND_EXIT_INFORMATION)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? EntryAndExitInformation { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FACEBOOK)]
        public string? Facebook { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ZALO)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Zalo { get; set; }
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_OTHER_SOCIAL_NETWORKS)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? OtherSocialNetworks { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_GAME_ACCOUNT)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? GameAccount { get; set; }
        [MaxLength(30, ErrorMessage = StaticVariable.LIMIT_BANK_ACCOUNT)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? BankAccount { get; set; }
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Research { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_VEHICLES)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Vehicles { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_DANGEROUS_LEVEL)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? DangerousLevel { get; set; }
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? ApproachArrange { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? ReleaseDate { get; set; }
        public CriminalStatus Status { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_OTHER_INFORMATION)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public EditCriminalCommandHandler(
            IUnitOfWork<long> unitOfWork,
            ICriminalRepository criminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IMapper mapper,
            IUploadService uploadService,
            IBackgroundJobClient backgroundJobClient
        )
        {
            _criminalImageRepository = criminalImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _criminalRepository = criminalRepository;
            _uploadService = uploadService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Result<EditCriminalCommand>> Handle(EditCriminalCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return await Result<EditCriminalCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var isCitizenIdExists = await _criminalRepository.FindAsync(_ => _.CitizenId.Equals(request.CitizenId));
            if (isCitizenIdExists != null)
            {
                return await Result<EditCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }
            var isPhoneNumberExists = await _criminalRepository.FindAsync(_ => _.PhoneNumber.Equals(request.PhoneNumber) && !_.IsDeleted);
            if (isPhoneNumberExists != null)
            {
                return await Result<EditCriminalCommand>.FailAsync(StaticVariable.PHONE_NUMBER_EXISTS_MSG);
            }
            if (!string.IsNullOrEmpty(request.FatherCitizenId))
            {
                if (request.FatherCitizenId.Equals(request.CitizenId) || request.FatherCitizenId.Equals(request.MotherCitizenId))
                {
                    return await Result<EditCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_REPEAT);
                }
            }
            if (!string.IsNullOrEmpty(request.MotherCitizenId))
            {
                if (request.MotherCitizenId.Equals(request.CitizenId) || request.MotherCitizenId.Equals(request.FatherCitizenId))
                {
                    return await Result<EditCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_REPEAT);
                }
            }
            if (!string.IsNullOrEmpty(request.Facebook))
            {
                var isFacebookExists = await _criminalRepository.FindAsync(_ => !string.IsNullOrEmpty(_.Facebook) && _.Facebook.Equals(request.Facebook) && !_.IsDeleted);
                if (isFacebookExists != null)
                {
                    return await Result<EditCriminalCommand>.FailAsync(StaticVariable.FACEBOOK_EXISTS_MSG);
                }
            }
            var editCriminal = await _criminalRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (editCriminal == null) return await Result<EditCriminalCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var oldAvatar = editCriminal.Avatar;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    _mapper.Map(request, editCriminal);
                    await _criminalRepository.UpdateAsync(editCriminal);

                    await _unitOfWork.Commit(cancellationToken);

                    var imagesInDB = await _criminalImageRepository.Entities.Where(_ => _.CriminalId == editCriminal.Id).ToListAsync(cancellationToken);

                    if (request.CriminalImages != null && request.CriminalImages.Any())
                    {
                        var images = _mapper.Map<List<CriminalImage>>(request.CriminalImages);
                        var requestImage = images.Select(x =>
                        {
                            x.Id = 0;
                            x.CriminalId = editCriminal.Id;
                            return x;
                        }).ToList();
                        await _criminalImageRepository.AddRangeAsync(requestImage);
                        await _unitOfWork.Commit(cancellationToken);

                        var listA = request.CriminalImages.Select(_ => _.FilePath);
                        var listB = imagesInDB.Select(_ => _.FilePath);

                        if (imagesInDB.Any())
                        {
                            await _criminalImageRepository.RemoveRangeAsync(imagesInDB);  //remove all images in db
                            await _unitOfWork.Commit(cancellationToken);

                            var listRemoveImages = listB.Except(listA).ToList();

                            //Remove images in server not exist in request
                            if (listRemoveImages.Any())
                            {
                                await _uploadService.DeleteRangeAsync(listRemoveImages);
                                _backgroundJobClient.Enqueue(() => _uploadService.RemoveImageFromGGDrive(editCriminal.Id, listRemoveImages, false));
                            }
                        }

                        var listUploadImagesToDrive = listA.Except(listB).ToList();
                        _backgroundJobClient.Enqueue(() => _uploadService.UploadToGGDrive(editCriminal.Id, listUploadImagesToDrive));
                    }
                    else
                    {
                        if (imagesInDB.Any())
                        {
                            await _criminalImageRepository.RemoveRangeAsync(imagesInDB);
                            await _unitOfWork.Commit(cancellationToken);
                            await _uploadService.DeleteRangeAsync(imagesInDB.Select(i => i.FilePath).ToList());
                            _backgroundJobClient.Enqueue(() => _uploadService.RemoveImageFromGGDrive(editCriminal.Id, imagesInDB.Select(i => i.FilePath).ToList(), false));
                        }
                    }

                    if (oldAvatar != null)
                    {
                        if (editCriminal.Avatar != null)
                        {
                            if (!editCriminal.Avatar.Equals(oldAvatar))
                            {
                                await _uploadService.DeleteAsync(oldAvatar);
                                _backgroundJobClient.Enqueue(() => _uploadService.RemoveImageFromGGDrive(editCriminal.Id, new List<string> { oldAvatar }, false));
                                _backgroundJobClient.Enqueue(() => _uploadService.UploadToGGDrive(editCriminal.Id, new List<string> { editCriminal.Avatar }));
                            }
                        }
                        else
                        {
                            await _uploadService.DeleteAsync(oldAvatar);
                            _backgroundJobClient.Enqueue(() => _uploadService.RemoveImageFromGGDrive(editCriminal.Id, new List<string> { oldAvatar }, false));
                        }
                    }
                    else
                        if (editCriminal.Avatar != null)
                        _backgroundJobClient.Enqueue(() => _uploadService.UploadToGGDrive(editCriminal.Id, new List<string> { editCriminal.Avatar }));

                    await transaction.CommitAsync(cancellationToken);
                    return await Result<EditCriminalCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<EditCriminalCommand>.FailAsync(ex.Message);
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
