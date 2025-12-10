using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Streams;
using VidHub.Core.Utilities;
using VidHub.Core.Utilities.Helper;
using Windows.Storage;

namespace VidHub.Core
{
    public class Video : FocusableObject, IComparable, IComparable<Video>, IComparer, IComparer<Video>, IEqualityComparer<Video>, IEquatable<Video>
    {
        private static int IDProvider = 0;

        public static List<string> ExtensionTypes => [".avi", ".flv", ".m2ts", ".m4v", ".mkv", ".mov", ".mp4", ".mpeg", ".mpg", ".mts", ".webm", ".wmv"];


        private int id;
        private string hash;
        private string title;
        private DateTime date;
        private TimeSpan duration;
        private string previewImagePath;
        private string filePath;
        private IEnumerable<VideoStream> videoStreams;
        private IEnumerable<AudioStream> audioStreams;
        private IEnumerable<SubtitleStream> subtitleStreams;
        private IEnumerable<MediaStream> unknownStreams;
        private FormatStream formatStream;
        private DetailedVideoState healthState;

        [JsonIgnore] public int ID { get => id; set => SetFocusedProperty(ref id, value); }
        public string Hash { get => hash; set => SetFocusedProperty(ref hash, value); }
        public string Title { get => title; set => SetFocusedProperty(ref title, value); }
        public DateTime Date { get => date; set => SetFocusedProperty(ref date, value); }
        public TimeSpan Duration { get => duration; set => SetFocusedProperty(ref duration, value); }
        public string PreviewImagePath { get => previewImagePath; set => SetFocusedProperty(ref previewImagePath, value); }
        public string FilePath { get => filePath; set => SetFocusedProperty(ref filePath, value); }

        public FormatStream FormatStream { get => formatStream; set => SetFocusedProperty(ref formatStream, value); }
        public IEnumerable<VideoStream> VideoStreams { get => videoStreams; set => SetFocusedProperty(ref videoStreams, value); }
        public IEnumerable<AudioStream> AudioStreams { get => audioStreams; set => SetFocusedProperty(ref audioStreams, value); }
        public IEnumerable<SubtitleStream> SubtitleStreams { get => subtitleStreams; set => SetFocusedProperty(ref subtitleStreams, value); }
        public IEnumerable<MediaStream> UnknownStreams { get => unknownStreams; set => SetFocusedProperty(ref unknownStreams, value); }
        [JsonIgnore] public VideoStream? DefaultVideoStream => VideoStreams.FirstOrDefault(s => s.IsDefault) ?? VideoStreams.FirstOrDefault();
        [JsonIgnore] public AudioStream? DefaultAudioStream => AudioStreams.FirstOrDefault(s => s.IsDefault) ?? AudioStreams.FirstOrDefault();

        public DetailedVideoState HealthState { get => healthState; set => SetFocusedProperty(ref healthState, value); }


        public Video()
        {
            _ = Interlocked.Increment(ref IDProvider);
            id = IDProvider;
            hash = string.Empty;
            title = string.Empty;
            date = DateTime.MinValue;
            duration = TimeSpan.Zero;
            previewImagePath = string.Empty;
            filePath = string.Empty;
            formatStream = new FormatStream(new Dictionary<string, string>());
            videoStreams = [];
            audioStreams = [];
            subtitleStreams = [];
            unknownStreams = [];
            healthState = new DetailedVideoState();
        }
        public Video(string file) : this()
        {
            filePath = Path.GetFullPath(file);
            VideoHasher hasher = new(this);
            hash = hasher.GenerateHash();
        }
        public Video(Uri file) : this(file.AbsolutePath) { }
        public Video(StorageFile file) : this(file.Path) { }


        public void Load()
        {
            if (LoadCache())
            {
                Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this);
            }
            else
            {
                foreach (Action action in LoadActions())
                {
                    try { action(); }
                    catch { }
                }
            }
            SaveCache();
        }

        private bool LoadCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");
            if (!VidHubSettings.Instance.Performance.UseCacheLoading || !File.Exists(cachePath))
            {
                return false;
            }

            string json = File.ReadAllText(cachePath);
            Video video = JsonSerializer.Deserialize<Video>(json)!;

            Title = video.Title;
            Date = video.Date;
            Duration = video.Duration;
            PreviewImagePath = video.PreviewImagePath;
            FilePath = video.FilePath;
            FormatStream = video.FormatStream;
            VideoStreams = video.VideoStreams;
            AudioStreams = video.AudioStreams;
            SubtitleStreams = video.SubtitleStreams;
            UnknownStreams = video.UnknownStreams;

