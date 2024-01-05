using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ReportingImage
{
    [Table("reporting_image")]
    public class ReportingImage : AuditableBaseEntity<long>
    {
        [Column("reporting_id", TypeName = "bigint")]
        public long ReportingId { get; set; }
        [Column("file_name", TypeName = "nvarchar(50)")]
        public string? FileName { get; set; }
        [Column("file_path", TypeName = "nvarchar(500)")]
        public string FilePath { get; set; } = null!;

        //Relationship
        public virtual Domain.Entities.CrimeReporting.CrimeReporting CrimeReporting { get; set; } = null!;
    }
}
