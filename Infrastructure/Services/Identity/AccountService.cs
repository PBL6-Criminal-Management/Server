﻿using Application.Dtos.Requests.Account;
using Application.Exceptions;
using Application.Interfaces.Services.Account;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
        public async Task<Role> GetRoleIdAsync(long userId)
        {
            var user = _userManager.Users.Where(user => user.UserId == userId).FirstOrDefault();

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null && roles.Count > 0)
                    return (Role)Enum.Parse(typeof(Role),roles.First());
            }
            return Role.None;
        }
        public async Task<bool> ChangeRole(long userId, Role role)
        {
            var user = _userManager.Users.Where(user =>user.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null && roles.Count > 0)
                {
                    if ((Role)Enum.Parse(typeof(Role), roles.First()) == role) {
                        return true;
                    }
                    var result = await _userManager.RemoveFromRoleAsync(user, roles.First());
                    if(result.Succeeded)
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