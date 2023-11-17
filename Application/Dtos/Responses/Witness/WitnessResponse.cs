using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Witness
{
    public class WitnessResponse
    {
        public string Name { get; set; } = null!;
        [JsonPropertyName("citizen_id")]
        public string CitizenID { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime Date { get; set; }
    }
}