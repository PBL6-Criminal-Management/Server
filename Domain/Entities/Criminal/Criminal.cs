using Domain.Contracts;
using Domain.Entities.WantedCriminal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Criminal
{
    [Table("criminal")]
    public class Criminal : AuditableBaseEntity<long>
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column("another_name", TypeName = "nvarchar(100)")]
        public string AnotherName { get; set; }
        [Column("CCCD/CMND", TypeName = "varchar(12)")]
        public string CCCD_CMND { get; set; }
        [Column("gender", TypeName = "bit")]
        public bool? Gender { get; set; }
        [Column("birthday", TypeName = "datetime")]
        public DateTime Birthday { get; set; }
        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }
        [Column("phone_model", TypeName = "nvarchar(100)")]
        public string PhoneModel { get; set; }
        [Column("career_and_workplace", TypeName = "nvarchar(300)")]
        public string CareerAndWorkplace { get; set; }
        [Column("characteristics", TypeName = "nvarchar(500)")]
        public string Characteristics { get; set; }
        [Column("home_town", TypeName = "nvarchar(200)")]
        public string HomeTown { get; set; }
        [Column("ethnicity", TypeName = "nvarchar(50)")]
        public string Ethnicity { get; set; }
        [Column("religion", TypeName = "nvarchar(50)")]
        public string? Religion { get; set; }
        [Column("nationality", TypeName = "nvarchar(50)")]
        public string Nationality { get; set; }
        [Column("father_name", TypeName = "nvarchar(100)")]
        public string FatherName { get; set; }
        [Column("father_CCCD/CMND", TypeName = "varchar(12)")]
        public string Father_CCCD_CMND { get; set; }
        [Column("father_birthday", TypeName = "datetime")]
        public DateTime FatherBirthday { get; set; }
        [Column("mother_name", TypeName = "nvarchar(100)")]
        public string MotherName { get; set; }
        [Column("mother_CCCD/CMND", TypeName = "varchar(12)")]
        public string Mother_CCCD_CMND { get; set; }
        [Column("mother_birthday", TypeName = "datetime")]
        public DateTime MotherBirthday { get; set; }
        [Column("permanent_residence", TypeName = "nvarchar(200)")]
        public string PermanentResidence { get; set; }
        [Column("current_accommodation", TypeName = "nvarchar(200)")]
        public string CurrentAccommodation { get; set; }
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
        [Column("date_of_most_recent_crime", TypeName = "datetime")]
        public DateTime DateOfMostRecentCrime { get; set; }
        [Column("release_date", TypeName = "datetime")]
        public DateTime? ReleaseDate { get; set; }
        [Column("status", TypeName = "smallint")]
        public int Status { get; set; }
        [Column("other_information", TypeName = "nvarchar(500)")]
        public string? OtherInformation { get; set; }

        // Relationship
        public virtual ICollection<Domain.Entities.WantedCriminal.WantedCriminal> WantedCriminals { get; set; }
        public virtual ICollection<Domain.Entities.CriminalImage.CriminalImage> CriminalImages { get; set;}
        public virtual ICollection<Domain.Entities.CaseCriminal.CaseCriminal> CaseCriminals { get; set; }
    }
}
