using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseImage;
using Application.Interfaces.CaseInvestigator;
using Application.Interfaces.CaseVictim;
using Application.Interfaces.CaseWitness;
using Application.Interfaces.Evidence;
using Application.Interfaces.Repositories;
using Application.Interfaces.WantedCriminal;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Case.Command.Delete
{
    public class DeleteCaseCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }
    }
    internal class DeleteCaseCommandHandler : IRequestHandler<DeleteCaseCommand, Result<long>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseImageRepository _caseImageRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly ICaseWitnessRepository _caseWitnessRepository;
        private readonly ICaseInvestigatorRepository _caseInvestigatorRepository;
        private readonly ICaseVictimRepository _caseVictimRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly IUploadService _uploadService;
        public DeleteCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IEvidenceRepository evidenceRepository, ICaseWitnessRepository caseWitnessRepository,
            IUploadService uploadService, ICaseInvestigatorRepository caseInvestigatorRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICaseVictimRepository caseVictimRepository)
        {
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseImageRepository = caseImageRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _evidenceRepository = evidenceRepository;
            _caseWitnessRepository = caseWitnessRepository;
            _caseInvestigatorRepository = caseInvestigatorRepository;
            _caseVictimRepository = caseVictimRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _uploadService = uploadService;
        }
        public async Task<Result<long>> Handle(DeleteCaseCommand request, CancellationToken cancellationToken)
        {
            var caseDelete = await _caseRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (caseDelete == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _caseRepository.DeleteAsync(caseDelete);
                    var caseCriminal = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _caseCriminalRepository.DeleteRange(caseCriminal);
                    var caseImage = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _caseImageRepository.DeleteRange(caseImage);
                    var evidence = await _evidenceRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _evidenceRepository.DeleteRange(evidence);
                    var caseWitness = await _caseWitnessRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _caseWitnessRepository.DeleteRange(caseWitness);
                    var caseInvestigator = await _caseInvestigatorRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _caseInvestigatorRepository.DeleteRange(caseInvestigator);
                    var caseVictim = await _caseVictimRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _caseVictimRepository.DeleteRange(caseVictim);
                    var wantedCriminals = await _wantedCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    await _wantedCriminalRepository.DeleteRange(wantedCriminals);

                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    foreach (var image in caseImage)
                    {
                        var check = await _uploadService.DeleteAsync(image.FilePath);
                        if (check.Succeeded == false)
                        {
                            return await Result<long>.FailAsync(StaticVariable.ERROR_DELETE_IMAGE);
                        }
                    }
                    return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_CASE);
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