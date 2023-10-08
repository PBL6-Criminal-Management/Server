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

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; }
        public virtual Domain.Entities.Criminal.Criminal Criminal { get; set; }
    }
}
