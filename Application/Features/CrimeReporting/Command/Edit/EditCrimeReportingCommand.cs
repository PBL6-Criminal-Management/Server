using System.ComponentModel.DataAnnotations;
using Application.Interfaces.CrimeReporting;
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CrimeReporting.Command.Edit
{
    public class EditCrimeReportingCommand : IRequest<Result<Domain.Entities.CrimeReporting.CrimeReporting>>
    {
        public long Id { get; set; }
        public ReportStatus Status { get; set; }
        [RegularExpression(@"^[\p{L}0-9,.: -]+$", ErrorMessage = StaticVariable.TITLE_CONTAINS_SPECIAL_CHARACTERS)]
        public string? Note { get; set; }
    }
    internal class EditCrimeReportingCommandHandler : IRequestHandler<EditCrimeReportingCommand, Result<Domain.Entities.CrimeReporting.CrimeReporting>>
    {
        private readonly ICrimeReportingRepository _crimeReportingRepository;
        private readonly IUnitOfWork<long> _unitOfWork;
        public EditCrimeReportingCommandHandler(ICrimeReportingRepository crimeReportingRepository,
           IUnitOfWork<long> unitOfWork)
        {
            _crimeReportingRepository = crimeReportingRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Domain.Entities.CrimeReporting.CrimeReporting>> Handle(EditCrimeReportingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return await Result<Domain.Entities.CrimeReporting.CrimeReporting>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var editReport = await _crimeReportingRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (editReport == null) return await Result<Domain.Entities.CrimeReporting.CrimeReporting>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            var executionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    editReport.Status = request.Status;
                    editReport.Note = request.Note;
                    await _crimeReportingRepository.UpdateAsync(editReport);
                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<Domain.Entities.CrimeReporting.CrimeReporting>.SuccessAsync(editReport);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<Domain.Entities.CrimeReporting.CrimeReporting>.FailAsync(ex.Message);
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