using Domain.Constants.Enum;

namespace Application.Dtos.Responses.Identity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public Role Role { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public long UserId { get; set; }
    }
}