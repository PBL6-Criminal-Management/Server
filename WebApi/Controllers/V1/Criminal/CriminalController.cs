using Application.Features.Criminal.Command.Add;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Criminal.Command.Edit;
using Application.Features.Criminal.Command.Delete;
using Application.Features.Criminal.Queries.GetAll;
using Application.Features.Criminal.Queries.GetById;

namespace WebApi.Controllers.V1.Criminal
{
    [ApiController]
    [Route("api/v{version:apiVersion}/criminal")]
    public class CriminalController : BaseApiController<CriminalController>
    {
        /// <summary>
        /// Add Criminal
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPost]
        public async Task<IActionResult> AddCriminal(AddCriminalCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }


        /// <summary>
        /// Get All Criminal 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllCriminalResponse>>> GetAllCriminal([FromQuery] GetAllCriminalQuery parameter)
        {
            return Ok(await Mediator.Send(new GetAllCriminalQuery()
            {
                Status = parameter.Status,
                YearOfBirth = parameter.YearOfBirth,
                Gender = parameter.Gender,
                Characteristics = parameter.Characteristics,
                TypeOfViolation = parameter.TypeOfViolation,
                Area = parameter.Area,
                Charge = parameter.Charge,
                IsExport = parameter.IsExport,
                OrderBy = parameter.OrderBy,
                Keyword = parameter.Keyword,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
            }));
        }

        /// <summary>
        /// Get Criminal detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetCriminalByIdResponse>>> GetCriminalById(long id)
        {
            return Ok(await Mediator.Send(new GetCriminalByIdQuery()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Edit Criminal
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPut]
        public async Task<IActionResult> EditCriminal(EditCriminalCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Delete Criminal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCriminal(long id)
        {
            return Ok(await Mediator.Send(new DeleteCriminalCommand()
            {
                Id = id
            }));
        }
    }
}
