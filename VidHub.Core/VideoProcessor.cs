using System.Diagnostics;
using System.Globalization;
using VidHub.Core.Enums;
using VidHub.Core.Settings;
using VidHub.Core.Streams;

namespace VidHub.Core
{
    internal class ProcessResult(int exitCode, string standardOutput, string standardError)
    {
        public int ExitCode { get; } = exitCode;
        public string StandardOutput { get; } = standardOutput;
        public string StandardError { get; } = standardError;
        public bool Successful => exitCode == 0 && string.IsNullOrEmpty(standardError);
        public bool Failure => exitCode != 0 || !string.IsNullOrEmpty(standardError);
    }

    internal class VideoProcessor(Video video)
    {
        // TODO: Implement actual executable path finding
        private const string ffmpegPath = "ffmpeg";
        private const string ffprobePath = "ffprobe";
        private TimeSpan timeout = TimeSpan.FromSeconds(10);
        private IDictionary<string, string>? metadata;


        public FormatStream GetFormatStream()
        {
            if (metadata == null || metadata.Count == 0)
            {
                ExtractMetadata();
            }
            return new FormatStream(metadata?.Where(kv => kv.Key.StartsWith("format")).ToDictionary(kv => kv.Key[7..], kv => kv.Value) ?? new Dictionary<string, string>());
        }
        public IEnumerable<VideoStream> GetVideoStreams()
        {
            return GetStreamsOfType("video").Select(s => new VideoStream(s));
        }
        public IEnumerable<AudioStream> GetAudioStreams()
        {
            return GetStreamsOfType("audio").Select(s => new AudioStream(s));
        }
        public IEnumerable<SubtitleStream> GetSubtitleStreams()
        {
            return GetStreamsOfType("subtitle").Select(s => new SubtitleStream(s));
        }
        public IEnumerable<MediaStream> GetUnknownStreams()
        {
            if (metadata == null)
            {
                _ = ExtractMetadata();
            }
            HashSet<string> knownTypes = ["video", "audio", "subtitle"];
            return metadata?
                .Where(kv => kv.Key.StartsWith("streams.stream"))
                .GroupBy(kv => kv.Key[15..].Split('.')[0])
                .Select(group => group.ToDictionary(kv => kv.Key[(group.Key.Length + 16)..], kv => kv.Value))
                .Where(dict => !knownTypes.Contains(dict["codec_type"]))
                .Select(s => new MediaStream(s)) ?? [];
        }


        public VideoHealth HealthCheck()
        {
            return HealthCheck(VidHubSettings.Instance.VideoHealth.Type);
        }
        public VideoHealth HealthCheck(VideoHealthCheckType Type)
        {
            try
            {
                if (Type == VideoHealthCheckType.QUICKCHECK)
                {
                    string[] frameArgs = { "-frames:v", "1" };
                    (string level, VideoHealth failure)[] checks =
                    [
                        ("fatal", VideoHealth.CRITICALCORRUPTION),
                        ("error", VideoHealth.SERIOUSCORRUPTION),
                        ("warning", VideoHealth.MINORCORRUPTION)
                    ];
                    foreach ((string? level, VideoHealth failure) in checks)
                    {
                        ProcessResult result = RunCheck(level, frameArgs);
                        if (result.Failure)
                        {
                            return failure;
                        }
                    }
                    return VideoHealth.HEALTHY;
                }
                else
                {
                    timeout = TimeSpan.FromMinutes(5);
                    (string level, VideoHealth failure)[] checks =
                    [
                        ("fatal", VideoHealth.CRITICALCORRUPTION),
                        ("error", VideoHealth.SERIOUSCORRUPTION),
                        ("warning", VideoHealth.MINORCORRUPTION)
                    ];
                    foreach ((string? level, VideoHealth failure) in checks)
                    {
                        ProcessResult result = RunCheck(level);
                        if (result.Failure)
                        {
                            timeout = TimeSpan.FromSeconds(10);
                            return failure;
                        }
                    }
                    timeout = TimeSpan.FromSeconds(10);
                    return VideoHealth.HEALTHY;
                }
            }
            catch
            {
                return VideoHealth.UNKNOWNERROR;
            }
        }

