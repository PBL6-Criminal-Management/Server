using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ReportingImage
{
    public class ReportingImage : AuditableBaseEntity<long>
    {
        [Column("reporting_id", TypeName = "bigint")]
        public long? ReportingId { get; set; }
        [Column("file_name", TypeName = "nvarchar(50)")]
        public string? FileName { get; set; }
        [Column("file_path", TypeName = "nvarchar(500)")]
        public string? FilePath { get; set; }
    }
}
