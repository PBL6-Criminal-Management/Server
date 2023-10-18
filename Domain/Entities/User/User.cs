using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.User
{
    [Table("user")]
    public class User : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Column("CMND/CCCD", TypeName = "varchar(12)")]
        public string CMND_CCCD { get; set; } = null!;
        [Column("gender", TypeName = "bit")]
        public bool? Gender { get; set; }
        [Column("birthday", TypeName = "date")]
        public DateOnly? Birthday { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = null!;
        [Column("address", TypeName = "nvarchar(200)")]
        public string Address { get; set; } = null!;
        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; } = null!;
        [Column("image", TypeName = "varchar(500)")]
        public string? Image { get; set; }
    }
}
