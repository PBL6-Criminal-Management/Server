using AutoMapper;

namespace Application.Mappings.Evidence
{
    public class EvidenceMappings : Profile
    {
        public EvidenceMappings()
        {
            CreateMap<Domain.Entities.Evidence.Evidence, Application.Dtos.Requests.Evidence.EvidenceRequest>()
                .ReverseMap()
                .ForMember(dest => dest.Id, otp => otp.Ignore());
            CreateMap<Domain.Entities.Evidence.Evidence, Application.Dtos.Responses.Evidence.EvidenceResponse>()
                .ReverseMap();
        }
    }
}
