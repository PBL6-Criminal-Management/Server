using AutoMapper;

namespace Application.Mappings.Victim
{
    public class VictimMappings : Profile
    {
        public VictimMappings()
        {
            CreateMap<Domain.Entities.Victim.Victim, Application.Dtos.Requests.Victim.VictimRequest>()
                .ReverseMap();
        }
    }
}