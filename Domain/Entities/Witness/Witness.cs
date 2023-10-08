using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Witness
{
    [Table("witness")]
    public class Witness : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column("CMND/CCCD", TypeName = "varchar(12)")]
        public string CMND_CCCD { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }
        [Column("address", TypeName = "nvarchar(200)")]
        public string Address { get; set; }
        [Column("testimony", TypeName = "text")]
        public string Testimony { get; set; }
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; }
    }
}
