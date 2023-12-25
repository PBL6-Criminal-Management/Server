using System.ComponentModel.DataAnnotations;
using Application.Dtos.Requests.Image;
using Application.Hubs.Notification;
using Application.Interfaces.CrimeReporting;
using Application.Interfaces.ReportingImage;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Notification;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CrimeReporting.Command.Add
{
    public class AddCrimeReportingCommand : IRequest<Result<AddCrimeReportingCommand>>
    {
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_REPORTER_NAME)]
        [RegularExpression(@"^[\p{L} ']+$", ErrorMessage = StaticVariable.NAME_CONTAINS_VALID_CHARACTER)]
        public string ReporterName { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_REPORTER_EMAIL)]
        [EmailAddress(ErrorMessage = StaticVariable.INVALID_EMAIL)]
        public string? ReporterEmail { get; set; }
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_REPORTER_PHONE)]
        public string ReporterPhone { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_REPORTER_ADDRESS)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.ADDRESS_VALID_CHARACTER)]
        public string ReporterAddress { get; set; } = null!;
        public string Content { get; set; } = null!;
        public ReportStatus Status { get; set; }
        public string? Note { get; set; }
        public List<ImageRequest>? ReportingImages { get; set; }
    }
    internal class AddCrimeReportingCommandHandler : IRequestHandler<AddCrimeReportingCommand, Result<AddCrimeReportingCommand>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICrimeReportingRepository _crimeReportingRepository;
        private readonly IReportingImageRepository _reportingImageRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationService> _hubContext;
        public AddCrimeReportingCommandHandler(IUnitOfWork<long> unitOfWork, ICrimeReportingRepository crimeReportingRepository,
            IReportingImageRepository reportingImageRepository, IHubContext<NotificationService> hubContext,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _crimeReportingRepository = crimeReportingRepository;
            _reportingImageRepository = reportingImageRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }
        public async Task<Result<AddCrimeReportingCommand>> Handle(AddCrimeReportingCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var addReport = _mapper.Map<Domain.Entities.CrimeReporting.CrimeReporting>(request);
                    await _crimeReportingRepository.AddAsync(addReport);
                    await _unitOfWork.Commit(cancellationToken);
                    if (request.ReportingImages != null)
                    {
                        var addImage = _mapper.Map<List<Domain.Entities.ReportingImage.ReportingImage>>(request.ReportingImages);
                        addImage.ForEach(x => x.ReportingId = addReport.Id);
                        await _reportingImageRepository.AddRangeAsync(addImage);
                    }
                    await transaction.CommitAsync(cancellationToken);
                    await _hubContext.Clients.All.SendAsync("Thông báo : ", addReport);
                    return await Result<AddCrimeReportingCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<AddCrimeReportingCommand>.FailAsync(ex.Message);
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });
            return result;
        }
    }
}