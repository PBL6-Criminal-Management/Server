using Application.Features.Case.Command.Add;
using Application.Features.Case.Command.Delete;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.Case
{
    [ApiController]
    [Route("api/v{version:apiVersion}/case")]
    public class CaseController : BaseApiController<CaseController>
    {
        /// <summary>
        /// Add Case
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPost]
        public async Task<IActionResult> AddCase(AddCaseCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Delete Case
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCase(long id)
        {
            return Ok(await Mediator.Send(new DeleteCaseCommand()
            {
                Id = id
            }));
        }
    }
}
