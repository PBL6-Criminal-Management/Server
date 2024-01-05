using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Constants;

namespace Application.Dtos.Requests.Victim
{
    public class VictimRequest
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        public string Name { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = StaticVariable.CITIZEN_ID_VALID_CHARACTER)]
        public string CitizenId { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [RegularExpression(@"^(?:\+84|84|0)(3|5|7|8|9|1[2689])([0-9]{8,10})\b$", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [DefaultValue("stringst")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.ADDRESS_VALID_CHARACTER)]
        public string Address { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public string Testimony { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime Date { get; set; }
    }
}