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

            if(request.FilePath != null)
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
                    FilePath = dbPath.Replace("/", "\\"),
                    FileUrl = Path.Combine(_currentUserService.HostServerName, dbPath).Replace("\\", "/")
                });

                listFiles.Add(new UploadFile { File = file, FileName = fileName});
            }

            //Only upload criminal images to gg drive
            if(request.FilePath != null && request.FilePath.Split("/").Count() == 2)
            {
                string rootFolder = request.FilePath.Split("/")[0];
                if(rootFolder.Equals(StaticVariable.TRAINED_IMAGES_FOLDER_NAME) && int.TryParse(request.FilePath.Split("/")[1], out int criminalId))
                {
                    UploadToGGDrive(listFiles, criminalId.ToString());
                }
            }

            if(listMessages.Length > 0)
                return await Result<List<UploadResponse>>.SuccessAsync(responses, listMessages.Remove(0, Environment.NewLine.Length).ToString().Split(Environment.NewLine).ToList());
            else
                return await Result<List<UploadResponse>>.SuccessAsync(responses);
        }

        static string TrimNonUrlCharacters(string input)
        {
            // Define a regular expression pattern to match characters not allowed in a URL
            string pattern = "^:?#%!$&'()*+,;=]$";
            foreach (char c in pattern)
                input = input.Replace(c.ToString(), "");

            while(input.Length > 0 && (input.StartsWith('/') || input.StartsWith('\\')))
                input = input.TrimStart('/').TrimStart('\\');

            while(input.Length > 0 && (input.EndsWith('/') || input.EndsWith('\\')))
                input = input.TrimEnd('/').TrimEnd('\\');

            return input;
        }

        private void UploadToGGDrive(List<UploadFile> files, string criminalId)
        {
            string ApplicationName = "CriminalManagement";

            GoogleCredential credential;

            using(var stream = new FileStream("../Infrastructure/Services/exalted-pattern-400909-3eaa10f4b2b4.json", FileMode.Open, FileAccess.Read))
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

            string? folderXId = FindFolderByName(service, criminalId, StaticVariable.TRAINED_IMAGES_FOLDER_ID);
            if (folderXId == null)
                folderXId = CreateFolder(service, criminalId, StaticVariable.TRAINED_IMAGES_FOLDER_ID);

            foreach(UploadFile file in files)
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

        public string GetFullUrl(string? filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var result = Path.Combine(_currentUserService.HostServerName, filePath).Replace("\\", "/");
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
            return Result<bool>.SuccessAsync(true, ApplicationConstants.SuccessMessage.DeletedSuccess);
        }       
    }

    class UploadFile
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}