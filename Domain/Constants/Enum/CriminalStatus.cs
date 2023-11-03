using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enum
{
    public enum CriminalStatus
    {
        [Display(Description = "Đang ngồi tù")]
        InPrison = 0,
        [Display(Description = "Đã được thả")]
        Released = 1,
        [Display(Description = "Bị truy nã")]
        Wanted = 2,
        [Display(Description = "Chưa kết án")]
        NotYetConvicted = 3,
        [Display(Description = "Án treo")]
        SuspendedSentence = 4
    }
}
