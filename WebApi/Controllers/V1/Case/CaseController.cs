using Application.Features.Case.Command.Add;
using Application.Features.Case.Command.Delete;
using Application.Features.Case.Command.Edit;
using Application.Features.Case.Queries.GetAll;
using Application.Features.Case.Queries.GetById;
using Domain.Constants;
using Domain.Wrappers;
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
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCase(long id)
        {
            return Ok(await Mediator.Send(new DeleteCaseCommand()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Get Case detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetCaseByIdResponse>>> GetCaseById(long id)
        {
            return Ok(await Mediator.Send(new GetCaseByIdQuery()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Edit Case
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPut]
        public async Task<IActionResult> EditCase(EditCaseCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Get All Case 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllCaseResponse>>> GetAllCriminal([FromQuery] GetAllCaseParameter parameter)
        {
            return Ok(await Mediator.Send(new GetAllCaseQuery()
            {
                Status = parameter.Status,
                TimeTakesPlace = parameter.TimeTakesPlace,
                TypeOfViolation = parameter.TypeOfViolation,
                Area = parameter.Area,
                IsExport = parameter.IsExport,
                OrderBy = parameter.OrderBy,
                Keyword = parameter.Keyword,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
            }));
        }
    }
}
