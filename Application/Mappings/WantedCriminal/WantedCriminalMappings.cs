using Application.Dtos.Requests.WantedCriminal;
using Application.Dtos.Responses.WantedCriminal;
using AutoMapper;
using Domain.Entities.WantedCriminal;

namespace Application.Mappings.Criminal
{
    public class WantedCriminalMappings : Profile
    {
        public WantedCriminalMappings() {
            CreateMap<WantedCriminalRequest, WantedCriminal>().ReverseMap();
            CreateMap<WantedCriminal, WantedCriminalResponse>().ReverseMap();
            CreateMap<WantedCriminal, WantedCriminalOfCaseResponse > ().ReverseMap();
        }
    }
}
