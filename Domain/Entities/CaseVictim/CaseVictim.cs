using System.ComponentModel.DataAnnotations.Schema;
using Domain.Contracts;

namespace Domain.Entities.CaseVictim
{
    [Table("case_victim")]
    public class CaseVictim : AuditableBaseEntity<long>
    {
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }
        [Column("victim_id", TypeName = "bigint")]
        public long VictimId { get; set; }
        [Column("testimony", TypeName = "text")]
        public string Testimony { get; set; } = null!;

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; } = null!;
        public virtual Domain.Entities.Victim.Victim Victim { get; set; } = null!;
    }
}