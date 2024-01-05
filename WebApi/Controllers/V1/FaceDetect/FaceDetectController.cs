using Application.Features.FaceDetect.Command.Retrain;
using Application.Features.FaceDetect.Queries.Detect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1.FaceDetect
{
    [ApiController]
    [Route("api/v{version:apiVersion}/face-detect")]
    public class FaceDetectController : BaseApiController<FaceDetectController>
    {
        /// <summary>
        /// Retrain AI model
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("retrain")]
        public async Task<ActionResult<string>> Retrain()
        {
            return Ok(await Mediator.Send(new RetrainCommand()));
        }

        /// <summary>
        /// Face detect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("detect")]
        public async Task<ActionResult<DetectResponse>> Detect([FromForm] DetectQuery request)
        {
            return Ok(await Mediator.Send(new DetectQuery()
            {
                CriminalImage = request.CriminalImage
            }));
        }
    }
}
