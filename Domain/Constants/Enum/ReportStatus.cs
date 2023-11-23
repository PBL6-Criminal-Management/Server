using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum ReportStatus
    {
        [Display(Description = "Chưa xử lý")]
        NoProcess = 0,
        [Display(Description = "Đang xử lý ")]
        Processing = 1,
        [Display(Description = "Đã xử lý")]
        Processed = 2
    }
}