using System.ComponentModel.DataAnnotations.Schema;
using Domain.Contracts;

namespace Domain.Entities.Victim
{
    [Table("victim")]
    public class Victim : AuditableBaseEntity<long>
    {

        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Column("citizen_id", TypeName = "varchar(12)")]
        public string CitizenID { get; set; } = null!;
        [Column("gender", TypeName = "bit")]
        public bool? Gender { get; set; }
        [Column("birthday", TypeName = "date")]
        public DateOnly? Birthday { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = null!;
        [Column("address", TypeName = "nvarchar(200)")]
        public string Address { get; set; } = null!;

        //Relationship
        public virtual ICollection<Domain.Entities.CaseVictim.CaseVictim>? CaseVictims { get; set; }

    }
}