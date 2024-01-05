using Application.Dtos.Requests.Image;
using AutoMapper;

namespace Application.Mappings.CriminalImage
{
    public class CriminalImageMappings : Profile
    {
        public CriminalImageMappings() {
            CreateMap<Domain.Entities.CriminalImage.CriminalImage, ImageRequest>().ReverseMap();
        }
    }
}
