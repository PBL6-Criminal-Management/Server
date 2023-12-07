namespace Application.Dtos.Requests.Testimony
{
    public class TestimonyRequest
    {
        public long Id { get; set; }
        public string Testimony { get; set; } = null!;
    }
}