using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Victim
{
    public class VictimResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public bool? Gender { get; set; }
        public string CitizenId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
    }
}