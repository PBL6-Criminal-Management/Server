using Application.Interfaces.Services;
using Domain.Wrappers;
using MediatR;

namespace Application.Features.FaceDetect.Command.Retrain
{
    public class RetrainCommand : IRequest<Result<string>>
    {
    }
    internal class RetrainCommandHandler : IRequestHandler<RetrainCommand, Result<string>>
    {
        private readonly IFaceDetectService _faceDetectService;

        public RetrainCommandHandler(IFaceDetectService faceDetectService)
        {
            _faceDetectService = faceDetectService;
        }

        public async Task<Result<string>> Handle(RetrainCommand request, CancellationToken cancellationToken)
        {
            return await Result<string>.SuccessAsync(_faceDetectService.TrainImagesFromDir());
        }
    }
}
