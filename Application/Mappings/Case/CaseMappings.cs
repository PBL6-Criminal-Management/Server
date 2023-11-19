using Application.Features.Case.Command.Add;
using Application.Features.Case.Command.Edit;
using Application.Features.Case.Queries.GetById;
using AutoMapper;

namespace Application.Mappings.Case
{
    public class CaseMappings : Profile
    {
        public CaseMappings()
        {
            CreateMap<Domain.Entities.Case.Case, AddCaseCommand>().ReverseMap();
            CreateMap<Domain.Entities.Case.Case, GetCaseByIdResponse>()
                .ForMember(dest => dest.Evidences, otp => otp.Ignore())
                .ReverseMap();
            CreateMap<Domain.Entities.Case.Case, EditCaseCommand>()
                .ReverseMap()
                .ForMember(dest => dest.Evidences, otp => otp.Ignore());
        }
    }
}
