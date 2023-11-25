using Application.Parameters;
using Domain.Constants.Enum;

namespace Application.Features.WantedCriminal.Queries.GetAll
{
    public class GetAllWantedCriminalParameter : RequestParameter
    {
        public WantedType? WantedType { get; set; }
        public int? YearOfBirth { get; set; }
    }
}
