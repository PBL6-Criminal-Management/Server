using Application.Features.Account.Queries.GetAll;
using Application.Features.Account.Queries.GetById;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = RoleConstants.AdministratorRole)]
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
        /// <param name=""></param>
        /// <returns></returns>
        //[Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpGet]
        public async Task<ActionResult<Result<GetAccountByIdResponse>>> GetAllAccount([FromQuery]GetAllUserParameter parameter)
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
    }
}
