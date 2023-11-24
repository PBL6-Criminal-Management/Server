using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ICheckFileSize
    {
        const long MAX_SIZE_IMAGE = 50 * 1024 * 1024;
        const long MAX_SIZE_VIDEO = 200 * 1024 * 1024;
        string CheckImageSize(IFormFile file);
        string CheckVideoSize(IFormFile file);
    }
}