using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Witness
{
    public class WitnessResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string CitizenId { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime Date { get; set; }
    }
}