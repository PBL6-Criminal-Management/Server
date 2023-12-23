using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Entities;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.Queries.GetByToken
{
    public class GetProfileByTokenQuery : IRequest<Result<GetProfileByTokenResponse>>
    {

    }
    internal class GetProfileByTokenQueryHandler : IRequestHandler<GetProfileByTokenQuery, Result<GetProfileByTokenResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadService _uploadService;
        public GetProfileByTokenQueryHandler(IAccountRepository accountRepository, IUserService userService, UserManager<AppUser> userManager, ICurrentUserService currentUserService, IUploadService uploadService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _userManager = userManager;
            _uploadService = uploadService;
            _currentUserService = currentUserService;
        }
        public async Task<Result<GetProfileByTokenResponse>> Handle(GetProfileByTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(_ => !string.IsNullOrEmpty(_.UserName) && _.UserName.Equals(_currentUserService.Username) && !_.IsDeleted).FirstOrDefaultAsync();
            if (user == null)
            {
                return await Result<GetProfileByTokenResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var account = await _accountRepository.Entities.Where(_ => _.Id == user.UserId && !_.IsDeleted)
                             .Select(_ => new GetProfileByTokenResponse
                             {
                                 Name = _.Name,
                                 CitizenId = _.CitizenId,
                                 Gender = _.Gender,
                                 Birthday = _.Birthday,
                                 PhoneNumber = _.PhoneNumber,
                                 Address = _.Address,
                                 Email = _.Email,
                                 Image = _.Image,
                                 ImageLink = _uploadService.GetFullUrl(_.Image),
                             }).FirstOrDefaultAsync();
            if (account == null)
                return await Result<GetProfileByTokenResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            return await Result<GetProfileByTokenResponse>.SuccessAsync(account);
        }
    }
}