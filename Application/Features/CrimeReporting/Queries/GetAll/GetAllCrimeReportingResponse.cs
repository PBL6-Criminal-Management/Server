using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.CrimeReporting.Queries.GetAll
{
    public class GetAllCrimeReportingResponse
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string ReporterName { get; set; } = null!;
        public string? ReporterEmail { get; set; }
        public string ReporterPhone { get; set; } = null!;
        public string ReporterAddress { get; set; } = null!;
        public string Content { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime CreatedAt { get; set; }
        public ReportStatus Status { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly SendingTime { get; set; }
    }
}