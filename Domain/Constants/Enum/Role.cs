using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum Role
    {
        [Display(Description = "Admin")]
        Admin = 0,
        [Display(Description = "Officer")]
        Officer = 1,
        [Display(Description = "Investigator")]
        Investigator = 2,
        [Display(Description = "None")]
        None = 3
    }
}
