using System.Text.Json.Serialization;

namespace Application.Dtos.Requests.Testimony
{
    public class TestimonyRequest
    {
        public long Id { get; set; }
        public string Testimony { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime Date { get; set; }
    }
}