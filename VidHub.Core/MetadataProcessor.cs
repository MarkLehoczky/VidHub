using System.Diagnostics;
using System.Globalization;

namespace VidHub.Core
{
    internal class MetadataProcessor(string filePath)
    {
        public DateTime ExtractDate()
        {
            string date = RunSuccessfulProcess("ffprobe", "-v", "error", "-show_entries", "format_tags=creation_time", "-of", "default=noprint_wrappers=1:nokey=1", filePath);
            return DateTime.Parse(date.Trim(), CultureInfo.InvariantCulture);
        }

        public TimeSpan ExtractDuration()
        {
            string duration = RunSuccessfulProcess("ffprobe", "-v", "error", "-show_entries", "format=duration", "-of", "default=noprint_wrappers=1:nokey=1", filePath);
            return TimeSpan.FromSeconds(double.Parse(duration.Trim(), CultureInfo.InvariantCulture));
        }

        public string ExtractPreviewImage(string imageName, TimeSpan frame)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, imageName + ".jpg");

            _ = Directory.CreateDirectory(previewDirectory);

            _ = RunSuccessfulProcess("ffmpeg", "-v", "error", "-y", "-ss", frame.TotalSeconds.ToString(CultureInfo.InvariantCulture), "-i", filePath, "-frames:v", "1", previewPath);
            return previewPath;
        }


        private (int, string, string) RunProcess(string filename, params string[] arguments)
        {
            using Process process = new();
            process.StartInfo.FileName = filename;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            foreach (string arg in arguments)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            _ = process.Start();
            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            if (!process.WaitForExit(10000))
            {
                process.Kill();
                throw new TimeoutException($"Process `{filename} {string.Join(' ', arguments)}` timed out after {10000} ms.");
            }

            string output = outputTask.Result;
            string error = errorTask.Result;

            return (process.ExitCode, output, error);
        }

        private string RunSuccessfulProcess(string filename, params string[] arguments)
        {
            (int exitCode, string output, string error) = RunProcess(filename, arguments);
            return exitCode != 0 || !string.IsNullOrEmpty(error)
                ? throw new Exception($"Process `{filename} {string.Join(' ', arguments)}` failed with exit code {exitCode}. Error: {error}")
                : output;
        }
    }
}
