using System.Diagnostics;
using System.Globalization;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Streams;

namespace VidHub.Core.Utilities.Internal
{
    internal class ProcessResult(int exitCode, string standardOutput, string standardError)
    {
        public int ExitCode { get; } = exitCode;
        public string StandardOutput { get; } = standardOutput;
        public string StandardError { get; } = standardError;
        public bool Successful => ExitCode == 0 && string.IsNullOrEmpty(StandardError);
        public bool Failure => ExitCode != 0 || !string.IsNullOrEmpty(StandardError);
    }


    internal class VideoProcessor(Video video)
    {
        // TODO: Implement actual executable path finding
        private const string ffmpegPath = "ffmpeg";
        private const string ffprobePath = "ffprobe";
        private TimeSpan timeout = TimeSpan.FromSeconds(10);


        public VideoMetadata ProcessMetadata()
        {
            Dictionary<string, string> metadata = ExtractMetadata();
            IEnumerable<IDictionary<string, string>> streams = metadata
                .Where(kv => kv.Key.StartsWith("streams.stream"))
                .GroupBy(kv => kv.Key[15..].Split('.')[0])
                .Select(group => group.ToDictionary(kv => kv.Key[(group.Key.Length + 16)..], kv => kv.Value));

            VideoFormat format = new(metadata.Where(kv => kv.Key.StartsWith("format")).ToDictionary(kv => kv.Key[7..], kv => kv.Value));
            IEnumerable<VideoStream> videoStreams = streams
                .Where(dict =>
                    dict["codec_type"].Contains("video", StringComparison.OrdinalIgnoreCase))
                .Select(s => new VideoStream(s));
            IEnumerable<AudioStream> audioStreams = streams
                .Where(dict =>
                    dict["codec_type"].Contains("audio", StringComparison.OrdinalIgnoreCase))
                .Select(s => new AudioStream(s));
            IEnumerable<SubtitleStream> subtitleStreams = streams
                .Where(dict =>
                    dict["codec_type"].Contains("subtitle", StringComparison.OrdinalIgnoreCase))
                .Select(s => new SubtitleStream(s));
            IEnumerable<MediaStream> unknownStreams = streams
                .Where(dict =>
                    !dict["codec_type"].Contains("video", StringComparison.OrdinalIgnoreCase)
                    && !dict["codec_type"].Contains("audio", StringComparison.OrdinalIgnoreCase)
                    && !dict["codec_type"].Contains("subtitle", StringComparison.OrdinalIgnoreCase))
                .Select(s => new MediaStream(s));

            return new VideoMetadata
            {
                Format = format,
                VideoStreams = videoStreams,
                AudioStreams = audioStreams,
                SubtitleStreams = subtitleStreams,
                UnknownStreams = unknownStreams
            };
        }

        public HealthState HealthCheck()
        {
            return HealthCheck(VidHubSettings.Instance.Health.Type);
        }
        public HealthState HealthCheck(HealthType type)
        {
            HealthState state = HealthState.NOTCHECKED;
            try
            {
                if (type == HealthType.QUICKCHECK)
                {
                    timeout = TimeSpan.FromSeconds(5);
                    state = RunProcess(ffmpegPath, "-v", "fatal", "-i", video.FilePath, "-frames:v", "1", "-f", "null", "-").Failure
                        ? HealthState.CRITICALCORRUPTION
                        : RunProcess(ffmpegPath, "-v", "error", "-i", video.FilePath, "-frames:v", "1", "-f", "null", "-").Failure
                            ? HealthState.SERIOUSCORRUPTION
                            : RunProcess(ffmpegPath, "-v", "warning", "-i", video.FilePath, "-frames:v", "1", "-f", "null", "-").Failure
                                ? HealthState.MINORCORRUPTION
                                : HealthState.HEALTHY;
                }
                else if (type == HealthType.FULLCHECK)
                {
                    timeout = TimeSpan.FromMinutes(5);
                    state = RunProcess(ffmpegPath, "-v", "fatal", "-i", video.FilePath, "-f", "null", "-").Failure
                        ? HealthState.CRITICALCORRUPTION
                        : RunProcess(ffmpegPath, "-v", "error", "-i", video.FilePath, "-f", "null", "-").Failure
                            ? HealthState.SERIOUSCORRUPTION
                            : RunProcess(ffmpegPath, "-v", "warning", "-i", video.FilePath, "-f", "null", "-").Failure
                                ? HealthState.MINORCORRUPTION
                                : HealthState.HEALTHY;
                }
            }
            catch
            {
                state = HealthState.UNKNOWNERROR;
            }
            finally
            {
                timeout = TimeSpan.FromSeconds(10);
            }
            return state;
        }

        public bool ExtractEmbeddedImage(out string? previewImagePath)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, video.Hash + ".jpg");
            _ = Directory.CreateDirectory(previewDirectory);

            ProcessResult result = TryRunProcess(ffmpegPath, "-v", "error", "-y", "-i", video.FilePath, "-map", "0:v", "-map", "-0:V", "-c", "copy", previewPath);
            previewImagePath = File.Exists(previewPath) ? previewPath : null;
            return result.Successful && previewImagePath != null;
        }
        public bool GeneratePreviewImage(out string? previewImagePath)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Previews");
            string previewPath = Path.Combine(previewDirectory, video.Hash + ".jpg");
            _ = Directory.CreateDirectory(previewDirectory);

            ProcessResult result = TryRunProcess(ffmpegPath, "-v", "error", "-y", "-ss", VidHubSettings.Instance.GetPreviewImageTime(video).TotalSeconds.ToString(CultureInfo.InvariantCulture), "-i", video.FilePath, "-frames:v", "1", previewPath);
            previewImagePath = File.Exists(previewPath) ? previewPath : null;
            return result.Successful && previewImagePath != null;
        }
        public bool ProcessPreviewImage(out string? previewImagePath)
        {
            return VidHubSettings.Instance.Dialogs.PreviewImageFormat.ExtractEmbeddedImage
                ? ExtractEmbeddedImage(out previewImagePath) || GeneratePreviewImage(out previewImagePath)
                : GeneratePreviewImage(out previewImagePath);
        }


        private Dictionary<string, string> ExtractMetadata()
        {
            ProcessResult result = TryRunProcess(ffprobePath, "-v", "error", "-print_format", "flat", "-show_format", "-show_streams", video.FilePath);
            if (!result.Successful)
            {
                return [];
            }
            return result.StandardOutput
                .Split('\n')
                .Select(line => line.Split('=', 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0].Trim().ToLower(), parts => parts[1].Trim().Trim('"'));
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
        private ProcessResult TryRunProcess(string executable, params string[] arguments)
        {
            try
            {
                return RunProcess(executable, arguments);
            }
            catch (Exception ex)
            {
                return new ProcessResult(int.MinValue, $"`{ex.GetType().Name}` exception was thrown.", ex.Message);
            }
        }
    }
}
