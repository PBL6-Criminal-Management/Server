using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Criminal
{
    public class CriminalResponse
    {
        public long Id { get; set; }        
        public string Name { get; set; } = null!;
        public string? AnotherName { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        public bool? Gender { get; set; }
        public string CitizenId { get; set; } = null!;
        public string HomeTown { get; set; } = null!;
        public string PermanentResidence { get; set; } = null!;
        public string CurrentAccommodation { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Ethnicity { get; set; } = null!;   
        public string Charge { get; set; } = null!;
        public string? Reason { get; set; }
        public string Testimony { get; set; } = null!;
        public TypeOfViolation TypeOfViolation { get; set; }
        public string? Weapon { get; set; }
    }
}