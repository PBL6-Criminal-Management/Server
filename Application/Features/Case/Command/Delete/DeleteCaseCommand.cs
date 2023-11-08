using Application.Interfaces;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseImage;
using Application.Interfaces.Criminal;
using Application.Interfaces.Evidence;
using Application.Interfaces.Repositories;
using Application.Interfaces.Witness;
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
        private readonly IWitnessRepository _witnessRepository;
        private readonly IUploadService _uploadService;
        public DeleteCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IEvidenceRepository evidenceRepository, IWitnessRepository witnessRepository,
            IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseImageRepository = caseImageRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _evidenceRepository = evidenceRepository;
            _witnessRepository = witnessRepository;
            _uploadService = uploadService;
        }
        public async Task<Result<long>> Handle(DeleteCaseCommand request, CancellationToken cancellationToken)
        {
            var caseDelete = await _caseRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (caseDelete == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            try
            {
                await _caseRepository.DeleteAsync(caseDelete);
                var caseCriminal = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                await _caseCriminalRepository.DeleteRange(caseCriminal);
                var caseImage = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                await _caseImageRepository.DeleteRange(caseImage);
                foreach (var image in caseImage)
                {
                    var check = await _uploadService.DeleteAsync(image.FilePath);
                    if (check.Succeeded == false)
                    {
                        return await Result<long>.FailAsync(StaticVariable.ERROR_DELETE_IMAGE);
                    }
                }
                var evidence = await _evidenceRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                await _evidenceRepository.DeleteRange(evidence);
                var witness = await _witnessRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                await _witnessRepository.DeleteRange(witness);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_CASE);
            }
            catch (System.Exception ex)
            {
                return await Result<long>.FailAsync(ex.Message);
            }
        }
    }
}