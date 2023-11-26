using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.WantedCriminal.Queries.GetAll
{
    public class GetAllWantedCriminalResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? AnotherName { get; set; }
        public int YearOfBirth { get; set; }
        public string PermanentResidence { get; set; } = null!;
        public string Characteristics { get; set; } = null!;
        public string Charge { get; set; } = null!;
        public WantedType WantedType { get; set; }
        public string Avatar { get; set; } = null!;
        public string? MurderWeapon { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
