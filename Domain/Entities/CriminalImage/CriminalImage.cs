using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CriminalImage
{
    [Table("criminal_image")]
    public class CriminalImage : AuditableBaseEntity<long>
    {
        [Column("criminal_id", TypeName = "bigint")]
        public long CriminalId { get; set; }
        [Column("file_name", TypeName = "nvarchar(50)")]
        public string? FileName { get; set; }
        [Column("file_path", TypeName = "nvarchar(500)")]
        public string FilePath { get; set; } = null!;
        //Relationship
        public virtual Domain.Entities.Criminal.Criminal Criminal { get; set; } = null!;
    }
}
