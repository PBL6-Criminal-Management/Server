using Application.Dtos.Responses.File;
using Application.Dtos.Responses.WantedCriminal;
using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.FaceDetect.Queries.Detect
{
    public class DetectResponse
    {
        public bool CanPredict { get; set; }
        public byte[]? ResultFile { get; set; }
        public double? DetectConfidence { get; set; }
        public FoundCriminal? FoundCriminal { get; set; }
    }

    public class FoundCriminal
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
        public string? RelatedCases { get; set; }
        public string? DangerousLevel { get; set; }
        public string? Charge { get; set; }
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
        public string? Avatar { get; set; }
        public string? AvatarLink { get; set; }
        public string? Vehicles { get; set; }
        public List<FileResponse>? CriminalImages { get; set; }
        public bool IsWantedCriminal { get; set; }
        public List<WantedCriminalResponse>? WantedCriminals { get; set; }
    }
}
