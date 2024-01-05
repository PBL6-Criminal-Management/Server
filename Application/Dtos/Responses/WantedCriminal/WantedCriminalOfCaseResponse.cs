using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Constants;
using Domain.Constants.Enum;

namespace Application.Dtos.Responses.WantedCriminal
{
    public class WantedCriminalOfCaseResponse
    {
        public long CriminalId { get; set; }
        public WantedType WantedType { get; set; }
        public string? CurrentActivity { get; set; }
        public string WantedDecisionNo { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly WantedDecisionDay { get; set; }
        public string DecisionMakingUnit { get; set; } = null!;
        public string? Weapon { get; set; }

    }
}