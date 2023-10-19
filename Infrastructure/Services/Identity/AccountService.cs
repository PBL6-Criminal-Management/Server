using Application.Dtos.Requests.Account;
using Application.Interfaces.Services.Account;
using Domain.Entities;
using Domain.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userName)
        {
            var user = await this._userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return await Result.FailAsync("Không tìm thấy người dùng");
            }

            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
            {
                return await Result.FailAsync("Mật khẩu không khớp.");
            }

            var identityResult = await this._userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync("Đổi mật khẩu không thành công");
        }

        public async Task<bool> IsExistUsername(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }
        public async Task<bool> AddAcount(AppUser user, string password, string role)
        {
            await _userManager.CreateAsync(user, password);
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
        public async Task<string> GetRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if(roles != null && roles.Count > 0)
                    return roles.First();
            }
            return null;
        }
        public async Task<int> GetRoleIdAsync(string role)
        {
            if (string.IsNullOrEmpty(role)) return -1;
            var roleId = (await _roleManager.FindByNameAsync(role))?.Id;
            if (string.IsNullOrEmpty(roleId)) return -1;
            return int.Parse(roleId);
        }
    }
}