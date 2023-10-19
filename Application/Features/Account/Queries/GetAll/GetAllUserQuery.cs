using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Services.Account;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Account.Queries.GetAll
{
    public class GetAllUserQuery : GetAllUserParameter, IRequest<PaginatedResult<GetAllUserResponse>>
    {
    }
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PaginatedResult<GetAllUserResponse>>
    {
        private readonly IAccountRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly IUploadService _uploadService;

        public GetAllUserQueryHandler(IAccountRepository userRepository,
            IAccountService accountService, IUploadService uploadService)
        {
            _userRepository = userRepository;
            _accountService = accountService;
            _uploadService = uploadService;
        }
        public async Task<PaginatedResult<GetAllUserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();
            var query = _userRepository.Entities.AsEnumerable()
                        .Where(user => !user.IsDeleted)
                        .AsQueryable()
                        .Select(user => new GetAllUserResponse
                        {
                            Name = user.Name,
                            Email = user.Email,
                            CreatedAt = user.CreatedAt,
                            Username = user.Name,
                            Address = user.Address,
                            Birthday = user.Birthday,
                            Image = user.Image,
                            ImageLink = _uploadService.GetFullUrl(user.Image),
                            Role = _accountService.GetRoleAsync(user.Id.ToString()).Result,
                            RoleId = _accountService.GetRoleIdAsync(_accountService.GetRoleAsync(user.Id.ToString()).Result).Result
                        }).AsEnumerable()
                         .Where(user => (string.IsNullOrEmpty(request.Keyword)
                         || StringHelper.Contains(user.Name, request.Keyword)
                         || user.Email.Contains(request.Keyword))
                         && (!request.RoleId.HasValue || user.RoleId == request.RoleId)
                         && (string.IsNullOrEmpty(request.Area) || StringHelper.Contains(user.Address, request.Area))
                         && (!request.YearOfBirth.HasValue || user.Birthday.ToString().Contains(request.YearOfBirth.ToString())))
                         .AsQueryable()
                         .Select(user => new GetAllUserResponse
                         {
                             Name = user.Name,
                             Email = user.Email,
                             CreatedAt = user.CreatedAt,
                             Username = user.Name,
                             Address = user.Address,
                             Birthday = user.Birthday,
                             RoleId = user.RoleId,
                             Role = user.Role,
                             Image = user.Image,
                             ImageLink = user.ImageLink,
                         });
            var data = query.OrderBy(request.OrderBy);
            var totalRecord = data.Count();
            List<GetAllUserResponse> result;

            //Pagination
            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            return PaginatedResult<GetAllUserResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);

        }
    }
}
