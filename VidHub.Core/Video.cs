using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Internal;
using VidHub.Platform.VidHubEnvironment;
using Windows.Storage;

namespace VidHub.Core
{
    internal class VideoTemplate
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string PreviewImagePath { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public VideoMetadata Metadata { get; set; } = new();
        public HashSet<long> TagID { get; set; } = [];
    }


    public class Video : FocusableObject, ICloneable, IComparable, IComparable<Video>, IComparer, IComparer<Video>, IEqualityComparer<Video>, IEquatable<Video>
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
        private VideoMetadata metadata;
        private DetailedHealth health;
        private bool loadingFinished;
        private HashSet<long> tagID;
        private readonly ILogger logger = VidHubContext.Logger;

        [JsonIgnore] public int ID { get => id; private set => SetFocusedProperty(ref id, value); }
        [JsonIgnore] public string Hash { get => hash; private set => SetFocusedProperty(ref hash, value); }
        public string Title { get => title; set => SetFocusedProperty(ref title, value); }
        public DateTime Date { get => date; set => SetFocusedProperty(ref date, value); }
        public TimeSpan Duration { get => duration; set => SetFocusedProperty(ref duration, value); }
        public string PreviewImagePath { get => previewImagePath; set => SetFocusedProperty(ref previewImagePath, value); }
        public string FilePath { get => filePath; set => SetFocusedProperty(ref filePath, value); }
        public VideoMetadata Metadata { get => metadata; set => SetFocusedProperty(ref metadata, value); }
        [JsonIgnore] public DetailedHealth Health { get => health; set => SetFocusedProperty(ref health, value); }
        [JsonIgnore] public bool LoadingFinished { get => loadingFinished; set => SetFocusedProperty(ref loadingFinished, value); }
        public HashSet<long> TagID { get => tagID; set => SetFocusedProperty(ref tagID, value); }
        public List<Tag> AddedTags => [.. VidHubSettings.Instance.General.Tags.Where(t => TagID.Contains(t.ID))];
        public List<Tag> NotAddedTags => [.. VidHubSettings.Instance.General.Tags.Where(t => !TagID.Contains(t.ID))];


        public Video()
        {
            logger.LogTrace("Video constructor (default) invoked");
            _ = Interlocked.Increment(ref IDProvider);
            id = IDProvider;
            hash = string.Empty;
            title = string.Empty;
            date = DateTime.MinValue;
            duration = TimeSpan.Zero;
            previewImagePath = string.Empty;
            filePath = string.Empty;
            metadata = new VideoMetadata();
            health = new DetailedHealth();
            loadingFinished = false;
            tagID = [];
        }
        public Video(string file) : this()
        {
            filePath = Path.GetFullPath(file);
            logger.LogTrace("Video constructed for file={File}", filePath);
            VideoHasher hasher = new(this);
            hash = hasher.GenerateHash();
            logger.LogDebug("Hash generated for file={File} hash={Hash}", filePath, hash);
        }
        public Video(Uri file) : this(file.AbsolutePath) { }
        public Video(StorageFile file) : this(file.Path) { }


        public bool AddTag(Tag tag)
        {
            bool result = TagID.Add(tag.ID);
            SaveCache();
            OnPropertyChanged(nameof(AddedTags));
            return result;
        }
        public bool RemoveTag(Tag tag)
        {
            bool result = TagID.Remove(tag.ID);
            SaveCache();
            OnPropertyChanged(nameof(AddedTags));
            return result;
        }

        public void CheckHealth()
        {
            logger.LogTrace("CheckHealth entered for file={File}", filePath);
            if (VidHubSettings.Instance.Health.Type is HealthType.NONE)
            {
                logger.LogDebug("Health checking disabled (Type=NONE)");
                return;
            }
            if (VidHubSettings.Instance.Health.Type is HealthType.EXISTENCECHECK)
            {
                Health = File.Exists(FilePath) ? HealthState.HEALTHY : HealthState.FILENOTFOUND;
                logger.LogDebug("Existence check result for {File}: {State}", FilePath, Health);
                return;
            }
            if (!File.Exists(FilePath))
            {
                Health = HealthState.FILENOTFOUND;
                logger.LogWarning("File not found during health check: {File}", FilePath);
                return;
            }

            Health = HealthState.INPROGRESS;
            logger.LogDebug("Starting detailed health check for {File}", FilePath);
            VideoProcessor processor = new(this);
            Health = processor.HealthCheck();
            logger.LogDebug("HealthCheck completed for {File} with state={State}", FilePath, Health);
        }

