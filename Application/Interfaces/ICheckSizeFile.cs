using Application.Dtos.Requests;

namespace Application.Interfaces
{
    public interface ICheckSizeFile
    {
        const long IMAGE_MAX_SIZE = 50 * 1024 * 1024;
        const long VIDEO_MAX_SIZE = 100 * 1024 * 1024;
        string CheckImageSize(CheckImageSizeRequest request);
        string CheckVideoSize(CheckVideoSizeRequest request);
    }
}