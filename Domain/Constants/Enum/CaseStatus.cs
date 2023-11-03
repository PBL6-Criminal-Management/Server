using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum CaseStatus
    {
        [Display(Description = "Chưa xét xử")]
        NotYetTried = 0,
        [Display(Description = "Đang điều tra")]
        Investigating = 1,
        [Display(Description = "Đã xét xử")]
        Tried = 2
    }
}
