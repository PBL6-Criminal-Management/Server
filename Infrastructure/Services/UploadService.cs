using Application.Dtos.Requests;
using Application.Dtos.Responses.Upload;
using Application.Interfaces;
using Application.Shared;
using Domain.Wrappers;

namespace Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly ICurrentUserService _currentUserService;

        public UploadService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<UploadResponse>>> UploadAsync(UploadRequest request)
        {
           List<UploadResponse> responses = new List<UploadResponse>();
           foreach(var file in request.Files)
           {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var folderName = (request.FilePath != null) ? Path.Combine("Files", request.FilePath) : "Files";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var isImage = CheckFilesIsImage(request);
                var isVideo = CheckFilesIsVideo(request);
                var isMaxSizeImage = CheckImageSize(request);
                var isMaxSizeVideo = CheckVideoSize(request);
                if (!isImage && !isVideo)
                {
                    return await Result<List<UploadResponse>>.FailAsync($"{file.FileName} is not an valid image or video");
                }
                if (!isMaxSizeImage && isImage)
                {
                    return await Result<List<UploadResponse>>.FailAsync($"{file.FileName} is over allowed image size(5MB) ");
                }
                if (!isMaxSizeVideo && isVideo)
                {
                    return await Result<List<UploadResponse>>.FailAsync($"{file.FileName} is over allowed video size(30MB)");
                }

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(dbPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                responses.Add( new UploadResponse()
                {
                    FilePath = dbPath,
                    FileUrl = Path.Combine(_currentUserService.HostServerName, dbPath).Replace("\\", "/")
                });
           }

            return await Result<List<UploadResponse>>.SuccessAsync(responses);
        }

        public string GetFullUrl(string? filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var result = _currentUserService.HostServerName + "/" + filePath;
                return result;
            }

            return "";
        }

        public Task<Result<bool>> DeleteAsync(string filePath)
        {
            var fileToDelete = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (File.Exists(fileToDelete))
            {
                File.Delete(fileToDelete);
            }
            return Result<bool>.SuccessAsync(true, ApplicationConstants.SuccessMessage.DeletedSuccess); ;
            //throw new ApiException(ApplicationConstants.ErrorMessage.NotFound);
        }

        public static bool CheckFilesIsImage(UploadRequest request)
        {
            if (request.Files != null)
            {
                foreach(var file in request.Files)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
                    if (!allowedExtensions.Contains(extension))
                    {
                        return false;
                    }

                    string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif" };
                    if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckFilesIsVideo(UploadRequest request)
        {
            if (request.Files != null)
            {
                foreach( var file in request.Files)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string[] allowedImagesExtensions = { ".mp3", ".mp4", ".mpeg" };
                    if (!allowedImagesExtensions.Contains(extension))
                    {
                        return false;
                    }

                    string[] allowedMimeTypes = { "video/mp3", "video/mp4", "video/mpeg" };
                    if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckImageSize(UploadRequest request)
        {
            foreach(var file in request.Files)
            {
                long fileSize = file.Length;
                long maxSizeImage = ICheckSizeFile.IMAGE_MAX_SIZE; //5MB  
                if (fileSize >= maxSizeImage)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckVideoSize(UploadRequest request)
        {
            foreach (var file in request.Files)
            {
                long fileSize = file.Length;
                long maxSizeVideo = ICheckSizeFile.VIDEO_MAX_SIZE; //30MB

                if (fileSize >= maxSizeVideo)
                {
                    return false;
                }
            }
            return true;
        }
    }
}