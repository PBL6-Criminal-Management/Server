using Application.Features.CrimeReporting.Command.Add;
using Application.Features.CrimeReporting.Queries.GetById;
using AutoMapper;

namespace Application.Mappings.CrimeReporting
{
    public class CrimeReportingMappings : Profile
    {
        public CrimeReportingMappings()
        {
            CreateMap<AddCrimeReportingCommand, Domain.Entities.CrimeReporting.CrimeReporting>()
                .ForMember(dest => dest.ReportingImages, otp => otp.Ignore())
                .ReverseMap();
            CreateMap<GetCrimeReportingByIdResponse, Domain.Entities.CrimeReporting.CrimeReporting>()
                .ReverseMap()
                .ForMember(dest => dest.ReportingImages, otp => otp.Ignore());
        }
    }
}