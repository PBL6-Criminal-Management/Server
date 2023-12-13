using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Domain.Constants.Enum;

namespace Application.Dtos.Requests.Criminal
{
    public class CriminalRequest
    {
        public long Id { get; set; }
        public string Testimony { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        public string Charge { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_REASON)]
        public string? Reason { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_WEAPON)]
        public string? Weapon { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
    }
}