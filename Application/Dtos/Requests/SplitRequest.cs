using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Requests
{
    public class SplitRequest
    {
        public List<IFormFile> Files { get; set; } = default!;
        public string? FilePath { get; set; } = default;
        public int? NumberImagesEachSecond { get; set; }
    }
}