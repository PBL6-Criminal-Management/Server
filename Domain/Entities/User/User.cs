using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.User
{
    [Table("user")]
    public class User : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }
        [Column("CMND/CCCD", TypeName = "varchar(12)")]
        public string? CMND_CCCD { get; set; }
        [Column("gender", TypeName = "bit")]
        public bool gender { get; set; }
        [Column("birthday", TypeName = "datetime")]
        public DateTime? Birthday { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string? PhoneNumber { get; set; }
        [Column("address", TypeName = "nvarchar(200)")]
        public string? Address { get; set; }
        [Column("email", TypeName = "varchar(100)")]
        public string? Email { get; set; }
        [Column("image", TypeName = "varchar(500)")]
        public string Image { get; set; }
        [Column("is_active", TypeName = "bit")]
        public bool? IsActive { get; set; }
    }
}
