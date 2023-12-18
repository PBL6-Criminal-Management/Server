using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Domain.Constants.Enum;

namespace Application.Dtos.Requests.Criminal
{
    public class CriminalRequest
    {
        public long Id { get; set; }
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Testimony { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string Charge { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_REASON)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Reason { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_WEAPON)]
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Weapon { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
    }
}