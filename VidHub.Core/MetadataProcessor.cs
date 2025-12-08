using System.Diagnostics;
using System.Globalization;
using VidHub.Core.Settings;
using VidHub.Core.Streams;

namespace VidHub.Core
{
    internal class MetadataProcessor(string filePath)
    {
        private IDictionary<string, string> metadata = new Dictionary<string, string>();


        public IDictionary<string, string> ExtractMetadata()
        {
            string rawData = RunSuccessfulProcess("ffprobe", "-v", "error", "-print_format", "flat", "-show_format", "-show_streams", filePath);
            return rawData
                .Split('\n')
                .Select(line => line.Split('=', 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0].Trim().ToLower(), parts => parts[1].Trim().Trim('"'));
        }

        private IEnumerable<IDictionary<string, string>> GetStreams(string type)
        {
            if (metadata == null || metadata.Count == 0)
            {
                metadata = ExtractMetadata();
            }

            return metadata
                .Where(kv => kv.Key.StartsWith("streams.stream"))
                .GroupBy(kv => kv.Key[15..].Split('.')[0])
                .Select(group => group.ToDictionary(kv => kv.Key[(group.Key.Length + 16)..], kv => kv.Value))
                .Where(dict => dict["codec_type"].Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public FormatStream GetFormatStream()
        {
            if (metadata == null || metadata.Count == 0)
            {
                metadata = ExtractMetadata();
            }

            return new FormatStream(metadata.Where(kv => kv.Key.StartsWith("format")).ToDictionary(kv => kv.Key[7..], kv => kv.Value));
        }

        public IEnumerable<VideoStream> GetVideoStreams()
        {
            return GetStreams("video").Select(s => new VideoStream(s));
        }

        public IEnumerable<AudioStream> GetAudioStreams()
        {
            return GetStreams("audio").Select(s => new AudioStream(s));
        }

        public IEnumerable<SubtitleStream> GetSubtitleStreams()
        {
            return GetStreams("subtitle").Select(s => new SubtitleStream(s));
        }

        public IEnumerable<MediaStream> GetUnknownStreams()
        {
            if (metadata == null || metadata.Count == 0)
            {
                metadata = ExtractMetadata();
            }

            HashSet<string> knownTypes = ["video", "audio", "subtitle"];
            return metadata
                .Where(kv => kv.Key.StartsWith("streams.stream"))
                .GroupBy(kv => kv.Key[15..].Split('.')[0])
                .Select(group => group.ToDictionary(kv => kv.Key[(group.Key.Length + 16)..], kv => kv.Value))
                .Where(dict => !knownTypes.Contains(dict["codec_type"]))
                .Select(s => new MediaStream(s));
        }

        public string QuickHealthCheck()
        {
            (_, _, string error) = RunProcess("ffmpeg", "-v", "error", "-i", filePath, "-frames:v", "1", "-f", "null", "-");
            return error;
        }
        public string FullHealthCheck()
        {
            (_, _, string error) = RunProcess(int.MaxValue, "ffmpeg", "-v", "error", "-i", filePath, "-f", "null", "-");
            return error;
        }

        public string ExtractPreviewImage(string imageName, TimeSpan frame)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, imageName + ".jpg");

            _ = Directory.CreateDirectory(previewDirectory);

            if (VidHubSettings.Instance.PreviewImageCustomization.ExtractEmbeddedImageCommand)
            {
                (int exitCode, _, _) = RunProcess("ffmpeg", "-v", "error", "-y", "-i", filePath, "-map", "0:v", "-map", "-0:V", "-c", "copy", previewPath);
                if (exitCode == 0 && File.Exists(previewPath))
                {
                    return previewPath;
                }
            }

            _ = RunSuccessfulProcess("ffmpeg", "-v", "error", "-y", "-ss", frame.TotalSeconds.ToString(CultureInfo.InvariantCulture), "-i", filePath, "-frames:v", "1", previewPath);
            return previewPath;
        }


        private (int, string, string) RunProcess(int timeout, string filename, params string[] arguments)
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

            if (!process.WaitForExit(timeout))
            {
                process.Kill();
                throw new TimeoutException($"Process `{filename} {string.Join(' ', arguments)}` timed out after {10000} ms.");
            }

            string output = outputTask.Result;
            string error = errorTask.Result;

            return (process.ExitCode, output, error);
        }
        private (int, string, string) RunProcess(string filename, params string[] arguments)
        {
            return RunProcess(10000, filename, arguments);
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
