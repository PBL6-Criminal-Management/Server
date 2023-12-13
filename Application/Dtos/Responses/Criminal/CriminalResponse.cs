using System.Text.Json.Serialization;
using Domain.Constants.Enum;

namespace Application.Dtos.Responses.Criminal
{
    public class CriminalResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Testimony { get; set; } = null!;
        public string Charge { get; set; } = null!;
        public string? Reason { get; set; }
        public string? Weapon { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
    }
}