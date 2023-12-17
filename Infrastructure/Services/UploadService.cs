using Application.Dtos.Requests;
using Application.Dtos.Responses.Upload;
using Application.Interfaces;
using Application.Shared;
using Domain.Constants;
using Domain.Wrappers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System.Text;

namespace Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICheckFileType _checkFileType;
        private readonly ICheckFileSize _checkFileSize;

        public UploadService(ICurrentUserService currentUserService, ICheckFileType checkFileType, ICheckFileSize checkFileSize)
        {
            _currentUserService = currentUserService;
            _checkFileType = checkFileType;
            _checkFileSize = checkFileSize;
        }

        public async Task<Result<List<UploadResponse>>> SplitVideoAsync(SplitRequest request)
        {
            if(request.NumberImagesEachSecond <= 0)
                return await Result<List<UploadResponse>>.FailAsync(StaticVariable.NUMBER_IMAGES_EACH_SECOND_MUST_BE_POSITIVE);

            List<UploadResponse> listImages = new();
            StringBuilder listMessages = new StringBuilder();

            if (request.FilePath != null)
                request.FilePath = TrimNonUrlCharacters(request.FilePath);

            var folderName = (request.FilePath != null) ? Path.Combine("Files", request.FilePath) : "Files";  //'Files/' + folder name
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);                       //Current path + '/Files/' + folder name

            foreach (var video in request.Files)
            {
                var checkFileIsVideo = _checkFileType.CheckFileIsVideo(video);
                var checkVideoSize = _checkFileSize.CheckVideoSize(video);
                if (checkFileIsVideo == "")
                {
                    if (checkVideoSize != "")
                    {
                        listMessages.Append(Environment.NewLine + checkVideoSize);
                        continue;
                    }
                }
                else
                {
                    listMessages.Append(Environment.NewLine + $"File {video.FileName} không phải là video hợp lệ!");
                    continue;
                }

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var videoName = TrimNonUrlCharacters(video.FileName);           //File name
                var path = Path.Combine(pathToSave, videoName);                 //File absolute path: Current path + '/Files/' + folder name + File name
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }

                var listImagesResult = new VideoSplitServiceUsingCommand().ExtractImagesFromVideoAndSave(path, folderName, Path.GetFileNameWithoutExtension(videoName), request.NumberImagesEachSecond);

                listImagesResult.ForEach(i => i.FileUrl = Path.Combine(_currentUserService.HostServerName, i.FilePath).Replace("\\", "/"));

                listImages.AddRange(listImagesResult);

                await DeleteAsync(path);
            }

            if (listMessages.Length > 0)
                return await Result<List<UploadResponse>>.SuccessAsync(listImages, listMessages.Remove(0, Environment.NewLine.Length).ToString().Split(Environment.NewLine).ToList());
            else
                return await Result<List<UploadResponse>>.SuccessAsync(listImages);
        }

        public async Task<Result<List<UploadResponse>>> UploadAsync(UploadRequest request)
        {
            List<UploadResponse> responses = new List<UploadResponse>();
            StringBuilder listMessages = new StringBuilder();

            if (request.FilePath != null)
                request.FilePath = TrimNonUrlCharacters(request.FilePath);

            var folderName = (request.FilePath != null) ? Path.Combine("Files", request.FilePath) : "Files";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            foreach (var file in request.Files)
            {
                var fileName = $"{Guid.NewGuid()}_{TrimNonUrlCharacters(file.FileName)}";
                var checkFileIsImage = _checkFileType.CheckFileIsImage(file);
                var checkFileIsVideo = _checkFileType.CheckFileIsVideo(file);
                var checkImageSize = _checkFileSize.CheckImageSize(file);
                var checkVideoSize = _checkFileSize.CheckVideoSize(file);
                if (checkFileIsImage == "" || checkFileIsVideo == "")
                {
                    if (checkFileIsImage == "")
                    {
                        if (checkImageSize != "")
                        {
                            listMessages.Append(Environment.NewLine + checkImageSize);
                            continue;
                        }
                    }
                    else if (checkVideoSize != "")
                    {
                        listMessages.Append(Environment.NewLine + checkVideoSize);
                        continue;
                    }
                }
                else
                {
                    listMessages.Append(Environment.NewLine + $"File {file.FileName} không phải là ảnh hoặc video hợp lệ!");
                    continue;
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

                responses.Add(new UploadResponse()
                {
                    FilePath = dbPath.Replace("\\", "/"),
                    FileUrl = Path.Combine(_currentUserService.HostServerName, dbPath).Replace("\\", "/")
                });
            }

            if (listMessages.Length > 0)
                return await Result<List<UploadResponse>>.SuccessAsync(responses, listMessages.Remove(0, Environment.NewLine.Length).ToString().Split(Environment.NewLine).ToList());
            else
                return await Result<List<UploadResponse>>.SuccessAsync(responses);
        }

        string TrimNonUrlCharacters(string input)
        {
            // Define a regular expression pattern to match characters not allowed in a URL
            string pattern = "^:?#%!$&'()*+,;=]$";
            foreach (char c in pattern)
                input = input.Replace(c.ToString(), "");

            while (input.Length > 0 && (input.StartsWith('/') || input.StartsWith('\\')))
                input = input.TrimStart('/').TrimStart('\\');

            while (input.Length > 0 && (input.EndsWith('/') || input.EndsWith('\\')))
                input = input.TrimEnd('/').TrimEnd('\\');

            return input;
        }

        DriveService CreateDriveService()
        {
            string ApplicationName = "CriminalManagement";

            GoogleCredential credential;

            string filePath = Directory.GetCurrentDirectory().Split("\\WebApi")[0] + "/Infrastructure/Services/exalted-pattern-400909-3eaa10f4b2b4.json";
            if (!Path.Exists(filePath))
                filePath = Directory.GetCurrentDirectory() + "/Services/exalted-pattern-400909-3eaa10f4b2b4.json";

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.Scope.Drive,
                });
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public bool UploadToGGDrive(long criminalId, List<string> filePaths)
        {
            var service = CreateDriveService();

            string? folderXId = FindFolderByName(service, criminalId.ToString(), StaticVariable.TRAINED_IMAGES_FOLDER_ID);
            if (folderXId == null)
                folderXId = CreateFolder(service, criminalId.ToString(), StaticVariable.TRAINED_IMAGES_FOLDER_ID);

            foreach (var filePath in filePaths)
            {
                if (!IsFileExists(filePath))
                    continue;

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),                              // The desired name for the file on Google Drive
                    Parents = new List<string> { folderXId },          // The ID of the target folder
                };

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var request = service.Files.Create(fileMetadata, fileStream, "image/png");
                    request.Fields = "id";
                    IUploadProgress progress = request.Upload();
                    if (UploadStatus.Failed.Equals(progress.Status))
                        return false;
                }
            }

            return true;
        }

        string? FindFolderByName(DriveService service, string folderName, string parentFolderId)
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Q = $"name='{folderName}' and '{parentFolderId}' in parents and mimeType='application/vnd.google-apps.folder'";
            listRequest.Fields = "files(id)";

            // Execute the request
            var result = listRequest.Execute();
            var files = result.Files;

            // Check if the folder exists
            if (files != null && files.Count > 0)
            {
                return files[0].Id;
            }

            return null; // Folder not found
        }

        string CreateFolder(DriveService service, string folderName, string parentId)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId },
            };

            var request = service.Files.Create(folderMetadata);
            request.Fields = "id";
            var folder = request.Execute();

            return folder.Id;
        }

        public bool IsFileExists(string? filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
                return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filePath));

            return false;
        }

        public string GetFullUrl(string? filePath)
        {
            if (IsFileExists(filePath))
            {
                var result = Path.Combine(_currentUserService.HostServerName, filePath!).Replace("\\", "/");
                return result;
            }

            return "";
        }

        public Task<Result<bool>> DeleteAsync(string filePath)
        {
            var fileToDelete = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (File.Exists(fileToDelete) && !filePath.Equals("Files/Avatar/NotFound/notFoundAvatar.jpg"))
            {
                File.Delete(fileToDelete);
            }

            return Result<bool>.SuccessAsync(true, ApplicationConstants.SuccessMessage.DeletedSuccess);
        }

        public Task<Result<bool>> DeleteRangeAsync(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
                DeleteAsync(filePath);

            return Result<bool>.SuccessAsync(true, ApplicationConstants.SuccessMessage.DeletedSuccess);
        }

        public void RemoveImageFromGGDrive(long criminalId, List<string> filePaths, bool removeParentFolder = false)
        {
            var service = CreateDriveService();

            if (removeParentFolder)
            {
                var folderId = GetFolderIdByPath(service, criminalId.ToString(), StaticVariable.TRAINED_IMAGES_FOLDER_ID);
                if (folderId != null)
                {
                    service.Files.Delete(folderId).Execute();
                }
            }
            else
            {
                foreach (var filePath in filePaths)
                {
                    var path = Path.Combine(criminalId.ToString(), Path.GetFileNameWithoutExtension(filePath));
                    var fileId = GetFileIdByPath(service, path, StaticVariable.TRAINED_IMAGES_FOLDER_ID);
                    if (fileId != null)
                    {
                        service.Files.Delete(fileId).Execute();
                    }
                }
            }
        }

        string? GetFileIdByPath(DriveService service, string filePath, string rootFolder)
        {
            string fileName = Path.GetFileName(filePath);
            string? folderPath = Path.GetDirectoryName(filePath);

            var request = service.Files.List();

            if (folderPath != null)
            {
                // Get the ID of the parent folder
                string? folderId = GetFolderIdByPath(service, folderPath, rootFolder);

                if (folderId != null)
                {
                    // Perform a file search by name and parent folder
                    request.Q = $"name = '{fileName}' and '{folderId}' in parents";
                }
                else
                    return null;
            }
            else
                request.Q = $"name = '{fileName}' and '{rootFolder}' in parents";

            var result = request.Execute();
            var file = result.Files.FirstOrDefault();

            return file?.Id;
        }

        string? GetFolderIdByPath(DriveService service, string folderPath, string rootFolder)
        {
            // Split the path into individual folder names
            string[] folderNames = folderPath.Split('/');

            // Iterate through each folder in the path and find its ID
            string currentFolderId = rootFolder;
            foreach (string folderName in folderNames)
            {
                var request = service.Files.List();
                request.Q = $"name = '{folderName}' and '{currentFolderId}' in parents";
                var result = request.Execute();
                var folder = result.Files.FirstOrDefault();

                if (folder != null)
                {
                    currentFolderId = folder.Id;
                }
                else
                {
                    // If any folder is not found, return null
                    return null;
                }
            }

            return currentFolderId;
        }
    }
}