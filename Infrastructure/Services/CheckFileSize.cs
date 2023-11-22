using Application.Dtos.Requests;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CheckFileSize : ICheckSizeFile
    {
        public string CheckImageSize(CheckImageSizeRequest request)
        {
            string result = "";
            foreach (IFormFile file in request.Files)
            {
                long fileSize = file.Length;
                long maxSizeImage = ICheckSizeFile.IMAGE_MAX_SIZE; //50MB  
                if (fileSize >= maxSizeImage)
                {
                    result = StaticVariable.IMAGE_LENGTH_IS_TOO_BIG + $" ({maxSizeImage / (1024 * 1024)} MB)";
                    return result;
                }

            }
            return result;
        }

        public string CheckVideoSize(CheckVideoSizeRequest request)
        {
            string result = "";
            foreach (IFormFile file in request.Files)
            {
                long fileSize = file.Length;
                long maxSizeVideo = ICheckSizeFile.VIDEO_MAX_SIZE; //30MB

                if (fileSize >= maxSizeVideo)
                {
                    result = StaticVariable.VIDEO_LENGTH_IS_TOO_BIG + $" ({maxSizeVideo / (1024 * 1024)} MB)";
                    return result;
                }

            }
            return result;
        }
    }
}