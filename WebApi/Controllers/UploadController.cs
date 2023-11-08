using Application.Dtos.Requests;
using Application.Exceptions;
using Application.Interfaces;
using Application.Shared;
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
        /// <param name="uploadRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [RequestSizeLimit(30 * 1024 * 1024)] //50MB Max upload request
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] string filePath)
        {
            if (Request.Form.Files.Count != 0)
            {
                List<IFormFile> files = new List<IFormFile>();
                foreach (var file in Request.Form.Files)
                {
                    files.Add(file);
                }
                
                var result = await _uploadService.UploadAsync(new UploadRequest
                {
                    FilePath = filePath,
                    Files = files
                });
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
        [Authorize]
        public async Task<IActionResult> DeleteFile([FromForm] string filePath)
        {
            var result = await _uploadService.DeleteAsync(filePath);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}