using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum WantedType
    {
        [Display(Description = "Bình thường")]
        Normal = 0,
        [Display(Description = "Nguy hiểm")]
        Dangerous = 1,
        [Display(Description = "Đặc biệt")]
        Special = 2
    }
}
