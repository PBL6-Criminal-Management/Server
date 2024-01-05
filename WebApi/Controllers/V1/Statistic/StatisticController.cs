using Application.Features.Statistic.Queries.CriminalStructure;
using Domain.Constants;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.Statistic
{
    [ApiController]
    [Route("api/v{version:apiVersion}/statistic")]
    public class StatisticController : BaseApiController<StatisticController>
    {
        /// <summary>
        /// Get Criminal Structure Statistic
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("criminal-structure")]
        public async Task<ActionResult<Result<GetCriminalStructureResponse>>> GetCriminalStructureStatistic(int Year)
        {
            return Ok(await Mediator.Send(new GetCriminalStructureQuery { Year = Year}));
        }

        /// <summary>
        /// Get Criminal Situation Developments Statistic
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("criminal-situation-developments")]
        public async Task<ActionResult<Result<CriminalSituationDevelopmentsResponse>>> GetCriminalSituationDevelopmentsStatistic([FromQuery] CriminalSituationDevelopmentsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get Social Order Situation Statistic
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("social-order-situation")]
        public async Task<ActionResult<Result<SocialOrderSituationResponse>>> GetSocialOrderSituationStatistic([FromQuery] SocialOrderSituationQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
