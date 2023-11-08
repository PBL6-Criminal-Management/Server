using AutoMapper;

namespace Application.Mappings.Evidence
{
    public class EvidenceMappings : Profile
    {
        public EvidenceMappings() { 
            CreateMap<Domain.Entities.Evidence.Evidence, Application.Features.Case.Command.Add.AddCaseCommand.Evidence>()
                .ReverseMap();
        }
    }
}
