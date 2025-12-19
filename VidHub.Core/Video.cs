using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Internal;
using Windows.Storage;

namespace VidHub.Core
{
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

        [JsonIgnore] public int ID { get => id; private set => SetFocusedProperty(ref id, value); }
        [JsonIgnore] public string Hash { get => hash; private set => SetFocusedProperty(ref hash, value); }
        public string Title { get => title; set => SetFocusedProperty(ref title, value); }
        public DateTime Date { get => date; set => SetFocusedProperty(ref date, value); }
        public TimeSpan Duration { get => duration; set => SetFocusedProperty(ref duration, value); }
        public string PreviewImagePath { get => previewImagePath; set => SetFocusedProperty(ref previewImagePath, value); }
        public string FilePath { get => filePath; set => SetFocusedProperty(ref filePath, value); }
        public VideoMetadata Metadata { get => metadata; set => SetFocusedProperty(ref metadata, value); }
        public DetailedHealth Health { get => health; set => SetFocusedProperty(ref health, value); }
        public bool LoadingFinished { get => loadingFinished; set => SetFocusedProperty(ref loadingFinished, value); }


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
            metadata = new VideoMetadata();
            health = new DetailedHealth();
            loadingFinished = false;
        }
        public Video(string file) : this()
        {
            filePath = Path.GetFullPath(file);
            VideoHasher hasher = new(this);
            hash = hasher.GenerateHash();
        }
        public Video(Uri file) : this(file.AbsolutePath) { }
        public Video(StorageFile file) : this(file.Path) { }


        public void CheckHealth()
        {
            if (VidHubSettings.Instance.Health.Type is HealthType.NONE)
            {
                return;
            }
            if (VidHubSettings.Instance.Health.Type is HealthType.EXISTENCECHECK)
            {
                Health = File.Exists(FilePath) ? HealthState.HEALTHY : HealthState.FILENOTFOUND;
                return;
            }
            if (!File.Exists(FilePath))
            {
                Health = HealthState.FILENOTFOUND;
                return;
            }

            Health = HealthState.INPROGRESS;
            VideoProcessor processor = new(this);
            Health = processor.HealthCheck();
        }

        public void Load()
        {
            if (LoadCache())
            {
                Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this);
                return;
            }
            VideoProcessor processor = new(this);
            Metadata = processor.ProcessMetadata();
            Title = VidHubSettings.Instance.GetCustomizedVideoTitle(this);
            Date = Metadata.Format is not null && Metadata.Format.CreationTime != DateTime.MinValue
                ? Metadata.Format.CreationTime
                : File.GetLastWriteTime(FilePath);
            Duration = Metadata.DefaultVideoStream is not null && Metadata.DefaultVideoStream.Duration != TimeSpan.Zero
                ? Metadata.DefaultVideoStream.Duration
                : Metadata.Format is not null
                    ? Metadata.Format.Duration
                    : TimeSpan.Zero;
            ProcessPreviewImage();
            SaveCache();
        }


        private bool LoadCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Data");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");
            if (!VidHubSettings.Instance.Performance.UseCacheLoading || !File.Exists(cachePath))
            {
                return false;
            }

            string json = File.ReadAllText(cachePath);
            Video? video = JsonSerializer.Deserialize<Video>(json);
            if (video is null)
            {
                return false;
            }

            Title = video.Title;
            Date = video.Date;
            Duration = video.Duration;
            PreviewImagePath = video.PreviewImagePath;
            FilePath = video.FilePath;
            Metadata = video.Metadata;

            return !string.IsNullOrEmpty(PreviewImagePath);
        }
        private void SaveCache()
        {
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
        }

        private bool ProcessPreviewImage()
        {
            VideoProcessor processor = new(this);
            bool result = processor.ProcessPreviewImage(out string? extractedImagePath);
            PreviewImagePath = extractedImagePath ?? string.Empty;
            return result;
        }


        protected override bool SetFocusedProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            bool result = base.SetFocusedProperty(ref field, newValue, propertyName);
            SaveCache();
            return result;
        }


        public object Clone()
        {
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
