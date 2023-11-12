using System.ComponentModel.DataAnnotations.Schema;
using Domain.Contracts;

namespace Domain.Entities.CaseInvestigator
{
    [Table("case_investigator")]
    public class CaseInvestigator : AuditableBaseEntity<long>
    {
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }
        [Column("investigator_id", TypeName = "bigint")]
        public long InvestigatorId { get; set; }

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; } = null!;
        public virtual Domain.Entities.User.User Investigator { get; set; } = null!;
    }
}