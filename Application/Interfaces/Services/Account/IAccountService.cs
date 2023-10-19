using Application.Dtos.Requests.Account;
using Domain.Entities;
using Domain.Wrappers;

namespace Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);
        Task<bool> IsExistUsername(string username);
        Task<bool> AddAcount(AppUser employee, string password, string role);
        Task<string> GetRoleAsync(string userId);
        Task<int> GetRoleIdAsync(string role);
    }
}