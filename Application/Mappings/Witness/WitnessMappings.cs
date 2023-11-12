
using AutoMapper;

namespace Application.Mappings.Witness
{
    public class WitnessMappings : Profile
    {
        public WitnessMappings()
        {
            CreateMap<Domain.Entities.Witness.Witness, Application.Dtos.Requests.Witness.WitnessRequest>()
                .ReverseMap();
        }
    }
}