        public void Load()
        {
            logger.LogTrace("Load entered for file={File}", FilePath);
            if (LoadCache())
            {
                Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this);
                logger.LogInformation("Loaded from cache and title set for {File}", FilePath);
                return;
            }
            VideoProcessor processor = new(this);
            Metadata = processor.ProcessMetadata();
            logger.LogDebug("Metadata processed for {File}. VideoStreams={VideoStreamsCount}", FilePath, Metadata?.VideoStreams?.Count() ?? 0);
            Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this);
            Date = Metadata.Format is not null && Metadata.Format.CreationTime != DateTime.MinValue
                ? Metadata.Format.CreationTime
                : File.GetLastWriteTime(FilePath);
            Duration = Metadata.DefaultVideoStream is not null && Metadata.DefaultVideoStream.Duration != TimeSpan.Zero
                ? Metadata.DefaultVideoStream.Duration
                : Metadata.Format is not null
                    ? Metadata.Format.Duration
                    : TimeSpan.Zero;
            logger.LogDebug("Computed Date={Date} Duration={Duration} for {File}", Date, Duration, FilePath);
            _ = ProcessPreviewImage();
            SaveCache();
            logger.LogInformation("Load completed for {File}", FilePath);
        }


        private bool LoadCache()
        {
            logger.LogTrace("LoadCache entered for hash={Hash}", Hash);
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Data");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");
            if (!VidHubSettings.Instance.Performance.UseCacheLoading || !File.Exists(cachePath))
            {
                logger.LogDebug("Cache loading disabled or cache not present for {Hash}", Hash);
                return false;
            }

            try
            {
                string json = File.ReadAllText(cachePath);
                JsonSerializerOptions jsonOptions = new()
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    PropertyNameCaseInsensitive = true,
                };
                VideoTemplate? video = JsonSerializer.Deserialize<VideoTemplate>(json, jsonOptions);
                if (video is null)
                {
                    logger.LogWarning("Cache deserialized to null for {CachePath}", cachePath);
                    return false;
                }

                title = video.Title;
                date = video.Date;
                duration = video.Duration;
                previewImagePath = video.PreviewImagePath;
                filePath = video.FilePath;
                metadata = video.Metadata;
                tagID = video.TagID;

                logger.LogDebug("Cache loaded for {Hash}, previewImagePresent={HasPreview}", Hash, !string.IsNullOrEmpty(PreviewImagePath));
                return !string.IsNullOrEmpty(PreviewImagePath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Exception while loading cache for {CachePath}", cachePath);
                return false;
            }
        }
        private void SaveCache()
        {
            logger.LogTrace("SaveCache entered for hash={Hash}", Hash);
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Data");
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
            logger.LogDebug("Cache saved to {CachePath}", cachePath);
        }

        private bool ProcessPreviewImage()
        {
            logger.LogTrace("ProcessPreviewImage entered for file={File}", FilePath);
            VideoProcessor processor = new(this);
            bool result = processor.ProcessPreviewImage(out string? extractedImagePath);
            PreviewImagePath = extractedImagePath ?? string.Empty;
            logger.LogDebug("ProcessPreviewImage result={Result} path={Path}", result, PreviewImagePath);
            return result;
        }


        protected override bool SetFocusedProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            bool result = base.SetFocusedProperty(ref field, newValue, propertyName);
            SaveCache();
            logger.LogTrace("SetFocusedProperty for {Property} resulted in changed={Changed}", propertyName, result);
            return result;
        }


        public object Clone()
        {
            logger.LogTrace("Clone called for {File}", FilePath);
            return MemberwiseClone();
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
            logger.LogTrace("GetHashCode(Video) for {File}", obj?.FilePath);
            return obj is null ? 0 : obj.Hash != null ? StringComparer.Ordinal.GetHashCode(obj.Hash) : 0;
        }
        public override int GetHashCode()
        {
            logger.LogTrace("GetHashCode() for {File}", FilePath);
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
