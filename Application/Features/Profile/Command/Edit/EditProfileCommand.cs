using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.Command.Edit
{
    public class EditProfileCommand : IRequest<Result<EditProfileCommand>>
    {
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        public string Name { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.CITIZEN_ID_VALID_CHARACTER)]
        public string CitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.ADDRESS_VALID_CHARACTER)]
        public string Address { get; set; } = null!;

        [EmailAddress(ErrorMessage = StaticVariable.INVALID_EMAIL)]
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_EMAIL)]
        public string Email { get; set; } = null!;
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;

        public bool? Gender { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_IMAGE)]
        public string? Image { get; set; }
    }
    internal class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<EditProfileCommand>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        public EditProfileCommandHandler(IAccountRepository accountRepository, IUnitOfWork<long> unitOfWork,
            IMapper mapper, IUploadService uploadService, ICurrentUserService currentUserService,
            UserManager<AppUser> userManager, IUserService userService)
        {
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _uploadService = uploadService;
            _userManager = userManager;
            _userService = userService;
        }
        public async Task<Result<EditProfileCommand>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.Where(_ => _.UserName == _currentUserService.Username && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                return await Result<EditProfileCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var editAccount = await _accountRepository.FindAsync(_ => _.Id == user.UserId && !_.IsDeleted);
            if (editAccount == null)
            {
                return await Result<EditProfileCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var isCitizenIdExists = _accountRepository.Entities.FirstOrDefault(x => x.CitizenId == request.CitizenId && !x.IsDeleted && x.Id != user.UserId);
            if (isCitizenIdExists != null)
            {
                return await Result<EditProfileCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }

            var isEmailExists = _accountRepository.Entities.FirstOrDefault(x => x.Email == request.Email && !x.IsDeleted && x.Id != user.UserId);
            if (isEmailExists != null)
            {
                return await Result<EditProfileCommand>.FailAsync(StaticVariable.EMAIL_EXISTS_MSG);
            }

            var isPhoneNumberExists = _accountRepository.Entities.Any(x => x.PhoneNumber.Equals(request.PhoneNumber) && !x.IsDeleted && x.Id != user.UserId);
            if (isPhoneNumberExists)
            {
                return await Result<EditProfileCommand>.FailAsync(StaticVariable.PHONE_NUMBER_EXISTS_MSG);
            }
            string deleteImagePath = "";
            if (editAccount.Image != null && editAccount.Image != request.Image)
            {
                deleteImagePath = editAccount.Image;
            }
            _mapper.Map(request, editAccount);
            var executionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {

                    await _accountRepository.UpdateAsync(editAccount);
                    await _unitOfWork.Commit(cancellationToken);
                    await _userService.EditUser(new Dtos.Requests.Identity.EditUserRequest
                    {
                        Id = editAccount.Id,
                        FullName = editAccount.Name,
                        Email = editAccount.Email,
                        Phone = editAccount.PhoneNumber,
                        ImageFile = editAccount.Image,
                        IsActive = true
                    });

                    await transaction.CommitAsync(cancellationToken);
                    if (!deleteImagePath.Equals(""))
                    {
                        await _uploadService.DeleteAsync(deleteImagePath);
                    }
                    return await Result<EditProfileCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<EditProfileCommand>.FailAsync(ex.Message);
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