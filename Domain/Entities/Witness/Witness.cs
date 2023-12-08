using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Witness
{
    [Table("witness")]
    public class Witness : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Column("citizen_id", TypeName = "varchar(12)")]
        public string CitizenId { get; set; } = null!;
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = null!;
        [Column("address", TypeName = "nvarchar(200)")]
        public string Address { get; set; } = null!;
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }

        //Relationship
        public virtual ICollection<Domain.Entities.CaseWitness.CaseWitness> CaseWitnesses { get; set; } = null!;
    }
}
