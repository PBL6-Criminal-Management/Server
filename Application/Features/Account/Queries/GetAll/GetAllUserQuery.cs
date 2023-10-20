﻿using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Services.Account;
using Domain.Entities;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;

namespace Application.Features.Account.Queries.GetAll
{
    public class GetAllUserQuery : GetAllUserParameter, IRequest<PaginatedResult<GetAllUserResponse>>
    {
    }
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PaginatedResult<GetAllUserResponse>>
    {
        private readonly IAccountRepository _accountRepostitory;
        private readonly IAccountService _accountService;
        private readonly IUploadService _uploadService;
        private readonly UserManager<AppUser> _userManager;

        public GetAllUserQueryHandler(IAccountRepository accountRepostitory,
            IAccountService accountService, IUploadService uploadService,
            UserManager<AppUser> userManager)
        {
            _accountRepostitory = accountRepostitory;
            _accountService = accountService;
            _uploadService = uploadService;
            _userManager = userManager;
        }
        public async Task<PaginatedResult<GetAllUserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();
            var query = _accountRepostitory.Entities.AsEnumerable()
                        .Where(user => !user.IsDeleted)
                        .Select(user => new GetAllUserResponse
                        {
                            Name = user.Name,
                            Email = user.Email,
                            CreatedAt = user.CreatedAt,
                            Username = _userManager.Users.Where(u => u.UserId == user.Id).FirstOrDefault() == null ? "" : _userManager.Users.Where(u => u.UserId == user.Id).FirstOrDefault().UserName,
                            Address = user.Address,
                            Birthday = user.Birthday,
                            Image = user.Image,
                            ImageLink = _uploadService.GetFullUrl(user.Image),
                            RoleId = _accountService.GetRoleIdAsync(user.Id).Result
                        })
                        .AsQueryable()
                        .Where(user => (string.IsNullOrEmpty(request.Keyword)
                         || StringHelper.Contains(user.Name, request.Keyword)
                         || user.Email.Contains(request.Keyword))
                         && (!request.RoleId.HasValue || user.RoleId == request.RoleId)
                         && (string.IsNullOrEmpty(request.Area) || StringHelper.Contains(user.Address, request.Area))
                         && (!request.YearOfBirth.HasValue || (user.Birthday.HasValue && user.Birthday.Value.Year.Equals(request.YearOfBirth))))
                         .AsQueryable()
                         .Select(user => new GetAllUserResponse
                         {
                             Name = user.Name,
                             Email = user.Email,
                             CreatedAt = user.CreatedAt,
                             Username = user.Username,
                             Address = user.Address,
                             Birthday = user.Birthday,
                             RoleId = user.RoleId,
                             Image = user.Image,
                             ImageLink = user.ImageLink,
                         });
            var data = query.OrderBy(request.OrderBy);
            List<GetAllUserResponse> result;

            //Pagination
            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            var totalRecord = result.Count();
            return PaginatedResult<GetAllUserResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);

        }
    }
}