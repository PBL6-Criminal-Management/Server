using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.Criminal.Queries.GetById
{
    public class GetCriminalByIdResponse
    {
        public string Name { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        public bool? Gender { get; set; }
        public string? AnotherName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string HomeTown { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Ethnicity { get; set; } = null!;
        public string? Religion { get; set; }
        public string CitizenId { get; set; } = null!;
        public string CareerAndWorkplace { get; set; } = null!;
        public string PermanentResidence { get; set; } = null!;
        public string CurrentAccommodation { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly FatherBirthday { get; set; }
        public string FatherCitizenId { get; set; } = null!;
        public string MotherName { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly MotherBirthday { get; set; }
        public string MotherCitizenId { get; set; } = null!;
        public string Characteristics { get; set; } = null!;
        //public string OtherCharacteristics { get; set; } = null!;
        public CriminalStatus Status { get; set; }
        public string RelatedCases { get; set; } = null!;
        public string? DangerousLevel { get; set; }
        public string Charge { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly DateOfMostRecentCrime { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? ReleaseDate { get; set; }
        public string? EntryAndExitInformation { get; set; }
        public string? BankAccount { get; set; }
        public string? GameAccount { get; set; }
        public string? Facebook { get; set; }
        public string? Zalo { get; set; }
        public string? OtherSocialNetworks { get; set; }
        public string PhoneModel { get; set; } = null!;
        public string? Research { get; set; }
        public string? ApproachArrange { get; set; }
        public string? OtherInformation { get; set; }
        public bool IsWantedCriminal { get; set; }
        public WantedType? WantedType { get; set; }
        public string? CurrentActivity { get; set; }
        public string? WantedDecisionNo { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? WantedDecisionDay { get; set; }
        public string? DecisionMakingUnit { get; set; } = null!;
        public string? Image { get; set; }
        public string? ImageLink { get; set; }
        //public string? Vehicles { get; set; }
    }
}
