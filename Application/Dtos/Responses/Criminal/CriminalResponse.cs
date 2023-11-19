namespace Application.Dtos.Responses.Criminal
{
    public class CriminalResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}