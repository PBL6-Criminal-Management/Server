using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CheckFileType : ICheckFileType
    {
        public string CheckFileIsImage(IFormFile file)
        {
            string[] allowedExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
            if (!ValidateFileType(file, allowedExtensions, true))
            {
                return $"{file.FileName} không phải là ảnh hợp lệ! (Chỉ chấp nhận: {String.Join(", ", allowedExtensions)})";
            }

            string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                return $"Kiểu ảnh {file.FileName} không hợp lệ";
            }
            return "";
        }

        public string CheckFileIsVideo(IFormFile file)
        {
            string[] allowedVideosExtensions = { ".mp3", ".mp4", ".mpeg" };
            if (!ValidateFileType(file, allowedVideosExtensions, false))
            {
                return $"{file.FileName} không phải là video hợp lệ! (Chỉ chấp nhận: {String.Join(", ", allowedVideosExtensions)})";
            }

            string[] allowedMimeTypes = { "video/mp3", "video/mp4", "video/mpeg" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                return $"Kiểu video {file.FileName} không hợp lệ";
            }
            return "";
        }

        private static bool ValidateFileType(IFormFile file, string[] allowedExtensions, bool isImage)
        {
            // Check if the file is not null and has content
            if (file != null && file.Length > 0)
            {
                // Read the first N bytes from the file
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                // Perform validation based on the allowed file extensions
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (allowedExtensions.Contains(fileExtension))
                {
                    if (isImage)
                    {
                        //check some first bytes
                        return IsImageFile(fileBytes);
                    }
                    else
                    {
                        //check some first bytes
                        return IsVideoFile(fileBytes);
                    }
                }
            }

            return false;
        }

        private static bool IsImageFile(byte[] fileBytes)
        {
            // Check for common image formats based on magic numbers or file signatures
            return IsJpeg(fileBytes) || IsPng(fileBytes) || IsGif(fileBytes);
        }

        private static bool IsVideoFile(byte[] fileBytes)
        {
            // Check for common video formats based on magic numbers or file signatures
            return IsMp4(fileBytes) || IsMpeg(fileBytes);
        }

        private static bool IsJpeg(byte[] fileBytes)
        {
            return fileBytes.Length >= 2 && fileBytes[0] == 0xFF && fileBytes[1] == 0xD8;
        }

        private static bool IsPng(byte[] fileBytes)
        {
            return fileBytes.Length >= 8 &&
                    fileBytes[0] == 0x89 && fileBytes[1] == 0x50 &&
                    fileBytes[2] == 0x4E && fileBytes[3] == 0x47 &&
                    fileBytes[4] == 0x0D && fileBytes[5] == 0x0A &&
                    fileBytes[6] == 0x1A && fileBytes[7] == 0x0A;
        }

        private static bool IsGif(byte[] fileBytes)
        {
            return fileBytes.Length >= 6 &&
                    fileBytes[0] == 0x47 && fileBytes[1] == 0x49 &&
                    fileBytes[2] == 0x46 && fileBytes[3] == 0x38 &&
                    (fileBytes[4] == 0x37 || fileBytes[4] == 0x39) &&
                    fileBytes[5] == 0x61;
        }

        private static bool IsMp4(byte[] fileBytes)
        {
            return fileBytes.Length >= 8 &&
                    fileBytes[4] == 0x66 && fileBytes[5] == 0x74 &&
                    fileBytes[6] == 0x79 && fileBytes[7] == 0x70;
        }

        private static bool IsMpeg(byte[] fileBytes)
        {
            // Add logic to check for the MPEG magic number or other characteristics
            // You may need to consult the specifications for MPEG files or use a library like FFmpeg.
            // This is a simplified example and may not cover all cases.
            return fileBytes.Length >= 3 &&
                    fileBytes[0] == 0x00 && fileBytes[1] == 0x00 &&
                    (fileBytes[2] == 0x01 || fileBytes[2] == 0xBA);
        }
    }

}