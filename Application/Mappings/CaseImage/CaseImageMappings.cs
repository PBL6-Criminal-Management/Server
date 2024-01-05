using Application.Dtos.Requests.Image;
using AutoMapper;

namespace Application.Mappings.CaseImage
{
    public class CaseImageMappings : Profile
    {
        public CaseImageMappings()
        {
            CreateMap<Domain.Entities.CaseImage.CaseImage, ImageRequest>().ReverseMap();
        }
    }
}
