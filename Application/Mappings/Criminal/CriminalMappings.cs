using Application.Features.Criminal.Command.Add;
using Application.Features.Criminal.Command.Edit;
using AutoMapper;

namespace Application.Mappings.Criminal
{
    public class CriminalMappings : Profile
    {
        public CriminalMappings() {
            CreateMap<AddCriminalCommand, Domain.Entities.Criminal.Criminal>()
                .ForMember(dest => dest.CriminalImages, otp => otp.Ignore())
                .ReverseMap();
            CreateMap<EditCriminalCommand, Domain.Entities.Criminal.Criminal>()
                .ForMember(dest => dest.CriminalImages, otp => otp.Ignore())
                .ReverseMap();
        }
    }
}
