using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Constants;

namespace Application.Dtos.Requests.Witness
{
    public class WitnessRequest
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Name { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string CitizenId { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()-_=+[\]{}|;:',.<>\/?~]{8,}$", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [DefaultValue("stringst")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Address { get; set; } = null!;
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Testimony { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime Date { get; set; }
    }
}