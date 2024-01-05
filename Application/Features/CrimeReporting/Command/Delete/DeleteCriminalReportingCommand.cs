using Application.Interfaces;
using Application.Interfaces.CrimeReporting;
using Application.Interfaces.ReportingImage;
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CrimeReporting.Command.Delete
{
    public class DeleteCrimeReportingCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }
    }
    internal class DeleteCrimeReportingCommandHandler : IRequestHandler<DeleteCrimeReportingCommand, Result<long>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICrimeReportingRepository _crimeReportingRepository;
        private readonly IReportingImageRepository _reportingImageRepository;
        private readonly IUploadService _uploadService;
        public DeleteCrimeReportingCommandHandler(IUnitOfWork<long> unitOfWork, ICrimeReportingRepository crimeReportingRepository,
            IReportingImageRepository reportingImageRepository, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _crimeReportingRepository = crimeReportingRepository;
            _reportingImageRepository = reportingImageRepository;
            _uploadService = uploadService;
        }
        public async Task<Result<long>> Handle(DeleteCrimeReportingCommand request, CancellationToken cancellationToken)
        {
            var reportDelete = _crimeReportingRepository.Entities.Where(_ => _.Id == request.Id && !_.IsDeleted).FirstOrDefault();
            if (reportDelete == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _crimeReportingRepository.DeleteAsync(reportDelete);
                    var reportImage = await _reportingImageRepository.Entities.Where(_ => _.ReportingId == request.Id && !_.IsDeleted).ToListAsync();
                    await _reportingImageRepository.RemoveRangeAsync(reportImage);
                    foreach (var image in reportImage)
                    {
                        var check = await _uploadService.DeleteAsync(image.FilePath);
                        if (check.Succeeded == false)
                        {
                            return await Result<long>.FailAsync(StaticVariable.ERROR_DELETE_IMAGE);
                        }
                    }
                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_REPORT);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<long>.FailAsync(ex.Message);
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