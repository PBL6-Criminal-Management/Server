using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("app_user")]
    public class AppUser : IdentityUser<string>, IAuditableEntity<string>
    {
        [Column("full_name", TypeName = "nvarchar(max)")]
        public string? FullName { get; set; }
        [Column("avatar_url", TypeName = "nvarchar(max)")]
        public string? AvatarUrl { get; set; }
        [Column("is_active", TypeName = "bit")]
        public bool IsActive { get; set; }
        [Column("refresh_token", TypeName = "varchar(max)")]
        public string? RefreshToken { get; set; }
        [Column("refresh_token_expiry_time", TypeName = "datetime")]
        public DateTime RefreshTokenExpiryTime { get; set; }
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
    }
}