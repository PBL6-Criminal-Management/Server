using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Victim
{
    public class VictimResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public bool? Gender { get; set; }
        [JsonPropertyName("citizen_id")]
        public string CitizenID { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}