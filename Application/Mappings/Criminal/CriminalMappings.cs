

using Application.Features.Criminal.Command.Add;
using AutoMapper;

namespace Application.Mappings.Criminal
{
    public class CriminalMappings : Profile
    {
        public CriminalMappings() { 
            CreateMap<AddCriminalCommand, Domain.Entities.Criminal.Criminal>().ReverseMap();
        }
    }
}
