namespace Application.Dtos.Requests.Image
{
    public class ImageRequest
    {
        public string FilePath { get; set; } = null!;
        public string? FileName { get; set; }
    }
}
