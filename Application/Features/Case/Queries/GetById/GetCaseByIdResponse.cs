using Application.Dtos.Responses.Criminal;
using Application.Dtos.Responses.Evidence;
using Application.Dtos.Responses.File;
using Application.Dtos.Responses.User;
using Application.Dtos.Responses.Victim;
using Application.Dtos.Responses.WantedCriminal;
using Application.Dtos.Responses.Witness;
using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.Case.Queries.GetById
{
    public class GetCaseByIdResponse
    {
        public long Id { get; set; }
        public string? Reason { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public CaseStatus Status { get; set; }
        public string Charge { get; set; } = null!;
        public string Area { get; set; } = null!;
        public List<EvidenceResponse>? Evidences { get; set; }
        public List<WitnessResponse>? Witnesses { get; set; }
        public List<FileResponse>? CaseImages { get; set; }
        public List<CriminalResponse>? Criminals { get; set; }
        public List<UserResponse>? Investigators { get; set; }
        public List<VictimResponse>? Victims { get; set; }
        public List<WantedCriminalOfCaseResponse>? WantedCriminalResponse { get; set; }
        public string? Description { get; set; }

    }
}
