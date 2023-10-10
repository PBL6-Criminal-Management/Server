using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Evidence
{
    [Table("evidence")]
    public class Evidence : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column("description", TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        [Column("case_id", TypeName = "bigint")]
        public long CaseId { get; set; }

        //Relationship
        public virtual Domain.Entities.Case.Case Case { get; set; }
    }
}
