using Application.Dtos.Responses.Upload;
using System.Diagnostics;
using Domain.Constants;

namespace Infrastructure.Services
{
    public class VideoSplitServiceUsingCommand
    {
        public class SplitedFile
        {
            public string FileName { get; set; } = null!;
            public byte[] FileData { get; set; } = null!;
            public long FileLength { get; set; }
        }
        public List<UploadResponse> ExtractImagesFromVideoAndSave(string path, string folderName, string videoName, int? frameNumbersEachSecond = StaticVariable.DEFAULT_FRAMES_NUMBER_EACH_SECOND)
        {
            List<UploadResponse> responses = new List<UploadResponse>();

            string firstPath = Path.Combine(folderName, Guid.NewGuid() + "_" + videoName);

            RunFFmpegCommand($"-i \"{path}\" -vf fps={frameNumbersEachSecond} \"{firstPath + "_%05d.jpg"}\"");

            int imageCount = Directory.GetFiles(folderName).Where(fileName => fileName.StartsWith(firstPath)).Count();

            for (int i = 1; i <= imageCount; i++)
            {
                string filename = string.Format(firstPath + "_{0:D5}.jpg", i);
                responses.Add(new UploadResponse
                {
                    FilePath = filename.Replace("\\", "/"),
                    FileUrl = ""
                });
            }

            return responses;
        }

        void RunFFmpegCommand(string command)
        {
            //FFmpeg not support MacOS and Android
            OperatingSystem os = Environment.OSVersion;
            var ffmpegPath = os.Platform == PlatformID.Win32NT ? @".\ffmpeg\x86_64\ffmpeg.exe" : @".\ffmpeg\x86_64\ffmpeg";
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = command,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            })
            {
                process.Start();

                // Wait for the process to exit
                process.WaitForExit();
            }
        }
    }
}
