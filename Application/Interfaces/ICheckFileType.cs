using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ICheckFileType
    {
        string CheckFileIsImage(IFormFile file);
        string CheckFileIsVideo(IFormFile file);
    }
}