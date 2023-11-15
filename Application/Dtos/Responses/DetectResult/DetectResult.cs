namespace Application.Dtos.Responses.DetectResult
{
    public class DetectResult
    {
        public string? Message { get; set; }
        public int? CriminalId { get; set; }
        public byte[]? DetectResultFile { get; set; }
        public double? DetectConfidence { get; set; }
    }
}
