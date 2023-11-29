using Application.Dtos.Responses.File;
using Domain.Constants.Enum;

namespace Application.Features.CrimeReporting.Queries.GetById
{
    public class GetCrimeReportingByIdResponse
    {
        public long Id { get; set; }
        public string ReporterName { get; set; } = null!;
        public string? ReporterEmail { get; set; }
        public string ReporterPhone { get; set; } = null!;
        public string ReporterAddress { get; set; } = null!;
        public string Content { get; set; } = null!;
        public ReportStatus Status { get; set; }
        public string? Note { get; set; }
        public List<FileResponse>? ReportingImages { get; set; }
    }
}