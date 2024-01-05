using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Account.Command.Delete
{
    public class DeleteAccountCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }
    }
    internal class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result<long>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork<long> _unitOfWork;

        public DeleteAccountCommandHandler(IAccountRepository accountRepository,
            IUserService userService, IUnitOfWork<long> unitOfWork)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<long>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _accountRepository.Entities.Where(a => a.Id == request.Id && !a.IsDeleted).FirstOrDefault();
            if (account == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _userService.DeleteUser(new Dtos.Requests.Identity.DeleteUserRequest
                    {
                        Id = request.Id,
                    });
                    await _accountRepository.DeleteAsync(account);
                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<long>.SuccessAsync(request.Id, StaticVariable.DELETE_USER);
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