            return !string.IsNullOrEmpty(PreviewImagePath) || ProcessPreviewImage();
        }

        private List<Action> LoadActions()
        {
            VideoProcessor metadataProcessor = new(this);

            return [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => FormatStream = metadataProcessor.GetFormatStream(),
                () => VideoStreams = metadataProcessor.GetVideoStreams(),
                () => AudioStreams = metadataProcessor.GetAudioStreams(),
                () => SubtitleStreams = metadataProcessor.GetSubtitleStreams(),
                () => UnknownStreams = metadataProcessor.GetUnknownStreams(),
                () => Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this),
                () => Date = FormatStream.CreationTime != DateTime.MinValue ? FormatStream.CreationTime : File.GetLastWriteTime(FilePath),
                () => Duration = DefaultVideoStream!.Duration != TimeSpan.Zero ? DefaultVideoStream.Duration : FormatStream.Duration,
                () => ProcessPreviewImage()
            ];
        }

        public bool ProcessPreviewImage()
        {
            try
            {
                VideoProcessor processor = new(this);
                if (processor.ProcessPreviewImage(out var extractedImagePath))
                {
                    PreviewImagePath = extractedImagePath!;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void SaveCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");

            JsonSerializerOptions jsonOptions = new()
            {
                NumberHandling = JsonNumberHandling.WriteAsString,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                WriteIndented = true,
            };

            _ = Directory.CreateDirectory(cacheDirectory);
            File.WriteAllText(cachePath, JsonSerializer.Serialize(this, jsonOptions));
        }

        public void HealthCheck()
        {
            List<VideoHealthCheckType> existenceCheckTypes = [VideoHealthCheckType.EXISTENCECHECK, VideoHealthCheckType.QUICKCHECK, VideoHealthCheckType.FULLCHECK];

            if (existenceCheckTypes.Contains(VidHubSettings.Instance.VideoHealth.Type))
            {
                HealthState = VideoHealth.FILENOTFOUND;
            }
            if (VidHubSettings.Instance.VideoHealth.Type == VideoHealthCheckType.NONE || VidHubSettings.Instance.VideoHealth.Type == VideoHealthCheckType.EXISTENCECHECK)
            {
                return;
            }

            HealthState = VideoHealth.INPROGRESS;
            VideoProcessor processor = new(this);
            HealthState = processor.HealthCheck();
        }


        public int CompareTo(object? obj)
        {
            return obj is null ? 1 : obj is Video other ? ((IComparable<Video>)this).CompareTo(other) : 1;
        }
        public int CompareTo(Video? other)
        {
            return other is null ? 1 : Comparer<int>.Default.Compare(ID, other.ID);
        }

        public int Compare(object? x, object? y)
        {
            return ReferenceEquals(x, y)
                ? 0
                : x is null ? -1 : y is null ? 1 : x is Video left && y is Video right ? ((IComparer<Video>)this).Compare(left, right) : 1;
        }
        public int Compare(Video? x, Video? y)
        {
            return ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : ((IComparable<Video>)x).CompareTo(y);
        }

        public int GetHashCode(object obj)
        {
            return obj is Video video ? ((IEqualityComparer<Video>)this).GetHashCode(video) : 0;
        }
        public int GetHashCode(Video obj)
        {
            return obj is null ? 0 : obj.Hash != null ? StringComparer.Ordinal.GetHashCode(obj.Hash) : 0;
        }
        public override int GetHashCode()
        {
            return Hash != null ? StringComparer.Ordinal.GetHashCode(Hash) : base.GetHashCode();
        }

        public bool Equals(Video? x, Video? y)
        {
            return ReferenceEquals(x, y) || (x is not null && y is not null && string.Equals(x.Hash, y.Hash, StringComparison.Ordinal));
        }
        public bool Equals(Video? other)
        {
            return other is not null && (ReferenceEquals(this, other) || string.Equals(Hash, other.Hash, StringComparison.Ordinal));
        }
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Video other && ((IEquatable<Video>)this).Equals(other));
        }

        public static bool operator ==(Video left, Video right)
        {
            return left is null ? right is null : left.Equals(right);
        }
        public static bool operator !=(Video left, Video right)
        {
            return !(left == right);
        }
        public static bool operator <(Video left, Video right)
        {
            return left is null ? right is not null : ((IComparable<Video>)left).CompareTo(right) < 0;
        }
        public static bool operator <=(Video left, Video right)
        {
            return left is null || ((IComparable<Video>)left).CompareTo(right) <= 0;
        }
        public static bool operator >(Video left, Video right)
        {
            return left is not null && ((IComparable<Video>)left).CompareTo(right) > 0;
        }
        public static bool operator >=(Video left, Video right)
        {
            return left is null ? right is null : ((IComparable<Video>)left).CompareTo(right) >= 0;
        }

        public override string ToString()
        {
            return $"Video ([{ID}]: {Title}    <{FilePath}>    ({Date:yyyy-MM-dd} - {Duration:h\\:mm\\:ss})";
        }
    }
}
