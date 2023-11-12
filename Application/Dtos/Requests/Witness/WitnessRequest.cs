using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Constants;

namespace Application.Dtos.Requests.Witness
{
    public class WitnessRequest
    {
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_CMND_CCCD)]
        [JsonPropertyName("cmnd_cccd")]
        public string CMND_CCCD { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()-_=+[\]{}|;:',.<>\/?~]{8,}$", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [DefaultValue("stringst")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}