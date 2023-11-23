using Application.Features.CrimeReporting.Queries.GetAll;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.CrimeReporting
{
    [ApiController]
    [Route("api/v{version:apiVersion}/crime-reporting")]
    public class CrimeReportingController : BaseApiController<CrimeReportingController>
    {
        /// <summary>
        /// Get All Case 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllCrimeReportingResponse>>> GetAllCriminal([FromQuery] GetAllCrimeReportingParameter parameter)
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
    }
}
