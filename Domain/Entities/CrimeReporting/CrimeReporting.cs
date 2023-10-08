using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CrimeReporting
{
    [Table("crime_reporting")]
    public class CrimeReporting : AuditableBaseEntity<long>
    {
        [Column("reporter_name", TypeName = "nvarchar(100)")]
        public string ReporterName { get; set; }
        [Column("reporter_email", TypeName = "varchar(100)")]
        public string? ReporterEmail { get; set; }
        [Column("reporter_phone", TypeName = "nvarchar(15)")]
        public string ReporterPhone { get; set; }
        [Column("reporter_address", TypeName = "nvarchar(200)")]
        public string ReporterAddress { get; set; }
        [Column("content", TypeName = "text")]
        public string Content { get; set; }
        [Column("status", TypeName = "smallint")]
        public int Status { get; set; }
        [Column("note", TypeName = "text")]
        public string? Note { get; set; }

        //Relationship
        public virtual ICollection<Domain.Entities.ReportingImage.ReportingImage> ReportingImages { get; set; }
    }
}
