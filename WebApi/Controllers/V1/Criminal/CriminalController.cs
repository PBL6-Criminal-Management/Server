using Application.Features.Criminal.Command.Add;
using Domain.Constants;
using Application.Features.Criminal.Queries.GetAll;
using Domain.Wrappers;
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

        
        /// <summary>
        /// Get All Criminal 
        /// </summary>
        /// <param name=""></param>
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
    }
}
