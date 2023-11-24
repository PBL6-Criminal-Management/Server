using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CheckFileSize : ICheckFileSize
    {
        public string CheckImageSize(IFormFile file)
        {
            string result = "";
            long fileSize = file.Length;
            long maxSizeImage = ICheckFileSize.MAX_SIZE_IMAGE;
            if (fileSize > maxSizeImage)
            {
                result = $"Ảnh {file.FileName} vượt quá kích thước tối đa cho phép ({maxSizeImage / (1024 * 1024)} MB)";
                return result;
            }
            return result;
        }

        public string CheckVideoSize(IFormFile file)
        {
            string result = "";
            long fileSize = file.Length;
            long maxSizeVideo = ICheckFileSize.MAX_SIZE_VIDEO;

            if (fileSize > maxSizeVideo)
            {
                result = $"Video {file.FileName} vượt quá kích thước tối đa cho phép ({maxSizeVideo / (1024 * 1024)} MB)";
                return result;
            }
            return result;
        }
    }
}