        public bool ExtractPreviewImage(out string? previewImagePath)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, video.Hash + ".jpg");
            _ = Directory.CreateDirectory(previewDirectory);

            ProcessResult result = RunProcess(ffmpegPath, "-v", "error", "-y", "-i", video.FilePath, "-map", "0:v", "-map", "-0:V", "-c", "copy", previewPath);
            previewImagePath = File.Exists(previewPath) ? previewPath : null;
            return result.Successful && previewImagePath != null;
        }
        public bool GeneratePreviewImage(out string? previewImagePath)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, video.Hash + ".jpg");
            _ = Directory.CreateDirectory(previewDirectory);

            ProcessResult result = RunProcess(ffmpegPath, "-v", "error", "-y", "-ss", VidHubSettings.Instance.GetPreviewImageTime(video).TotalSeconds.ToString(CultureInfo.InvariantCulture), "-i", video.FilePath, "-frames:v", "1", previewPath);
            previewImagePath = File.Exists(previewPath) ? previewPath : null;
            return result.Successful && previewImagePath != null;
        }
        public bool ProcessPreviewImage(out string? previewImagePath)
        {
            return VidHubSettings.Instance.Modals.PreviewImageFormat.ExtractEmbeddedImage
                ? ExtractPreviewImage(out previewImagePath) || GeneratePreviewImage(out previewImagePath)
                : GeneratePreviewImage(out previewImagePath);
        }


        private string RunProcessOrFail(string executable, params string[] arguments)
        {
            ProcessResult result = RunProcess(executable, arguments);
            return result.ExitCode != 0 || !string.IsNullOrEmpty(result.StandardError)
                ? throw new Exception($"Process `{executable} {string.Join(' ', arguments)}` failed with exit code {result.ExitCode}.", new Exception(result.StandardError))
                : result.StandardOutput;
        }
        private ProcessResult RunProcess(string executable, params string[] arguments)
        {
            using Process process = new();
            process.StartInfo.FileName = executable;
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
                throw new TimeoutException($"Process `{process.ProcessName} [{process.Id}]` timed out after {process.TotalProcessorTime} ms.");
            }

            return new ProcessResult(process.ExitCode, outputTask.Result, errorTask.Result);
        }
        private bool TryRunProcess(string executable, string[] arguments, out ProcessResult? result)
        {
            result = null;
            try
            {
                result = RunProcess(executable, arguments);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private ProcessResult RunCheck(string level, params string[] extraArgs)
        {
            List<string> args = ["-v", level, "-i", video.FilePath];
            if (extraArgs != null && extraArgs.Length > 0)
            {
                args.AddRange(extraArgs);
            }
            args.AddRange(["-f", "null", "-"]);
            return RunProcess(ffmpegPath, [.. args]);
        }

        private bool ExtractMetadata()
        {
            try
            {
                string rawData = RunProcessOrFail(ffprobePath, "-v", "error", "-print_format", "flat", "-show_format", "-show_streams", video.FilePath);
                metadata = rawData
                    .Split('\n')
                    .Select(line => line.Split('=', 2))
                    .Where(parts => parts.Length == 2)
                    .ToDictionary(parts => parts[0].Trim().ToLower(), parts => parts[1].Trim().Trim('"'));
                return true;
            }
            catch
            {
                metadata = null;
                return false;
            }
        }
        private IEnumerable<IDictionary<string, string>> GetStreamsOfType(string type)
        {
            if (metadata == null)
            {
                _ = ExtractMetadata();
            }
            return metadata?
                .Where(kv => kv.Key.StartsWith("streams.stream"))
                .GroupBy(kv => kv.Key[15..].Split('.')[0])
                .Select(group => group.ToDictionary(kv => kv.Key[(group.Key.Length + 16)..], kv => kv.Value))
                .Where(dict => dict["codec_type"].Contains(type, StringComparison.OrdinalIgnoreCase)) ?? [];
        }
    }
}
