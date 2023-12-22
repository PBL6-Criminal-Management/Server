using Application.Features.Account.Command.Add;
using Application.Features.Account.Command.Edit;
using Application.Features.Profile.Command.Edit;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.User
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AddAccountCommand, Domain.Entities.User.User>().ReverseMap();

            CreateMap<AddAccountCommand, AppUser>()
                    .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                    .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Image))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ReverseMap();
            CreateMap<EditAccountCommand, Domain.Entities.User.User>().ReverseMap();
            CreateMap<EditProfileCommand, Domain.Entities.User.User>().ReverseMap();
        }
    }
}
