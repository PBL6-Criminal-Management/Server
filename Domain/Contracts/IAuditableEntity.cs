namespace Domain.Contracts
{
    public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {
        string? CreatedBy { get; set; }

        DateTime CreatedAt { get; set; }

        string? UpdateBy { get; set; }

        DateTime? UpdateAt { get; set; }

        bool IsDeleted { get; set; }
    }
}