using Domain.Constants.Enum;
using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CaseCriminal
{
    [Table("case_criminal")]
    public class CaseCriminal : AuditableBaseEntity<long>
    {
        [Column("criminal_id", TypeName = "bigint")]
        public long CriminalId { get; set; }
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }
        [Column("testimony", TypeName = "text")]
        public string Testimony { get; set; } = null!;
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("charge", TypeName = "nvarchar(100)")]
        public string Charge { get; set; } = null!;
        [Column("reason", TypeName = "nvarchar(500)")]
        public string? Reason { get; set; }
        [Column("weapon", TypeName = "nvarchar(100)")]
        public string? Weapon { get; set; }
        [Column("type_of_violation", TypeName = "smallint")]
        public TypeOfViolation TypeOfViolation { get; set; }
        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; } = null!;
        public virtual Domain.Entities.Criminal.Criminal Criminal { get; set; } = null!;
    }
}
