using Application.Dtos.Requests.Identity;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);
        Task<bool> ResetPasswordByAdmin(AppUser user, string newPassword);
        Task<bool> AddUser(AppUser user, string password, string role);
        Task<IResult> EditUser(EditUserRequest request);
        Task<IResult> DeleteUser(DeleteUserRequest request);
        Task<bool> IsExistUsername(string username);
        Task<Role> GetRoleIdAsync(long userId);
        Task<bool> ChangeRole(long userId, Role role);
        string GenerateRandomPassword(PasswordOptions? opts = null);
    }
}