using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.Account.Queries.GetById
{
    public class GetAccountByIdResponse
    {
        public string Name { get; set; } = null!;
        [JsonPropertyName("cmnd_cccd")]
        public string CMND_CCCD { get; set; } = null!;
        public bool? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public string AccountName { get; set; } = null!;
        //public Role Role { get; set; }
        public string Role { get; set; } = null!;
        public string? Image { get; set; }
        public string? ImageLink { get; set; }
    }
}
