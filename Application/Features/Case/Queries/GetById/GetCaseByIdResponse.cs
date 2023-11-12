using Application.Dtos.Responses.Criminal;
using Application.Dtos.Responses.Evidence;
using Application.Dtos.Responses.User;
using Application.Dtos.Responses.Victim;
using Application.Dtos.Responses.Witness;
using Domain.Constants.Enum;

namespace Application.Features.Case.Queries.GetById
{
    public class GetCaseByIdResponse
    {
        public long Id { get; set; }
        public string? Reason { get; set; }
        public string? MurderWeapon { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public short Status { get; set; }
        public string Charge { get; set; } = null!;
        public List<EvidenceResponse>? Evidences { get; set; } = new List<EvidenceResponse>();
        public List<WitnessResponse>? Witnesses { get; set; } = new List<WitnessResponse>();
        public List<string>? CaseImages { get; set; } = new List<string>();
        public List<CriminalResponse>? Criminals { get; set; } = new List<CriminalResponse>();
        public List<UserResponse>? Investigators { get; set; } = new List<UserResponse>();

        public List<VictimResponse>? Victims { get; set; } = new List<VictimResponse>();
    }
}
