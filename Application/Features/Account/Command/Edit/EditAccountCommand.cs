using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Account.Command.Edit
{
    public class EditAccountCommand : IRequest<Result<EditAccountCommand>>
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        public string CitizenId { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        public string Address { get; set; } = null!;

        [EmailAddress(ErrorMessage = StaticVariable.INVALID_EMAIL)]
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_EMAIL)]
        public string Email { get; set; } = null!;
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;

        public Role? Role { get; set; }

        public bool? Gender { get; set; }

        public bool IsActive { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_IMAGE)]
        public string? Image { get; set; }

    }
    internal class EditAccountCommandHandler : IRequestHandler<EditAccountCommand, Result<EditAccountCommand>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;

        public EditAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork<long> unitOfWork,
            IMapper mapper, IUploadService uploadService, IUserService userService, ICurrentUserService currentUserService, UserManager<AppUser> userManager)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _userService = userService;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<EditAccountCommand>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindAsync(a => a.Id == request.Id);
            if (account == null)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }

            var user = _userManager.Users.Where(e => e.UserId == request.Id && !e.IsDeleted).FirstOrDefault();

            var roleId = await _userService.GetRoleIdAsync(account.Id);
            if (!_currentUserService.RoleName.Equals(RoleConstants.AdministratorRole))
            {
                if(user == null || !_currentUserService.Username.Equals(user.UserName))
                    return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_EDIT_ACCOUNT_PERMISSION);

                if (roleId != request.Role)
                    return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_EDIT_ROLE_PERMISSION);

                if (user.IsActive != request.IsActive)
                    return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_EDIT_IS_ACTIVE_PERMISSION);
            }


            var isCitizenIdExists = _accountRepository.Entities.FirstOrDefault(x => x.CitizenId == request.CitizenId && !x.IsDeleted && x.Id != request.Id);
            if (isCitizenIdExists != null)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }

            var isEmailExists = _accountRepository.Entities.FirstOrDefault(x => x.Email == request.Email && !x.IsDeleted && x.Id != request.Id);
            if (isEmailExists != null)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.EMAIL_EXISTS_MSG);
            }

            var isPhoneNumberExists = _accountRepository.Entities.Any(x => x.PhoneNumber.Equals(request.PhoneNumber) && !x.IsDeleted && x.Id != request.Id);
            if (isPhoneNumberExists)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.PHONE_NUMBER_EXISTS_MSG);
            }
            //if (!Enum.IsDefined(typeof(Role), request.Role))
            //{
            //    return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_FOUND_ROLE);
            //}
            string deleleImagePath = "";
            if (account.Image != null && account.Image != request.Image)
            {
                deleleImagePath = account.Image;
            }
            _mapper.Map(request, account);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    string? message = null;

                    if (user != null)
                    {
                        var checkChangeRole = await _userService.ChangeRole(request.Id, request.Role == null? Role.None : (Role)request.Role);
                        if (!checkChangeRole)
                        {
                            return await Result<EditAccountCommand>.FailAsync(StaticVariable.CHANGE_ROLE_FAIL);
                        }
                    }
                    else
                        message = StaticVariable.USER_HAVE_NOT_ROLE;

                    await _accountRepository.UpdateAsync(account);
                    await _unitOfWork.Commit(cancellationToken);
                    await _userService.EditUser(new Dtos.Requests.Identity.EditUserRequest
                    {
                        Id = request.Id,
                        FullName = request.Name,
                        Email = request.Email,
                        Phone = request.PhoneNumber,
                        ImageFile = request.Image,
                        IsActive = request.IsActive
                    });

                    await transaction.CommitAsync(cancellationToken);
                    if (!deleleImagePath.Equals(""))
                    {
                        await _uploadService.DeleteAsync(deleleImagePath);
                    }
                    
                    if(message != null)
                    {
                        request.IsActive = false;
                        request.Role = null;
                        return await Result<EditAccountCommand>.SuccessAsync(request, message);
                    }
                    else
                        return await Result<EditAccountCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<EditAccountCommand>.FailAsync(ex.Message);
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
