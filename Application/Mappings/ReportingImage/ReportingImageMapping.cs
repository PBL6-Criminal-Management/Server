using Application.Dtos.Requests.Image;
using AutoMapper;

namespace Application.Mappings.ReportingImage
{
    public class ReportingImageMappings : Profile
    {
        public ReportingImageMappings()
        {
            CreateMap<Domain.Entities.ReportingImage.ReportingImage, ImageRequest>().ReverseMap();
        }
    }
}