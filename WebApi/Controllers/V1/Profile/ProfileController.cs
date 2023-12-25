using Application.Features.Profile.Command.Edit;
using Application.Features.Profile.Queries.GetByToken;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.Profile
{
    [ApiController]
    [Route("api/v{version:apiVersion}/profile")]
    public class ProfileController : BaseApiController<ProfileController>
    {
        /// <summary>
        /// Edit Profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditReport(EditProfileCommand command)
        {
            var result = await Mediator.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Get Profile
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Result<GetProfileByTokenResponse>>> GetReportById()
        {
            return Ok(await Mediator.Send(new GetProfileByTokenQuery()
            {
            }));
        }
    }
}
