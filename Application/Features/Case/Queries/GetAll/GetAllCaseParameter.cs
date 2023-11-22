using System.Text.Json.Serialization;
using Application.Parameters;
using Domain.Constants.Enum;

namespace Application.Features.Case.Queries.GetAll
{
    public class GetAllCaseParameter : RequestParameter
    {
        public CaseStatus? Status { get; set; }
        public TypeOfViolation? TypeOfViolation { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime? TimeTakesPlace { get; set; }
        public string? Area { get; set; }
    }
}