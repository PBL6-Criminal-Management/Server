using Domain.Constants.Enum;
using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Criminal
{
    [Table("criminal")]
    public class Criminal : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Column("another_name", TypeName = "nvarchar(100)")]
        public string AnotherName { get; set; } = null!;
        [Column("CMND/CCCD", TypeName = "varchar(12)")]
        public string CMND_CCCD { get; set; } = null!;
        [Column("gender", TypeName = "bit")]
        public bool? Gender { get; set; }
        [Column("birthday", TypeName = "date")]
        public DateOnly Birthday { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = null!;
        [Column("phone_model", TypeName = "nvarchar(100)")]
        public string PhoneModel { get; set; } = null!;
        [Column("career_and_workplace", TypeName = "nvarchar(300)")]
        public string CareerAndWorkplace { get; set; } = null!;
        [Column("characteristics", TypeName = "nvarchar(500)")]
        public string Characteristics { get; set; } = null!;
        [Column("home_town", TypeName = "nvarchar(200)")]
        public string HomeTown { get; set; } = null!;
        [Column("ethnicity", TypeName = "nvarchar(50)")]
        public string Ethnicity { get; set; } = null!;
        [Column("religion", TypeName = "nvarchar(50)")]
        public string? Religion { get; set; }
        [Column("nationality", TypeName = "nvarchar(50)")]
        public string Nationality { get; set; } = null!;
        [Column("father_name", TypeName = "nvarchar(100)")]
        public string FatherName { get; set; } = null!;
        [Column("father_CMND/CCCD", TypeName = "varchar(12)")]
        public string Father_CMND_CCCD { get; set; } = null!;
        [Column("father_birthday", TypeName = "date")]
        public DateOnly FatherBirthday { get; set; }
        [Column("mother_name", TypeName = "nvarchar(100)")]
        public string MotherName { get; set; } = null!;
        [Column("mother_CMND/CCCD", TypeName = "varchar(12)")]
        public string Mother_CMND_CCCD { get; set; } = null!;
        [Column("mother_birthday", TypeName = "date")]
        public DateOnly MotherBirthday { get; set; }
        [Column("permanent_residence", TypeName = "nvarchar(200)")]
        public string PermanentResidence { get; set; } = null!;
        [Column("current_accommodation", TypeName = "nvarchar(200)")]
        public string CurrentAccommodation { get; set; } = null!;
        [Column("entry_and_exit_information", TypeName = "nvarchar(500)")]
        public string? EntryAndExitInformation { get; set; }
        [Column("facebook", TypeName = "nvarchar(100)")]
        public string? Facebook { get; set; }
        [Column("zalo", TypeName = "nvarchar(100)")]
        public string? Zalo { get; set; }
        [Column("other_social_networks", TypeName = "nvarchar(300)")]
        public string? OtherSocialNetworks { get; set; }
        [Column("game_account", TypeName = "nvarchar(100)")]
        public string? GameAccount { get; set; }
        [Column("research", TypeName = "text")]
        public string? Research { get; set; }
        [Column("vehicles", TypeName = "nvarchar(200)")]
        public string? Vehicles { get; set; }
        [Column("dangerous_level", TypeName = "nvarchar(200)")]
        public string? DangerousLevel { get; set; }
        [Column("approach_arrange", TypeName = "text")]
        public string? ApproachArrange { get; set; }
        [Column("date_of_most_recent_crime", TypeName = "date")]
        public DateOnly DateOfMostRecentCrime { get; set; }
        [Column("release_date", TypeName = "date")]
        public DateOnly? ReleaseDate { get; set; }
        [Column("status", TypeName = "smallint")]
        public CriminalStatus Status { get; set; }
        [Column("other_information", TypeName = "nvarchar(500)")]
        public string? OtherInformation { get; set; }

        // Relationship
        public virtual ICollection<Domain.Entities.WantedCriminal.WantedCriminal>? WantedCriminals { get; set; }
        public virtual ICollection<Domain.Entities.CriminalImage.CriminalImage>? CriminalImages { get; set;}
        public virtual ICollection<Domain.Entities.CaseCriminal.CaseCriminal> CaseCriminals { get; set; } = null!;
    }
}
