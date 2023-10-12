using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contracts
{
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
        public TId Id { get; set; } = default!;
        [Column("created_by", TypeName = "nvarchar(100)")]
        public string? CreatedBy { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_by", TypeName = "nvarchar(100)")]
        public string? UpdatedBy { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        [Column("is_deleted", TypeName = "bit")]
        public bool IsDeleted { get; set; } = false;
    }
}