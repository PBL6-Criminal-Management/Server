namespace Application.Dtos.Responses.DetectResult
{
    public class DetectResult
    {
        public string? error { get; set; }
        public string? message { get; set; }
        public string? result { get; set; }
        public bool? isPredictable { get; set; }
        public int? label { get; set; }
        public double? confidence { get; set; }
        public byte[]? image { get; set; }
    }
}
