using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CaseImage
{
    [Table("case_image")]
    public class CaseImage : AuditableBaseEntity<long>
    {
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }
        [Column("file_name", TypeName = "nvarchar(50)")]
        public string? FileName { get; set; }
        [Column("file_path", TypeName = "nvarchar(500)")]
        public string FilePath { get; set; } = null!;
        [Column("evidence_id", TypeName = "bigint")]
        public long? EvidenceId { get; set; }

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; } = null!;
    }
}
