using Application.Dtos.Responses.File;
using Application.Interfaces;
using Application.Interfaces.CrimeReporting;
using Application.Interfaces.ReportingImage;
using AutoMapper;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CrimeReporting.Queries.GetById
{
    public class GetCrimeReportingByIdQuery : IRequest<Result<GetCrimeReportingByIdResponse>>
    {
        public long Id { get; set; }
    }
    internal class GetCrimeReportingByIdQueryHandler : IRequestHandler<GetCrimeReportingByIdQuery, Result<GetCrimeReportingByIdResponse>>
    {
        private readonly ICrimeReportingRepository _crimeReportingRepository;
        private readonly IReportingImageRepository _reportingImageRepository;
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;
        public GetCrimeReportingByIdQueryHandler(ICrimeReportingRepository crimeReportingRepository, IReportingImageRepository reportingImageRepository,
            IUploadService uploadService, IMapper mapper)
        {
            _crimeReportingRepository = crimeReportingRepository;
            _reportingImageRepository = reportingImageRepository;
            _uploadService = uploadService;
            _mapper = mapper;
        }
        public async Task<Result<GetCrimeReportingByIdResponse>> Handle(GetCrimeReportingByIdQuery request, CancellationToken cancellationToken)
        {
            var reportCrime = _crimeReportingRepository.Entities.Where(_ => _.Id == request.Id && !_.IsDeleted).FirstOrDefault();
            if (reportCrime == null)
                return await Result<GetCrimeReportingByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            var response = _mapper.Map<GetCrimeReportingByIdResponse>(reportCrime);
            var reportingImage = await _reportingImageRepository.Entities.Where(_ => _.ReportingId == request.Id && !_.IsDeleted).ToListAsync();
            response.ReportingImages = reportingImage.Select(i => new FileResponse
            {
                FileName = i.FileName,
                FileUrl = _uploadService.GetFullUrl(i.FilePath)
            }).ToList();
            return await Result<GetCrimeReportingByIdResponse>.SuccessAsync(response);
        }
    }
}