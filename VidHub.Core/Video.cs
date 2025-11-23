using Blake3;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VidHub.Core.Streams;
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
        private IEnumerable<VideoStream> videoStreams;
        private IEnumerable<AudioStream> audioStreams;
        private IEnumerable<SubtitleStream> subtitleStreams;
        private IEnumerable<MediaStream> unknownStreams;

        [JsonIgnore] public int ID { get => id; set => SetProperty(ref id, value); }
        protected string Hash { get => hash; set => SetProperty(ref hash, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public DateTime Date { get => date; set => SetProperty(ref date, value); }
        public TimeSpan Duration { get => duration; set => SetProperty(ref duration, value); }
        public string PreviewImagePath { get => previewImagePath; set => SetProperty(ref previewImagePath, value); }
        public string FilePath { get => filePath; set => SetProperty(ref filePath, value); }

        public FormatStream FormatStream { get; set; }
        public IEnumerable<VideoStream> VideoStreams { get => videoStreams; set => SetProperty(ref videoStreams, value); }
        public IEnumerable<AudioStream> AudioStreams { get => audioStreams; set => SetProperty(ref audioStreams, value); }
        public IEnumerable<SubtitleStream> SubtitleStreams { get => subtitleStreams; set => SetProperty(ref subtitleStreams, value); }
        public IEnumerable<MediaStream> UnknownStreams { get => unknownStreams; set => SetProperty(ref unknownStreams, value); }
        [JsonIgnore] public VideoStream? DefaultVideoStream => VideoStreams.FirstOrDefault(s => s.IsDefault) ?? VideoStreams.FirstOrDefault();
        [JsonIgnore] public AudioStream? DefaultAudioStream => AudioStreams.FirstOrDefault(s => s.IsDefault) ?? AudioStreams.FirstOrDefault();


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
            FormatStream = new FormatStream(new Dictionary<string, string>());
            VideoStreams = [];
            AudioStreams = [];
            SubtitleStreams = [];
            UnknownStreams = [];
        }
        public Video(string file) : this()
        {
            filePath = Path.GetFullPath(file);
            hash = GenerateHash();
        }
        public Video(Uri file) : this()
        {
            filePath = file.AbsolutePath;
            hash = GenerateHash();
        }
        public Video(StorageFile file) : this()
        {
            filePath = file.Path;
            hash = GenerateHash();
        }


        public void Load(bool cacheLoad, TimeSpan frame, bool extractEmbeddedImage)
        {
            if (cacheLoad && LoadCache())
            {
                return;
            }

            foreach (Action action in LoadActions(frame, extractEmbeddedImage))
            {
                try { action(); }
                catch { }
            }

            SaveCache();
        }
        public void Load(bool cacheLoad, double percentage, bool extractEmbeddedImage)
        {
            if (cacheLoad && LoadCache())
            {
                return;
            }

            foreach (Action action in LoadActions(percentage, extractEmbeddedImage))
            {
                try { action(); }
                catch { }
            }

            SaveCache();
        }

        public void ExtractPreviewImage(TimeSpan frame, bool extractEmbeddedImage)
        {
            try { PreviewImagePath = new MetadataProcessor(FilePath).ExtractPreviewImage(Hash, frame > Duration ? Duration : frame, extractEmbeddedImage); }
            catch { }
        }


        private List<Action> LoadActions(TimeSpan frame, bool extractEmbeddedImage)
        {
            MetadataProcessor metadataProcessor = new(FilePath);

            return [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => Date = File.GetLastWriteTime(FilePath),
                () => Date = metadataProcessor.ExtractDate(),
                () => Duration = metadataProcessor.ExtractDuration(),
                () => ExtractPreviewImage(frame, extractEmbeddedImage),
                () => FormatStream = metadataProcessor.GetFormatStream(),
                () => VideoStreams = metadataProcessor.GetVideoStreams(),
                () => AudioStreams = metadataProcessor.GetAudioStreams(),
                () => SubtitleStreams = metadataProcessor.GetSubtitleStreams(),
            ];
        }

        private List<Action> LoadActions(double percentage, bool extractEmbeddedImage)
        {
            MetadataProcessor metadataProcessor = new(FilePath);

            return [
                () => Title = Path.GetFileNameWithoutExtension(FilePath),
                () => Date = File.GetLastWriteTime(FilePath),
                () => Date = metadataProcessor.ExtractDate(),
                () => Duration = metadataProcessor.ExtractDuration(),
                () => ExtractPreviewImage(Duration * percentage, extractEmbeddedImage),
                () => FormatStream = metadataProcessor.GetFormatStream(),
                () => VideoStreams = metadataProcessor.GetVideoStreams(),
                () => AudioStreams = metadataProcessor.GetAudioStreams(),
                () => SubtitleStreams = metadataProcessor.GetSubtitleStreams(),
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

        private string GenerateHash()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string baseHash = GenerateHash(FilePath);
            //string baseHash = GenerateHash(File.OpenRead(FilePath));
            stopwatch.Stop();
            Debug.WriteLine($"Initial hashing completed in {stopwatch.ElapsedMilliseconds} ms for file '{FilePath}'.");
            string currentHash = baseHash;
            int salt = 0;

            while (true)
            {
                string cacheFilePath = Path.Combine(Path.GetTempPath(), "VidHub", "Cache", $"{currentHash}.json");

                if (!File.Exists(cacheFilePath))
                {
                    return currentHash;
                }

                if (SameContent(cacheFilePath))
                {
                    return currentHash;
                }

                salt++;
                currentHash = GenerateHash($"{baseHash}:{salt}");
            }
        }
        private string GenerateHash(Stream stream)
        {
            Hasher hasher = Hasher.New();
            byte[] buffer = new byte[1024 * 1024 * 8];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hasher.Update(buffer.AsSpan(0, bytesRead));
            }
            Hash generatedHash = hasher.Finalize();

            return generatedHash.ToString().Replace("-", "").ToLowerInvariant();
        }
        private string GenerateHash(string data)
        {
            return Hasher.Hash(Encoding.UTF8.GetBytes(data)).ToString().Replace("-", "").ToLowerInvariant();
        }

        private bool SameContent(string cacheFilePath)
        {
            try
            {
                Video cache = JsonSerializer.Deserialize<Video>(File.ReadAllText(cacheFilePath)) ?? new Video();

                return File.Exists(cache.FilePath) && new FileInfo(cache.FilePath).Length == new FileInfo(FilePath).Length;
            }
            catch
            {
                return false;
            }
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
