using Domain.Constants.Enum;

namespace Application.Features.Account.Queries.GetAll
{
    public class GetAllUserResponse
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role? RoleId { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; }
        public string? ImageLink { get; set; }
        public DateOnly? Birthday { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
