using Application.Parameters;
using Domain.Constants.Enum;

namespace Application.Features.Criminal.Queries.GetAll
{
    public class GetAllCriminalParameter : RequestParameter
    {
        public CriminalStatus? Status { get; set; }
        public int? YearOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Characteristics { get; set; }
        public TypeOfViolation? TypeOfViolation { get; set; }
        public string? Area { get; set; }
        public string? Charge { get; set; }
    }
}
