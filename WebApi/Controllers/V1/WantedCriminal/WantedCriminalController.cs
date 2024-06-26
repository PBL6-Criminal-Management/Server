using Application.Features.WantedCriminal.Queries.GetAll;
using Domain.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.WantedCriminal
{
    [ApiController]
    [Route("api/v{version:apiVersion}/wanted-criminal")]
    public class WantedCriminalController : BaseApiController<WantedCriminalController>
    {
        /// <summary>
        /// Get All Wanted Criminal 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllWantedCriminalResponse>>> GetAllWantedCriminal([FromQuery] GetAllWantedCriminalQuery query)
        {
            return Ok(await Mediator.Send(new GetAllWantedCriminalQuery()
            {
                Name = query.Name,
                Charge = query.Charge,
                Characteristics = query.Characteristics,
                DecisionMakingUnit = query.DecisionMakingUnit,
                PermanentResidence = query.PermanentResidence,
                Weapon = query.Weapon,
                WantedType = query.WantedType,
                YearOfBirth = query.YearOfBirth,
                IsExport = query.IsExport,
                OrderBy = query.OrderBy,
                Keyword = query.Keyword,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
            }));
        }
    }
}
