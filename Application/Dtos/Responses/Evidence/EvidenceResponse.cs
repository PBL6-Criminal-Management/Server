namespace Application.Dtos.Responses.Evidence
{
    public class EvidenceResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}