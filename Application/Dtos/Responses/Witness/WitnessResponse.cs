using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Witness
{
    public class WitnessResponse
    {
        public string Name { get; set; } = null!;
        [JsonPropertyName("cmnd_cccd")]
        public string CMND_CCCD { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}