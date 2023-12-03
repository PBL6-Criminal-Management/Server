using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Entities;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Account.Queries.GetById
{
    public class GetAccountByIdQuery : IRequest<Result<GetAccountByIdResponse>>
    {
        public long Id { get; set; }
    }

    internal class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<GetAccountByIdResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadService _uploadService;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepository, IUserService userService, UserManager<AppUser> userManager, ICurrentUserService currentUserService, IUploadService uploadService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _userManager = userManager;
            _uploadService = uploadService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.Where(e => e.UserId == request.Id && !e.IsDeleted).FirstOrDefault();

            if (!_currentUserService.RoleName.Equals(RoleConstants.AdministratorRole))
            {
                if (user == null || !_currentUserService.Username.Equals(user.UserName))
                    return await Result<GetAccountByIdResponse>.FailAsync(StaticVariable.NOT_VIEW_ACCOUNT_INFOR_PERMISSION);
            }

            var account = await (from e in _accountRepository.Entities
                                  where e.Id == request.Id && !e.IsDeleted
                                  select new GetAccountByIdResponse()
                                  {
                                      Name = e.Name,
                                      CitizenId = e.CitizenId,
                                      Gender = e.Gender,
                                      Birthday = e.Birthday,
                                      PhoneNumber = e.PhoneNumber,
                                      Address = e.Address,
                                      Email = e.Email,
                                      IsActive = user != null? user.IsActive : false,
                                      Username = user != null? user.UserName : null,
                                      Role = user != null? _userService.GetRoleIdAsync(user.UserId).Result : null,
                                      Image = e.Image,
                                      ImageLink = _uploadService.GetFullUrl(_uploadService.IsFileExists(e.Image) ? e.Image : "Files/Avatar/NotFound/notFoundAvatar.jpg"),
                                  }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (account == null)
                return await Result<GetAccountByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            return await Result<GetAccountByIdResponse>.SuccessAsync(account);
        }
    }
}