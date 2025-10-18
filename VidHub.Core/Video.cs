using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Storage;

namespace VidHub.Core
{
    public class Video : ObservableObject, IComparable, IComparable<Video>, IComparer, IComparer<Video>, IEqualityComparer<Video>, IEquatable<Video>
    {
        public static List<string> ExtensionTypes => [".mp4", ".mov", ".wmv", ".mkv"];

        private static int IDProvider = 0;


        private int id;
        protected string hash;
        private string title;
        private DateTime date;
        private TimeSpan duration;
        private string previewImagePath;
        private string filePath;

        // TODO: Implement advanced metadata extraction
        [JsonIgnore] public int ID { get => id; set => SetProperty(ref id, value); }
        protected string Hash { get => hash; set => SetProperty(ref hash, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public DateTime Date { get => date; set => SetProperty(ref date, value); }
        public TimeSpan Duration { get => duration; set => SetProperty(ref duration, value); }
        public string PreviewImagePath { get => previewImagePath; set => SetProperty(ref previewImagePath, value); }
        public string FilePath { get => filePath; set => SetProperty(ref filePath, value); }


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
        }
        public Video(string file) : this()
        {
            filePath = Path.GetFullPath(file);
            hash = GetFileHash();
        }
        public Video(Uri file) : this()
        {
            filePath = file.AbsolutePath;
            hash = GetFileHash();
        }
        public Video(StorageFile file) : this()
        {
            filePath = file.Path;
            hash = GetFileHash();
        }


        public void Load(bool cacheLoad, TimeSpan frame)
        {
            if (cacheLoad && LoadCache())
            {
                return;
            }

            foreach (Action action in LoadActions(frame))
            {
                try { action(); }
                catch { }
            }

            SaveCache();
        }
        public void Load(bool cacheLoad, double percentage)
        {
            if (cacheLoad && LoadCache())
            {
                return;
            }

            foreach (Action action in LoadActions(percentage))
            {
                try { action(); }
                catch { }
            }

            SaveCache();
        }

        public void ExtractPreviewImage(TimeSpan frame)
        {
            try { PreviewImagePath = new MetadataProcessor(FilePath).ExtractPreviewImage(Hash, frame > Duration ? Duration : frame); }
            catch { }
        }


        private List<Action> LoadActions(TimeSpan frame)
        {
            return [
            () => Title = Path.GetFileNameWithoutExtension(FilePath),
            () => Date = File.GetLastWriteTime(FilePath),
            () => Date = new MetadataProcessor(FilePath).ExtractDate(),
            () => Duration = new MetadataProcessor(FilePath).ExtractDuration(),
            () => ExtractPreviewImage(frame),
            ];
        }

        private List<Action> LoadActions(double percentage)
        {
            return [
                    () => Title = Path.GetFileNameWithoutExtension(FilePath),
            () => Date = File.GetLastWriteTime(FilePath),
            () => Date = new MetadataProcessor(FilePath).ExtractDate(),
            () => Duration = new MetadataProcessor(FilePath).ExtractDuration(),
            () => ExtractPreviewImage(Duration * percentage),
            ];
        }

        private bool LoadCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");

            if (!File.Exists(cachePath))
            {
                return false;
            }

            string json = File.ReadAllText(cachePath);
            Video? video = JsonSerializer.Deserialize<Video>(json);

            if (video is null)
            {
                return false;
            }

            if (!File.Exists(video.PreviewImagePath))
            {
                return false;
            }

            Title = video.Title;
            Date = video.Date;
            Duration = video.Duration;
            PreviewImagePath = video.PreviewImagePath;
            FilePath = video.FilePath;

            return true;
        }

        // TODO: Implement single cache file
        private void SaveCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");

            _ = Directory.CreateDirectory(cacheDirectory);
            File.WriteAllText(cachePath, JsonSerializer.Serialize(this));
        }

        // TODO: Handle key collision
        private string GetFileHash()
        {
            return BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(FilePath))).TrimStart('-').ToLowerInvariant();
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

        public bool Equals(Video? x, Video? y)
        {
            return ReferenceEquals(x, y) || (x is not null && y is not null && string.Equals(x.Hash, y.Hash, StringComparison.Ordinal));
        }

        public int GetHashCode(Video obj)
        {
            return obj is null ? 0 : obj.Hash != null ? StringComparer.Ordinal.GetHashCode(obj.Hash) : 0;
        }

        public bool Equals(Video? other)
        {
            return other is not null && (ReferenceEquals(this, other) || string.Equals(Hash, other.Hash, StringComparison.Ordinal));
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is not null && obj is Video other && ((IEquatable<Video>)this).Equals(other));
        }

        public override int GetHashCode()
        {
            return Hash != null ? StringComparer.Ordinal.GetHashCode(Hash) : base.GetHashCode();
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
