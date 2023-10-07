namespace Domain.Contracts
{
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
        public TId Id { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}