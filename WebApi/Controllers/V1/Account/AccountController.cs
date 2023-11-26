using Application.Features.Account.Command.Delete;
﻿using Application.Features.Account.Command.Add;
using Application.Features.Account.Queries.GetAll;
using Application.Features.Account.Queries.GetById;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Account.Command.Edit;

namespace WebApi.Controllers.V1.Account
{
    [ApiController]
    [Route("api/v{version:apiVersion}/account")]
    public class AccountController : BaseApiController<AccountController>
    {
        /// <summary>
        /// Get Account detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetAccountByIdResponse>>> GetAccountById(long id)
        {
            return Ok(await Mediator.Send(new GetAccountByIdQuery()
            {
                Id = id
            }));
        }

        /// <summary>
        /// Get All Account 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllUserResponse>>> GetAllAccount([FromQuery] GetAllUserQuery parameter)
        {
            return Ok(await Mediator.Send(new GetAllUserQuery()
            {
                RoleId = parameter.RoleId,
                Area = parameter.Area,
                YearOfBirth = parameter.YearOfBirth,
                IsExport = parameter.IsExport,
                OrderBy = parameter.OrderBy,
                Keyword = parameter.Keyword,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
            }));
        }
        /// <summary>
        /// Delete Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(long id)
        {
            return Ok(await Mediator.Send(new DeleteAccountCommand()
            {
                Id = id
            }));
        }

        /// <summary>
        /// Add Account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpPost]
        public async Task<IActionResult> AddAccount(AddAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Edit Account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditAcount(EditAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
    }
}
