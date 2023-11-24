using Application.Exceptions;
using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Account;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Account.Command.Add
{
    public class AddAccountCommand : IRequest<Result<AddAccountCommand>>
    {
        public long? Id { get; set; }

        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; } = null!;

        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        public string CitizenID { get; set; } = null!;

        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = StaticVariable.INVALID_USER_NAME)]
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_USERNAME)]
        [DefaultValue("string")]
        public string UserName { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()-_=+[\]{}|;:',.<>\/?~]{8,}$", ErrorMessage = StaticVariable.INVALID_PASSWORD)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = StaticVariable.LIMIT_PASSWORD)]
        [DefaultValue("stringst")]
        public string Password { get; set; } = null!;

        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        public string Address { get; set; } = null!;

        [EmailAddress(ErrorMessage = StaticVariable.INVALID_EMAIL)]
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_EMAIL)]
        public string Email { get; set; } = null!;

        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;

        public Role Role { get; set; }

        public bool? Gender { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_IMAGE)]
        public string? Image { get; set; }
    }

    internal class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, Result<AddAccountCommand>>
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IAccountService _accountService;

        public AddAccountCommandHandler(IMapper mapper, IAccountRepository AccountRepository, IUnitOfWork<long> unitOfWork, IAccountService accountService)
        {
            _mapper = mapper;
            _accountRepository = AccountRepository;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        public async Task<Result<AddAccountCommand>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            request.Id = null;

            var isUsernameExists = await _accountService.IsExistUsername(request.UserName);
            if (isUsernameExists)
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.USERNAME_EXISTS_MSG);
            }

            var isCitizenIDExists = _accountRepository.Entities.FirstOrDefault(x => x.CitizenID == request.CitizenID && !x.IsDeleted);
            if (isCitizenIDExists != null)
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }

            var isEmailExists = _accountRepository.Entities.FirstOrDefault(x => x.Email == request.Email && !x.IsDeleted);
            if (isEmailExists != null)
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.EMAIL_EXISTS_MSG);
            }

            var isPhoneNumberExists = _accountRepository.Entities.Any(x => x.PhoneNumber.Equals(request.PhoneNumber) && !x.IsDeleted);
            if (isPhoneNumberExists)
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.PHONE_NUMBER_EXISTS_MSG);
            }

            if(!Enum.IsDefined(typeof(Role), request.Role))
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.NOT_FOUND_ROLE);
            }

            var addAccount = _mapper.Map<Domain.Entities.User.User>(request);

            await _accountRepository.AddAsync(addAccount);
            await _unitOfWork.Commit(cancellationToken);
            request.Id = addAccount.Id;

            var user = _mapper.Map<AppUser>(request);
            
            bool result = await _accountService.AddAcount(user, request.Password, request.Role.ToDescriptionString());
            if (result == false)
            {
                return await Result<AddAccountCommand>.FailAsync(StaticVariable.UNKNOWN_ERROR);
            }

            return await Result<AddAccountCommand>.SuccessAsync(request);
        }
    }
}
