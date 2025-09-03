using System.Diagnostics;
using System.Globalization;

namespace VidHub.Core
{
    internal class MetadataProcessor(string filePath)
    {
        public string FilePath { get; set; } = filePath;
        public int Timeout { get; set; } = 10000;


        public DateTime ExtractDate()
        {
            string date = RunSuccessfulProcess("ffprobe", "-v", "error", "-show_entries", "format_tags=creation_time", "-of", "default=noprint_wrappers=1:nokey=1", FilePath);
            return DateTime.Parse(date.Trim(), CultureInfo.InvariantCulture);
        }

        public TimeSpan ExtractDuration()
        {
            string duration = RunSuccessfulProcess("ffprobe", "-v", "error", "-show_entries", "format=duration", "-of", "default=noprint_wrappers=1:nokey=1", FilePath);
            return TimeSpan.FromSeconds(double.Parse(duration.Trim(), CultureInfo.InvariantCulture));
        }

        public string GenerateThumbnail(string thumbnailName, TimeSpan frame)
        {
            string thumbnailDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Thumbnails");
            string thumbnailPath = Path.Combine(thumbnailDirectory, thumbnailName + ".jpg");

            Directory.CreateDirectory(thumbnailDirectory);

            RunSuccessfulProcess("ffmpeg", "-v", "error", "-y", "-ss", frame.TotalSeconds.ToString(CultureInfo.InvariantCulture), "-i", FilePath, "-frames:v", "1", thumbnailPath);
            return thumbnailPath;
        }


        private (int, string, string) RunProcess(string filename, params string[] arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            foreach (var arg in arguments)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            process.Start();
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            if (!process.WaitForExit(Timeout))
            {
                process.Kill();
                Console.WriteLine($"Process `{filename} {string.Join(' ', arguments)}` timed out after {Timeout} ms.");
                throw new TimeoutException($"Process `{filename} {string.Join(' ', arguments)}` timed out after {Timeout} ms.");
            }

            var output = outputTask.Result;
            var error = errorTask.Result;

            return (process.ExitCode, output, error);
        }

        private string RunSuccessfulProcess(string filename, params string[] arguments)
        {
            var (exitCode, output, error) = RunProcess(filename, arguments);
            if (exitCode != 0 || !string.IsNullOrEmpty(error))
            {
                throw new Exception($"Process `{filename} {string.Join(' ', arguments)}` failed with exit code {exitCode}. Error: {error}");
            }
            return output;
        }
    }

}
