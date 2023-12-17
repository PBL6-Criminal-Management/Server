using Application.Dtos.Requests;
using Application.Dtos.Responses.Upload;
using Domain.Wrappers;

namespace Application.Interfaces
{
    public interface IUploadService
    {
        Task<Result<List<UploadResponse>>> SplitVideoAsync(SplitRequest request);
        Task<Result<List<UploadResponse>>> UploadAsync(UploadRequest request);
        bool UploadToGGDrive(long criminalId, List<string> filePaths);
        bool IsFileExists(string? filePath);
        string GetFullUrl(string? filePath);
        Task<Result<bool>> DeleteAsync(string filePath);
        Task<Result<bool>> DeleteRangeAsync(List<string> filePaths);
        void RemoveImageFromGGDrive(long criminalId, List<string> filePaths, bool removeParentFolder = false);
    }
}