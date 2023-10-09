using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("app_role")]
    public class AppRole : IdentityRole, IAuditableEntity<string>
    {
        [Column("description" , TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        [Column("created_by", TypeName = "nvarchar(max)")]
        public string? CreatedBy { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_by", TypeName = "nvarchar(max)")]
        public string? UpdatedBy { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        [Column("is_deleted", TypeName = "bit")]
        public bool IsDeleted { get; set; }
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }

        public AppRole() : base()
        {
            RoleClaims = new HashSet<AppRoleClaim>();
        }

        public AppRole(string roleName, string? roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<AppRoleClaim>();
            Description = roleDescription;
        }
    }
}