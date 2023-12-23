using System.Text.Json.Serialization;

namespace Application.Features.Profile.Queries.GetByToken
{
    public class GetProfileByTokenResponse
    {
        public string Name { get; set; } = null!;
        public string CitizenId { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Image { get; set; }
        public string? ImageLink { get; set; }
    }
}