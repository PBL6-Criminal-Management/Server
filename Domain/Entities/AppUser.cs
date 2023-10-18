using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("app_user")]
    public class AppUser : IdentityUser<string>, IAuditableEntity<string>
    {
        [Column("full_name", TypeName = "nvarchar(100)")]
        public string? FullName { get; set; }
        [Column("avatar_url", TypeName = "nvarchar(500)")]
        public string? AvatarUrl { get; set; }
        [Column("user_id", TypeName = "bigint")]
        public long UserId { get; set; }
        [Column("is_active", TypeName = "bit")]
        public bool IsActive { get; set; }
        [Column("refresh_token", TypeName = "varchar(2000)")]
        public string? RefreshToken { get; set; }
        [Column("refresh_token_expiry_time", TypeName = "datetime")]
        public DateTime RefreshTokenExpiryTime { get; set; }
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

        public static implicit operator global::System.String(AppUser v)
        {
            throw new global::System.NotImplementedException();
        }
    }
}