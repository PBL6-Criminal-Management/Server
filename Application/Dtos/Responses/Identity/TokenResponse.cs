using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Dtos.Responses.Identity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public Role Role { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime TokenExpiryTime { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime RefreshTokenExpiryTime { get; set; }
        public long UserId { get; set; }
    }
}