using Application.Features.Criminal.Command.Add;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.Criminal
{
    [ApiController]
    [Route("api/v{version:apiVersion}/criminal")]
    public class CriminalController : BaseApiController<CriminalController>
    {
        /// <summary>
        /// Add Account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPost]
        public async Task<IActionResult> AddAccount(AddCriminalCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
    }
}
