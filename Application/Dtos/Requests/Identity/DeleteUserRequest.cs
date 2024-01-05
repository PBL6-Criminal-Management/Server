using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Requests.Identity
{
    public class DeleteUserRequest
    {
        [Required]
        public long Id { get; set; }
    }
}
