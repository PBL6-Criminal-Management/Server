using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Services.Account;
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
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUploadService _uploadService;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepository, IAccountService accountService, UserManager<AppUser> userManager, IUploadService uploadService)
        {
            _accountRepository = accountRepository; 
            _accountService = accountService;
            _userManager = userManager;
            _uploadService = uploadService;
        }

        public async Task<Result<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.Where(e => e.UserId == request.Id && !e.IsDeleted).FirstOrDefault();
            if(user == null)
                return await Result<GetAccountByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var account = await (from e in _accountRepository.Entities
                                  where e.Id == request.Id && !e.IsDeleted
                                  select new GetAccountByIdResponse()
                                  {
                                      Name = e.Name,
                                      CitizenID = e.CitizenID,
                                      Gender = e.Gender,
                                      Birthday = e.Birthday,
                                      PhoneNumber = e.PhoneNumber,
                                      Address = e.Address,
                                      Email = e.Email,
                                      IsActive = user.IsActive,
                                      AccountName = user.UserName!,
                                      Role = _accountService.GetRoleIdAsync(user.UserId).Result,
                                      Image = e.Image,
                                      ImageLink = _uploadService.GetFullUrl(e.Image),
                                  }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (account == null)
                return await Result<GetAccountByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            return await Result<GetAccountByIdResponse>.SuccessAsync(account);
        }
    }
}