using Application.Dtos.Requests;
using Application.Exceptions;
using Application.Interfaces;
using Application.Shared;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/upload")]
    public class UploadController : BaseApiController<UploadController>
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(2147483648)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] UploadRequest request)
        {
            if (request.Files.Count != 0)
            {
                var result = await _uploadService.UploadAsync(request);
                return result.Succeeded ? Ok(result) : BadRequest(result);
            }

            throw new ApiException(ApplicationConstants.ErrorMessage.InvalidFile);
        }

        /// <summary>
        /// Split Video
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.AdministratorRole + "," + RoleConstants.OfficerRole)]
        [HttpPost("split-video")]
        [RequestSizeLimit(2147483648)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SplitVideo([FromForm] SplitRequest request)
        {
            if (request.Files.Count != 0)
            {
                var result = await _uploadService.SplitVideoAsync(request);
                return result.Succeeded ? Ok(result) : BadRequest(result);
            }

            throw new ApiException(ApplicationConstants.ErrorMessage.InvalidFile);
        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string filePath)
        {
            var result = await _uploadService.DeleteAsync(filePath);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}