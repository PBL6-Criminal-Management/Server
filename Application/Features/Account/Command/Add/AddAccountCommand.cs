using Application.Dtos.Requests.SendEmail;
using Application.Exceptions;
using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Helpers;
using Domain.Wrappers;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        public string CitizenId { get; set; } = null!;

        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = StaticVariable.INVALID_USER_NAME)]
        [DefaultValue(null)]
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_USERNAME)]
        public string? Username { get; set; }

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
        private readonly IUserService _userService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailService _mailService;

        public AddAccountCommandHandler(IMapper mapper, IAccountRepository AccountRepository, IUnitOfWork<long> unitOfWork, IUserService userService, IBackgroundJobClient backgroundJobClient, IEmailService mailService)
        {
            _mapper = mapper;
            _accountRepository = AccountRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _backgroundJobClient = backgroundJobClient;
            _mailService = mailService;
        }

        public async Task<Result<AddAccountCommand>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            request.Id = null;

            string[] wordsInName = StringHelper.ConvertFromVietnameseText(request.Name).Split(' ');

            request.Username = wordsInName.Last() + string.Join(string.Empty, wordsInName.Take(wordsInName.Length - 1).Select(w => w[0]));
            long count = 2; string name = request.Username;
            while (true)
            {
                var isUsernameExists = await _userService.IsExistUsername(request.Username);
                if (isUsernameExists)
                {
                    request.Username = name + count;
                    count++;
                }
                else
                    break;
            }

            var isCitizenIdExists = _accountRepository.Entities.FirstOrDefault(x => x.CitizenId == request.CitizenId && !x.IsDeleted);
            if (isCitizenIdExists != null)
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

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var addAccount = _mapper.Map<Domain.Entities.User.User>(request);

                    await _accountRepository.AddAsync(addAccount);
                    await _unitOfWork.Commit(cancellationToken);
                    request.Id = addAccount.Id;

                    var user = _mapper.Map<AppUser>(request);

                    bool result = await _userService.AddUser(user, request.Password, request.Role.ToDescriptionString());
                    if (result == false)
                    {
                        return await Result<AddAccountCommand>.FailAsync(StaticVariable.UNKNOWN_ERROR);
                    }
                    var mailRequest = new EmailRequest()
                    {
                        Body = $"Tài khoản của bạn đã được tạo trên hệ thống:<br>" +
                        $"Tên đăng nhập: {request.Username}<br>" +
                        $"Mật khẩu: {request.Password}<br>" +
                        $"Hãy đăng nhập vào hệ thống và đổi mật khẩu ngay để tránh bị lộ thông tin cá nhân.<br>" +
                        $"Liên hệ với người quản trị nếu bạn gặp bất kì vấn đề gì khi đăng nhập vào hệ thống!<br>",
                        Subject = "Tài khoản được cấp",
                        To = request.Email
                    };
                    _backgroundJobClient.Enqueue(() => _mailService.SendAsync(mailRequest));

                    await transaction.CommitAsync(cancellationToken);
                    return await Result<AddAccountCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<AddAccountCommand>.FailAsync(ex.Message);
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
