
using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Account;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
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
        public string CitizenID { get; set; } = null!;
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

        public Role Role { get; set; }

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
        private readonly IAccountService _accountService;

        public EditAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork<long> unitOfWork,
            IMapper mapper, IUploadService uploadService, IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _accountService = accountService;
        }

        public async Task<Result<EditAccountCommand>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindAsync(a => a.Id == request.Id);
            if (account == null)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var isCitizenIDExists = _accountRepository.Entities.FirstOrDefault(x => x.CitizenID == request.CitizenID && !x.IsDeleted && x.Id != request.Id);
            if (isCitizenIDExists != null)
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
            if (!Enum.IsDefined(typeof(Role), request.Role))
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.NOT_FOUND_ROLE);
            }
            string deleleImagePath = "";
            if (account.Image != null && account.Image != request.Image)
            {
                deleleImagePath = account.Image;
            }
            _mapper.Map(request, account);
            var checkChangeRole = await _accountService.ChangeRole(request.Id, request.Role);
            if (!checkChangeRole)
            {
                return await Result<EditAccountCommand>.FailAsync(StaticVariable.CHANGE_ROLE_FAIL);
            }
            await _accountRepository.UpdateAsync(account);
            await _unitOfWork.Commit(cancellationToken);
            if (!deleleImagePath.Equals(""))
            {
                await _uploadService.DeleteAsync(deleleImagePath);
            }
            return await Result<EditAccountCommand>.SuccessAsync(request);
        }
    }
}
