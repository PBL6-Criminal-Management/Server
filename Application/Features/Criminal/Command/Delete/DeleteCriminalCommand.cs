using Application.Interfaces.CaseCriminal;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Repositories;
using Application.Interfaces.WantedCriminal;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;

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
        private readonly IUnitOfWork<long> _unitOfWork;

        public DeleteCriminalCommandHandler(
            ICriminalRepository criminalRepository,
            ICaseCriminalRepository caseCriminalRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            ICriminalImageRepository criminalImageRepository,
            IUnitOfWork<long> unitOfWork
        )
        {
            _criminalRepository = criminalRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _criminalImageRepository = criminalImageRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<long>> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
        {
            var criminal = await _criminalRepository.FindAsync(a => a.Id == request.Id && !a.IsDeleted);
            if (criminal == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            await _criminalRepository.DeleteAsync(criminal);

            var caseCriminal = _caseCriminalRepository.Entities.Where(a => a.CriminalId == request.Id && !a.IsDeleted);
            if (caseCriminal != null) 
                await _caseCriminalRepository.DeleteRange(caseCriminal.ToList());

            var wantedCriminal = await _wantedCriminalRepository.FindAsync(a => a.CriminalId == request.Id && !a.IsDeleted);
            if (wantedCriminal != null) 
                await _wantedCriminalRepository.DeleteAsync(wantedCriminal);

            var criminalImages = _criminalImageRepository.Entities.Where(a => a.CriminalId == request.Id && !a.IsDeleted);
            if (criminalImages != null) 
                await _criminalImageRepository.DeleteRange(criminalImages.ToList());

            await _unitOfWork.Commit(cancellationToken);
            return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_SUCCESS);
        }
    }
}
