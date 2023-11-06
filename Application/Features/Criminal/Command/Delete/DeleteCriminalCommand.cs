using Application.Interfaces.Criminal;
using Application.Interfaces.Repositories;
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
        private readonly IUnitOfWork<long> _unitOfWork;

        public DeleteCriminalCommandHandler(ICriminalRepository criminalRepository, IUnitOfWork<long> unitOfWork)
        {
            _criminalRepository = criminalRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<long>> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
        {
            var criminal = await _criminalRepository.FindAsync(a => a.Id == request.Id && !a.IsDeleted);
            if (criminal == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            await _criminalRepository.DeleteAsync(criminal);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_USER);
        }
    }
}
