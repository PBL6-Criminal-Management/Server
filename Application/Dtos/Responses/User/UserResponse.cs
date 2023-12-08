namespace Application.Dtos.Responses.User
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Birthday { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}