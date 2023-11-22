using Application.Interfaces;
using Application.Dtos.Requests;
using Microsoft.AspNetCore.Http;
using Domain.Constants;

namespace Infrastructure.Services
{
    public class CheckFileType : ICheckFileType
    {
        public string CheckFilesIsImage(CheckImagesTypeRequest request)
        {
            foreach (IFormFile file in request.Files)
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
                    if (!allowedExtensions.Contains(extension))
                    {
                        return StaticVariable.FILE_IS_NOT_IMAGE + $" (Chỉ chấp nhận: {String.Join(", ", allowedExtensions)})";
                    }

                    string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif" };
                    if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    {
                        return StaticVariable.FILE_TYPE_IS_INVALID;
                    }
                }
            }
            return "";
        }

        string ICheckFileType.CheckFilesIsVideo(CheckVideoTypeRequest request)
        {
            foreach (IFormFile file in request.Files)
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string[] allowedVideosExtensions = { ".mp3", ".mp4", ".mpeg" };
                    if (!allowedVideosExtensions.Contains(extension))
                    {
                        return StaticVariable.FILE_IS_NOT_VIDEO + $" (Chỉ chấp nhận: {String.Join(", ", allowedVideosExtensions)})";
                    }

                    string[] allowedMimeTypes = { "video/mp3", "video/mp4", "video/mpeg" };
                    if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    {
                        return StaticVariable.FILE_TYPE_IS_INVALID;
                    }
                }
            }
            return "";
        }

    }
}