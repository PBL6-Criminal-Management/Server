using Domain.Constants.Enum;

namespace Application.Features.Criminal.Queries.GetAll
{
    public class GetAllCriminalResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int YearOfBirth { get; set; }
        public string PermanentResidence { get; set; } = null!;
        public CriminalStatus Status { get; set; }
        public string Charge { get; set; } = null!;
        public DateOnly DateOfMostRecentCrime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
