using Application.Dtos.Requests;
using Application.Dtos.Responses.Upload;
using Application.Interfaces;
using Application.Shared;
using Domain.Constants;
using Domain.Wrappers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
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

        public async Task<Result<List<UploadResponse>>> UploadAsync(UploadRequest request)
        {
            List<UploadResponse> responses = new List<UploadResponse>();
            List<UploadFile> listFiles = new List<UploadFile>();
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

                listFiles.Add(new UploadFile { File = file, FileName = fileName });
            }

            //Only upload criminal images to gg drive
            if (request.FilePath != null && request.FilePath.Split("/").Count() == 2)
            {
                string rootFolder = request.FilePath.Split("/")[0];
                if (rootFolder.Equals(StaticVariable.TRAINED_IMAGES_FOLDER_NAME) && int.TryParse(request.FilePath.Split("/")[1], out int criminalId))
                {
                    UploadToGGDrive(listFiles, criminalId.ToString());
                }
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
                filePath = Directory.GetCurrentDirectory() + "/exalted-pattern-400909-3eaa10f4b2b4.json";

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.Scope.DriveFile,
                });
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        void UploadToGGDrive(List<UploadFile> files, string criminalId)
        {
            var service = CreateDriveService();

            string? folderXId = FindFolderByName(service, criminalId, StaticVariable.TRAINED_IMAGES_FOLDER_ID);
            if (folderXId == null)
                folderXId = CreateFolder(service, criminalId, StaticVariable.TRAINED_IMAGES_FOLDER_ID);

            foreach (UploadFile file in files)
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = file.FileName, // The desired name for the file on Google Drive
                    Parents = new List<string> { folderXId }, // The ID of the target folder
                };

                using (var stream = file.File.OpenReadStream())
                {
                    var request = service.Files.Create(fileMetadata, stream, "image/png");
                    request.Fields = "id";
                    request.Upload();
                }
            }
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

            if (File.Exists(fileToDelete) && !fileToDelete.Equals("Files/Avatar/NotFound/notFoundAvatar.jpg"))
            {
                File.Delete(fileToDelete);
            }

            //Remove image on gg drive if it belongs to Criminal
            //Files/StaticVariable.TRAINED_IMAGES_FOLDER_NAME/<CriminalId>/<File_name.png>
            if (filePath != null && filePath.Split("/").Count() == 4)
            {
                string rootFolder = filePath.Split("/")[1];
                if (rootFolder.Equals(StaticVariable.TRAINED_IMAGES_FOLDER_NAME) && int.TryParse(filePath.Split("/")[2], out _))
                {
                    RemoveImageFromGGDrive(CreateDriveService(), string.Join("/", filePath.Split("/").TakeLast(2)));
                }
            }

            return Result<bool>.SuccessAsync(true, ApplicationConstants.SuccessMessage.DeletedSuccess);
        }

        void RemoveImageFromGGDrive(DriveService service, string filePath)
        {
            var fileId = GetFileIdByPath(service, filePath);
            if (fileId != null)
            {
                service.Files.Delete(fileId).Execute();
            }
        }

        string? GetFileIdByPath(DriveService service, string filePath)
        {
            string fileName = Path.GetFileName(filePath)!;
            string folderPath = Path.GetDirectoryName(filePath)!;

            // Get the ID of the parent folder
            string? folderId = GetFolderIdByPath(service, folderPath);

            if (folderId != null)
            {
                // Perform a file search by name and parent folder
                var request = service.Files.List();
                request.Q = $"name = '{fileName}' and '{folderId}' in parents";
                var result = request.Execute();
                var file = result.Files.FirstOrDefault();

                return file?.Id;
            }

            return null;
        }

        string? GetFolderIdByPath(DriveService service, string folderPath)
        {
            // Split the path into individual folder names
            string[] folderNames = folderPath.Split('/');

            // Iterate through each folder in the path and find its ID
            string currentFolderId = StaticVariable.TRAINED_IMAGES_FOLDER_ID;
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

    class UploadFile
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}