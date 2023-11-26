using Application.Dtos.Requests.Identity;
using Application.Dtos.Requests.SendEmail;
using Application.Exceptions;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using IResult = Domain.Wrappers.IResult;

namespace Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _mailService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public UserService(UserManager<AppUser> userManager, IEmailService mailService, IBackgroundJobClient backgroundJobClient)
        {
            _userManager = userManager;
            _mailService = mailService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new ApiException(StaticVariable.NOT_FOUND_EMAIL);
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //send mail
            var route = "auth/reset-password";
            var endpointUri = new Uri(string.Concat($"{request.UrlFE}/", route));
            //var filePath = _environment.WebRootPath + "\\templates\\forgot-password-sender.html";
            //var str = new StreamReader(filePath);
            //var mailText = await str.ReadToEndAsync();
            //str.Close();
            var passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);

            //var logo = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.Value}/logo/logo-nineplus.png";
            //var mailBody = mailText.Replace("[EndPointUrl]", HtmlEncoder.Default.Encode(passwordResetUrl)).Replace("[Logo]", logo);
            var mailRequest = new EmailRequest()
            {
                Body = "Kích vào link sau để thiết lập lại mật khẩu của bạn:\n" + passwordResetUrl,
                Subject = "Thiết lập lại mật khẩu",
                To = request.Email
            };
            _backgroundJobClient.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result.SuccessAsync(StaticVariable.SEND_EMAIL_SUCCESSFULLY);
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request.Password.Length < 8) return await Result.FailAsync(StaticVariable.PASSWORD_TOO_SHORT);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(StaticVariable.NOT_FOUND_EMAIL);
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(StaticVariable.CHANGE_PASSWORD_SUCCESSFULLY);
            }
            return await Result.FailAsync(StaticVariable.SYS_ERROR);
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string username)
        {
            var user = await this._userManager.FindByNameAsync(username);
            if (user == null)
            {
                return await Result.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }

            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
            {
                return await Result.FailAsync(StaticVariable.NOT_MATCH_PASSWORD);
            }

            var identityResult = await this._userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            string msg = string.Join("\n", identityResult.Errors.Select(e => MapErrorMessage(e.Code)));

            return identityResult.Succeeded ? await Result.SuccessAsync(StaticVariable.CHANGE_PASSWORD_SUCCESSFULLY) : await Result.FailAsync(msg);
        }

        private string MapErrorMessage(string errorCode)
        {
            switch (errorCode)
            {
                case "PasswordTooShort":
                    return StaticVariable.PASSWORD_TOO_SHORT;
                case "PasswordMismatch":
                    return StaticVariable.INCORRECT_PASSWORD;
                case "PasswordRequiresDigit":
                    return StaticVariable.PASSWORD_REQUIRE_DIGIT;
                case "PasswordRequiresLower":
                    return StaticVariable.PASSWORD_REQUIRE_LOWER;
                case "PasswordRequiresNonAlphanumeric":
                    return StaticVariable.PASSWORD_REQUIRE_NON_ALPHANUMERIC;
                case "PasswordRequiresUpper":
                    return StaticVariable.PASSWORD_REQUIRE_UPPER;
                case "DuplicateEmail":
                    return StaticVariable.EMAIL_EXISTS_MSG;
                
                default:
                    return $"Thay đổi mật khẩu thất bại với lỗi: {errorCode}";
            }
        }

        public async Task<bool> AddUser(AppUser user, string password, string role)
        {
            await _userManager.CreateAsync(user, password);
            var result = await _userManager.AddToRoleAsync(user, role);
            string msg = string.Join("\n", result.Errors.Select(e => MapErrorMessage(e.Code)));
            if(msg != "")
                throw new ApiException(msg);

            return result.Succeeded;
        }

        public async Task<IResult> EditUser(EditUserRequest request)
        {
            var user = _userManager.Users.Where(user => user.UserId == request.Id).FirstOrDefault();
            if (user == null)
            {
                return await Result.FailAsync(StaticVariable.SYS_ERROR);
            }
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.PhoneNumber = request.Phone;
            user.AvatarUrl = request.ImageFile;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync();
            }
            return await Result.FailAsync(StaticVariable.SYS_ERROR);
        }

        public async Task<IResult> DeleteUser(DeleteUserRequest request)
        {
            var user = _userManager.Users.Where(user => user.UserId == request.Id).FirstOrDefault();
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(StaticVariable.SYS_ERROR);
            }

            user.IsActive = false;
            user.IsDeleted = true;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync();
            }
            return await Result.FailAsync(StaticVariable.SYS_ERROR);
        }

        public async Task<bool> IsExistUsername(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }

        public async Task<Role> GetRoleIdAsync(long userId)
        {
            var user = _userManager.Users.Where(user => user.UserId == userId).FirstOrDefault();

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null && roles.Count > 0)
                    return (Role)Enum.Parse(typeof(Role), roles.First());
            }
            return Role.None;
        }

        public async Task<bool> ChangeRole(long userId, Role role)
        {
            var user = _userManager.Users.Where(user => user.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null && roles.Count > 0)
                {
                    if ((Role)Enum.Parse(typeof(Role), roles.First()) == role)
                    {
                        return true;
                    }
                    var result = await _userManager.RemoveFromRoleAsync(user, roles.First());
                    if (result.Succeeded)
                    {
                        var check = await _userManager.AddToRoleAsync(user, role.ToDescriptionString());
                        return check.Succeeded;
                    }
                }
                else
                {
                    var check = await _userManager.AddToRoleAsync(user, role.ToDescriptionString());
                    return check.Succeeded;
                }
            }
            return false;
        }
    }
}