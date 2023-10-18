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
    }
}
