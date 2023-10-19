using Application.Interfaces.Account;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;

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
            var account = await _accountRepository.FindAsync(a => a.Id == request.Id && !a.IsDeleted);
            if (account == null) return await Result<long>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            try
            {
                await _userService.DeleteUser(new Dtos.Requests.Identity.DeleteUserRequest
                {
                    Id = request.Id,
                });
                await _accountRepository.DeleteAsync(account);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<long>.SuccessAsync(request.Id, $"Xóa tài khoản có id {request.Id} thành công");
            }catch (System.Exception ex)
            {
                return await Result<long>.FailAsync(ex.Message);
            }
        }
    }
}
