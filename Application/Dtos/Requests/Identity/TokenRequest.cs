using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Requests.Identity
{
    public class TokenRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}