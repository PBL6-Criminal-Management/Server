using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum TypeOfViolation
    {
        [Display(Description = "Dân sự")]
        Civil = 0,
        [Display(Description = "Hình sự")]
        Criminal = 1,
    }
}
