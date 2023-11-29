using Application.Features.CrimeReporting.Command.Add;
using Application.Features.CrimeReporting.Command.Delete;
using Application.Features.CrimeReporting.Command.Edit;
using Application.Features.CrimeReporting.Queries.GetAll;
using Application.Features.CrimeReporting.Queries.GetById;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.CrimeReporting
{
    [ApiController]
    [Route("api/v{version:apiVersion}/report")]
    public class CrimeReportingController : BaseApiController<CrimeReportingController>
    {
        /// <summary>
        /// Get All Report 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllCrimeReportingResponse>>> GetAllReport([FromQuery] GetAllCrimeReportingParameter parameter)
        {
            return Ok(await Mediator.Send(new GetAllCrimeReportingQuery()
            {
                IsExport = parameter.IsExport,
                OrderBy = parameter.OrderBy,
                Keyword = parameter.Keyword,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
            }));
        }
        /// <summary>
        /// Add Report
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddReport(AddCrimeReportingCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Delete Report
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(long id)
        {
            return Ok(await Mediator.Send(new DeleteCrimeReportingCommand()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Get Report detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetCrimeReportingByIdResponse>>> GetReportById(long id)
        {
            return Ok(await Mediator.Send(new GetCrimeReportingByIdQuery()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Edit Report
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPut]
        public async Task<IActionResult> EditReport(EditCrimeReportingCommand command)
        {
            var result = await Mediator.Send(command);
            return (result.Succeeded) ? Ok(result) : BadRequest(result);
        }
    }
}
