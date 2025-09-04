using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidHub.Core
{
    public class Video : IComparable<Video>, IEquatable<Video>
    {
        public static List<string> ExtensionTypes => [".mp4", ".mov", ".wmv", ".mkv"];
        private static int IDProvider = 0;

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string ThumbnailPath { get; set; }
        public string FilePath { get; set; }
        [JsonIgnore] public int ID { get; set; }
        [JsonIgnore] protected string Hash { get; set; }


        public Video()
        {
            Title = string.Empty;
            Date = DateTime.MinValue;
            Duration = TimeSpan.Zero;
            ThumbnailPath = string.Empty;
            FilePath = string.Empty;
            ID = IDProvider++;
            Hash = string.Empty;
        }
        public Video(string filePath) : this()
        {
            FilePath = Path.GetFullPath(filePath);
            Hash = BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(FilePath))).Replace("-", "").ToLowerInvariant();
        }


        public void Load()
        {
            if (LoadCache())
            {
                return;
            }

            foreach (var action in LoadActions())
            {
                action();
            }

            SaveCache();
        }

        public bool TryLoad()
        {
            if (LoadCache())
            {
                return true;
            }

            bool success = true;
            var processor = new MetadataProcessor(FilePath);

            foreach (var action in LoadActions())
            {
                try
                {
                    action();
                }
                catch
                {
                    success = false;
                }
            }

            SaveCache();
            return success;
        }


        private bool LoadCache()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");

            if (File.Exists(cachePath))
            {
                var json = File.ReadAllText(cachePath);
                var video = JsonSerializer.Deserialize<Video>(json);

                if (video != null)
                {
                    Title = video.Title;
                    Date = video.Date;
                    Duration = video.Duration;
                    ThumbnailPath = video.ThumbnailPath;
                    FilePath = video.FilePath;
                    return true;
                }
            }

            return false;
        }

        private bool SaveCache(bool overwrite = true)
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "VidHub", "Cache");
            string cachePath = Path.Combine(cacheDirectory, Hash + ".json");

            Directory.CreateDirectory(cacheDirectory);

            var exists = Path.Exists(cachePath);

            if (overwrite || !exists)
            {
                var json = JsonSerializer.Serialize<Video>(this);
                File.WriteAllText(cachePath, json);
            }

            return exists;
        }

        private List<Action> LoadActions()
        {
            var processor = new MetadataProcessor(FilePath);

            return
            [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => Date = File.GetLastWriteTime(FilePath),
                () => Date = processor.ExtractDate(),
                () => Duration = processor.ExtractDuration(),
                () => ThumbnailPath = processor.GenerateThumbnail(Hash, Duration / 2),
            ];
        }


        public int CompareTo(Video? other)
        {
            if (other == null) return -1;
            if (ReferenceEquals(this, other)) return 0;
            int comparison = ID.CompareTo(other.ID);
            if (comparison != 0) return comparison;
            comparison = FilePath.CompareTo(other.FilePath);
            if (comparison != 0) return comparison;
            comparison = Title.CompareTo(other.Title);
            if (comparison != 0) return comparison;
            comparison = Date.CompareTo(other.Date);
            if (comparison != 0) return comparison;
            return Duration.CompareTo(other.Duration);
        }

        public bool Equals(Video? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.FilePath != FilePath) return false;
            if (other.Title != Title) return false;
            if (other.Date != Date) return false;
            if (other.Duration != Duration) return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj as Video);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FilePath, Title, Date, Duration);
        }

        public override string ToString()
        {
            return $"Title: {Title}    Date: {Date}    Duration: {Duration}";
        }
    }
}
