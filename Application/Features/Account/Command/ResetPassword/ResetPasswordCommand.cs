using Application.Dtos.Requests.SendEmail;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Entities;
using Domain.Wrappers;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Account.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Result<bool>>
    {
        public long? Id { get; set; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailService _mailService;

        public ResetPasswordCommandHandler(
            UserManager<AppUser> userManager,
            IUserService userService, 
            IBackgroundJobClient backgroundJobClient, 
            IEmailService mailService)
        {
            _userManager = userManager;
            _userService = userService;
            _backgroundJobClient = backgroundJobClient;
            _mailService = mailService;
        }

        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.Where(e => e.UserId == request.Id && !e.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                return await Result<bool>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }

            if(user.Email == null)
                return await Result<bool>.FailAsync(StaticVariable.USER_HAVE_NOT_EMAIL);

            try
            {
                var newPwd = _userService.GenerateRandomPassword();
                bool resetResult = _userService.ResetPasswordByAdmin(user, newPwd).Result;
                if (resetResult == false)
                {
                    return await Result<bool>.FailAsync(StaticVariable.UNKNOWN_ERROR);
                }
                var mailRequest = new EmailRequest()
                {
                    Body = $"Mật khẩu cho tài khoản của bạn đã được quản trị viên thiết lập lại:<br>" +
                    $"Mật khẩu mới: {newPwd}<br>" +
                    $"Hãy đăng nhập vào hệ thống và đổi mật khẩu ngay để tránh bị lộ thông tin cá nhân.<br>" +
                    $"Liên hệ với người quản trị nếu bạn gặp bất kì vấn đề gì khi đăng nhập vào hệ thống!<br>",
                    Subject = "Thiết lập lại mật khẩu",
                    To = user.Email
                };
                _backgroundJobClient.Enqueue(() => _mailService.SendAsync(mailRequest));

                return await Result<bool>.SuccessAsync(StaticVariable.RESET_PASSWORD_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailAsync(ex.Message);
            }
        }
    }
}
