namespace Application.Dtos.Requests.Criminal
{
    public class CriminalRequest
    {
        public long Id { get; set; }
        public string Testimony { get; set; } = null!;
    }
}