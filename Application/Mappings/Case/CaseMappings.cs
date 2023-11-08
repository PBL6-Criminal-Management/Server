using Application.Features.Case.Command.Add;
using AutoMapper;

namespace Application.Mappings.Case
{
    public class CaseMappings : Profile
    {
        public CaseMappings()
        {
            CreateMap<Domain.Entities.Case.Case, AddCaseCommand>().ReverseMap();
        }
    }
}
