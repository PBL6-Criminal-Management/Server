using System.Text.Json.Serialization;
using Domain.Constants.Enum;

namespace Application.Features.Case.Queries.GetAll
{
    public class GetAllCaseResponse
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Charge { get; set; } = null!;
        public string? TimeTakesPlace { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public CaseStatus Status { get; set; }
        public string Area { get; set; } = null!;
        public List<CriminalId>? CriminalOfCase { get; set; }
        public class CriminalId
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
        }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}