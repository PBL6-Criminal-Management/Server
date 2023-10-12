using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("app_role_claim")]
    public class AppRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        [Column("description", TypeName = "nvarchar(2000)")]
        public string? Description { get; set; }
        [Column("group", TypeName = "nvarchar(300)")]
        public string? Group { get; set; }
        [Column("created_by", TypeName = "nvarchar(100)")]
        public string? CreatedBy { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_by", TypeName = "nvarchar(100)")]
        public string? UpdatedBy { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        [Column("is_deleted", TypeName = "bit")]
        public bool IsDeleted { get; set; }
        public virtual AppRole Role { get; set; } = default!;

        public AppRoleClaim() : base()
        {
        }

        public AppRoleClaim(string roleClaimDescription = null!, string roleClaimGroup = null!) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}