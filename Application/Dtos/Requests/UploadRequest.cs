using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Requests
{
    public class UploadRequest
    {
        public List<IFormFile> Files { get; set; } = default!;
        public string? FilePath { get; set; } = default;
    }
}