using Application.Interfaces;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Repositories;
using Application.Interfaces.WantedCriminal;
using Domain.Constants;
using Domain.Wrappers;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Criminal.Command.Delete
{
    public class DeleteCriminalCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }
    }
    internal class DeleteCriminalCommandHandler : IRequestHandler<DeleteCriminalCommand, Result<long>>
    {
        private readonly ICriminalRepository _criminalRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly ICriminalImageRepository _criminalImageRepository;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public DeleteCriminalCommandHandler(
            ICriminalRepository criminalRepository,
            ICaseCriminalRepository caseCriminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IUploadService uploadService,
            IUnitOfWork<long> unitOfWork,
            IBackgroundJobClient backgroundJobClient
        )
        {
            _criminalRepository = criminalRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _uploadService = uploadService;
            _unitOfWork = unitOfWork;
            _backgroundJobClient = backgroundJobClient;
        }
        public async Task<Result<long>> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
        {
            var criminal = await _criminalRepository.FindAsync(a => a.Id == request.Id && !a.IsDeleted);
            if (criminal == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _criminalRepository.DeleteAsync(criminal);

                    var caseCriminal = _caseCriminalRepository.Entities.Where(a => a.CriminalId == request.Id && !a.IsDeleted);
                    if (caseCriminal != null)
                        await _caseCriminalRepository.DeleteRange(caseCriminal.ToList());

                    var wantedCriminal = await _wantedCriminalRepository.FindAsync(a => a.CriminalId == request.Id && !a.IsDeleted);
                    if (wantedCriminal != null)
                        await _wantedCriminalRepository.DeleteAsync(wantedCriminal);

                    var criminalImages = _criminalImageRepository.Entities.Where(a => a.CriminalId == request.Id && !a.IsDeleted);
                    if (criminalImages != null)
                    {
                        await _criminalImageRepository.RemoveRangeAsync(criminalImages.ToList());
                        await _uploadService.DeleteRangeAsync(criminalImages.Select(i => i.FilePath).ToList());
                    }
                    if(criminal.Avatar != null)
                        await _uploadService.DeleteAsync(criminal.Avatar);

                    _backgroundJobClient.Enqueue(() => _uploadService.RemoveImageFromGGDrive(request.Id, new List<string>(), true));

                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_SUCCESS);
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
