using System.ComponentModel.DataAnnotations.Schema;
using Domain.Contracts;

namespace Domain.Entities.CaseWitness
{
    [Table("case_witness")]
    public class CaseWitness : AuditableBaseEntity<long>
    {
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }
        [Column("witness_id", TypeName = "bigint")]
        public long WitnessId { get; set; }
        [Column("testimony", TypeName = "text")]
        public string Testimony { get; set; } = null!;
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; } = null!;
        public virtual Domain.Entities.Witness.Witness Witness { get; set; } = null!;
    }
}