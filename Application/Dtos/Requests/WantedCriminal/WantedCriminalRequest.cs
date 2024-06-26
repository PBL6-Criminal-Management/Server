using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Constants;
using Domain.Constants.Enum;

namespace Application.Dtos.Requests.WantedCriminal
{
    public class WantedCriminalRequest
    {
        public long CriminalId { get; set; }
        public WantedType WantedType { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CURRENT_ACTIVITY)]
        public string? CurrentActivity { get; set; }
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_WANTED_DECISION_NO)]
        [RegularExpression(@"^[\p{L}0-9. -]+$", ErrorMessage = StaticVariable.WANTED_DECISION_NO_VALID_CHARACTER)]
        public string WantedDecisionNo { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly WantedDecisionDay { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_DECISION_MAKING_UNIT)]
        [RegularExpression(@"^[\p{L}0-9,. -]+$", ErrorMessage = StaticVariable.DECISION_MAKING_UNIT_VALID_CHARACTER)]
        public string DecisionMakingUnit { get; set; } = null!;
    }
}