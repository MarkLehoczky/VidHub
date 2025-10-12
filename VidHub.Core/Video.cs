using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VidHub.Core.Helpers;
using VidHub.Platform;

namespace VidHub.Core
{
    public partial class Video : ObservableObject, IComparable<Video>, IEquatable<Video>
    {

        public static List<string> ExtensionTypes => [".mp4", ".mov", ".wmv", ".mkv"];
        private static int IDProvider = 0;

        private string title;
        private DateTime date;
        private TimeSpan duration;
        private string thumbnailPath;
        private string filePath;
        private string hash;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public TimeSpan Duration
        {
            get => duration;
            set => SetProperty(ref duration, value);
        }

        public string ThumbnailPath
        {
            get => thumbnailPath;
            set => SetProperty(ref thumbnailPath, value);
        }

        public string FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }


        [JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore]
        protected string Hash
        {
            get => hash;
            set => SetProperty(ref hash, value);
        }


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


        public void Load(bool cacheLoad, TimeSpan frame)
        {
            if (cacheLoad && LoadCache(frame))
            {
                return;
            }

            foreach (var action in LoadActions(frame))
            {
                action();
            }

            SaveCache();
        }
        public void Load(bool cacheLoad, double percentage)
        {
            if (cacheLoad && LoadCache(percentage))
            {
                return;
            }

            foreach (var action in LoadActions(percentage))
            {
                action();
            }

            SaveCache();
        }

        public bool TryLoad(bool cacheLoad, TimeSpan frame)
        {
            if (cacheLoad && LoadCache(frame))
            {
                return true;
            }

            bool success = true;
            var processor = new MetadataProcessor(FilePath);

            foreach (var action in LoadActions(frame))
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
        public bool TryLoad(bool cacheLoad, double percentage)
        {
            if (cacheLoad && LoadCache(percentage))
            {
                return true;
            }

            bool success = true;
            var processor = new MetadataProcessor(FilePath);

            foreach (var action in LoadActions(percentage))
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


        private bool LoadCache(TimeSpan frame)
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

                    if (!File.Exists(ThumbnailPath))
                    {
                        new MetadataProcessor(FilePath).GenerateThumbnail(Hash, frame > Duration ? Duration : frame);
                    }

                    return true;
                }
            }

            return false;
        }
        private bool LoadCache(double percentage)
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

                    if (!File.Exists(ThumbnailPath))
                    {
                        new MetadataProcessor(FilePath).GenerateThumbnail(Hash, Duration * percentage);
                    }

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

        private List<Action> LoadActions(TimeSpan frame)
        {
            var processor = new MetadataProcessor(FilePath);

            return
            [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => Date = File.GetLastWriteTime(FilePath),
                () => Date = processor.ExtractDate(),
                () => Duration = processor.ExtractDuration(),
                () => ThumbnailPath = processor.GenerateThumbnail(Hash, frame > Duration ? Duration : frame),
            ];
        }
        private List<Action> LoadActions(double percentage)
        {
            var processor = new MetadataProcessor(FilePath);

            return
            [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => Date = File.GetLastWriteTime(FilePath),
                () => Date = processor.ExtractDate(),
                () => Duration = processor.ExtractDuration(),
                () => ThumbnailPath = processor.GenerateThumbnail(Hash, Duration * percentage),
            ];
        }

        public void ExtractThumbnail(TimeSpan frame)
        {
            var processor = new MetadataProcessor(FilePath);
            try
            {
                ThumbnailPath = processor.GenerateThumbnail(Hash, frame > Duration ? Duration : frame);
            }
            catch { }
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

        [RelayCommand]
        private async Task RenameAsync()
        {
            await Context.MainWindow.ShowDialogAsync(ModalType.RenameVideo, $"Rename '{Title}'", "Confirm", this);
        }
    }
}
