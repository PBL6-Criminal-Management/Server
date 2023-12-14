using Domain.Constants.Enum;

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
        public DateTime CreatedAt { get; set; }
        public ReportStatus Status { get; set; }
        public DateOnly SendingTime { get; set; }
    }
}