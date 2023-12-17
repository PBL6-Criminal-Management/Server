using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.Criminal.Queries.GetAll
{
    public class GetAllCriminalResponse
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int YearOfBirth { get; set; }
        public string PermanentResidence { get; set; } = null!;
        public CriminalStatus Status { get; set; }
        public string? Charge { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? DateOfMostRecentCrime { get; set; }
        public string AvatarLink { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
