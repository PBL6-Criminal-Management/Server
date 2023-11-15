using Application.Dtos.Responses.DetectResult;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IFaceDetectService
    {
        DetectResult FaceDetect(IFormFile file, bool enableSaveImage);
        string TrainImagesFromDir();
    }
}